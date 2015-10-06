using DKC.JBus.Helpers;
using StackExchange.Profiling;
using StackExchange.Profiling.Mvc;
using StackExchange.Profiling.Storage;
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
            //string s = Crypto.HashPassword("adm");
            //s = Crypto.HashPassword("cus");
            //s = Crypto.HashPassword("off");

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

            //InitProfilerSettings();
            // Setup profiler for Controllers via a Global ActionFilter
            GlobalFilters.Filters.Add(new ProfilingActionFilter());

            // initialize automatic view profiling
            var copy = ViewEngines.Engines.ToList();
            ViewEngines.Engines.Clear();
            foreach (var item in copy)
            {
                ViewEngines.Engines.Add(new ProfilingViewEngine(item));
            }
        }

        //protected void Session_Start()
        //{
        //    // unless you access your session object on the backend, a new sessionId will be generated with each request
        //    Session["init"] = 0; // fix the session until it expires.
        //}

        protected void Application_BeginRequest()
        {
            MiniProfiler.Start();
        }

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
            MiniProfiler.Stop();
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

        /// <summary>
        /// Gets or sets a value indicating whether disable profiling results.
        /// </summary>
        public static bool DisableProfilingResults { get; set; }

        /// <summary>
        /// Customize aspects of the MiniProfiler.
        /// </summary>
        private void InitProfilerSettings()
        {
            // a powerful feature of the MiniProfiler is the ability to share links to results with other developers.
            // by default, however, long-term result caching is done in HttpRuntime.Cache, which is very volatile.
            //
            // let's rig up serialization of our profiler results to a database, so they survive app restarts.

            // Setting up a MultiStorage provider. This will store results in the HttpRuntimeCache (normally the default) and in SqlLite as well.
            //MultiStorageProvider multiStorage = new MultiStorageProvider(
            //    new HttpRuntimeCacheStorage(new TimeSpan(1, 0, 0)));
            //MiniProfiler.Settings.Storage = multiStorage;

            // different RDBMS have different ways of declaring sql parameters - SQLite can understand inline sql parameters just fine
            // by default, sql parameters won't be displayed
            MiniProfiler.Settings.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();

            // these settings are optional and all have defaults, any matching setting specified in .RenderIncludes() will
            // override the application-wide defaults specified here, for example if you had both:
            //    MiniProfiler.Settings.PopupRenderPosition = RenderPosition.Right;
            //    and in the page:
            //    @MiniProfiler.RenderIncludes(position: RenderPosition.Left)
            // then the position would be on the left that that page, and on the right (the app default) for anywhere that doesn't
            // specified position in the .RenderIncludes() call.
            MiniProfiler.Settings.PopupRenderPosition = RenderPosition.Right; // defaults to left
            MiniProfiler.Settings.PopupMaxTracesToShow = 10;                  // defaults to 15
            MiniProfiler.Settings.RouteBasePath = "~/profiler";               // e.g. /profiler/mini-profiler-includes.js

            // optional settings to control the stack trace output in the details pane
            // the exclude methods are not thread safe, so be sure to only call these once per appdomain
            //MiniProfiler.Settings.ExcludeType("SessionFactory"); // Ignore any class with the name of SessionFactory
            //MiniProfiler.Settings.ExcludeAssembly("NHibernate"); // Ignore any assembly named NHibernate
            //MiniProfiler.Settings.ExcludeMethod("Flush");        // Ignore any method with the name of Flush
            // MiniProfiler.Settings.ShowControls = true;
            MiniProfiler.Settings.StackMaxLength = 256;          // default is 120 characters

            // because profiler results can contain sensitive data (e.g. sql queries with parameter values displayed), we
            // can define a function that will authorize clients to see the json or full page results.
            // we use it on http://stackoverflow.com to check that the request cookies belong to a valid developer.
            MiniProfiler.Settings.Results_Authorize = request =>
            {
                // you may implement this if you need to restrict visibility of profiling on a per request basis

                // for example, for this specific path, we'll only allow profiling if a query parameter is set
                //if ("/Home/ResultsAuthorization".Equals(request.Url.LocalPath, StringComparison.OrdinalIgnoreCase))
                //{
                //    return (request.Url.Query).ToLower().Contains("isauthorized");
                //}

                // all other paths can check our global switch
                return !DisableProfilingResults;
            };

            // the list of all sessions in the store is restricted by default, you must return true to allow it
            MiniProfiler.Settings.Results_List_Authorize = request =>
            {
                // you may implement this if you need to restrict visibility of profiling lists on a per request basis
                return true; // all requests are kosher
            };
        }
    }
}