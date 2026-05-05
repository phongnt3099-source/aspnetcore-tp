using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ThienPhucDental
{
    public class ThienPhucDentalCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ThienPhucDentalCoreSharedModule).GetAssembly());
        }
    }
}