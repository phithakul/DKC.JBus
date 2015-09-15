using DKC.JBus.Infrastructure;
using System.Web;
using System.Web.Mvc;

namespace DKC.JBus
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // ถ้าใช้ extent class ไม่ต้อง register base class เพราะจะทำงานทั้ง extent class และ base class
            filters.Add(new CustomAuthorizeAttribute());
            //filters.Add(new HandleErrorAttribute()); // ให้ elmah add แทน
            filters.Add(new NoCacheAttribute());
            //filters.Add(new RequireHttpsAttribute());
        }
    }
}