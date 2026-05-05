using System.Threading.Tasks;
using Abp.Application.Services;
using ThienPhucDental.Sessions.Dto;

namespace ThienPhucDental.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
