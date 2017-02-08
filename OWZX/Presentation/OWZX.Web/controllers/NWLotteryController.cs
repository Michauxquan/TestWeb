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
    public class NWLotteryController : Controller
    {
        //
        // GET: /NWLottery/

        public ActionResult LTIndex()
        {
            return View();
        }

    }
}
