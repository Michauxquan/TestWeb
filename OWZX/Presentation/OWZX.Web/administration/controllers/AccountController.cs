using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.Web.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OWZX.Web.Admin.Controllers
{
    public class AccountController : BaseAdminController
    {
        /// <summary>
        /// 登录
        /// </summary>
        public ActionResult Login()
        {
            string returnUrl = WebHelper.GetQueryString("returnUrl");
            if (returnUrl.Length == 0)
                returnUrl = "/admin/home/index";

            if (WorkContext.Uid > 0)
                return PromptView(returnUrl, "您已经登录，无须重复登录!");
            if (WorkContext.ShopConfig.LoginFailTimes != 0 && LoginFailLogs.GetLoginFailTimesByIp(WorkContext.IP) >= WorkContext.ShopConfig.LoginFailTimes)
                return PromptView(returnUrl, "您已经输入错误" + WorkContext.ShopConfig.LoginFailTimes + "次密码，请15分钟后再登录!");

            //get请求
            if (WebHelper.IsGet())
            {
                LoginModel model = new LoginModel();

                model.ReturnUrl = returnUrl;
                model.ShadowName = WorkContext.ShopConfig.ShadowName;
                model.IsRemember = WorkContext.ShopConfig.IsRemember == 1;
                model.IsVerifyCode = CommonHelper.IsInArray(WorkContext.PageKey, WorkContext.ShopConfig.VerifyPages);
                model.OAuthPluginList = Plugins.GetOAuthPluginList();

                return View(model);
            }

            //ajax请求
            string accountName = WebHelper.GetFormString(WorkContext.ShopConfig.ShadowName);
            string password = WebHelper.GetFormString("password");
            string verifyCode = WebHelper.GetFormString("verifyCode");
            int isRemember = WebHelper.GetFormInt("isRemember");

            StringBuilder errorList = new StringBuilder("[");
            //验证账户名
            if (string.IsNullOrWhiteSpace(accountName))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "账户名不能为空", "}");
            }
            else if (accountName.Length < 4 || accountName.Length > 50)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "账户名必须大于3且不大于50个字符", "}");
            }
            else if ((!SecureHelper.IsSafeSqlString(accountName, false)))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "账户名不存在", "}");
            }

            //验证密码
            if (string.IsNullOrWhiteSpace(password))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "password", "密码不能为空", "}");
            }
            else if (password.Length < 4 || password.Length > 32)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "password", "密码必须大于3且不大于32个字符", "}");
            }

            //验证验证码
            if (CommonHelper.IsInArray(WorkContext.PageKey, WorkContext.ShopConfig.VerifyPages))
            {
                if (string.IsNullOrWhiteSpace(verifyCode))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "verifyCode", "验证码不能为空", "}");
                }
                else if (verifyCode.ToLower() != Sessions.GetValueString(WorkContext.Sid, "verifyCode"))
                {
                    errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "verifyCode", "验证码不正确", "}");
                }
            }

            //当以上验证全部通过时
            PartUserInfo partUserInfo = null;
            if (errorList.Length == 1)
            {
                if (BSPConfig.ShopConfig.LoginType.Contains("2") && ValidateHelper.IsEmail(accountName))//邮箱登录
                {
                    partUserInfo = Users.GetPartUserByEmail(accountName);
                    if (partUserInfo == null)
                        errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "邮箱不存在", "}");
                }
                else if (BSPConfig.ShopConfig.LoginType.Contains("3") && ValidateHelper.IsMobile(accountName))//手机登录
                {
                    partUserInfo = Users.GetPartUserByMobile(accountName);
                    if (partUserInfo == null)
                        errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "手机不存在", "}");
                }
                else if (BSPConfig.ShopConfig.LoginType.Contains("1"))//用户名登录
                {
                    partUserInfo = Users.GetPartUserByName(accountName);
                    if (partUserInfo == null)
                        errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "用户名不存在", "}");
                }

                if (partUserInfo != null)
                {
                    if (Users.CreateUserPassword(password, partUserInfo.Salt) != partUserInfo.Password)//判断密码是否正确
                    {
                        LoginFailLogs.AddLoginFailTimes(WorkContext.IP, DateTime.Now);//增加登录失败次数
                        errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "password", "密码不正确", "}");
                    }
                    else if (partUserInfo.UserRid == 1)//当用户等级是禁止访问等级时
                    {
                        if (partUserInfo.LiftBanTime > DateTime.Now)//达到解禁时间
                        {
                            UserRankInfo userRankInfo = UserRanks.GetUserRankByCredits(partUserInfo.PayCredits);
                            Users.UpdateUserRankByUid(partUserInfo.Uid, userRankInfo.UserRid);
                            partUserInfo.UserRid = userRankInfo.UserRid;
                        }
                        else
                        {
                            errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "accountName", "您的账号当前被锁定,不能访问", "}");
                        }
                    }
                }
            }

            if (errorList.Length > 1)//验证失败时
            {
                return AjaxResult("error", errorList.Remove(errorList.Length - 1, 1).Append("]").ToString(), true);
            }
            else//验证成功时
            {
                //删除登录失败日志
                LoginFailLogs.DeleteLoginFailLogByIP(WorkContext.IP);
                ////更新用户最后访问
                //Users.UpdateUserLastVisit(partUserInfo.Uid, DateTime.Now, WorkContext.IP, WorkContext.RegionId);
               
                //将用户信息写入cookie中
                ShopUtils.SetUserCookie(partUserInfo, (WorkContext.ShopConfig.IsRemember == 1 && isRemember == 1) ? 30 : -1,"admin");

                return AjaxResult("success", "登录成功");
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        public ActionResult Logout()
        {
            if (WorkContext.Uid > 0)
            {
                WebHelper.DeleteCookie("admin_bsp");
                Sessions.RemoverSession(WorkContext.Sid);
                //OnlineUsers.DeleteOnlineUserBySid(WorkContext.Sid);
            }
            return Redirect("/admin/account/login");
        }
    }
}
