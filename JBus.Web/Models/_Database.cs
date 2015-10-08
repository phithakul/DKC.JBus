using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JBus.Web.Models
{
    public class AppDatabase : Dapper.Database<AppDatabase>
    {
        public Table<AppUser> Users { get; private set; }
        public Table<AppSetting> AppSettings { get; private set; }
        public Table<SmsLog> SmsLogs { get; private set; }
        public Table<MailLog> MailLogs { get; private set; }
        public Table<BusOperator> BusOperators { get; private set; }
    }
}