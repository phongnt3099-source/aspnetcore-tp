using System.Threading.Tasks;
using ThienPhucDental.Authorization.Users;

namespace ThienPhucDental.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
