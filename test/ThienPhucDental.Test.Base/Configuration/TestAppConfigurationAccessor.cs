using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using ThienPhucDental.Configuration;

namespace ThienPhucDental.Test.Base.Configuration
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(ThienPhucDentalTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}
