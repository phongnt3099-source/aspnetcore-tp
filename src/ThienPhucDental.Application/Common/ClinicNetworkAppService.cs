using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThienPhucDental.Authorization;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Consts;
using ThienPhucDental.CoreModule.Utils;
using ThienPhucDental.ProcedureHelpers;

namespace ThienPhucDental.Common
{
    [AbpAuthorize]
    public class ClinicNetworkAppService : ThienPhucDentalAppServiceBase, IClinicNetworkAppService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClinicNetworkAppService(IHttpContextAccessor httpContextAccessor )
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_ClinicNetwork)]
        public async Task<List<CM_CLINIC_NETWORK_ENTITY>> CM_CLINIC_NETWORK_Search()
        {
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<CM_CLINIC_NETWORK_ENTITY>(CommonStoreProcedureConsts.CM_CLINIC_NETWORK_SEARCH, new {});
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_ClinicNetwork_Create)]
        public async Task<InsertResult> CM_CLINIC_NETWORK_Ins(CM_CLINIC_NETWORK_ENTITY input)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_CLINIC_NETWORK_INS, new
                {
                    P_TENANT_ID  = AbpSession.TenantId.HasValue ? (int)AbpSession.TenantId.Value : 0,
                    P_ORGANIZATION_UNIT_NAME = input.ORGANIZATION_UNIT_NAME,
                    P_ALLOWED_IP = input.ALLOWED_IP,
                    P_DESCRIPTION = input.DESCRIPTION,
                    P_USER_NAME = GetCurrentUser().UserName
                })).FirstOrDefault();
            return result;
            
        }

        [AbpAuthorize(AppPermissions.Pages_Common_ClinicNetwork_Update)]
        public async Task<InsertResult> CM_CLINIC_NETWORK_Upd(CM_CLINIC_NETWORK_ENTITY input)
        {
            input.TENANT_ID = (int)AbpSession.TenantId;
            input.USER_NAME = GetCurrentUser().UserName;
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_CLINIC_NETWORK_UPD, input)).FirstOrDefault();
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_ClinicNetwork_Delete)]
        public async Task<CommonResult> CM_CLINIC_NETWORK_Del(string id)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_CLINIC_NETWORK_DEL, new
                {
                    P_ID = id,
                    P_MODIFIER_ID = GetCurrentUser().UserName
                })).FirstOrDefault();
            return result;
        }

        public async Task<string> GetCurrentClientIp()
        {
            var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

            // Xử lý trường hợp chạy qua Proxy / Cloudflare (nếu có)
            if (_httpContextAccessor.HttpContext?.Request.Headers.ContainsKey("X-Forwarded-For") == true)
            {
                ip = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            }

            return await Task.FromResult(ip ?? "127.0.0.1");
        }
    }
}