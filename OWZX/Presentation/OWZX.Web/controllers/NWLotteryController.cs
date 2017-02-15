using Newtonsoft.Json;
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
        /// <summary>
        /// 彩票是否开奖
        /// </summary>
        /// <returns></returns>
        public ActionResult LotteryOpen()
        {
            int type = WebHelper.GetFormInt("type");
            string expect = WebHelper.GetFormString("expect");
            bool result = LotteryList.ExistsLotteryOpen(type.ToString(), expect);
            if (result)
                return Content("1");
            else
                return Content("0");
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
            List<MD_BettMode> model = NewUser.GetModeList(1, 20, " where a.uid=" + WorkContext.Uid.ToString());
            return View(model);
        }
        /// <summary>
        /// 获取投注模式
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBettMode()
        {
            int id = WebHelper.GetQueryInt("id");
            MD_BettMode btmode = NewUser.GetModeList(1, 1, " where a.modeid=" + id.ToString())[0];
            string btjson = JsonConvert.SerializeObject(btmode);
            return Content(btjson);
        }
        /// <summary>
        /// 投注
        /// </summary>
        /// <returns></returns>
        public ActionResult AddBettInfo()
        {
            MD_Bett bett = new MD_Bett
            {
                Uid = WorkContext.Uid,
                Lotteryid = WebHelper.GetFormInt("lotterytype"),
                Lotterynum = WebHelper.GetFormString("fcnum"),
                Bettinfo = WebHelper.GetFormString("cusbettinfo"),
                Bettnum = WebHelper.GetFormString("bettnumber"),
                Money = WebHelper.GetFormInt("bettTotalEggs"),
                Bettmode = WebHelper.GetFormInt("bettmodel")
            };
            bool result = Lottery.AddNewBett(bett);
            if (result)
                return Content("1");
            else
                return Content("0");
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
            DataTable dt = LotteryList.GetUserBett(type, WorkContext.Uid, pageindex, pagesize);
            LotteryRecord record = new LotteryRecord()
            {
                PageModel = new PageModel(20, pageindex, dt.Rows.Count > 0 ? int.Parse(dt.Rows[0]["totalcount"].ToString()) : 0),
                Records = dt
            };
            return View(record);
        }
        /// <summary>
        /// 投注模式
        /// </summary>
        /// <returns></returns>
        public ActionResult _BettMode()
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
            List<MD_BettMode> model = NewUser.GetModeList(1, 20, " where a.uid=" + WorkContext.Uid.ToString());
            return View(model);
        }

        /// <summary>
        /// 添加投注模式
        /// </summary>
        /// <returns></returns>
        public ActionResult AddBettMode()
        {
            MD_BettMode btmode = new MD_BettMode
            {
                Name = WebHelper.GetFormString("mdname"),
                Bettinfo = WebHelper.GetFormString("list"),
                Bettnum = WebHelper.GetFormString("listnum"),
                Betttotal = WebHelper.GetFormInt("sum"),
                Uid = WorkContext.Uid
            };
            bool result = NewUser.AddMode(btmode);
            if (result)
                return Content("1");
            else
                return Content("0");
        }
        /// <summary>
        /// 删除模式
        /// </summary>
        /// <returns></returns>
        public ActionResult DelBettMode()
        {
            bool result = NewUser.DeleteMode(WebHelper.GetFormString("mdname"), WorkContext.Uid, WebHelper.GetFormInt("lotterytype"));
            if (result)
                return Content("1");
            else
                return Content("0");
        }


    }
}
