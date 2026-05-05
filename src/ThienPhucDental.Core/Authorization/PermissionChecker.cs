using Abp.Authorization;
using ThienPhucDental.Authorization.Roles;
using ThienPhucDental.Authorization.Users;

namespace ThienPhucDental.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
