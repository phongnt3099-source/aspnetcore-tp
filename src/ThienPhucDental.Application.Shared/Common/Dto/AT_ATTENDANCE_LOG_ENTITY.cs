using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ThienPhucDental.Authorization.Users.Dto;

namespace ThienPhucDental.Common.Dto
{
    public class AT_ATTENDANCE_LOG_ENTITY : PagedAndSortedInputDto
    {
        public long LOG_ID { get; set; }
        public string USER_NAME { get; set; }
        public string CHECK_TIME { get; set; }
        public string CHECK_TYPE { get; set; }
        public string STATUS { get; set; }
        public double? CONFIDENCE_SCORE { get; set; }
        public string DEVICE_IP { get; set; }
        public string NOTES { get; set; }
        public string CREATE_DT { get; set; }
    }

    public class AttendanceInputDto
    {
        public List<float> EmbeddingVector { get; set; }
        public string Base64Image { get; set; } // Ảnh chụp từ Kiosk gửi lên
        public string DeviceIp { get; set; }     // IP của thiết bị Kiosk
        public string CheckType { get; set; }    // 'IN' hoặc 'OUT'
    }

    public class AttendanceResultDto
    {
        public string Result { get; set; }       // '1': Thành công, '-1': Lỗi
        public string USER_NAME { get; set; }
        public string Message { get; set; }
        public double ConfidenceScore { get; set; }
    }
    public class RegisterFaceInputDto
    {
        // Nhận chuỗi token mã hóa gộp nếu dùng link ?c=...
        public string C { get; set; }

        public long? UserId { get; set; }

        public string Token { get; set; }

        public string ImageBase64 { get; set; }

        public List<float> EmbeddingVector { get; set; }

        public float LivenessScore { get; set; }
    }

    public class ValidateFaceLinkOutputDto
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }

    public class AttendanceCheckDto
    {
        /// <summary>
        /// Nhân viên này có thuộc diện bắt buộc chấm công hay không (lấy từ CM_EMPLOYEE.IS_ATTENDANCE_REQUIRED)
        /// </summary>
        public bool IsAttendanceRequired { get; set; }

        /// <summary>
        /// Hôm nay đã thực hiện Check-in (vào ca) thành công chưa
        /// </summary>
        public bool HasCheckedInToday { get; set; }

        /// <summary>
        /// Họ và tên nhân viên (để hiển thị lời chào trên UI popup/kiosk)
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Thời gian check-in đầu ngày (nếu đã check-in)
        /// </summary>
        public DateTime? CheckInTime { get; set; }

        /// <summary>
        /// Hôm nay đã thực hiện Check-out (ra về/hết ca) chưa (phục vụ cảnh báo lúc đăng xuất)
        /// </summary>
        public bool HasCheckedOutToday { get; set; }

        /// <summary>
        /// Thời gian check-out gần nhất (nếu có)
        /// </summary>
        public DateTime? CheckOutTime { get; set; }
    }

    public class AttendanceRawStatusDto
    {
        public string EMP_ID { get; set; }
        public string EMP_NAME { get; set; }
        public string USER_NAME { get; set; }
        public bool IS_ATTENDANCE_REQUIRED { get; set; }

        public long? LOG_ID { get; set; }
        public DateTime? CHECK_TIME { get; set; }
        public string CHECK_TYPE { get; set; } // 'IN' hoặc 'OUT'
        public string STATUS { get; set; }
        public double? CONFIDENCE_SCORE { get; set; }
    }
    public class AttendanceStatusCacheItem
    {
        public bool IsAttendanceRequired { get; set; }
        public bool HasCheckedInToday { get; set; }
        public DateTime? CheckInTime { get; set; }
        public bool HasCheckedOutToday { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string FullName { get; set; }
    }
}
