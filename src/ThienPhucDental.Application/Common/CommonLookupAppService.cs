using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using ThienPhucDental.Authorization;
using ThienPhucDental.Common.Dto;
using ThienPhucDental.CoreModule.Utils;
using ThienPhucDental.Editions;
using ThienPhucDental.Editions.Dto; 
using ThienPhucDental.CoreModule.Consts;
using ThienPhucDental.ProcedureHelpers;
using Abp.UI;
using System;

namespace ThienPhucDental.Common
{
    [AbpAuthorize]
    public class CommonLookupAppService : ThienPhucDentalAppServiceBase, ICommonLookupAppService, IAllCodeAppService
    {

        //protected IStoreProcedureProvider storeProcedureProvider;
        private readonly IStoreProcedureProvider _storeProcedureProvider; 
        private readonly EditionManager _editionManager;    

        public CommonLookupAppService(EditionManager editionManager, IStoreProcedureProvider storeProcedureProvider)
        {
            _editionManager = editionManager;
            _storeProcedureProvider = storeProcedureProvider;
            //storeProcedureProvider = IocManager.Instance.Resolve<IStoreProcedureProvider>();

        }

        public async Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false)
        {
            var subscribableEditions = (await _editionManager.Editions.Cast<SubscribableEdition>().ToListAsync())
                .WhereIf(onlyFreeItems, e => e.IsFree)
                .OrderBy(e => e.MonthlyPrice);

            return new ListResultDto<SubscribableEditionComboboxItemDto>(
                subscribableEditions.Select(e => new SubscribableEditionComboboxItemDto(e.Id.ToString(), e.DisplayName, e.IsFree)).ToList()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users)]
        public async Task<PagedResultDto<FindUsersOutputDto>> FindUsers(FindUsersInput input)
        {
            if (AbpSession.TenantId != null)
            {
                //Prevent tenants to get other tenant's users.
                input.TenantId = AbpSession.TenantId;
            }

            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var query = UserManager.Users
                    .WhereIf(
                        !input.Filter.IsNullOrWhiteSpace(),
                        u =>
                            u.Name.Contains(input.Filter) ||
                            u.Surname.Contains(input.Filter) ||
                            u.UserName.Contains(input.Filter) ||
                            u.EmailAddress.Contains(input.Filter)
                    ).WhereIf(input.ExcludeCurrentUser, u => u.Id != AbpSession.GetUserId());

                var userCount = await query.CountAsync();
                var users = await query
                    .OrderBy(u => u.Name)
                    .ThenBy(u => u.Surname)
                    .PageBy(input)
                    .ToListAsync();
                
                return new PagedResultDto<FindUsersOutputDto>(userCount, ObjectMapper.Map<List<FindUsersOutputDto>>(users));
            }
        }

        public GetDefaultEditionNameOutput GetDefaultEditionName()
        {
            return new GetDefaultEditionNameOutput
            {
                Name = EditionManager.DefaultEditionName
            };
        }


        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode)]
        public async Task<CM_ALLCODE_ENTITY> CM_ALLCODE_GetByCDNAME(string cdType, string cdName, string cdVal)
        {
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_ALLCODE_ENTITY>(CommonStoreProcedureConsts.CM_ALLCODE_BYID, new
            {
                P_CDNAME = cdName,
                P_CDTYPE = cdType,
                P_CDVAL = cdVal
            })).FirstOrDefault();
            return result;  
        }
        public async Task<List<CM_ALLCODE_ENTITY>> CM_ALLCODE_DROPDOWNLIST(string cdType, string cdName)
        {
            var result = await _storeProcedureProvider
                .GetDataFromStoredProcedure<CM_ALLCODE_ENTITY>(CommonStoreProcedureConsts.CM_ALLCODE_DROPDOWNLIST, new
                {
                    P_CDNAME = cdName,
                    P_CDTYPE = cdType,
                });

            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode)]
        public async Task<PagedResultDto<CM_ALLCODE_ENTITY>> CM_ALLCODE_Search(CM_ALLCODE_ENTITY input)
        {
            var result = await _storeProcedureProvider.GetPagingData<CM_ALLCODE_ENTITY>(CommonStoreProcedureConsts.CM_ALLCODE_SEARCH, input);
            return result;
        }

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode_Create)]
        public async Task<InsertResult> CM_ALLCODE_Ins(CM_ALLCODE_ENTITY input)
        {
            try
            {
                if (input == null)
                    throw new UserFriendlyException("Dữ liệu đầu vào không được để trống!");

                if (_storeProcedureProvider == null)
                    throw new UserFriendlyException("_storeProcedureProvider chưa được inject!");

                Logger.Info($"CM_ALLCODE_Ins called - Code: {input.CDNAME}, Name: {input.CDTYPE ?? "null"}");

                var resultList = await _storeProcedureProvider
                    .GetDataFromStoredProcedure<InsertResult>(
                        CommonStoreProcedureConsts.CM_ALLCODE_INS,
                        input);

                var result = resultList?.FirstOrDefault();

                if (result == null)
                    return new InsertResult { Id = "-1", Ids = "Không có kết quả trả về" };

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                throw new UserFriendlyException("Lỗi khi thêm CM_ALLCODE: " + ex.Message, ex);
            }
            //var result = (await storeProcedureProvider
            //    .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_ALLCODE_INS, input)).FirstOrDefault();
        }   

        //[AbpAuthorize(AppPermissions.Pages_Common_AllCode_Update)]
        public async Task<InsertResult> CM_ALLCODE_Upd(CM_ALLCODE_ENTITY input)
        {
            return (await _storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_ALLCODE_UPD, input)).FirstOrDefault();
        }

       // [AbpAuthorize(AppPermissions.Pages_Common_AllCode_Delete)]
        public async Task<CommonResult> CM_ALLCODE_Del(int id)      
        {
            var result =(await _storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_ALLCODE_DEL, new
                {
                    ALL_CODE_ID = id
                })).FirstOrDefault();
            return result;
        }
        public async Task<List<CM_ROLES_ENTITY>> CM_ROLES_DROPDOWNLIST()
        {
            var result = await _storeProcedureProvider
                .GetDataFromStoredProcedure<CM_ROLES_ENTITY>(CommonStoreProcedureConsts.CM_ROLES_DROPDOWNLIST,new { });

            return result;
        }
    }
}
