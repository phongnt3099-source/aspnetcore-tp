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
    public class CustomerNoteAppService: ICustomerNoteAppService
    {
        private readonly IStoreProcedureProvider _storeProcedureProvider;

        public CustomerNoteAppService(IStoreProcedureProvider storeProcedureProvider)
        {
            _storeProcedureProvider = storeProcedureProvider;

        }

        public async Task<InsertResult> CM_CUSTOMER_NOTE_Ins(CM_CUSTOMER_NOTE_ENTITY input)
        {
            
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_CUSTOMER_NOTE_INS, input)).FirstOrDefault();
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode_Update)]
        public async Task<InsertResult> CM_CUSTOMER_NOTE_Upd(CM_CUSTOMER_NOTE_ENTITY input)
        {
            return (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_CUSTOMER_NOTE_UPD, input)).FirstOrDefault();
        }

        // [AbpAuthorize(AppPermissions.Pages_Common_AllCode_Delete)]
        public async Task<CommonResult> CM_CUSTOMER_NOTE_Del(string id, string currentUserId)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_CUSTOMER_NOTE_DEL, new
                {
                    P_NOTE_ID = id,
                    P_UPDATE_ID = currentUserId
                })).FirstOrDefault();
            return result;
        }
        public async Task<List<CM_CUSTOMER_NOTE_ENTITY>> CM_CUSTOMER_NOTE_Get(string cus_id)
        {
            var result = await _storeProcedureProvider
                .GetDataFromStoredProcedure<CM_CUSTOMER_NOTE_ENTITY>(CommonStoreProcedureConsts.CM_CUSTOMER_NOTE_GET, new
                {
                    P_CUS_ID = cus_id
                });

            return result;
        }
    }
}
