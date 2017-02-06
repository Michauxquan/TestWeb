using OWZX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Web.Admin.Models
{
    /// <summary>
    /// 登陆模型类
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 返回地址
        /// </summary>
        public string ReturnUrl { get; set; }
        /// <summary>
        /// 影子账号名
        /// </summary>
        public string ShadowName { get; set; }
        /// <summary>
        /// 是否允许记住用户
        /// </summary>
        public bool IsRemember { get; set; }
        /// <summary>
        /// 是否启用验证码
        /// </summary>
        public bool IsVerifyCode { get; set; }
        /// <summary>
        /// 开放授权插件
        /// </summary>
        public List<PluginInfo> OAuthPluginList { get; set; }
    }
    
}
