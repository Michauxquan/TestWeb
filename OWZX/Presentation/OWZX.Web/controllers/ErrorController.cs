using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OWZX.Web.Controllers
{
    public class ErrorController : BaseWebController
    {
        /// <summary>
        /// 错误页
        /// </summary>
        public ActionResult Index(string m)
        {
            if (string.IsNullOrWhiteSpace(m))
                ViewData["Message"] = string.Empty;
            else
            {
                m = HttpUtility.UrlDecode(m);
                ViewData["Message"] = "服务器内部错误";
            }
            return APIResult("error","服务器内部错误");
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
                ViewData["Message"] = "服务器内部错误";
            }
            return View();
        }

    }
}
