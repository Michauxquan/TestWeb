﻿using OWZX.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Data
{
   public class LotteryList
    {
        #region 竞猜数据
        /// <summary>
        /// 根据类型获取竞猜首页数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
       public static DataSet GetLotteryByType(int type, int pageindex, int pagesize, int uid = -1)
       {
           return OWZX.Core.BSPData.RDBS.GetLotteryByType(type,pageindex,pagesize,uid);
       }

       /// <summary>
        /// 是否已开奖
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expect"></param>
        /// <returns></returns>
       public static bool ExistsLotteryOpen(string type, string expect)
       {
           return OWZX.Core.BSPData.RDBS.ExistsLotteryOpen(type, expect);
       }

       /// <summary>
       /// 获取投注记录
       /// </summary>
       /// <param name="type"></param>
       /// <param name="uid"></param>
       /// <returns></returns>
       public static DataTable GetUserBett(int type, int uid, int pageindex, int pagesize)
       {
           return OWZX.Core.BSPData.RDBS.GetUserBett(type, uid,pageindex,pagesize);
       }

       /// <summary>
        /// 获取最新彩票记录
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
       public static DataTable NewestLottery(string type)
       {
           return OWZX.Core.BSPData.RDBS.NewestLottery(type);
       }
        #endregion

       #region 赔率
       /// <summary>
       /// 获取赔率信息
       /// </summary>
       /// <param name="type"></param>
       /// <returns></returns>
       public static DataSet GetLotterySet(int type)
       {
           return OWZX.Core.BSPData.RDBS.GetLotterySet(type);
       }

       /// <summary>
       /// 添加彩票赔率
       /// </summary>
       /// <param name="chag"></param>
       /// <returns></returns>
       public static string AddLotSet(MD_LotSetOdds mode)
       {
           return OWZX.Core.BSPData.RDBS.AddLotSet(mode);
       }

       /// <summary>
       /// 更新彩票赔率
       /// </summary>
       /// <param name="msg"></param>
       /// <returns></returns>
       public static string UpdateLotSet(MD_LotSetOdds mode)
       {
           return OWZX.Core.BSPData.RDBS.UpdateLotSet(mode);
       }

       /// <summary>
       /// 删除彩票赔率
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       public static string DelLotSet(int setid)
       {
           return OWZX.Core.BSPData.RDBS.DelLotSet(setid);
       }

       /// <summary>
       ///获取彩票赔率
       /// </summary>
       /// <param name="condition">没有where</param>
       /// <returns></returns>
       public static DataSet GetLotSetList(string type, string condition = "")
       {
           return OWZX.Core.BSPData.RDBS.GetLotSetList(type, condition);
       }
       #endregion
    }
}
