namespace ThienPhucDental.Url
{
    public interface IAppUrlService
    {
        string CreateEmailActivationUrlFormat(int? tenantId);
        
        string CreateEmailChangeRequestUrlFormat(int? tenantId);

        string CreatePasswordResetUrlFormat(int? tenantId);

        string CreateEmailActivationUrlFormat(string tenancyName);


        string CreatePasswordResetUrlFormat(string tenancyName);

        string CreateFaceRegistrationUrlFormat(int? tenantId);

        string CreateFaceRegistrationUrlFormat(string tenancyName);
    }
}
