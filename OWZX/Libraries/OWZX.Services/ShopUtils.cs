using System;
using System.IO;
using System.Web;

using OWZX.Core;

namespace OWZX.Services
{
    public partial class ShopUtils
    {
        #region  加密/解密

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="encryptStr">加密字符串</param>
        public static string AESEncrypt(string encryptStr)
        {
            return SecureHelper.AESEncrypt(encryptStr, BSPConfig.ShopConfig.SecretKey);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="decryptStr">解密字符串</param>
        public static string AESDecrypt(string decryptStr)
        {
            return SecureHelper.AESDecrypt(decryptStr, BSPConfig.ShopConfig.SecretKey);
        }

        #endregion

        #region Cookie

        /// <summary>
        /// 获得用户sid
        /// </summary>
        /// <returns></returns>
        public static string GetSidCookie(string prefixkey)
        {
            return WebHelper.GetCookie(prefixkey+"_sid");
        }

        /// <summary>
        /// 设置用户sid
        /// </summary>
       /// <param name="sid"></param>
       /// <param name="prefixkey">前缀 区分前端和后台</param>
        public static void SetSidCookie(string sid,string prefixkey)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[prefixkey+"_sid"];
            if (cookie == null)
                cookie = new HttpCookie(prefixkey + "_sid");

            cookie.Value = sid;
            cookie.Expires = DateTime.Now.AddDays(15);
            string cookieDomain = BSPConfig.ShopConfig.CookieDomain;
            if (cookieDomain.Length != 0)
                cookie.Domain = cookieDomain;

            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 获得用户id
        /// </summary>
        /// <returns></returns>
        public static int GetUidCookie(string prefixkey)
        {
            return TypeHelper.StringToInt(GetBSPCookie(prefixkey,"uid"), -1);
        }

        /// <summary>
        /// 设置用户id
        /// </summary>
        public static void SetUidCookie(int uid,string prefixkey)
        {
            SetBSPCookie("uid", uid.ToString(), prefixkey);
        }

        /// <summary>
        /// 获得cookie密码
        /// </summary>
        /// <returns></returns>
        public static string GetCookiePassword(string prefixkey)
        {
            return WebHelper.UrlDecode(GetBSPCookie(prefixkey,"password"));
        }

        /// <summary>
        /// 解密cookie密码
        /// </summary>
        /// <param name="cookiePassword">cookie密码</param>
        /// <returns></returns>
        public static string DecryptCookiePassword(string cookiePassword)
        {
            return AESDecrypt(cookiePassword).Trim();
        }

        /// <summary>
        /// 设置cookie密码
        /// </summary>
        public static void SetCookiePassword(string password, string prefixkey)
        {
            SetBSPCookie("password", WebHelper.UrlEncode(AESEncrypt(password)),prefixkey);
        }

        /// <summary>
        /// 设置用户
        /// </summary>
        /// <param name="partUserInfo"></param>
        /// <param name="expires"></param>
        /// <param name="key">区分前端和后台</param>
        public static void SetUserCookie(PartUserInfo partUserInfo, int expires,string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key + "_bsp"];
            if (cookie == null)
                cookie = new HttpCookie(key + "_bsp");

            cookie.Values["uid"] = partUserInfo.Uid.ToString();
            cookie.Values["password"] = WebHelper.UrlEncode(AESEncrypt(partUserInfo.Password));
            if (expires > 0)
            {
                cookie.Values["expires"] = expires.ToString();
                cookie.Expires = DateTime.Now.AddDays(expires);
            }
            string cookieDomain = BSPConfig.ShopConfig.CookieDomain;
            if (cookieDomain.Length != 0)
                cookie.Domain = cookieDomain;

            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 获得cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetBSPCookie(string prefixkey, string key)
        {
            return WebHelper.GetCookie(prefixkey+"_bsp", key);
        }

        /// <summary>
        /// 设置cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="prefixkey">区分前端和后台</param>
        public static void SetBSPCookie(string key, string value,string prefixkey)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[prefixkey+"_bsp"];
            if (cookie == null)
                cookie = new HttpCookie(prefixkey + "_bsp");

            cookie[key] = value;

            int expires = TypeHelper.StringToInt(cookie.Values["expires"]);
            if (expires > 0)
                cookie.Expires = DateTime.Now.AddDays(expires);

            string cookieDomain = BSPConfig.ShopConfig.CookieDomain;
            if (cookieDomain.Length != 0)
                cookie.Domain = cookieDomain;

            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 获得访问referer
        /// </summary>
        public static string GetRefererCookie()
        {
            string referer = WebHelper.UrlDecode(WebHelper.GetCookie("referer"));
            if (referer.Length == 0)
                referer = "/";
            return referer;
        }

        /// <summary>
        /// 设置访问referer
        /// </summary>
        public static void SetRefererCookie(string url)
        {
            WebHelper.SetCookie("referer", WebHelper.UrlEncode(url));
        }

        /// <summary>
        /// 获得后台访问referer
        /// </summary>
        public static string GetAdminRefererCookie()
        {
            string adminReferer = WebHelper.UrlDecode(WebHelper.GetCookie("adminreferer"));
            if (adminReferer.Length == 0)
                adminReferer = "/admin/home/shopruninfo";
            return adminReferer;
        }

        /// <summary>
        /// 设置后台访问referer
        /// </summary>
        public static void SetAdminRefererCookie(string url)
        {
            WebHelper.SetCookie("adminreferer", WebHelper.UrlEncode(url));
        }

        #endregion
    }
}
