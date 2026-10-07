using Abp.Dependency;
using Castle.Core.Logging;
using OpenCvSharp;
using System;
using System.IO;
using System.Runtime.InteropServices;
using ThienPhucDental.Biometrics.Dto;

namespace ThienPhucDental.Biometrics
{
    public class FaceDetectorService : IFaceDetectorService, ISingletonDependency, IDisposable
    {
        private const int DETECT_SIZE = 320;
        private readonly FaceDetectorYN _detector;
        private readonly object _detectorLock = new object();
        public ILogger Logger { get; set; }

        // 5 điểm mốc chuẩn ArcFace (112x112) theo hệ quy chiếu người quan sát nhìn vào ảnh (Left-to-Right)
        // [0] Mắt bên trái ảnh (X ~ 38.3)
        // [1] Mắt bên phải ảnh (X ~ 73.5)
        // [2] Đỉnh mũi (X ~ 56.0)
        // [3] Khóe miệng bên trái ảnh (X ~ 41.5)
        // [4] Khóe miệng bên phải ảnh (X ~ 70.7)
        private static readonly Point2f[] StandardLandmarks = new Point2f[]
        {
            new Point2f(38.2946f, 51.6963f),
            new Point2f(73.5318f, 51.5014f),
            new Point2f(56.0252f, 71.7366f),
            new Point2f(41.5493f, 92.3655f),
            new Point2f(70.7299f, 92.2041f)
        };

        public FaceDetectorService()
        {
            Logger = NullLogger.Instance;
            string modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "AIModels", "face_detection_yunet_2023mar.onnx");

            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException($"[FACE-DETECTOR] Không tìm thấy tệp mô hình YuNet tại: {modelPath}");
            }

            // Khởi tạo detector duy nhất 1 lần với kích thước cố định 320x320
            _detector = FaceDetectorYN.Create(
                modelPath,
                "",
                new Size(DETECT_SIZE, DETECT_SIZE),
                scoreThreshold: 0.9f,
                nmsThreshold: 0.3f
            );

            Logger.Info("[FACE-DETECTOR] Khởi tạo thành công YuNet FaceDetectorYN Singleton (320x320).");
        }

        public FaceDetectionResult DetectAndAlign(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return new FaceDetectionResult { IsValid = false, ErrorMessage = "Dữ liệu hình ảnh rỗng." };
            }

            using var mat = Cv2.ImDecode(imageBytes, ImreadModes.Color);
            if (mat.Empty())
            {
                return new FaceDetectionResult { IsValid = false, ErrorMessage = "Không thể giải mã dữ liệu hình ảnh." };
            }

            // 1. Chuẩn bị ảnh tạm 320x320 để dò mặt (tăng tốc độ và tương thích hoàn toàn OpenCvSharp)
            using var resizedForDetect = new Mat();
            Cv2.Resize(mat, resizedForDetect, new Size(DETECT_SIZE, DETECT_SIZE));

            float scaleX = (float)mat.Width / DETECT_SIZE;
            float scaleY = (float)mat.Height / DETECT_SIZE;

            using var faces = new Mat();

            // Khóa đồng bộ đa luồng nhẹ khi chạy suy luận Detect
            lock (_detectorLock)
            {
                _detector.Detect(resizedForDetect, faces);
            }

            // 2. Kiểm tra số lượng khuôn mặt
            if (faces.Rows == 0)
            {
                return new FaceDetectionResult { IsValid = false, ErrorMessage = "Không tìm thấy khuôn mặt rõ nét. Vui lòng căn chỉnh lại góc nhìn." };
            }

            if (faces.Rows > 1)
            {
                return new FaceDetectionResult { IsValid = false, ErrorMessage = "Phát hiện nhiều hơn một khuôn mặt. Vui lòng chỉ đứng một mình trước máy ảnh." };
            }

            // 3. Trích xuất và scale ngược tọa độ về kích thước ảnh gốc
            float x = faces.At<float>(0, 0) * scaleX;
            float y = faces.At<float>(0, 1) * scaleY;
            float w = faces.At<float>(0, 2) * scaleX;
            float h = faces.At<float>(0, 3) * scaleY;
            float score = faces.At<float>(0, 14);

            // 3.1. Validate kích thước pixel tối thiểu
            if (w < 80 || h < 80)
            {
                return new FaceDetectionResult { IsValid = false, ErrorMessage = "Khuôn mặt quá nhỏ hoặc ở quá xa camera. Vui lòng lại gần hơn." };
            }

            // 3.2. Validate tỷ lệ khuôn mặt so với khung hình (faceRatio)
            float faceHeightRatio = h / (float)mat.Height;
            float faceWidthRatio = w / (float)mat.Width;

            if (faceHeightRatio < 0.20f)
            {
                return new FaceDetectionResult { IsValid = false, ErrorMessage = "Khuôn mặt ở quá xa khung hình. Vui lòng di chuyển lại gần camera hơn." };
            }

            if (faceHeightRatio > 0.85f || faceWidthRatio > 0.85f)
            {
                return new FaceDetectionResult { IsValid = false, ErrorMessage = "Khuôn mặt ở quá gần camera. Vui lòng lùi ra xa một chút." };
            }

            // 3.3. Validate độ tin cậy
            if (score < 0.90f)
            {
                return new FaceDetectionResult { IsValid = false, ErrorMessage = "Góc mặt không đủ rõ nét hoặc bị che khuất." };
            }

            // 4. Ánh xạ chính xác thứ tự 5 Landmarks của YuNet sang ArcFace và scale về ảnh gốc:
            // YuNet [4,5]: Mắt phải người -> Nằm bên TRÁI ảnh
            // YuNet [6,7]: Mắt trái người  -> Nằm bên PHẢI ảnh
            // YuNet [8,9]: Đỉnh mũi
            // YuNet [10,11]: Khóe miệng phải -> Nằm bên TRÁI ảnh
            // YuNet [12,13]: Khóe miệng trái -> Nằm bên PHẢI ảnh
            var detectedLandmarks = new Point2f[]
            {
                new Point2f(faces.At<float>(0, 4) * scaleX, faces.At<float>(0, 5) * scaleY),
                new Point2f(faces.At<float>(0, 6) * scaleX, faces.At<float>(0, 7) * scaleY),
                new Point2f(faces.At<float>(0, 8) * scaleX, faces.At<float>(0, 9) * scaleY),
                new Point2f(faces.At<float>(0, 10) * scaleX, faces.At<float>(0, 11) * scaleY),
                new Point2f(faces.At<float>(0, 12) * scaleX, faces.At<float>(0, 13) * scaleY)
            };

            // Dùng Mat.FromArray để tương thích hoàn toàn InputArray
            using var srcMat = Mat.FromArray(detectedLandmarks);
            using var dstMat = Mat.FromArray(StandardLandmarks);

            using var affineMat = Cv2.EstimateAffinePartial2D(srcMat, dstMat);

            // Kiểm tra an toàn trước khi gọi WarpAffine
            if (affineMat == null || affineMat.Empty() || affineMat.Rows != 2 || affineMat.Cols != 3)
            {
                Logger.Warn("[FACE-DETECTOR] Không thể ước lượng ma trận Affine hợp lệ từ 5 điểm landmarks.");
                return new FaceDetectionResult
                {
                    IsValid = false,
                    ErrorMessage = "Không thể căn chỉnh góc mặt. Vui lòng giữ thẳng đầu và nhìn trực diện vào camera."
                };
            }

            // 5. Căn chỉnh Affine Transform trực tiếp từ ảnh gốc sang 112x112 cho EdgeFace-XS
            using var aligned112 = new Mat();
            Cv2.WarpAffine(
                mat,
                aligned112,
                affineMat,
                new Size(112, 112),
                InterpolationFlags.Linear,
                BorderTypes.Constant,
                Scalar.All(0)
            );

            float[] tensorValues = CreateImageNetTensor(aligned112);

            // 6. Crop mở rộng 2.7x quanh khuôn mặt trên ảnh gốc cho MiniFASNetV2
            byte[] crop80Bytes = CropExpandedFace(mat, x, y, w, h, 2.7f);

            return new FaceDetectionResult
            {
                IsValid = true,
                AlignedFace112Tensor = tensorValues,
                CroppedFace80Bytes = crop80Bytes
            };
        }

        private float[] CreateImageNetTensor(Mat resized112)
        {
            const int width = 112;
            const int height = 112;
            const int channelSize = width * height;
            float[] tensor = new float[3 * channelSize];

            byte[] rawBytes = new byte[channelSize * 3];
            Marshal.Copy(resized112.Data, rawBytes, 0, rawBytes.Length);

            const float inv255 = 1.0f / 255.0f;
            const float meanR = 0.485f, meanG = 0.456f, meanB = 0.406f;
            const float invStdR = 1.0f / 0.229f, invStdG = 1.0f / 0.224f, invStdB = 1.0f / 0.225f;

            // Chuyển BGR sang RGB và chuẩn hóa theo chuẩn ImageNet (CHW)
            for (int i = 0; i < channelSize; i++)
            {
                int srcIdx = i * 3;
                float b = rawBytes[srcIdx] * inv255;
                float g = rawBytes[srcIdx + 1] * inv255;
                float r = rawBytes[srcIdx + 2] * inv255;

                tensor[i] = (r - meanR) * invStdR;                   // Kênh R
                tensor[channelSize + i] = (g - meanG) * invStdG;     // Kênh G
                tensor[2 * channelSize + i] = (b - meanB) * invStdB; // Kênh B
            }

            return tensor;
        }

        private byte[] CropExpandedFace(Mat src, float x, float y, float w, float h, float scale)
        {
            float cx = x + w / 2f;
            float cy = y + h / 2f;
            float newW = w * scale;
            float newH = h * scale;

            int x1 = (int)Math.Max(0, cx - newW / 2f);
            int y1 = (int)Math.Max(0, cy - newH / 2f);
            int x2 = (int)Math.Min(src.Width, cx + newW / 2f);
            int y2 = (int)Math.Min(src.Height, cy + newH / 2f);

            var cropRect = new Rect(x1, y1, x2 - x1, y2 - y1);
            using var cropped = new Mat(src, cropRect);
            using var resized80 = new Mat();
            Cv2.Resize(cropped, resized80, new Size(80, 80));

            return resized80.ToBytes(".jpg");
        }

        public void Dispose()
        {
            _detector?.Dispose();
        }
    }
}