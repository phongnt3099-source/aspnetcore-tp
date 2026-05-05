using Microsoft.AspNetCore.Antiforgery;

namespace ThienPhucDental.Web.Controllers
{
    public class AntiForgeryController : ThienPhucDentalControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
