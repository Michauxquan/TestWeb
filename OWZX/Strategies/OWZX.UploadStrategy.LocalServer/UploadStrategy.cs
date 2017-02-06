using System;
using System.IO;
using System.Web;

using OWZX.Core;
using System.Drawing;
using System.Text;

namespace OWZX.UploadStrategy.LocalServer
{
    /// <summary>
    /// 本地服务器上传策略
    /// </summary>
    public partial class UploadStrategy : IUploadStrategy
    {
        /// <summary>
        /// 保存上传的文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filepath">格式：/upload/finance/123/</param>
        /// <returns></returns>
        public string SaveUploadFile(HttpPostedFileBase file, string filepath)
        {
            if (file == null)
                return "-1";

            ShopConfigInfo shopConfig = BSPConfig.ShopConfig;

            string fileName = file.FileName;
            string extension = Path.GetExtension(fileName);
            //if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, shopConfig.UploadImgType))
            //    return "-2";

            int fileSize = file.ContentLength;
            if (fileSize > shopConfig.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath(filepath);
            string newFileName = string.Format("file_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名


            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            string path = dirPath + newFileName;


            file.SaveAs(path);

            return newFileName;
        }
        /// <summary>
        /// 保存上传的图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="filepath">格式：/upload/finance/123/</param>
        /// <returns></returns>
        public string SaveUploadImgNoSize(HttpPostedFileBase image, string filepath)
        {
            if (image == null)
                return "-1";

            ShopConfigInfo shopConfig = BSPConfig.ShopConfig;

            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, shopConfig.UploadImgType))
                return "-2";

            int fileSize = image.ContentLength;
            if (fileSize > shopConfig.UploadImgSize)
                return "-3";



            string dirPath = IOHelper.GetMapPath(filepath);
            
            string newFileName = string.Format("img_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名
            
            string[] sizeList = StringHelper.SplitString(shopConfig.ProductShowThumbSize);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            string path = dirPath + newFileName;

            image.SaveAs(path);

            return newFileName;
        }
        /// <summary>
        /// 处理海报图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="filepath">格式：/upload/finance/123/</param>
        /// <returns></returns>
        public string SaveUploadImgForPoster(HttpPostedFileBase image, string filepath)
        {
            if (image == null)
                return "-1";

            ShopConfigInfo shopConfig = BSPConfig.ShopConfig;

            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, shopConfig.UploadImgType))
                return "-2";

            int fileSize = image.ContentLength;
            if (fileSize > shopConfig.UploadImgSize)
                return "-3";



            string dirPath = IOHelper.GetMapPath(filepath);
            string newFileName = string.Format("img_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名

            string[] sizeList = StringHelper.SplitString(shopConfig.ProductShowThumbSize);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            string path = dirPath + newFileName;

            image.SaveAs(path);
            string hbfilename = "hb" + newFileName;

            string url = "http://2d-code.cn/2dcode/api.php?key=c_e541Q4CL7uA9kSMaYQTBOWIOvhohLggo0nOtyr8QmU&url="
                   + System.Web.HttpUtility.UrlEncode(BSPConfig.ShopConfig.SiteUrl, Encoding.UTF8) + "&logo=http://app.himicall.com/images/hmk.jpg&border=2&cl=H&size=100";

            string watermarkPath = IOHelper.GetMapPath("/watermarks/" + shopConfig.WatermarkImg);
            //动态生成二维码
            //IOHelper.HttpDownloadFile(url, watermarkPath);

            IOHelper.GenerateImageWatermark(dirPath + newFileName, watermarkPath, dirPath + hbfilename, shopConfig.WatermarkPosition, shopConfig.WatermarkImgOpacity, shopConfig.WatermarkQuality);
            File.Delete(path);
            return hbfilename;
        }
        /// <summary>
        /// 保存上传的图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="filepath">格式：/upload/finance/123/</param>
        /// <param name="imgsize">格式：宽*高</param>
        /// <returns></returns>
        public string SaveUploadImg(HttpPostedFileBase image, string filepath,string imgsize="")
        {
            if (image == null)
                return "-1";

            ShopConfigInfo shopConfig = BSPConfig.ShopConfig;

            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, shopConfig.UploadImgType))
                return "-2";

            int fileSize = image.ContentLength;
            if (fileSize > shopConfig.UploadImgSize)
                return "-3";



            string dirPath = IOHelper.GetMapPath(filepath);
            string newFileName = string.Format("img_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名

            string[] sizeList = StringHelper.SplitString(shopConfig.ProductShowThumbSize);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            string path = dirPath + newFileName;
            
            image.SaveAs(path);

            string state = "";
            if (imgsize == "")
            {
                using (Image imgsv = Image.FromFile(path))
                {
                    if (imgsv.Width != 350 || imgsv.Height != 350)
                    {
                        state = "-4";
                    }
                }
               
            }
            else
            {
                //指定图片宽高
                using (Image imgsv = Image.FromFile(path))
                {
                    if (imgsv.Width != int.Parse(imgsize.Split('*')[0]) || imgsv.Height != int.Parse(imgsize.Split('*')[1]))
                    {
                        state = "-4";
                    }
                }
            }
            if (state != "")
            {
                File.Delete(path);
                return state;
            }

            foreach (string size in sizeList)
            {
                if (size == "100_100" || size == "350_350" || size == "800_800")
                {
                    string thumbDirPath = string.Format("{0}thumb{1}/", dirPath, size);

                    if (!Directory.Exists(thumbDirPath))
                        Directory.CreateDirectory(thumbDirPath);
                    string[] widthAndHeight = StringHelper.SplitString(size, "_");
                    IOHelper.GenerateThumb(path,
                                           thumbDirPath + newFileName,
                                           TypeHelper.StringToInt(widthAndHeight[0]),
                                           TypeHelper.StringToInt(widthAndHeight[1]),
                                           "H");
                }
            }

            return newFileName;
        }
        /// <summary>
        /// 保存上传的图片
        /// </summary>
        /// <param name="avatar"></param>
        /// <returns></returns>
        public string SaveUploadImg(HttpPostedFileBase image,int userid) {
            if (image == null)
                return "-1";

            ShopConfigInfo shopConfig = BSPConfig.ShopConfig;

            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, shopConfig.UploadImgType))
                return "-2";

            int fileSize = image.ContentLength;
            if (fileSize > shopConfig.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/finance/");
            string newFileName = string.Format("fn_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名

            string[] sizeList = StringHelper.SplitString(shopConfig.ProductShowThumbSize);

            string sourceDirPath = dirPath + "source\\";
            if (userid != 0)
                sourceDirPath += userid + "\\";
            if (!Directory.Exists(sourceDirPath))
                Directory.CreateDirectory(sourceDirPath);
            string sourcePath = sourceDirPath + newFileName;
            image.SaveAs(sourcePath);

            

            string state = "";
            using (Image imgsv = Image.FromFile(sourcePath))
            {
                if (imgsv.Width != 350 || imgsv.Height != 350)
                {
                    state = "-4";
                }
            }
            if (state != "")
            {
                File.Delete(sourcePath);
                return state;
            }

            if (userid != 0)
                dirPath += userid + "\\";

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            string path = dirPath + newFileName;

            if (shopConfig.WatermarkType == 1)//文字水印
            {
                IOHelper.GenerateTextWatermark(sourcePath, path, shopConfig.WatermarkText, shopConfig.WatermarkTextSize, shopConfig.WatermarkTextFont, shopConfig.WatermarkPosition, shopConfig.WatermarkQuality);
            }
            else if (shopConfig.WatermarkType == 2)//图片水印
            {
                string watermarkPath = IOHelper.GetMapPath("/watermarks/" + shopConfig.WatermarkImg);
                IOHelper.GenerateImageWatermark(sourcePath, watermarkPath, path, shopConfig.WatermarkPosition, shopConfig.WatermarkImgOpacity, shopConfig.WatermarkQuality);
            }
            else
            {
                image.SaveAs(path);
            }

            foreach (string size in sizeList)
            {
                if (size == "100_100" || size == "350_350" || size == "800_800")
                {
                    string thumbDirPath = string.Format("{0}thumb{1}/", dirPath, size);

                    if (!Directory.Exists(thumbDirPath))
                        Directory.CreateDirectory(thumbDirPath);
                    string[] widthAndHeight = StringHelper.SplitString(size, "_");
                    IOHelper.GenerateThumb(path,
                                           thumbDirPath + newFileName,
                                           TypeHelper.StringToInt(widthAndHeight[0]),
                                           TypeHelper.StringToInt(widthAndHeight[1]),
                                           "H");
                }
            }
            return newFileName;
        }
        /// <summary>
        /// 保存上传的用户头像
        /// </summary>
        /// <param name="avatar">用户头像</param>
        /// <returns></returns>
        public string SaveUploadUserAvatar(HttpPostedFileBase avatar)
        {
            if (avatar == null)
                return "-1";

            ShopConfigInfo shopConfig = BSPConfig.ShopConfig;

            string fileName = avatar.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, shopConfig.UploadImgType))
                return "-2";

            int fileSize = avatar.ContentLength;
            if (fileSize > shopConfig.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/user/");
            string newFileName = string.Format("ua_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);
            string[] sizeList = StringHelper.SplitString(shopConfig.UserAvatarThumbSize);

            string sourceDirPath = dirPath + "source/";
            if (!Directory.Exists(sourceDirPath))
                Directory.CreateDirectory(sourceDirPath);

            string sourcePath = sourceDirPath + newFileName;
            avatar.SaveAs(sourcePath);

            foreach (string size in sizeList)
            {
                string thumbDirPath = string.Format("{0}thumb{1}/", dirPath, size);
                if (!Directory.Exists(thumbDirPath))
                    Directory.CreateDirectory(thumbDirPath);
                string[] widthAndHeight = StringHelper.SplitString(size, "_");
                IOHelper.GenerateThumb(sourcePath,
                                       thumbDirPath + newFileName,
                                       TypeHelper.StringToInt(widthAndHeight[0]),
                                       TypeHelper.StringToInt(widthAndHeight[1]),
                                       "H");
            }
            return newFileName;
        }

        /// <summary>
        /// 保存上传的用户等级头像
        /// </summary>
        /// <param name="avatar">用户等级头像</param>
        /// <returns></returns>
        public string SaveUploadUserRankAvatar(HttpPostedFileBase avatar)
        {
            if (avatar == null)
                return "-1";

            ShopConfigInfo shopConfig = BSPConfig.ShopConfig;

            string fileName = avatar.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, shopConfig.UploadImgType))
                return "-2";

            int fileSize = avatar.ContentLength;
            if (fileSize > shopConfig.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/userrank/");
            string newFileName = string.Format("ura_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);
            string[] sizeList = StringHelper.SplitString(shopConfig.UserRankAvatarThumbSize);

            string sourceDirPath = dirPath + "source/";
            if (!Directory.Exists(sourceDirPath))
                Directory.CreateDirectory(sourceDirPath);

            string sourcePath = sourceDirPath + newFileName;
            avatar.SaveAs(sourcePath);

            foreach (string size in sizeList)
            {
                string thumbDirPath = string.Format("{0}thumb{1}/", dirPath, size);
                if (!Directory.Exists(thumbDirPath))
                    Directory.CreateDirectory(thumbDirPath);
                string[] widthAndHeight = StringHelper.SplitString(size, "_");
                IOHelper.GenerateThumb(sourcePath,
                                       thumbDirPath + newFileName,
                                       TypeHelper.StringToInt(widthAndHeight[0]),
                                       TypeHelper.StringToInt(widthAndHeight[1]),
                                       "H");
            }
            return newFileName;
        }

        /// <summary>
        /// 保存上传的品牌logo
        /// </summary>
        /// <param name="logo">品牌logo</param>
        /// <returns></returns>
        public string SaveUploadBrandLogo(HttpPostedFileBase logo)
        {
            if (logo == null)
                return "-1";

            ShopConfigInfo shopConfig = BSPConfig.ShopConfig;

            string fileName = logo.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, shopConfig.UploadImgType))
                return "-2";

            int fileSize = logo.ContentLength;
            if (fileSize > shopConfig.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/brand/");
            string newFileName = string.Format("b_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);
            string[] sizeList = StringHelper.SplitString(shopConfig.BrandThumbSize);

            string sourceDirPath = dirPath + "source/";
            if (!Directory.Exists(sourceDirPath))
                Directory.CreateDirectory(sourceDirPath);

            string sourcePath = sourceDirPath + newFileName;
            logo.SaveAs(sourcePath);

            foreach (string size in sizeList)
            {
                string thumbDirPath = string.Format("{0}thumb{1}/", dirPath, size);
                if (!Directory.Exists(thumbDirPath))
                    Directory.CreateDirectory(thumbDirPath);
                string[] widthAndHeight = StringHelper.SplitString(size, "_");
                IOHelper.GenerateThumb(sourcePath,
                                       thumbDirPath + newFileName,
                                       TypeHelper.StringToInt(widthAndHeight[0]),
                                       TypeHelper.StringToInt(widthAndHeight[1]),
                                       "H");
            }
            return newFileName;
        }

        /// <summary>
        /// 保存新闻编辑器中的图片
        /// </summary>
        /// <param name="image">新闻图片</param>
        /// <returns></returns>
        public string SaveNewsEditorImage(HttpPostedFileBase image)
        {
            if (image == null)
                return "-1";

            ShopConfigInfo shopConfig = BSPConfig.ShopConfig;

            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, shopConfig.UploadImgType))
                return "-2";

            int fileSize = image.ContentLength;
            if (fileSize > shopConfig.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/news/");
            string newFileName = string.Format("n_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            image.SaveAs(dirPath + newFileName);

            return newFileName;
        }

        /// <summary>
        /// 保存帮助编辑器中的图片
        /// </summary>
        /// <param name="image">帮助图片</param>
        /// <returns></returns>
        public string SaveHelpEditorImage(HttpPostedFileBase image)
        {
            if (image == null)
                return "-1";

            ShopConfigInfo shopConfig = BSPConfig.ShopConfig;

            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, shopConfig.UploadImgType))
                return "-2";

            int fileSize = image.ContentLength;
            if (fileSize > shopConfig.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/help/");
            string newFileName = string.Format("h_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            image.SaveAs(dirPath + newFileName);

            return newFileName;
        }

        /// <summary>
        /// 保存商品编辑器中的图片
        /// </summary>
        /// <param name="image">商品图片</param>
        /// <returns></returns>
        public string SaveProductEditorImage(HttpPostedFileBase image,string cateid="")
        {
            if (image == null)
                return "-1";

            ShopConfigInfo shopConfig = BSPConfig.ShopConfig;

            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, shopConfig.UploadImgType))
                return "-2";

            int fileSize = image.ContentLength;
            if (fileSize > shopConfig.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/product/editor/");
            string newFileName = string.Format("pe_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名

            string sourceDirPath = dirPath + "source\\";
            if (cateid != "")
                sourceDirPath += cateid + "\\";
            if (!Directory.Exists(sourceDirPath))
                Directory.CreateDirectory(sourceDirPath);
            string sourcePath = sourceDirPath + newFileName;
            image.SaveAs(sourcePath);

            if (cateid != "")
                dirPath += cateid + "\\";

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            string path = dirPath + newFileName;
            
            if (shopConfig.WatermarkType == 1)//文字水印
            {
                IOHelper.GenerateTextWatermark(sourcePath, path, shopConfig.WatermarkText, shopConfig.WatermarkTextSize, shopConfig.WatermarkTextFont, shopConfig.WatermarkPosition, shopConfig.WatermarkQuality);
            }
            else if (shopConfig.WatermarkType == 2)//图片水印
            {
                string watermarkPath = IOHelper.GetMapPath("/watermarks/" + shopConfig.WatermarkImg);
                IOHelper.GenerateImageWatermark(sourcePath, watermarkPath, path, shopConfig.WatermarkPosition, shopConfig.WatermarkImgOpacity, shopConfig.WatermarkQuality);
            }
            else
            {
                image.SaveAs(path);
            }

            return newFileName;
        }

        /// <summary>
        /// 保存上传的商品图片
        /// </summary>
        /// <param name="image">商品图片</param>
        /// <returns></returns>
        public string SaveUplaodProductImage(HttpPostedFileBase image, string cateid = "")
        {
            if (image == null)
                return "-1";

            ShopConfigInfo shopConfig = BSPConfig.ShopConfig;

            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, shopConfig.UploadImgType))
                return "-2";

            int fileSize = image.ContentLength;
            if (fileSize > shopConfig.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/product/show/");
            string name = "ps_" + DateTime.Now.ToString("yyMMddHHmmssfffffff");
            string newFileName = name + extension;
            string[] sizeList = StringHelper.SplitString(shopConfig.ProductShowThumbSize);

            string sourceDirPath = string.Format("{0}source\\", dirPath);

            if (cateid != "")
                sourceDirPath += cateid + "\\";

            if (!Directory.Exists(sourceDirPath))
                Directory.CreateDirectory(sourceDirPath);
            string sourcePath = sourceDirPath + newFileName;
            image.SaveAs(sourcePath);

            string state = "";
            using (Image imgsv = Image.FromFile(sourcePath))
            {
                if (imgsv.Width != 350 || imgsv.Height != 350)
                {
                    state = "-4";
                }
            }
            if (state != "")
            {
                File.Delete(sourcePath);
                return state;
            }

            if (shopConfig.WatermarkType == 1)//文字水印
            {
                string path = string.Format("{0}{1}_text{2}", sourceDirPath, name, extension);
                IOHelper.GenerateTextWatermark(sourcePath, path, shopConfig.WatermarkText, shopConfig.WatermarkTextSize, shopConfig.WatermarkTextFont, shopConfig.WatermarkPosition, shopConfig.WatermarkQuality);
                sourcePath = path;
            }
            else if (shopConfig.WatermarkType == 2)//图片水印
            {
                string path = string.Format("{0}{1}_img{2}", sourceDirPath, name, extension);
                string watermarkPath = IOHelper.GetMapPath("/watermarks/" + shopConfig.WatermarkImg);
                IOHelper.GenerateImageWatermark(sourcePath, watermarkPath, path, shopConfig.WatermarkPosition, shopConfig.WatermarkImgOpacity, shopConfig.WatermarkQuality);
                sourcePath = path;
            }

            

            foreach (string size in sizeList)
            {
                string thumbDirPath = string.Format("{0}thumb{1}/", dirPath, size);
                if (cateid != "")
                    thumbDirPath += cateid + "\\";
                if (!Directory.Exists(thumbDirPath))
                    Directory.CreateDirectory(thumbDirPath);
                string[] widthAndHeight = StringHelper.SplitString(size, "_");
                IOHelper.GenerateThumb(sourcePath,
                                       thumbDirPath + newFileName,
                                       TypeHelper.StringToInt(widthAndHeight[0]),
                                       TypeHelper.StringToInt(widthAndHeight[1]),
                                       "H");
            }
            return newFileName;
        }

        /// <summary>
        /// 保存上传的广告图片
        /// </summary>
        /// <param name="image">广告图片</param>
        /// <returns></returns>
        public string SaveUploadAdvertImage(HttpPostedFileBase image)
        {
            if (image == null)
                return "-1";

            ShopConfigInfo shopConfig = BSPConfig.ShopConfig;

            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, shopConfig.UploadImgType))
                return "-2";

            int fileSize = image.ContentLength;
            if (fileSize > shopConfig.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/adv/");
            string newFileName = string.Format("ad_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            image.SaveAs(dirPath + newFileName);

            return newFileName;
        }

        /// <summary>
        /// 保存上传的友情链接Logo
        /// </summary>
        /// <param name="logo">友情链接logo</param>
        /// <returns></returns>
        public string SaveUploadFriendLinkLogo(HttpPostedFileBase logo)
        {
            if (logo == null)
                return "-1";

            ShopConfigInfo shopConfig = BSPConfig.ShopConfig;

            string fileName = logo.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, shopConfig.UploadImgType))
                return "-2";

            int fileSize = logo.ContentLength;
            if (fileSize > shopConfig.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/friendlink/");
            string newFileName = string.Format("fr_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            logo.SaveAs(dirPath + newFileName);

            return newFileName;
        }

        /// <summary>
        /// 保存上传的基本资料图片
        /// </summary>
        /// <param name="baseinfo">基本资料图片</param>
        /// <returns></returns>
        public string SaveUploadBaseInfo(HttpPostedFileBase baseinfo)
        {
            if (baseinfo == null)
                return "-1";

            ShopConfigInfo shopConfig = BSPConfig.ShopConfig;

            string fileName = baseinfo.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, shopConfig.UploadImgType))
                return "-2";

            int fileSize = baseinfo.ContentLength;
            if (fileSize > shopConfig.UploadImgSize)
                return "-3";

            string dirPath = IOHelper.GetMapPath("/upload/baseinfo/");
            string newFileName = string.Format("fr_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension);//生成文件名

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            baseinfo.SaveAs(dirPath + newFileName);

            //string watermarkPath = IOHelper.GetMapPath("/watermarks/" + shopConfig.WatermarkImg);
            //IOHelper.GenerateImageWatermark(dirPath + newFileName, watermarkPath, dirPath + "dd.jpg", shopConfig.WatermarkPosition, shopConfig.WatermarkImgOpacity, shopConfig.WatermarkQuality);
            //IOHelper.BuildWatermark(dirPath + newFileName, watermarkPath, "", dirPath+"dd.jpg");

           

            return newFileName;
        }
    }
}
