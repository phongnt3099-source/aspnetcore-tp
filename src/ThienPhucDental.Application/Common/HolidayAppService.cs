using Abp.Application.Services.Dto;
using Abp.Authorization;
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
    public class HolidayAppService : ThienPhucDentalAppServiceBase,IHolidayAppService
    {
        private readonly IStoreProcedureProvider _storeProcedureProvider;
        public HolidayAppService(IStoreProcedureProvider storeProcedureProvider)
        {
            _storeProcedureProvider = storeProcedureProvider;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_Holiday)]
        public async Task<List<CM_HOLIDAY_ENTITY>> CM_HOLIDAY_Search()
        {
            var result = await _storeProcedureProvider.GetDataFromStoredProcedure<CM_HOLIDAY_ENTITY>(CommonStoreProcedureConsts.CM_HOLIDAY_SEARCH, new {});
            return result;
        }

        public async Task<CM_HOLIDAY_ENTITY> CM_HOLIDAY_ById(string id)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<CM_HOLIDAY_ENTITY>(CommonStoreProcedureConsts.CM_HOLIDAY_BY_ID, new
                {
                    P_HOLIDAY_ID = id
                })).FirstOrDefault();
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_Holiday_Create)]
        public async Task<InsertResult> CM_HOLIDAY_Ins(CM_HOLIDAY_ENTITY input)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_HOLIDAY_INS, new
                {
                    P_HOLIDAY_ID = input.HOLIDAY_ID,
                    P_HOLIDAY_NAME = input.HOLIDAY_NAME,
                    P_FROM_DATE = input.FROM_DATE,
                    P_TO_DATE = input.TO_DATE,
                    P_IS_RECURRING = input.IS_RECURRING,
                    P_NOTES = input.NOTES,
                    P_MAKER_ID = GetCurrentUser().UserName,

                    P_ISACTIVE = input.ISACTIVE
                })).FirstOrDefault();
            return result;
        }

        [AbpAuthorize(AppPermissions.Pages_Common_Holiday_Update)]
        public async Task<InsertResult> CM_HOLIDAY_Upd(CM_HOLIDAY_ENTITY input)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_HOLIDAY_UPD, input)).FirstOrDefault();
            return result;
        }

        [AbpAuthorize(AppPermissions.Pages_Common_Holiday_Delete)]
        public async Task<CommonResult> CM_HOLIDAY_Del(string id)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_HOLIDAY_DEL, new
                {
                    P_HOLIDAY_ID = id,
                })).FirstOrDefault();
            return result;
        }
    }
}