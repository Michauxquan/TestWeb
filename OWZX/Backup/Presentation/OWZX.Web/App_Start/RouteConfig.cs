using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OWZX.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //默认路由(此路由不能删除)
            routes.MapRoute("default",
                            "{controller}/{action}/{id}",
                            new { controller = "home", action = "index", id = UrlParameter.Optional },
                            new[] { "OWZX.Web.Controllers" });
        }
    }
}