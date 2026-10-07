using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Utils;

namespace ThienPhucDental.Common
{
    public interface IClinicNetworkAppService: IApplicationService
    {
        Task<List<CM_CLINIC_NETWORK_ENTITY>> CM_CLINIC_NETWORK_Search();
        Task<InsertResult> CM_CLINIC_NETWORK_Ins(CM_CLINIC_NETWORK_ENTITY input);
        Task<InsertResult> CM_CLINIC_NETWORK_Upd(CM_CLINIC_NETWORK_ENTITY input);
        Task<CommonResult> CM_CLINIC_NETWORK_Del(string id);
        Task<string> GetCurrentClientIp();
    }
}
