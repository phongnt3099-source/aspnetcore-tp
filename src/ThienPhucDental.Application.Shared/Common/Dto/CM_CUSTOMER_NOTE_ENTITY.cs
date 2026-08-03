using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Common.Dto
{
    public class CM_CUSTOMER_NOTE_ENTITY
    {
        public long NOTE_ID { get; set; }
        public string CUS_ID { get; set; }
        public string NOTE_CONTENT { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string MAKER_ID { get; set; }
        public string UPDATE_ID { get; set; }
        public DateTime? UPDATE_DT { get; set; }
        public bool? RECORD_STATUS { get; set; }

        
        public string MAKER_NAME { get; set; }
    }
}
