using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ThienPhucDental.Startup
{
    [DependsOn(typeof(ThienPhucDentalCoreModule))]
    public class ThienPhucDentalGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ThienPhucDentalGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}