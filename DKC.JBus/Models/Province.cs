using System;
using System.Collections.Generic;

namespace DKC.JBus.Models
{
    public class Province
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public static IEnumerable<Province> GetDisplayProvinces()
        {
            return Current.DB.Query<Province>(@"
select * from Provinces
order by Name");
        }
    }
}