using System.Web;
using System.Web.Mvc;

namespace SSW.HealthCheck.MVC4.Examples
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}