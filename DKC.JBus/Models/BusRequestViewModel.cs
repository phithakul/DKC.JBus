using Dapper;
using DKC.JBus.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DKC.JBus.Models
{
    public class BusRequestViewModel
    {
        public int Id { get; set; }
        public int RequestUserId { get; set; }
        public User RequestUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string RequesterPhone { get; set; }
        public string CoorName { get; set; }
        public string CoorPhone { get; set; }

        public List<BusRequestPay> BusRequestPays { get; set; }

        public decimal Amount { get; set; }
        public TravelType TravelType { get; set; }
        public byte ReqReceipt { get; set; }
        public TravelInfo Departure { get; set; }
        public TravelInfo Arrival { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public byte[] RowVersion { get; set; }

        public static IEnumerable<BusRequest> GetList()
        {
            //            var builder = new SqlBuilder();
            //            var template = builder.AddTemplate(@"
            //select c.Id, c.Code, c.Name, c.Note, c.Active,
            //    ct.Id, ct.Name,
            //    p.Id, p.Code, p.Name
            //from Requests r
            //    join CourseTypes ct on c.CourseTypeId=ct.Id
            //    left join CoursePositions cp on c.Id=cp.CourseId
            //    left join Positions p on cp.PositionId=p.Id
            ///**where**/
            //order by c.Code");
            //            if (!codeOrName.IsNullOrEmpty())
            //            {
            //                builder.Where(@"(UPPER(c.Code) collate Thai_BIN2 LIKE UPPER('%' + @codeOrName + '%')
            //                    OR UPPER(c.Name) collate Thai_BIN2 LIKE UPPER('%' + @codeOrName + '%'))",
            //                    new { codeOrName });
            //            }
            //            if (activeOnly)
            //            {
            //                builder.Where("Active=1");
            //            }
            //            var lookup = new Dictionary<int, Course>();
            //            Current.DB.Query<Course, CourseType, Position, Course>(template.RawSql,
            //                (c, ct, p) =>
            //                {
            //                    Course course;
            //                    if (!lookup.TryGetValue(c.Id, out course))
            //                    {
            //                        lookup.Add(c.Id, course = c);
            //                        course.CourseType = ct;
            //                    }
            //                    if (p != null)
            //                    {
            //                        course.Positions.Add(p);
            //                    }
            //                    return course;
            //                },
            //                template.Parameters);
            //            return (IEnumerable<Course>)lookup.Values;
            return Enumerable.Empty<BusRequest>();
        }
    }
}