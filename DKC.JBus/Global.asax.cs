using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace DKC.JBus
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();

            // disable the X-AspNetMvc-Version: header
            MvcHandler.DisableMvcResponseHeader = true;
            ConfigureViewEngines();
            ConfigureAntiForgeryTokens();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.DefaultBinder = new CustomModelBinder();
            DatabaseMigrator.Init();
        }

        //protected void Session_Start()
        //{
        //    // unless you access your session object on the backend, a new sessionId will be generated with each request
        //    Session["init"] = 0; // fix the session until it expires.
        //}
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            // manipulate the ticket created in the AuthenticateUser() method.
            // write the user info into a data store

            if (Request.IsAuthenticated)
            {
                if (HttpContext.Current.User.Identity is FormsIdentity)
                {
                    var id = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = id.Ticket;

                    string[] roles = (ticket.UserData ?? "").Split(',');
                    HttpContext.Current.User = new GenericPrincipal(id, roles);
                }
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            Current.DisposeDB();
            //Current.DisposeRegisteredConnections();
        }

        /// <summary>
        /// Configures the view engines. By default, Asp.Net MVC includes the Web Forms (WebFormsViewEngine) and
        /// Razor (RazorViewEngine) view engines. You can remove view engines you are not using here for better
        /// performance.
        /// </summary>
        private static void ConfigureViewEngines()
        {
            // Only use the RazorViewEngine.
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }

        /// <summary>
        /// Configures the anti-forgery tokens.
        /// </summary>
        private static void ConfigureAntiForgeryTokens()
        {
            // Rename the Anti-Forgery cookie from "__RequestVerificationToken" to "f".
            // This adds a little security through obscurity and also saves sending a
            // few characters over the wire. Sadly there is no way to change the form input
            // name which is hard coded in the @Html.AntiForgeryToken helper and the
            // ValidationAntiforgeryTokenAttribute to  __RequestVerificationToken.
            // <input name="__RequestVerificationToken" type="hidden" value="..." />
            AntiForgeryConfig.CookieName = "f";

            // If you have enabled SSL. Uncomment this line to ensure that the Anti-Forgery
            // cookie requires SSL to be sent accross the wire.
            // AntiForgeryConfig.RequireSsl = true;
        }
    }
}