using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThienPhucDental.Common.Dto;
//using ThienPhucDental.CoreModule.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThienPhucDental.CoreModule.Utils;
using ThienPhucDental.ProcedureHelpers;

namespace ThienPhucDental.Common
{
    public interface IAllCodeAppService: IApplicationService
    {
        Task<CM_ALLCODE_ENTITY> CM_ALLCODE_GetByCDNAME(string cdName, string cdType, string cdVal);
        Task<List<CM_ALLCODE_ENTITY>> CM_ALLCODE_DROPDOWNLIST(string cdType, string cdName);
        Task<PagedResultDto<CM_ALLCODE_ENTITY>> CM_ALLCODE_Search(CM_ALLCODE_ENTITY input);

        Task<InsertResult> CM_ALLCODE_Ins(CM_ALLCODE_ENTITY input);
        Task<InsertResult> CM_ALLCODE_Upd(CM_ALLCODE_ENTITY input);
        Task<CommonResult> CM_ALLCODE_Del(int id);
    }
}
