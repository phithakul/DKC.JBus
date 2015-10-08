using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JBus.Web
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
                namespaces: new[] { "JBus.Web.Controllers" }
            );
            routes.MapRoute(
                name: null,
                url: "",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { "JBus.Web.Controllers" }
            );
            routes.MapRoute(
                name: null,
                url: "login",
                defaults: new { controller = "Account", action = "Login" },
                namespaces: new[] { "JBus.Web.Controllers" }
            );
            routes.MapRoute(
                name: null,
                url: "logout",
                defaults: new { controller = "Account", action = "Logout" },
                namespaces: new[] { "JBus.Web.Controllers" }
            );

            // domain ----------------------------------------------------------
            routes.MapRoute(
                name: null,
                url: "requests/{action}/{id}",
                defaults: new { controller = "BusRequest", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JBus.Web.Controllers" }
            );
            routes.MapRoute(
                name: null,
                url: "orders/{action}/{id}",
                defaults: new { controller = "BusOrder", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JBus.Web.Controllers" }
            );
            routes.MapRoute(
                name: null,
                url: "payments/{action}/{id}",
                defaults: new { controller = "Payment", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JBus.Web.Controllers" }
            );
            routes.MapRoute(
                name: null,
                url: "paymentgroups/{action}/{id}",
                defaults: new { controller = "PaymentGroup", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JBus.Web.Controllers" }
            );
            routes.MapRoute(
                name: null,
                url: "busoperators/{action}/{id}",
                defaults: new { controller = "BusOperator", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JBus.Web.Controllers" }
            );

            // admin ----------------------------------------------------------
            routes.MapRoute(
                name: null,
                url: "admin/users/{action}/{id}",
                defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JBus.Web.Controllers" }
            );
            routes.MapRoute(
                name: null,
                url: "admin/{action}",
                defaults: new { controller = "Admin", action = "Index" },
                namespaces: new[] { "JBus.Web.Controllers" }
            );

            // error ----------------------------------------------------------
            routes.MapRoute(
                name: null,
                url: "error",
                defaults: new { controller = "Error", action = "ErrorPage" },
                namespaces: new[] { "JBus.Web.Controllers" }
            );
            routes.MapRoute(
                name: null,
                url: "404",
                defaults: new { controller = "Error", action = "PageNotFound" },
                namespaces: new[] { "JBus.Web.Controllers" }
            );
        }
    }
}