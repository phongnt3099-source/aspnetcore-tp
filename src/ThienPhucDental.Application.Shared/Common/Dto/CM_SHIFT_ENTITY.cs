using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Common.Dto
{
    public class CM_SHIFT_ENTITY : PagedAndSortedInputDto
    {
        public string SHIFT_ID { get; set; }
        public string SHIFT_NAME { get; set; }
        public int TENANT_ID { get; set; }
        public long? ORGANIZATION_UNIT_ID { get; set; }
        public string CHECK_IN_TIME { get; set; }
        public string CHECK_OUT_TIME { get; set; }
        public int? LATE_TOLERANCE { get; set; }
        public string BREAK_START_TIME { get; set; }
        public string BREAK_END_TIME { get; set; }
        public string ISACTIVE { get; set; }
        public bool? ALLOW_BREAK { get; set; }
        public string DAYS_OF_WEEK { get; set; }


        public bool? isEditing { get; set; }
    }
}
