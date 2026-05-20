using System;
using System.Collections.Generic;
using System.Text;
using ThienPhucDental.Common.Dto;

namespace ThienPhucDental.Medical.Dto
{
    public class MED_EXAMINATION_ENTITY : PagedAndSortedInputDto
    {
        public string EXM_ID { get; set; }
        public string EXM_CODE { get; set; }
        public string EXM_PATIENT_ID { get; set; }
        public string EXM_DOCTOR_ID { get; set; }
        public string EXM_DATE { get; set; }
        public string EXM_CHIEF_COMPLAINT { get; set; }
        public string EXM_CLINICAL_SIGNS { get; set; }
        public string EXM_DIAGNOSIS { get; set; }
        public int? EXM_PULSE { get; set; }
        public string EXM_BLOOD_PRESSURE { get; set; }
        public string EXM_NOTE { get; set; }
        public string EXM_CREATE { get; set; }
        public byte? EXM_STATUS { get; set; }
        public decimal? EXM_TOTAL_DISCOUNT { get; set; }
        public decimal? EXM_FINAL_AMOUNT { get; set; }
        public decimal? EXM_TOTAL_RAW { get; set; }
        public decimal? EXM_SUB_TOTAL { get; set; }
        public string MAKER_ID { get; set; }
        public List<MED_TREATMENT_DETAIL_ENTITY> TreatmentDetails { get; set; }
    }
}
