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
    public class AppAlipay1Controller : BaseWebController
    {

        /// <summary>
        /// 支付
        /// </summary>
        public ActionResult Pay()
        {
            string result = WebHelper.GetPostStr();
            NameValueCollection parmas = WebHelper.GetParmList(result);
            //if (parmas.Keys.Count < 5)
            //{
            //    return Content("缺少请求参数");
            //}
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

            parms.Add("service", "mobile.securitypay.pay");
            parms["partner"] = partner;
            parms.Add("_input_charset", AlipayConfig.AppInputCharset);
            parms["seller_id"] = sellerEmail;
            parms["out_trade_no"] = parmas["outtradeno"];
            parms["subject"] = subject;
            parms["body"] = body;
            parms["total_fee"] = totalFee;
            parms.Add("notify_url", notifyUrl);
            parms.Add("payment_type", paymentType);
            parms.Add("it_b_pay", "30m");

            
            string sign = AlipayRSAFromPkcs8.sign(AlipayCore.CreateLinkString(AlipayCore.FilterPara(parms)), AlipayConfig.PrivateKey, AlipayConfig.AppInputCharset);
            parms.Add("sign", HttpUtility.UrlEncode(sign, e));
            parms.Add("sign_type", AlipayConfig.AppSignType);

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

            string content = JsonHelper.StringDicToJson(dicArray);
            return AjaxResult("success", content, true);
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
                    decimal tradeMoney = TypeHelper.StringToDecimal(Request.Form["total_fee"]);//交易金额
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
