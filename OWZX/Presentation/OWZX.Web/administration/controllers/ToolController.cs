using System;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Collections.Generic;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using System.Collections.Specialized;
using System.IO;

namespace OWZX.Web.Admin.Controllers
{
    /// <summary>
    /// 后台工具控制器类
    /// </summary>
    public partial class ToolController : Controller
    {
        private string ip = "";//ip地址
        private ShopConfigInfo shopConfigInfo = BSPConfig.ShopConfig;//商城配置信息
        private PartUserInfo partUserInfo = null;//用户信息

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            ip = WebHelper.GetIP();
            //当用户ip不在允许的后台访问ip列表时
            if (!string.IsNullOrEmpty(shopConfigInfo.AdminAllowAccessIP) && !ValidateHelper.InIPList(ip, shopConfigInfo.AdminAllowAccessIP))
            {
                filterContext.Result = HttpNotFound();
                return;
            }
            //当用户IP被禁止时
            if (BannedIPs.CheckIP(ip))
            {
                filterContext.Result = HttpNotFound();
                return;
            }

            //获得用户id
            int uid = ShopUtils.GetUidCookie("admin");
            if (uid < 1)
                uid = WebHelper.GetRequestInt("uid");
            if (uid < 1)//当用户为游客时
            {
                //创建游客
                partUserInfo = Users.CreatePartGuest();
            }
            else//当用户为会员时
            {
                //获得保存在cookie中的密码
                string encryptPwd = ShopUtils.GetCookiePassword("admin");
                if (string.IsNullOrWhiteSpace(encryptPwd))
                    encryptPwd = WebHelper.GetRequestString("password");
                //防止用户密码被篡改为危险字符
                if (encryptPwd.Length == 0 || !SecureHelper.IsBase64String(encryptPwd))
                {
                    //创建游客
                    partUserInfo = Users.CreatePartGuest();
                    ShopUtils.SetUidCookie(-1, "admin");
                    ShopUtils.SetCookiePassword("", "admin");
                }
                else
                {
                    partUserInfo = Users.GetPartUserByUidAndPwd(uid, ShopUtils.DecryptCookiePassword(encryptPwd));
                    if (partUserInfo == null)
                    {
                        partUserInfo = Users.CreatePartGuest();
                        ShopUtils.SetUidCookie(-1, "admin");
                        ShopUtils.SetCookiePassword("", "admin");
                    }
                }
            }

            //当用户等级是禁止访问等级时
            if (partUserInfo.UserRid == 1)
            {
                filterContext.Result = HttpNotFound();
                return;
            }

            //如果当前用户没有登录
            if (partUserInfo.Uid < 1)
            {
                filterContext.Result = HttpNotFound();
                return;
            }

            //如果当前用户不是管理员
            if (partUserInfo.AdminGid == 1)
            {
                filterContext.Result = HttpNotFound();
                return;
            }
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
            string cateid = "";
            if (Array.IndexOf<string>(myvalue, "cateid") > 0)
            {
                cateid = WebHelper.GetQueryString("cateid");
            }

            string imgsize = string.Empty;
            if (Array.IndexOf<string>(myvalue, "imgsize") > 0)
            {
                imgsize = WebHelper.GetQueryString("imgsize");
            }

            if (operation == "uploadimage")
            {
                HttpPostedFileBase file = Request.Files[0];
                string filepath = "/upload/imgs/";
                string result = Uploads.SaveUploadImg(file, filepath, imgsize);
                return Content(result);

            }
            else if (operation == "uploadimagenosize")
            {
                HttpPostedFileBase file = Request.Files[0];
                string filepath = "/upload/imgs/";
                string result = Uploads.SaveUploadImgNoSize(file, filepath);
                return Content(result);

            }
            else if (operation == "uploadhbimage")
            {
                //海报图片
                HttpPostedFileBase file = Request.Files[0];
                string result = Uploads.SaveUploadImgForPoster(file, "/upload/imgs/");
                //ueditor   
                string jsres = "{\"url\":\"/upload/imgs/"+ result + "\",\"title\":\"" + file.FileName + "\",\"original\":\"" + file.FileName + "\",\"state\":\"" + GetUEState(result) + "\"}";
                return Content(jsres);
            }
            else if (operation == "uploadfile")
            {
                //生成二维码
                string url = "http://2d-code.cn/2dcode/api.php?key=c_e541Q4CL7uA9kSMaYQTBOWIOvhohLggo0nOtyr8QmU&url="
                    + HttpUtility.UrlEncode(BSPConfig.ShopConfig.SiteUrl, Encoding.UTF8) + "&logo=http://app.himicall.com/images/hmk.jpg&border=3&cl=H&size=200";

                HttpPostedFileBase file = Request.Files[0];
                string filepath = "/upload/files/" ;
                string result = Uploads.SaveUploadFile(file, filepath);
                return Content(result);

            }
            else if (operation == "uploadproductimage")//上传商品图片
            {
                HttpPostedFileBase file = Request.Files[0];
                string result = Uploads.SaveUplaodProductImage(file, cateid);
                return Content(result);
            }
            else if (operation == "uploadprjimg")
            {
                //int uid = -1;
                //if (Array.IndexOf<string>(myvalue, "uid") > 0)
                //{
                //    uid = WebHelper.GetQueryInt("uid");
                //}
                HttpPostedFileBase file = Request.Files[0];
                string result = Uploads.SaveUploadImg(file, "/upload/project/");
                return Content(result);
            }
            else if (operation == "uploadproducteditorimage")//上传商品编辑器中图片
            {
                HttpPostedFileBase file = Request.Files[0 ];
                string result = Uploads.SaveProductEditorImage(file,cateid);
                //ueditor   
                string jsres = "{\"url\":\"/upload/product/editor/" + cateid + "/" + result + "\",\"title\":\"" + file.FileName + "\",\"original\":\"" + file.FileName + "\",\"state\":\"" + GetUEState(result) + "\"}";
                return Content(jsres);
                //umeditor 
                //return Content(string.Format("{2}'url':'upload/product/editor/{0}','state':'{1}','originalName':'','name':'','size':'','type':''{3}", result, GetUEState(result), "{", "}"));
            }
            else if (operation == "uploadadvertimage")//上传广告图片
            {
                HttpPostedFileBase file = Request.Files[0];
                string result = Uploads.SaveUploadAdvertImage(file);
                return Content(result);
            }
            else if (operation == "uploadnewseditorimage")//上传公告编辑器中的图片
            {
                HttpPostedFileBase file = Request.Files[0];
                string result = Uploads.SaveNewsEditorImage(file);
                //ueditor   
                string jsres = "{\"url\":\"/upload/news/"+ result + "\",\"title\":\"" + file.FileName + "\",\"original\":\"" + file.FileName + "\",\"state\":\"" + GetUEState(result) + "\"}";
                return Content(jsres);
                //umeditor 
                //return Content(string.Format("{2}'url':'upload/news/{0}','state':'{1}','originalName':'','name':'','size':'','type':''{3}", result, GetUEState(result), "{", "}"));
            }
            else if (operation == "uploadbrandlogo")//上传品牌logo
            {
                HttpPostedFileBase file = Request.Files[0];
                string result = Uploads.SaveUploadBrandLogo(file);
                return Content(result);
            }
            else if (operation == "uploadhelpeditorimage")//上传帮助编辑器中的图片
            {
                HttpPostedFileBase file = Request.Files[0];
                string result = Uploads.SaveHelpEditorImage(file);
                return Content(string.Format("{2}'url':'upload/help/{0}','state':'{1}','originalName':'','name':'','size':'','type':''{3}", result, GetUEState(result), "{", "}"));
            }
            else if (operation == "uploadfriendlinklogo")//上传友情链接logo
            {
                HttpPostedFileBase file = Request.Files[0];
                string result = Uploads.SaveUploadFriendLinkLogo(file);
                return Content(result);
            }
            else if (operation == "uploaduserrankavatar")//上传用户等级头像
            {
                HttpPostedFileBase file = Request.Files[0];
                string result = Uploads.SaveUploadUserRankAvatar(file);
                return Content(result);
            }
            else if (operation == "uploadbaseinfo")//上传基本资料图片
            {
                HttpPostedFileBase file = Request.Files[0];
                string result = Uploads.SaveUploadBaseInfo(file);
                string jsres = "{\"url\":\"/upload/baseinfo/" + result + "\",\"title\":\"" + file.FileName + "\",\"original\":\"" + file.FileName + "\",\"state\":\"" + GetUEState(result) + "\"}";
                return Content(jsres);
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

            return Content(sb.ToString());
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

            return Content(sb.ToString());
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

            return Content(sb.ToString());
        }

        /// <summary>
        /// 获得ueditor状态
        /// </summary>
        /// <param name="result">上传结果</param>
        /// <returns></returns>
        private string GetUEState(string result)
        {
            if (result == "-1")
            {
                return "上传图片不能为空";
            }
            else if (result == "-2")
            {
                return "不允许的图片类型";
            }
            else if (result == "-3")
            {
                return "图片大小超出网站限制";
            }
            else
            {
                return "SUCCESS";
            }
        }

    }
}
