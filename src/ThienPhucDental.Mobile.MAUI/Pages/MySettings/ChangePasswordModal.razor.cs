using Microsoft.AspNetCore.Components;
using AbpZeroTemplate.Authorization.Users.Profile;
using AbpZeroTemplate.Authorization.Users.Profile.Dto;
using AbpZeroTemplate.Core.Dependency;
using AbpZeroTemplate.Core.Threading;
using AbpZeroTemplate.Mobile.MAUI.Models.Settings;
using AbpZeroTemplate.Mobile.MAUI.Shared;

namespace AbpZeroTemplate.Mobile.MAUI.Pages.MySettings
{
    public partial class ChangePasswordModal : ModalBase
    {
        [Parameter] public EventCallback OnSave { get; set; }

        public override string ModalId => "change-password";

        public ChangePasswordModel ChangePasswordModel { get; set; } = new ChangePasswordModel();

        private readonly IProfileAppService _profileAppService;

        public ChangePasswordModal()
        {
            _profileAppService = DependencyResolver.Resolve<IProfileAppService>();
        }

        protected virtual async Task Save()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestExecuter.Execute(async () =>
                {
                    await _profileAppService.ChangePassword(new ChangePasswordInput
                    {
                        CurrentPassword = ChangePasswordModel.CurrentPassword,
                        NewPassword = ChangePasswordModel.NewPassword
                    });

                }, async () =>
                {
                    if (ChangePasswordModel.IsChangePasswordDisabled)
                    {
                        return;
                    }

                    await OnSave.InvokeAsync();
                });
            });
        }

        protected virtual async Task Cancel()
        {
            await Hide();
        }
    }
}
