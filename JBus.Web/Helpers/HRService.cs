using Dapper;
using JBus.Web.Constants;
using JBus.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace JBus.Web.Helpers
{
    public class HRService
    {
        public static AppUser GetUser(string username)
        {
            DbConnection cnn = new SqlConnection(Application.HRConnectionString);
            AppUser user = null;
#if !DEBUG
            try
            {
                if (Current.Profiler != null)
                    cnn = new StackExchange.Profiling.Data.ProfiledDbConnection(cnn, new ErrorLoggingProfiler(Current.Profiler));
                cnn.Open();
                user = cnn.Query<User>("select * from Users where Username=@Username",
                    new { username }).FirstOrDefault();
            }
            finally
            {
                if (cnn != null)
                {
                    cnn.Dispose();
                }
            }

#else
            user = new AppUser()
            {
                Username = username,
                FullName = "พิฐากูร สุวรรณเนกข์",
                Department = "ศรัทธาภิบาล",
                Section = "ธรรมวารี",
                MemberType = "อาสาสมัคร"
            };
#endif
            return user;
        }
    }
}