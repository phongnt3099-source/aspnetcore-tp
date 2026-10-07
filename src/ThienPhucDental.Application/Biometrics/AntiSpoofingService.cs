using Abp.Dependency;
using Castle.Core.Logging;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace ThienPhucDental.Biometrics
{
    public class AntiSpoofingService : IAntiSpoofingService, ISingletonDependency, IDisposable
    {
        private readonly InferenceSession _session;
        private readonly string _inputNodeName;
        public ILogger Logger { get; set; }

        // Ngưỡng phát hiện người thật an toàn cho camera thực tế
        private const float LivenessThreshold = 0.65f;

        public AntiSpoofingService()
        {
            Logger = NullLogger.Instance;
            string modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "AIModels", "minifasnetv2.onnx");

            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException($"[ANTI-SPOOF] Không tìm thấy file mô hình MiniFASNetV2 tại: {modelPath}");
            }

            var sessionOptions = new SessionOptions
            {
                GraphOptimizationLevel = GraphOptimizationLevel.ORT_ENABLE_ALL,
                IntraOpNumThreads = 1
            };

            _session = new InferenceSession(modelPath, sessionOptions);
            _inputNodeName = _session.InputMetadata.Keys.First();

            Logger.Info("[ANTI-SPOOF] Khởi tạo thành công MiniFASNetV2 Session (Singleton).");
        }

        /// <summary>
        /// Nhận trực tiếp ảnh byte[] đã được FaceDetectorService crop 2.7x và resize 80x80.
        /// Giữ nguyên toàn bộ bối cảnh viền ngoài, không crop thêm bất kỳ tỷ lệ nào.
        /// </summary>
        public bool IsRealFace(byte[] croppedFace80Bytes, out float realScore)
        {
            realScore = 0f;

            if (croppedFace80Bytes == null || croppedFace80Bytes.Length == 0)
            {
                Logger.Warn("[ANTI-SPOOF] Dữ liệu ảnh crop rỗng.");
                return false;
            }

            using var mat = Cv2.ImDecode(croppedFace80Bytes, ImreadModes.Color);
            if (mat.Empty())
            {
                Logger.Warn("[ANTI-SPOOF] Không thể giải mã dữ liệu ảnh crop.");
                return false;
            }

            const int targetW = 80;
            const int targetH = 80;

            // Đảm bảo đúng chuẩn 80x80 mà không center-crop
            using var finalMat = (mat.Width == targetW && mat.Height == targetH)
                ? mat.Clone()
                : mat.Resize(new Size(targetW, targetH));

            const int channelSize = targetW * targetH;
            float[] tensorData = new float[3 * channelSize];

            byte[] rawBytes = new byte[channelSize * 3];
            Marshal.Copy(finalMat.Data, rawBytes, 0, rawBytes.Length);

            // Chuẩn hóa MiniFASNetV2: Hệ màu BGR, scale 1.0 / 255.0f (CHW)
            const float inv255 = 1.0f / 255.0f;
            for (int i = 0; i < channelSize; i++)
            {
                int srcIdx = i * 3;
                tensorData[i] = rawBytes[srcIdx] * inv255;                  // Channel B
                tensorData[channelSize + i] = rawBytes[srcIdx + 1] * inv255;  // Channel G
                tensorData[2 * channelSize + i] = rawBytes[srcIdx + 2] * inv255; // Channel R
            }

            var dimensions = new[] { 1, 3, targetH, targetW };
            var tensor = new DenseTensor<float>(tensorData, dimensions);

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(_inputNodeName, tensor)
            };

            using var results = _session.Run(inputs);
            float[] rawLogits = results.First().AsEnumerable<float>().ToArray();
            float[] probabilities = Softmax(rawLogits);

            Logger.Warn($"[ANTI-SPOOF DEBUG] Raw Logits: [{string.Join(", ", rawLogits.Select(x => x.ToString("F4")))}]");
            Logger.Warn($"[ANTI-SPOOF DEBUG] Probabilities: [{string.Join(", ", probabilities.Select(x => x.ToString("P2")))}]");

            // Xác định điểm số người thật dựa trên cấu trúc checkpoint thực tế:
            if (probabilities.Length == 3)
            {
                // Index 0: Fake Screen
                // Index 1: Fake Paper
                // Index 2: Real Face
                realScore = probabilities[2];
            }
            else if (probabilities.Length == 2)
            {
                realScore = probabilities[1];
            }
            else
            {
                realScore = probabilities[0];
            }

            Logger.Warn($"[ANTI-SPOOF] Điểm xác thực khuôn mặt thật: {realScore:P2} (Ngưỡng yêu cầu: {LivenessThreshold:P2})");

            return realScore >= LivenessThreshold;
        }

        private float[] Softmax(float[] logits)
        {
            float max = logits.Max();
            float sum = 0f;
            float[] exp = new float[logits.Length];

            for (int i = 0; i < logits.Length; i++)
            {
                exp[i] = (float)Math.Exp(logits[i] - max);
                sum += exp[i];
            }

            for (int i = 0; i < logits.Length; i++)
            {
                exp[i] /= sum;
            }

            return exp;
        }

        public void Dispose()
        {
            _session?.Dispose();
        }
    }
}