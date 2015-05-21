using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
