using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using ThienPhucDental.Authorization.Users;
using ThienPhucDental.MultiTenancy;

namespace ThienPhucDental.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}