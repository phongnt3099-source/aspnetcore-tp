using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Extensions;
using ThienPhucDental.Attendance.Requests.Dto;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Consts;
using ThienPhucDental.CoreModule.Utils;
using ThienPhucDental.ProcedureHelpers;

namespace ThienPhucDental.Attendance.Requests
{
    [AbpAuthorize]
    public class RequestAppService : ThienPhucDentalAppServiceBase, IRequestAppService
    {
        public async Task<List<ApproverDto>> AT_REQUEST_GetApprovers(string excludeEmpId, string keyword)
        {
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<ApproverDto>(
                CommonStoreProcedureConsts.AT_REQUEST_GET_APPROVERS,
                new
                {
                    P_EXCLUDE_EMP_ID = excludeEmpId,
                    P_KEYWORD = keyword.IsNullOrEmpty() ? null : keyword
                }
            );

            return result ?? new List<ApproverDto>();
        }

        public async Task<CommonResult> AT_REQUEST_Create(CreateRequestInput input)
        {
            try
            {
                if (input.EMP_ID.IsNullOrEmpty())
                    return new CommonResult { Result = "0", ErrorDesc = "Mã nhân viên không được để trống." };

                if (input.APPROVER_ID.IsNullOrEmpty())
                    return new CommonResult { Result = "0", ErrorDesc = "Vui lòng chọn người duyệt." };

                if (input.EMP_ID == input.APPROVER_ID)
                    return new CommonResult { Result = "0", ErrorDesc = "Không thể tự duyệt đơn của chính mình." };

                var result = (await storeProcedureProvider
                    .GetDataFromStoredProcedure<CommonResult>(
                        CommonStoreProcedureConsts.AT_REQUEST_CREATE,
                        new
                        {
                            P_EMP_ID = input.EMP_ID,
                            P_REQUEST_TYPE = input.REQUEST_TYPE,
                            P_APPROVER_ID = input.APPROVER_ID,
                            P_FROM_DATE = input.FROM_DATE,
                            P_TO_DATE = input.TO_DATE,
                            P_FROM_TIME = input.FROM_TIME,
                            P_TO_TIME = input.TO_TIME,
                            P_FIX_DATE = input.FIX_DATE,
                            P_FIX_CHECK_IN = input.FIX_CHECK_IN,
                            P_FIX_CHECK_OUT = input.FIX_CHECK_OUT,
                            P_REASON = input.REASON,
                            P_ATTACHMENT_URL = input.ATTACHMENT_URL
                        })).FirstOrDefault();

                return result ?? new CommonResult { Result = "0", ErrorDesc = "Không có phản hồi từ hệ thống." };
            }
            catch (Exception ex)
            {
                Logger.Error($"AT_REQUEST_Create Error: {ex.Message}", ex);
                return new CommonResult { Result = "-1", ErrorDesc = $"Lỗi hệ thống: {ex.Message}" };
            }
        }

        public async Task<List<RequestDto>> AT_REQUEST_GetPendingForMe(string approverId)
        {
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<RequestDto>(
                CommonStoreProcedureConsts.AT_REQUEST_GET_PENDING_FOR_ME,
                new { P_APPROVER_ID = approverId }
            );

            return result ?? new List<RequestDto>();
        }

        public async Task<List<RequestDto>> AT_REQUEST_GetMyRequests(string empId, string status)
        {
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<RequestDto>(
                CommonStoreProcedureConsts.AT_REQUEST_GET_MY_REQUESTS,
                new
                {
                    P_EMP_ID = empId,
                    P_STATUS = status.IsNullOrEmpty() ? null : status
                }
            );

            return result ?? new List<RequestDto>();
        }

        public async Task<CommonResult> AT_REQUEST_Approve(ApproveRequestInput input)
        {
            try
            {
                if (input.REQUEST_ID <= 0)
                    return new CommonResult { Result = "0", ErrorDesc = "Mã đơn không hợp lệ." };

                if (input.APPROVER_ID.IsNullOrEmpty())
                    return new CommonResult { Result = "0", ErrorDesc = "Không xác định được người duyệt." };

                var result = (await storeProcedureProvider
                    .GetDataFromStoredProcedure<CommonResult>(
                        CommonStoreProcedureConsts.AT_REQUEST_APPROVE,
                        new
                        {
                            P_REQUEST_ID = input.REQUEST_ID,
                            P_APPROVER_ID = input.APPROVER_ID,
                            P_NOTE = input.NOTE
                        })).FirstOrDefault();

                return result ?? new CommonResult { Result = "0", ErrorDesc = "Không có phản hồi từ hệ thống." };
            }
            catch (Exception ex)
            {
                Logger.Error($"AT_REQUEST_Approve Error: {ex.Message}", ex);
                return new CommonResult { Result = "-1", ErrorDesc = $"Lỗi hệ thống: {ex.Message}" };
            }
        }

        public async Task<CommonResult> AT_REQUEST_Reject(RejectRequestInput input)
        {
            try
            {
                if (input.REQUEST_ID <= 0)
                    return new CommonResult { Result = "0", ErrorDesc = "Mã đơn không hợp lệ." };

                if (input.REASON.IsNullOrEmpty())
                    return new CommonResult { Result = "0", ErrorDesc = "Vui lòng nhập lý do từ chối." };

                var result = (await storeProcedureProvider
                    .GetDataFromStoredProcedure<CommonResult>(
                        CommonStoreProcedureConsts.AT_REQUEST_REJECT,
                        new
                        {
                            P_REQUEST_ID = input.REQUEST_ID,
                            P_APPROVER_ID = input.APPROVER_ID,
                            P_REASON = input.REASON
                        })).FirstOrDefault();

                return result ?? new CommonResult { Result = "0", ErrorDesc = "Không có phản hồi từ hệ thống." };
            }
            catch (Exception ex)
            {
                Logger.Error($"AT_REQUEST_Reject Error: {ex.Message}", ex);
                return new CommonResult { Result = "-1", ErrorDesc = $"Lỗi hệ thống: {ex.Message}" };
            }
        }

        public async Task<CommonResult> AT_REQUEST_Cancel(long requestId, string empId)
        {
            try
            {
                if (requestId <= 0)
                    return new CommonResult { Result = "0", ErrorDesc = "Mã đơn không hợp lệ." };

                var result = (await storeProcedureProvider
                    .GetDataFromStoredProcedure<CommonResult>(
                        CommonStoreProcedureConsts.AT_REQUEST_CANCEL,
                        new
                        {
                            P_REQUEST_ID = requestId,
                            P_EMP_ID = empId
                        })).FirstOrDefault();

                return result ?? new CommonResult { Result = "0", ErrorDesc = "Không có phản hồi từ hệ thống." };
            }
            catch (Exception ex)
            {
                Logger.Error($"AT_REQUEST_Cancel Error: {ex.Message}", ex);
                return new CommonResult { Result = "-1", ErrorDesc = $"Lỗi hệ thống: {ex.Message}" };
            }
        }
    }
}