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
        public bool? ST_ISACTIVE { get; set; }
    }
}
