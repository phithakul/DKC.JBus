using DKC.JBus.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Security;

namespace DKC.JBus.Infrastructure
{
    /// <summary>
    /// Extend AuthorizeAttribute to correctly handle AJAX authorization
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = base.AuthorizeCore(httpContext);
            if (authorized && httpContext.User.IsInRole("Agent"))
            {
                int id;
                if (Int32.TryParse(httpContext.User.Identity.Name, out id))
                {
                    User.UpdateActivity(id);
                }
                else
                {
                    authorized = false;
                }
            }
            return authorized;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new HttpException(403, "403 Forbidden - " + filterContext.RequestContext.HttpContext.Request.RawUrl), System.Web.HttpContext.Current);
            }
            else
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new HttpException(401, "401 Unauthorized - " + filterContext.RequestContext.HttpContext.Request.RawUrl), System.Web.HttpContext.Current);
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        Error = "NotAuthorized",
                        LogOnUrl = FormsAuthentication.LoginUrl
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                // this is a standard request, let parent filter to handle it
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}