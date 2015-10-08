using System;
using System.Collections.Generic;
using System.Text;

namespace JBus.Web.Models
{
    public class Amphur
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}