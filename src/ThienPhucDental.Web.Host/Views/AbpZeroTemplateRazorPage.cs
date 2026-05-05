using Abp.AspNetCore.Mvc.Views;

namespace ThienPhucDental.Web.Views
{
    public abstract class ThienPhucDentalRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected ThienPhucDentalRazorPage()
        {
            LocalizationSourceName = ThienPhucDentalConsts.LocalizationSourceName;
        }
    }
}
