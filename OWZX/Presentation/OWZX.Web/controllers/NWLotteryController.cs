using OWZX.Model;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.Web.Models;
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
        public ActionResult LTIndex(int id)
        {
            MD_LotteryList list = LotteryList.GetLotteryByType(id, 1, 20, WorkContext.Uid);

            LotteryModel lot = new LotteryModel()
            {
                PageModel = new PageModel(20, 1, list.TotalCount),
                lotterylist = list
            };
            return View(lot); 
        }
        /// <summary>
        /// dd28获取数据
        /// </summary>
        /// <returns></returns>
        public ActionResult LT28Data(int type, int pageindex = 1, int pagesize = 20)
        {
            MD_LotteryList list = LotteryList.GetLotteryByType(type, pageindex, pagesize, WorkContext.Uid);
            LotteryModel lot = new LotteryModel()
            {
                PageModel = new PageModel(20, pageindex, list.TotalCount),
                lotterylist = list
            };
            return View(lot); 
        }



        public ActionResult LT36Index()
        {
            return View();
        }
    }
}
