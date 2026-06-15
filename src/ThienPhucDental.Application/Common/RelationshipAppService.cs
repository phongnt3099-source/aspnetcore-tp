using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Consts;
using ThienPhucDental.CoreModule.Utils;
using ThienPhucDental.ProcedureHelpers;

namespace ThienPhucDental.Common
{
    [AbpAuthorize]
    public class RelationshipAppService : IRelationshipAppService
    {
        private readonly IStoreProcedureProvider _storeProcedureProvider;
        public RelationshipAppService(IStoreProcedureProvider storeProcedureProvider)
        {
            _storeProcedureProvider = storeProcedureProvider;

        }

        public async Task<InsertResult> CM_RELATIONSHIP_UpdateFamilyRole(CM_RELATIONSHIP_ENTITY input)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_RELATIONSHIP_UPDATEFAMILYROLE, input)).FirstOrDefault();
            return result;
        }

        public async Task<InsertResult> CM_RELATIONSHIP_SaveSingle(CM_RELATIONSHIP_ENTITY input)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_RELATIONSHIP_SAVESINGLE, input)).FirstOrDefault();
            return result;
        }

        public async Task<CM_RELATIONSHIP_ENTITY> CM_RELATIONSHIP_ById(string Id)
        {
            var result = (await _storeProcedureProvider.GetDataFromStoredProcedure<CM_RELATIONSHIP_ENTITY>(CommonStoreProcedureConsts.CM_RELATIONSHIP_BYID, new
            {
                P_CUS_ID = Id
            })).FirstOrDefault();

            return result;
        }

        public async Task<CommonResult> CM_RELATIONSHIP_RemoveFromFamily(CM_RELATIONSHIP_ENTITY input)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_RELATIONSHIP_REMOVEFROMFAMILY, input )).FirstOrDefault();
            return result;
        }
    }
}
