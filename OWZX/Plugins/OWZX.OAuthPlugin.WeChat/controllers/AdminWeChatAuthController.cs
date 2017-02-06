using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.OAuthPlugin.WeChat;

namespace OWZX.Web.Admin.Controllers
{
    /// <summary>
    /// 后台WeChat开放授权控制器类
    /// </summary>
    public class AdminWeChatOAuthController : BaseAdminController
    {
        /// <summary>
        /// 配置
        /// </summary>
        [HttpGet]
        [ChildActionOnly]
        public ActionResult Config()
        {
            PluginSetInfo pluginSetInfo = PluginUtils.GetPluginSet();

            ConfigModel model = new ConfigModel();
            model.AuthUrl = pluginSetInfo.AuthUrl;
            model.AppID = pluginSetInfo.AppID;
            model.AppSecret = pluginSetInfo.AppSecret;
            model.Server = pluginSetInfo.Server;
            model.UNamePrefix = pluginSetInfo.UNamePrefix;

            return View("~/plugins/OWZX.OAuthPlugin.WeChat/views/adminwechatoauth/config.cshtml", model);
        }

        /// <summary>
        /// 配置
        /// </summary>
        [HttpPost]
        public ActionResult Config(ConfigModel model)
        {
            if (ModelState.IsValid)
            {
                PluginSetInfo pluginSetInfo = new PluginSetInfo();

                pluginSetInfo.AuthUrl = model.AuthUrl.Trim();
                pluginSetInfo.AppID = model.AppID.Trim();
                pluginSetInfo.AppSecret = model.AppSecret.Trim();
                pluginSetInfo.Server = model.Server.Trim();
                pluginSetInfo.UNamePrefix = model.UNamePrefix.Trim();

                PluginUtils.SavePluginSet(pluginSetInfo);
                AddAdminOperateLog("修改微信开放授权插件配置信息");
                return PromptView(Url.Action("config", "plugin", new { configController = "AdminWeChatOAuth", configAction = "Config" }), "插件配置修改成功");
            }
            return PromptView(Url.Action("config", "plugin", new { configController = "AdminWeChatOAuth", configAction = "Config" }), "信息有误，请重新填写");
        }
    }
}
