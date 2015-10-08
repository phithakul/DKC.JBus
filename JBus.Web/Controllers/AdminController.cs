using JBus.Web.Helpers;
using JBus.Web.Infrastructure;
using JBus.Web.Models;
using JBus.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;

namespace JBus.Web.Controllers
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