using System;
using System.Web;

using OWZX.Core;

namespace OWZX.Services
{
    /// <summary>
    /// 上传操作管理类
    /// </summary>
    public partial class Uploads
    {
        private static IUploadStrategy _iuploadstrategy = BSPUpload.Instance;//上传策略

        /// <summary>
        /// 保存上传的文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filepath">格式：/upload/finance/123/</param>
        /// <returns></returns>
        public static string SaveUploadFile(HttpPostedFileBase file, string filepath)
        {
            return _iuploadstrategy.SaveUploadFile(file, filepath);
        }
        /// <summary>
        /// 保存上传的图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="filepath">格式：/upload/finance/123/</param>
        /// <returns></returns>
        public static string SaveUploadImgNoSize(HttpPostedFileBase image, string filepath)
        {
            return _iuploadstrategy.SaveUploadImgNoSize(image, filepath);
        }

        /// <summary>
        /// 处理海报图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="filepath">格式：/upload/finance/123/</param>
        /// <returns></returns>
        public static string SaveUploadImgForPoster(HttpPostedFileBase image, string filepath)
        {
            return _iuploadstrategy.SaveUploadImgForPoster(image, filepath);
        }
        /// <summary>
        /// 保存上传的图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="filepath">格式：/upload/finance/123/</param>
        /// <param name="imgsize">格式：宽*高</param>
        /// <returns></returns>
        public static string SaveUploadImg(HttpPostedFileBase image, string filepath, string imgsize = "")
        {
            return _iuploadstrategy.SaveUploadImg(image, filepath, imgsize);
        }
        /// <summary>
        /// 保存上传的财务票据
        /// </summary>
        /// <param name="img"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static string SaveUploadImg(HttpPostedFileBase img,int userid)
        {
            return _iuploadstrategy.SaveUploadImg(img,userid);
        }
        /// <summary>
        /// 保存上传的用户头像
        /// </summary>
        /// <param name="avatar">用户头像</param>
        /// <returns></returns>
        public static string SaveUploadUserAvatar(HttpPostedFileBase avatar)
        {
            return _iuploadstrategy.SaveUploadUserAvatar(avatar);
        }

        /// <summary>
        /// 保存上传的用户等级头像
        /// </summary>
        /// <param name="avatar">用户等级头像</param>
        /// <returns></returns>
        public static string SaveUploadUserRankAvatar(HttpPostedFileBase avatar)
        {
            return _iuploadstrategy.SaveUploadUserRankAvatar(avatar);
        }

        /// <summary>
        /// 保存上传的品牌logo
        /// </summary>
        /// <param name="logo">品牌logo</param>
        /// <returns></returns>
        public static string SaveUploadBrandLogo(HttpPostedFileBase logo)
        {
            return _iuploadstrategy.SaveUploadBrandLogo(logo);
        }

        /// <summary>
        /// 保存新闻编辑器中的图片
        /// </summary>
        /// <param name="image">新闻图片</param>
        /// <returns></returns>
        public static string SaveNewsEditorImage(HttpPostedFileBase image)
        {
            return _iuploadstrategy.SaveNewsEditorImage(image);
        }

        /// <summary>
        /// 保存帮助编辑器中的图片
        /// </summary>
        /// <param name="image">帮助图片</param>
        /// <returns></returns>
        public static string SaveHelpEditorImage(HttpPostedFileBase image)
        {
            return _iuploadstrategy.SaveHelpEditorImage(image);
        }

        /// <summary>
        /// 保存商品编辑器中的图片
        /// </summary>
        /// <param name="image">商品图片</param>
        /// <param name="cateid">分类id</param>
        /// <returns></returns>
        public static string SaveProductEditorImage(HttpPostedFileBase image,string cateid="")
        {
            return _iuploadstrategy.SaveProductEditorImage(image, cateid);
        }

        /// <summary>
        /// 保存上传的商品图片
        /// </summary>
        /// <param name="image">商品图片</param>
        /// <param name="cateid">分类id</param>
        /// <returns></returns>
        public static string SaveUplaodProductImage(HttpPostedFileBase image, string cateid = "")
        {
            return _iuploadstrategy.SaveUplaodProductImage(image, cateid);
        }

        /// <summary>
        /// 保存上传的广告图片
        /// </summary>
        /// <param name="image">广告图片</param>
        /// <returns></returns>
        public static string SaveUploadAdvertImage(HttpPostedFileBase image)
        {
            return _iuploadstrategy.SaveUploadAdvertImage(image);
        }

        /// <summary>
        /// 保存上传的友情链接Logo
        /// </summary>
        /// <param name="logo">友情链接logo</param>
        /// <returns></returns>
        public static string SaveUploadFriendLinkLogo(HttpPostedFileBase logo)
        {
            return _iuploadstrategy.SaveUploadFriendLinkLogo(logo);
        }

        /// <summary>
        /// 保存上传的基本资料图片
        /// </summary>
        /// <param name="baseinfo">基本资料图片</param>
        /// <returns></returns>
        public static string SaveUploadBaseInfo(HttpPostedFileBase baseinfo)
        {
            return _iuploadstrategy.SaveUploadBaseInfo(baseinfo);
        }
    }
}
