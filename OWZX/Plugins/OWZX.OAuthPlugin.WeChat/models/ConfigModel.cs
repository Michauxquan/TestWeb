using System;
using System.ComponentModel.DataAnnotations;

namespace OWZX.OAuthPlugin.WeChat
{
    /// <summary>
    /// 配置模型类
    /// </summary>
    public class ConfigModel
    {
        /// <summary>
        /// 验证Url
        /// </summary>
        [Required(ErrorMessage = "验证Url不能为空！")]
        public string AuthUrl { get; set; }
        /// <summary>
        /// 应用程序ID
        /// </summary>
        [Required(ErrorMessage = "应用程序ID不能为空！")]
        public string AppID { get; set; }
        /// <summary>
        /// 应用程序密钥
        /// </summary>
        [Required(ErrorMessage = "应用程序密钥不能为空！")]
        public string AppSecret { get; set; }
        /// <summary>
        /// 服务商
        /// </summary>
        [Required(ErrorMessage = "服务商不能为空！")]
        public string Server { get; set; }
        /// <summary>
        /// 用户名前缀
        /// </summary>
        [Required(ErrorMessage = "用户名前缀不能为空！")]
        public string UNamePrefix { get; set; }
    }
}
