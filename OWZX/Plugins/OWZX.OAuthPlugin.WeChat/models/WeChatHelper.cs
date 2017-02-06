using Newtonsoft.Json;
using OWZX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.OAuthPlugin.WeChat
{
    class WeChatHelper
    {
       static PluginSetInfo pluginSetInfo = PluginUtils.GetPluginSet();

       
        /// <summary>
        /// 根据AppID和AppSecret获得access token(默认过期时间为2小时)
        /// </summary>
        /// <returns>Dictionary</returns>
        public static Dictionary<string, object> get_access_token()
        {
            //获得配置信息
            
            string send_url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" +
                              pluginSetInfo.AppID + "&secret=" + pluginSetInfo.AppSecret + "";
            //发送并接受返回值
            string result = WebHelper.GetRequestData(send_url, "");
            if (result.Contains("errmsg"))
            {
                return null;
            }
            try
            {
                Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                return dic;
            }
            catch
            {
                return null;
            }
        } /// <summary>
        /// 取得临时的Access Token(默认过期时间为2小时)
        /// </summary>
        /// <param name="code">临时Authorization Code</param>
        /// <param name="state">防止CSRF攻击，成功授权后回调时会原样带回</param>
        /// <returns>Dictionary</returns>
        public static Dictionary<string, object> get_access_token(string code, string state)
        {
            //获得配置信息
            string send_url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" +
                              pluginSetInfo.AppID + "&secret=" + pluginSetInfo.AppSecret + "&code=" + code + "&grant_type=authorization_code";
            //发送并接受返回值
            string result = WebHelper.GetRequestData(send_url,"");
            if (result.Contains("errmsg"))
            {
                return null;
            }
            try
            {
                Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                return dic;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 根据access_token判断access_token是否过期
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns>true表示未失效</returns>
        public static bool check_access_token(string access_token)
        {
            //获得配置信息
            string send_url = "https://api.weixin.qq.com/sns/auth?access_token=" + access_token + "&openid=" + pluginSetInfo.AppID ;
            //发送并接受返回值
            string result = WebHelper.GetRequestData(send_url, "");
            try
            {
                Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                if (dic.ContainsKey("errmsg"))
                {
                    if (dic["errmsg"].ToString() == "ok")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                return false;

            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 若fresh_token已过期则根据refresh_token取得新的refresh_token
        /// </summary>
        /// <param name="refresh_token">refresh_token</param>
        /// <returns>Dictionary</returns>
        public static Dictionary<string, object> get_refresh_token(string refresh_token)
        {
            //获得配置信息
            string send_url =
                "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=" +
                              pluginSetInfo.AppID + "&grant_type=refresh_token&refresh_token=" + refresh_token;
            //发送并接受返回值
            string result = WebHelper.GetRequestData(send_url, "");
            if (result.Contains("errmsg"))
            {
                return null;
            }
            try
            {
                Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                return dic;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取登录用户自己的基本资料
        /// </summary>
        /// <param name="access_token">临时的Access Token</param>
        /// <param name="open_id">用户openid</param>
        /// <returns>Dictionary</returns>
        public static Dictionary<string, object> get_user_info(string access_token, string open_id)
        {
            //获得配置信息
            //发送并接受返回值   
            string send_url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + open_id;
            //发送并接受返回值
            string result = WebHelper.GetRequestData(send_url,"");
            if (result.Contains("errmsg"))
            {
                return null;
            }
            //反序列化JSON
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
            return dic;
        }
    }
}
