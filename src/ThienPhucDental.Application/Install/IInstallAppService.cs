using System.Threading.Tasks;
using Abp.Application.Services;
using ThienPhucDental.Install.Dto;

namespace ThienPhucDental.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}