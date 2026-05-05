using Abp.AspNetCore.Mvc.ViewComponents;

namespace ThienPhucDental.Web.Public.Views
{
    public abstract class ThienPhucDentalViewComponent : AbpViewComponent
    {
        protected ThienPhucDentalViewComponent()
        {
            LocalizationSourceName = ThienPhucDentalConsts.LocalizationSourceName;
        }
    }
}