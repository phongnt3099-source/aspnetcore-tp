using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Medical.Dto
{
    public class ExaminationTimelineDto
    {
        public string EXM_PATIENT_ID { get; set; }
        public string EXM_ID { get; set; }
        public string TimelineDate { get; set; }
        public string Diagnosis { get; set; }
        public string MainDoctorName { get; set; }
        public string AllServices { get; set; }
        public string ExaminationStatus { get; set; }
    }
}
