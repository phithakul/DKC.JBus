using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DKC.JBus
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // create routes from most specific to general.

            // - http://sellmyproducts/
            // - http://sellmyproducts/Page#
            // - http://sellmyproducts/category
            // - http://sellmyproducts/category/Page#

            //routes.IgnoreRoute("index.html"); //ignore the specific HTML start page
            //routes.IgnoreRoute(""); //to ignore any default root requests

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: "home",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { "DKC.JBus.Controllers" }
            );
            routes.MapRoute(
                name: null,
                url: "",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { "DKC.JBus.Controllers" }
            );
            routes.MapRoute(
                name: null,
                url: "login",
                defaults: new { controller = "Account", action = "Login" },
                namespaces: new[] { "DKC.JBus.Controllers" }
            );
            routes.MapRoute(
                name: null,
                url: "logout",
                defaults: new { controller = "Account", action = "Logout" },
                namespaces: new[] { "DKC.JBus.Controllers" }
            );

            // admin ----------------------------------------------------------
            routes.MapRoute(
                name: null,
                url: "admin/users/{action}/{id}",
                defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "DKC.JBus.Controllers" }
            );
            routes.MapRoute(
                name: null,
                url: "admin/{action}",
                defaults: new { controller = "Admin", action = "Index" },
                namespaces: new[] { "DKC.JBus.Controllers" }
            );

            // error ----------------------------------------------------------
            routes.MapRoute(
                name: null,
                url: "error",
                defaults: new { controller = "Error", action = "ErrorPage" },
                namespaces: new[] { "DKC.JBus.Controllers" }
            );
            routes.MapRoute(
                name: null,
                url: "404",
                defaults: new { controller = "Error", action = "PageNotFound" },
                namespaces: new[] { "DKC.JBus.Controllers" }
            );
        }
    }
}