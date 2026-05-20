using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Common.Dto
{
    public class ServiceDto
    {
        public string SRV_ID { get; set; }
        public string SRV_TYPEID { get; set; }
        public string SRV_CODE { get; set; }
        public string SRV_NAME { get; set; }
        public string SRV_UNIT { get; set; }
        public decimal? SRV_PRICE { get; set; }
        public decimal? SRV_PRICE_TO { get; set; }
        public decimal? TAX_RATE { get; set; }
        public bool? PRICE_INCLUDES_VAT { get; set; }
        public string VAT_NAME { get; set; }
        public string VAT_UNIT { get; set; }
        public bool? HAS_WARRANTY { get; set; }
        public string WARRANTY_PERIOD { get; set; }
        public string SRV_NOTE { get; set; }
        public bool? ISACTIVE { get; set; }
    }
}
