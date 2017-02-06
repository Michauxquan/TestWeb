using System;
using System.Web;
using System.Text;

using OWZX.Core;

namespace OWZX.SMSStrategy.OWZX
{
    /// <summary>
    /// 简单短信策略
    /// </summary>
    public partial class SMSStrategy : ISMSStrategy
    {
        private string _url;
        private string _username;
        private string _password;
        private Encoding _encoding = Encoding.GetEncoding("gbk");

        /// <summary>
        /// 短信服务器地址
        /// </summary>
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// 短信账号
        /// </summary>
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        /// <summary>
        /// 短信密码
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="to">接收人号码</param>
        /// <param name="body">短信内容</param>
        /// <returns>是否发送成功</returns>
        public bool SendSY(string to, string body)
        {
            //此方法适用于广州圣亚短信
            string url = _url;//HttpUtility.UrlEncode(body, _encoding)
            string data = _url+"?"+string.Format("{0}&user={1}&passwd={2}&msg={3}&phone={4}", "act=sendmsg",HttpUtility.UrlEncode(_username,Encoding.UTF8), _password, body, to);
            string content = WebHelper.GetRequestData(data, "get", null);
            //以下各种情况的判断要根据不同平台具体调整
            int status=int.Parse(content);
            if (status > 0)
            {
                return true;
            }
            else
            {
                string error = string.Empty;
                switch (status)
                {
                    case 0: error = "失败";
                        break;
                    case -1: error = "账号错误";
                        break;
                    case -2: error = "密码错误";
                        break;
                    case -3: error = "账号已停用";
                        break;
                    case -4: error = "内容或号码为空";
                        break;
                    case -5: error = "内容包含非法关键词";
                        break;
                    case -6: error = "时间格式错误";
                        break;
                    case -7: error = "余额不足";
                        break;
                    case -100: error = "无效参数";
                        break;
                    case -200: error = "系统错误";
                        break;
                }
                return false;
            }
        }
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="to">接收人号码</param>
        /// <param name="body">短信内容</param>
        /// <returns>是否发送成功</returns>
        public bool Send(string to, string body)
        {
            //此方法适用于国都短信
            string url = string.Format("{0}?OperID={1}&OperPass={2}&DesMobile={3}&Content={4}&ContentType=15", _url, _username, _password, to, HttpUtility.UrlEncode(body, _encoding));
            string content = WebHelper.GetRequestData(url, "get", null);

            //以下各种情况的判断要根据不同平台具体调整
            if (content.Contains("<code>03</code>"))
            {
                return true;
            }
            else
            {
                if (content.Substring(0, 1) == "2") //余额不足
                {
                    //"手机短信余额不足";
                    //TODO
                }
                else
                {
                    //短信发送失败的其他原因
                    //TODO
                }
                return false;
            }
        }


       
    }
}
