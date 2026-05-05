using System.Collections.Generic;
using ThienPhucDental.Authorization.Permissions.Dto;

namespace ThienPhucDental.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}