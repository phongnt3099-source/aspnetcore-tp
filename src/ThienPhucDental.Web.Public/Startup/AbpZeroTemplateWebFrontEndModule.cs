using Abp.AspNetZeroCore;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ThienPhucDental.Configuration;
using ThienPhucDental.EntityFrameworkCore;

namespace ThienPhucDental.Web.Public.Startup
{
    [DependsOn(
        typeof(ThienPhucDentalWebCoreModule)
    )]
    public class ThienPhucDentalWebFrontEndModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public ThienPhucDentalWebFrontEndModule(IWebHostEnvironment env, ThienPhucDentalEntityFrameworkCoreModule ThienPhucDentalEntityFrameworkCoreModule)
        {
            _appConfiguration = env.GetAppConfiguration();
            ThienPhucDentalEntityFrameworkCoreModule.SkipDbSeed = true;
        }

        public override void PreInitialize()
        {
            Configuration.Modules.AbpWebCommon().MultiTenancy.DomainFormat = _appConfiguration["App:WebSiteRootAddress"] ?? "https://localhost:44303/";
            Configuration.Modules.AspNetZero().LicenseCode = _appConfiguration["AbpZeroLicenseCode"];

            //Changed AntiForgery token/cookie names to not conflict to the main application while redirections.
            Configuration.Modules.AbpWebCommon().AntiForgery.TokenCookieName = "Public-XSRF-TOKEN";
            Configuration.Modules.AbpWebCommon().AntiForgery.TokenHeaderName = "Public-X-XSRF-TOKEN";

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            Configuration.Navigation.Providers.Add<FrontEndNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ThienPhucDentalWebFrontEndModule).GetAssembly());
        }
    }
}
