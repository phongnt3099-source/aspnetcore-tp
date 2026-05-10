using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Utils;
using ThienPhucDental.Medical.Dto;

namespace ThienPhucDental.Medical
{
    public interface IAppointmentAppService: IApplicationService
    {
        Task<PagedResultDto<MED_APPOINTMENT_ENTITY>> MED_APPOINTMENT_Search(MED_APPOINTMENT_ENTITY input);
        Task<MED_APPOINTMENT_ENTITY> MED_APPOINTMENT_GetById(string Id);
        Task<InsertResult> MED_APPOINTMENT_Ins(MED_APPOINTMENT_ENTITY input);
        Task<CommonResult> MED_APPOINTMENT_Del(string id);
    }
}
