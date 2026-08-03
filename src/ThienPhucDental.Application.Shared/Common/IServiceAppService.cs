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
    public interface IServiceAppService: IApplicationService
    {
        Task<List<CM_SERVICES_ENTITY>> CM_SERVICES_GetByType(string Keyword, string ST_ID);
        Task<InsertResult> CM_SERVICES_Ins(CM_SERVICES_ENTITY input);
        Task<CommonResult> CM_SERVICES_Del(string id);
        Task<InsertResult> CM_SERVICES_Upd(CM_SERVICES_ENTITY input);
        Task<PagedResultDto<CM_SERVICES_ENTITY>> CM_SERVICES_Search(CM_SERVICES_ENTITY input);
    }
}
