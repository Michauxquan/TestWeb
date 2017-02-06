using System;
using System.Text;
using System.Drawing;
using System.Web.Mvc;
using System.Collections.Generic;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using System.Web;
using System.IO;
using System.Data;

namespace OWZX.Web.Controllers
{
    /// <summary>
    /// 工具控制器类
    /// </summary>
    public partial class ToolController : Controller
    {

        /// <summary>
        /// 验证图片
        /// </summary>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <returns></returns>
        public ImageResult VerifyImage(int width = 56, int height = 20)
        {
            //获得用户唯一标示符sid
            string sid = ShopUtils.GetSidCookie("web");
            //当sid为空时
            if (sid == null)
            {
                //生成sid
                sid = Sessions.GenerateSid();
                //将sid保存到cookie中
                ShopUtils.SetSidCookie(sid, "web");
            }

            //生成验证值
            string verifyValue = Randoms.CreateRandomValue(4, false).ToLower();
            //生成验证图片
            RandomImage verifyImage = Randoms.CreateRandomImage(verifyValue, width, height, Color.White, Color.Blue, Color.DarkRed);
            //将验证值保存到session中
            Sessions.SetItem(sid, "verifyCode", verifyValue);

            //输出验证图片
            return new ImageResult(verifyImage.Image, verifyImage.ContentType);
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            string operation = WebHelper.GetQueryString("operation");

            string[] myvalue = Request.QueryString.AllKeys;
            if (Array.IndexOf<string>(myvalue, "action") > 0)
            {
                string confg = WebHelper.GetQueryString("action");
                if (confg == "config")
                {
                    string path = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                    FileInfo myFile = new FileInfo(path + @"components\\ueditor\\net\\config.json");
                    // OpenText 创建一个UTF-8 编码的StreamReader对象 
                    StreamReader sr5 = myFile.OpenText();
                    string json = sr5.ReadToEnd();
                    return Content(json);
                }
            }

            string userid = string.Empty;
            if (Array.IndexOf<string>(myvalue, "userid") > 0)
            {
                userid = WebHelper.GetQueryString("action");
            }

            if (operation == "uploadfinanceimage")//上传财务图片
            {
                string fnuserid = WebHelper.GetCookie("fnn_web", "fn_userid");
                HttpPostedFileBase file = Request.Files[0];
                string result = Uploads.SaveUploadImg(file, int.Parse(fnuserid));
                return Content(result);
            }
            else if (operation == "uploadimage")
            {
                HttpPostedFileBase file = Request.Files[0];
                string filepath = "/upload/imgs/" + userid;
                string result = Uploads.SaveUploadImg(file, filepath);
                return Content(result);

            }
            else if (operation == "uploadfile")
            {
                HttpPostedFileBase file = Request.Files[0];
                string filepath = "/upload/files/" + userid;
                string result = Uploads.SaveUploadFile(file, filepath);
                return Content(result);

            }
            else if (operation == "uploadimgnosize")
            {
                HttpPostedFileBase file = Request.Files[0];
                string filepath = "/upload/imgs/";
                string result = Uploads.SaveUploadImg(file, filepath);
                return Content(result);
            }
            return HttpNotFound();
        }
        /// <summary>
        /// 省列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ProvinceList()
        {
            List<RegionInfo> regionList = Regions.GetProvinceList();

            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            foreach (RegionInfo info in regionList)
            {
                sb.AppendFormat("{0}\"id\":\"{1}\",\"name\":\"{2}\"{3},", "{", info.RegionId, info.Name, "}");
            }

            if (regionList.Count > 0)
                sb.Remove(sb.Length - 1, 1);

            sb.Append("]");

            return AjaxResult("success", sb.ToString(), true);
        }

        /// <summary>
        /// 市列表
        /// </summary>
        /// <param name="provinceId">省id</param>
        /// <returns></returns>
        public ActionResult CityList(int provinceId = -1)
        {
            List<RegionInfo> regionList = Regions.GetCityList(provinceId);

            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            foreach (RegionInfo info in regionList)
            {
                sb.AppendFormat("{0}\"id\":\"{1}\",\"name\":\"{2}\"{3},", "{", info.RegionId, info.Name, "}");
            }

            if (regionList.Count > 0)
                sb.Remove(sb.Length - 1, 1);

            sb.Append("]");

            return AjaxResult("success", sb.ToString(), true);
        }

        /// <summary>
        /// 县或区列表
        /// </summary>
        /// <param name="cityId">市id</param>
        /// <returns></returns>
        public ActionResult CountyList(int cityId = -1)
        {
            List<RegionInfo> regionList = Regions.GetCountyList(cityId);

            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            foreach (RegionInfo info in regionList)
            {
                sb.AppendFormat("{0}\"id\":\"{1}\",\"name\":\"{2}\"{3},", "{", info.RegionId, info.Name, "}");
            }

            if (regionList.Count > 0)
                sb.Remove(sb.Length - 1, 1);

            sb.Append("]");

            return AjaxResult("success", sb.ToString(), true);
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
    }
}
