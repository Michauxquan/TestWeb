using System;
using System.Web.Mvc;
using System.Web.Routing;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;

namespace OWZX.Web
{
    public class BrnShopApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //将默认视图引擎替换为ThemeRazorViewEngine引擎
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ThemeRazorViewEngine());

            AreaRegistration.RegisterAllAreas();

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //启动事件机制
            BSPEvent.Start();
            ////启动服务
            //LotteryService.Instance.Start();
            //服务器宕机启动后重置在线用户表
            if (Environment.TickCount > 0 && Environment.TickCount < 900000)
                OnlineUsers.ResetOnlineUserTable();
        }
    }
}