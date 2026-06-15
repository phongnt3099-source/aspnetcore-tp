using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Consts;
using ThienPhucDental.CoreModule.Utils;
using ThienPhucDental.ProcedureHelpers;

namespace ThienPhucDental.Common
{
    [AbpAuthorize]
    public class ServiceAppService : IServiceAppService
    {
        private readonly IStoreProcedureProvider _storeProcedureProvider;

        public ServiceAppService(IStoreProcedureProvider storeProcedureProvider)
        {
            _storeProcedureProvider = storeProcedureProvider;

        }
        public async Task<List<CM_SERVICES_ENTITY>> CM_SERVICES_GetByType(string Keyword, string ST_ID)
        {
            var item = (await _storeProcedureProvider.GetDataFromStoredProcedure<CM_SERVICES_ENTITY>(CommonStoreProcedureConsts.CM_SERVICES_GETBYTYPE, new
            {
                @p_Keyword = Keyword,
                @p_ST_ID = ST_ID
            }));
            return item;
        }
        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode)]
        public async Task<PagedResultDto<CM_SERVICES_ENTITY>> CM_SERVICES_Search(CM_SERVICES_ENTITY input)
        {
            var result = await _storeProcedureProvider.GetPagingData<CM_SERVICES_ENTITY>(CommonStoreProcedureConsts.CM_SERVICES_SEARCH, input);
            return result;
        }
        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode_Create)]
        public async Task<InsertResult> CM_SERVICES_Ins(CM_SERVICES_ENTITY input)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_SERVICES_INS, input)).FirstOrDefault();
            return result;
        }
        public async Task<InsertResult> CM_SERVICES_Sync(CM_SERVICES_ENTITY input)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_EMPLOYEE_SYNC_USER, input)).FirstOrDefault();
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode_Update)]
        public async Task<InsertResult> CM_SERVICES_Upd(CM_SERVICES_ENTITY input)
        {
            return (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_SERVICES_UPD, input)).FirstOrDefault();
        }

        // [AbpAuthorize(AppPermissions.Pages_Common_AllCode_Delete)]
        public async Task<CommonResult> CM_SERVICES_Del(string id)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_SERVICES_DEL, new
                {
                    SRV_ID = id
                })).FirstOrDefault();
            return result;
        }
        
    }
}
