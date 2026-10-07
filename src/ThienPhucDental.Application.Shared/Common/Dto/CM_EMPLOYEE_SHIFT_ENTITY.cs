using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Common.Dto
{
    public class CM_EMPLOYEE_SHIFT_ENTITY
    {
        public string EMP_ID { get; set; }
        public string EMP_NAME { get; set; }
        public string EMP_PHONE { get; set; }
        public string EMP_CCCD { get; set; }
        public string SHIFT_ID { get; set; }
        public string SHIFT_NAME { get; set; }
        public string CHECK_IN_TIME { get; set; }
        public string CHECK_OUT_TIME { get; set; }
        public string APPLY_MONTH { get; set; }
        public string DAYS_OF_WEEK { get; set; }
        public string EXPIRE_DATE { get; set; }
        public string ASSIGNMENT_TYPE { get; set; }
        public string ISACTIVE { get; set; }
        public DateTime? CREATE_DATE { get; set; }

        // Bổ sung các thuộc tính phục vụ giao diện (nếu cần tương tác trực tiếp trên View)
        public bool? isEditing { get; set; }
    }
    public class CM_EMPLOYEE_SHIFT_SearchInput : PagedAndSortedInputDto
    {
        public string Keyword { get; set; }
        public string APPLY_MONTH { get; set; }
    }
}
