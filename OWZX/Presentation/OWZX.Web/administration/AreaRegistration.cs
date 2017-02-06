using System;
using System.Web.Mvc;

namespace OWZX.Web.Admin
{
    public class AreaRegistration : System.Web.Mvc.AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "admin";
            }
        }

        public override void RegisterArea(System.Web.Mvc.AreaRegistrationContext context)
        {
            //如果更改后没有效果，清空web中bin文件夹的所有dll
            //此路由不能删除
            context.MapRoute("admin_default",
                              "admin/{controller}/{action}/{id}",
                              new { controller = "home", action = "index", area = "admin", id = UrlParameter.Optional },
                              new[] { "OWZX.Web.Admin.Controllers" });

        }
    }
}
