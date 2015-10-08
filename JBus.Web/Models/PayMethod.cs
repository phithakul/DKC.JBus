using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace JBus.Web.Models
{
    public class PayMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsOwnerDebit { get; set; }
    }
}