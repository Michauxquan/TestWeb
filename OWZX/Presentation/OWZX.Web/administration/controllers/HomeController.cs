using System;
using System.Web;
using System.Web.Mvc;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.Web.Admin.Models;

namespace OWZX.Web.Admin.Controllers
{
    /// <summary>
    /// 后台首页控制器类
    /// </summary>
    public partial class HomeController : BaseAdminController
    {
        /// <summary>
        /// 首页
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 导航栏
        /// </summary>
        public ActionResult NavBar()
        {
            return View();
        }

        /// <summary>
        /// 菜单栏
        /// </summary>
        public ActionResult Menu()
        {
            return View();
        }

        /// <summary>
        /// 商城运行信息
        /// </summary>
        public ActionResult ShopRunInfo()
        {
            ShopRunInfoModel model = new ShopRunInfoModel();

           
            model.OnlineUserCount = OnlineUsers.GetOnlineUserCount();
            model.OnlineGuestCount = OnlineUsers.GetOnlineGuestCount();
            model.OnlineMemberCount = model.OnlineUserCount - model.OnlineGuestCount;

            model.ShopVersion = BSPVersion.SHOP_VERSION;
            model.NetVersion = Environment.Version.ToString();
            model.OSVersion = Environment.OSVersion.ToString();
            model.TickCount = (Environment.TickCount / 1000 / 60).ToString();
            model.ProcessorCount = Environment.ProcessorCount.ToString();
            model.WorkingSet = (Environment.WorkingSet / 1024 / 1024).ToString();

            ShopUtils.SetAdminRefererCookie(Url.Action("shopruninfo"));
            return View(model);
        }
    }
}
