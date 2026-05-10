using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.Common.Dto;

namespace ThienPhucDental.Common
{
    public interface IEmployeeAppService: IApplicationService
    {
        Task<List<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_DROPDOWNLIST(string EMP_ROLE);
    }
}
