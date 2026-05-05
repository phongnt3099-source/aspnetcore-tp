using System.Threading.Tasks;
using Abp.Dependency;

namespace ThienPhucDental.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}