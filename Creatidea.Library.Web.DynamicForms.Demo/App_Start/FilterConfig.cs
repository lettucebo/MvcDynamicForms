using System.Web;
using System.Web.Mvc;

namespace Creatidea.Library.Web.DynamicForms.Demo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
