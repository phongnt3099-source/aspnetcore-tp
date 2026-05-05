using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using ThienPhucDental.Dto;

namespace ThienPhucDental.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
