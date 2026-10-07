using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Biometrics
{
    public interface IAntiSpoofingService
    {
        // Nhận mảng byte của ảnh thay vì nhận kiểu Mat
        bool IsRealFace(byte[] imageBytes, out float realScore);
    }
}
