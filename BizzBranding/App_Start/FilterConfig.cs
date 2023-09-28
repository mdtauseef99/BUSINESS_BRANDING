using System.Web;
using System.Web.Mvc;
using BizzBranding.Filters;

namespace BizzBranding
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustumAuthorize());
        }
    }
}