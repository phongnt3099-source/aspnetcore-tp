using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Common.Dto
{
    public class CM_CLINIC_NETWORK_ENTITY : PagedAndSortedInputDto
    {
        public long ID { get; set; }
        public int TENANT_ID { get; set; }
        public string ORGANIZATION_UNIT_NAME { get; set; }
        public string ALLOWED_IP { get; set; }
        public string DESCRIPTION { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public string CREATION_TIME { get; set; }
        public bool? isEditing { get; set; }
        public string USER_NAME { get; set; }
    }
}
