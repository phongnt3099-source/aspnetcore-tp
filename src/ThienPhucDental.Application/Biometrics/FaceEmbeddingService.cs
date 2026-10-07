using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;

namespace ThienPhucDental.Biometrics
{
    public class FaceEmbeddingService : IFaceEmbeddingService, IDisposable, ISingletonDependency
    {
        private readonly InferenceSession _session;
        private readonly string _inputNodeName;

        public FaceEmbeddingService()
        {
            var modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "AIModels", "edgeface_xs.onnx");

            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException($"Không tìm thấy mô hình AI tại: {modelPath}");
            }

            int cpuCores = Math.Max(1, Environment.ProcessorCount);

            var sessionOptions = new SessionOptions
            {
                // Bật tối đa mức độ tối ưu hóa đồ thị (hợp nhất node tử, khử toán tử dư thừa)
                GraphOptimizationLevel = GraphOptimizationLevel.ORT_ENABLE_ALL,

                // Chỉ điều phối số luồng tính toán ma trận nội bộ (Intra-Op)
                IntraOpNumThreads = cpuCores switch
                {
                    <= 2 => 1,
                    <= 8 => 2,
                    _ => 4
                }
            };

            // Khởi tạo duy nhất 1 lần trong suốt vòng đời ứng dụng
            _session = new InferenceSession(modelPath, sessionOptions);
            _inputNodeName = _session.InputMetadata.Keys.First();
        }

        public float[] ExtractEmbedding(float[] inputTensorValues)
        {
            var dimensions = new[] { 1, 3, 112, 112 };
            var tensor = new DenseTensor<float>(inputTensorValues, dimensions);

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(_inputNodeName, tensor)
            };

            using var results = _session.Run(inputs);
            float[] rawEmbedding = results.First().AsEnumerable<float>().ToArray();

            // Thực hiện L2-normalize vector 512 chiều
            return NormalizeL2(rawEmbedding);
        }

        private float[] NormalizeL2(float[] vector)
        {
            double sumSq = 0.0;
            for (int i = 0; i < vector.Length; i++)
            {
                sumSq += vector[i] * vector[i];
            }

            float norm = (float)Math.Sqrt(sumSq);
            if (norm > 1e-6f)
            {
                float invNorm = 1.0f / norm;
                for (int i = 0; i < vector.Length; i++)
                {
                    vector[i] *= invNorm;
                }
            }

            return vector;
        }

        public void Dispose()
        {
            _session?.Dispose();
        }
    }
}
