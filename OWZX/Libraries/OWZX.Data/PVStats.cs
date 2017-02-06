using System;
using System.Data;
using System.Collections.Generic;

using OWZX.Core;

namespace OWZX.Data
{
    /// <summary>
    /// PV统计数据访问类
    /// </summary>
    public partial class PVStats
    {
        #region 辅助方法

        /// <summary>
        /// 从IDataReader创建PVStatInfo
        /// </summary>
        public static PVStatInfo BuildPVStatFromReader(IDataReader reader)
        {
            PVStatInfo pvStatInfo = new PVStatInfo();

            pvStatInfo.Category = reader["category"].ToString();
            pvStatInfo.Value = reader["value"].ToString();
            pvStatInfo.Count = TypeHelper.ObjectToInt(reader["count"]);

            return pvStatInfo;
        }

        #endregion

        /// <summary>
        /// 更新PV统计
        /// </summary>
        /// <param name="updatePVStatState">更新PV统计状态</param>
        public static void UpdatePVStat(UpdatePVStatState updatePVStatState)
        {
            OWZX.Core.BSPData.RDBS.UpdatePVStat(updatePVStatState);
        }
        /// <summary>
        /// 获取访问IP统计
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static DataTable GetWebFlow(int pageNumber, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetWebFlow(pageNumber,pageSize,condition);
        }
        /// <summary>
        /// 获得省级区域统计
        /// </summary>
        /// <returns></returns>
        public static DataTable GetProvinceRegionStat()
        {
            return OWZX.Core.BSPData.RDBS.GetProvinceRegionStat();
        }

        /// <summary>
        /// 获得市级区域统计
        /// </summary>
        /// <param name="provinceId">省id</param>
        /// <returns></returns>
        public static DataTable GetCityRegionStat(int provinceId)
        {
            return OWZX.Core.BSPData.RDBS.GetCityRegionStat(provinceId);
        }

        /// <summary>
        /// 获得区/县级区域统计
        /// </summary>
        /// <param name="cityId">市id</param>
        /// <returns></returns>
        public static DataTable GetCountyRegionStat(int cityId)
        {
            return OWZX.Core.BSPData.RDBS.GetCountyRegionStat(cityId);
        }

        /// <summary>
        /// 获得PV统计列表
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static List<PVStatInfo> GetPVStatList(string condition)
        {
            List<PVStatInfo> pvStatList = new List<PVStatInfo>();
            IDataReader reader = OWZX.Core.BSPData.RDBS.GetPVStatList(condition);
            while (reader.Read())
            {
                PVStatInfo pvStatInfo = BuildPVStatFromReader(reader);
                pvStatList.Add(pvStatInfo);
            }

            reader.Close();
            return pvStatList;
        }

        /// <summary>
        /// 获得PV统计
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static PVStatInfo GetPVStatByCategoryAndValue(string category, string value)
        {
            PVStatInfo pvStatInfo = null;
            IDataReader reader = OWZX.Core.BSPData.RDBS.GetPVStatByCategoryAndValue(category, value);
            if (reader.Read())
            {
                pvStatInfo = BuildPVStatFromReader(reader);
            }

            reader.Close();
            return pvStatInfo;
        }
    }
}
