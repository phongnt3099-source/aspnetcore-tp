using System;
using System.Collections.Generic;
using System.Text;
using ThienPhucDental.Biometrics.Dto;

namespace ThienPhucDental.Biometrics
{
    public interface IFaceDetectorService
    {
        FaceDetectionResult DetectAndAlign(byte[] imageBytes);
    }
}
