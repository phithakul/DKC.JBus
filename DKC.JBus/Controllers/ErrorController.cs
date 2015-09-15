using DKC.JBus.Helpers;
using System.Web.Mvc;

namespace DKC.JBus.Controllers
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