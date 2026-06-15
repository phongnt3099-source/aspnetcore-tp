using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Common.Dto
{
    public class CM_SERVICES_ENTITY : PagedAndSortedInputDto
    {
        public Guid SRV_ID { get; set; }
        public Guid SRV_TYPEID { get; set; }
        public string SRV_CODE { get; set; }
        public string SRV_NAME { get; set; }
        public string SRV_UNIT { get; set; }
        public decimal? SRV_PRICE { get; set; }
        public decimal? SRV_PRICE_TO { get; set; }
        public bool? PRICE_INCLUDES_VAT { get; set; }
        public string VAT_NAME { get; set; }
        public int? TAX_RATE { get; set; }
        public decimal? SRV_TAX_RATE { get; set; }
        public bool? SRV_PRICE_INCLUDES_VAT { get; set; }
        public string SRV_VAT_NAME { get; set; }
        public string SRV_VAT_UNIT { get; set; }
        public bool? HAS_WARRANTY { get; set; }
        public int WARRANTY_PERIOD { get; set; }
        public string SERVICE_TYPE_ID { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string SRV_NOTE { get; set; }
        public string MAKER_ID { get; set; }
        public string CREATE_DT { get; set; }
        public string UPDATE_DT { get; set; }
        public string UPDATE_USER { get; set; }
        public bool? ISACTIVE { get; set; }
    }
}
