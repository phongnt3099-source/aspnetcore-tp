using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Medical.Dto
{
    public class ExaminationStatusCountDto
    {
        public int? COUNT_NEW {  get; set; }
        public int? COUNT_DOING {  get; set; }
        public int? COUNT_COMPLETE {  get; set; }
        public int? COUNT_CANCEL {  get; set; }
    }
}
