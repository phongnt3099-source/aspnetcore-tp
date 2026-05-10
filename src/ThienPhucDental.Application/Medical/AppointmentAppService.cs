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
using ThienPhucDental.Medical.Dto;
using ThienPhucDental.ProcedureHelpers;

namespace ThienPhucDental.Medical
{
    [AbpAuthorize]
    public class AppointmentAppService : IAppointmentAppService
    {
        private readonly IStoreProcedureProvider _storeProcedureProvider;

        public AppointmentAppService(IStoreProcedureProvider storeProcedureProvider)
        {
            _storeProcedureProvider = storeProcedureProvider;

        }
        public async Task<MED_APPOINTMENT_ENTITY> MED_APPOINTMENT_GetById(string Id)
        {
            var result = (await _storeProcedureProvider.GetDataFromStoredProcedure<MED_APPOINTMENT_ENTITY>(CommonStoreProcedureConsts.MED_APPOINTMENT_BYID, new
            {
                P_APP_ID = Id
            })).FirstOrDefault();
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode)]
        public async Task<PagedResultDto<MED_APPOINTMENT_ENTITY>> MED_APPOINTMENT_Search(MED_APPOINTMENT_ENTITY input)
        {
            var result = await _storeProcedureProvider.GetPagingData<MED_APPOINTMENT_ENTITY>(CommonStoreProcedureConsts.MED_APPOINTMENT_SEARCH, input);
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode_Create)]
        public async Task<InsertResult> MED_APPOINTMENT_Ins(MED_APPOINTMENT_ENTITY input)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.MED_APPOINTMENT_INS, input)).FirstOrDefault();
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode_Update)]
        public async Task<InsertResult> MED_APPOINTMENT_Upd(MED_APPOINTMENT_ENTITY input)
        {
            return (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.MED_APPOINTMENT_UPD, input)).FirstOrDefault();
        }

        // [AbpAuthorize(AppPermissions.Pages_Common_AllCode_Delete)]
        public async Task<CommonResult> MED_APPOINTMENT_Del(string id)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.MED_APPOINTMENT_DEL, new
                {
                    APP_ID = id
                })).FirstOrDefault();
            return result;
        }
    }
}
