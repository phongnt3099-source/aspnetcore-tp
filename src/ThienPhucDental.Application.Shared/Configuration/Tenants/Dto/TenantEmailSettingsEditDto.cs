using Abp.Auditing;
using ThienPhucDental.Configuration.Dto;

namespace ThienPhucDental.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}