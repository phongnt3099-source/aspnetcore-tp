using Abp.Domain.Services;

namespace ThienPhucDental
{
    public abstract class ThienPhucDentalDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected ThienPhucDentalDomainServiceBase()
        {
            LocalizationSourceName = ThienPhucDentalConsts.LocalizationSourceName;
        }
    }
}
