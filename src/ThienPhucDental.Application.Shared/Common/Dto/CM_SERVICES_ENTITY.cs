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
        public decimal? SRV_TAX_RATE { get; set; }
        public bool? SRV_PRICE_INCLUDES_VAT { get; set; }
        public string SRV_VAT_NAME { get; set; }
        public string SRV_VAT_UNIT { get; set; }
        public bool? SRV_HAS_WARRANTY { get; set; }
        public string SRV_WARRANTY_PERIOD { get; set; }
        public string SRV_NOTE { get; set; }
        public bool? SRV_ISACTIVE { get; set; }
    }
}
