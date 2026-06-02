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
    }
}
