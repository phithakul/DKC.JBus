using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;

namespace JBus.Web.Helpers.Security
{
    public class ActiveDirectory
    {
        public static bool AuthenticateUser(string username, string password)
        {
            if (!AppSettings.DomainName.IsNullOrEmpty())
            {
                if (!username.EndsWith("@" + AppSettings.DomainName))
                    username = username + "@" + AppSettings.DomainName;
            }

            var authed = RunCommand(ldc =>
            {
                // user has authenticated at this point, as the credentials were used to login to the dc.
                ldc.Bind(new NetworkCredential(username, password));
                return true;
            });
            return authed;
        }

        public static T RunCommand<T>(Func<LdapConnection, T> command, int retries = 3)
        {
            try
            {
                using (var ldc = new LdapConnection(new LdapDirectoryIdentifier(AppSettings.DomainControllerIP, 389)))
                {
                    ldc.AuthType = AuthType.Basic;
                    return command(ldc);
                }
            }
            catch (Exception)
            {
                if (retries > 0)
                {
                    Thread.Sleep(500);
                    RunCommand(command, retries - 1);
                }
                else
                {
                    // TODO:
                    //Current.LogException("Could not contact current AD", ex);
                }
            }
            return default(T);
        }
    }
}