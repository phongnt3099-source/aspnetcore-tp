using AbpZeroTemplate.Models.NavigationMenu;

namespace AbpZeroTemplate.Services.Navigation
{
    public interface IMenuProvider
    {
        List<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}