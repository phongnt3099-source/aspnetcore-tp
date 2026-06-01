using Abp.Application.Services.Dto;
using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ThienPhucDental.CoreModule.Consts;
using ThienPhucDental.CoreModule.Utils;
using ThienPhucDental.Finance.Dto;
using ThienPhucDental.Medical.Dto;
using ThienPhucDental.ProcedureHelpers;

namespace ThienPhucDental.Finance
{
    [AbpAuthorize]
    public class TransactionAppService : ITransactionAppService
    {
        private readonly IStoreProcedureProvider _storeProcedureProvider;
        public TransactionAppService(IStoreProcedureProvider storeProcedureProvider)
        {
            _storeProcedureProvider = storeProcedureProvider;

        }
        public async Task<FIN_TRANSACTION_ENTITY> FIN_TRANSACTION_GetById(string Id)
        {
            var result = (await _storeProcedureProvider.GetDataFromStoredProcedure<FIN_TRANSACTION_ENTITY>(CommonStoreProcedureConsts.FIN_TRANSACTION_BYID, new
            {
                P_FT_ID = Id
            })).FirstOrDefault();

            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode)]
        public async Task<PagedResultDto<FIN_TRANSACTION_ENTITY>> FIN_TRANSACTION_Search(FIN_TRANSACTION_ENTITY input)
        {
            var result = await _storeProcedureProvider.GetPagingData<FIN_TRANSACTION_ENTITY>(CommonStoreProcedureConsts.FIN_TRANSACTION_SEARCH, input);

            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode_Create)]
        public async Task<InsertResult> FIN_TRANSACTION_Ins(FIN_TRANSACTION_ENTITY input)
        {
            
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.FIN_TRANSACTION_INS, input)).FirstOrDefault();
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode_Update)]
        public async Task<InsertResult> FIN_TRANSACTION_Upd(FIN_TRANSACTION_ENTITY input)
        {
           
            return (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.FIN_TRANSACTION_UPD, input)).FirstOrDefault();
        }

        // [AbpAuthorize(AppPermissions.Pages_Common_AllCode_Delete)]
        public async Task<CommonResult> FIN_TRANSACTION_Del(string id, string maker_id)
        {
            var result = (await _storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.FIN_TRANSACTION_DEL, new
                {
                    P_FT_ID = id,
                    P_MAKER_ID = maker_id
                })).FirstOrDefault();
            return result;
        }
        
        public async Task<List<FIN_TRANSACTION_ENTITY>> FIN_TRANSACTION_GetLogByExm (string Id)
        {
            var result = (await _storeProcedureProvider.GetDataFromStoredProcedure<FIN_TRANSACTION_ENTITY>(CommonStoreProcedureConsts.FIN_TRANSACTION_GETLOGBYEXM, new
            {
                P_FT_EXM_ID = Id
            }));

            return result;
        }
    }
}
