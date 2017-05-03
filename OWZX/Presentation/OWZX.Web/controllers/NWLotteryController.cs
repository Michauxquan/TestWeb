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
using System.Text;
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
            int total = 0;
            int stop = 0;
            string title = string.Empty;
            InitParms(id, ref total, ref stop, ref title);
            total = ResetTotalTime(id, list, total);

            LotteryModel lot = new LotteryModel()
            {
                LotteryType = id,
                TotalS = total,
                StopTime = stop,
                Title = title,
                PageModel = new PageModel(20, 1, list.TotalCount),
                lotterylist = list
            };
            return View(lot);
        }

        private static int ResetTotalTime(int id, MD_LotteryList list, int total)
        {
            //dd28,dd36,ddlhb,pkgj,pkgyj  24:00~9:00 暂停投注
            //cakeno28,cakeno36  20:00~待定  暂停投注
            //hg28 5:00~7:00 暂停投注
            if (id == 1 || id == 4 || id == 9 || id == 7 || id == 8)
            {
                if (DateTime.Now > DateTime.Parse("00:00") && DateTime.Now < DateTime.Parse("9:02"))
                {
                    total = list.RemainS;
                }
            }
            else if (id == 2 || id == 5)
            {
                if (DateTime.Now > DateTime.Parse("20:00") && DateTime.Now < DateTime.Parse("23:00"))
                {
                    total = list.RemainS;
                }
            }
            else if (id == 6)
            {
                if (DateTime.Now > DateTime.Parse("5:00") && DateTime.Now < DateTime.Parse("7:00"))
                {
                    total = list.RemainS;
                }
            }
            else if (id == 3 || id == 10 || id == 11 || id == 12)
            {
                total = list.RemainS;
            }
            return total;
        }

        private static void InitParms(int id, ref int total, ref int stop, ref string title)
        {
            switch (id)
            {
                case (int)LotType.dd28:
                    title = "蛋蛋28首页";
                    total = 300;
                    stop = 30;
                    break;
                case (int)LotType.dd36: title = "蛋蛋36首页";
                    total = 300;
                    stop = 30;
                    break;
                case (int)LotType.ddlhb: title = "蛋蛋龙虎豹首页";
                    total = 300;
                    stop = 30;
                    break;
                case (int)LotType.cakeno28: title = "加拿大28首页";
                    total = 210;
                    stop = 30;
                    break;
                case (int)LotType.cakeno36: title = "加拿大36首页";
                    total = 210;
                    stop = 30;
                    break;
                case (int)LotType.pkgj: title = "PK冠军首页";
                    total = 300;
                    stop = 30;
                    break;
                case (int)LotType.pkgyj: title = "PK冠亚军首页";
                    total = 300;
                    stop = 30;
                    break;
                case (int)LotType.hg28:
                    title = "韩国28首页";
                    total = 115;
                    stop = 25;
                    break;
                case (int)LotType.js28:
                    title = "急速28首页";
                    total = 120;
                    stop = 20;
                    break;
                case (int)LotType.js10:
                    title = "急速10首页";
                    total = 120;
                    stop = 20;
                    break;
                case (int)LotType.js11:
                    title = "急速11首页";
                    total = 120;
                    stop = 20;
                    break;
                case (int)LotType.js16:
                    title = "急速16首页";
                    total = 120;
                    stop = 20;
                    break;
                case (int)LotType.hk6:
                    title = "六合彩首页";
                    total = 60*60*24*2;
                    stop = 600;
                    break;
            }
        }


        /// <summary>
        /// 竞猜首页数据（当期，上一次结果，竞猜集合）
        /// </summary>
        /// <returns></returns>
        public ActionResult _Index()
        {
            int id = WebHelper.GetFormInt("type");//彩票类型
            int page = WebHelper.GetFormInt("page", 1);
            MD_LotteryList list = LotteryList.GetLotteryByType(id, page, 20, WorkContext.Uid);
            int total = 0;
            int stop = 0;
            string title = string.Empty;
            InitParms(id, ref total, ref stop, ref title);
            total = ResetTotalTime(id, list, total);
            LotteryModel lot = new LotteryModel()
            {
                LotteryType = id,
                TotalS = total,
                StopTime = stop,
                PageModel = new PageModel(20, page, list.TotalCount),
                lotterylist = list
            };
            return View(lot);
        }

        /// <summary>
        /// 通用页面
        /// </summary>
        /// <returns></returns>
        public ActionResult _TabCommon()
        {
            int id = WebHelper.GetFormInt("type");//彩票类型
            int page = WebHelper.GetFormInt("page", 1);
            MD_LotteryList list = LotteryList.GetLotteryByType(id, 1, 20, WorkContext.Uid);
            int total = 0;
            int stop = 0;
            string title = string.Empty;
            InitParms(id, ref total, ref stop, ref title);
            total = ResetTotalTime(id, list, total);

            LotteryModel lot = new LotteryModel()
            {
                LotteryType = id,
                TotalS = total,
                StopTime = stop,
                Title = title,
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
            int pageindex = WebHelper.GetFormInt("page");
            MD_LotteryList list = LotteryList.GetLotteryByType(type, pageindex, 20, WorkContext.Uid);
            int total = 0;
            int stop = 0;
            string title = string.Empty;
            InitParms(type, ref total, ref stop, ref title);
            total = ResetTotalTime(type, list, total);
            LotteryModel lot = new LotteryModel()
            {
                LotteryType = type,
                TotalS = total,
                StopTime = stop,
                PageModel = new PageModel(20, pageindex, list.TotalCount),
                lotterylist = list
            };
            return View(lot);
        }
        /// <summary>
        /// LHB获取数据
        /// </summary>
        /// <returns></returns>
        public ActionResult _ContentLHB()
        {
            int type = WebHelper.GetFormInt("type");
            int pageindex = WebHelper.GetFormInt("page");
            MD_LotteryList list = LotteryList.GetLotteryByType(type, pageindex, 20, WorkContext.Uid);
            int total = 0;
            int stop = 0;
            string title = string.Empty;
            InitParms(type, ref total, ref stop, ref title);
            total = ResetTotalTime(type, list, total);
            LotteryModel lot = new LotteryModel()
            {
                LotteryType = type,
                TotalS = total,
                StopTime = stop,
                PageModel = new PageModel(20, pageindex, list.TotalCount),
                lotterylist = list
            };
            return View(lot);
        }
        /// <summary>
        /// pk获取数据
        /// </summary>
        /// <returns></returns>
        public ActionResult _ContentPK()
        {
            int type = WebHelper.GetFormInt("type");
            int pageindex = WebHelper.GetFormInt("page");
            MD_LotteryList list = LotteryList.GetLotteryByType(type, pageindex, 20, WorkContext.Uid);
            int total = 0;
            int stop = 0;
            string title = string.Empty;
            InitParms(type, ref total, ref stop, ref title);
            total = ResetTotalTime(type, list, total);
            LotteryModel lot = new LotteryModel()
            {
                LotteryType = type,
                TotalS = total,
                StopTime = stop,
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

        #region 投注
        /// <summary>
        /// 投注
        /// </summary>
        /// <returns></returns>
        public ActionResult _BettPage()
        {
            int type = WebHelper.GetFormInt("type");
            string expect = WebHelper.GetFormString("expect");
            DataSet ds = LotteryList.GetLotSetList(type.ToString());
            ViewData["ltset"] = ds;
            ViewData["exists"] = NewUser.ExistsMode(WorkContext.Uid, type);
            ViewData["lotterytype"] = type;
            ViewData["expect"] = expect;
            List<MD_BettMode> model = NewUser.GetModeList(1, 20, " where a.uid=" + WorkContext.Uid.ToString() + " and a.lotterytype=" + type.ToString());
            return View(model);
        }
        public ActionResult _BettPageLHC()
        {
            int type = WebHelper.GetFormInt("type");
            string expect = WebHelper.GetFormString("expect");
            DataSet ds = LotteryList.GetLotSetList(type.ToString(),"",true);
            ViewData["ltset"] = ds;
            ViewData["exists"] = NewUser.ExistsMode(WorkContext.Uid, type);
            ViewData["lotterytype"] = type;
            ViewData["expect"] = expect;
            List<MD_BettMode> model = NewUser.GetModeList(1, 20, " where a.uid=" + WorkContext.Uid.ToString() + " and a.lotterytype=" + type.ToString());
            return View(model);
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
            if (bett.Money < 10)
            {
                return Content("4");
            }
            string result = Lottery.AddNewBett(bett);
            if (result.EndsWith("成功"))
                return Content("1");
            else
                return Content(result);
        }
        #endregion

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
        /// 游戏规则
        /// </summary>
        /// <returns></returns>
        public ActionResult _LTRule()
        {
            int type = WebHelper.GetFormInt("type");
            ViewData["ltruletype"] = type;
            return View();
        }
        #region 投注记录
        /// <summary>
        /// 投注记录
        /// </summary>
        /// <returns></returns>
        public ActionResult _BettRecord()
        {
            int type = WebHelper.GetFormInt("type");
            int pageindex = WebHelper.GetFormInt("page");
            int pagesize = 20;
            DataTable dt = LotteryList.GetUserBett(type, WorkContext.Uid, pageindex, pagesize);
            LotteryRecord record = new LotteryRecord()
            {
                LotteryType=type,
                PageModel = new PageModel(20, pageindex, dt.Rows.Count > 0 ? int.Parse(dt.Rows[0]["totalcount"].ToString()) : 0),
                Records = dt
            };
            return View(record);
        }
        /// <summary>
        /// 投注详情
        /// </summary>
        /// <returns></returns>
        public ActionResult _BettDetails()
        {
            int type = WebHelper.GetFormInt("type");
            int betid = WebHelper.GetFormInt("bettid");
            DataSet ds = LotteryList.GetLotSetList(type.ToString());
            ViewData["ltset"] = ds;
            DataTable dt = LotteryList.GetUserBett(type, WorkContext.Uid, 1, 1, " where a.bettid=" + betid.ToString());
            ViewData["bett"] = dt;
            return View();
        }

         /// <summary>
        /// 投注详情
        /// </summary>
        /// <returns></returns>
        public ActionResult _BettDetailsLHC()
        {
            int type = WebHelper.GetFormInt("type");
            int betid = WebHelper.GetFormInt("bettid");
            DataSet ds = LotteryList.GetLotSetList(type.ToString(),"",true);
            ViewData["ltset"] = ds;
            DataTable dt = LotteryList.GetUserBett(type, WorkContext.Uid, 1, 1, " where a.bettid=" + betid.ToString());
            ViewData["bett"] = dt;
            return View();
        }
        
        #endregion

        /// <summary>
        /// 添加投注模式
        /// </summary>
        /// <returns></returns>
        public ActionResult AddModeFRecord()
        {
            int betid = WebHelper.GetFormInt("bettid");
            string name = WebHelper.GetFormString("name");
            string result = NewUser.AddModeFromRecord(name, betid);
            if (result.EndsWith("成功"))
                return Content("1");
            else
                return Content(result);
        }
       
        #region 模式
        /// <summary>
        /// 投注模式
        /// </summary>
        /// <returns></returns>
        public ActionResult _BettMode()
        {
            int type = WebHelper.GetFormInt("type");

            DataSet ds = LotteryList.GetLotSetList(type.ToString());
            ViewData["ltset"] = ds;
            ViewData["lotterytype"] = type;
            List<MD_BettMode> model = NewUser.GetModeList(1, 20, " where a.uid=" + WorkContext.Uid.ToString() + " and a.lotterytype=" + type.ToString());
            if (type == 9)
            {
                Dictionary<string, int> dic = new Dictionary<string, int>();
                dic["大"] = 1; dic["小"] = 2; dic["单"] = 3; dic["双"] = 4; dic["极大"] = 5; dic["大单"] = 6;
                dic["小单"] = 7; dic["大双"] = 8; dic["小双"] = 9; dic["极小"] = 10; dic["龙"] = 11; dic["虎"] = 12;
                dic["豹"] = 13;
                model.ForEach((x) =>
                {
                    foreach (KeyValuePair<string, int> item in dic)
                    {
                        x.Bettinfo = x.Bettinfo.Replace(item.Key, item.Value.ToString());
                    }
                });
            }
            return View(model);
        }

        
        public ActionResult _BettModeLHC()
        {
            int type = WebHelper.GetFormInt("type");

            DataSet ds = LotteryList.GetLotSetList(type.ToString());
            ViewData["ltset"] = ds;
            ViewData["lotterytype"] = type;
            List<MD_BettMode> model = NewUser.GetModeList(1, 20, " where a.uid=" + WorkContext.Uid.ToString() + " and a.lotterytype=" + type.ToString());
            if (type == 9)
            {
                Dictionary<string, int> dic = new Dictionary<string, int>();
                dic["大"] = 1; dic["小"] = 2; dic["单"] = 3; dic["双"] = 4; dic["极大"] = 5; dic["大单"] = 6;
                dic["小单"] = 7; dic["大双"] = 8; dic["小双"] = 9; dic["极小"] = 10; dic["龙"] = 11; dic["虎"] = 12;
                dic["豹"] = 13;
                model.ForEach((x) =>
                {
                    foreach (KeyValuePair<string, int> item in dic)
                    {
                        x.Bettinfo = x.Bettinfo.Replace(item.Key, item.Value.ToString());
                    }
                });
            }
            return View(model);
        }
        #endregion

        public ActionResult GetProvBettInfo(int type = 0,string lotterynum="")
        {
            var condition = "";
            if (!string.IsNullOrEmpty(lotterynum))
            {
                condition = " where a.lotterynum='" + lotterynum + "' ";
            }
            var list = LotteryList.GetUserBett(type, WorkContext.Uid, 1, 2, condition); 
            string btjson = JsonConvert.SerializeObject(list);
            return Content(btjson);
        }

        /// <summary>
        /// 添加投注模式
        /// </summary>
        /// <returns></returns>
        public ActionResult AddBettMode()
        {
            MD_BettMode btmode = new MD_BettMode
            {
                LotteryType = WebHelper.GetFormInt("lotterytype"),
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
        /// <summary>
        /// 自动投注
        /// </summary>
        /// <returns></returns>
        public ActionResult _AutoBett()
        {
            int type = WebHelper.GetFormInt("type");
            ViewData["lotterytype"] = type;

            DataSet ds = LotteryList.GetUserAtBett(WorkContext.Uid, type);
            ds.Tables[0].TableName = "auto";
            ds.Tables[1].TableName = "btmd";
            string json = "-1";
            if (ds.Tables[0].Rows.Count > 0)
                json = JsonConvert.SerializeObject(ds);

            //DataSet dsmode = LotteryList.GetUserAtBett(WorkContext.Uid, type);
            DataTable dtbase = ds.Tables[1];
            DataTable dt = LotteryList.NewestLottery(type.ToString());
            DataTable dtauto = ds.Tables[0];

            StringBuilder strb = new StringBuilder();
            strb.Append("{");
            strb.Append("\"base\":" + JsonConvert.SerializeObject(dtbase));
            strb.Append(",\"fcinfo\":" + JsonConvert.SerializeObject(dt));
            strb.Append(",\"autoinfo\":" + JsonConvert.SerializeObject(dtauto));
            strb.Append("}");
            string result = strb.ToString();

            ViewData["auto"] = json;
            ViewData["add"] = result.ToString();
            //List<MD_BettMode> model = NewUser.GetModeList(1, 20, " where a.uid=" + WorkContext.Uid.ToString() + " and a.lotterytype="+ type.ToString());
            return View();
        }
        /// <summary>
        /// 获取用户自动投注
        /// </summary>
        /// <returns></returns>
        private  ActionResult GetUserBett()
        {
            int type = WebHelper.GetFormInt("type");
            ViewData["lotterytype"] = type;
            DataSet ds = LotteryList.GetUserAtBett(WorkContext.Uid, type);
            ds.Tables[0].TableName = "auto";
            ds.Tables[1].TableName = "btmd";
            string json = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
                json = JsonConvert.SerializeObject(ds);

            return Content(json);
        }
        /// <summary>
        /// 获取启动投注基础信息
        /// </summary>
        /// <returns></returns>
        private ActionResult GetUserBettMode()
        {
            int type = WebHelper.GetFormInt("type");

            ViewData["lotterytype"] = type;
            DataSet ds = LotteryList.GetUserAtBett(WorkContext.Uid, type);
            DataTable dtbase = ds.Tables[1];
            DataTable dt = LotteryList.NewestLottery(type.ToString());
            DataTable dtauto = ds.Tables[0];

            StringBuilder strb = new StringBuilder();
            strb.Append("{");
            strb.Append("\"base\":" + JsonConvert.SerializeObject(dtbase));
            strb.Append(",\"fcinfo\":" + JsonConvert.SerializeObject(dt));
            strb.Append(",\"autoinfo\":" + JsonConvert.SerializeObject(dtauto));
            strb.Append("}");
            string result = strb.ToString();
            return Content(result);
        }
        /// <summary>
        /// 停止自动投注
        /// </summary>
        /// <returns></returns>
        public ActionResult StopAutoBett()
        {
            int type = WebHelper.GetFormInt("type");
            bool result = LotteryList.StopAutoBett(WorkContext.Uid, type);
            if (result)
                return Content("1");
            else
                return Content("0");
        }
        /// <summary>
        /// 启动自动投注
        /// </summary>
        /// <returns></returns>
        public ActionResult AddAutoBett()
        {
            MD_AutoBett bett = new MD_AutoBett
            {
                Uid = WorkContext.Uid,
                LotteryId = WebHelper.GetFormInt("type"),
                SelModeId = WebHelper.GetFormInt("startmd"),
                StartExpect = WebHelper.GetFormString("nowfcnum"),
                MaxBettNum = WebHelper.GetFormInt("maxbetnum"),
                MinGold = WebHelper.GetFormInt("mingold"),
                AllSelMode = WebHelper.GetFormString("allselmd")
            };
            string msg = LotteryList.AddAutoBett(bett);
            StringBuilder strb = new StringBuilder();
            if (msg.EndsWith("成功"))
            {
                strb.Append("{\"Result\":true,\"Msg\":\"成功\"}");
            }
            else
            {
                strb.Append("{\"Result\":false,\"Msg\":\"" + msg + "\"}");
            }

            return Content(strb.ToString());
        }
        /// <summary>
        /// 自动投注规则
        /// </summary>
        /// <returns></returns>
        public ActionResult _AutoRule()
        {
            return View();
        }
    }
}
