using System;
using System.Text;

namespace OWZX.PayPlugin.Alipay
{
    /// <summary>
    /// 基础配置类,设置帐户有关信息及返回路径
    /// </summary>
    public class AlipayConfig
    {
        private static string _seller = "";//收款支付宝帐户
        private static string _partner = "";//合作身份者ID，以2088开头由16位纯数字组成的字符串
        private static string _key = ""; //交易安全检验码，由数字和字母组成的32位字符串
        private static Encoding _code = null;//字符编码格式 目前支持 gbk 或 utf-8
        private static string _inputcharset = "";//字符编码格式(文本)
        private static string _signtype = "";//签名方式，选择项：RSA、DSA、MD5
        private static string _gateway = "";//支付宝网关地址（新）
        private static string _veryfyurl = "";//支付宝消息验证地址

        private static string _privatekey = "";//商户的私钥
        private static string _publickey = "";//支付宝的公钥，无需修改该值
        private static string _appinputcharset = "";//app字符编码格式 目前支持 gbk 或 utf-8
        private static string _appsigntype = "";//app签名方式，选择项：RSA、DSA、MD5
        private static string _appveryfyurl = "";//app支付宝消息验证地址

        static AlipayConfig()
        {
            _seller = PluginUtils.GetPluginSet().Seller;
            _partner = PluginUtils.GetPluginSet().Partner;
            _key = PluginUtils.GetPluginSet().Key;
            _code = Encoding.GetEncoding("utf-8");
            _inputcharset = "utf-8";
            _signtype = "MD5";
            _gateway = "https://mapi.alipay.com/gateway.do?";
            _veryfyurl = "https://mapi.alipay.com/gateway.do?service=notify_verify&";
            _privatekey = PluginUtils.GetPluginSet().PrivateKey;
            //_privatekey ="MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBALQd0QZ2uURTwqipEQRQDhEPK3Ch6v6dtravaFoX7n4+ib0so2AnDbKboJxT8Uu4/e0RCmreA1d0ok/9kVPtlMwKeGuiP8E2xUsUvoVwg/SMT4U8yX9heb76BIZeNRRMgmkZYq1UsHApv+KOCnnHwXf/ud1fXucf9/irjBXRwznRAgMBAAECgYBwtsf7t4gwzgne6g4QGNj0q/2POoSIWcHhiNtQpfFFZ3ViwPBsV4Qm5WUY7x7tOBMPq75NKioFLKP2UsQDNYYLLQci1q/iCYINdpk6jaxH4yGNTlIO2WjFbANosXcSwaMQzdtE6uMCavA2P2Nm1hXM/0xSWiHiyDd7+de1rny9UQJBAN5U22bvR2AsAcKMRxH/vOUL4YA0K0W6G3B8C9G4HE3B2Lb1CmufstTx5SpkJY72sr4lHC8FJZ70sDv5SIKxRv8CQQDPZGgr0Sh1OMpUBvD/0ajJ1hnWZ43p0HmJIOF81aE50NpigLHgwFvEmKZtWiMGZu2SuEY0ZNxuf0Y695ocMc8vAkAeET3Gsu9lKy5lwBDQd1R1aWDqtKNxf5S8Zpo2l36EaYXEYGkzWtqVf80tKXQG3IgZvO0N0tVepNq8kZ7jxdPDAkEAkbga+a6cnsCoaSH3c8f8sNSekudv7zlsK83Oocf44Ia+6zdBxIlj8V7QkUUkFvt7MfwIWAgGWh1TW1teDTFyQQJAMDmusDcV0jV34NBJ1w2bJLaJ2+9WSqJITO7iWDnrsqCdacNI5MS0cv4gTei43DtYQKpxrSl8taAhKsqwZO5c+g==";// PluginUtils.GetPluginSet().PrivateKey;
            _publickey = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";
            _appinputcharset = "utf-8";
            _appsigntype = "RSA";
            _appveryfyurl = "https://mapi.alipay.com/gateway.do?service=notify_verify&";
        }

        /// <summary>
        /// 重置支付宝配置
        /// </summary>
        public static void ReSet()
        {
            _seller = PluginUtils.GetPluginSet().Seller;
            _partner = PluginUtils.GetPluginSet().Partner;
            _key = PluginUtils.GetPluginSet().Key;
            _privatekey = PluginUtils.GetPluginSet().PrivateKey;
        }


        /// <summary>
        /// 收款支付宝帐户ID
        /// </summary>
        public static string Seller
        {
            get { return _seller; }
        }

        /// <summary>
        /// 合作者身份ID
        /// </summary>
        public static string Partner
        {
            get { return _partner; }
        }

        /// <summary>
        /// 交易安全校验码
        /// </summary>
        public static string Key
        {
            get { return _key; }
        }

        /// <summary>
        /// 字符编码格式
        /// </summary>
        public static Encoding Code
        {
            get { return _code; }
        }

        /// <summary>
        /// 字符编码格式(文本)
        /// </summary>
        public static string InputCharset
        {
            get { return _inputcharset; }
        }

        /// <summary>
        /// 签名方式
        /// </summary>
        public static string SignType
        {
            get { return _signtype; }
        }

        /// <summary>
        /// 支付宝网关地址（新）
        /// </summary>
        public static string Gateway
        {
            get { return _gateway; }
        }

        /// <summary>
        /// 支付宝消息验证地址
        /// </summary>
        public static string VeryfyUrl
        {
            get { return _veryfyurl; }
        }


        /// <summary>
        /// 商户的私钥
        /// </summary>
        public static string PrivateKey
        {
            get { return _privatekey; }
        }
        /// <summary>
        /// 支付宝的公钥，无需修改该值
        /// </summary>
        public static string PublicKey
        {
            get { return _publickey; }
        }
        /// <summary>
        /// app字符编码格式 目前支持 gbk 或 utf-8
        /// </summary>
        public static string AppInputCharset
        {
            get { return _appinputcharset; }
        }
        /// <summary>
        /// app签名方式，选择项：RSA、DSA、MD5
        /// </summary>
        public static string AppSignType
        {
            get { return _appsigntype; }
        }
        /// <summary>
        /// app支付宝消息验证地址
        /// </summary>
        public static string AppVeryfyUrl
        {
            get { return _appveryfyurl; }
        }
    }
}