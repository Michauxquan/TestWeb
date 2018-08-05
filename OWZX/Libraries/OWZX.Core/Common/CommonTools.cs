using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Core
{
    public class CommonTools
    {

        public static CommonTools commontools = new CommonTools();

        public Dictionary<string, string> GetIpNameDic(string ip)
        {
            Dictionary<string, string> ipDic = new Dictionary<string, string>();
            ipDic.Add("ip", "");
            ipDic.Add("country", "");
            ipDic.Add("province", "");
            ipDic.Add("city", "");
            ipDic.Add("district", "");
            if (string.IsNullOrEmpty(ip) || ip == "127.0.0.1")
            {
                ip = "";
            }
            //string result = HttpUtils.HttpGet("http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=json&ip=" + ip, "");
            string result = HttpUtils.HttpGet("http://ip.taobao.com/service/getIpInfo.php?ip=" + ip, "");

            if (!string.IsNullOrEmpty(result) && (result.Contains("province") || result.Contains("region")))
            {
                Dictionary<string, object> getResult =  JsonHelper.JsonToDictionary(result);
                if (getResult.ContainsKey("country"))
                {
                    ipDic["country"] = getResult["country"].ToString();
                }
                if (getResult.ContainsKey("province"))
                {
                    ipDic["province"] = getResult["province"].ToString();
                }
                else if (getResult.ContainsKey("region"))
                {
                    ipDic["province"] = getResult["region"].ToString();
                }
                if (getResult.ContainsKey("city"))
                {
                    ipDic["city"] = getResult["city"].ToString();
                }
                if (getResult.ContainsKey("district"))
                {
                    ipDic["district"] = getResult["district"].ToString();
                }
            }

            return ipDic;
        }
        public string GetIpName(string ip)
        {
            var result = GetIpNameDic(ip);

            return result["province"]+" "+ result["city"];
        }
    }
}
