using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OWZX.Web.Admin.Controllers
{
    public class ErrorController : BaseAdminController
    {
        /// <summary>
        /// 错误页
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        public ActionResult Info(string m)
        {
            if (string.IsNullOrWhiteSpace(m))
                ViewData["Message"] = string.Empty;
            else
            {
                m = HttpUtility.UrlDecode(m);
                ViewData["Message"] = m;
            }
            return View();
        }

    }
}
