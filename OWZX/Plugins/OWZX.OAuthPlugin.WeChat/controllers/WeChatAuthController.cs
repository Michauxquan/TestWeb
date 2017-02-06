using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.OAuthPlugin.WeChat;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace OWZX.Web.Controllers
{
    /// <summary>
    /// 前台WeChat开放授权控制器类
    /// </summary>
    public class WeChatOAuthController : BaseWebController
    {
        PluginSetInfo pluginSetInfo = PluginUtils.GetPluginSet();
        /// <summary>
        /// 登陆
        /// </summary>
        public ActionResult Login()
        {
            //返回url
            string returnUrl = WebHelper.GetQueryString("returnUrl");
            if (returnUrl.Length == 0)
                returnUrl = "/";

            if (WorkContext.ShopConfig.LoginType == "")
                return PromptView(returnUrl, "商城目前已经关闭登陆功能!");
            if (WorkContext.Uid > 0)
                return PromptView(returnUrl, "您已经登录，无须重复登录!");


            string returnurl = string.Format(@"http://{0}{1}", BSPConfig.ShopConfig.SiteUrl, Url.Action("CallBack"));
            string url = WeiXinOAuth.GetWeiXinRqUrl(pluginSetInfo.AppID, pluginSetInfo.AppSecret, returnurl, "", "", "", "code");
            //WebHelper.GetRequestData(url, "");
            return Redirect(url);
        }

        /// <summary>
        /// 回调
        /// </summary>
        public ActionResult CallBack()
        {
            //返回url
            string returnUrl = WebHelper.GetQueryString("returnUrl");
            if (returnUrl.Length == 0)
                returnUrl = "/";
            string[] rtval = Request.QueryString.AllKeys;
            if (Array.IndexOf<string>(rtval, "code") == -1)
            {
                return PartialView("您未授权微信登录，请重新扫描登陆");
            }

            if (WorkContext.ShopConfig.LoginType == "")
                return PromptView(returnUrl, "目前已经关闭登陆功能!");
            if (WorkContext.Uid > 0)
                return PromptView(returnUrl, "您已经登录，无须重复登录!");
            //返回的随机值
            string backSalt = WebHelper.GetQueryString("state");
            //Authorization Code
            string code = WebHelper.GetQueryString("code");

            //获取access_token
            string tokenurl = WeiXinOAuth.GetWeiXinRqUrl(pluginSetInfo.AppID, pluginSetInfo.AppSecret, "", code, "", "", "access_token");
            WeiXinAccessTokenResult token = WeiXinOAuth.GetAccessToken(tokenurl, pluginSetInfo.AppID, pluginSetInfo.AppSecret);
            if (token.ErrorResult.errcode == 40029)
            {
                return PartialView("获取微信授权码错误，请重新扫描登陆");
            }


            //判断此用户是否已经存在
            int uid = OAuths.GetUidByOpenIdAndServer(token.SuccessResult.openid, pluginSetInfo.Server);
            if (uid > 0)//存在时
            {
                PartUserInfo partUserInfo = Users.GetPartUserById(uid);
                //更新用户最后访问
                Users.UpdateUserLastVisit(partUserInfo.Uid, DateTime.Now, WorkContext.IP, WorkContext.RegionId);
                
                ShopUtils.SetUserCookie(partUserInfo, -1, "web");

                return Redirect("/");
            }
            else
            {
                //获取用户信息
                string userurl = WeiXinOAuth.GetWeiXinRqUrl("", "", "", "", token.SuccessResult.openid, token.SuccessResult.access_token, "openid");
                WeiXinUserInfoResult userinfo = WeiXinOAuth.GetWeiXinUserInfo(userurl);
                if (userinfo.ErrorMsg.errcode == 40003)
                {
                    return PartialView("获取用户信息失败，请重新扫描登陆");
                }


                UserInfo userInfo = OAuths.CreateOAuthUser(userinfo.UserInfo.nickname, pluginSetInfo.UNamePrefix, token.SuccessResult.openid,
                    pluginSetInfo.Server, WorkContext.RegionId, userinfo.UserInfo.unionid);
                if (userInfo != null)
                {
                    ShopUtils.SetUserCookie(userInfo, -1, "web");
                    return Redirect("/");
                }
                else
                {
                    return PartialView("用户创建失败");
                }

            }
        }
    }
}
