using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Medical.Dto
{
    public class AppointmentDto
    {
        public int APP_ID { get; set; }

        public DateTime APP_DATE { get; set; }

        public string APP_TIME { get; set; }

        public int RANGE_TIME { get; set; }
        public string HOUR { get; set; }
        public string MINUTE { get; set; }
        public string SLOT_NAME { get; set; }

        public string APP_CUST_ID { get; set; }

        public string APP_DOC_ID { get; set; }

        public string APP_ASSISTANT_ID_1 { get; set; }

        public string APP_ASSISTANT_ID_2 { get; set; }

        public string APP_STATUS { get; set; }

        public string APP_CONTENT { get; set; }

        public string APP_RECORD_STATUS { get; set; }

        public string APP_MAKER_ID { get; set; }

        public DateTime? APP_CREATE_DT { get; set; }

        public string CUST_NAME { get; set; }
        public string DOC_NAME { get; set; }
    }
}
