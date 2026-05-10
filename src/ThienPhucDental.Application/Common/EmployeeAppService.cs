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
    public class EmployeeAppService : IEmployeeAppService
    {
        private readonly IStoreProcedureProvider _storeProcedureProvider;
        public EmployeeAppService(IStoreProcedureProvider storeProcedureProvider)
        {
            _storeProcedureProvider = storeProcedureProvider;

        }

        public async Task<List<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_DROPDOWNLIST(string EMP_ROLE)
        {
            var result = await _storeProcedureProvider
                .GetDataFromStoredProcedure<CM_EMPLOYEE_ENTITY>(CommonStoreProcedureConsts.CM_EMPLOYEE_DROPDOWNLIST, new
                {
                    P_EMP_ROLE = EMP_ROLE
                });

            return result;
        }
    }
}
