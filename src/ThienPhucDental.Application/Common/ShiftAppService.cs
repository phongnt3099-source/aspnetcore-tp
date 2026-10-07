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
    public class ShiftAppService : ThienPhucDentalAppServiceBase,IShiftAppService
    {

        //[AbpAuthorize(AppPermissions.Pages_Common_Shift)]
        public async Task<List<CM_SHIFT_ENTITY>> CM_SHIFT_Search()
        {
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<CM_SHIFT_ENTITY>(CommonStoreProcedureConsts.CM_SHIFT_SEARCH, new {});
            return result;
        }

        public async Task<CM_SHIFT_ENTITY> CM_SHIFT_ById(string id)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<CM_SHIFT_ENTITY>(CommonStoreProcedureConsts.CM_SHIFT_BY_ID, new
                {
                    P_SHIFT_ID = id
                })).FirstOrDefault();
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_Shift_Create)]
        public async Task<InsertResult> CM_SHIFT_Ins(CM_SHIFT_ENTITY input)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_SHIFT_INS, new
                {
                    P_SHIFT_ID = input.SHIFT_ID,
                    P_SHIFT_NAME = input.SHIFT_NAME,
                    P_CHECK_IN_TIME = input.CHECK_IN_TIME,
                    P_CHECK_OUT_TIME = input.CHECK_OUT_TIME,
                    P_LATE_TOLERANCE = input.LATE_TOLERANCE,
                    P_ALLOW_BREAK = (input.ALLOW_BREAK == true) ? '1' : '0',
                    P_BREAK_START_TIME = input.ALLOW_BREAK == true ? input.BREAK_START_TIME : null,
                    P_BREAK_END_TIME = input.ALLOW_BREAK == true ? input.BREAK_END_TIME : null,
                    P_DAYS_OF_WEEK = input.DAYS_OF_WEEK,
                    P_ISACTIVE = input.ISACTIVE
                })).FirstOrDefault();
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_Shift_Update)]
        public async Task<InsertResult> CM_SHIFT_Upd(CM_SHIFT_ENTITY input)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_SHIFT_UPD, input)).FirstOrDefault();
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_Shift_Delete)]
        public async Task<CommonResult> CM_SHIFT_Del(string id)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_SHIFT_DEL, new
                {
                    P_SHIFT_ID = id
                })).FirstOrDefault();
            return result;
        }
    }
}