using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Utils;

namespace ThienPhucDental.Common
{
    public interface ICustomerNoteAppService: IApplicationService
    {
        Task<InsertResult> CM_CUSTOMER_NOTE_Ins(CM_CUSTOMER_NOTE_ENTITY input);
        Task<InsertResult> CM_CUSTOMER_NOTE_Upd(CM_CUSTOMER_NOTE_ENTITY input);
        Task<CommonResult> CM_CUSTOMER_NOTE_Del(string id, string currentUserId);
        Task<List<CM_CUSTOMER_NOTE_ENTITY>> CM_CUSTOMER_NOTE_Get(string cus_id);
    }
}
