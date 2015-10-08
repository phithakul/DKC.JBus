using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace JBus.Web.Models
{
    public class BusRequestPay
    {
        public int Id { get; set; }
        public PayMethod PayMethod { get; set; }
        public decimal Amount { get; set; }
    }
}