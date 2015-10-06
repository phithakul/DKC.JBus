using System;
using System.Collections.Generic;

namespace DKC.JBus.Domains
{
    public class Province
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }

        public static IEnumerable<Province> GetDisplayProvinces()
        {
            return Current.DB.Query<Province>(@"
select * from Provinces
order by Name");
        }
    }
}