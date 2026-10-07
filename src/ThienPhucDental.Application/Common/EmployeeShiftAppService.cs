using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Extensions;
using Abp.Json;
using System;
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
    public class EmployeeShiftAppService : ThienPhucDentalAppServiceBase, IEmployeeShiftAppService
    {
        public async Task<PagedResultDto<CM_EMPLOYEE_SHIFT_ENTITY>> CM_EMPLOYEE_SHIFT_Search(CM_EMPLOYEE_SHIFT_SearchInput input)
        {
            var result = await storeProcedureProvider.GetPagingData<CM_EMPLOYEE_SHIFT_ENTITY>(
                CommonStoreProcedureConsts.CM_EMPLOYEE_SHIFT_SEARCH,
                input
            );  
            return result;
        }

        public async Task<List<dynamic>> CM_EMPLOYEE_SHIFT_InsOrUpd(List<CM_EMPLOYEE_SHIFT_ENTITY> input)
        {
            try
            {
                if (input == null || !input.Any()) 
                {
                    return new List<dynamic> { new { Result = "-1", ErrorDesc = "Danh sách phân công ca không được để trống." } };
                }

                var result = await storeProcedureProvider
                    .GetMultiResultValueFromStore(CommonStoreProcedureConsts.CM_EMPLOYEE_SHIFT_INS_OR_UPD, new
                    {
                        p_SHIFT_DATA = input.ToJsonString(), 
                        p_MAKER_ID = AbpSession.UserId?.ToString()
                    });

                return result ?? new List<dynamic>();
            }
            catch (Exception ex)
            {
                Logger.Error($"CM_EMPLOYEE_SHIFT_InsOrUpd Error: {ex.Message}", ex);
                return new List<dynamic> { new { Result = "-1", ErrorDesc = $"Lỗi hệ thống: {ex.Message}" } };
            }
        }

        public async Task<InsertResult> CM_EMPLOYEE_SHIFT_Upd(CM_EMPLOYEE_SHIFT_ENTITY input)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_EMPLOYEE_SHIFT_UPD, new
                {
                    p_EMP_ID = input.EMP_ID,
                    p_SHIFT_ID = input.SHIFT_ID,
                    p_DAYS_OF_WEEK = input.DAYS_OF_WEEK,
                    p_MAKER_ID = AbpSession.UserId?.ToString()
                })).FirstOrDefault();
            return result;
        }

        public async Task<CommonResult> CM_EMPLOYEE_SHIFT_Del(string empId, string type,string month)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_EMPLOYEE_SHIFT_DEL, new
                {
                    p_EMP_ID = empId,
                    p_ASSIGNMENT_TYPE = type,
                    p_APPLY_MONTH = month,
                    p_MAKER_ID = AbpSession.UserId?.ToString()
                })).FirstOrDefault();
            return result;
        }

        public async Task<List<CM_EMPLOYEE_ENTITY>> GetAllActiveEmployees()
        {
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<CM_EMPLOYEE_ENTITY>(
                CommonStoreProcedureConsts.CM_EMPLOYEE_SHIFT_GETALLACTIVE,
                new { }
            );
            return result;
        }

        public async Task<List<CM_EMPLOYEE_SHIFT_ENTITY>> CM_EMPLOYEE_SHIFT_GetByEmpAndMonth(string empId, string applyMonth)
        {
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<CM_EMPLOYEE_SHIFT_ENTITY>(
                CommonStoreProcedureConsts.CM_EMPLOYEE_SHIFT_BY_EMP_AND_MONTH,
                new
                {
                    P_EMP_ID = empId,
                    P_APPLY_MONTH = applyMonth
                }
            );

            return result ?? new List<CM_EMPLOYEE_SHIFT_ENTITY>();
        }
    }
}