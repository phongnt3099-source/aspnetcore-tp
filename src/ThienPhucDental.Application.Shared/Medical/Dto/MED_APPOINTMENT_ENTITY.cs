using System;
using System.Collections.Generic;
using System.Text;
using ThienPhucDental.Common.Dto;

namespace ThienPhucDental.Medical.Dto
{
    public class MED_APPOINTMENT_ENTITY: PagedAndSortedInputDto
    {
        public string APP_ID { get; set; }

        public string APP_DATE { get; set; }

        public int RANGE_TIME { get; set; }
        public string HOUR { get; set; }
        public string MINUTE { get; set; }
        public string START_TIME { get; set; }
        public string SLOT_NAME { get; set; }

        public string APP_CUST_ID { get; set; }

        public string APP_DOC_ID { get; set; }

        public string APP_ASSISTANT_ID_1 { get; set; }

        public string APP_ASSISTANT_ID_2 { get; set; }

        public string APP_STATUS { get; set; }

        public string APP_CONTENT { get; set; }

        public string RECORD_STATUS { get; set; }

        public string MAKER_ID { get; set; }

        public string CREATE_DT { get; set; }
    }
}
