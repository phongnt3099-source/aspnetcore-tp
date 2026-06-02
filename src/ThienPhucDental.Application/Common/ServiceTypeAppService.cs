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
    public class ServiceTypeAppService: IServiceTypeAppService
    {
        private readonly IStoreProcedureProvider _storeProcedureProvider;

        public ServiceTypeAppService(IStoreProcedureProvider storeProcedureProvider)
        {
            _storeProcedureProvider = storeProcedureProvider;

        }
        public async Task<List<CM_SERVICE_TYPE_ENTITY>> CM_SERVICES_GetAll()
        {
            var item = (await _storeProcedureProvider.GetDataFromStoredProcedure<CM_SERVICE_TYPE_ENTITY>(CommonStoreProcedureConsts.CM_SERVICE_TYPE_GETALL, new { }));
            return item;
        }
        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode)]
        public async Task<PagedResultDto<CM_SERVICE_TYPE_ENTITY>> CM_SERVICE_TYPE_Search(CM_SERVICE_TYPE_ENTITY input)
        {
            var result = await _storeProcedureProvider.GetPagingData<CM_SERVICE_TYPE_ENTITY>(CommonStoreProcedureConsts.CM_SERVICE_TYPE_SEARCH, input);
            return result;
        }
        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode_Create)]
        public async Task<InsertResult> CM_SERVICE_TYPE_Ins(CM_SERVICE_TYPE_ENTITY input)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_SERVICE_TYPE_INS, input)).FirstOrDefault();
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode_Update)]
        public async Task<InsertResult> CM_SERVICE_TYPE_Upd(CM_SERVICE_TYPE_ENTITY input)
        {
            return (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_SERVICE_TYPE_UPD, input)).FirstOrDefault();
        }

        // [AbpAuthorize(AppPermissions.Pages_Common_AllCode_Delete)]
        public async Task<CommonResult> CM_SERVICE_TYPE_Del(string id)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_SERVICE_TYPE_DEL, new
                {
                    ST_ID = id
                })).FirstOrDefault();
            return result;
        }
    }
}
