using System;
using System.IO;

namespace OWZX.Core
{
    /// <summary>
    /// OWZX搜索管理类
    /// </summary>
    public class BSPSearch
    {
        private static ISearchStrategy _isearchstrategy = null;//搜索策略

        static BSPSearch()
        {
            try
            {
                string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "OWZX.SearchStrategy.*.dll", SearchOption.TopDirectoryOnly);
                _isearchstrategy = (ISearchStrategy)Activator.CreateInstance(Type.GetType(string.Format("OWZX.SearchStrategy.{0}.SearchStrategy, OWZX.SearchStrategy.{0}", fileNameList[0].Substring(fileNameList[0].IndexOf("SearchStrategy.") + 15).Replace(".dll", "")),
                                                                                          false,
                                                                                          true));
            }
            catch
            {
                throw new BSPException("创建'搜索策略对象'失败,可能存在的原因:未将'搜索策略对象'添加到bin目录中;'搜索策略对象'文件名不符合'OWZX.SearchStrategy.{策略名称}.dll'格式");
            }
        }

        /// <summary>
        /// 搜索策略实例
        /// </summary>
        public static ISearchStrategy Instance
        {
            get { return _isearchstrategy; }
        }
    }
}
