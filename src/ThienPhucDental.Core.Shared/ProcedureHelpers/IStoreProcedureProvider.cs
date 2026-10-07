using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ThienPhucDental.ProcedureHelpers
{
    public interface IStoreProcedureProvider
    {
        Task<List<TModel>> GetDataFromStoredProcedure<TModel>(string storedProcName, object parameters) where TModel : class;
        Task<PagedResultDto<TModel>> GetPagingData<TModel>(string storedProcName, object parameters) where TModel : class;
        Task<List<dynamic>> GetMultiResultValueFromStore(string storedProcName, object parameters);

    }
}
