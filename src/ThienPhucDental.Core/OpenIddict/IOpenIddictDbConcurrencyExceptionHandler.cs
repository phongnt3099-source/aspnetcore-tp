using System.Threading.Tasks;
using Abp.Domain.Uow;

namespace ThienPhucDental.OpenIddict
{
    public interface IOpenIddictDbConcurrencyExceptionHandler
    {
        Task HandleAsync(AbpDbConcurrencyException exception);
    }
}