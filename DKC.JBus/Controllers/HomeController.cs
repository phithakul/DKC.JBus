using DKC.JBus.Infrastructure;
using DKC.JBus.Domains;
using StackExchange.Profiling;
using System.Threading;
using System.Web.Mvc;

namespace DKC.JBus.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}