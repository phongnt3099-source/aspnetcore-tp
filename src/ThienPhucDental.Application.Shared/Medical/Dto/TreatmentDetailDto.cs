using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Medical.Dto
{
    public class TreatmentDetailDto
    {
        public string TD_ID { get; set; }
        public string TD_EXM_ID { get; set; }
        public string TD_SRV_ID { get; set; }
        public string TD_TOOTH_NUMBER { get; set; }
        public int? TD_QUANTITY { get; set; }
        public decimal? TD_UNIT_PRICE { get; set; }
        public decimal? TD_DISCOUNT_AMOUNT { get; set; }
        public decimal? TD_FINAL_PRICE { get; set; }
        public string TD_ASSIGNED_DOCTOR_ID { get; set; }
        public byte? TD_STATUS { get; set; }
    }
}
