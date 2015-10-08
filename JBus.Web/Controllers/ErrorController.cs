using JBus.Web.Helpers;
using System.Web.Mvc;

namespace JBus.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : BaseController
    {
        public ActionResult ErrorPage()
        {
            return View("Error");
        }

        public new ActionResult PageNotFound()
        {
            return base.PageNotFound();
        }
    }
}