using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ThienPhucDental
{
    [DependsOn(typeof(ThienPhucDentalCoreSharedModule))]
    public class ThienPhucDentalApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ThienPhucDentalApplicationSharedModule).GetAssembly());
        }
    }
}