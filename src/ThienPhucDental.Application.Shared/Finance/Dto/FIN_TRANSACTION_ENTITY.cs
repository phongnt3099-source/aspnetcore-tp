using System;
using System.Collections.Generic;
using System.Text;
using ThienPhucDental.Common.Dto;

namespace ThienPhucDental.Finance.Dto
{
    public class FIN_TRANSACTION_ENTITY : PagedAndSortedInputDto
    {
        public string FT_ID { get; set; }

        public int FT_TYPE { get; set; }

        public int METHOD_ID { get; set; } 

        public string PATIENT_ID { get; set; }

        public string DOCUMENT_DATE { get; set; }

        public decimal TOTAL_AMOUNT { get; set; }

        public decimal? TOTAL_PAID { get; set; }

        public string NOTES { get; set; }

        public string CATEGORY_ID { get; set; }

        public string MAKER_ID { get; set; }

        public string CREATE_DT { get; set; }

        public string FT_EXM_ID { get; set; }
    }
}
