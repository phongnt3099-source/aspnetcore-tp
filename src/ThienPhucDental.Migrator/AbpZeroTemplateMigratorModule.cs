using Abp.AspNetZeroCore;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;
using ThienPhucDental.Configuration;
using ThienPhucDental.EntityFrameworkCore;
using ThienPhucDental.Migrator.DependencyInjection;

namespace ThienPhucDental.Migrator
{
    [DependsOn(typeof(ThienPhucDentalEntityFrameworkCoreModule))]
    public class ThienPhucDentalMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public ThienPhucDentalMigratorModule(ThienPhucDentalEntityFrameworkCoreModule ThienPhucDentalEntityFrameworkCoreModule)
        {
            ThienPhucDentalEntityFrameworkCoreModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(ThienPhucDentalMigratorModule).GetAssembly().GetDirectoryPathOrNull(),
                addUserSecrets: true
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                ThienPhucDentalConsts.ConnectionStringName
                );
            Configuration.Modules.AspNetZero().LicenseCode = _appConfiguration["AbpZeroLicenseCode"];

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(typeof(IEventBus), () =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                );
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ThienPhucDentalMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}