using Abp.AspNetCore.Mvc.Authorization;
using ThienPhucDental.Authorization.Users.Profile;
using ThienPhucDental.Graphics;
using ThienPhucDental.Storage;

namespace ThienPhucDental.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService,
            IImageValidator imageValidator) :
            base(tempFileCacheManager, profileAppService, imageValidator)
        {
        }
    }
}