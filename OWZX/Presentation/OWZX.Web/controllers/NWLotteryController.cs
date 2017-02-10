using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OWZX.Web.controllers
{
    /// <summary>
    /// 竞猜控制器
    /// </summary>
    public class NWLotteryController : BaseWebController
    {
        //
        // GET: /NWLottery/
        /// <summary>
        /// dd28
        /// </summary>
        /// <returns></returns>
        public ActionResult LTIndex()
        {
            return View();
        }

        public ActionResult LT36Index()
        {
            return View();
        }
    }
}
