using Abp.AutoMapper;
using AbpZeroTemplate.Authorization.Users.Dto;

namespace AbpZeroTemplate.Mobile.MAUI.Models.User
{
    [AutoMapFrom(typeof(CreateOrUpdateUserInput))]
    public class UserCreateOrUpdateModel : CreateOrUpdateUserInput
    {

    }
}
