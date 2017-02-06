using System;
using System.IO;

namespace OWZX.Core
{
    /// <summary>
    /// OWZX数据管理类
    /// </summary>
    public partial class BSPData
    {
        private static IRDBSStrategy _irdbsstrategy = null;//关系型数据库策略

        private static object _locker = new object();//锁对象
        private static bool _enablednosql = false;//是否启用非关系型数据库
       

        static BSPData()
        {
            try
            {
                string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "OWZX.RDBSStrategy.*.dll", SearchOption.TopDirectoryOnly);
                _irdbsstrategy = (IRDBSStrategy)Activator.CreateInstance(Type.GetType(string.Format("OWZX.RDBSStrategy.{0}.RDBSStrategy, OWZX.RDBSStrategy.{0}", fileNameList[0].Substring(fileNameList[0].LastIndexOf("RDBSStrategy.") + 13).Replace(".dll", "")),
                                                                                            false,
                                                                                            true));
            }
            catch
            {
                throw new BSPException("创建'关系数据库策略对象'失败,可能存在的原因:未将'关系数据库策略程序集'添加到bin目录中;'关系数据库策略程序集'文件名不符合'OWZX.RDBSStrategy.{策略名称}.dll'格式");
            }
            _enablednosql = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "OWZX.NOSQLStrategy.*.dll", SearchOption.TopDirectoryOnly).Length > 0;
        }

        /// <summary>
        /// 关系型数据库
        /// </summary>
        public static IRDBSStrategy RDBS
        {
            get { return _irdbsstrategy; }
        }
    }
}
