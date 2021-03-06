﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace JBus.Web.Infrastructure
{
    public class HandleAntiForgeryError : IExceptionFilter
    {
        #region IExceptionFilter Members

        public void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception as HttpAntiForgeryException;
            if (exception != null)
            {
                // TODO:
                //Elmah.ErrorSignal.FromCurrentContext().Raise(exception);

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            Error = "AntiForgeryError",
                            LogOnUrl = FormsAuthentication.LoginUrl
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    var routeValues = new RouteValueDictionary();
                    routeValues["controller"] = "Account";
                    routeValues["action"] = "Login";
                    filterContext.Result = new RedirectToRouteResult(routeValues);
                }
                filterContext.ExceptionHandled = true;
            }
        }

        #endregion IExceptionFilter Members
    }
}