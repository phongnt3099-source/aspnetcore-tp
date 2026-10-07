using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ThienPhucDental.Common.Dto
{
    public class CM_EMPLOYEE_FACE_ENTITY: IMayHaveTenant
    {
        public string EMP_ID { get; set; }
        public byte[] FACE_VECTOR { get; set; }
        public string FACE_IMAGE_URL { get; set; }
        public int? TenantId { get; set; }
    }
}
