using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace ThienPhucDental.Web.Public.Views
{
    public abstract class ThienPhucDentalRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected ThienPhucDentalRazorPage()
        {
            LocalizationSourceName = ThienPhucDentalConsts.LocalizationSourceName;
        }
    }
}
