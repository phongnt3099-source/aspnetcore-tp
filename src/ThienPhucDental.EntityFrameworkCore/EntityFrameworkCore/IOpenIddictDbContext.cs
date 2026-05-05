using Microsoft.EntityFrameworkCore;
using ThienPhucDental.OpenIddict.Applications;
using ThienPhucDental.OpenIddict.Authorizations;
using ThienPhucDental.OpenIddict.Scopes;
using ThienPhucDental.OpenIddict.Tokens;

namespace ThienPhucDental.EntityFrameworkCore
{
    public interface IOpenIddictDbContext
    {
        DbSet<OpenIddictApplication> Applications { get; }

        DbSet<OpenIddictAuthorization> Authorizations { get; }

        DbSet<OpenIddictScope> Scopes { get; }

        DbSet<OpenIddictToken> Tokens { get; }
    }

}