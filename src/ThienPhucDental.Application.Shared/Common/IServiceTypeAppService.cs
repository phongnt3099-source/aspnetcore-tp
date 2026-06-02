using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Utils;
using ThienPhucDental.Medical.Dto;

namespace ThienPhucDental.Common
{
    public interface IServiceTypeAppService : IApplicationService
    {
        Task<List<CM_SERVICE_TYPE_ENTITY>> CM_SERVICES_GetAll();
        Task<InsertResult> CM_SERVICE_TYPE_Ins(CM_SERVICE_TYPE_ENTITY input);
        Task<CommonResult> CM_SERVICE_TYPE_Del(string id);
        Task<PagedResultDto<CM_SERVICE_TYPE_ENTITY>> CM_SERVICE_TYPE_Search(CM_SERVICE_TYPE_ENTITY input);
    }
}
