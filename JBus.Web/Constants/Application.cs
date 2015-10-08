using JBus.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace JBus.Web.Constants
{
    public class Application
    {
        public const string Code = "JBus";
        public const string Name = "ระบบจองรถบัส";
        public const string Version = "2558.09.15.0";

        private static string _connectionString = ConfigurationManager.ConnectionStrings["AppConnection"].ConnectionString;
        private static string _hrConnectionString = ConfigurationManager.ConnectionStrings["HRConnection"].ConnectionString;
        private static int _ticketTimeout = Int32.Parse(ConfigurationManager.AppSettings["EXPIRED_LOGIN_MINUTE"]);
        private static bool _isLoginViaLdap = (ConfigurationManager.AppSettings["LOGIN_VIA_LDAP"].ToString().ToLower() == "true");

        public static string ConnectionString { get { return _connectionString; } }
        public static string HRConnectionString { get { return _hrConnectionString; } }

        public static string SqLiteConnectionString
        {
            get { return "Data Source = " + HttpContext.Current.Server.MapPath("~/App_Data/TestMiniProfiler.sqlite"); }
        }

        public static int TicketTimeout { get { return _ticketTimeout; } }
        public static bool IsLoginViaLdap { get { return _isLoginViaLdap; } }
    }
}