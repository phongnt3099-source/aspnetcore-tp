using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using ThienPhucDental.MultiTenancy.Accounting.Dto;

namespace ThienPhucDental.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
