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
using ThienPhucDental.Medical.Dto;
using ThienPhucDental.Medical.Jobs;
using ThienPhucDental.ProcedureHelpers;
using Abp.BackgroundJobs;
using Abp.Runtime.Session;

namespace ThienPhucDental.Medical
{
    [AbpAuthorize]
    public class AppointmentAppService : IAppointmentAppService
    {
        private readonly IStoreProcedureProvider _storeProcedureProvider;
        private readonly IBackgroundJobManager _backgroundJobManager;
        public IAbpSession AbpSession { get; set; } = NullAbpSession.Instance;


        public AppointmentAppService(IStoreProcedureProvider storeProcedureProvider,
                                     IBackgroundJobManager backgroundJobManager)
        {
            _storeProcedureProvider = storeProcedureProvider;
            _backgroundJobManager = backgroundJobManager;

        }
        public async Task<MED_APPOINTMENT_ENTITY> MED_APPOINTMENT_GetById(string Id)
        {
            var result = (await _storeProcedureProvider.GetDataFromStoredProcedure<MED_APPOINTMENT_ENTITY>(CommonStoreProcedureConsts.MED_APPOINTMENT_BYID, new
            {
                P_APP_ID = Id
            })).FirstOrDefault();
            return result;
        }

        [AbpAuthorize(AppPermissions.Pages_Medical_Appointment)]
        public async Task<PagedResultDto<MED_APPOINTMENT_ENTITY>> MED_APPOINTMENT_Search(MED_APPOINTMENT_ENTITY input)
        {
            var result = await _storeProcedureProvider.GetPagingData<MED_APPOINTMENT_ENTITY>(CommonStoreProcedureConsts.MED_APPOINTMENT_SEARCH, input);
            return result;
        }

        [AbpAuthorize(AppPermissions.Pages_Medical_Appointment_Create)]
        public async Task<InsertResult> MED_APPOINTMENT_Ins(MED_APPOINTMENT_ENTITY input)
        {
            // 1. Thực thi Store Procedure lưu/tạo lịch hẹn
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.MED_APPOINTMENT_INS, input)).FirstOrDefault();

            // 2. Chuyển đổi chuỗi ngày giờ (APP_DATE + START_TIME) thành DateTime
            // Giả sử APP_DATE = "2026-08-20", START_TIME = "09:30"

            if (!string.IsNullOrWhiteSpace(input.APP_DATE) && !string.IsNullOrWhiteSpace(input.START_TIME))
            {
                if (DateTime.TryParse($"{input.APP_DATE} {input.START_TIME}", out DateTime appointmentTime))
                {
                    var now = DateTime.Now;

                    // Job 1: Nhắc Bác sĩ trước 15 phút
                    var doctorRemindTime = appointmentTime.AddMinutes(-15);
                    //if (doctorRemindTime > now)
                    //{
                    //    await _backgroundJobManager.EnqueueAsync<AppointmentReminderJob, AppointmentReminderArgs>(
                    //        new AppointmentReminderArgs
                    //        {
                    //            AppId = result.Id,
                    //            Type = AppointmentReminderType.DoctorBefore15Min
                    //        },
                    //        delay: doctorRemindTime - now // Tham số delay
                    //    );
                    //}

                    // Job 2: Báo Lễ tân sau 15 phút trễ
                    var overdueAlertTime = appointmentTime.AddMinutes(15);
                    if (overdueAlertTime > now)
                    {
                        await _backgroundJobManager.EnqueueAsync<AppointmentReminderJob, AppointmentReminderArgs>(
                            new AppointmentReminderArgs
                            {
                                AppId = result.Id,
                                Type = AppointmentReminderType.ReceptionistOverdue15Min,
                                CreatorUserId = AbpSession.UserId,
                                TenantId = AbpSession.TenantId
                            },
                            delay: overdueAlertTime - now
                        );
                    }
                }
            }
                return result;
        }

        [AbpAuthorize(AppPermissions.Pages_Medical_Appointment_Update)]
        public async Task<InsertResult> MED_APPOINTMENT_Upd(MED_APPOINTMENT_ENTITY input)
        {
            return (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.MED_APPOINTMENT_UPD, input)).FirstOrDefault();
        }

         [AbpAuthorize(AppPermissions.Pages_Medical_Appointment_Delete)]
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
