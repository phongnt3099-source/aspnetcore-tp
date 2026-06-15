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
    public interface IEmployeeAppService: IApplicationService
    {
        Task<List<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_DROPDOWNLIST(string EMP_ROLE);
        Task<InsertResult> CM_EMPLOYEE_Ins(CM_EMPLOYEE_ENTITY input);
        Task<CommonResult> CM_EMPLOYEE_Del(string id, string maker);
        Task<InsertResult> CM_EMPLOYEE_Upd(CM_EMPLOYEE_ENTITY input);
        Task<PagedResultDto<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_Search(CM_EMPLOYEE_ENTITY input);
        Task<InsertResult> CM_EMPLOYEE_Sync (CM_EMPLOYEE_ENTITY input);
    }
}
