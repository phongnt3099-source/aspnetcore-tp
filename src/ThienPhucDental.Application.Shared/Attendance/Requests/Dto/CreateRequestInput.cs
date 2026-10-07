using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ThienPhucDental.Attendance.Requests.Dto
{
    public class CreateRequestInput
    {
        [Required]
        [StringLength(20)]
        public string EMP_ID { get; set; }

        [Required]
        [StringLength(30)]
        public string REQUEST_TYPE { get; set; }
        // LEAVE_ANNUAL | LEAVE_SICK | LEAVE_UNPAID
        // BUSINESS_TRIP | ATTENDANCE_FIX | LATE_EXPLAIN

        [Required]
        [StringLength(20)]
        public string APPROVER_ID { get; set; }

        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        public TimeSpan? FROM_TIME { get; set; }
        public TimeSpan? TO_TIME { get; set; }

        public DateTime? FIX_DATE { get; set; }
        public TimeSpan? FIX_CHECK_IN { get; set; }
        public TimeSpan? FIX_CHECK_OUT { get; set; }

        [StringLength(500)]
        public string REASON { get; set; }

        [StringLength(500)]
        public string ATTACHMENT_URL { get; set; }
    }
}
