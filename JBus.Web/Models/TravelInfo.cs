using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JBus.Web.Models
{
    public class TravelInfo
    {
        public int Id { get; set; }
        public int FromProvinceId { get; set; }
        public int FromAmphurId { get; set; }
        public string PickupPoint { get; set; }
        public int ToProvinceId { get; set; }
        public int ToAmphurId { get; set; }
        public string DropOffPoint { get; set; }

        public DateTime StartDate { get; set; }

        public int MonkQty { get; set; }
        public int LaymanQty { get; set; }

        public int Bus1Qty { get; set; }
        public int Bus2Qty { get; set; }
        public int FanQty { get; set; }

        public string StopPoint1 { get; set; }
        public string StopPoint2 { get; set; }
        public string StopPoint3 { get; set; }
        public string StopPoint4 { get; set; }
        public string StopPoint5 { get; set; }
    }
}