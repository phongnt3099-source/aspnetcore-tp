using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Common.Dto
{
    public class CM_ATTACH_FILE_ENTITY : PagedAndSortedInputDto
    {
        public string ATT_ID { get; set; }
        public string REF_ID { get; set; }
        public string REF_TYPE { get; set; }
        public string FILE_PATH { get; set; }
        public string FILE_NAME { get; set; }
        public string NOTES { get; set; }
    }
}
