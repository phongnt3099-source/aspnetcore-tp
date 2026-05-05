using ThienPhucDental.MultiTenancy.Payments.Stripe;

namespace ThienPhucDental.Web.Controllers
{
    public class StripeController : StripeControllerBase
    {
        public StripeController(
            StripeGatewayManager stripeGatewayManager,
            StripePaymentGatewayConfiguration stripeConfiguration) 
            : base(stripeGatewayManager, stripeConfiguration)
        {
        }
    }
}
