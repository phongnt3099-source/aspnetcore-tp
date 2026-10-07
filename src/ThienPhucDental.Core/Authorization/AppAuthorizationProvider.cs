using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace ThienPhucDental.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));
            //pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangeProfilePicture, L("UpdateUsersProfilePicture"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeDefaultLanguage, L("ChangeDefaultLanguage"));
            
            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicProperties = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties, L("DynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Create, L("CreatingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Edit, L("EditingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Delete, L("DeletingDynamicProperties"));

            var dynamicPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue, L("DynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Create, L("CreatingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit, L("EditingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete, L("DeletingDynamicPropertyValue"));

            var dynamicEntityProperties = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties, L("DynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Create, L("CreatingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Edit, L("EditingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Delete, L("DeletingDynamicEntityProperties"));

            var dynamicEntityPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue, L("EntityDynamicPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create, L("CreatingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit, L("EditingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete, L("DeletingDynamicEntityPropertyValue"));

            var massNotification = administration.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification, L("MassNotifications"));
            massNotification.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification_Create, L("MassNotificationCreate"));
            
            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            
            var maintenance = administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            maintenance.CreateChildPermission(AppPermissions.Pages_Administration_NewVersion_Create, L("SendNewVersionNotification"));
            
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);

            //ALLCODE
            var allcode = pages.CreateChildPermission(AppPermissions.Pages_Common_AllCode, L("AllCodes"), multiTenancySides: MultiTenancySides.Host);
            allcode.CreateChildPermission(AppPermissions.Pages_Common_AllCode_Create, L("CreatingNewAllCodes"), multiTenancySides: MultiTenancySides.Host);
            allcode.CreateChildPermission(AppPermissions.Pages_Common_AllCode_Update, L("EditingAllCodes"), multiTenancySides: MultiTenancySides.Host);
            allcode.CreateChildPermission(AppPermissions.Pages_Common_AllCode_Delete, L("DeletingAllCodes"), multiTenancySides: MultiTenancySides.Host);

            // 1. Quản lý Khách hàng
            var customer = pages.CreateChildPermission(AppPermissions.Pages_Common_Customer, L("Pages_Common_Customer"));
            customer.CreateChildPermission(AppPermissions.Pages_Common_Customer_Create, L("Create"));
            customer.CreateChildPermission(AppPermissions.Pages_Common_Customer_ViewDetail, L("ViewDetail"));
            customer.CreateChildPermission(AppPermissions.Pages_Common_Customer_Update, L("Edit"));
            customer.CreateChildPermission(AppPermissions.Pages_Common_Customer_Print, L("Print"));
            customer.CreateChildPermission(AppPermissions.Pages_Common_Customer_Delete, L("Delete"));

            // 2. Quản lý Nhân viên
            var employee = pages.CreateChildPermission(AppPermissions.Pages_Common_Employee, L("Pages_Common_Employee"));
            employee.CreateChildPermission(AppPermissions.Pages_Common_Employee_Create, L("Create"));
            employee.CreateChildPermission(AppPermissions.Pages_Common_Employee_Update, L("Edit"));
            employee.CreateChildPermission(AppPermissions.Pages_Common_Employee_Delete, L("Delete"));

            // 3. Quản lý Dịch vụ
            var service = pages.CreateChildPermission(AppPermissions.Pages_Common_Service, L("Pages_Common_Service"));
            service.CreateChildPermission(AppPermissions.Pages_Common_Service_Create, L("Create"));
            service.CreateChildPermission(AppPermissions.Pages_Common_Service_Update, L("Edit"));
            service.CreateChildPermission(AppPermissions.Pages_Common_Service_Delete, L("Delete"));

            // 4. Quản lý Loại dịch vụ
            var serviceType = pages.CreateChildPermission(AppPermissions.Pages_Common_ServiceType, L("Pages_Common_ServiceType"));
            serviceType.CreateChildPermission(AppPermissions.Pages_Common_ServiceType_Create, L("Create"));
            serviceType.CreateChildPermission(AppPermissions.Pages_Common_ServiceType_Update, L("Edit"));
            serviceType.CreateChildPermission(AppPermissions.Pages_Common_ServiceType_Delete, L("Delete"));

            // 5. Quản lý Lịch hẹn
            var appointment = pages.CreateChildPermission(AppPermissions.Pages_Medical_Appointment, L("Pages_Medical_Appointment"));
            appointment.CreateChildPermission(AppPermissions.Pages_Medical_Appointment_Create, L("Create"));
            appointment.CreateChildPermission(AppPermissions.Pages_Medical_Appointment_Update, L("Edit"));
            appointment.CreateChildPermission(AppPermissions.Pages_Medical_Appointment_Delete, L("Delete"));

            // 6. Phân quyền Khám & Điều trị
            var medicalExamination = pages.CreateChildPermission(AppPermissions.Pages_Medical_Examination, L("Pages_Medical_Examination"));
            medicalExamination.CreateChildPermission(AppPermissions.Pages_Medical_Examination_Create, L("Create"));
            medicalExamination.CreateChildPermission(AppPermissions.Pages_Medical_Examination_Update, L("Edit"));
            medicalExamination.CreateChildPermission(AppPermissions.Pages_Medical_Examination_Delete, L("Delete"));
            medicalExamination.CreateChildPermission(AppPermissions.Pages_Medical_Examination_Print, L("Print"));

            // 7. Phân quyền Phiếu Thu Chi
            var financeTransaction = pages.CreateChildPermission(AppPermissions.Pages_Finance_Transaction, L("Pages_Finance_Transaction"));
            financeTransaction.CreateChildPermission(AppPermissions.Pages_Finance_Transaction_Create, L("Create"));
            financeTransaction.CreateChildPermission(AppPermissions.Pages_Finance_Transaction_Update, L("Edit")); // Hoặc dùng chung L("Edit")
            financeTransaction.CreateChildPermission(AppPermissions.Pages_Finance_Transaction_Delete, L("Delete"));
            financeTransaction.CreateChildPermission(AppPermissions.Pages_Finance_Transaction_Print, L("Print"));
            financeTransaction.CreateChildPermission(AppPermissions.Pages_Finance_Transaction_AdminBypass, L("AdminBypassHistoryLock"));

            // --- ATTENDANCE CONFIGURATION ---
            var attendanceConfig = pages.CreateChildPermission(AppPermissions.Pages_Common_Holiday, L("AttendanceConfiguration"));
            attendanceConfig.CreateChildPermission(AppPermissions.Pages_Common_Holiday_Create, L("Create"));
            attendanceConfig.CreateChildPermission(AppPermissions.Pages_Common_Holiday_Update, L("Edit"));
            attendanceConfig.CreateChildPermission(AppPermissions.Pages_Common_Holiday_Delete, L("Delete"));

            // --- EMPLOYEE SHIFT CONFIGURATION ---
            var employeeShiftConfig = pages.CreateChildPermission(AppPermissions.Pages_Common_EmployeeShift, L("EmployeeShiftConfiguration"));
            employeeShiftConfig.CreateChildPermission(AppPermissions.Pages_Common_EmployeeShift_Create, L("Create"));
            employeeShiftConfig.CreateChildPermission(AppPermissions.Pages_Common_EmployeeShift_Update, L("Edit"));
            employeeShiftConfig.CreateChildPermission(AppPermissions.Pages_Common_EmployeeShift_Delete, L("Delete"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ThienPhucDentalConsts.LocalizationSourceName);
        }
    }
}
