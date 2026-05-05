using System.Threading.Tasks;
using Abp.Application.Services;
using ThienPhucDental.Configuration.Host.Dto;

namespace ThienPhucDental.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
