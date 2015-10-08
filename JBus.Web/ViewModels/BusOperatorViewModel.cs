using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JBus.Web.ViewModels
{
    public class BusOperatorViewModel
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
    }
}