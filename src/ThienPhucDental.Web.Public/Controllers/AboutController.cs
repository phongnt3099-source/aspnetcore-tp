using Microsoft.AspNetCore.Mvc;
using ThienPhucDental.Web.Controllers;

namespace ThienPhucDental.Web.Public.Controllers
{
    public class AboutController : ThienPhucDentalControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}