using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Utils;

namespace ThienPhucDental.Common
{
    public interface IRelationshipAppService : IApplicationService
    {
        Task<CM_RELATIONSHIP_ENTITY> CM_RELATIONSHIP_ById(string Id);
        Task<InsertResult> CM_RELATIONSHIP_SaveSingle(CM_RELATIONSHIP_ENTITY input);
        Task<InsertResult> CM_RELATIONSHIP_UpdateFamilyRole(CM_RELATIONSHIP_ENTITY input);
        Task<CommonResult> CM_RELATIONSHIP_RemoveFromFamily(CM_RELATIONSHIP_ENTITY input); 
    }
}
