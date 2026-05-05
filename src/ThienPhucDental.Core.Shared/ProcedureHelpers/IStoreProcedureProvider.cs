using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ThienPhucDental.ProcedureHelpers
{
    public interface IStoreProcedureProvider
    {
        string ConnectionString { get; set; }
        Task<List<TModel>> GetDataFromStoredProcedure<TModel>(string storedProcName, object parameters) where TModel : class;
        Task<PagedResultDto<TModel>> GetPagingData<TModel>(string storedProcName, object parameters) where TModel : class;

    }
}
