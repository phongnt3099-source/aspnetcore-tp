using System.Threading.Tasks;
using Abp.Application.Services;
using ThienPhucDental.MultiTenancy.Dto;
using ThienPhucDental.MultiTenancy.Payments.Dto;

namespace ThienPhucDental.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
        
        Task<long> StartExtendSubscription(StartExtendSubscriptionInput input);
        
        Task<StartUpgradeSubscriptionOutput> StartUpgradeSubscription(StartUpgradeSubscriptionInput input);
        
        Task<long> StartTrialToBuySubscription(StartTrialToBuySubscriptionInput input);
    }
}
