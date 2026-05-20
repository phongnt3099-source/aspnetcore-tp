using Abp.Application.Services;
using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Consts;
using ThienPhucDental.ProcedureHelpers;

namespace ThienPhucDental.Common
{
    [AbpAuthorize]
    public class ServiceAppService : IServiceAppService
    {
        private readonly IStoreProcedureProvider _storeProcedureProvider;

        public ServiceAppService(IStoreProcedureProvider storeProcedureProvider)
        {
            _storeProcedureProvider = storeProcedureProvider;

        }
        public async Task<List<CM_SERVICES_ENTITY>> CM_SERVICES_GetByType(string Keyword, string ST_ID)
        {
            var item = (await _storeProcedureProvider.GetDataFromStoredProcedure<CM_SERVICES_ENTITY>(CommonStoreProcedureConsts.CM_SERVICES_GETBYTYPE, new
            {
                @p_Keyword = Keyword,
                @p_ST_ID = ST_ID
            }));
            return item;
        }
    }
}
