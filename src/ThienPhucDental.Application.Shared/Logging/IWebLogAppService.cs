using Abp.Application.Services;
using ThienPhucDental.Dto;
using ThienPhucDental.Logging.Dto;

namespace ThienPhucDental.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
