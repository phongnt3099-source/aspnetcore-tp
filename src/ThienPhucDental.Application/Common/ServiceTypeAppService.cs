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
    public class ServiceTypeAppService: IServiceTypeAppService
    {
        private readonly IStoreProcedureProvider _storeProcedureProvider;

        public ServiceTypeAppService(IStoreProcedureProvider storeProcedureProvider)
        {
            _storeProcedureProvider = storeProcedureProvider;

        }
        public async Task<List<CM_SERVICE_TYPE_ENTITY>> CM_SERVICES_GetAll()
        {
            var item = (await _storeProcedureProvider.GetDataFromStoredProcedure<CM_SERVICE_TYPE_ENTITY>(CommonStoreProcedureConsts.CM_SERVICE_TYPE_GETALL, new { }));
            return item;
        }
    }
}
