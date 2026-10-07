using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Biometrics.Dto
{
    public class FaceDetectionResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public float[] AlignedFace112Tensor { get; set; } // Dành cho EdgeFace-XS (112x112, ImageNet Norm)
        public byte[] CroppedFace80Bytes { get; set; }    // Dành cho MiniFASNetV2 (80x80, Scale 2.7x)
    }
}
