using System;
using System.Web.Mvc;
using System.Web.Routing;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.IO;
using OWZX.Web.Models;
using System.Data;
using OWZX.Model;
using System.Web;

namespace OWZX.Web.Controllers
{
    /// <summary>
    /// 首页控制器类
    /// </summary>
    public partial class HomeController : BaseWebController
    {
        /// <summary>
        /// 首页
        /// </summary>
        public ActionResult Index()
        {
            //首页的数据需要在其视图文件中直接调用，所以此处不再需要视图模型
            return View();
        }
        /// <summary>
        /// 推广
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Expand(string account = "")
        {
            if (account == string.Empty)
                return Content("访问的分享地址无效");
            ViewData["account"] = account;
            return View();
        }
        /// <summary>
        /// 验证手机号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="paccount"></param>
        /// <returns></returns>
        public ActionResult Validate(string account, string paccount)
        {
            if (account == string.Empty)
            {
                return AjaxResult("error", "请输入手机号！");
            }
            else if (account.Length != 11)
            {
                return AjaxResult("error", "请输入有效手机号！");
            }
            else if (paccount == string.Empty || paccount.Trim().Length != 11)
            {
                return AjaxResult("error", "访问的分享地址无效！");
            }
            else if (paccount == account)
            {
                return AjaxResult("error", "推广功能只能推荐好友注册呦！");
            }
            else
            {

                //bool result = ValidateHelper.IsGZYDModbile(account);
                //if (!result)
                //{
                //    return AjaxResult("error", "主人！请使用您的广州移动号码申请《黑米壳通行证》有更多惊喜等着你哟！！");
                //}
                //else
                {
                    //验证手机号是否已经领过
                    DataTable dt = Users.ValidateUser(account);
                    if (dt.Rows.Count > 0)
                    {
                        //return AjaxResult("error", "您已经领取过,每个手机号只能领取一次奥！");
                        return AjaxResult("error", "99");
                    }


                    //发送短信验证码，将验证码记录到数据库
                    string code = Randoms.CreateRandomValue(6);

                    PartUserInfo puser = Users.GetPartUserByMobile(paccount);
                    if (puser == null)
                    {
                        return AjaxResult("error", "访问的分享地址无效！");
                    }
                    //记录数据库
                    bool addcode = Users.AddInviteInfo(paccount, account, code);
                    if (addcode)
                    {
                        //发送短信
                        try
                        {
                            bool smsres = SMSes.SendAliSMS(account, "register", code);
                            if (!smsres)
                            {
                                Users.DelInviteInfo(account);
                                return AjaxResult("error", "短信验证码发送失败,请稍后再试");
                            }
                            else
                            {
                                return AjaxResult("success", "发送成功");
                            }
                        }
                        catch (Exception ex)
                        {
                            Users.DelInviteInfo(account);
                            return AjaxResult("error", "短信验证码发送失败,请稍后再试");
                        }
                    }
                    else
                    {
                        return AjaxResult("error", "发送失败");
                    }
                }
            }

        }
        /// <summary>
        /// 推广
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Expand(string account, string code)
        {
            if (account == "")
                return AjaxResult("error", "请输入手机号");
            if (code == "")
                return AjaxResult("error", "请输入验证码");
            bool result = ValidateHelper.IsMobile(account);
            //bool result = ValidateHelper.IsGZYDModbile(account);
            //if (!result)
            //{
            //    return AjaxResult("error", "主人！请使用您的广州移动号码申请《黑米壳通行证》有更多惊喜等着你哟！！");
            //}
            //else
            {
                bool coderes = Users.ValidateCode(account, code);
                if (coderes)
                {
                    AppUpdateConfigInfo update = BSPConfig.AppUpdateConfig;


                    string data = JsonConvert.SerializeObject(update).ToLower();
                    bool file = new FileInfo(Server.MapPath("~/") + update.DownLoadUrl).Exists;
                    if (file)
                    {
                        string appfilename = BSPConfig.ShopConfig.SiteUrl + "/" + update.DownLoadUrl;
                        return AjaxResult("success", appfilename);
                    }
                    else
                        return AjaxResult("error", "抱歉，应用程序已经走丢！");
                }
                else
                {
                    return AjaxResult("error", "手机号或验证码错误");
                }
            }
        }
        //下载
        public ActionResult Download()
        {
            //AppUpdateConfigInfo update = BSPConfig.AppUpdateConfig;
            //ViewData["url"] = BSPConfig.ShopConfig.SiteUrl + "/" + update.DownLoadUrl;
            return View();
        }

        public ActionResult DlApp()
        {
            return View();
        
        }

        public ActionResult Down()
        {
            return View();
        }
        /// <summary>
        /// 系统公告
        /// </summary>
        /// <returns></returns>
        public ActionResult Notice(int id=-1)
        {
            //List<BaseInfoModel> listbase = AdminBaseInfo.GetBaseInfoList(-1, "  and title='系统公告'");
            string where=string.Empty ;
            if (id > 0)
                where = " where newsid="+id;
            DataTable dt = News.GetNewsList(10, 1, where);
            List<MD_NewsInfo> list = (List<MD_NewsInfo>)ModelConvertHelper<MD_NewsInfo>.ConvertToModel(dt);

            return View(list);
        }
        /// <summary>
        /// 帮助信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Help()
        {

            List<BaseInfoModel> listbase = AdminBaseInfo.GetBaseInfoList(-1, "  and title='帮助信息'");
            return View(listbase);
        }
    }
}
