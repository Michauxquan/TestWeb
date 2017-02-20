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
    public partial class UserController : BaseAdminController
    {
        #region 用户列表
        /// <summary>
        /// 用户列表
        /// </summary>
        private ActionResult List(string userName, string email, string mobile, int userRid = 0, int adminGid = 0, int pageNumber = 1, int pageSize = 15)
        {
            string condition = AdminUsers.AdminGetUserListCondition(userName, email, mobile, userRid, adminGid);
            //管理员可以在后台修改自己的密码
            //if (condition != "")
            //    condition += " and owzx_users.uid<>" + WorkContext.Uid;
            //else
            //    condition = " owzx_users.uid<>" + WorkContext.Uid;

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminUsers.AdminGetUserCount(condition));

            List<SelectListItem> userRankList = new List<SelectListItem>();
            userRankList.Add(new SelectListItem() { Text = "全部等级", Value = "0" });
            foreach (UserRankInfo info in AdminUserRanks.GetUserRankList())
            {
                userRankList.Add(new SelectListItem() { Text = info.Title, Value = info.UserRid.ToString() });
            }

            List<SelectListItem> adminGroupList = new List<SelectListItem>();
            adminGroupList.Add(new SelectListItem() { Text = "全部组", Value = "0" });
            foreach (AdminGroupInfo info in AdminGroups.GetAdminGroupList())
            {
                adminGroupList.Add(new SelectListItem() { Text = info.Title, Value = info.AdminGid.ToString() });
            }

            UserListModel model = new UserListModel()
            {
                PageModel = pageModel,
                UserList = AdminUsers.AdminGetUserList(pageModel.PageSize, pageModel.PageNumber, condition),
                UserName = userName,
                Email = email,
                Mobile = mobile,
                UserRid = userRid,
                UserRankList = userRankList,
                AdminGid = adminGid,
                AdminGroupList = adminGroupList

            };

            ShopUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&userName={3}&email={4}&mobile={5}&userRid={6}&adminGid={7}",
                                                          Url.Action("list"), pageModel.PageNumber, pageModel.PageSize,
                                                          userName, email, mobile, userRid, adminGid));
            return View(model);
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        public ActionResult List(string userName = "", string mobile = "", int userrid = -1, int pageNumber = 1, int pageSize = 15)
        {
            HashSet<string> actionlist = AdminGroups.GetAdminGroupActionHashSetNoCache(WorkContext.AdminGid);
            ShopUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&userName={3}&mobile={4}&userrid={5}",
                                                          Url.Action("list"), pageNumber, pageSize,
                                                          userName, mobile, userrid));

            StringBuilder strb = new StringBuilder();
            strb.Append(" where 1=1");
            if (userName != "")
                strb.Append(" and a.nickname like '%" + userName + "%'");

            if (mobile != "")
                strb.Append(" and a.mobile='" + mobile + "'");

            strb.Append("order by a.uid desc");


            DataTable dt = AdminUsers.GetUserList(pageSize, pageNumber, strb.ToString());
            if (dt.Columns[0].ColumnName == "error")
                return PromptView("用户获取失败");

            UserListModel model = new UserListModel()
            {
                PageModel = new PageModel(pageSize, pageNumber, dt.Rows.Count),
                UserList = dt,
                UserName = userName,
                Mobile = mobile,
                UserRid = userrid
            };


            return View(model);
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        [HttpGet]
        public ActionResult Add()
        {
            UserModel model = new UserModel();
            Load(model.RegionId);
            return View(model);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        [HttpPost]
        private ActionResult Add(UserModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Password))
                ModelState.AddModelError("Password", "密码不能为空");

            if (AdminUsers.IsExistUserName(model.UserName))
                ModelState.AddModelError("UserName", "名称已经存在");

            if (AdminUsers.IsExistEmail(model.Email))
                ModelState.AddModelError("Email", "email已经存在");

            if (AdminUsers.IsExistMobile(model.Mobile))
                ModelState.AddModelError("Mobile", "手机号已经存在");

            if (ModelState.IsValid)
            {
                string salt = Users.GenerateUserSalt();
                string nickName;
                if (string.IsNullOrWhiteSpace(model.NickName))
                    nickName = "owzx" + Randoms.CreateRandomValue(7);
                else
                    nickName = model.NickName;

                UserInfo userInfo = new UserInfo()
                {
                    UserName = model.UserName,
                    Email = model.Email == null ? "" : model.Email,
                    Mobile = model.Mobile == null ? "" : model.Mobile,
                    Salt = salt,
                    Password = Users.CreateUserPassword(model.Password, salt),
                    UserRid = model.UserRid,
                    AdminGid = model.AdminGid,
                    NickName = WebHelper.HtmlEncode(nickName),
                    Avatar = model.Avatar == null ? "" : WebHelper.HtmlEncode(model.Avatar),
                    PayCredits = model.PayCredits,
                    RankCredits = AdminUserRanks.GetUserRankById(model.UserRid).CreditsLower,
                    VerifyEmail = 0,
                    VerifyMobile = 0,
                    LiftBanTime = UserRanks.IsBanUserRank(model.UserRid) ? DateTime.Now.AddDays(WorkContext.UserRankInfo.LimitDays) : new DateTime(1900, 1, 1),
                    LastVisitTime = DateTime.Now,
                    LastVisitIP = WorkContext.IP,
                    LastVisitRgId = WorkContext.RegionId,
                    RegisterTime = DateTime.Now,
                    RegisterIP = WorkContext.IP,
                    RegisterRgId = WorkContext.RegionId,
                    Gender = model.Gender,
                    RealName = model.RealName == null ? "" : WebHelper.HtmlEncode(model.RealName),
                    Bday = model.Bday ?? new DateTime(1970, 1, 1),
                    IdCard = model.IdCard == null ? "" : model.IdCard,
                    RegionId = model.RegionId,
                    Address = model.Address == null ? "" : WebHelper.HtmlEncode(model.Address),
                    Bio = model.Bio == null ? "" : WebHelper.HtmlEncode(model.Bio)
                };

                AdminUsers.CreateUser(userInfo);
                AddAdminOperateLog("添加用户", "添加用户,用户为:" + model.UserName);
                return PromptView("用户添加成功");
            }
            Load(model.RegionId);

            return View(model);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        [HttpPost]
        public ActionResult Add(string name = "")
        {
            string form = Request.Form.ToString();
            form = HttpUtility.UrlDecode(form);
            Dictionary<string, string> parms = CommonHelper.ParmsToDic(form);

            if (AdminUsers.IsExistMobile(parms["Mobile"]))
                ModelState.AddModelError("Mobile", "手机号已经存在");
            UserModel us = new UserModel();
            if (ModelState.IsValid)
            {
                string salt = Users.GenerateUserSalt();

                UserInfo userInfo = new UserInfo()
                {
                    UserName = parms["Mobile"],
                    Email = "",
                    Mobile = parms["Mobile"],
                    Salt = salt,
                    Password = Users.CreateUserPassword(parms["Password"].ToString(), salt),
                    UserRid = int.Parse(parms["UserRid"].ToString()),
                    AdminGid = int.Parse(parms["AdminGid"]),
                    NickName = parms["NickName"],
                    Avatar = "",
                    PayCredits = 0,
                    RankCredits = 0,
                    VerifyEmail = 0,
                    VerifyMobile = 0,
                    LiftBanTime = new DateTime(1900, 1, 1),
                    LastVisitTime = DateTime.Now,
                    LastVisitIP = WorkContext.IP,
                    LastVisitRgId = WorkContext.RegionId,
                    RegisterTime = DateTime.Now,
                    RegisterIP = WorkContext.IP,
                    RegisterRgId = WorkContext.RegionId,
                    Gender = 0,
                    RealName = "",
                    Bday = new DateTime(1970, 1, 1),
                    IdCard = "",
                    RegionId = -1,
                    Address = "",
                    Bio = ""
                };
                int uid = AdminUsers.CreateUser(userInfo);
                if (uid > 0)
                {
                    AddAdminOperateLog("添加用户", "添加用户,用户为:" + parms["Mobile"]);
                    return PromptView("用户添加成功");
                }
                else
                    return PromptView("用户添加失败");
            }
            else
            {
                us = new UserModel
                {
                    Mobile = parms["Mobile"],
                    NickName = parms["NickName"],
                    Password = parms["Password"],
                    ConfirmPassword = parms["ConfirmPassword"],
                    UserRid = int.Parse(parms["UserRid"])

                };
            }
            Load(-1);
            return View(us);
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        [HttpGet]
        public ActionResult Edit(int uid = -1)
        {
            UserInfo userInfo = AdminUsers.GetUserById(uid);
            if (userInfo == null)
                return PromptView("用户不存在");

            UserModel model = new UserModel();
            model.UserName = userInfo.UserName;
            model.Mobile = userInfo.Mobile;
            model.NickName = userInfo.NickName;
            model.Password = userInfo.Password;
            model.UserRid = userInfo.UserRid;
            model.AdminGid = userInfo.AdminGid;
            Load(model.RegionId);
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdAccount(int id,decimal fee)
        {
            UserInfo userInfo = AdminUsers.GetUserById(id);
            if (userInfo == null)
                return AjaxResult("success", "用户不存在"); 
            var result = Users.UpdateUserAccount(id,fee);
            if (result)
            {
                AddAdminOperateLog("修改用户账余", "修改用户账余,用户ID为:" + id);
                return AjaxResult("success","修改用户账余成功");
            }
            else
            {
                return AjaxResult("error","用户账余修改失败");
            }
        } 
        /// <summary>
        /// 编辑用户
        /// </summary>
        [HttpPost]
        public ActionResult Edit(UserModel model, int uid = -1)
        {
            UserInfo userInfo = AdminUsers.GetUserById(uid);
            if (userInfo == null)
                return PromptView("用户不存在");


            int uid4 = AdminUsers.GetUidByEmail(model.Email);
            if (uid4 > 0 && uid4 != uid)
                ModelState.AddModelError("Mobile", "邮箱已经存在");

            if (ModelState.IsValid)
            {
                string nickName;
                if (string.IsNullOrWhiteSpace(model.NickName))
                    nickName = userInfo.NickName;
                else
                    nickName = model.NickName;

                userInfo.UserName = model.Mobile;
                userInfo.Mobile = model.Mobile;
                if (!string.IsNullOrWhiteSpace(model.Password))
                    userInfo.Password = Users.CreateUserPassword(model.Password, userInfo.Salt);
                userInfo.UserRid = model.UserRid;
                userInfo.NickName = WebHelper.HtmlEncode(nickName);

                userInfo.AdminGid = model.AdminGid;
                userInfo.UserType = model.UserType;
                bool result = false;


                result = AdminUsers.UpdateUser(userInfo);
                if (result)
                {
                    AddAdminOperateLog("修改用户", "修改用户,用户ID为:" + uid);
                    return PromptView("用户修改成功");
                }
                else
                {
                    return PromptView("用户修改失败");
                }
            }

            Load(model.RegionId);

            return View(model);
        }


        /// <summary>
        /// 编辑用户提现密码
        /// </summary>
        [HttpGet]
        public ActionResult EditDraw(int uid = -1)
        {
            UserInfo userInfo = AdminUsers.GetUserById(uid);
            if (userInfo == null)
                return PromptView("用户不存在");
            Load(0);
            List<MD_DrawAccount> list = Recharge.GetDrawAccountList(1, 1, " where a.uid=" + uid);
            MD_DrawAccount draw = new MD_DrawAccount();
            if (list.Count > 0)
            {
                draw = list[0];
                draw.Drawpwd = "";
                return View(draw);
            }
            else
            {
                return PromptView("用户未绑定银行卡");
            }
        }

        /// <summary>
        /// 编辑用户提现密码
        /// </summary>
        [HttpPost]
        public ActionResult EditDraw(MD_DrawAccount model, int uid = -1)
        {
            UserInfo userInfo = AdminUsers.GetUserById(uid);
            if (userInfo == null)
                return PromptView("用户不存在");

            if (ModelState.IsValid)
            {
                model.Drawpwd = Users.CreateUserPassword(model.Drawpwd, userInfo.Salt);
                model.Account = userInfo.Mobile;
                bool result = false;

                result = Recharge.UpdateDrawPWD(model);
                if (result)
                {
                    return PromptView("提现密码修改成功");
                }
                else
                {
                    return PromptView("提现密码修改失败");
                }
            }
            Load(0);
            
            return View(model);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ActionResult Del(int uid)
        {
            bool result = Users.DelUserByUid(uid);
            if (result)
                return PromptView("用户删除成功");
            else
                return PromptView("用户删除失败");
        }
        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public FileResult ExportExcel(string username = "", string mobile = "")
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(" where 1=1");
            if (username != "")
                strb.Append(" and a.username like '" + username + "%'");

            if (mobile != "")
                strb.Append(" and a.mobile='" + mobile + "'");

            strb.Append("order by a.uid desc");


            DataTable dt = AdminUsers.GetUserList(-1, 1, strb.ToString());

            Dictionary<string, string> listcol = new Dictionary<string, string>() { };
            listcol["编号"] = "uid"; listcol["用户名"] = "username"; listcol["手机"] = "mobile"; listcol["姓名"] = "nickname"; listcol["职位"] = "userrank"; listcol["推荐人"] = "recomuser";
            listcol["在用套餐"] = "hassuite"; listcol["剩余分钟数"] = "totalmin"; listcol["有效期"] = "remainmin"; listcol["充值总额"] = "recharge"; listcol["总赠送分钟数"] = "giftmin";
            listcol["收益总额"] = "totalincome"; listcol["注册时间"] = "registertime";
            listcol["访问时间"] = "lastvisittime";

            string html = ExcelHelper.BuildHtml(dt, listcol);


            //第一种:使用FileContentResult
            byte[] fileContents = Encoding.Default.GetBytes(html);
            return File(fileContents, "application/ms-excel", "用户信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            ////第二种:使用FileStreamResult
            //var fileStream = new MemoryStream(fileContents);
            //return File(fileStream, "application/ms-excel", "fileStream.xls");

            ////第三种:使用FilePathResult
            ////服务器上首先必须要有这个Excel文件,然会通过Server.MapPath获取路径返回.
            //var fileName = Server.MapPath("~/Files/fileName.xls");
            //return File(fileName, "application/ms-excel", "fileName.xls");
        }

        private void Load(int regionId)
        {
            List<SelectListItem> adminGroupList = new List<SelectListItem>();
            adminGroupList.Add(new SelectListItem() { Text = "选择管理员组", Value = "0" });
            foreach (AdminGroupInfo info in AdminGroups.GetAdminGroupList())
            {
                adminGroupList.Add(new SelectListItem() { Text = info.Title, Value = info.AdminGid.ToString() });
            }
            ViewData["adminGroupList"] = adminGroupList;

            RegionInfo regionInfo = Regions.GetRegionById(regionId);
            if (regionInfo != null)
            {
                ViewData["provinceId"] = regionInfo.ProvinceId;
                ViewData["cityId"] = regionInfo.CityId;
                ViewData["countyId"] = regionInfo.RegionId;
            }
            else
            {
                ViewData["provinceId"] = -1;
                ViewData["cityId"] = -1;
                ViewData["countyId"] = -1;
            }

            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();



        }
        #endregion


    }
}
