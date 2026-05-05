using System.Threading.Tasks;
using Abp.Webhooks;

namespace ThienPhucDental.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
