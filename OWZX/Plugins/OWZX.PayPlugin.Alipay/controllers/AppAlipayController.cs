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
    public class AppAlipayController : BaseWebController
    {

        /// <summary>
        /// 支付
        /// </summary>
        public ActionResult Pay()
        {
            string result = WebHelper.GetPostStr();
            NameValueCollection parmas = WebHelper.GetParmList(result);

            //服务器异步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数
            string notifyUrl = string.Format("{0}/appalipay/notify", BSPConfig.ShopConfig.SiteUrl);


            //付款金额
            string totalFee = decimal.Parse(parmas["totalfee"]).ToString();

            
            //记录充值信息
            RechargeModel rech = new RechargeModel
            {
                Out_trade_no = parmas["outtradeno"],
                Account = parmas["account"],
                SuiteId = parmas["vossuiteid"],
                PlatForm = "支付宝",
                Type = int.Parse(parmas["type"]),
                Role = int.Parse(parmas["role"])
            };
            bool addres = Recharge.AddRecharge(rech);
            if (!addres)
                return AjaxResult("error", "记录充值信息失败");

            return AjaxResult("success", "验证成功");
        }

        /// <summary>
        /// 通知调用
        /// </summary>
        public ActionResult Notify()
        {
            //string forms = "discount=0.00&payment_type=1&subject=%u9ed1%u7c73%u58f3&trade_no=2016111521001004630270573220&buyer_email=15670179992&gmt_create=2016-11-15+09%3a29%3a16&notify_type=trade_status_sync&quantity=1&out_trade_no=1115092904-1643&seller_id=2088421544580296&notify_time=2016-11-15+10%3a55%3a09&body=%u8bdd%u8d39%u5957%u9910&trade_status=TRADE_SUCCESS&is_total_fee_adjust=N&total_fee=0.01&gmt_payment=2016-11-15+09%3a29%3a17&seller_email=cbaohai%40126.com&price=0.01&buyer_id=2088012967203633&notify_id=53526949d59c9297c6641bd8e450a03kv2&use_coupon=N&sign_type=RSA&sign=WDJhrGG2kEgGvKtfPG3WoMn8mwleWbyDTR0scZ6HmSK39OZnlHKS8lSTkYM09ZY4y7%2bxTM1IeNmFsVcbK%2bVJK2jHYX43RyEIyBkCgJVuo0IpUDPYZidyrgEeGKoeV0FPutykbLIueQNSoAENkPsAmFWd1J1FQMYvF8akJL1R%2b5k%3d";
            //forms = HttpUtility.UrlDecode(forms, Encoding.UTF8);
            SortedDictionary<string, string> sPara = AlipayCore.GetRequestPost();
            //Dictionary<string, string> dic = CommonHelper.ParmsToDic(forms);
            //foreach (KeyValuePair<string, string> kv in dic)
            //{
            //    sPara[kv.Key] = kv.Value;
            //}
            //BSPLog.Instance.Write(Request.Form.ToString());
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                bool verifyResult = AlipayNotify.AppVerify(sPara, sPara["notify_id"], sPara["sign"], AlipayConfig.AppSignType, AlipayConfig.PublicKey, AlipayConfig.AppInputCharset, AlipayConfig.AppVeryfyUrl, AlipayConfig.Partner);
                if (verifyResult && (sPara["trade_status"] == "TRADE_FINISHED" || sPara["trade_status"] == "TRADE_SUCCESS"))//验证成功
                {
                    string out_trade_no = sPara["out_trade_no"];//商户订单号
                    string tradeSN = sPara["trade_no"];//支付宝交易号
                    decimal tradeMoney = TypeHelper.StringToDecimal(sPara["total_fee"]);//交易金额
                    DateTime tradeTime = TypeHelper.StringToDateTime(sPara["gmt_payment"]);//交易时间

                    return Content(Recharge.UpdateRecharge(tradeMoney.ToString(), out_trade_no, sPara["gmt_payment"], tradeSN));

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
