using OWZX.Core;
using OWZX.Model;
using OWZX.Services;
using OWZX.Web.Admin.Models;
using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OWZX.Web.Admin.Controllers
{
    public class BaseInfoController : BaseAdminController
    {
        #region 基本资料
        /// <summary>
        /// 获取基本资料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BaseInfoList(int baseid = -1)
        {
            List<BaseInfoModel> listbase = AdminBaseInfo.GetBaseInfoList(baseid);
            ShopUtils.SetAdminRefererCookie(Url.Action("baseinfolist"));
            return View(listbase);
        }
        /// <summary>
        /// 编辑基本资料
        /// </summary>
        /// <param name="baseid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditBaseInfo(int baseid = -1)
        {
            List<BaseInfoModel> listbase = AdminBaseInfo.GetBaseInfoList(baseid);
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            if (listbase.Count == 0)
                return View(new BaseInfoModel());
            BaseInfoModel baseinfo = listbase[0];
            return View(baseinfo);
        }
        /// <summary>
        /// 修改基础信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditBaseInfo(BaseInfoModel model)
        {
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            bool result = AdminBaseInfo.UpdateBaseInfo(model.BaseId, model.Title, model.Content);
            if (result)
                return PromptView("保存成功");
            else
                return PromptView("保存失败");
        }
        #endregion

        #region 海报
        /// <summary>
        /// 获取海报信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PosterBaseInfo()
        {
            List<BaseInfoModel> listbase = AdminBaseInfo.GetBaseInfoList(4);
            BaseInfoModel baseinfo = listbase[0];
            return View(baseinfo);
        }

        /// <summary>
        /// 获取海报信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PosterBaseInfo(BaseInfoModel baseinfo)
        {
            List<BaseInfoModel> listbase = AdminBaseInfo.GetBaseInfoList(4);

            BaseInfoModel hbinfo = listbase[0];
            hbinfo.Content = baseinfo.Content;
            bool result = AdminBaseInfo.UpdateBaseInfo(hbinfo.BaseId, hbinfo.Title, hbinfo.Content);
            if (result)
                return PromptView("保存成功");
            else
                return PromptView("保存失败");
        }
        #endregion

        #region 基础类型
        /// <summary>
        /// 获取基础类型
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult BaseTypeList(int id = -1)
        {
            string condition = "";
            List<BaseTypeModel> listbase = AdminBaseInfo.GetBaseTypeList(condition);
            BaseTypeListModel basetypelist = new BaseTypeListModel() { basetypelist = listbase };
            ViewData["parentid"] = id;
            return View(basetypelist);
        }

        /// <summary>
        /// 获取基础类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BaseTypeDetails(int id)
        {
            List<BaseTypeModel> listbase = AdminBaseInfo.GetBaseTypeList(" where parentid=" + id);
            BaseTypeListModel basetypelist = new BaseTypeListModel() { basetypelist = listbase };
            return View(basetypelist);
        }
        [HttpGet]
        /// <summary>
        /// 添加基础类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddBaseType(int id = -1)
        {
            Load(id);
            BaseTypeModel basetype = new BaseTypeModel();
            basetype.Parentid = id;
            return View(basetype);
        }
        [HttpPost]
        /// <summary>
        /// 添加基础类型
        /// </summary>
        /// <param name="basetype"></param>
        /// <returns></returns>
        public ActionResult AddBaseType(BaseTypeModel basetype, int parentid = -1)
        {
            if (AdminBaseInfo.GetBaseTypeByParentId(basetype.Parentid, basetype.Type))
                ModelState.AddModelError("Type", "名称已经存在");

            if (ModelState.IsValid)
            {
                bool result = AdminBaseInfo.AddBaseType(basetype);
                if (result)
                    return PromptView("添加成功");
                else
                    return PromptView("添加失败");
            }
            Load(basetype.Parentid);
            return View(basetype);
        }
        [HttpGet]
        /// <summary>
        /// 修改基础类型
        /// </summary>
        /// <param name="basetype"></param>
        /// <returns></returns>
        public ActionResult EditBaseType(int systypeid)
        {
            BaseTypeModel basetype = AdminBaseInfo.GetBaseTypeList(" where systypeid=" + systypeid)[0];
            Load(basetype.Parentid == 0 ? basetype.Systypeid : basetype.Parentid);
            return View(basetype);
        }
        [HttpPost]
        /// <summary>
        /// 修改基础类型
        /// </summary>
        /// <param name="basetype"></param>
        /// <returns></returns>
        public ActionResult EditBaseType(BaseTypeModel basetype)
        {
            if (AdminBaseInfo.GetBaseTypeByParentId(basetype.Parentid, basetype.Type, basetype.Systypeid))
                ModelState.AddModelError("Type", "名称已经存在");

            if (ModelState.IsValid)
            {
                bool result = AdminBaseInfo.UpdateBaseType(basetype);
                if (result)
                    return PromptView("修改成功");
                else
                    return PromptView("修改失败");
            }
            Load(basetype.Parentid == 0 ? basetype.Systypeid : basetype.Parentid);
            return View(basetype);
        }
        /// <summary>
        /// 删除基础类型
        /// </summary>
        /// <param name="basetype"></param>
        /// <returns></returns>
        public ActionResult DeleteBaseType(int systypeid)
        {
            Load(systypeid);
            if (AdminBaseInfo.GetBaseTypeList(" where  parentid=" + systypeid).Count > 0)
                return PromptView("删除失败,请删除此类型下的子类型");
            bool result = AdminBaseInfo.DeleteBaseType(systypeid);
            if (result)
                return PromptView("删除成功");
            else
                return PromptView("删除失败");
        }

        private void Load(int parentid = -1)
        {
            ShopUtils.SetAdminRefererCookie(string.Format("{0}/" + parentid, Url.Action("basetypelist")));

            ViewData["baseTypeList"] = AdminBaseInfo.GetBaseTypeList();
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
        }
        #endregion

        #region 房间信息
        /// <summary>
        /// 获取房间信息
        /// </summary>
        /// <returns></returns>
        public ActionResult RoomList(int roomid = -1)
        {
            StringBuilder strb = new StringBuilder();
            if (roomid > 0)
                strb.Append(" where a.roomid=" + roomid);
            List<MD_LotteryRoom> listbase = Lottery.GetRoomList(1, -1, strb.ToString());
            ShopUtils.SetAdminRefererCookie(Url.Action("roomlist"));
            return View(listbase);
        }
        /// <summary>
        /// 添加房间信息
        /// </summary>
        /// <param name="baseid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddRoom()
        {
            LoadRoom();
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            MD_LotteryRoom baseinfo = new MD_LotteryRoom { Lotterytype = -1, Room = -1, Maxuser = 0 };
            return View(baseinfo);
        }

        /// <summary>
        /// 添加房间信息
        /// </summary>
        /// <param name="baseid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRoom(MD_LotteryRoom roominfo)
        {
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            //接收参数名称不能与提交的参数名相同（不区分大小写）
            if (ModelState.IsValid)
            {
                bool result = Lottery.AddRoom(roominfo);
                if (result)
                    return PromptView("保存成功");
                else
                    return PromptView("保存失败");
            }
            return View(roominfo);
        }
        /// <summary>
        /// 编辑房间信息
        /// </summary>
        /// <param name="baseid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditRoom(int roomid = -1)
        {
            StringBuilder strb = new StringBuilder();
            if (roomid > 0)
                strb.Append(" where a.roomid=" + roomid);
            List<MD_LotteryRoom> listbase = Lottery.GetRoomList(1, -1, strb.ToString());
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            if (listbase.Count == 0)
                return View(new MD_LotteryRoom());
            MD_LotteryRoom baseinfo = listbase[0];
            LoadRoom();
            return View(baseinfo);
        }
        /// <summary>
        /// 修改房间信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditRoom(MD_LotteryRoom ltmd)
        {
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            bool result = Lottery.UpdateRoom(ltmd);
            if (result)
                return PromptView("保存成功");
            else
                return PromptView("保存失败");
        }

        public void LoadRoom()
        {
            List<SelectListItem> lotterylist = new List<SelectListItem>();
            lotterylist.Add(new SelectListItem() { Text = "请选择", Value = "-1" });
            foreach (BaseTypeModel info in AdminBaseInfo.GetBaseTypeList("where outtypeid=47"))
            {
                lotterylist.Add(new SelectListItem() { Text = info.Type, Value = info.Systypeid.ToString() });
            }
            ViewData["Lotterytype"] = lotterylist;


            List<SelectListItem> roomlist = new List<SelectListItem>();
            roomlist.Add(new SelectListItem() { Text = "请选择", Value = "-1" });
            foreach (BaseTypeModel info in AdminBaseInfo.GetBaseTypeList("where parentid=19"))
            {
                roomlist.Add(new SelectListItem() { Text = info.Type, Value = info.Systypeid.ToString() });
            }
            ViewData["Room"] = roomlist;
            List<SelectListItem> bettlist = new List<SelectListItem>();
            bettlist.Add(new SelectListItem() { Text = "请选择", Value = "-1" });
            foreach (BaseTypeModel info in AdminBaseInfo.GetBaseTypeList("where parentid=29"))
            {
                bettlist.Add(new SelectListItem() { Text = info.Type, Value = info.Systypeid.ToString() });
            }
            ViewData["Bett"] = bettlist;
        }
        #endregion

        #region 回水规则信息
        /// <summary>
        /// 获取回水规则
        /// </summary>
        /// <returns></returns>
        public ActionResult BackRuleList(int ruleid = -1)
        {
            StringBuilder strb = new StringBuilder();
            if (ruleid > 0)
                strb.Append(" where a.rateid=" + ruleid);
            List<MD_BackRate> listbase = Lottery.GetRateRuleList(1, -1, strb.ToString());
            ShopUtils.SetAdminRefererCookie(Url.Action("backrulelist"));


            return View(listbase);
        }
        /// <summary>
        /// 添加回水规则
        /// </summary>
        /// <param name="baseid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddBackRule()
        {
            LoadRoom();
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            MD_BackRate baseinfo = new MD_BackRate { Roomtypeid = -1 };
            return View(baseinfo);
        }

        /// <summary>
        /// 添加回水规则
        /// </summary>
        /// <param name="baseid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddBackRule(MD_BackRate ruleinfo)
        {
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            //接收参数名称不能与提交的参数名相同（不区分大小写）
            if (ModelState.IsValid)
            {
                bool result = Lottery.AddRateRule(ruleinfo);
                if (result)
                    return PromptView("保存成功");
                else
                    return PromptView("保存失败");
            }
            return View(ruleinfo);
        }
        /// <summary>
        /// 编辑回水规则
        /// </summary>
        /// <param name="baseid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditBackRule(int rateid = -1)
        {
            StringBuilder strb = new StringBuilder();
            if (rateid > 0)
                strb.Append(" where a.rateid=" + rateid);
            List<MD_BackRate> listbase = Lottery.GetRateRuleList(1, -1, strb.ToString());
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            if (listbase.Count == 0)
                return View(new MD_LotteryRoom());
            MD_BackRate baseinfo = listbase[0];
            LoadRoom();
            return View(baseinfo);
        }
        /// <summary>
        /// 修改回水规则
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditBackRule(MD_BackRate ltmd)
        {
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            bool result = Lottery.UpdateRateRule(ltmd);
            if (result)
                return PromptView("保存成功");
            else
                return PromptView("保存失败");
        }

        #endregion

        #region 赔率
        public ActionResult LotterySetList(int type = -1, int bttype = -1, int roomtype = -1, int pageSize = 15, int pageNumber = 1)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append("where 1=1");
            if (type > 0)
                strb.Append(" and e.systypeid=" + type);
            if (bttype > 0)
                strb.Append(" and a.type=" + bttype);
            roomtype = 20;
            strb.Append(" and a.roomtype= " + roomtype);
            List<MD_LotterySet> listbase = Lottery.GetLotterySetList(pageNumber, pageSize, strb.ToString());

            LotterySets list = new LotterySets
            {
                type = type,
                roomtype = roomtype, 
                bttype = bttype,
                PageModel = new PageModel(pageSize, pageNumber, listbase.Count > 0 ? listbase[0].TotalCount : 0),
                SetList = listbase
            };
            return View(list);
        }

        /// <summary>
        /// 编辑回水规则
        /// </summary>
        /// <param name="baseid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditLotterySet(int bttypeid = -1)
        {
            StringBuilder strb = new StringBuilder();
            if (bttypeid > 0)
                strb.Append(" where a.bttypeid=" + bttypeid);
            strb.Append(" and a.roomtype=20 ");
            List<MD_LotterySet> listbase = Lottery.GetLotterySetList(1, 1, strb.ToString()); ;
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            if (listbase.Count == 0)
                return View(new MD_LotterySet());
            MD_LotterySet baseinfo = listbase[0];
            baseinfo.Odds = baseinfo.Odds.Replace("1:", "");
            LoadRoom();
            return View(baseinfo);
        }
        /// <summary>
        /// 修改回水规则
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditLotterySet(MD_LotterySet ltmd)
        {
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            bool result = Lottery.UpdateLotterySet(ltmd);
            if (result)
                return PromptView("保存成功");
            else
                return PromptView("保存失败");
        }
        #endregion
    }
}
