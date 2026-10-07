using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Utils;

namespace ThienPhucDental.Common
{
    public interface IAttendanceAppService : IApplicationService
    {
        Task<AttendanceResultDto> ProcessFaceAttendanceAsync(AttendanceInputDto input);
        Task<InsertResult> RegisterEmployeeFaceAsync(RegisterFaceInputDto input);
        Task<ValidateFaceLinkOutputDto> ValidateFaceRegistrationLinkAsync(string c, long? userId, string token);
    }
}
