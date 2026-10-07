using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Utils;

namespace ThienPhucDental.Common
{
    public interface IEmployeeShiftAppService: IApplicationService
    {
        Task<PagedResultDto<CM_EMPLOYEE_SHIFT_ENTITY>> CM_EMPLOYEE_SHIFT_Search(CM_EMPLOYEE_SHIFT_SearchInput input);
        Task<List<dynamic>> CM_EMPLOYEE_SHIFT_InsOrUpd(List<CM_EMPLOYEE_SHIFT_ENTITY> input);
        Task<CommonResult> CM_EMPLOYEE_SHIFT_Del(string empId,string type,string month);
        Task<List<CM_EMPLOYEE_ENTITY>> GetAllActiveEmployees();
    }
}
