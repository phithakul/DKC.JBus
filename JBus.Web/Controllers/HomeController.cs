using JBus.Web.Infrastructure;
using JBus.Web.Models;
using StackExchange.Profiling;
using System.Threading;
using System.Web.Mvc;

namespace JBus.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}