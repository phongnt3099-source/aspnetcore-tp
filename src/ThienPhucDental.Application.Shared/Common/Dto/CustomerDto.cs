using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Common.Dto
{
    public class CustomerDto
    {
        public string CUS_ID { get; set; }
        public string CUS_CODE { get; set; }
        public string CUS_DOB { get; set; }
        public string CUS_GENDER { get; set; }
        public string CUS_PHONE { get; set; }
        public string CUS_NAME { get; set; }
        public string CUS_PHONE2 { get; set; }
        public string CUS_EMAIL { get; set; }
        public string CUS_ADDRESS { get; set; }
        public string CUS_WARD { get; set; }
        public string CUS_CITY { get; set; }
        public string CUS_MEDICAL_HISTORY { get; set; }
        public string CUS_MEDICAL_HISTORY_NOTES { get; set; }
        public string CUS_JOB { get; set; }
        public string CUS_ETHNICITY { get; set; }
        public string CUS_NATIONALITY { get; set; }
        public string CUS_CCCD { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string UPDATE_ID { get; set; }
        public DateTime? UPDATE_DT { get; set; }
        public string RECORD_STATUS { get; set; }
    }
}
