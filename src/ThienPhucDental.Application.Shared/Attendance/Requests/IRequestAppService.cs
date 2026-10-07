using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.Attendance.Requests.Dto;
using ThienPhucDental.CoreModule.Utils;

namespace ThienPhucDental.Attendance.Requests
{
    public interface IRequestAppService : IApplicationService
    {
        Task<List<ApproverDto>> AT_REQUEST_GetApprovers(string excludeEmpId, string keyword);
        Task<CommonResult> AT_REQUEST_Create(CreateRequestInput input);
        Task<List<RequestDto>> AT_REQUEST_GetPendingForMe(string approverId);
        Task<List<RequestDto>> AT_REQUEST_GetMyRequests(string empId, string status);
        Task<CommonResult> AT_REQUEST_Approve(ApproveRequestInput input);
        Task<CommonResult> AT_REQUEST_Reject(RejectRequestInput input);
        Task<CommonResult> AT_REQUEST_Cancel(long requestId, string empId);
    }
}
