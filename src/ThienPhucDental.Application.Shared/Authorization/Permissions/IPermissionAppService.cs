using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ThienPhucDental.Authorization.Permissions.Dto;

namespace ThienPhucDental.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
