using System;
using System.Web;
using System.Web.Mvc;

using OWZX.Core;
using OWZX.Services;
using System.Text;

namespace OWZX.Web.Framework
{
    /// <summary>
    /// 基础控制器类
    /// </summary>
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
          
            Logs.Write("控制器：" + filterContext.Controller.ToString() + ";异常信息："+ filterContext.Exception);
            string redirect = string.Empty;
            if (filterContext.Controller.ToString().Contains("OWZX.Web.Admin"))
            {
                // 跳转地址
                 redirect = "/admin/error/Info?m=" + HttpUtility.UrlEncode(filterContext.Exception.Message);
            }else
                redirect = "/error/index?m=" + HttpUtility.UrlEncode(filterContext.Exception.Message);//返回调用接口异常
            // 跳转至错误提示页面
            filterContext.ExceptionHandled = true;
            filterContext.Result =new RedirectResult(redirect);
        }

        /// <summary>
        /// 获得路由中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        protected int GetRouteInt(string key, int defaultValue)
        {
            return TypeHelper.ObjectToInt(RouteData.Values[key], defaultValue);
        }

        /// <summary>
        /// 获得路由中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        protected int GetRouteInt(string key)
        {
            return GetRouteInt(key, 0);
        }

        /// <summary>
        /// 获得路由中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        protected string GetRouteString(string key, string defaultValue)
        {
            object value = RouteData.Values[key];
            if (value != null)
                return value.ToString();
            else
                return defaultValue;
        }

        /// <summary>
        /// 获得路由中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        protected string GetRouteString(string key)
        {
            return GetRouteString(key, "");
        }
       
        /// <summary>
        /// 提示信息视图
        /// </summary>
        /// <param name="backUrl">返回地址</param>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        protected ViewResult PromptView(string backUrl, string message)
        {
            return View("prompt", new PromptModel(backUrl, message));
        }

        /// <summary>
        /// 提示信息视图
        /// </summary>
        /// <param name="backUrl">返回地址</param>
        /// <param name="message">提示信息</param>
        /// <param name="isAutoBack">是否自动返回</param>
        /// <returns></returns>
        protected ViewResult PromptView(string backUrl, string message, bool isAutoBack)
        {
            return View("prompt", new PromptModel(backUrl, message, isAutoBack));
        }

        /// <summary>
        /// ajax请求结果
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        protected ActionResult AjaxResult(string state, string content)
        {
            return AjaxResult(state, content, false);
        }

        /// <summary>
        /// ajax请求结果
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="content">内容</param>
        /// <param name="isObject">是否为对象</param>
        /// <returns></returns>
        protected ActionResult AjaxResult(string state, string content, bool isObject)
        {
            return Content(string.Format("{0}\"state\":\"{1}\",\"content\":{2}{3}{4}{5}", "{", state, isObject ? "" : "\"", content, isObject ? "" : "\"", "}"));
        }

        /// <summary>
        /// 接口请求结果
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="biz_content">内容</param>
        /// <returns></returns>
        protected ActionResult APIResult(string state, string biz_content)
        {
            return APIResult(state, biz_content, false);
        }

        /// <summary>
        /// 接口请求结果
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="biz_content">内容</param>
        /// <param name="isObject">是否为对象</param>
        /// <returns></returns>
        protected ActionResult APIResult(string state, string biz_content, bool isObject)
        {
            return Content(string.Format("{0}\"state\":\"{1}\",\"biz_content\":{2}{3}{4}{5}", "{", state, isObject ? "" : "\"", biz_content, isObject ? "" : "\"", "}"));
        }
    }

   
}
