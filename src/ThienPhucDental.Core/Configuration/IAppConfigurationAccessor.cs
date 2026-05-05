using Microsoft.Extensions.Configuration;

namespace ThienPhucDental.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
