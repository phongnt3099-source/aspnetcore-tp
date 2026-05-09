using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.UI;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Consts;
using ThienPhucDental.CoreModule.Utils;
using ThienPhucDental.Editions;
using ThienPhucDental.ProcedureHelpers;

namespace ThienPhucDental.Common
{
    [AbpAuthorize]
    public class CustomerAppService: ICustomerAppService
    {
        private readonly IStoreProcedureProvider _storeProcedureProvider;

        public CustomerAppService( IStoreProcedureProvider storeProcedureProvider)
        {
            _storeProcedureProvider = storeProcedureProvider;

        }
        public async Task<CM_CUSTOMER_ENTITY> CM_CUSTOMER_GetById(string Id)
        {
            var result = (await _storeProcedureProvider.GetDataFromStoredProcedure<CM_CUSTOMER_ENTITY>(CommonStoreProcedureConsts.CM_CUSTOMER_BYID, new
            {
                P_CUS_ID = Id
            })).FirstOrDefault();
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode)]
        public async Task<PagedResultDto<CM_CUSTOMER_ENTITY>> CM_CUSTOMER_Search(CM_CUSTOMER_ENTITY input)
        {
            var result = await _storeProcedureProvider.GetPagingData<CM_CUSTOMER_ENTITY>(CommonStoreProcedureConsts.CM_CUSTOMER_SEARCH, input);
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode_Create)]
        public async Task<InsertResult> CM_CUSTOMER_Ins(CM_CUSTOMER_ENTITY input)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_CUSTOMER_INS, input)).FirstOrDefault();
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode_Update)]
        public async Task<InsertResult> CM_CUSTOMER_Upd(CM_CUSTOMER_ENTITY input)
        {
            return (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_CUSTOMER_UPD, input)).FirstOrDefault();
        }

        // [AbpAuthorize(AppPermissions.Pages_Common_AllCode_Delete)]
        public async Task<CommonResult> CM_CUSTOMER_Del(string id)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_CUSTOMER_DEL, new
                {
                    CUS_ID = id
                })).FirstOrDefault();
            return result;
        }
    }
}
