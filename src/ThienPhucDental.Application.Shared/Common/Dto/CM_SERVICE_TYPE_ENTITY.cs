using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Common.Dto
{
    public class CM_SERVICE_TYPE_ENTITY : PagedAndSortedInputDto
    {
        public Guid ST_ID { get; set; }
        public string ST_NAME { get; set; }
        public string ST_DESCRIPTION { get; set; }
        public int? TOTAL_PROCEDURES { get; set; }
        public bool? ISACTIVE { get; set; }
        public string STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public string CREATE_DT { get; set; }
        public string UPDATE_DT { get; set; }
        public string UPDATE_USER { get; set; }
    }
}
