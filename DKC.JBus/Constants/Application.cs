using DKC.JBus.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DKC.JBus.Constants
{
    public class Application
    {
        public const string Name = "JBus";
        public const string Version = "2558.09.15.0";

        private static int _ticketTimeout = Int32.Parse(ConfigurationManager.AppSettings["EXPIRED_LOGIN_MINUTE"]);
        private static bool _isLoginViaLdap = (ConfigurationManager.AppSettings["LOGIN_VIA_LDAP"].ToString().ToLower() == "true");
        private static string _connectionStrings = ConfigurationManager.ConnectionStrings["AppConnection"].ConnectionString;

        public static int TicketTimeout { get { return _ticketTimeout; } }
        public static bool IsLoginViaLdap { get { return _isLoginViaLdap; } }
        public static string ConnectionStrings { get { return _connectionStrings; } }
    }
}