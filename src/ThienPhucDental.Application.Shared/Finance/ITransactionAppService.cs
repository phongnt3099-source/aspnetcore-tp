using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThienPhucDental.CoreModule.Utils;
using ThienPhucDental.Finance.Dto;

namespace ThienPhucDental.Finance
{
    public interface ITransactionAppService : IApplicationService
    {
        Task<PagedResultDto<FIN_TRANSACTION_ENTITY>> FIN_TRANSACTION_Search(FIN_TRANSACTION_ENTITY input);
        Task<FIN_TRANSACTION_ENTITY> FIN_TRANSACTION_GetById(string Id);
        Task<InsertResult> FIN_TRANSACTION_Ins(FIN_TRANSACTION_ENTITY input);
        Task<InsertResult> FIN_TRANSACTION_Upd(FIN_TRANSACTION_ENTITY input);
        Task<CommonResult> FIN_TRANSACTION_Del(string id, string maker_id);
        Task<FinancialDashboardDto> FIN_TRANSACTION_GetDashboard(FIN_TRANSACTION_ENTITY input);
        Task<List<FIN_TRANSACTION_ENTITY>> FIN_TRANSACTION_GetLogByExm(string Id);
    }
}
