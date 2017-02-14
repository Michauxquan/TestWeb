using OWZX.Core;
using OWZX.Model;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
                LotteryType = id,
                PageModel = new PageModel(20, 1, list.TotalCount),
                lotterylist = list
            };
            return View(lot);
        }
        /// <summary>
        /// dd28获取数据
        /// </summary>
        /// <returns></returns>
        public ActionResult _Content()
        {
            int type = WebHelper.GetFormInt("type");
            int pageindex = WebHelper.GetFormInt("pageindex");
            MD_LotteryList list = LotteryList.GetLotteryByType(type, pageindex, 20, WorkContext.Uid);
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

        /// <summary>
        /// 投注
        /// </summary>
        /// <returns></returns>
        public ActionResult _BettPage()
        {
            int type = WebHelper.GetFormInt("type");
            int settype = 13;
            switch (type)
            { 
                case 1:
                case 2:
                case 6:
                    settype = (int)LotterySetType.number;
                    break;
                case 4:
                case 5:
                    settype = (int)LotterySetType.sdz;
                    break;
                case 9:
                    settype = (int)LotterySetType.lhb;
                    break;
                case 7:
                    break;
                case 8:
                    break;
            }
            DataSet ds = LotteryList.GetLotterySet(settype);
            ViewData["ltset"] = ds;
            ViewData["exists"] = NewUser.ExistsMode(WorkContext.Uid);
            return View();
        }
        /// <summary>
        /// 游戏规则
        /// </summary>
        /// <returns></returns>
        public ActionResult _LTRule()
        {
            int type = WebHelper.GetFormInt("type");
            ViewData["ltruletype"] = type;
            return View();
        }
        /// <summary>
        /// 投注记录
        /// </summary>
        /// <returns></returns>
        public ActionResult _BettRecord()
        {
            int type = WebHelper.GetFormInt("type");
            int pageindex = WebHelper.GetFormInt("pageindex");
            int pagesize = 20;
            DataTable dt = LotteryList.GetUserBett(type,2 , pageindex, pagesize);//WorkContext.Uid
            LotteryRecord record = new LotteryRecord()
            {
                PageModel = new PageModel(20, pageindex, dt.Rows.Count > 0 ? int.Parse(dt.Rows[0]["totalcount"].ToString()) : 0),
                Records = dt
            };
            return View(record);
        }
    }
}
