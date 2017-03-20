using System;
using System.Text;
using System.Data;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.Web.Models;
using OWZX.Model;

namespace OWZX.Web.Controllers
{
    /// <summary>
    /// 用户中心控制器类
    /// </summary>
    public partial class UCenterController : BaseWebController
    {
        #region 用户信息

        /// <summary>
        /// 用户信息
        /// </summary>
        public ActionResult UserInfo()
        {
            UserInfoModel model = new UserInfoModel();

            model.UserInfo = Users.GetUserById(WorkContext.Uid);
            model.UserRankInfo = WorkContext.UserRankInfo;
            model.AdminGroupInfo = WorkContext.AdminGroupInfo;

            RegionInfo regionInfo = Regions.GetRegionById(model.UserInfo.RegionId);
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
            return View(model);
        }
        /// <summary>
        /// 用户default
        /// </summary>
        public ActionResult UserDefault()
        {
            UserInfoModel model = new UserInfoModel();
            model.UserInfo = Users.GetUserById(WorkContext.Uid);
            ViewData["registerurl"] = BSPConfig.ShopConfig.SiteUrl + "/account/register?invitecode="+ model.UserInfo.InviteCode;
            return View(model);
        }
        /// <summary>
        /// 编辑用户信息
        /// </summary>
        public ActionResult EditUser()
        { 
            string nickName = WebHelper.GetFormString("nickName");
            string avatar = WebHelper.GetFormString("avatar");
            string realName = WebHelper.GetFormString("realName");
            int gender = WebHelper.GetFormInt("gender");
            string mobile = WebHelper.GetFormString("mobile");
            string QQ = WebHelper.GetFormString("qqNum"); 

            StringBuilder errorList = new StringBuilder("["); 

            //验证昵称
            if (nickName.Length > 10)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "nickName", "昵称的长度不能大于10", "}");
            }
            else if (FilterWords.IsContainWords(nickName))
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "nickName", "昵称中包含禁止单词", "}");
            }

            //验证真实姓名
            if (realName.Length > 5)
            {
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "realName", "真实姓名的长度不能大于5", "}");
            }

            //验证性别
            if (gender < 0 || gender > 2)
                errorList.AppendFormat("{0}\"key\":\"{1}\",\"msg\":\"{2}\"{3},", "{", "gender", "请选择正确的性别", "}");

           

            if (errorList.Length == 1)
            {

                Users.UpdateUser(WorkContext.Uid, WebHelper.HtmlEncode(nickName), WebHelper.HtmlEncode(avatar), gender, WebHelper.HtmlEncode(realName), mobile, QQ);
     
                return AjaxResult("success", "信息更新成功");
            }
            else
            {
                return AjaxResult("error", errorList.Remove(errorList.Length - 1, 1).Append("]").ToString(), true);
            }
        }

        public ActionResult GetUserMoney()
        {
            PartUserInfo pu = Users.GetPartUserById(WorkContext.Uid);
            var content = "{\"totalmoney\":\""+pu.TotalMoney+"\",\"bankmoney\":\""+pu.BankMoney+"\"}";
            return AjaxResult("sussace", content,true);
        }

        #endregion

        #region 安全中心

        /// <summary>
        /// 账户安全信息
        /// </summary>
        public ActionResult SafeInfo()
        {
            return View(WorkContext.PartUserInfo);
        }

        /// <summary>
        /// 安全验证 //已改动
        /// </summary>
        public ActionResult SafeVerify()
        {
            string action = WebHelper.GetQueryString("act").ToLower();
            string mode = WebHelper.GetQueryString("mode").ToLower();

            if (action.Length == 0 || !CommonHelper.IsInArray(action, new string[4] { "updatesafepassword", "updatepassword", "updatemobile", "updateemail" }) || (mode.Length > 0 && !CommonHelper.IsInArray(mode, new string[4] { "safepassword", "password", "mobile", "email" })))
                return HttpNotFound();

            SafeVerifyModel model = new SafeVerifyModel();
            model.Action = action;

            if (mode.Length == 0)
            {
                if (WorkContext.PartUserInfo.VerifyMobile == 1)//通过手机验证
                    model.Mode = "mobile";
                else if (WorkContext.PartUserInfo.VerifyEmail == 1)//通过邮箱验证
                    model.Mode = "email";
                else if (WorkContext.PartUserInfo.VerifySafePassWord == 1)//通过安全码验证
                    model.Mode = "safepassword";
                else//通过密码验证
                    model.Mode = "password";
            }
            else
            {
                if (mode == "mobile" && WorkContext.PartUserInfo.VerifyMobile == 1)
                    model.Mode = "mobile";
                else if (mode == "email" && WorkContext.PartUserInfo.VerifyEmail == 1)
                    model.Mode = "email";
                else if (mode == "safepassword" && WorkContext.PartUserInfo.VerifySafePassWord == 1)
                    model.Mode = "safepassword";
                else
                    model.Mode = "password";
            }

            return View(model);
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        public ActionResult VerifyPassword()
        {
            string action = WebHelper.GetQueryString("act").ToLower();
            string password = WebHelper.GetFormString("password");
            string verifyCode = WebHelper.GetFormString("verifyCode");

            if (action.Length == 0 || !CommonHelper.IsInArray(action, new string[4] { "updatepassword", "updatesafepassword", "updatemobile", "updateemail" }))
                return AjaxResult("noaction", "动作不存在");

            //检查验证码
            if (string.IsNullOrWhiteSpace(verifyCode))
            {
                return AjaxResult("verifycode", "验证码不能为空");
            }
            if (verifyCode.ToLower() != Sessions.GetValueString(WorkContext.Sid, "verifyCode"))
            {
                return AjaxResult("verifycode", "验证码不正确");
            }

            //检查密码
            if (string.IsNullOrWhiteSpace(password))
            {
                return AjaxResult("password", "密码不能为空");
            }
            if (Users.CreateUserPassword(password, WorkContext.PartUserInfo.Salt) != WorkContext.Password)
            {
                return AjaxResult("password", "密码不正确");
            }

            string v = ShopUtils.AESEncrypt(string.Format("{0},{1},{2},{3}", WorkContext.Uid, action, DateTime.Now, Randoms.CreateRandomValue(6)));
            string url = Url.Action("safeupdate", new RouteValueDictionary { { "v", v } });
            return AjaxResult("success", url);
        }
        /// <summary>
        /// 验证安全密码  //已改动
        /// </summary>
        public ActionResult VerifySafePassword()
        {
            string action = WebHelper.GetQueryString("act").ToLower();
            string password = WebHelper.GetFormString("safepassword");
            string verifyCode = WebHelper.GetFormString("verifyCode");

            if (action.Length == 0 || !CommonHelper.IsInArray(action, new string[4] { "updatesafepassword", "updatepassword", "updatemobile", "updateemail" }))
                return AjaxResult("noaction", "动作不存在");

            //检查验证码
            if (string.IsNullOrWhiteSpace(verifyCode))
            {
                return AjaxResult("verifycode", "验证码不能为空");
            }
            if (verifyCode.ToLower() != Sessions.GetValueString(WorkContext.Sid, "verifyCode"))
            {
                return AjaxResult("verifycode", "验证码不正确");
            }

            //检查密码
            if (string.IsNullOrWhiteSpace(password))
            {
                return AjaxResult("safepassword", "密码不能为空");
            }
            if (Users.CreateUserSafePassword(password, WorkContext.PartUserInfo.Salt) != WorkContext.SafePassword)
            {
                return AjaxResult("safepassword", "密码不正确");
            }

            string v = ShopUtils.AESEncrypt(string.Format("{0},{1},{2},{3}", WorkContext.Uid, action, DateTime.Now, Randoms.CreateRandomValue(6)));
            string url = Url.Action("safeupdate", new RouteValueDictionary { { "v", v } });
            return AjaxResult("success", url);
        }
        /// <summary>
        /// 发送验证手机短信
        /// </summary>
        public ActionResult SendVerifyMobile()
        {
            if (WorkContext.PartUserInfo.VerifyMobile == 0)
                return AjaxResult("unverifymobile", "手机号没有通过验证,所以不能发送验证短信");

            string moibleCode = Randoms.CreateRandomValue(6);
            //发送验证手机短信
            SMSes.SendSCVerifySMS(WorkContext.UserMobile, moibleCode);
            //将验证值保存在session中
            Sessions.SetItem(WorkContext.Sid, "ucsvMoibleCode", moibleCode);

            return AjaxResult("success", "短信已经发送,请查收");
        }

        /// <summary>
        /// 验证手机
        /// </summary>
        public ActionResult VerifyMobile()
        {
            string action = WebHelper.GetQueryString("act").ToLower();
            string moibleCode = WebHelper.GetFormString("moibleCode");
            string verifyCode = WebHelper.GetFormString("verifyCode");

            if (action.Length == 0 || !CommonHelper.IsInArray(action, new string[3] { "updatepassword", "updatemobile", "updateemail" }))
                return AjaxResult("noaction", "动作不存在");
            if (WorkContext.PartUserInfo.VerifyMobile == 0)
                return AjaxResult("unverifymobile", "手机号没有通过验证,所以不能进行验证");

            //检查验证码
            if (string.IsNullOrWhiteSpace(verifyCode))
            {
                return AjaxResult("verifycode", "验证码不能为空");
            }
            if (verifyCode.ToLower() != Sessions.GetValueString(WorkContext.Sid, "verifyCode"))
            {
                return AjaxResult("verifycode", "验证码不正确");
            }

            //检查手机码
            if (string.IsNullOrWhiteSpace(moibleCode))
            {
                return AjaxResult("moiblecode", "手机码不能为空");
            }
            if (Sessions.GetValueString(WorkContext.Sid, "ucsvMoibleCode") != moibleCode)
            {
                return AjaxResult("moiblecode", "手机码不正确");
            }

            string v = ShopUtils.AESEncrypt(string.Format("{0},{1},{2},{3}", WorkContext.Uid, action, DateTime.Now, Randoms.CreateRandomValue(6)));
            string url = Url.Action("safeupdate", new RouteValueDictionary { { "v", v } });
            return AjaxResult("success", url);
        }

        /// <summary>
        /// 发送验证邮箱邮件
        /// </summary>
        public ActionResult SendVerifyEmail()
        {
            string action = WebHelper.GetQueryString("act").ToLower();
            string verifyCode = WebHelper.GetFormString("verifyCode");

            if (action.Length == 0 || !CommonHelper.IsInArray(action, new string[4] { "updatesafepassword","updatepassword", "updatemobile", "updateemail" }))
                return AjaxResult("noaction", "动作不存在");
            if (WorkContext.PartUserInfo.VerifyEmail == 0)
                return AjaxResult("unverifyemail", "邮箱没有通过验证,所以不能发送验证邮件");

            //检查验证码
            if (string.IsNullOrWhiteSpace(verifyCode))
            {
                return AjaxResult("verifycode", "验证码不能为空");
            }
            if (verifyCode.ToLower() != Sessions.GetValueString(WorkContext.Sid, "verifyCode"))
            {
                return AjaxResult("verifycode", "验证码不正确");
            }

            string v = ShopUtils.AESEncrypt(string.Format("{0},{1},{2},{3}", WorkContext.Uid, action, DateTime.Now, Randoms.CreateRandomValue(6)));
            string url = string.Format("http://{0}{1}", Request.Url.Authority, Url.Action("safeupdate", new RouteValueDictionary { { "v", v } }));
            //发送验证邮件
            Emails.SendSCVerifyEmail(WorkContext.UserEmail, WorkContext.UserName, url);
            return AjaxResult("success", "邮件已经发送,请前往你的邮箱进行验证");
        }

        /// <summary>
        /// 安全更新
        /// </summary>
        public ActionResult SafeUpdate()
        {
            string v = WebHelper.GetQueryString("v");
            //解密字符串
            string realV;
            try
            {
                realV = ShopUtils.AESDecrypt(v);
            }
            catch (Exception ex)
            {
                //如果v来自邮件，那么需要url解码
                realV = ShopUtils.AESDecrypt(WebHelper.UrlDecode(v));
            }

            //数组第一项为uid，第二项为动作，第三项为验证时间,第四项为随机值
            string[] result = StringHelper.SplitString(realV);
            if (result.Length != 4)
                return HttpNotFound();

            int uid = TypeHelper.StringToInt(result[0]);
            string action = result[1];
            DateTime time = TypeHelper.StringToDateTime(result[2]);

            //判断当前用户是否为验证用户
            if (uid != WorkContext.Uid)
                return HttpNotFound();
            //判断验证时间是否过时
            if (DateTime.Now.AddMinutes(-30) > time)
                return PromptView("此链接已经失效，请重新验证");

            SafeUpdateModel model = new SafeUpdateModel();
            model.Action = action;
            model.V = WebHelper.UrlEncode(v);

            return View(model);
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        public ActionResult UpdatePassword()
        {
            string v = WebHelper.GetQueryString("v");
            //解密字符串
            string realV = ShopUtils.AESDecrypt(v);

            //数组第一项为uid，第二项为动作，第三项为验证时间,第四项为随机值
            string[] result = StringHelper.SplitString(realV);
            if (result.Length != 4)
                return AjaxResult("noauth", "您的权限不足");

            int uid = TypeHelper.StringToInt(result[0]);
            string action = result[1];
            DateTime time = TypeHelper.StringToDateTime(result[2]);

            //判断当前用户是否为验证用户
            if (uid != WorkContext.Uid)
                return AjaxResult("noauth", "您的权限不足");
            //判断验证时间是否过时
            if (DateTime.Now.AddMinutes(-30) > time)
                return AjaxResult("expired", "密钥已过期,请重新验证");

            string password = WebHelper.GetFormString("password");
            string confirmPwd = WebHelper.GetFormString("confirmPwd");
            string verifyCode = WebHelper.GetFormString("verifyCode");

            //检查验证码
            if (string.IsNullOrWhiteSpace(verifyCode))
            {
                return AjaxResult("verifycode", "验证码不能为空");
            }
            if (verifyCode.ToLower() != Sessions.GetValueString(WorkContext.Sid, "verifyCode"))
            {
                return AjaxResult("verifycode", "验证码不正确");
            }

            //检查密码
            if (string.IsNullOrWhiteSpace(password))
            {
                return AjaxResult("password", "密码不能为空");
            }
            if (password.Length < 4 || password.Length > 32)
            {
                return AjaxResult("password", "密码不能小于3且不大于32个字符");
            }
            if (password != confirmPwd)
            {
                return AjaxResult("confirmpwd", "两次密码不相同");
            }

            string p = Users.CreateUserPassword(password, WorkContext.PartUserInfo.Salt);
            //设置新密码
            Users.UpdateUserPasswordByUid(WorkContext.Uid, p);
            //同步cookie中密码
            ShopUtils.SetCookiePassword(p, "web");

            string url = Url.Action("safesuccess", new RouteValueDictionary { { "act", "updatePassword" } });
            return AjaxResult("success", url);
        }
        /// <summary>
        /// 更新密码
        /// </summary>
        public ActionResult UpdateSafePassword()
        {
            string v = WebHelper.GetQueryString("v");
            //解密字符串
            string realV = ShopUtils.AESDecrypt(v);

            //数组第一项为uid，第二项为动作，第三项为验证时间,第四项为随机值
            string[] result = StringHelper.SplitString(realV);
            if (result.Length != 4)
                return AjaxResult("noauth", "您的权限不足");

            int uid = TypeHelper.StringToInt(result[0]);
            string action = result[1];
            DateTime time = TypeHelper.StringToDateTime(result[2]);

            //判断当前用户是否为验证用户
            if (uid != WorkContext.Uid)
                return AjaxResult("noauth", "您的权限不足");
            //判断验证时间是否过时
            if (DateTime.Now.AddMinutes(-30) > time)
                return AjaxResult("expired", "密钥已过期,请重新验证");

            string password = WebHelper.GetFormString("safepassword");
            string confirmPwd = WebHelper.GetFormString("confirmSafePwd");
            string verifyCode = WebHelper.GetFormString("verifyCode");

            //检查验证码
            if (string.IsNullOrWhiteSpace(verifyCode))
            {
                return AjaxResult("verifycode", "验证码不能为空");
            }
            if (verifyCode.ToLower() != Sessions.GetValueString(WorkContext.Sid, "verifyCode"))
            {
                return AjaxResult("verifycode", "验证码不正确");
            }

            //检查密码
            if (string.IsNullOrWhiteSpace(password))
            {
                return AjaxResult("safepassword", "安全密码不能为空");
            }
            if (password.Length < 4 || password.Length > 32)
            {
                return AjaxResult("safepassword", "安全密码不能小于3且不大于32个字符");
            }
            if (password != confirmPwd)
            {
                return AjaxResult("confirmpwd", "两次密码不相同");
            }

            string p = Users.CreateUserPassword(password, WorkContext.PartUserInfo.Salt);
            //设置新密码
            //未改动
            Users.UpdateUserSafePasswordByUid(WorkContext.Uid, p);
            //同步cookie中密码
            //ShopUtils.SetCookiePassword(p, "web");

            string url = Url.Action("safesuccess", new RouteValueDictionary { { "act", "updatePassword" } });
            return AjaxResult("success", url);
        }
        /// <summary>
        /// 发送更新手机确认短信
        /// </summary>
        public ActionResult SendUpdateMobile()
        {
            string v = WebHelper.GetQueryString("v");
            //解密字符串
            string realV = ShopUtils.AESDecrypt(v);

            //数组第一项为uid，第二项为动作，第三项为验证时间,第四项为随机值
            string[] result = StringHelper.SplitString(realV);
            if (result.Length != 4)
                return AjaxResult("noauth", "您的权限不足");

            int uid = TypeHelper.StringToInt(result[0]);
            string action = result[1];
            DateTime time = TypeHelper.StringToDateTime(result[2]);

            //判断当前用户是否为验证用户
            if (uid != WorkContext.Uid)
                return AjaxResult("noauth", "您的权限不足");
            //判断验证时间是否过时
            if (DateTime.Now.AddMinutes(-30) > time)
                return AjaxResult("expired", "密钥已过期,请重新验证");

            string mobile = WebHelper.GetFormString("mobile");

            //检查手机号
            if (string.IsNullOrWhiteSpace(mobile))
            {
                return AjaxResult("mobile", "手机号不能为空");
            }
            if (!ValidateHelper.IsMobile(mobile))
            {
                return AjaxResult("mobile", "手机号格式不正确");
            }
            int tempUid = Users.GetUidByMobile(mobile);
            if (tempUid > 0 && tempUid != WorkContext.Uid)
                return AjaxResult("mobile", "手机号已经存在");

            string mobileCode = Randoms.CreateRandomValue(6);
            //发送短信
            SMSes.SendSCUpdateSMS(mobile, mobileCode);
            //将验证值保存在session中
            Sessions.SetItem(WorkContext.Sid, "ucsuMobile", mobile);
            Sessions.SetItem(WorkContext.Sid, "ucsuMobileCode", mobileCode);

            return AjaxResult("success", "短信已发送,请查收");
        }

        /// <summary>
        /// 更新手机号
        /// </summary>
        public ActionResult UpdateMobile()
        {
            string v = WebHelper.GetQueryString("v");
            //解密字符串
            string realV = ShopUtils.AESDecrypt(v);

            //数组第一项为uid，第二项为动作，第三项为验证时间,第四项为随机值
            string[] result = StringHelper.SplitString(realV);
            if (result.Length != 4)
                return AjaxResult("noauth", "您的权限不足");

            int uid = TypeHelper.StringToInt(result[0]);
            string action = result[1];
            DateTime time = TypeHelper.StringToDateTime(result[2]);

            //判断当前用户是否为验证用户
            if (uid != WorkContext.Uid)
                return AjaxResult("noauth", "您的权限不足");
            //判断验证时间是否过时
            if (DateTime.Now.AddMinutes(-30) > time)
                return AjaxResult("expired", "密钥已过期,请重新验证");

            string mobile = WebHelper.GetFormString("mobile");
            string moibleCode = WebHelper.GetFormString("moibleCode");
            string verifyCode = WebHelper.GetFormString("verifyCode");

            //检查验证码
            if (string.IsNullOrWhiteSpace(verifyCode))
            {
                return AjaxResult("verifycode", "验证码不能为空");
            }
            if (verifyCode.ToLower() != Sessions.GetValueString(WorkContext.Sid, "verifyCode"))
            {
                return AjaxResult("verifycode", "验证码不正确");
            }

            //检查手机号
            if (string.IsNullOrWhiteSpace(mobile))
            {
                return AjaxResult("mobile", "手机号不能为空");
            }
            if (Sessions.GetValueString(WorkContext.Sid, "ucsuMobile") != mobile)
            {
                return AjaxResult("mobile", "接收手机不一致");
            }

            //检查手机码
            if (string.IsNullOrWhiteSpace(moibleCode))
            {
                return AjaxResult("moiblecode", "手机码不能为空");
            }
            if (Sessions.GetValueString(WorkContext.Sid, "ucsuMobileCode") != moibleCode)
            {
                return AjaxResult("moiblecode", "手机码不正确");
            }

            //更新手机号
            Users.UpdateUserMobileByUid(WorkContext.Uid, mobile);
            //发放验证手机积分
            //Credits.SendVerifyMobileCredits(ref WorkContext.PartUserInfo, DateTime.Now);

            string url = Url.Action("safesuccess", new RouteValueDictionary { { "act", "updateMobile" } });
            return AjaxResult("success", url);
        }

        /// <summary>
        /// 发送更新邮箱确认邮件
        /// </summary>
        public ActionResult SendUpdateEmail()
        {
            string v = WebHelper.GetQueryString("v");
            //解密字符串
            string realV = ShopUtils.AESDecrypt(v);

            //数组第一项为uid，第二项为动作，第三项为验证时间,第四项为随机值
            string[] result = StringHelper.SplitString(realV);
            if (result.Length != 4)
                return AjaxResult("noauth", "您的权限不足");

            int uid = TypeHelper.StringToInt(result[0]);
            string action = result[1];
            DateTime time = TypeHelper.StringToDateTime(result[2]);

            //判断当前用户是否为验证用户
            if (uid != WorkContext.Uid)
                return AjaxResult("noauth", "您的权限不足");
            //判断验证时间是否过时
            if (DateTime.Now.AddMinutes(-30) > time)
                return AjaxResult("expired", "密钥已过期,请重新验证");

            string email = WebHelper.GetFormString("email");
            string verifyCode = WebHelper.GetFormString("verifyCode");

            //检查验证码
            if (string.IsNullOrWhiteSpace(verifyCode))
            {
                return AjaxResult("verifycode", "验证码不能为空");
            }
            if (verifyCode.ToLower() != Sessions.GetValueString(WorkContext.Sid, "verifyCode"))
            {
                return AjaxResult("verifycode", "验证码不正确");
            }

            //检查邮箱
            if (string.IsNullOrWhiteSpace(email))
            {
                return AjaxResult("email", "邮箱不能为空");
            }
            if (!ValidateHelper.IsEmail(email))
            {
                return AjaxResult("email", "邮箱格式不正确");
            }
            if (!SecureHelper.IsSafeSqlString(email, false))
            {
                return AjaxResult("email", "邮箱已经存在");
            }
            int tempUid = Users.GetUidByEmail(email);
            if (tempUid > 0 && tempUid != WorkContext.Uid)
                return AjaxResult("email", "邮箱已经存在");


            string v2 = ShopUtils.AESEncrypt(string.Format("{0},{1},{2},{3}", WorkContext.Uid, email, DateTime.Now, Randoms.CreateRandomValue(6)));
            string url = string.Format("http://{0}{1}", Request.Url.Authority, Url.Action("updateemail", new RouteValueDictionary { { "v", v2 } }));

            //发送验证邮件
            Emails.SendSCUpdateEmail(email, WorkContext.UserName, url);
            return AjaxResult("success", "邮件已经发送，请前往你的邮箱进行验证");
        }

        /// <summary>
        /// 更新邮箱
        /// </summary>
        public ActionResult UpdateEmail()
        {
            string v = WebHelper.GetQueryString("v");
            //解密字符串
            string realV;
            try
            {
                realV = ShopUtils.AESDecrypt(v);
            }
            catch (Exception ex)
            {
                //如果v来自邮件，那么需要url解码
                realV = ShopUtils.AESDecrypt(WebHelper.UrlDecode(v));
            }

            //数组第一项为uid，第二项为邮箱名，第三项为验证时间,第四项为随机值
            string[] result = StringHelper.SplitString(realV);
            if (result.Length != 4)
                return HttpNotFound();

            int uid = TypeHelper.StringToInt(result[0]);
            string email = result[1];
            DateTime time = TypeHelper.StringToDateTime(result[2]);

            //判断当前用户是否为验证用户
            if (uid != WorkContext.Uid)
                return HttpNotFound();
            //判断验证时间是否过时
            if (DateTime.Now.AddMinutes(-30) > time)
                return PromptView("此链接已经失效，请重新验证");
            int tempUid = Users.GetUidByEmail(email);
            if (tempUid > 0 && tempUid != WorkContext.Uid)
                return PromptView("此链接已经失效，邮箱已经存在");

            //更新邮箱名
            Users.UpdateUserEmailByUid(WorkContext.Uid, email);
            //发放验证邮箱积分
            //Credits.SendVerifyEmailCredits(ref WorkContext.PartUserInfo, DateTime.Now);

            return RedirectToAction("safesuccess", new RouteValueDictionary { { "act", "updateEmail" }, { "remark", email } });
        }

        /// <summary>
        /// 安全成功
        /// </summary>
        public ActionResult SafeSuccess()
        {
            string action = WebHelper.GetQueryString("act").ToLower();
            string remark = WebHelper.GetQueryString("remark");

            if (action.Length == 0 || !CommonHelper.IsInArray(action, new string[3] { "updatepassword", "updatemobile", "updateemail" }))
                return HttpNotFound();

            SafeSuccessModel model = new SafeSuccessModel();
            model.Action = action;
            model.Remark = remark;

            return View(model);
        }

        #endregion

        #region 元宝明细
        /// <summary>
        /// 元宝明细
        /// </summary>
        public ActionResult AccountDetail(int Uid=-1 , string start = "", string end = "", int pageSize = 15, int pageNumber = 1)
        {

            StringBuilder strb = new StringBuilder();
            strb.Append(" where 1=1");
            if (Uid==-1)
            {
                Uid = WorkContext.PartUserInfo.Uid;
            }
            strb.Append(" and b.uid=" + Uid + "");
            if (start != string.Empty)
                strb.Append(" and a.addtime between '" + start + "' and '" + end + "'");

            List<MD_Change> list = NewUser.GetAChangeList(pageNumber, pageSize, strb.ToString());
            UserChangeList userlist = new UserChangeList
            {
                Uid = Uid,
                Start = start,
                End = end,
                PageModel = new PageModel(pageSize, pageNumber, list.Count > 0 ? list[0].TotalCount : 0),
                ChangeList = list
            };
            return View(userlist);
        }

        public ActionResult GetCardcode(string orderid)
        {
           MD_UserOrder order= ChangeWare.GetOrderDetail(" and orderid='" + orderid + "'");
            if (order != null && order.OrderID > -1)
            {
                //发送验证邮件
                Emails.SendEmail(WorkContext.UserEmail, "卡号为:" + order.OrderCode + "的卡密是:" + order.Content);
                return AjaxResult("success", "邮件已经发送,请前往你的邮箱进行验证");
            }
            else
            {
                return AjaxResult("error", "暂未获取到对应卡密信息");
            }
        }

        /// <summary>
        /// 兑换明细
        /// </summary>
        public ActionResult ChangeRecord(int uid=-1,int type=-1 ,int pageSize = 15, int pageNumber = 1)
        { 
            StringBuilder strb = new StringBuilder();
            strb.Append(" ");
            if (uid == -1)
            {
                uid = WorkContext.PartUserInfo.Uid;
            }
            strb.Append(" and a.userid="+uid+" ");
            if (type>-1)
            {
                var btime =
                    DateTime.Now.AddDays(type == 1
                        ? -7
                        : (type == 2 ? -30 : (type == 3 ? -180 : (type == 4 ? -365 : -1)))).ToString("yyyy-MM-dd HH:mm:ss");
                strb.Append(" and a.createtime between '" + btime + "' and '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            } 
            List<MD_UserOrder> list = ChangeWare.GetUserOrderList(pageNumber, pageSize, strb.ToString());
            WareChangeList warelist = new WareChangeList
            {
                type = type,
                uid = uid,
                PageModel = new PageModel(pageSize, pageNumber, list.Count > 0 ? list[0].TotalCount : 0),
                ChangeList = list
            };
            return View(warelist);
        }
        /// <summary>
        /// 我的银行
        /// </summary>
        public ActionResult UserBank()
        {
            return View(WorkContext.PartUserInfo);
        }

        public ActionResult BankChange(decimal changefee, int type = 0, string safepassword = "")
        {
            string msg = "";
            if (!string.IsNullOrEmpty(safepassword) && type == 1)
            {
                return AjaxResult("data", "安全密码错误"); 
            }
            
            msg= Users.BankChange(WorkContext.Uid, changefee, type);
           
            return AjaxResult("data", msg);
        }

        public ActionResult LoginLimit(int uid=-1,int type = -1, int pageSize = 15, int pageNumber = 1)
        {
            UserInfoModel model = new UserInfoModel();
            model.UserInfo = Users.GetUserById(WorkContext.Uid);
            RegionInfo regionInfo = Regions.GetRegionById(model.UserInfo.RegionId);
            RegionInfo regionInfotwo = Regions.GetRegionById(model.UserInfo.RegionIdTwo);
            if (regionInfo != null)
            {
                ViewData["provinceId"] = regionInfo.ParentId;
                ViewData["cityId"] = regionInfo.RegionId; 
            }
            else
            {
                ViewData["provinceId"] = -1;
                ViewData["cityId"] = -1; 
            }
            if (regionInfotwo != null)
            {
                ViewData["provinceIdtwo"] = regionInfotwo.ParentId;
                ViewData["cityIdtwo"] = regionInfotwo.RegionId;  
            }
            else
            {
                ViewData["provinceIdtwo"] = -1;
                ViewData["cityIdtwo"] = -1; 
            }

            StringBuilder strb = new StringBuilder();
            strb.Append(" ");
            if (uid == -1)
            {
                uid = WorkContext.PartUserInfo.Uid;
            }
            if (uid > -1)
            {
                strb.Append(" and a.uid="+uid+" ");
            }
            if (type > -1)
            {
                var btime =
                    DateTime.Now.AddDays(type == 1
                        ? -7
                        : (type == 2 ? -30 : (type == 3 ? -180 : (type == 4 ? -365 : -1)))).ToString("yyyy-MM-dd HH:mm:ss");
                strb.Append(" and a.createtime between '" + btime + "' and '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            }
            ViewData["isverifylog"] = WorkContext.PartUserInfo.VerifyLoginRg;
            ViewData["IP"] = WorkContext.IP;
            List<MD_UsersLog> list = LoginFailLogs.GetUserLoginList(pageNumber, pageSize, strb.ToString());
            UserLogList loglist = new UserLogList
            {
                type = type,
                uid=uid,
                PartUser = WorkContext.PartUserInfo,
                PageModel = new PageModel(pageSize, pageNumber, list.Count > 0 ? list[0].TotalCount : 0),
                LogList = list
            };
            return View(loglist); 
    }


        public ActionResult UpdateVerifyRgLog(int uid =-1,int isveritylog=0,int regionid=0, int regionidtwo=0)
        {
            if (uid < 1)
            {
                return AjaxResult("data", "此用户不存在!"); 
            }

            var result = Users.UpdateUserVerifyLog(uid, isveritylog, regionid, regionidtwo);
            if (result)
            {
                PartUserInfo part = WorkContext.PartUserInfo;
                part.VerifyLoginRg = isveritylog;
                WorkContext.PartUserInfo= part;
            }
            return AjaxResult("data", result?"设置成功":"设置失败"); 

        }

        #endregion
        #region 用户回水
        public ActionResult GetOwnerBack(int type)
        {

            if (WorkContext.PartUserInfo != null && WorkContext.PartUserInfo.Uid > 0)
            {
                var result = NewUser.UpdUserBackReport(WorkContext.PartUserInfo.Uid, type);
                return AjaxResult("data", result); 
            }
            else
            {
                return AjaxResult("data", "用户信息已过期,请重新登录!");
            }

           

        }
        #endregion
        protected sealed override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            //不允许游客访问
            if (WorkContext.Uid < 1)
            {
                if (WorkContext.IsHttpAjax)//如果为ajax请求
                    filterContext.Result = Content("nologin");
                else//如果为普通请求
                    filterContext.Result = RedirectToAction("login", "account", new RouteValueDictionary { { "returnUrl", WorkContext.Url } });
            }
        }
    }
}
