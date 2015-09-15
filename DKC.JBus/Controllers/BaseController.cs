using DKC.JBus.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace DKC.JBus.Controllers
{
    public class BaseController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

#if DEBUG
        private Stopwatch watch;
#endif

        protected override void Initialize(RequestContext requestContext)
        {
#if DEBUG
            watch = new Stopwatch();
            watch.Start();
#endif

            Current.Controller = this; // allow code to easily find this controller
            ValidateRequest = false; // allow html/sql in form values - remember to validate!
            base.Initialize(requestContext);
        }

        // In that base controller class, override HandleUnknownAction method. Now, another scenario that a 404 may happen is that when the controller exists, but there is no action for it.
        // In this case, the routing table will not be able to trap it easily - but the controller class has a method that handle that.
        /// <summary>
        /// called when the url doesn't match any of our known routes
        /// </summary>
        //protected override void HandleUnknownAction(string actionName)
        //{
        //    PageNotFound().ExecuteResult(ControllerContext);
        //}

        /// <summary>
        /// Gets the shared DataContext to be used by a Request's controllers.
        /// </summary>
        public AppDatabase DB
        {
            get { return Current.DB; }
        }

#if DEBUG

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            log.Debug("OnActionExecuting -> " + Request.Url.PathAndQuery + " Duration: " + watch.ElapsedMilliseconds);
            Trace.WriteLine("OnActionExecuting -> " + Request.Url.PathAndQuery + " Duration: " +
                            watch.ElapsedMilliseconds);
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// fires after the controller finishes execution
        /// </summary>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            log.Debug("OnActionExecuted -> " + Request.Url.PathAndQuery + " Duration: " + watch.ElapsedMilliseconds);
            Trace.WriteLine("OnActionExecuted -> " + Request.Url.PathAndQuery + " Duration: " +
                            watch.ElapsedMilliseconds);

            base.OnActionExecuted(filterContext);
        }

#endif

        private User _currentUser;

        /// <summary>
        /// Gets a User object representing the current request's client.
        /// </summary>
        public User CurrentUser
        {
            get
            {
                if (_currentUser == null) InitCurrentUser();
                return _currentUser;
            }
        }

        /// <summary>
        /// initializes current user based on the current Request's cookies/authentication status. This
        /// method could return a newly created, Anonymous User if no means of identification are found.
        /// </summary>
        protected void InitCurrentUser()
        {
            _currentUser = GetCurrentUser(Request, User.Identity.Name);
        }

        public static User GetCurrentUser(HttpRequest request, string identity)
        {
            return GetCurrentUser(request.IsAuthenticated, request.UserHostAddress, identity);
        }

        public static User GetCurrentUser(HttpRequestBase request, string identity)
        {
            return GetCurrentUser(request.IsAuthenticated, request.UserHostAddress, identity);
        }

        private static User GetCurrentUser(bool isAuthenticated, string userHostAddress, string identity)
        {
            var user = new User();
            user.IsAnonymous = true;

            if (isAuthenticated)
            {
                int id;
                if (Int32.TryParse(identity, out id))
                {
                    User lookup = Current.DB.Users.Get(id);
                    if (lookup != null)
                    {
                        user = lookup;
                        user.IsAnonymous = false;
                    }
                }
                else
                {
                    FormsAuthentication.SignOut();
                }
            }

            //user.IPAddress = userHostAddress;
            return user;
        }

        public Dictionary<string, string> GetErrorsFromModelState()
        {
            var errors = new Dictionary<string, string>();
            foreach (var key in ModelState.Keys)
            {
                if (ModelState[key].Errors.Count > 0)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        errors[key] = error.ErrorMessage;
                    }
                }
                else
                {
                    errors[key] = "";
                }
            }
            return errors;
        }

        //public Dictionary<string, object> GetErrorsFromModelState()
        //{
        //    var errors = new Dictionary<string, object>();
        //    errors["Code"] = ModelState["Code"].Errors;
        //    errors["Name"] = ModelState["Name"].Errors;
        //    return errors;
        //}

        //public ActionResult GoToReferrer()
        //{
        //    if (Request.UrlReferrer != null)
        //    {
        //        return Redirect(Request.UrlReferrer.AbsoluteUri);
        //    }

        //    return RedirectToAction("Index", "Home");
        //}

        /// <summary>
        /// returns our standard page not found view
        /// </summary>
        protected ViewResult PageNotFound()
        {
            string url = Request.Url.ToString();
            //Elmah.ErrorSignal.FromCurrentContext().Raise(new HttpException(404, string.Format("Request Not Found: {0}", url)), System.Web.HttpContext.Current);
            //Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.NotFound; // ถ้าไม่กำหนดจะ response 200 ซึ่งไม่ถูกต้อง
            return View("PageNotFound");
        }

        /// <summary>
        /// returns a 301 permanent redirect
        /// </summary>
        /// <returns></returns>
        protected ContentResult PageMovedPermanentlyTo(string url)
        {
            Response.RedirectLocation = url;
            Response.StatusCode = (int)HttpStatusCode.MovedPermanently;
            return null;
        }
    }
}