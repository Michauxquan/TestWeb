using Newtonsoft.Json;
using OWZX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.OAuthPlugin.WeChat
{
   public class WeiXinOAuth
    {
        /// <summary>
        /// 获取请求地址
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetWeiXinRqUrl(string appId, string appSecret, string redirectUrl,string code,string openid,string token,string type)
        {
            Random r = new Random();
            string url = string.Empty;
            switch (type)
            {
                case "code": //微信登录授权
                    url = "https://open.weixin.qq.com/connect/qrconnect?appid=" 
                        + appId + "&redirect_uri=" + redirectUrl + "&response_type=code&scope=snsapi_login&state=STATE#wechat_redirect";
                    break;
                case "access_token"://获取access_token
                    url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid="
                        + appId + "&secret=" + appSecret + "&code=" + code + "&grant_type=authorization_code";
                    break;
                case "refresh_token"://刷新access_token
                    url = "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid="
                        + appId + "&grant_type=refresh_token&refresh_token=REFRESH_TOKEN";
                    break;
                case "openid"://获取用户信息
                    
                    url = "https://api.weixin.qq.com/sns/userinfo?access_token="+
                        token + "&openid=" + openid + "&lang=zh_CN"; 
                    break;
            }
           
            return url;
        }
        /// <summary>
        /// 通过code获取access_token
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static WeiXinAccessTokenResult GetWeiXinAccessToken(string url,string appId, string appSecret, string code)
        {
            //string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + appId + "&secret=" + appSecret +
            //    "&code=" + code + "&grant_type=authorization_code";
            string jsonStr = WebHelper.GetRequestData(url, ""); 
            WeiXinAccessTokenResult result = new WeiXinAccessTokenResult();
            if (jsonStr.Contains("errcode"))
            {
                WeiXinErrorMsg errorResult = new WeiXinErrorMsg();
                errorResult = JsonConvert.DeserializeObject<WeiXinErrorMsg>(jsonStr);
                result.ErrorResult = errorResult;
                result.Result = false;
            }
            else
            {
                WeiXinAccessTokenModel model = new WeiXinAccessTokenModel();
                model = JsonConvert.DeserializeObject<WeiXinAccessTokenModel>(jsonStr);
                result.SuccessResult = model;
                result.Result = true;
            }
            return result;
        }

        /*accesstoken 目前有效时间7200s
         */

        /// <summary>
        /// 获取每次操作微信API的Token访问令牌
        /// </summary>
        /// <param name="appid">应用ID</param>
        /// <param name="secret">开发者凭据</param>
        /// <returns></returns>
        public static WeiXinAccessTokenResult GetAccessToken(string url, string appid, string secret)
        {
            //正常情况下access_token有效期为7200秒,这里使用缓存设置短于这个时间即可
            WeiXinAccessTokenResult access_token = MemoryCacheHelper.GetCacheItem<WeiXinAccessTokenResult>("access_token", delegate()
            {
                WeiXinAccessTokenResult result = new WeiXinAccessTokenResult();
                string jsonStr = HttpGetAccessToken(url, appid, secret);
                if (jsonStr.Contains("errcode"))
                {
                    WeiXinErrorMsg errorResult = new WeiXinErrorMsg();
                    errorResult = JsonConvert.DeserializeObject<WeiXinErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    WeiXinAccessTokenModel model = new WeiXinAccessTokenModel();
                    model = JsonConvert.DeserializeObject<WeiXinAccessTokenModel>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }
                return result;
            },
                new TimeSpan(0, 0, 7000)//7000秒过期
            );

            return access_token;
        }
        private static string HttpGetAccessToken(string url,string appid, string secret)
        {
            string result = WebHelper.GetRequestData(url, "");
            return result;
        }
        /// <summary>
        /// 拉取用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static WeiXinUserInfoResult GetWeiXinUserInfo(string url)
        {
            //string url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + accessToken + "&openid=" + openId + "&lang=zh_CN";
            string jsonStr = WebHelper.GetRequestData(url, ""); 
            WeiXinUserInfoResult result = new WeiXinUserInfoResult();
            if (jsonStr.Contains("errcode"))
            {
                WeiXinErrorMsg errorResult = new WeiXinErrorMsg();
                errorResult = JsonConvert.DeserializeObject<WeiXinErrorMsg>(jsonStr);
                result.ErrorMsg = errorResult;
                result.Result = false;
            }
            else
            {
                WeiXinUserInfo userInfo = new WeiXinUserInfo();
                userInfo = JsonConvert.DeserializeObject<WeiXinUserInfo>(jsonStr);
                result.UserInfo = userInfo;
                result.Result = true;
            }
            return result;
        }
    }
}
