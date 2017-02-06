using log4net;
using Newtonsoft.Json;
using OWZX.Core;
using OWZX.Core.Alipay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.SMSStrategy.OWZX
{
    //阿里大于短信平台
    public partial class SMSStrategy : ISMSStrategy
    {
        //private string _app_url;
        private string _app_key;
        private string _app_secret;
        ///// <summary>
        ///// 短信服务器地址
        ///// </summary>
        //public string App_Url
        //{
        //    get { return _app_url; }
        //    set { _app_url = value; }
        //}
        /// <summary>
        /// 应用key
        /// </summary>
        public string App_Key
        {
            get { return _app_key; }
            set { _app_key = value; }
        }
        /// <summary>
        /// 应用secret
        /// </summary>
        public string App_Secret
        {
            get { return _app_secret; }
            set { _app_secret = value; }
        }
        private readonly static ILog logger = LogManager.GetLogger("短信AliStrategy");
        /// <summary>
        /// 阿里大于发送短信
        /// </summary>
        /// <param name="to">接收人号码</param>
        /// <param name="smsparam">短信模板变量</param>
        /// <param name="smssignname">短信签名名称</param>
        /// <param name="tempcode">模板编号</param>
        /// <returns></returns>
        public bool AliSend(string to, string smsparam, string smssignname, string tempcode)
        {


            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = BuildParms(to, smsparam, smssignname, tempcode);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder();
            
                query.Append(_app_secret);
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(value);
                }
            }

            // 第三步：使用MD5/HMAC加密
            byte[] bytes;

            query.Append(_app_secret);
                MD5 md5 = MD5.Create();
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));

            // 第四步：把二进制转化为大写的十六进制
            StringBuilder signres = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                signres.Append(bytes[i].ToString("X2"));
            }

            string sign = signres.ToString();

            sortedParams["sign"] = sign;

            //把参数组中所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
            string strRequestData = AlipayCore.CreateLinkStringUrlencode(sortedParams, Encoding.UTF8);

            string result = WebHelper.GetRequestData(_url+"?"+strRequestData ,"get");
            if (result.Contains("<success>true</success>"))
                return true;
            else
            {
                logger.Error(result);
                return false;
            }
            
        }
        public SortedDictionary<string, string> BuildParms(string to, string smsparam, string smssignname, string tempcode)
        {
            SortedDictionary<string, string> aliparms = new SortedDictionary<string, string>(StringComparer.Ordinal);
            aliparms["method"] = "alibaba.aliqin.fc.sms.num.send";
            aliparms["app_key"] = _app_key;
            aliparms["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            aliparms["v"] = "2.0";
            aliparms["sign_method"] = "md5";//签名的摘要算法
            
            aliparms["sms_type"] = "normal";
            aliparms["sms_free_sign_name"] = smssignname;//短信签名 名称
            aliparms["sms_param"] = smsparam;//短信模板变量
            aliparms["rec_num"] = to; //接收号码
            aliparms["sms_template_code"] = tempcode; //模板编号

            return aliparms;
          
        }


       
    }
}
