using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ThienPhucDental.Configure;
using ThienPhucDental.Startup;
using ThienPhucDental.Test.Base;

namespace ThienPhucDental.GraphQL.Tests
{
    [DependsOn(
        typeof(ThienPhucDentalGraphQLModule),
        typeof(ThienPhucDentalTestBaseModule))]
    public class ThienPhucDentalGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ThienPhucDentalGraphQLTestModule).GetAssembly());
        }
    }
}