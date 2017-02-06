using System;
using System.Text;

using OWZX.Core;

namespace OWZX.Services
{
    /// <summary>
    /// 短信操作管理类
    /// </summary>
    public partial class SMSes
    {
        private static object _locker = new object();//锁对象
        private static ISMSStrategy _ismsstrategy = null;//短信策略
        private static SMSConfigInfo _smsconfiginfo = null;//短信配置
        private static ShopConfigInfo _shopconfiginfo = null;//商城配置

        static SMSes()
        {
            _ismsstrategy = BSPSMS.Instance;
            _smsconfiginfo = BSPConfig.SMSConfig;
            _shopconfiginfo = BSPConfig.ShopConfig;
            _ismsstrategy.Url = _smsconfiginfo.Url;
            _ismsstrategy.App_Key = _smsconfiginfo.App_Key;
            _ismsstrategy.App_Secret = _smsconfiginfo.App_Secret;
            _ismsstrategy.UserName = _smsconfiginfo.UserName;
            _ismsstrategy.Password = _smsconfiginfo.Password;
        }

        /// <summary>
        /// 重置短信配置
        /// </summary>
        public static void ResetSMS()
        {
            lock (_locker)
            {
                _smsconfiginfo = BSPConfig.SMSConfig;
                _ismsstrategy.Url = _smsconfiginfo.Url;
                _ismsstrategy.App_Key = _smsconfiginfo.App_Key;
                _ismsstrategy.App_Secret = _smsconfiginfo.App_Secret;
                _ismsstrategy.UserName = _smsconfiginfo.UserName;
                _ismsstrategy.Password = _smsconfiginfo.Password;
            }
        }

        /// <summary>
        /// 重置商城信息
        /// </summary>
        public static void ResetShop()
        {
            lock (_locker)
            {
                _shopconfiginfo = BSPConfig.ShopConfig;
            }
        }

        /// <summary>
        /// 发送找回密码短信
        /// </summary>
        /// <param name="to">接收手机</param>
        /// <param name="code">验证值</param>
        /// <returns></returns>
        public static bool SendFindPwdMobile(string to, string code)
        {
            StringBuilder body = new StringBuilder(_smsconfiginfo.FindPwdBody);
            body.Replace("{shopname}", _shopconfiginfo.WebName);
            body.Replace("{code}", code);
            return _ismsstrategy.Send(to, body.ToString());
        }

        /// <summary>
        /// 安全中心发送验证手机短信
        /// </summary>
        /// <param name="to">接收手机</param>
        /// <param name="code">验证值</param>
        /// <returns></returns>
        public static bool SendSCVerifySMS(string to, string code)
        {
            StringBuilder body = new StringBuilder(_smsconfiginfo.SCVerifyBody);
            body.Replace("{shopname}", _shopconfiginfo.WebName);
            body.Replace("{code}", code);
            return _ismsstrategy.Send(to, body.ToString());
        }

        /// <summary>
        /// 安全中心发送确认更新手机短信
        /// </summary>
        /// <param name="to">接收手机</param>
        /// <param name="code">验证值</param>
        /// <returns></returns>
        public static bool SendSCUpdateSMS(string to, string code)
        {
            StringBuilder body = new StringBuilder(_smsconfiginfo.SCUpdateBody);
            body.Replace("{shopname}", _shopconfiginfo.WebName);
            body.Replace("{code}", code);
            return _ismsstrategy.Send(to, body.ToString());
        }

        /// <summary>
        /// 发送注册欢迎短信
        /// </summary>
        /// <param name="to">接收手机</param>
        /// <returns></returns>
        public static bool SendWebcomeSMS(string to)
        {
            StringBuilder body = new StringBuilder(_smsconfiginfo.WebcomeBody);
            body.Replace("{shopname}", _shopconfiginfo.WebName);
            body.Replace("{regtime}", CommonHelper.GetDateTime());
            body.Replace("{mobile}", to);
            return _ismsstrategy.Send(to, body.ToString());
        }

        /// <summary>
        /// 阿里大于发送短信
        /// </summary>
        /// <param name="to">接收人号码</param>
        /// <param name="type">类型</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public static bool SendAliSMS(string to, string type,string code)
        {
            string smssignname = "";
            string tempcode = "";
            string smsparam = "";
            smssignname = "黑米壳";
            if (type == "login")
            {
                //smssignname = "登录验证";
                tempcode = "SMS_14765471";
                smsparam = "{\"code\":\"" + code + "\",\"product\":\"黑米壳\"}";
            }
            else if (type == "register")
            {
                //smssignname = "注册验证";
                tempcode = "SMS_14765469";
                smsparam = "{\"code\":\"" + code + "\",\"product\":\"黑米壳\"}";
            }
            else if (type == "resetpwd")
            {
                //smssignname = "变更验证";
                tempcode = "SMS_14765467";
                smsparam = "{\"code\":\"" + code + "\",\"product\":\"黑米壳\"}";
            }
            else if (type == "findpwd")
            {
                //smssignname = "身份验证";
                tempcode = "SMS_14765466";
                smsparam = "{\"code\":\"" + code + "\",\"product\":\"黑米壳\"}";
            }
            return _ismsstrategy.AliSend(to, smsparam, smssignname, tempcode);
        }

        /// <summary>
        /// 广州圣亚发送短信
        /// </summary>
        /// <param name="to">接收人号码</param>
        /// <param name="body">内容</param>
        /// <returns></returns>
        public static bool SendSY(string to, string body)
        {
            return _ismsstrategy.SendSY(to, body);
        }
    }
}
