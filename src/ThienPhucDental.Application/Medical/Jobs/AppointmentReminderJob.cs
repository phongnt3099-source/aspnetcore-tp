using Abp;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Notifications;
using Abp.RealTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.CoreModule.Consts;
using ThienPhucDental.Medical.Dto;
using ThienPhucDental.Notifications;
using ThienPhucDental.ProcedureHelpers;
using Abp.RealTime;

namespace ThienPhucDental.Medical.Jobs
{
    public enum AppointmentReminderType
    {
        DoctorBefore15Min = 1,
        ReceptionistOverdue15Min = 2
    }

    public class AppointmentReminderArgs
    {
        public string AppId { get; set; } 
        public AppointmentReminderType Type { get; set; }
        public long? CreatorUserId { get; set; }
        public int? TenantId { get; set; }
    }

    public class AppointmentReminderJob : AsyncBackgroundJob<AppointmentReminderArgs>, ITransientDependency
    {
        private readonly IStoreProcedureProvider _storeProcedureProvider;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IOnlineClientManager _onlineClientManager;

        public AppointmentReminderJob(
            IStoreProcedureProvider storeProcedureProvider,
            INotificationPublisher notificationPublisher,
            IOnlineClientManager onlineClientManager)
        {
            _storeProcedureProvider = storeProcedureProvider;
            _notificationPublisher = notificationPublisher;
            _onlineClientManager = onlineClientManager;
        }

        public override async Task ExecuteAsync(AppointmentReminderArgs args)
        {
            var appointment = (await _storeProcedureProvider.GetDataFromStoredProcedure<MED_APPOINTMENT_ENTITY>(CommonStoreProcedureConsts.MED_APPOINTMENT_BYID, new
            {
                P_APP_ID = args.AppId
            })).FirstOrDefault();

            // Chỉ gửi thông báo nếu lịch hẹn tồn tại và trạng thái vẫn là "chua-den"
            if (appointment == null || appointment.APP_STATUS != "chua-den")
            {
                return;
            }

            // 3. Xử lý thông báo theo loại Job
            if (args.Type == AppointmentReminderType.DoctorBefore15Min)
            {
                // Nhắc Bác sĩ trước 15 phút
                if (long.TryParse(appointment.APP_DOC_ID ?? appointment.DOC_ID, out long docUserId))
                {
                    await _notificationPublisher.PublishAsync(
                        notificationName: "App.Appointment.DoctorReminder",
                        data: new MessageNotificationData(
                            $"Lịch hẹn với bệnh nhân {appointment.CUS_NAME} sẽ bắt đầu lúc {appointment.START_TIME}."
                        ),
                        severity: NotificationSeverity.Info,
                        userIds: new[] { new UserIdentifier(null, docUserId) }
                    );
                }
            }
            else if (args.Type == AppointmentReminderType.ReceptionistOverdue15Min)
            {
                // Báo Lễ tân khi khách trễ 15 phút (gửi đích danh cho người đã tạo lịch)
                if (args.CreatorUserId.HasValue)
                {
                    var targetUser = new UserIdentifier(args.TenantId, args.CreatorUserId.Value);

                    // 1. Kiểm tra xem Server có thấy User này đang kết nối SignalR không
                    var onlineClients = await _onlineClientManager.GetAllByUserIdAsync(targetUser);

                    Logger.Info($"[SignalR Check] Số lượng kết nối online của UserId {args.CreatorUserId}: {onlineClients.Count}");

                    if (onlineClients.Count == 0)
                    {
                        Logger.Warn($"[SignalR Warning] User {args.CreatorUserId} (Tenant: {args.TenantId}) KHÔNG có kết nối nào online!");
                    }

                    var allClients = await _onlineClientManager.GetAllClientsAsync();

                    Logger.Info($"[SignalR Debug] Tổng số kết nối trên toàn Server: {allClients.Count}");
                    foreach (var client in allClients)
                    {
                        Logger.Info($"--> Client online: ConnectionId={client.ConnectionId}, UserId={client.UserId}, TenantId={client.TenantId}");
                    }

                    // 2. Bắn thông báo
                    await _notificationPublisher.PublishAsync(
                        notificationName: AppNotificationNames.AppointmentOverdueAlert,
                        data: new MessageNotificationData(
                            $"Bệnh nhân {appointment.CUS_NAME} (Hẹn lúc: {appointment.START_TIME}) đã quá 15 phút chưa đến."
                        ),
                        severity: NotificationSeverity.Warn,
                        userIds: new[] { targetUser }
                    );
                }
            }
        }
    }
}
