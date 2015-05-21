using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ZYNY_ZhuiSu
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            // Ddl/Ddl_Keys_Address/1/City/Ddl_City/Suburb,Street
            routes.MapRoute(
                "Ddl"
                , "Ddl/{DdlKeyConfig}/{parentId}/{ddlName}/{key}/{childIds}"
                , new { controller = "Ddl", action = "Index", childIds = "" }
            );

        }
    }
}
