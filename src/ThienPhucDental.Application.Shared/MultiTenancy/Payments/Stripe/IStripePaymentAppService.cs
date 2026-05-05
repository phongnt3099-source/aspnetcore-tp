using System.Threading.Tasks;
using Abp.Application.Services;
using ThienPhucDental.MultiTenancy.Payments.Dto;
using ThienPhucDental.MultiTenancy.Payments.Stripe.Dto;

namespace ThienPhucDental.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();
        
        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}