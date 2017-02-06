using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.Web.Admin.Models;
using System.Configuration;
using Newtonsoft.Json;
using System.Text;
using OWZX.Model;


namespace OWZX.Web.Admin.Controllers
{
    /// <summary>
    /// 意见控制器类
    /// </summary>
    public partial class AdviceController : BaseAdminController
    {

        #region 意见
        /// <summary>
        /// 意见列表
        /// </summary>
        public ActionResult AdviceList(int pageSize = 15, int pageNumber = 1)
        {
            List<AdviceInfoModel> advicelist = Advice.GetUserAdvice(pageNumber, pageSize, " order by addtime desc");
            AdviceInfoList model = new AdviceInfoList()
            {
                AdviceList = advicelist,
                PageModel = new PageModel(pageSize, pageNumber, advicelist.Count > 0 ? advicelist[0].TotalCount : 0)
            };

            return View(model);
        }

        /// <summary>
        /// 回复意见
        /// </summary>
        /// <param name="adviceid"></param>
        /// <param name="reply"></param>
        /// <returns></returns>
        public ActionResult EditAdvice(int adviceid, string reply = "")
        {
            AdviceInfoModel advice = new AdviceInfoModel { Adviceid = adviceid, reply = reply, replyuid = WorkContext.Uid };
            bool result = Advice.UpdateUserAdvice(advice);
            if (result)
                return Content("1");
            else
                return Content("0");
        }
        #endregion

    }
}
