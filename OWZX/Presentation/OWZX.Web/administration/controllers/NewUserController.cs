using System;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.Collections.Generic;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.Web.Admin.Models;
using System.Text;
using OWZX.Core.Helper;
using OWZX.Model;

namespace OWZX.Web.Admin.Controllers
{
    /// <summary>
    /// 后台用户控制器类
    /// </summary>
    public partial class NewUserController : BaseAdminController
    {
        #region 用户回水
        /// <summary>
        /// 用户回水
        /// </summary>
        public ActionResult BackList(string Account = "", int pageSize = 15, int pageNumber = 1)
        {
            ShopUtils.SetAdminRefererCookie(Url.Action("backlist"));
            string where = string.Empty;
            if (Account != string.Empty)
                where = " where rtrim(b.mobile)='" + Account + "'";
            List<MD_UserBack> backlist = NewUser.GetBackList(pageNumber, pageSize, where);
            UserBackList model = new UserBackList()
            {
                Account = Account,
                BackList = backlist,
                PageModel = new PageModel(pageSize, pageNumber, backlist.Count > 0 ? backlist[0].TotalCount : 0)
            };

            return View(model);
        }

        /// <summary>
        /// 结算回水
        /// </summary>
        /// <param name="backid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ActionResult EditBack(int backid, string status)
        {
            List<MD_UserBack> list = NewUser.GetBackList(1, -1, " where a.backid=" + backid);
            if (list.Count == 0)
            {
                return PromptView("用户回水不存在");
            }
            MD_UserBack bk = list[0];
            bk.Status = short.Parse(status); bk.Updateuid = WorkContext.Uid;
            bool result = NewUser.UpdateUserBack(bk);
            if (result)
                return PromptView("更新成功");
            else
                return PromptView("更新失败");
        }
        /// <summary>
        ///删除回水
        /// </summary>
        /// <param name="backid"></param>
        /// <returns></returns>
        public ActionResult DelBack(int backid)
        {
            List<MD_UserBack> list = NewUser.GetBackList(1, -1, " where a.backid=" + backid);
            if (list.Count == 0)
            {
                return PromptView("用户回水不存在");
            }
            bool result = NewUser.DeleteUserBack(backid.ToString());
            if (result)
                return PromptView("删除成功");
            else
                return PromptView("删除失败");
        }
        #endregion

        #region 用户充值记录
        /// <summary>
        /// 用户充值记录
        /// </summary>
        public ActionResult RemitList(string Account = "", string type = "", int pageSize = 15, int pageNumber = 1)
        {
            ShopUtils.SetAdminRefererCookie(Url.Action("remitlist"));
            StringBuilder strb = new StringBuilder();
            strb.Append(" where 1=1");
            if (Account != string.Empty)
                strb.Append(" and rtrim(b.mobile)='" + Account + "'");
            if (type != string.Empty)
                strb.Append(" and a.type='" + type + "'");
            List<MD_Remit> remitlist = NewUser.GetUserRemitList(pageNumber, pageSize, strb.ToString());
            UserRemitList model = new UserRemitList()
            {
                Account = Account,
                type = type,
                RemitList = remitlist,
                PageModel = new PageModel(pageSize, pageNumber, remitlist.Count > 0 ? remitlist[0].TotalCount : 0)
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult AddRemit()
        {
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            UserRemit remit = new UserRemit
            {
                Type = "人工充值",
                Name = "系统充值",
                Account = "系统充值"
            };
            return View(remit);
        }
        [HttpPost]
        public ActionResult AddRemit(UserRemit remit)
        {
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            if (!Users.IsExistMobile(remit.Mobile))
            {
                return PromptView("账号不存在");
            }
            MD_Remit rmt = new MD_Remit
            {
                Mobile = remit.Mobile,
                Type = "人工充值",
                Name = "系统充值",
                Account = "系统充值",
                Money = remit.Money,
                Status = 0
            };
            bool addres = NewUser.AddUserRemit(rmt);
            if (addres)
            {
                return PromptView("添加成功");
            }
            else
            {
                return PromptView("添加失败");
            }
        }
        /// <summary>
        /// 处理充值记录 更新实际金额
        /// </summary>
        /// <param name="remitid"></param>
        /// <param name="realmoney"></param>
        /// <returns></returns>
        public ActionResult EditRemitMoney(int remitid, int realmoney)
        {
            List<MD_Remit> list = NewUser.GetUserRemitList(1, -1, " where a.remitid=" + remitid);
            if (list.Count == 0)
            {
                return PromptView("用户转账记录不存在");
            }
            MD_Remit bk = list[0];
            bk.RealMoney = realmoney;
            bk.Updateuid = WorkContext.Uid;
            bool result = NewUser.UpdateUserRemit(bk);
            if (result)
                return Content("1");
            else
                return Content("0");
        }
        /// <summary>
        /// 处理充值记录 记录异常信息
        /// </summary>
        /// <param name="remitid"></param>
        /// <param name="realmoney"></param>
        /// <returns></returns>
        public ActionResult EditRemitRemark(int remitid, string remark)
        {
            List<MD_Remit> list = NewUser.GetUserRemitList(1, -1, " where a.remitid=" + remitid);
            if (list.Count == 0)
            {
                return PromptView("用户转账记录不存在");
            }
            MD_Remit bk = list[0];
            bk.Remark = remark;
            bk.Updateuid = WorkContext.Uid;
            bool result = NewUser.UpdateUserRemit(bk);
            if (result)
                return Content("1");
            else
                return Content("0");
        }

        /// <summary>
        /// 处理充值记录
        /// </summary>
        /// <param name="remitid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ActionResult EditRemit(int remitid, short status, string remark = "")
        {
            List<MD_Remit> list = NewUser.GetUserRemitList(1, -1, " where a.remitid=" + remitid);
            if (list.Count == 0)
            {
                return PromptView("用户转账记录不存在");
            }
            MD_Remit bk = list[0];

            if (status == 1)
            {
                bk.Status = 1;
            }
            else if (status == 2)
            {
                if (bk.RealMoney == 0)
                    return PromptView("请输入实际转账金额");
                if (bk.Remark != "" && bk.Remark != null)
                    return PromptView("已输入失败原因,不能完成转账");
                bk.Status = 2;
            }
            else if (status == 3)
            {
                if (bk.Remark == "")
                    return PromptView("请输入失败原因");
                bk.Remark = remark;
                bk.Status = 3;
            }

            bk.Updateuid = WorkContext.Uid;
            bool result = NewUser.UpdateUserRemit(bk);
            if (result)
                return PromptView("更新成功");
            else
                return PromptView("更新失败");
        }
        /// <summary>
        ///删除转账记录
        /// </summary>
        /// <param name="remitid"></param>
        /// <returns></returns>
        public ActionResult DelRemit(int remitid)
        {
            List<MD_Remit> list = NewUser.GetUserRemitList(1, -1, " where a.remitid=" + remitid);
            if (list.Count == 0)
            {
                return PromptView("用户转账记录不存在");
            }
            bool result = NewUser.DeleteUserRemit(remitid.ToString());
            if (result)
                return PromptView("删除成功");
            else
                return PromptView("删除失败");
        }
        #endregion

        #region 用户账变记录
        public ActionResult ChangeList(string Account = "", string start = "", string end = "", int pageSize = 15, int pageNumber = 1)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(" where 1=1");
            if (Account != string.Empty)
                strb.Append(" and rtrim(b.mobile)='" + Account + "'");
            if(start!=string.Empty )
                strb.Append(" and a.addtime between '" + start + "' and '"+end+"'");

            List<MD_Change> list = NewUser.GetAChangeList(pageNumber, pageSize, strb.ToString());
            UserChangeList userlist = new UserChangeList
            {
                Account = Account,
                Start = start,
                End = end,
                PageModel = new PageModel(pageSize, pageNumber, list.Count > 0 ? list[0].TotalCount : 0),
                ChangeList = list
            };
            return View(userlist);
        }
        #endregion

        #region 用户银行卡信息
        public ActionResult UserBankList(string Account = "", string UserName = "",int pageSize = 15, int pageNumber = 1)
        {
            ShopUtils.SetAdminRefererCookie(Url.Action("userbanklist"));
            StringBuilder strb = new StringBuilder();
            strb.Append(" where 1=1");
            if (Account != string.Empty)
                strb.Append(" and rtrim(b.mobile)='" + Account + "'");
            if (UserName != string.Empty)
                strb.Append(" and a.username = '" + UserName + "'");

            List<MD_DrawAccount> list = Recharge.GetDrawAccountList(pageNumber, pageSize, strb.ToString());
            UserBankListModel userlist = new UserBankListModel
            {
                Mobile = Account,
                UserName = UserName,
                PageModel = new PageModel(pageSize, pageNumber, list.Count > 0 ? list[0].TotalCount : 0),
                BankList = list
            };
            return View(userlist);
        }
        [HttpGet]
        public ActionResult EditBank(int id = -1)
        {
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            List<MD_DrawAccount> list = Recharge.GetDrawAccountList(1, 1, " where a.drawaccid=" + id);
            if (list.Count > 0)
            {
                MD_DrawAccount draw = list[0];
                return View(draw);
            }
            else
            {
                return PromptView("获取用户银行卡信息失败");
            }
        }
        [HttpPost]
        public ActionResult EditBank(MD_DrawAccount model)
        {
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            List<MD_DrawAccount> list = Recharge.GetDrawAccountList(1, 1, " where  rtrim(b.mobile)='" + model.Account+"'");
            if (list.Count > 0)
            {
                MD_DrawAccount draw = list[0];
                draw.Username = model.Username;
                bool result = Recharge.UpdateDrawCardInfo(draw);
                if (result)
                    return PromptView("更新成功");
                else
                    return PromptView("更新失败");
            }
            else
            {
                return PromptView("获取用户银行卡信息失败");
            }
        }

        public ActionResult DelBank(int id=-1)
        {
            bool result = Recharge.DeleteDrawAccount(id.ToString());
            if (result)
                return PromptView("删除成功");
            else
                return PromptView("删除失败");
        }
        #endregion
    }
}
