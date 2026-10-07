using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Attendance.Requests.Dto
{
    public class RequestDto
    {
        public long REQUEST_ID { get; set; }
        public string EMP_ID { get; set; }
        public string EMP_NAME { get; set; }
        public string EMP_PHONE { get; set; }

        public string REQUEST_TYPE { get; set; }

        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        public TimeSpan? FROM_TIME { get; set; }
        public TimeSpan? TO_TIME { get; set; }

        public DateTime? FIX_DATE { get; set; }
        public TimeSpan? FIX_CHECK_IN { get; set; }
        public TimeSpan? FIX_CHECK_OUT { get; set; }

        public string REASON { get; set; }
        public string ATTACHMENT_URL { get; set; }

        public string APPROVER_ID { get; set; }
        public string APPROVER_NAME { get; set; }

        public string STATUS { get; set; }
        public DateTime? APPROVED_DT { get; set; }
        public string REJECT_REASON { get; set; }

        public DateTime CREATED_DT { get; set; }
        public int? HOURS_PENDING { get; set; }
        public int? TOTAL_DAYS { get; set; }
    }
}
