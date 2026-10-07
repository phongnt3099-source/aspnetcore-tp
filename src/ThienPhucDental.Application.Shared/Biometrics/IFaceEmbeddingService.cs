using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Biometrics
{
    public interface IFaceEmbeddingService
    {
        float[] ExtractEmbedding(float[] inputTensorValues);
    }
}
