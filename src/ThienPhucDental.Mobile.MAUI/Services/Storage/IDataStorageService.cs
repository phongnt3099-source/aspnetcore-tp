using AbpZeroTemplate.ApiClient;
using AbpZeroTemplate.ApiClient.Models;
using AbpZeroTemplate.Sessions.Dto;

namespace AbpZeroTemplate.Services.Storage
{
    public interface IDataStorageService
    {
        Task StoreAccessTokenAsync(string newAccessToken, string newEncryptedAccessToken);

        Task StoreAuthenticateResultAsync(AbpAuthenticateResultModel authenticateResultModel);

        AbpAuthenticateResultModel RetrieveAuthenticateResult();

        TenantInformation RetrieveTenantInfo();

        GetCurrentLoginInformationsOutput RetrieveLoginInfo();

        void ClearSessionPersistance();

        Task StoreLoginInformationAsync(GetCurrentLoginInformationsOutput loginInfo);

        Task StoreTenantInfoAsync(TenantInformation tenantInfo);
    }
}
