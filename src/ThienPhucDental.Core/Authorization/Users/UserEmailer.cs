using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Localization;
using Abp.Net.Mail;
using ThienPhucDental.Chat;
using ThienPhucDental.Editions;
using ThienPhucDental.Localization;
using ThienPhucDental.MultiTenancy;
using System.Net.Mail;
using System.Web;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.Timing;
using ThienPhucDental.Configuration;
using ThienPhucDental.Net.Emailing;

namespace ThienPhucDental.Authorization.Users
{
    /// <summary>
    /// Used to send email to users.
    /// </summary>
    public class UserEmailer : ThienPhucDentalServiceBase, IUserEmailer, ITransientDependency
    {
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly ICurrentUnitOfWorkProvider _unitOfWorkProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ISettingManager _settingManager;
        private readonly EditionManager _editionManager;
        private readonly UserManager _userManager;
        private readonly IAbpSession _abpSession;

        // used for styling action links on email messages.
        private string _emailButtonStyle =
            "padding-left: 30px; padding-right: 30px; padding-top: 12px; padding-bottom: 12px; color: #ffffff; background-color: #00bb77; font-size: 14pt; text-decoration: none;";

        private string _emailButtonColor = "#00bb77";

        public UserEmailer(
            IEmailTemplateProvider emailTemplateProvider,
            IEmailSender emailSender,
            IRepository<Tenant> tenantRepository,
            ICurrentUnitOfWorkProvider unitOfWorkProvider,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            EditionManager editionManager,
            UserManager userManager,
            IAbpSession abpSession)
        {
            _emailTemplateProvider = emailTemplateProvider;
            _emailSender = emailSender;
            _tenantRepository = tenantRepository;
            _unitOfWorkProvider = unitOfWorkProvider;
            _unitOfWorkManager = unitOfWorkManager;
            _settingManager = settingManager;
            _editionManager = editionManager;
            _userManager = userManager;
            _abpSession = abpSession;
        }

        /// <summary>
        /// Send email activation link to user's email address.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Email activation link</param>
        /// <param name="plainPassword">
        /// Can be set to user's plain password to include it in the email.
        /// </param>
        public virtual async Task SendEmailActivationLinkAsync(User user, string link, string plainPassword = null)
        {
            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                if (user.EmailConfirmationCode.IsNullOrEmpty())
                {
                    throw new Exception("Mã xác thực email chưa được khởi tạo.");
                }

                link = link.Replace("{userId}", user.Id.ToString());
                link = link.Replace("{confirmationCode}", Uri.EscapeDataString(user.EmailConfirmationCode));

                if (user.TenantId.HasValue)
                {
                    link = link.Replace("{tenantId}", user.TenantId.ToString());
                }

                link = EncryptQueryParameters(link);

                var tenancyName = GetTenancyNameOrNull(user.TenantId);
                var emailTemplate = GetTitleAndSubTitle(user.TenantId, "Xác Thực Tài Khoản", "Cảm ơn bạn đã lựa chọn dịch vụ của chúng tôi");

                var fullName = $"{user.Surname} {user.Name}".Trim();
                var displayTenancy = !tenancyName.IsNullOrEmpty() ? tenancyName : "Nha Khoa Thiên Phúc";

                var mailMessage = new StringBuilder();

                // Template HTML Luxury & Chuyên nghiệp
                mailMessage.AppendLine(@"
    <div style=""max-width: 580px; margin: 0 auto; background-color: #ffffff; font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; color: #2c3e50; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 20px rgba(0,0,0,0.06); border: 1px solid #eaeaea;"">
        
        <!-- Header / Banner -->
        <div style=""background: linear-gradient(135deg, #1e293b 0%, #0f172a 100%); padding: 32px 24px; text-align: center;"">
            <h1 style=""color: #d4af37; font-size: 22px; font-weight: 600; letter-spacing: 1px; margin: 0; text-transform: uppercase;"">
                XÁC NHẬN TÀI KHOẢN
            </h1>
            <p style=""color: #94a3b8; font-size: 13px; margin-top: 6px; margin-bottom: 0;"">
                Chỉ còn một bước nữa để hoàn tất đăng ký
            </p>
        </div>

        <!-- Body Content -->
        <div style=""padding: 36px 32px;"">
            <p style=""font-size: 15px; color: #334155; line-height: 1.6; margin-top: 0;"">
                Xin chào <strong style=""color: #0f172a;"">" + fullName + @"</strong>,
            </p>
            <p style=""font-size: 14px; color: #64748b; line-height: 1.6;"">
                Cảm ơn bạn đã đăng ký tài khoản. Dưới đây là thông tin đăng nhập được khởi tạo cho bạn:
            </p>

            <!-- Info Box -->
            <div style=""background-color: #f8fafc; border-left: 4px solid #d4af37; padding: 18px 20px; border-radius: 6px; margin: 24px 0;"">
                <table style=""width: 100%; border-collapse: collapse; font-size: 14px;"">
                    <tr>
                        <td style=""padding: 4px 0; color: #64748b; width: 120px;"">Hệ thống:</td>
                        <td style=""padding: 4px 0; color: #0f172a; font-weight: 600;"">" + displayTenancy + @"</td>
                    </tr>
                    <tr>
                        <td style=""padding: 4px 0; color: #64748b;"">Tài khoản:</td>
                        <td style=""padding: 4px 0; color: #0f172a; font-weight: 600;"">" + user.UserName + @"</td>
                    </tr>");

                if (!plainPassword.IsNullOrEmpty())
                {
                    mailMessage.AppendLine(@"
                    <tr>
                        <td style=""padding: 4px 0; color: #64748b;"">Mật khẩu:</td>
                        <td style=""padding: 4px 0; color: #0f172a; font-weight: 600;"">" + plainPassword + @"</td>
                    </tr>");
                }

                mailMessage.AppendLine(@"
                </table>
                        </div>

                        <!-- Call To Action Button -->
                        <div style=""text-align: center; margin: 36px 0;"">
                            <a href=""" + link + @""" style=""background: linear-gradient(135deg, #d4af37 0%, #b8860b 100%); color: #ffffff; text-decoration: none; padding: 14px 36px; border-radius: 30px; font-weight: 600; font-size: 14px; display: inline-block; letter-spacing: 0.5px; box-shadow: 0 4px 12px rgba(184, 134, 11, 0.25);"">
                                KÍCH HOẠT TÀI KHOẢN NGAY
                            </a>
                        </div>

                        <p style=""font-size: 13px; color: #94a3b8; line-height: 1.5; margin-bottom: 8px;"">
                            Nếu nút bấm trên không hoạt động, bạn có thể sao chép và dán liên kết dưới đây vào trình duyệt:
                        </p>
                        <div style=""word-break: break-all; font-size: 12px; color: #2563eb; background-color: #f1f5f9; padding: 10px; border-radius: 6px; font-family: monospace;"">
                            " + link + @"
                        </div>
                    </div>

                    <!-- Footer -->
                    <div style=""background-color: #f8fafc; border-top: 1px solid #f1f5f9; padding: 20px; text-align: center; font-size: 12px; color: #94a3b8;"">
                        <p style=""margin: 0;"">Email này được gửi tự động, vui lòng không phản hồi trực tiếp.</p>
                        <p style=""margin: 4px 0 0 0;"">&copy; Nha Khoa Thiên Phúc. All rights reserved.</p>
                    </div>
                </div>");

                await ReplaceBodyAndSendAsync(
                    user.EmailAddress,
                    "Xác thực địa chỉ Email - Nha Khoa Thiên Phúc",
                    emailTemplate,
                    mailMessage
                );
            });
        }

        /// <summary>
        /// Sends a password reset link to user's email.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Reset link</param>
        public async Task SendPasswordResetLinkAsync(User user, string link = null)
        {
            var expirationHours = await _settingManager.GetSettingValueAsync<int>(
                AppSettings.UserManagement.Password.PasswordResetCodeExpirationHours
            );

            if (user.PasswordResetCode.IsNullOrEmpty())
            {
                throw new Exception("Mã đặt lại mật khẩu chưa được khởi tạo.");
            }

            var tenancyName = GetTenancyNameOrNull(user.TenantId);
            var emailTemplate = GetTitleAndSubTitle(user.TenantId, "Yêu Cầu Đặt Lại Mật Khẩu", "Hướng dẫn thiết lập lại mật khẩu tài khoản");

            var fullName = $"{user.Surname} {user.Name}".Trim();
            var displayTenancy = !tenancyName.IsNullOrEmpty() ? tenancyName : "Nha Khoa Thiên Phúc";
            var mailMessage = new StringBuilder();

            // Xử lý link nếu có
            if (!link.IsNullOrEmpty())
            {
                link = link.Replace("{userId}", user.Id.ToString());
                link = link.Replace("{resetCode}", Uri.EscapeDataString(user.PasswordResetCode));

                var expireDate = Uri.EscapeDataString(Clock.Now.AddHours(expirationHours)
                    .ToString(ThienPhucDentalConsts.DateTimeOffsetFormat, CultureInfo.InvariantCulture));

                link = link.Replace("{expireDate}", expireDate);

                if (user.TenantId.HasValue)
                {
                    link = link.Replace("{tenantId}", user.TenantId.ToString());
                }

                link = EncryptQueryParameters(link);
            }

            // Template HTML Luxury & Chuyên nghiệp
            mailMessage.AppendLine(@"
                <div style=""max-width: 580px; margin: 0 auto; background-color: #ffffff; font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; color: #2c3e50; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 20px rgba(0,0,0,0.06); border: 1px solid #eaeaea;"">
    
                    <!-- Header / Banner -->
                    <div style=""background: linear-gradient(135deg, #1e293b 0%, #0f172a 100%); padding: 32px 24px; text-align: center;"">
                        <h1 style=""color: #d4af37; font-size: 22px; font-weight: 600; letter-spacing: 1px; margin: 0; text-transform: uppercase;"">
                            ĐẶT LẠI MẬT KHẨU
                        </h1>
                        <p style=""color: #94a3b8; font-size: 13px; margin-top: 6px; margin-bottom: 0;"">
                            Khôi phục quyền truy cập tài khoản của bạn
                        </p>
                    </div>

                    <!-- Body Content -->
                    <div style=""padding: 36px 32px;"">
                        <p style=""font-size: 15px; color: #334155; line-height: 1.6; margin-top: 0;"">
                            Xin chào <strong style=""color: #0f172a;"">" + fullName + @"</strong>,
                        </p>
                        <p style=""font-size: 14px; color: #64748b; line-height: 1.6;"">
                            Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn tại hệ thống. Thông tin chi tiết:
                        </p>

                        <!-- Info Box -->
                        <div style=""background-color: #f8fafc; border-left: 4px solid #d4af37; padding: 18px 20px; border-radius: 6px; margin: 24px 0;"">
                            <table style=""width: 100%; border-collapse: collapse; font-size: 14px;"">
                                <tr>
                                    <td style=""padding: 4px 0; color: #64748b; width: 130px;"">Hệ thống:</td>
                                    <td style=""padding: 4px 0; color: #0f172a; font-weight: 600;"">" + displayTenancy + @"</td>
                                </tr>
                                <tr>
                                    <td style=""padding: 4px 0; color: #64748b;"">Tài khoản:</td>
                                    <td style=""padding: 4px 0; color: #0f172a; font-weight: 600;"">" + user.UserName + @"</td>
                                </tr>
                                <tr>
                                    <td style=""padding: 4px 0; color: #64748b;"">Mã xác thực:</td>
                                    <td style=""padding: 4px 0; color: #d4af37; font-weight: 700; letter-spacing: 1px;"">" + user.PasswordResetCode + @"</td>
                                </tr>
                                <tr>
                                    <td style=""padding: 4px 0; color: #64748b;"">Thời hạn hiệu lực:</td>
                                    <td style=""padding: 4px 0; color: #e11d48; font-weight: 600;"">" + expirationHours + @" giờ</td>
                                </tr>
                            </table>
                        </div>");

                            if (!link.IsNullOrEmpty())
                            {
                                mailMessage.AppendLine(@"
                        <p style=""font-size: 14px; color: #64748b; text-align: center; margin-top: 28px;"">
                            Vui lòng nhấn vào nút bên dưới để tiến hành thiết lập mật khẩu mới:
                        </p>

                        <!-- Call To Action Button -->
                        <div style=""text-align: center; margin: 28px 0 36px 0;"">
                            <a href=""" + link + @""" style=""background: linear-gradient(135deg, #d4af37 0%, #b8860b 100%); color: #ffffff; text-decoration: none; padding: 14px 36px; border-radius: 30px; font-weight: 600; font-size: 14px; display: inline-block; letter-spacing: 0.5px; box-shadow: 0 4px 12px rgba(184, 134, 11, 0.25);"">
                                ĐẶT LẠI MẬT KHẨU
                            </a>
                        </div>

                        <p style=""font-size: 13px; color: #94a3b8; line-height: 1.5; margin-bottom: 8px;"">
                            Nếu nút bấm trên không hoạt động, bạn có thể sao chép và dán liên kết dưới đây vào trình duyệt:
                        </p>
                        <div style=""word-break: break-all; font-size: 12px; color: #2563eb; background-color: #f1f5f9; padding: 10px; border-radius: 6px; font-family: monospace;"">
                            " + link + @"
                        </div>");
                            }

                            mailMessage.AppendLine(@"
                        <p style=""font-size: 13px; color: #e11d48; margin-top: 24px; line-height: 1.5; background-color: #fff1f2; padding: 12px; border-radius: 6px;"">
                            ⚠️ <strong>Lưu ý:</strong> Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email hoặc liên hệ với quản trị viên để bảo vệ tài khoản.
                        </p>
                    </div>

                    <!-- Footer -->
                    <div style=""background-color: #f8fafc; border-top: 1px solid #f1f5f9; padding: 20px; text-align: center; font-size: 12px; color: #94a3b8;"">
                        <p style=""margin: 0;"">Email này được gửi tự động, vui lòng không phản hồi trực tiếp.</p>
                        <p style=""margin: 4px 0 0 0;"">&copy; Nha Khoa Thiên Phúc. All rights reserved.</p>
                    </div>
                </div>");

            await ReplaceBodyAndSendAsync(
                user.EmailAddress,
                "Yêu cầu Đặt lại Mật khẩu - Nha Khoa Thiên Phúc",
                emailTemplate,
                mailMessage
            );
        }

        public async Task TryToSendChatMessageMail(User user, string senderUsername, string senderTenancyName,
            ChatMessage chatMessage)
        {
            try
            {
                var emailTemplate = GetTitleAndSubTitle(user.TenantId, L("NewChatMessageEmail_Title"),
                    L("NewChatMessageEmail_SubTitle"));
                var mailMessage = new StringBuilder();

                mailMessage.AppendLine("<b>" + L("Sender") + "</b>: " + senderTenancyName + "/" + senderUsername +
                                       "<br />");
                mailMessage.AppendLine("<b>" + L("Time") + "</b>: " +
                                       chatMessage.CreationTime.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss") +
                                       " UTC<br />");
                mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + chatMessage.Message + "<br />");
                mailMessage.AppendLine("<br />");

                await ReplaceBodyAndSendAsync(user.EmailAddress, L("NewChatMessageEmail_Subject"), emailTemplate,
                    mailMessage);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        public async Task SendEmailChangeRequestLinkAsync(User user, string emailAddress, string link)
        {
            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                link = link.Replace("{userId}", user.Id.ToString());
                link = link.Replace("{emailAddress}", Uri.EscapeDataString(emailAddress));
                link = link.Replace("{oldMailAddress}", Uri.EscapeDataString(user.EmailAddress));

                if (user.TenantId.HasValue)
                {
                    link = link.Replace("{tenantId}", user.TenantId.ToString());
                }

                link = EncryptQueryParameters(link);

                var tenancyName = GetTenancyNameOrNull(user.TenantId);
                var emailTemplate = GetTitleAndSubTitle(user.TenantId, L("EmailChangeRequest_Title"),
                    L("EmailChangeRequest_SubTitle"));
                var mailMessage = new StringBuilder();

                mailMessage.AppendLine("<b>" + L("NameSurname") + "</b>: " + user.Name + " " + user.Surname + "<br />");

                if (!tenancyName.IsNullOrEmpty())
                {
                    mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
                }

                mailMessage.AppendLine("<b>" + L("UserName") + "</b>: " + user.UserName + "<br />");
                
                mailMessage.AppendLine("<b>" + L("NewEmailAddress") + "</b>: " + emailAddress + "<br />");

                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine(L("EmailChangeRequest_ClickTheLinkBelowToChangeYourEmail") + "<br /><br />");
                mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor +
                                       "\" href=\"" + link + "\">" + L("Verify") + "</a>");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<br />");
                mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" +
                                       L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
                mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");

                await ReplaceBodyAndSendAsync(user.EmailAddress, L("EmailChangeRequest_Subject"), emailTemplate,
                    mailMessage);
            });
        }

        public async Task TryToSendSubscriptionExpireEmail(int tenantId, DateTime utcNow)
        {
            try
            {
                using (_unitOfWorkManager.Begin())
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        var tenantAdmin = await _userManager.GetAdminAsync();
                        if (tenantAdmin == null || string.IsNullOrEmpty(tenantAdmin.EmailAddress))
                        {
                            return;
                        }

                        var hostAdminLanguage = await _settingManager.GetSettingValueForUserAsync(
                            LocalizationSettingNames.DefaultLanguage, tenantAdmin.TenantId, tenantAdmin.Id);
                        var culture = CultureHelper.GetCultureInfoByChecking(hostAdminLanguage);
                        var emailTemplate = GetTitleAndSubTitle(tenantId, L("SubscriptionExpire_Title"),
                            L("SubscriptionExpire_SubTitle"));
                        var mailMessage = new StringBuilder();

                        mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + L("SubscriptionExpire_Email_Body",
                            culture, utcNow.ToString("yyyy-MM-dd") + " UTC") + "<br />");
                        mailMessage.AppendLine("<br />");

                        await ReplaceBodyAndSendAsync(tenantAdmin.EmailAddress, L("SubscriptionExpire_Email_Subject"),
                            emailTemplate, mailMessage);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        public async Task TryToSendSubscriptionAssignedToAnotherEmail(int tenantId, DateTime utcNow,
            int expiringEditionId)
        {
            try
            {
                using (_unitOfWorkManager.Begin())
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        var tenantAdmin = await _userManager.GetAdminAsync();
                        if (tenantAdmin == null || string.IsNullOrEmpty(tenantAdmin.EmailAddress))
                        {
                            return;
                        }

                        var hostAdminLanguage = await _settingManager.GetSettingValueForUserAsync(
                            LocalizationSettingNames.DefaultLanguage, tenantAdmin.TenantId, tenantAdmin.Id);
                        var culture = CultureHelper.GetCultureInfoByChecking(hostAdminLanguage);
                        var expringEdition = await _editionManager.GetByIdAsync(expiringEditionId);
                        var emailTemplate = GetTitleAndSubTitle(tenantId, L("SubscriptionExpire_Title"),
                            L("SubscriptionExpire_SubTitle"));
                        var mailMessage = new StringBuilder();

                        mailMessage.AppendLine("<b>" + L("Message") + "</b>: " +
                                               L("SubscriptionAssignedToAnother_Email_Body", culture,
                                                   expringEdition.DisplayName, utcNow.ToString("yyyy-MM-dd") + " UTC") +
                                               "<br />");
                        mailMessage.AppendLine("<br />");

                        await ReplaceBodyAndSendAsync(tenantAdmin.EmailAddress, L("SubscriptionExpire_Email_Subject"),
                            emailTemplate, mailMessage);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        public async Task TryToSendFailedSubscriptionTerminationsEmail(List<string> failedTenancyNames, DateTime utcNow)
        {
            try
            {
                var hostAdmin = await _userManager.GetAdminAsync();
                if (hostAdmin == null || string.IsNullOrEmpty(hostAdmin.EmailAddress))
                {
                    return;
                }

                var hostAdminLanguage =
                    await _settingManager.GetSettingValueForUserAsync(LocalizationSettingNames.DefaultLanguage,
                        hostAdmin.TenantId, hostAdmin.Id);
                var culture = CultureHelper.GetCultureInfoByChecking(hostAdminLanguage);
                var emailTemplate = GetTitleAndSubTitle(null, L("FailedSubscriptionTerminations_Title"),
                    L("FailedSubscriptionTerminations_SubTitle"));
                var mailMessage = new StringBuilder();

                mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + L("FailedSubscriptionTerminations_Email_Body",
                    culture, string.Join(",", failedTenancyNames), utcNow.ToString("yyyy-MM-dd") + " UTC") + "<br />");
                mailMessage.AppendLine("<br />");

                await ReplaceBodyAndSendAsync(hostAdmin.EmailAddress, L("FailedSubscriptionTerminations_Email_Subject"),
                    emailTemplate, mailMessage);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        public async Task TryToSendSubscriptionExpiringSoonEmail(int tenantId, DateTime dateToCheckRemainingDayCount)
        {
            try
            {
                await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
                {
                    using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        var tenantAdmin = await _userManager.GetAdminAsync();
                        if (tenantAdmin == null || string.IsNullOrEmpty(tenantAdmin.EmailAddress))
                        {
                            return;
                        }

                        var tenantAdminLanguage = await _settingManager.GetSettingValueForUserAsync(
                            LocalizationSettingNames.DefaultLanguage,
                            tenantAdmin.TenantId,
                            tenantAdmin.Id
                        );

                        var culture = CultureHelper.GetCultureInfoByChecking(tenantAdminLanguage);

                        var emailTemplate = GetTitleAndSubTitle(
                            null,
                            L("SubscriptionExpiringSoon_Title"),
                            L("SubscriptionExpiringSoon_SubTitle")
                        );

                        var mailMessage = new StringBuilder();

                        mailMessage.AppendLine("<b>" + L("Message") + "</b>: " +
                                               L("SubscriptionExpiringSoon_Email_Body", culture,
                                                   dateToCheckRemainingDayCount.ToString("yyyy-MM-dd") + " UTC") +
                                               "<br />");
                        mailMessage.AppendLine("<br />");

                        await ReplaceBodyAndSendAsync(
                            tenantAdmin.EmailAddress,
                            L("SubscriptionExpiringSoon_Email_Subject"),
                            emailTemplate,
                            mailMessage
                        );
                    }
                });
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        public void TryToSendPaymentNotCompletedEmail(int tenantId, string urlToPayment)
        {
            try
            {
                _unitOfWorkManager.WithUnitOfWork(() =>
                {
                    using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        var tenantAdmin = _userManager.GetAdmin();
                        if (tenantAdmin == null || string.IsNullOrEmpty(tenantAdmin.EmailAddress))
                        {
                            return;
                        }

                        var emailTemplate = GetTitleAndSubTitle(null, L("SubscriptionPaymentNotCompleted_Title"),
                            L("SubscriptionPaymentNotCompleted_SubTitle"));
                        var mailMessage = new StringBuilder();

                        mailMessage.AppendLine(L("SubscriptionPaymentNotCompleted_Email_Body", urlToPayment) +
                                               "<br />");
                        mailMessage.AppendLine("<br />");

                        ReplaceBodyAndSend(tenantAdmin.EmailAddress, L("SubscriptionPaymentNotCompleted_Email_Subject"),
                            emailTemplate, mailMessage);
                    }
                });
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        private string GetTenancyNameOrNull(int? tenantId)
        {
            if (tenantId == null)
            {
                return null;
            }

            using (_unitOfWorkProvider.Current.SetTenantId(null))
            {
                return _tenantRepository.Get(tenantId.Value).TenancyName;
            }
        }

        private StringBuilder GetTitleAndSubTitle(int? tenantId, string title, string subTitle)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(tenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", title);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", subTitle);

            return emailTemplate;
        }

        private async Task ReplaceBodyAndSendAsync(string emailAddress, string subject, StringBuilder emailTemplate,
            StringBuilder mailMessage)
        {
            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            await _emailSender.SendAsync(new MailMessage
            {
                To = { emailAddress },
                Subject = subject,
                Body = emailTemplate.ToString(),
                IsBodyHtml = true
            });
        }

        private void ReplaceBodyAndSend(string emailAddress, string subject, StringBuilder emailTemplate,
            StringBuilder mailMessage)
        {
            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            _emailSender.Send(new MailMessage
            {
                To = { emailAddress },
                Subject = subject,
                Body = emailTemplate.ToString(),
                IsBodyHtml = true
            });
        }

        /// <summary>
        /// Returns link with encrypted parameters
        /// </summary>
        /// <param name="link"></param>
        /// <param name="encrptedParameterName"></param>
        /// <returns></returns>
        private string EncryptQueryParameters(string link, string encrptedParameterName = "c")
        {
            if (!link.Contains("?"))
            {
                return link;
            }

            var basePath = link.Substring(0, link.IndexOf('?'));
            var query = link.Substring(link.IndexOf('?')).TrimStart('?');

            return basePath + "?" + encrptedParameterName + "=" +
                   HttpUtility.UrlEncode(SimpleStringCipher.Instance.Encrypt(query));
        }

        public async Task SendFaceRegistrationLinkAsync(User user, string registrationToken, string link = null)
        {
            // Thời hạn link mặc định: 4 giờ (hoặc lấy từ setting nếu có)
            var expirationHours = 4;

            if (registrationToken.IsNullOrEmpty())
            {
                throw new Exception("Mã xác thực đăng ký khuôn mặt chưa được tạo.");
            }

            var tenancyName = GetTenancyNameOrNull(user.TenantId);
            var emailTemplate = GetTitleAndSubTitle(user.TenantId, "Đăng Ký Khuôn Mặt Chấm Công", "Thiết lập dữ liệu nhận diện khuôn mặt nhân viên");

            var fullName = $"{user.Surname} {user.Name}".Trim();
            var displayTenancy = !tenancyName.IsNullOrEmpty() ? tenancyName : "Nha Khoa Thiên Phúc";
            var mailMessage = new StringBuilder();

            // Xử lý link đăng ký
            if (!link.IsNullOrEmpty())
            {
                link = link.Replace("{userId}", user.Id.ToString());
                link = link.Replace("{token}", Uri.EscapeDataString(registrationToken));

                var expireDate = Uri.EscapeDataString(Clock.Now.AddHours(expirationHours)
                    .ToString(ThienPhucDentalConsts.DateTimeOffsetFormat, CultureInfo.InvariantCulture));

                link = link.Replace("{expireDate}", expireDate);

                if (user.TenantId.HasValue)
                {
                    link = link.Replace("{tenantId}", user.TenantId.ToString());
                }

                link = EncryptQueryParameters(link);
            }

            // Template HTML Luxury & Chuyên nghiệp
            mailMessage.AppendLine(@"
        <div style=""max-width: 580px; margin: 0 auto; background-color: #ffffff; font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; color: #2c3e50; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 20px rgba(0,0,0,0.06); border: 1px solid #eaeaea;"">
    
            <!-- Header / Banner -->
            <div style=""background: linear-gradient(135deg, #1e293b 0%, #0f172a 100%); padding: 32px 24px; text-align: center;"">
                <h1 style=""color: #d4af37; font-size: 22px; font-weight: 600; letter-spacing: 1px; margin: 0; text-transform: uppercase;"">
                    ĐĂNG KÝ KHUÔN MẶT CHẤM CÔNG
                </h1>
                <p style=""color: #94a3b8; font-size: 13px; margin-top: 6px; margin-bottom: 0;"">
                    Thiết lập dữ liệu sinh trắc học cá nhân phục vụ chấm công
                </p>
            </div>

            <!-- Body Content -->
            <div style=""padding: 36px 32px;"">
                <p style=""font-size: 15px; color: #334155; line-height: 1.6; margin-top: 0;"">
                    Xin chào <strong style=""color: #0f172a;"">" + fullName + @"</strong>,
                </p>
                <p style=""font-size: 14px; color: #64748b; line-height: 1.6;"">
                    Hệ thống đã tạo liên kết đăng ký dữ liệu khuôn mặt cho tài khoản của bạn. Vui lòng sử dụng thiết bị cá nhân hoặc thiết bị được cấp quyền để tiến hành chụp ảnh nhận diện:
                </p>

                <!-- Info Box -->
                <div style=""background-color: #f8fafc; border-left: 4px solid #d4af37; padding: 18px 20px; border-radius: 6px; margin: 24px 0;"">
                    <table style=""width: 100%; border-collapse: collapse; font-size: 14px;"">
                        <tr>
                            <td style=""padding: 4px 0; color: #64748b; width: 140px;"">Phòng khám:</td>
                            <td style=""padding: 4px 0; color: #0f172a; font-weight: 600;"">" + displayTenancy + @"</td>
                        </tr>
                        <tr>
                            <td style=""padding: 4px 0; color: #64748b;"">Tài khoản nhân sự:</td>
                            <td style=""padding: 4px 0; color: #0f172a; font-weight: 600;"">" + user.UserName + @"</td>
                        </tr>
                        <tr>
                            <td style=""padding: 4px 0; color: #64748b;"">Thời hạn liên kết:</td>
                            <td style=""padding: 4px 0; color: #e11d48; font-weight: 600;"">" + expirationHours + @" giờ (Sử dụng 1 lần)</td>
                        </tr>
                    </table>
                </div>");

            if (!link.IsNullOrEmpty())
            {
                mailMessage.AppendLine(@"
                <p style=""font-size: 14px; color: #64748b; text-align: center; margin-top: 28px;"">
                    Nhấn vào nút bên dưới để mở camera và hoàn tất quá trình nhận diện:
                </p>

                <!-- Call To Action Button -->
                <div style=""text-align: center; margin: 28px 0 36px 0;"">
                    <a href=""" + link + @""" style=""background: linear-gradient(135deg, #d4af37 0%, #b8860b 100%); color: #ffffff; text-decoration: none; padding: 14px 36px; border-radius: 30px; font-weight: 600; font-size: 14px; display: inline-block; letter-spacing: 0.5px; box-shadow: 0 4px 12px rgba(184, 134, 11, 0.25);"">
                        BẮT ĐẦU ĐĂNG KÝ
                    </a>
                </div>

                <p style=""font-size: 13px; color: #94a3b8; line-height: 1.5; margin-bottom: 8px;"">
                    Nếu nút bấm không mở được liên kết, bạn có thể sao chép URL sau vào trình duyệt:
                </p>
                <div style=""word-break: break-all; font-size: 12px; color: #2563eb; background-color: #f1f5f9; padding: 10px; border-radius: 6px; font-family: monospace;"">
                    " + link + @"
                </div>");
            }

            mailMessage.AppendLine(@"
                <!-- Hướng dẫn chụp ảnh -->
                <div style=""margin-top: 24px; padding: 14px; background-color: #f0fdf4; border: 1px solid #bbf7d0; border-radius: 6px;"">
                    <p style=""font-size: 13px; color: #166534; margin: 0 0 6px 0; font-weight: 600;"">
                        💡 Lưu ý khi chụp khuôn mặt:
                    </p>
                    <ul style=""font-size: 12px; color: #15803d; margin: 0; padding-left: 18px; line-height: 1.6;"">
                        <li>Đứng ở nơi có đầy đủ ánh sáng, không ngược sáng.</li>
                        <li>Nhìn thẳng vào camera, không đeo kính râm hoặc khẩu trang.</li>
                        <li>Đảm bảo giữ khuôn mặt nằm gọn trong khung tròn định vị.</li>
                    </ul>
                </div>

                <p style=""font-size: 13px; color: #e11d48; margin-top: 20px; line-height: 1.5; background-color: #fff1f2; padding: 12px; border-radius: 6px;"">
                    ⚠️ <strong>Cảnh báo bảo mật:</strong> Liên kết này chứa mã định danh sinh trắc học của riêng bạn. Tuyệt đối không chuyển tiếp (forward) email này cho người khác.
                </p>
            </div>

            <!-- Footer -->
            <div style=""background-color: #f8fafc; border-top: 1px solid #f1f5f9; padding: 20px; text-align: center; font-size: 12px; color: #94a3b8;"">
                <p style=""margin: 0;"">Email này được gửi tự động từ hệ thống chấm công, vui lòng không phản hồi trực tiếp.</p>
                <p style=""margin: 4px 0 0 0;"">&copy; Nha Khoa Thiên Phúc. All rights reserved.</p>
            </div>
        </div>");

            await ReplaceBodyAndSendAsync(
                user.EmailAddress,
                "Đăng ký khuôn mặt chấm công - Nha Khoa Thiên Phúc",
                emailTemplate,
                mailMessage
            );
        }
    }
}
