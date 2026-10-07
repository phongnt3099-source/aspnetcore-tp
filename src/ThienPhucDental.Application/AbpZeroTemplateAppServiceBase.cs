using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Dependency;
using Abp.IdentityFramework;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ThienPhucDental.Authorization.Users;
using ThienPhucDental.MultiTenancy;
using ThienPhucDental.ProcedureHelpers;

namespace ThienPhucDental
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class ThienPhucDentalAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected IStoreProcedureProvider storeProcedureProvider;

        private readonly IHttpContextAccessor httpContextAccessor;


        protected ThienPhucDentalAppServiceBase()
        {
            LocalizationSourceName = ThienPhucDentalConsts.LocalizationSourceName;
            storeProcedureProvider = IocManager.Instance.Resolve<IStoreProcedureProvider>();
            httpContextAccessor = IocManager.Instance.Resolve<IHttpContextAccessor>();
        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual User GetCurrentUser()
        {
            return AsyncHelper.RunSync(GetCurrentUserAsync);
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
            }
        }

        protected virtual Tenant GetCurrentTenant()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return TenantManager.GetById(AbpSession.GetTenantId());
            }
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

        protected string GetCurrentUserName()
        {
            return httpContextAccessor.HttpContext?.User?.Identity?.Name;
        }
    }
}