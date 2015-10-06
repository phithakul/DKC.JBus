using DKC.JBus.Helpers;
using DKC.JBus.Infrastructure;
using DKC.JBus.Domains;
using DKC.JBus.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;

namespace DKC.JBus.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}