using Dapper;
using DKC.JBus.Constants;
using DKC.JBus.Domains;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DKC.JBus.Helpers
{
    public class HRService
    {
        public static User GetUser(string username)
        {
            DbConnection cnn = new SqlConnection(Application.HRConnectionString);
            User user = null;
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
            user = new User()
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