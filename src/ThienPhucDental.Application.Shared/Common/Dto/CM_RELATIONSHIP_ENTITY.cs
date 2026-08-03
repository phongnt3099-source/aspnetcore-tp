using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Common.Dto
{
    public class CM_RELATIONSHIP_ENTITY
    {
        public string REL_ID { get; set; }
        public string CUS_ID { get; set; }
        public string CUS_CODE { get; set; }
        public string RELATED_CUS_ID { get; set; }
        public string FAMILY_ID { get; set; }
        public string ACTION { get; set; }
        public string CUS_INITIALS { get; set; }  
        public string REL_TYPE { get; set; }
        public string NOTES { get; set; }
        
        public string MAKER_ID { get; set; }
        public string CREATE_DT { get; set; }

        public string AGE { get; set; }

        public string UPDATE_ID { get; set; }
        public string UPDATE_DT { get; set; }

        public string CUS_NAME_TEMP { get; set; }
        public string isFamilyMember { get; set; }
        public string relTypeOriginal { get; set; }

        public string selectedRelType { get; set; }
        public string isDropdownOpen { get; set; }

    }
}
