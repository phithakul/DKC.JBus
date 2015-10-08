using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JBus.Web.Models
{
    public class BusOperator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string BankAcctName { get; set; }
        public string BankAcctNo { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string Contact { get; set; }
        public string Note { get; set; }
        public byte[] RowVersion { get; set; }

        public static IEnumerable<BusOperator> GetAlls()
        {
            var builder = new SqlBuilder();
            var template = builder.AddTemplate(@"
select *
from BusOperator bo
order by bo.Name");
            return Current.DB.Query<BusOperator>(template.RawSql, template.Parameters);
        }
    }
}