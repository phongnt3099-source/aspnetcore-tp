using Abp.Application.Services.Dto;
using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.Authorization;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Consts;
using ThienPhucDental.CoreModule.Utils;
using ThienPhucDental.ProcedureHelpers;

namespace ThienPhucDental.Common
{
    [AbpAuthorize]
    public class EmployeeAppService : IEmployeeAppService
    {
        private readonly IStoreProcedureProvider _storeProcedureProvider;
        public EmployeeAppService(IStoreProcedureProvider storeProcedureProvider)
        {
            _storeProcedureProvider = storeProcedureProvider;

        }

        public async Task<List<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_DROPDOWNLIST(string EMP_ROLE)
        {
            var result = await _storeProcedureProvider
                .GetDataFromStoredProcedure<CM_EMPLOYEE_ENTITY>(CommonStoreProcedureConsts.CM_EMPLOYEE_DROPDOWNLIST, new
                {
                    P_EMP_ROLE = EMP_ROLE
                });

            return result;
        }

        [AbpAuthorize(AppPermissions.Pages_Common_Employee)]
        public async Task<PagedResultDto<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_Search(CM_EMPLOYEE_ENTITY input)
        {
            var result = await _storeProcedureProvider.GetPagingData<CM_EMPLOYEE_ENTITY>(CommonStoreProcedureConsts.CM_EMPLOYEE_SEARCH, input);
            return result;
        }
        [AbpAuthorize(AppPermissions.Pages_Common_Employee_Create)]
        public async Task<InsertResult> CM_EMPLOYEE_Ins(CM_EMPLOYEE_ENTITY input)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_EMPLOYEE_INS, new
                {
                    P_EMP_NAME = input.EMP_NAME,
                    P_EMP_PHONE = input.EMP_PHONE,
                    P_EMP_DOB = input.EMP_DOB,
                    P_EMP_CCCD = input.EMP_CCCD,
                    P_EMP_GENDER = input.EMP_GENDER,
                    P_EMP_ADDRESS = input.EMP_ADDRESS,
                    P_EMP_CITY = input.EMP_CITY,
                    P_EMP_WARD = input.EMP_WARD,
                    P_EMP_NO = input.EMP_NO,
                    P_NOTES = input.NOTES,
                    P_MAKER_ID = input.MAKER_ID,
                    P_IS_CREATE_USER = (input.IS_CREATE_USER == true) ? 1 : 0,
                    P_EMAIL = input.EMAIL,
                    P_USERNAME = input.USER_NAME,
                })).FirstOrDefault();
            return result;
        }

        [AbpAuthorize(AppPermissions.Pages_Common_Employee_Update)]
        public async Task<InsertResult> CM_EMPLOYEE_Sync(CM_EMPLOYEE_ENTITY input)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_EMPLOYEE_SYNC_USER, input)).FirstOrDefault();
            return result;
        }

        [AbpAuthorize(AppPermissions.Pages_Common_Employee_Update)]
        public async Task<InsertResult> CM_EMPLOYEE_Upd(CM_EMPLOYEE_ENTITY input)
        {
            return (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_EMPLOYEE_UPD, input)).FirstOrDefault();
        }

         [AbpAuthorize(AppPermissions.Pages_Common_Employee_Delete)]
        public async Task<CommonResult> CM_EMPLOYEE_Del(string id, string maker_id)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_EMPLOYEE_DEL, new
                {
                    EMP_ID = id,
                    MAKER_ID = maker_id
                })).FirstOrDefault();
            return result;
        }
    }
}
