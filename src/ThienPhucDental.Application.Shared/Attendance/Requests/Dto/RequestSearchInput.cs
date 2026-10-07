using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Attendance.Requests.Dto
{
    public class RequestSearchInput
    {
        public string KEYWORD { get; set; }
        public string STATUS { get; set; }
        public string REQUEST_TYPE { get; set; }
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        public int SKIP_COUNT { get; set; } = 0;
        public int MAX_RESULT_COUNT { get; set; } = 20;
    }
}
