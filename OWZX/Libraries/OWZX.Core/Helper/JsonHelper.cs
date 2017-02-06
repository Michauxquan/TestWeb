using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace OWZX.Core
{
    public class JsonHelper
    {
        public JsonHelper()
        {

        }
        /// <summary>
        /// ajax请求结果
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="content">内容</param>
        /// <param name="isObject">是否为对象</param>
        /// <returns></returns>
        public static string JsonResult(string state, string content, bool isObject=false)
        {
            return string.Format("{0}\"state\":\"{1}\",\"content\":{2}{3}{4}{5}", "{", state, isObject ? "" : "\"", content, isObject ? "" : "\"", "}");
        }
        /// <summary>
        /// Json转Dictionary
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<string, object> JsonToDictionary(string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }
        /// <summary>
        /// 获取json中某个指定属性的值
        /// </summary>
        /// <param name="json"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetJsonValueByKey(string json, string key)
        {
            string jsvalue = JsonHelper.JsonToDictionary(json)[key].ToString();

            return jsvalue;   //获取指定属性的值  
        }
        /// <summary>
        /// 把对象序列化 JSON 字符串 
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象实体</param>
        /// <returns>JSON字符串</returns>
        public static string GetJson<T>(T obj)
        {
            //记住 添加引用 System.ServiceModel.Web 
            /**
             * 如果不添加上面的引用,System.Runtime.Serialization.Json; Json是出不来的哦
             * */
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                json.WriteObject(ms, obj);
                string szJson = Encoding.UTF8.GetString(ms.ToArray());
                return szJson;
            }
        }
        /// <summary>
        /// 把JSON字符串还原为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="szJson">JSON字符串</param>
        /// <returns>对象实体</returns>
        public static T ParseFormJson<T>(string szJson)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
            {
                DataContractJsonSerializer dcj = new DataContractJsonSerializer(typeof(T));
                return (T)dcj.ReadObject(ms);
            }
        }
        /// <summary>
        /// 返回分页JSon字符串
        /// </summary>
        /// <param name="rows">数据行</param>
        /// <param name="total">总条数</param>
        /// <param name="succnum">充值成功条数(可选)</param>
        /// <param name="succtotal">充值成功总费用(可选)</param>
        /// <returns></returns>
        public static string JsonToFormatter(string rows, string total, string succnum = "", string succtotal = "", string totalinstall = "")
        {
            StringBuilder strb = new StringBuilder();
            strb.Append("{");
            strb.Append("\"total\":" + total);
            strb.Append(",\"rows\":" + rows + "");
            if (succnum != "")
            {
                strb.Append(",\"succnum\":" + succnum);
            }
            if (succtotal != "")
            {
                strb.Append(",\"succtotal\":" + succtotal);
            }
            if (totalinstall != "")
                strb.Append(",\"TotalInstall\":" + totalinstall);
            strb.Append("}");
            return strb.ToString();
        }

        //只能转换单一对象
        /// <summary>
        /// JSON序列化
        /// </summary>
        public static string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            //替换Json的Date字符串
            string p = @"\\/Date\((\d+)\+\d+\)\\/";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            return jsonString;
        }
        //json 为单一对象结构，不能转换数组
        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            //将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式
            string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }
        /// <summary>    
        /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串    
        /// </summary>    
        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }
        /// <summary>    
        /// 将时间字符串转为Json时间    
        /// </summary>    
        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("///Date({0}+0800)///", ts.TotalMilliseconds);
            return result;
        }
        /// <summary>
        /// 将字典类型转json
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string ObjectDicToJson(Dictionary<string, object> dic)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append("{");
            foreach (KeyValuePair<string, object> item in dic)
            {
                strb.Append("\"" + item.Key + "\":\"" + item.Value + "\",");
            }
            if (strb.Length == 1)
                return "";
            strb = strb.Remove(strb.Length - 1, 1);
            strb.Append("}");
            return strb.ToString();
        }
        /// <summary>
        /// 将字典类型转json
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string StringDicToJson(Dictionary<string, string> dic)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append("{");
            foreach (KeyValuePair<string, string> item in dic)
            {
                strb.Append("\"" + item.Key + "\":\"" + item.Value + "\",");
            }
            if (strb.Length == 1)
                return "";
            strb = strb.Remove(strb.Length - 1, 1);
            strb.Append("}");
            return strb.ToString();
        }
        /// <summary>
        /// 将NameValueCollection类型转json
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string NameValueColToJson(NameValueCollection parmas)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append("{");
            foreach (string item in parmas.AllKeys)
            {
                strb.Append("\"" + item + "\":\"" + parmas[item] + "\",");
            }
            if (strb.Length == 1)
                return "";
            strb = strb.Remove(strb.Length - 1, 1);
            strb.Append("}");
            return strb.ToString();
        }
       
    }
}