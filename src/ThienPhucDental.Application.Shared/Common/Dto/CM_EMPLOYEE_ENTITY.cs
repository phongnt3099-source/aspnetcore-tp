using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ThienPhucDental.Common.Dto
{
    public class CM_EMPLOYEE_ENTITY: PagedAndSortedInputDto
    {
        public string EMP_ID { get; set; }

        public string EMP_NAME { get; set; }

        public string EMP_PHONE { get; set; }

        public DateTime? EMP_DOB { get; set; }

        public string EMP_CCCD { get; set; }

        public string EMP_GENDER { get; set; }

        public string EMP_ADDRESS { get; set; }

        public string EMP_CITY { get; set; }

        public string EMP_WARD { get; set; }

        public double? EMP_NO { get; set; }

        public string EMP_ROLE { get; set; }

        public string NOTES { get; set; }

        public string RECORD_STATUS { get; set; }

        public string ISACTIVE { get; set; }

        public string MAKER_ID { get; set; }

        public DateTime? CREATE_DT { get; set; }
    }
}
