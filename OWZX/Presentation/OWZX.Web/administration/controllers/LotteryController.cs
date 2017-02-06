using System;
using System.Web;
using System.Web.Mvc;
using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.Web.Admin.Models;
using System.Text;
using System.Collections.Generic;
using OWZX.Model;
using System.Data;

namespace OWZX.Web.Admin.Controllers
{
    /// <summary>
    /// 后台彩票控制器类
    /// </summary>
    public partial class LotteryController : BaseAdminController
    {
        /// <summary>
        /// 投注记录
        /// </summary>
        public ActionResult LotteryList(string account = "", int lottype = -1, string expect = "", int roomtype = -1, int vip = -1, int bttype = -1, int pageSize = 15, int pageNumber = 1)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(" where 1=1");
            if (account != "")
                strb.Append(" and rtrim(b.mobile)='" + account + "'");
            if (lottype != -1)
                strb.Append(" and a.lotteryid=" + lottype);
            if (expect != "")
                strb.Append(" and a.lotterynum='" + expect + "'");
            if (roomtype != -1)
                strb.Append(" and a.roomid=" + roomtype);
            if (vip != -1)
                strb.Append(" and a.vipid=" + vip);
            if (bttype != -1)
                strb.Append(" and c.type=" + bttype);

            List<MD_Bett> btlist = Lottery.GetBettList(pageNumber, pageSize, strb.ToString());
            LotteryListModel list = new LotteryListModel
            {
                account = account,
                lottype = lottype,
                expect = expect,
                roomtype = roomtype,
                vip = vip,
                bttype = bttype,
                PageModel = new PageModel(pageSize, pageNumber, btlist.Count > 0 ? btlist[0].TotalCount : 0),
                bettList = btlist
            };
            return View(list);
        }

        #region 财务报表
        public ActionResult ProfitList(int lottery = -1, string type = "每日盈亏", string start = "", string end = "", int pageSize = 15, int pageNumber = 1)
        {
            Load();
            StringBuilder strb = new StringBuilder();
            strb.Append(" where 1=1");
            if (lottery > 0)
                strb.Append(" and type=" + lottery);
            if (type != string.Empty && end != "")
                strb.Append(" and convert(varchar(10),b.opentime,120) between " + start+" and "+end);
            DataTable dt = Lottery.GetProfitListNoLottery(type, pageSize, pageNumber, strb.ToString());

            ProfitStatList list = new ProfitStatList
            {
                Lottery = lottery,
                Type = type,
                Start = start,
                End = end,
                ProfitList = dt,
                PageModel = new PageModel(pageSize, pageNumber, dt.Rows.Count > 0 ? int.Parse(dt.Rows[0]["TotalCount"].ToString()) : 0)
            };
            return View(list);
        }

        private void Load()
        {
            List<SelectListItem> statlist = new List<SelectListItem>();
            SelectListItem[] items = new SelectListItem[] {
                new SelectListItem() { Text = "每日盈亏", Value = "每日盈亏" } ,
                new SelectListItem() { Text = "每周盈亏", Value = "每周盈亏" } ,
                new SelectListItem() { Text = "每月盈亏", Value = "每月盈亏" } 
            };
            statlist.AddRange(items);

            ViewData["statlist"] = statlist;
        }
        #endregion

    }
}
