using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Common.Dto
{
    public class CM_HOLIDAY_ENTITY : PagedAndSortedInputDto
    {

        public int TENANT_ID { get; set; }
        public long? ORGANIZATION_UNIT_ID { get; set; }
        public string HOLIDAY_ID { get; set; }
        public string HOLIDAY_NAME { get; set; }
        public string HOLIDAY_DATE { get; set; }
        public string IS_RECURRING { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string ISACTIVE { get; set; }
        public string MAKER_ID { get; set; }
        public string CREATE_DT { get; set; }
        public string FROM_DATE { get; set; }
        public string TO_DATE { get; set; }
        public float SALARY_MULTIPLIER { get; set; }

        public bool? isEditing { get; set; }
    }
}
