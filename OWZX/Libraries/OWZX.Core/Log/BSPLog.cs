using System;
using System.IO;

namespace OWZX.Core
{
    /// <summary>
    /// OWZX日志管理类
    /// </summary>
    public class BSPLog
    {
        private static ILogStrategy _ilogstrategy = null;//日志策略

        static BSPLog()
        {
            try
            {
                string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "OWZX.LogStrategy.*.dll", SearchOption.TopDirectoryOnly);
                _ilogstrategy = (ILogStrategy)Activator.CreateInstance(Type.GetType(string.Format("OWZX.LogStrategy.{0}.LogStrategy, OWZX.LogStrategy.{0}", fileNameList[0].Substring(fileNameList[0].IndexOf("LogStrategy.") + 12).Replace(".dll", "")),
                                                                                    false,
                                                                                    true));
            }
            catch
            {
                throw new BSPException("创建'日志策略对象'失败,可能存在的原因:未将'日志策略程序集'添加到bin目录中;'日志策略程序集'文件名不符合'OWZX.LogStrategy.{策略名称}.dll'格式");
            }
        }

        /// <summary>
        /// 日志策略实例
        /// </summary>
        public static ILogStrategy Instance
        {
            get { return _ilogstrategy; }
        }
    }
}
