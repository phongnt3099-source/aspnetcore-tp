using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Utils;

namespace ThienPhucDental.Common
{
    public interface IHolidayAppService: IApplicationService
    {
        Task<List<CM_HOLIDAY_ENTITY>> CM_HOLIDAY_Search();
        Task<InsertResult> CM_HOLIDAY_Ins(CM_HOLIDAY_ENTITY input);
        Task<InsertResult> CM_HOLIDAY_Upd(CM_HOLIDAY_ENTITY input);
        Task<CommonResult> CM_HOLIDAY_Del(string id);
    }
}
