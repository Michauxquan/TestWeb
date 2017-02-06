using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OWZX.PayPlugin.WeChat
{
    /*
     *  商户接入微信支付，调用API必须遵循以下规则：
        接口规则

        传输方式	为保证交易安全性，采用HTTPS传输
        提交方式	采用POST方法提交
        数据格式	提交和返回数据都为XML格式，根节点名为xml
        字符编码	统一采用UTF-8字符编码
        签名算法	MD5，后续会兼容SHA1、SHA256、HMAC等。
        签名要求	请求和接收数据均需要校验签名，详细方法请参考安全规范-签名算法
        证书要求	调用申请退款、撤销订单接口需要商户证书
        判断逻辑	先判断协议字段返回，再判断业务返回，最后判断交易状态
     */
    public class PayConfig
    {
        /// <summary>
        /// 人民币
        /// </summary>
        public static string Tenpay = "1";

        /// <summary>
        /// mchid ， 微信支付商户号
        /// </summary>
        public static string MchId = "XXXXXXXXXXXXXXXXX"; //

        /// <summary>
        /// appid，应用ID， 在微信公众平台中 “开发者中心”栏目可以查看到
        /// </summary>
        public static string AppId = "XXXXXXXXXXXXXXXXX";

        /// <summary>
        /// appsecret ，应用密钥， 在微信公众平台中 “开发者中心”栏目可以查看到
        /// </summary>
        public static string AppSecret = "XXXXXXXXXXXXXXXXX";

        /// <summary>
        /// paysignkey，API密钥，在微信商户平台中“账户设置”--“账户安全”--“设置API密钥”，只能修改不能查看
        /// </summary>
        public static string AppKey = "XXXXXXXXXXXXXXXXX";

        /// <summary>
        /// 支付起点页面地址，也就是send.aspx页面完整地址
        /// 用于获取用户OpenId，支付的时候必须有用户OpenId，如果已知可以不用设置
        /// </summary>
        public static string SendUrl = "";

        /// <summary>
        /// 支付页面，请注意测试阶段设置授权目录，在微信公众平台中“微信支付”---“开发配置”--修改测试目录   
        /// 注意目录的层次，比如我的：http://zousky.com/WXPay/
        /// </summary>
        public static string PayUrl = "";

        /// <summary>
        ///  支付通知页面，请注意测试阶段设置授权目录，在微信公众平台中“微信支付”---“开发配置”--修改测试目录   
        /// 支付完成后的回调处理页面,替换成notify_url.asp所在路径
        /// </summary>
        public static string NotifyUrl = "";

        static PayConfig()
        {
            MchId = PluginUtils.GetPluginSet().WPMchId;
            AppId = PluginUtils.GetPluginSet().WPAppId;
            AppSecret = PluginUtils.GetPluginSet().WPAppSecret;
            AppKey = PluginUtils.GetPluginSet().WPAppKey;
        }

        public static void ReSet()
        {
            MchId = PluginUtils.GetPluginSet().WPMchId;
            AppId = PluginUtils.GetPluginSet().WPAppId;
            AppSecret = PluginUtils.GetPluginSet().WPAppSecret;
            AppKey = PluginUtils.GetPluginSet().WPAppKey;
        }
    }
}
