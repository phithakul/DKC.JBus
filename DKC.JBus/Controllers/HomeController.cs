using DKC.JBus.Infrastructure;
using DKC.JBus.Models;
using System.Web.Mvc;

namespace DKC.JBus.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}