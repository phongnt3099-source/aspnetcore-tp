using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Utils;

namespace ThienPhucDental.Common
{
    public interface IShiftAppService: IApplicationService
    {
        Task<List<CM_SHIFT_ENTITY>> CM_SHIFT_Search();
        Task<InsertResult> CM_SHIFT_Ins(CM_SHIFT_ENTITY input);
        Task<InsertResult> CM_SHIFT_Upd(CM_SHIFT_ENTITY input);
        Task<CommonResult> CM_SHIFT_Del(string id);
    }
}
