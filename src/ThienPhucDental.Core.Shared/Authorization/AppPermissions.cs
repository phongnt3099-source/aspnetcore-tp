namespace ThienPhucDental.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class AppPermissions
    {
        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string Pages = "Pages";

        //public const string Pages_DemoUiComponents = "Pages.DemoUiComponents";
        public const string Pages_Administration = "Pages.Administration";

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";
        public const string Pages_Administration_Users_Impersonation = "Pages.Administration.Users.Impersonation";
        public const string Pages_Administration_Users_Unlock = "Pages.Administration.Users.Unlock";
        public const string Pages_Administration_Users_ChangeProfilePicture = "Pages.Administration.Users.ChangeProfilePicture";

        public const string Pages_Administration_Languages = "Pages.Administration.Languages";
        public const string Pages_Administration_Languages_Create = "Pages.Administration.Languages.Create";
        public const string Pages_Administration_Languages_Edit = "Pages.Administration.Languages.Edit";
        public const string Pages_Administration_Languages_Delete = "Pages.Administration.Languages.Delete";
        public const string Pages_Administration_Languages_ChangeTexts = "Pages.Administration.Languages.ChangeTexts";
        public const string Pages_Administration_Languages_ChangeDefaultLanguage = "Pages.Administration.Languages.ChangeDefaultLanguage";

        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";

        public const string Pages_Administration_OrganizationUnits = "Pages.Administration.OrganizationUnits";
        public const string Pages_Administration_OrganizationUnits_ManageOrganizationTree = "Pages.Administration.OrganizationUnits.ManageOrganizationTree";
        public const string Pages_Administration_OrganizationUnits_ManageMembers = "Pages.Administration.OrganizationUnits.ManageMembers";
        public const string Pages_Administration_OrganizationUnits_ManageRoles = "Pages.Administration.OrganizationUnits.ManageRoles";

        public const string Pages_Administration_HangfireDashboard = "Pages.Administration.HangfireDashboard";

        public const string Pages_Administration_UiCustomization = "Pages.Administration.UiCustomization";

        public const string Pages_Administration_WebhookSubscription = "Pages.Administration.WebhookSubscription";
        public const string Pages_Administration_WebhookSubscription_Create = "Pages.Administration.WebhookSubscription.Create";
        public const string Pages_Administration_WebhookSubscription_Edit = "Pages.Administration.WebhookSubscription.Edit";
        public const string Pages_Administration_WebhookSubscription_ChangeActivity = "Pages.Administration.WebhookSubscription.ChangeActivity";
        public const string Pages_Administration_WebhookSubscription_Detail = "Pages.Administration.WebhookSubscription.Detail";
        public const string Pages_Administration_Webhook_ListSendAttempts = "Pages.Administration.Webhook.ListSendAttempts";
        public const string Pages_Administration_Webhook_ResendWebhook = "Pages.Administration.Webhook.ResendWebhook";

        public const string Pages_Administration_DynamicProperties = "Pages.Administration.DynamicProperties";
        public const string Pages_Administration_DynamicProperties_Create = "Pages.Administration.DynamicProperties.Create";
        public const string Pages_Administration_DynamicProperties_Edit = "Pages.Administration.DynamicProperties.Edit";
        public const string Pages_Administration_DynamicProperties_Delete = "Pages.Administration.DynamicProperties.Delete";

        public const string Pages_Administration_DynamicPropertyValue = "Pages.Administration.DynamicPropertyValue";
        public const string Pages_Administration_DynamicPropertyValue_Create = "Pages.Administration.DynamicPropertyValue.Create";
        public const string Pages_Administration_DynamicPropertyValue_Edit = "Pages.Administration.DynamicPropertyValue.Edit";
        public const string Pages_Administration_DynamicPropertyValue_Delete = "Pages.Administration.DynamicPropertyValue.Delete";

        public const string Pages_Administration_DynamicEntityProperties = "Pages.Administration.DynamicEntityProperties";
        public const string Pages_Administration_DynamicEntityProperties_Create = "Pages.Administration.DynamicEntityProperties.Create";
        public const string Pages_Administration_DynamicEntityProperties_Edit = "Pages.Administration.DynamicEntityProperties.Edit";
        public const string Pages_Administration_DynamicEntityProperties_Delete = "Pages.Administration.DynamicEntityProperties.Delete";

        public const string Pages_Administration_DynamicEntityPropertyValue = "Pages.Administration.DynamicEntityPropertyValue";
        public const string Pages_Administration_DynamicEntityPropertyValue_Create = "Pages.Administration.DynamicEntityPropertyValue.Create";
        public const string Pages_Administration_DynamicEntityPropertyValue_Edit = "Pages.Administration.DynamicEntityPropertyValue.Edit";
        public const string Pages_Administration_DynamicEntityPropertyValue_Delete = "Pages.Administration.DynamicEntityPropertyValue.Delete";
        
        public const string Pages_Administration_MassNotification = "Pages.Administration.MassNotification";
        public const string Pages_Administration_MassNotification_Create = "Pages.Administration.MassNotification.Create";
        
        public const string Pages_Administration_NewVersion_Create = "Pages_Administration_NewVersion_Create";
        
        //TENANT-SPECIFIC PERMISSIONS

        public const string Pages_Tenant_Dashboard = "Pages.Tenant.Dashboard";

        public const string Pages_Administration_Tenant_Settings = "Pages.Administration.Tenant.Settings";

        public const string Pages_Administration_Tenant_SubscriptionManagement = "Pages.Administration.Tenant.SubscriptionManagement";

        //HOST-SPECIFIC PERMISSIONS

        public const string Pages_Editions = "Pages.Editions";
        public const string Pages_Editions_Create = "Pages.Editions.Create";
        public const string Pages_Editions_Edit = "Pages.Editions.Edit";
        public const string Pages_Editions_Delete = "Pages.Editions.Delete";
        public const string Pages_Editions_MoveTenantsToAnotherEdition = "Pages.Editions.MoveTenantsToAnotherEdition";

        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Tenants_Create = "Pages.Tenants.Create";
        public const string Pages_Tenants_Edit = "Pages.Tenants.Edit";
        public const string Pages_Tenants_ChangeFeatures = "Pages.Tenants.ChangeFeatures";
        public const string Pages_Tenants_Delete = "Pages.Tenants.Delete";
        public const string Pages_Tenants_Impersonation = "Pages.Tenants.Impersonation";

        public const string Pages_Administration_Host_Maintenance = "Pages.Administration.Host.Maintenance";
        public const string Pages_Administration_Host_Settings = "Pages.Administration.Host.Settings";
        public const string Pages_Administration_Host_Dashboard = "Pages.Administration.Host.Dashboard";


        public const string Pages_Common_AllCode = "Pages.Common.AllCode";
        public const string Pages_Common_AllCode_Create = "Pages.Common.AllCode.Create";
        public const string Pages_Common_AllCode_Update = "Pages.Common.AllCode.Update";
        public const string Pages_Common_AllCode_Delete = "Pages.Common.AllCode.Delete";

        public const string Pages_Common_Customer = "Pages.Common.Customer";
        public const string Pages_Common_Customer_Create = "Pages.Common.Customer.Create";
        public const string Pages_Common_Customer_Update = "Pages.Common.Customer.Update";
        public const string Pages_Common_Customer_Delete = "Pages.Common.Customer.Delete";
        public const string Pages_Common_Customer_ViewDetail = "Pages.Common.Customer.ViewDetail";
        public const string Pages_Common_Customer_Print = "Pages.Common.Customer.Print";

        public const string Pages_Common_Employee = "Pages.Common.Employee";
        public const string Pages_Common_Employee_Create = "Pages.Common.Employee.Create";
        public const string Pages_Common_Employee_Update = "Pages.Common.Employee.Update";
        public const string Pages_Common_Employee_Delete = "Pages.Common.Employee.Delete";

        public const string Pages_Common_Service = "Pages.Common.Service";
        public const string Pages_Common_Service_Create = "Pages.Common.Service.Create";
        public const string Pages_Common_Service_Update = "Pages.Common.Service.Update";
        public const string Pages_Common_Service_Delete = "Pages.Common.Service.Delete";

        public const string Pages_Common_ServiceType = "Pages.Common.ServiceType";
        public const string Pages_Common_ServiceType_Create = "Pages.Common.ServiceType.Create";
        public const string Pages_Common_ServiceType_Update = "Pages.Common.ServiceType.Update";
        public const string Pages_Common_ServiceType_Delete = "Pages.Common.ServiceType.Delete";

        public const string Pages_Medical_Appointment = "Pages.Medical.Appointment";
        public const string Pages_Medical_Appointment_Create = "Pages.Medical.Appointment.Create";
        public const string Pages_Medical_Appointment_Update = "Pages.Medical.Appointment.Update";
        public const string Pages_Medical_Appointment_Delete = "Pages.Medical.Appointment.Delete";

        public const string Pages_Medical_Examination = "Pages.Medical.Examination";
        public const string Pages_Medical_Examination_Create = "Pages.Medical.Examination.Create";
        public const string Pages_Medical_Examination_Update = "Pages.Medical.Examination.Update";
        public const string Pages_Medical_Examination_Delete = "Pages.Medical.Examination.Delete";
        public const string Pages_Medical_Examination_Print = "Pages.Medical.Examination.Print";

        public const string Pages_Finance_Transaction = "Pages.Finance.Transaction";
        public const string Pages_Finance_Transaction_Create = "Pages.Finance.Transaction.Create";
        public const string Pages_Finance_Transaction_Print = "Pages.Finance.Transaction.Print";
        public const string Pages_Finance_Transaction_Update = "Pages.Finance.Transaction.Edit";
        public const string Pages_Finance_Transaction_AdminBypass = "Pages.Finance.Transaction.AdminBypass";
        public const string Pages_Finance_Transaction_Delete = "Pages.Common.Transaction.Delete";

        // --- HOLIDAY ---
        public const string Pages_Common_Holiday = "Pages.Common.Holiday";
        public const string Pages_Common_Holiday_Create = "Pages.Common.Holiday.Create";
        public const string Pages_Common_Holiday_Update = "Pages.Common.Holiday.Update";
        public const string Pages_Common_Holiday_Delete = "Pages.Common.Holiday.Delete";

        // --- CLINIC NETWORK ---
        public const string Pages_Common_ClinicNetwork = "Pages.Common.ClinicNetwork";
        public const string Pages_Common_ClinicNetwork_Create = "Pages.Common.ClinicNetwork.Create";
        public const string Pages_Common_ClinicNetwork_Update = "Pages.Common.ClinicNetwork.Update";
        public const string Pages_Common_ClinicNetwork_Delete = "Pages.Common.ClinicNetwork.Delete";

        // --- SHIFT ---
        public const string Pages_Common_Shift = "Pages.Common.Shift";
        public const string Pages_Common_Shift_Create = "Pages.Common.Shift.Create";
        public const string Pages_Common_Shift_Update = "Pages.Common.Shift.Update";
        public const string Pages_Common_Shift_Delete = "Pages.Common.Shift.Delete";

        // --- EMPLOYEE SHIFT (PHÂN CÔNG CA NHÂN SỰ) ---
        public const string Pages_Common_EmployeeShift = "Pages.Common.EmployeeShift";
        public const string Pages_Common_EmployeeShift_Create = "Pages.Common.EmployeeShift.Create";
        public const string Pages_Common_EmployeeShift_Update = "Pages.Common.EmployeeShift.Update";
        public const string Pages_Common_EmployeeShift_Delete = "Pages.Common.EmployeeShift.Delete";
    }
}
