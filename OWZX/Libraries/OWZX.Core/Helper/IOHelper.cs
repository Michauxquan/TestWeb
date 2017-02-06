using System;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using System.Net;

namespace OWZX.Core
{
    /// <summary>
    /// IO帮助类
    /// </summary>
    public class IOHelper
    {
        //是否已经加载了JPEG编码解码器
        private static bool _isloadjpegcodec = false;
        //当前系统安装的JPEG编码解码器
        private static ImageCodecInfo _jpegcodec = null;

        /// <summary>
        /// 获得文件物理路径
        /// </summary>
        /// <returns></returns>
        public static string GetMapPath(string path)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(path);
            }
            else
            {
                return System.Web.Hosting.HostingEnvironment.MapPath(path);
            }
        }

        #region  序列化

        /// <summary>
        /// XML序列化
        /// </summary>
        /// <param name="obj">序列对象</param>
        /// <param name="filePath">XML文件路径</param>
        /// <returns>是否成功</returns>
        public static bool SerializeToXml(object obj, string filePath)
        {
            bool result = false;

            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return result;

        }

        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <param name="type">目标类型(Type类型)</param>
        /// <param name="filePath">XML文件路径</param>
        /// <returns>序列对象</returns>
        public static object DeserializeFromXML(Type type, string filePath)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        #endregion

        #region  水印,缩略图

        /// <summary>
        /// 获得当前系统安装的JPEG编码解码器
        /// </summary>
        /// <returns></returns>
        public static ImageCodecInfo GetJPEGCodec()
        {
            if (_isloadjpegcodec == true)
                return _jpegcodec;

            ImageCodecInfo[] codecsList = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecsList)
            {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                {
                    _jpegcodec = codec;
                    break;
                }

            }
            _isloadjpegcodec = true;
            return _jpegcodec;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <param name="thumbPath">缩略图路径</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>   
        public static void GenerateThumb(string imagePath, string thumbPath, int width, int height, string mode)
        {
            Image image = Image.FromFile(imagePath);

            string extension = imagePath.Substring(imagePath.LastIndexOf(".")).ToLower();
            ImageFormat imageFormat = null;
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case ".bmp":
                    imageFormat = ImageFormat.Bmp;
                    break;
                case ".png":
                    imageFormat = ImageFormat.Png;
                    break;
                case ".gif":
                    imageFormat = ImageFormat.Gif;
                    break;
                default:
                    imageFormat = ImageFormat.Jpeg;
                    break;
            }

            int toWidth = width > 0 ? width : image.Width;
            int toHeight = height > 0 ? height : image.Height;

            int x = 0;
            int y = 0;
            int ow = image.Width;
            int oh = image.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）           
                    break;
                case "W"://指定宽，高按比例             
                    toHeight = image.Height * width / image.Width;
                    break;
                case "H"://指定高，宽按比例
                    toWidth = image.Width * height / image.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）           
                    if ((double)image.Width / (double)image.Height > (double)toWidth / (double)toHeight)
                    {
                        oh = image.Height;
                        ow = image.Height * toWidth / toHeight;
                        y = 0;
                        x = (image.Width - ow) / 2;
                    }
                    else
                    {
                        ow = image.Width;
                        oh = image.Width * height / toWidth;
                        x = 0;
                        y = (image.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp
            Image bitmap = new Bitmap(toWidth, toHeight);

            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(image,
                        new Rectangle(0, 0, toWidth, toHeight),
                        new Rectangle(x, y, ow, oh),
                        GraphicsUnit.Pixel);

            try
            {
                bitmap.Save(thumbPath, imageFormat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (g != null)
                    g.Dispose();
                if (bitmap != null)
                    bitmap.Dispose();
                if (image != null)
                    image.Dispose();
            }
        }

        /// <summary>
        /// 生成图片水印
        /// </summary>
        /// <param name="originalPath">源图路径</param>
        /// <param name="watermarkPath">水印图片路径</param>
        /// <param name="targetPath">保存路径 (完成路径 包括名称)</param>
        /// <param name="position">位置</param>
        /// <param name="opacity">透明度</param>
        /// <param name="quality">质量</param>
        public static void GenerateImageWatermark(string originalPath, string watermarkPath, string targetPath, int position, int opacity, int quality)
        {
            Image originalImage = null;
            Image watermarkImage = null;
            //图片属性
            ImageAttributes attributes = null;
            //画板
            Graphics g = null;
            try
            {

                originalImage = Image.FromFile(originalPath);
                watermarkImage = new Bitmap(watermarkPath);

                if (watermarkImage.Height >= originalImage.Height || watermarkImage.Width >= originalImage.Width)
                {
                    originalImage.Save(targetPath);
                    return;
                }

                if (quality < 0 || quality > 100)
                    quality = 100;

                ////水印透明度
                //float iii;
                //if (opacity > 0 && opacity <= 10)
                //    iii = (float)(opacity / 10.0F);
                //else
                //    iii = 0.5F;//

                //水印位置
                int x = 0;
                int y = 0;
                switch (position)
                {
                    case 1:
                        x = (int)(originalImage.Width * (float).01);
                        y = (int)(originalImage.Height * (float).01);
                        break;
                    case 2:
                        x = (int)((originalImage.Width * (float).50) - (watermarkImage.Width / 2));
                        y = (int)(originalImage.Height * (float).01);
                        break;
                    case 3:
                        x = (int)((originalImage.Width * (float).99) - (watermarkImage.Width));
                        y = (int)(originalImage.Height * (float).01);
                        break;
                    case 4:
                        x = (int)(originalImage.Width * (float).01);
                        y = (int)((originalImage.Height * (float).50) - (watermarkImage.Height / 2));
                        break;
                    case 5:
                        x = (int)((originalImage.Width * (float).50) - (watermarkImage.Width / 2));
                        y = (int)((originalImage.Height * (float).50) - (watermarkImage.Height / 2));
                        break;
                    case 6:
                        x = (int)((originalImage.Width * (float).99) - (watermarkImage.Width));
                        y = (int)((originalImage.Height * (float).50) - (watermarkImage.Height / 2));
                        break;
                    case 7:
                        x = (int)(originalImage.Width * (float).01);
                        y = (int)((originalImage.Height * (float).99) - watermarkImage.Height);
                        break;
                    case 8:
                        x = (int)((originalImage.Width * (float).50) - (watermarkImage.Width / 2));
                        y = (int)((originalImage.Height * (float).99) - watermarkImage.Height);
                        break;
                    case 9:
                        x = (int)((originalImage.Width * (float)0.83) - (watermarkImage.Width));
                        y = (int)((originalImage.Height * (float).95) - watermarkImage.Height);
                        break;
                    //case 9:
                    //    x = (int)((originalImage.Width * (float).99) - (watermarkImage.Width));
                    //    y = (int)((originalImage.Height * (float).99) - watermarkImage.Height);
                    //    break;

                }

                ////颜色映射表
                //ColorMap colorMap = new ColorMap();
                //colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                //colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                //ColorMap[] newColorMap = { colorMap };

                ////颜色变换矩阵,iii是设置透明度的范围0到1中的单精度类型
                //float[][] newColorMatrix ={ 
                //                            new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                //                            new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                //                            new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                //                            new float[] {0.0f,  0.0f,  0.0f,  iii, 0.0f},
                //                            new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                //                           };
                //float[][] newColorMatrix ={ 
                //                            new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                //                            new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                //                            new float[] {0.0f,  0.0f,  1f,  1f, 0.0f},
                //                           new float[] {0.0f,  0.0f,  0.0f,  iii, 0.0f},
                //                           new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                //                           };
                ////定义一个 5 x 5 矩阵
                //ColorMatrix matrix = new ColorMatrix(newColorMatrix);

                ////图片属性
                //attributes = new ImageAttributes();
                //attributes.SetRemapTable(newColorMap, ColorAdjustType.Bitmap);
                //attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                //画板
                g = Graphics.FromImage(originalImage);
                //绘制水印
                g.DrawImage(watermarkImage, new Rectangle(x, y, watermarkImage.Width, watermarkImage.Height), 0, 0, watermarkImage.Width, watermarkImage.Height, GraphicsUnit.Pixel);
                //保存图片
                EncoderParameters encoderParams = new EncoderParameters();
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, new long[] { quality });


                //if (GetJPEGCodec() != null)
                //    originalImage.Save(targetPath, _jpegcodec, encoderParams);
                //else
                    originalImage.Save(targetPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (g != null)
                    g.Dispose();
                if (attributes != null)
                    attributes.Dispose();
                if (watermarkImage != null)
                    watermarkImage.Dispose();
                if (originalImage != null)
                    originalImage.Dispose();
            }
        }

        /// <summary>      
        /// Creating a Watermarked Photograph with GDI+ for .NET      
        /// </summary>      
        /// <param name="rSrcImgPath">原始图片的物理路径</param>      
        /// <param name="rMarkImgPath">水印图片的物理路径</param>      
        /// <param name="rMarkText">水印文字（不显示水印文字设为空串）</param>      
        /// <param name="rDstImgPath">输出合成后的图片的物理路径</param>      
        public static void BuildWatermark(string rSrcImgPath, string rMarkImgPath, string rMarkText, string rDstImgPath)
        {
            //以下（代码）从一个指定文件创建了一个Image 对象，然后为它的 Width 和 Height定义变量。      
            //这些长度待会被用来建立一个以24 bits 每像素的格式作为颜色数据的Bitmap对象。      
            Image imgPhoto = Image.FromFile(rSrcImgPath);
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(72, 72);
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            //这个代码载入水印图片，水印图片已经被保存为一个BMP文件，以绿色(A=0,R=0,G=255,B=0)作为背景颜色。      
            //再一次，会为它的Width 和Height定义一个变量。      
            Image imgWatermark = new Bitmap(rMarkImgPath);
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;
            //这个代码以100%它的原始大小绘制imgPhoto 到Graphics 对象的（x=0,y=0）位置。      
            //以后所有的绘图都将发生在原来照片的顶部。      
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.DrawImage(
                 imgPhoto,
                 new Rectangle(0, 0, phWidth, phHeight),
                 0,
                 0,
                 phWidth,
                 phHeight,
                 GraphicsUnit.Pixel);
            //为了最大化版权信息的大小，我们将测试7种不同的字体大小来决定我们能为我们的照片宽度使用的可能的最大大小。      
            //为了有效地完成这个，我们将定义一个整型数组，接着遍历这些整型值测量不同大小的版权字符串。      
            //一旦我们决定了可能的最大大小，我们就退出循环，绘制文本      
            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };
            Font crFont = null;
            SizeF crSize = new SizeF();
            for (int i = 0; i < 7; i++)
            {
                crFont = new Font("arial", sizes[i],
                      FontStyle.Bold);
                crSize = grPhoto.MeasureString(rMarkText,
                      crFont);
                if ((ushort)crSize.Width < (ushort)phWidth)
                    break;
            }
            //因为所有的照片都有各种各样的高度，所以就决定了从图象底部开始的5%的位置开始。      
            //使用rMarkText字符串的高度来决定绘制字符串合适的Y坐标轴。      
            //通过计算图像的中心来决定X轴，然后定义一个StringFormat 对象，设置StringAlignment 为Center。      
            int yPixlesFromBottom = (int)(phHeight * .05);
            float yPosFromBottom = ((phHeight -
                 yPixlesFromBottom) - (crSize.Height / 2));
            float xCenterOfImg = (phWidth / 2);
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;
            //现在我们已经有了所有所需的位置坐标来使用60%黑色的一个Color(alpha值153)创建一个SolidBrush 。      
            //在偏离右边1像素，底部1像素的合适位置绘制版权字符串。      
            //这段偏离将用来创建阴影效果。使用Brush重复这样一个过程，在前一个绘制的文本顶部绘制同样的文本。      
            SolidBrush semiTransBrush2 =
                 new SolidBrush(Color.FromArgb(153, 0, 0, 0));
            grPhoto.DrawString(rMarkText,
                 crFont,
                 semiTransBrush2,
                 new PointF(xCenterOfImg + 1, yPosFromBottom + 1),
                 StrFormat);
            SolidBrush semiTransBrush = new SolidBrush(
                 Color.FromArgb(153, 255, 255, 255));
            grPhoto.DrawString(rMarkText,
                 crFont,
                 semiTransBrush,
                 new PointF(xCenterOfImg, yPosFromBottom),
                 StrFormat);
            //根据前面修改后的照片创建一个Bitmap。把这个Bitmap载入到一个新的Graphic对象。      
            Bitmap bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(
                 imgPhoto.HorizontalResolution,
                 imgPhoto.VerticalResolution);
            Graphics grWatermark =
                 Graphics.FromImage(bmWatermark);
            //通过定义一个ImageAttributes 对象并设置它的两个属性，我们就是实现了两个颜色的处理，以达到半透明的水印效果。      
            //处理水印图象的第一步是把背景图案变为透明的(Alpha=0, R=0, G=0, B=0)。我们使用一个Colormap 和定义一个RemapTable来做这个。      
            //就像前面展示的，我的水印被定义为100%绿色背景，我们将搜到这个颜色，然后取代为透明。      
            ImageAttributes imageAttributes =
                 new ImageAttributes();
            ColorMap colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };
            //第二个颜色处理用来改变水印的不透明性。      
            //通过应用包含提供了坐标的RGBA空间的5x5矩阵来做这个。      
            //通过设定第三行、第三列为0.3f我们就达到了一个不透明的水平。结果是水印会轻微地显示在图象底下一些。      
            imageAttributes.SetRemapTable(remapTable,
                 ColorAdjustType.Bitmap);
            float[][] colorMatrixElements = {       
                                             new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},      
                                             new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},      
                                             new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},      
                                             new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},      
                                             new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}      
                                        };
            ColorMatrix wmColorMatrix = new
                 ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(wmColorMatrix,
                 ColorMatrixFlag.Default,
                 ColorAdjustType.Bitmap);
            //随着两个颜色处理加入到imageAttributes 对象，我们现在就能在照片右手边上绘制水印了。      
            //我们会偏离10像素到底部，10像素到左边。      
            int markWidth;
            int markHeight;
            //mark比原来的图宽      
            if (phWidth <= wmWidth)
            {
                markWidth = phWidth - 10;
                markHeight = (markWidth * wmHeight) / wmWidth;
            }
            else if (phHeight <= wmHeight)
            {
                markHeight = phHeight - 10;
                markWidth = (markHeight * wmWidth) / wmHeight;
            }
            else
            {
                markWidth = wmWidth;
                markHeight = wmHeight;
            }
            int xPosOfWm = ((phWidth - markWidth) - 10);
            int yPosOfWm = 10;
            grWatermark.DrawImage(imgWatermark,
                 new Rectangle(xPosOfWm, yPosOfWm, markWidth,
                 markHeight),
                 0,
                 0,
                 wmWidth,
                 wmHeight,
                 GraphicsUnit.Pixel,
                 imageAttributes);
            //最后的步骤将是使用新的Bitmap取代原来的Image。 销毁两个Graphic对象，然后把Image 保存到文件系统。      
            imgPhoto = bmWatermark;
            grPhoto.Dispose();
            grWatermark.Dispose();

            imgPhoto.Save(rDstImgPath, ImageFormat.Jpeg);
            imgPhoto.Dispose();
            imgWatermark.Dispose();
        }

        /// <summary>
        /// 生成文字水印
        /// </summary>
        /// <param name="originalPath">源图路径</param>
        /// <param name="targetPath">保存路径</param>
        /// <param name="text">水印文字</param>
        /// <param name="textSize">文字大小</param>
        /// <param name="textFont">文字字体</param>
        /// <param name="position">位置</param>
        /// <param name="quality">质量</param>
        public static void GenerateTextWatermark(string originalPath, string targetPath, string text, int textSize, string textFont, int position, int quality)
        {
            Image originalImage = null;
            //画板
            Graphics g = null;
            try
            {
                originalImage = Image.FromFile(originalPath);
                //画板
                g = Graphics.FromImage(originalImage);
                if (quality < 0 || quality > 100)
                    quality = 80;

                Font font = new Font(textFont, textSize, FontStyle.Regular, GraphicsUnit.Pixel);
                SizeF sizePair = g.MeasureString(text, font);

                float x = 0;
                float y = 0;

                switch (position)
                {
                    case 1:
                        x = (float)originalImage.Width * (float).01;
                        y = (float)originalImage.Height * (float).01;
                        break;
                    case 2:
                        x = ((float)originalImage.Width * (float).50) - (sizePair.Width / 2);
                        y = (float)originalImage.Height * (float).01;
                        break;
                    case 3:
                        x = ((float)originalImage.Width * (float).99) - sizePair.Width;
                        y = (float)originalImage.Height * (float).01;
                        break;
                    case 4:
                        x = (float)originalImage.Width * (float).01;
                        y = ((float)originalImage.Height * (float).50) - (sizePair.Height / 2);
                        break;
                    case 5:
                        x = ((float)originalImage.Width * (float).50) - (sizePair.Width / 2);
                        y = ((float)originalImage.Height * (float).50) - (sizePair.Height / 2);
                        break;
                    case 6:
                        x = ((float)originalImage.Width * (float).99) - sizePair.Width;
                        y = ((float)originalImage.Height * (float).50) - (sizePair.Height / 2);
                        break;
                    case 7:
                        x = (float)originalImage.Width * (float).01;
                        y = ((float)originalImage.Height * (float).99) - sizePair.Height;
                        break;
                    case 8:
                        x = ((float)originalImage.Width * (float).50) - (sizePair.Width / 2);
                        y = ((float)originalImage.Height * (float).99) - sizePair.Height;
                        break;
                    case 9:
                        x = ((float)originalImage.Width * (float).99) - sizePair.Width;
                        y = ((float)originalImage.Height * (float).99) - sizePair.Height;
                        break;
                }

                g.DrawString(text, font, new SolidBrush(Color.White), x + 1, y + 1);
                g.DrawString(text, font, new SolidBrush(Color.Black), x, y);

                //保存图片
                EncoderParameters encoderParams = new EncoderParameters();
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, new long[] { quality });
                if (GetJPEGCodec() != null)
                    originalImage.Save(targetPath, _jpegcodec, encoderParams);
                else
                    originalImage.Save(targetPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (g != null)
                    g.Dispose();
                if (originalImage != null)
                    originalImage.Dispose();
            }
        }

        #endregion

        #region http下载文件
        /// <summary>
        /// Http下载文件
        /// </summary>
        public static string HttpDownloadFile(string url, string path)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            //创建本地文件写入流
            Stream stream = new FileStream(path, FileMode.Create);
            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
            }
            stream.Close();
            responseStream.Close();
            return path;
        }
        #endregion
    }
}
