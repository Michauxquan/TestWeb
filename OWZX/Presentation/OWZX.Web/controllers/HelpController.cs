using System;
using System.Web.Mvc;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.Web.Models;

namespace OWZX.Web.Controllers
{
    /// <summary>
    /// 帮助控制器类
    /// </summary>
    public partial class HelpController : BaseWebController
    {
        /// <summary>
        /// 问题
        /// </summary>
        public ActionResult Question()
        {
            //问题id
            int id = GetRouteInt("id");
            if (id == 0)
                id = WebHelper.GetQueryInt("id");

            HelpInfo helpInfo = Helps.GetHelpById(id);
            if (helpInfo == null)
                return PromptView("/", "你访问的页面不存在");


            QuestionModel model = new QuestionModel();
            model.HelpInfo = helpInfo;
            model.HelpList = Helps.GetHelpList();
            return View(model);
        }
    }
}
