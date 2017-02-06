using log4net;
using Newtonsoft.Json;
using OWZX.Core;
using OWZX.Model;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OWZX.Web.controllers
{
    //App账户管理
    public class AppAccountController : BaseWebController
    {
        private readonly static ILog logger = LogManager.GetLogger("AppAccount");
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult SendSMS()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                if (parmas.Keys.Count != 4)
                {
                    return APIResult("error", "缺少请求参数");
                }
                string account = parmas["account"].Trim().ToLower(); //手机
                string code = parmas["code"];//验证码
                string type = parmas["type"];

                string body = "【PC蛋蛋】您正在" + type + ",验证码" + code + ",若非本人操作，请勿泄露。";

                MD_SMSCode smscode = new MD_SMSCode
                {
                    Account = account,
                    Code = code,
                    Expiretime = DateTime.Now.AddMinutes(10)
                };
                bool sms = NewUser.AddSMSCode(smscode);
                if (sms)
                {
                    //发送短信
                    bool smsres = SMSes.SendSY(account, HttpUtility.UrlEncode(body, Encoding.UTF8));
                    if (!smsres)
                    {
                        return APIResult("error", "发送失败");
                    }

                    return APIResult("success", "发送成功");
                }
                else
                    return APIResult("error", "发送失败");
            }
            catch (Exception ex)
            {
                return APIResult("error", "发送失败");
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        public ActionResult Login()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                if (parmas.Keys.Count != 3)
                {
                    return APIResult("error", "缺少请求参数");
                }
                PartUserInfo partUserInfo = null;
                //string type=parmas["type"];//1:微信 2:QQ 3:账户
                //if (type == "1")
                //{
                //    string openid = parmas["openid"];
                //    string nickname = parmas["nickname"];

                //    //判断此用户是否已经存在
                //    int uid = OAuths.GetUidByOpenIdAndServer(openid, "微信");

                //    if (uid > 0)//存在时
                //    {
                //        partUserInfo = Users.GetPartUserById(uid);
                //    }
                //    else
                //    {
                //        UserInfo userInfo = OAuths.CreateOAuthUser(nickname, "wx", openid, "微信", WorkContext.RegionId, "");
                //        partUserInfo.Uid = userInfo.Uid;
                //    }
                //}
                //else if (type == "2")
                //{
                //    string openid = parmas["openid"];
                //    string nickname = parmas["nickname"];

                //    //判断此用户是否已经存在
                //    int uid = OAuths.GetUidByOpenIdAndServer(openid, "QQ");

                //    if (uid > 0)//存在时
                //    {
                //        partUserInfo = Users.GetPartUserById(uid);
                //    }
                //    else
                //    {
                //        UserInfo userInfo = OAuths.CreateOAuthUser(nickname, "qq", openid, "QQ", WorkContext.RegionId, "");
                //        partUserInfo.Uid = userInfo.Uid;
                //    }
                //}
                //else if (type == "3")
                //{
                //ajax请求
                string accountName = parmas["account"];//手机号
                string password = parmas["password"];
                string imei = parmas["imei"];

                //当以上验证全部通过时
                partUserInfo = Users.GetPartUserByMobile(accountName);
                if (partUserInfo != null)
                {
                    if (partUserInfo.Uid <= 0)
                        return APIResult("error", "账号不存在");

                    if (Users.CreateUserPassword(password, partUserInfo.Salt) != partUserInfo.Password)//判断密码是否正确
                    {
                        return APIResult("error", "密码不正确");
                    }
                    //更新IMEI号
                    partUserInfo.IMEI = imei;
                    bool partres = Users.UpdatePartUser(partUserInfo);
                    if (!partres)
                    {
                        return APIResult("error", "登录失败");
                    }
                }
                //}
                //更新用户最后访问
                Users.UpdateUserLastVisit(partUserInfo.Uid, DateTime.Now, WorkContext.IP, WorkContext.RegionId);


                return APIResult("success", "登录成功");
            }
            catch (Exception ex)
            {
                return APIResult("error", "登录失败");
            }
        }
        /// <summary>
        /// 验证账号是否存在
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidateAccount()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                if (parmas.Keys.Count != 2)
                {
                    return APIResult("error", "缺少请求参数");
                }

                if (Users.IsExistMobile(parmas["account"]))
                {
                    return APIResult("error", "手机号已经存在");
                }
                else
                {
                    return APIResult("success", "手机号可以注册");
                }
            }
            catch (Exception ex)
            {
                return APIResult("error", ex.Message);
            }
        }
        /// <summary>
        /// 注册
        /// </summary>
        public ActionResult Register()
        {
            try
            {
                //if (WorkContext.ShopConfig.RegTimeSpan > 0)
                //{
                //    DateTime registerTime = Users.GetRegisterTimeByRegisterIP(WorkContext.IP);
                //    if ((DateTime.Now - registerTime).Minutes <= WorkContext.ShopConfig.RegTimeSpan)
                //        return APIResult("error", "你注册太频繁，请间隔一定时间后再注册!");
                //}
                NameValueCollection parmas = WorkContext.postparms;
                if (parmas.Keys.Count != 3)
                {
                    return APIResult("error", "缺少请求参数");
                }
                //ajax请求
                string phone = string.Empty;
                string account = phone = parmas["account"].Trim();

                int userid = Users.GetUidByMobile(account);
                if (userid > 0)
                {
                    return AjaxResult("error", "账号已存在");
                }

                int invitecode = -1;
                if (parmas.AllKeys.Contains("invitecode"))
                    invitecode = int.Parse(parmas["invitecode"]); //介绍用户标识号
                string password = parmas["password"];
                string imei = parmas["imei"];


                UserInfo userInfo = null;

                userInfo = new UserInfo();
                userInfo.UserName = account;
                userInfo.UserId = Randoms.CreateRandomValue(8);
                userInfo.Email = string.Empty;
                userInfo.Mobile = phone;

                userInfo.Salt = Randoms.CreateRandomValue(6);
                userInfo.Password = Users.CreateUserPassword(password, userInfo.Salt);
                userInfo.UserRid = 7;//普通用户 UserRanks.GetLowestUserRank().UserRid;
                userInfo.AdminGid = 1;//非管理员组

                userInfo.NickName = Randoms.CreateRandomValue(6);
                userInfo.Avatar = "";
                userInfo.PayCredits = 0;
                userInfo.RankCredits = 0;
                userInfo.VerifyEmail = 0;
                userInfo.VerifyMobile = 0;

                userInfo.LastVisitIP = WorkContext.IP;
                userInfo.LastVisitRgId = WorkContext.RegionId;
                userInfo.LastVisitTime = DateTime.Now;
                userInfo.RegisterIP = WorkContext.IP;
                userInfo.RegisterRgId = WorkContext.RegionId;
                userInfo.RegisterTime = DateTime.Now;

                userInfo.Gender = WebHelper.GetFormInt("gender");
                userInfo.RealName = WebHelper.HtmlEncode(WebHelper.GetFormString("realName"));
                userInfo.Bday = new DateTime(1900, 1, 1);
                userInfo.IdCard = WebHelper.GetFormString("idCard");
                userInfo.RegionId = WebHelper.GetFormInt("regionId");
                userInfo.Address = WebHelper.HtmlEncode(WebHelper.GetFormString("address"));
                userInfo.Bio = WebHelper.HtmlEncode(WebHelper.GetFormString("bio"));

                userInfo.InviteCode = invitecode;
                userInfo.IMEI = imei;


                //创建用户
                userInfo.Uid = Users.CreateUser(userInfo);

                //添加用户失败
                if (userInfo.Uid < 1)
                    return APIResult("error", "注册失败");

                return APIResult("success", "注册成功");
            }
            catch (Exception ex)
            {
                Logs.Write("注册失败:" + ex.Message);
                return APIResult("error", "注册失败");
            }
        }
        /// <summary>
        /// 验证IMEI号是否匹配
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidateIMEI()
        {
            NameValueCollection parmas = WorkContext.postparms;
            if (parmas.Keys.Count != 2)
            {
                return APIResult("error", "缺少请求参数");
            }
            string account = parmas["account"].Trim();
            string imei = parmas["imei"];
            PartUserInfo partUserInfo = Users.GetPartUserByMobile(account);
            if (partUserInfo.Uid <= 0)
                return APIResult("error", "账号不存在");
            if (partUserInfo.IMEI.ToLower() != imei.TrimEnd().ToLower())
            {
                return APIResult("error", "账号已在其他手机登录");
            }
            return APIResult("success", "验证通过");
        }
        /// <summary>
        /// 退出
        /// </summary>
        public ActionResult Logout()
        {
            if (WorkContext.Uid > 0)
            {
                WebHelper.DeleteCookie("web_bsp");
                Sessions.RemoverSession(WorkContext.Sid);
                OnlineUsers.DeleteOnlineUserBySid(WorkContext.Sid);
            }
            return Redirect("/");
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        public ActionResult ResetPwd()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;

                string account = parmas["account"];
                string password = parmas["password"];
                string code = parmas["code"];
                //Logs.Write(" 找回密码：" + account + "__" + password + "__" + code);
                List<MD_SMSCode> smscode = NewUser.GetSMSCodeList(1, -1, " where rtrim(a.account)='" + account + "' and rtrim(a.code)='" + code + "' and DATEDIFF(minute,getdate(),a.expiretime)  between 0 and 10 ");
                if (smscode.Count == 0)
                {
                    return APIResult("error", "验证码错误");
                }

                PartUserInfo partUserInfo = Users.GetPartUserByMobile(account);


                //生成用户新密码
                string p = Users.CreateUserPassword(password, partUserInfo.Salt);
                //设置用户新密码
                bool upres = Users.UpdateUserPasswordByMobile(account, p);


                ////清空当前用户信息
                //WebHelper.DeleteCookie("web_bsp");
                //Sessions.RemoverSession(WorkContext.Sid);
                //OnlineUsers.DeleteOnlineUserBySid(WorkContext.Sid);
                if (upres)
                {
                    return APIResult("success", "更新成功");
                }
                else
                {
                    return APIResult("error", "更新失败");
                }

            }
            catch (Exception ex)
            {
                return APIResult("error", "更新失败");
            }

        }
        /// <summary>
        /// 设置提现密码
        /// </summary>
        /// <returns></returns>
        public ActionResult DrawPwd()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;

                MD_DrawAccount draw = new MD_DrawAccount
                {
                    Account = parmas["account"],
                    Drawpwd = parmas["password"]
                };

                PartUserInfo partUserInfo = Users.GetPartUserByMobile(parmas["account"]);

                draw.Drawpwd = Users.CreateUserPassword(draw.Drawpwd, partUserInfo.Salt);
                if (parmas.AllKeys.Contains("oldpwd"))
                {
                    string oldpwd = Users.CreateUserPassword(parmas["oldpwd"], partUserInfo.Salt);
                    bool pwdres = Recharge.ValidateDrawPwd(draw.Account, oldpwd);
                    if (!pwdres)
                        return APIResult("error", "原有密码错误");
                }
                if (partUserInfo.Password == draw.Drawpwd)
                {
                    return APIResult("error", "提现密码和登录密码不能一致");
                }

                bool saveres = Recharge.UpdateDrawPWD(draw);
                if (saveres)
                    return APIResult("success", "保存成功");
                else
                    return APIResult("error", "保存失败");

            }
            catch (Exception ex)
            {
                return APIResult("error", "保存失败");
            }
        }
        /// <summary>
        /// 是否设置提现密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ExistsDrawPd()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;

                bool result = Recharge.ValidateDrawPwd(parmas["account"]);

               if(!result)
                    return APIResult("success", "未设置提现密码");
                else
                    return APIResult("success", "已设置提现密码");

            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        /// <summary>
        /// 绑定银行卡
        /// </summary>
        /// <returns></returns>
        public ActionResult SetDrawAccount()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                //包含imei号
                if (parmas.Keys.Count != 7)
                {
                    return APIResult("error", "缺少请求参数");
                }

                MD_DrawAccount draw = new MD_DrawAccount
                {
                    Account = parmas["account"],
                    Username = parmas["username"],
                    Card = parmas["cardname"],
                    Cardnum = parmas["cardnum"],
                    Cardaddress = parmas["cardaddress"],
                    Drawpwd = parmas["pwd"]
                };

                bool result = Recharge.ValidateDrawPwd(parmas["account"]);

                if (!result)
                    return APIResult("error", "未设置提现密码");
                

                PartUserInfo partUserInfo = Users.GetPartUserByMobile(parmas["account"]);

                draw.Drawpwd = Users.CreateUserPassword(draw.Drawpwd, partUserInfo.Salt);

                bool pwdres = Recharge.ValidateDrawPwd(draw.Account, draw.Drawpwd);
                if (!pwdres)
                    return APIResult("error", "提现密码错误");
                bool saveres = Recharge.UpdateDrawCardInfo(draw);
                if (saveres)
                    return APIResult("success", "保存成功");
                else
                    return APIResult("error", "保存失败");
            }
            catch (Exception ex)
            {
                return APIResult("error", "保存失败");
            }
        }
        /// <summary>
        /// 获取银行卡
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDrawAccount()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                if (parmas.Keys.Count != 2)
                {
                    return APIResult("error", "缺少请求参数");
                }

                List<MD_DrawAccount> draw = Recharge.GetDrawAccountList(1, 1, " where rtrim(b.mobile)='" + parmas["account"] + "'");
                if (draw.Count > 0)
                {
                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "Account", "Username", "Card", "Cardnum", "Cardaddress" }, true);
                    string data = JsonConvert.SerializeObject(draw[0], jsetting).ToLower();
                    return APIResult("success", data, true);
                }
                else
                {
                    return APIResult("error", "没有提现账户信息");
                }
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }

        /// <summary>
        /// 更新用户头像昵称
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateUserInfo()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                string account = parmas["account"];

                PartUserInfo user = Users.GetPartUserByMobile(account);
                UserDetailInfo userdetails = Users.GetUserDetailById(user.Uid);

                user.NickName = HttpUtility.HtmlDecode(parmas["nickname"]);
                userdetails.SignName = HttpUtility.HtmlDecode(parmas["signname"]);

                Users.UpdateUserDetail(userdetails);
                bool udres = Users.UpdatePartUser(user);
                if (udres)
                {
                    return APIResult("success", "更新成功");
                }
                else
                {
                    return APIResult("error", "更新失败");
                }

            }
            catch (Exception ex)
            {
                return APIResult("error", "更新失败");
            }
        }

        /// <summary>
        /// 更新用户头像昵称
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateImg()
        {
            try
            {
                string account = Request.Form["account"];
                
                PartUserInfo user = Users.GetPartUserByMobile(account);
                user.Avatar = "";

                HttpPostedFileBase file = Request.Files[0];

                string filepath = "/upload/imgs/";
                string data = Uploads.SaveUploadImgNoSize(file, filepath);
                string conres = string.Empty;
                if (data == "-1")
                {
                    return APIResult("error", "图片为空");
                }
                else if (data == "-2")
                    return APIResult("error", "图片类型不允许");
                else if (data == "-3")
                    return APIResult("error", "图片太小超出限制");
                user.Avatar = data;

                bool udres = Users.UpdatePartUser(user);
                if (udres)
                {
                    return APIResult("success", "更新成功");
                }
                else
                {
                    return APIResult("error", "更新失败");
                }

            }
            catch (Exception ex)
            {
                return APIResult("error", "更新失败");
            }
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UserInfo()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                string account = parmas["account"];

                PartUserInfo user = Users.GetPartUserByMobile(account);
                UserDetailInfo userdetails = Users.GetUserDetailById(user.Uid);
               
                if (user.Avatar != null && user.Avatar != string.Empty)
                    user.Avatar = BSPConfig.ShopConfig.SiteUrl + "/upload/imgs/" + user.Avatar;
                else
                    user.Avatar = BSPConfig.ShopConfig.SiteUrl + "/upload/imgs/defaultcall.png";

                string data = "{\"Account\":\"" + account + "\",\"NickName\":\"" + user.NickName + "\",\"SignName\":\"" + userdetails.SignName + "\",\"Avatar\":\"" + user.Avatar + "\"}";

                return APIResult("success", data.ToLower(), true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }

        /// <summary>
        /// 更新安装包
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateApp()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;

                AppUpdateConfigInfo update = BSPConfig.AppUpdateConfig;
                if (update.Version != parmas["version"])
                {
                    string filepath = System.Web.HttpContext.Current.Request.MapPath("/") + "update\\" + update.AppFileName;
                    bool file = new FileInfo(filepath).Exists;
                    if (file)
                    {
                        string appfilename = update.AppFileName;
                        update.AppFileName = BSPConfig.ShopConfig.SiteUrl + "/update/" + update.AppFileName;
                        string data = JsonConvert.SerializeObject(update).ToLower();
                        update.AppFileName = appfilename;
                        return APIResult("success", data, true);
                    }
                    else
                        return APIResult("error", "抱歉，应用程序已经走丢！");

                }
                else
                {
                    return APIResult("error", "已是最新版本");
                }
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        /// <summary>
        /// 获取首页数据
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeData()
        {
            try
            {
                //DataTable dt = NewUser.HomeData();
                //if (dt.Rows.Count == 0)
                //{
                //    return APIResult("error", "获取失败");
                //}
                //else
                //{
                //    string data = "{\"users\":" + dt.Rows[0]["users"].ToString() + ",\"money\":" + dt.Rows[0]["money"].ToString() + ",\"percent\":\"98\"}";
                //    return APIResult("success", data, true);
                //}

                BaseInfo info = BSPConfig.BaseConfig.BaseList.Find(x => x.Key == "盈利数据");
                string data = "{\"users\":\"" + info.Account + "\",\"money\":\"" + info.Name + "\",\"percent\":\""+info.Bank+"\"}";
                return APIResult("success", data, true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }

    }
}
