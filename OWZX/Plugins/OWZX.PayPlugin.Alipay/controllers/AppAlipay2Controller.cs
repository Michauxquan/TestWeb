using System;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.PayPlugin.Alipay;
using System.Collections.Specialized;
using OWZX.Model;
using Newtonsoft.Json;

namespace OWZX.Web.Controllers
{
    /// <summary>
    /// App支付宝控制器类
    /// </summary>
    public class AppAlipay2Controller : BaseWebController
    {

        /// <summary>
        /// 支付
        /// </summary>
        public ActionResult Pay()
        {
            string result = WebHelper.GetPostStr();
            NameValueCollection parmas = WebHelper.GetParmList(result);
           
            //支付类型，必填，不能修改
            string paymentType = "1";

            //服务器异步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数
            string notifyUrl = string.Format("{0}/appalipay/notify", BSPConfig.ShopConfig.SiteUrl);

            //收款支付宝帐户
            string sellerEmail = AlipayConfig.Seller;
            //合作者身份ID
            string partner = AlipayConfig.Partner;
            //交易安全检验码
            string key = AlipayConfig.Key;

            //商户订单号
            string outTradeNo = "hmk" + DateTime.Now.ToString("yyMMdd") + Randoms.CreateRandomValue(6, true);
            //订单名称
            string subject = BSPConfig.ShopConfig.SiteTitle;
            //付款金额
            string totalFee = double.Parse(parmas["totalFee"]).ToString();
            //订单描述
            string body = "话费套餐";



            Encoding e = Encoding.GetEncoding(AlipayConfig.AppInputCharset);

            //把请求参数打包成数组
            SortedDictionary<string, string> parms = new SortedDictionary<string, string>();

            parms.Add("app_id", "2016092001932731");
            parms.Add("method", "alipay.trade.app.pay");
            parms.Add("charset", AlipayConfig.AppInputCharset);
            parms.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            parms.Add("version", "1.0");
            parms.Add("notify_url", notifyUrl);


            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("partner", partner);
            dic.Add("seller_id", sellerEmail);
            dic.Add("out_trade_no", outTradeNo);
            dic.Add("subject", subject);
            dic.Add("body", body);
            dic["total_amount"] = totalFee;
            dic["product_code"] = "QUICK_MSECURITY_PAY";
            dic["timeout_express"] = "30m";
            parms.Add("biz_content", JsonHelper.StringDicToJson(dic));
            parms.Add("sign_type", AlipayConfig.AppSignType);

            string sign = AlipayRSAFromPkcs8.sign(AlipayCore.CreateLinkString(AlipayCore.FilterPara(parms)), AlipayConfig.PrivateKey, AlipayConfig.AppInputCharset);
            parms.Add("sign", sign);
            
            //parms.Add("sign", HttpUtility.UrlEncode(sign, e));
            
            //parms.Remove("biz_content");
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in parms)
            {
                dicArray.Add(temp.Key, temp.Value);
            }
            
            //记录充值信息
            RechargeModel rech = new RechargeModel
            {
                Out_trade_no = outTradeNo,
                Account = parmas["account"],
                SuiteId = parmas["vossuiteid"],
                PlatForm = "支付宝",
                Type = int.Parse(parmas["type"]),
                Role = int.Parse(parmas["role"])
            };
            bool addres = Recharge.AddRecharge(rech);
            if (!addres)
                return AjaxResult("error", "记录充值信息失败");

            //string content = JsonHelper.StringDicToJson(dicArray);
            //content = content.Substring(0, content.Length - 1) + ",\"biz_content\":" + JsonHelper.StringDicToJson(dic) + "}";
            string content = AlipayCore.CreateLinkStringUrlencode(AlipayCore.FilterAllPara(parms), Encoding.UTF8);
            return AjaxResult("success", content);
        }

        /// <summary>
        /// 通知调用
        /// </summary>
        public ActionResult Notify()
        {
            SortedDictionary<string, string> sPara = AlipayCore.GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                bool verifyResult = AlipayNotify.AppVerify(sPara, Request.Form["notify_id"], Request.Form["sign"], AlipayConfig.AppSignType, AlipayConfig.PublicKey, AlipayConfig.AppInputCharset, AlipayConfig.AppVeryfyUrl, AlipayConfig.Partner);
                if (verifyResult && (Request.Form["trade_status"] == "TRADE_FINISHED" || Request.Form["trade_status"] == "TRADE_SUCCESS"))//验证成功
                {
                    string out_trade_no = Request.Form["out_trade_no"];//商户订单号
                    string tradeSN = Request.Form["trade_no"];//支付宝交易号
                    decimal tradeMoney = TypeHelper.StringToDecimal(Request.Form["total_amount"]);//交易金额
                    DateTime tradeTime = TypeHelper.StringToDateTime(Request.Form["gmt_payment"]);//交易时间

                    return Content(Recharge.UpdateRecharge(tradeMoney.ToString(), out_trade_no, Request.Form["gmt_payment"], tradeSN));

                }
                else//验证失败
                {
                    return Content("fail");
                }
            }
            else
            {
                return Content("无通知参数");
            }
        }


    }
}
