using System.Threading.Tasks;
using ThienPhucDental.Sessions.Dto;

namespace ThienPhucDental.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
