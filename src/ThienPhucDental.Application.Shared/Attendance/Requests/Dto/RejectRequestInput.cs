using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ThienPhucDental.Attendance.Requests.Dto
{
    public class RejectRequestInput
    {
        [Required]
        public long REQUEST_ID { get; set; }

        [Required]
        [StringLength(20)]
        public string APPROVER_ID { get; set; }

        [Required]
        [StringLength(500)]
        public string REASON { get; set; }
    }
}
