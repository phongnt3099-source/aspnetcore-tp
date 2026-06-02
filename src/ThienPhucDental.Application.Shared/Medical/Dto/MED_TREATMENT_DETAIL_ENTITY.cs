using System;
using System.Collections.Generic;
using System.Text;
using ThienPhucDental.Common.Dto;

namespace ThienPhucDental.Medical.Dto
{
    public class MED_TREATMENT_DETAIL_ENTITY : PagedAndSortedInputDto
    {
        public Guid? TD_ID { get; set; }
        public string TD_EXM_ID { get; set; }
        public Guid? TD_SRV_ID { get; set; }
        public string SRV_NAME { get; set; }
        public string TD_TOOTH_NUMBER { get; set; }
        public int? TD_QUANTITY { get; set; }
        public decimal? TD_UNIT_PRICE { get; set; }
        public decimal? TD_DISCOUNT_AMOUNT { get; set; }
        public decimal? TD_FINAL_PRICE { get; set; }
        public string TD_ASSIGNED_DOCTOR_ID { get; set; }
        public byte? TD_STATUS { get; set; }
        public decimal? TD_FINAL_PRICE_PER_UNIT { get; set; }
        public string discountType { get; set; }
        public decimal? discountValue { get; set; }
        public string TD_WARRANTY_EXPIRED_DATE { get; set; }
        public bool? TD_HAS_WARRANTY { get; set; }
        public string TD_WARRANTY_START_DATE { get; set; }
        
    }
}
