﻿using System;
using System.Data;
using System.Collections.Generic;

using OWZX.Core;
using OWZX.Model;

namespace OWZX.Services
{
    /// <summary>
    /// PV统计操作管理类
    /// </summary>
    public partial class PVStats
    {
        /// <summary>
        /// 更新PV统计
        /// </summary>
        /// <param name="updatePVStatState">更新PV统计状态</param>
        public static void UpdatePVStat(object updatePVStatState)
        {
            OWZX.Core.BSPData.RDBS.UpdatePVStat((UpdatePVStatState)updatePVStatState);
        }
        /// <summary>
        /// 获取访问IP统计
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static List<MD_VisitIP> GetWebFlow(int pageNumber, int pageSize, string condition = "")
        {
            DataTable dt= OWZX.Data.PVStats.GetWebFlow(pageNumber,pageSize,condition);
            List<MD_VisitIP> list = (List<MD_VisitIP>)ModelConvertHelper<MD_VisitIP>.ConvertToModel(dt);
            return list;
        }
        /// <summary>
        /// 获得省级区域统计
        /// </summary>
        /// <returns></returns>
        public static DataTable GetProvinceRegionStat()
        {
            return OWZX.Data.PVStats.GetProvinceRegionStat();
        }

        /// <summary>
        /// 获得市级区域统计
        /// </summary>
        /// <param name="provinceId">省id</param>
        /// <returns></returns>
        public static DataTable GetCityRegionStat(int provinceId)
        {
            return OWZX.Data.PVStats.GetCityRegionStat(provinceId);
        }

        /// <summary>
        /// 获得区/县级区域统计
        /// </summary>
        /// <param name="cityId">市id</param>
        /// <returns></returns>
        public static DataTable GetCountyRegionStat(int cityId)
        {
            return OWZX.Data.PVStats.GetCountyRegionStat(cityId);
        }

        /// <summary>
        /// 获得PV统计列表
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static List<PVStatInfo> GetPVStatList(string condition)
        {
            return OWZX.Data.PVStats.GetPVStatList(condition);
        }

        /// <summary>
        /// 获得PV统计
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static PVStatInfo GetPVStatByCategoryAndValue(string category, string value)
        {
            return OWZX.Data.PVStats.GetPVStatByCategoryAndValue(category, value);
        }

        /// <summary>
        /// 获得小时的PV统计列表
        /// </summary>
        /// <param name="startHour">开始小时</param>
        /// <param name="endHour">结束小时</param>
        /// <returns></returns>
        public static List<PVStatInfo> GetHourPVStatList(string startHour, string endHour)
        {
            return GetPVStatList(string.Format(" [category]='hour' AND [value]>='{0}' AND [value]<='{1}'", startHour, endHour));
        }

        /// <summary>
        /// 获得今天小时的PV统计列表
        /// </summary>
        /// <returns></returns>
        public static List<PVStatInfo> GetTodayHourPVStatList()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            return GetHourPVStatList(date + "00", date + "23");
        }

        /// <summary>
        /// 获得浏览器统计
        /// </summary>
        /// <returns></returns>
        public static List<PVStatInfo> GetBrowserStat()
        {
            return GetPVStatList(" [category]='browser'");
        }

        /// <summary>
        /// 获得操作系统统计
        /// </summary>
        /// <returns></returns>
        public static List<PVStatInfo> GetOSStat()
        {
            return GetPVStatList(" [category]='os'");
        }
    }
}
