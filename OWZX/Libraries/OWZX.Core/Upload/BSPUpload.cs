using System;
using System.IO;

namespace OWZX.Core
{
    /// <summary>
    /// OWZX上传管理类
    /// </summary>
    public class BSPUpload
    {
        private static IUploadStrategy _iuploadstrategy = null;//上传策略

        static BSPUpload()
        {
            try
            {
                string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "OWZX.UploadStrategy.*.dll", SearchOption.TopDirectoryOnly);
                _iuploadstrategy = (IUploadStrategy)Activator.CreateInstance(Type.GetType(string.Format("OWZX.UploadStrategy.{0}.UploadStrategy, OWZX.UploadStrategy.{0}", fileNameList[0].Substring(fileNameList[0].IndexOf("UploadStrategy.") + 15).Replace(".dll", "")),
                                                                                          false,
                                                                                          true));
            }
            catch
            {
                throw new BSPException("创建'上传策略对象'失败,可能存在的原因:未将'上传策略程序集'添加到bin目录中;'上传策略程序集'文件名不符合'OWZX.UploadStrategy.{策略名称}.dll'格式");
            }
        }

        /// <summary>
        /// 上传策略实例
        /// </summary>
        public static IUploadStrategy Instance
        {
            get { return _iuploadstrategy; }
        }
    }
}
