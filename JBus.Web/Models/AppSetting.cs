using Dapper;
using JBus.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace JBus.Web.Models
{
    public class AppSetting
    {
        public int Id { get; set; }

        public string Setting { get; set; }

        public string Value { get; set; }

        public static AppSetting Get(string setting)
        {
            return Current.DB.Query<AppSetting>(@"
select * from AppSettings
where Setting=@setting", new { setting }).SingleOrDefault();
        }

        public static void Save(string settingName)
        {
            dynamic list = null;

            var sql = @"
update AppSettings with (serializable)
set Value = @value
where Setting = @setting
if @@rowcount = 0
begin
    insert AppSettings (Setting, Value)
    values (@setting,@value)
end";

            Current.DB.BeginTransaction();
            try
            {
                Current.DB.Execute(sql, list);
                Current.DB.CommitTransaction();
            }
            catch (SqlException)
            {
                Current.DB.RollbackTransaction();
                throw;
            }
        }
    }
}