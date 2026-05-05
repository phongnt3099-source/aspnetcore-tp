using System.Threading.Tasks;
using Abp.Application.Services;
using ThienPhucDental.MultiTenancy.Payments.PayPal.Dto;

namespace ThienPhucDental.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
