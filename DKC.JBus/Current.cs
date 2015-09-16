using DKC.JBus.Constants;
using DKC.JBus.Controllers;
using DKC.JBus.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Caching;

namespace DKC.JBus
{
    /// <summary>
    /// Helper class that provides quick access to common objects used across a single request.
    /// </summary>
    public static class Current
    {
        /// <summary>
        /// Shortcut to HttpContext.Current.
        /// </summary>
        public static HttpContext Context
        {
            get { return HttpContext.Current; }
        }

        /// <summary>
        /// Shortcut to HttpContext.Current.Request.
        /// </summary>
        public static HttpRequest Request
        {
            get { return Context.Request; }
        }

        /// <summary>
        /// Gets the controller for the current request; should be set during init of current request's controller.
        /// </summary>
        public static BaseController Controller
        {
            get { return Context.Items["Controller"] as BaseController; }
            set { Context.Items["Controller"] = value; }
        }

        /// <summary>
        /// Gets the current "authenticated" user from this request's controller.
        /// </summary>
        public static User User
        {
            get
            {
                if (Controller == null)
                {
                    return BaseController.GetCurrentUser(HttpContext.Current.Request, HttpContext.Current.User.Identity.Name);
                }
                return Controller.CurrentUser;
            }
        }

        /// <summary>
        /// Gets the single data context for this current request.
        /// </summary>
        public static AppDatabase DB
        {
            get
            {
                AppDatabase result = null;
                if (Context != null)
                {
                    result = Context.Items["DB"] as AppDatabase;
                }
                else
                {
                    // unit tests
                    result = CallContext.GetData("DB") as AppDatabase;
                }

                if (result == null)
                {
                    DbConnection cnn = new SqlConnection(Application.ConnectionStrings);
                    //if (Current.Profiler != null)
                    //    cnn = new StackExchange.Profiling.Data.ProfiledDbConnection(cnn, new ErrorLoggingProfiler(Current.Profiler));
                    cnn.Open();
                    result = AppDatabase.Init(cnn, 30);
                    if (Context != null)
                    {
                        Context.Items["DB"] = result;
                    }
                    else
                    {
                        CallContext.SetData("DB", result);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Allows end of reqeust code to clean up this request's DB.
        /// </summary>
        public static void DisposeDB()
        {
            AppDatabase db = null;
            if (Context != null)
            {
                db = Context.Items["DB"] as AppDatabase;
            }
            else
            {
                db = CallContext.GetData("DB") as AppDatabase;
            }
            if (db != null)
            {
                db.Dispose();
                if (Context != null)
                {
                    Context.Items["DB"] = null;
                }
                else
                {
                    CallContext.SetData("DB", null);
                }
            }
        }

        /// <summary>
        /// retrieve an integer from the HttpRuntime.Cache; returns 0 if value does not exist
        /// </summary>
        public static int GetCachedInt(string key)
        {
            object o = HttpRuntime.Cache[key];
            if (o == null) return 0;
            return (int)o;
        }

        /// <summary>
        /// remove a cached object from the HttpRuntime.Cache
        /// </summary>
        public static void RemoveCachedObject(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        /// <summary>
        /// retrieve an object from the HttpRuntime.Cache
        /// </summary>
        public static object GetCachedObject(string key)
        {
            return HttpRuntime.Cache[key];
        }

        /// <summary>
        /// add an object to the HttpRuntime.Cache with an absolute expiration time
        /// </summary>
        public static void SetCachedObject(string key, object o, int durationSecs)
        {
            HttpRuntime.Cache.Add(
                key,
                o,
                null,
                DateTime.Now.AddSeconds(durationSecs),
                Cache.NoSlidingExpiration,
                CacheItemPriority.High,
                null);
        }

        /// <summary>
        /// add an object to the HttpRuntime.Cache with a sliding expiration time
        /// </summary>
        public static void SetCachedObjectSliding(string key, object o, int slidingSecs)
        {
            HttpRuntime.Cache.Add(
                key,
                o,
                null,
                Cache.NoAbsoluteExpiration,
                new TimeSpan(0, 0, slidingSecs),
                CacheItemPriority.High,
                null);
        }

        /// <summary>
        /// add a non-removable, non-expiring object to the HttpRuntime.Cache
        /// </summary>
        public static void SetCachedObjectPermanent(string key, object o)
        {
            HttpRuntime.Cache.Remove(key);
            HttpRuntime.Cache.Add(
                key,
                o,
                null,
                Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable,
                null);
        }

        /// <summary>
        /// retrieves a string from the HttpContext.Cache, or null if the key doesn't exist
        /// </summary>
        public static string GetCachedString(string key)
        {
            object o = HttpRuntime.Cache[key];
            if (o != null) return o.ToString();
            return null;
        }

        /// <summary>
        /// places a string in the HttpContext.Cache
        /// cached with "sliding expiration", so will only be deleted if NOT accessed for durationSecs
        /// </summary>
        public static void SetCachedString(string key, int durationSecs, string s)
        {
            HttpRuntime.Cache.Add(
                key,
                s,
                null,
                DateTime.MaxValue,
                TimeSpan.FromSeconds(durationSecs),
                CacheItemPriority.High,
                null);
        }
    }
}