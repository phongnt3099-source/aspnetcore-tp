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
    public interface ICustomerAppService: IApplicationService
    {
        Task<PagedResultDto<CM_CUSTOMER_ENTITY>> CM_CUSTOMER_Search(CM_CUSTOMER_ENTITY input);
        Task<CM_CUSTOMER_ENTITY> CM_CUSTOMER_GetById(string Id);
        Task<InsertResult> CM_CUSTOMER_Ins(CM_CUSTOMER_ENTITY input);
        Task<CommonResult> CM_CUSTOMER_Del(string id);
    }
}
