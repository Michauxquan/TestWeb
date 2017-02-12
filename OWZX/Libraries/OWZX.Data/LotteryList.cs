﻿using System;
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
       public static DataTable GetUserBett(int type, int uid)
       {
           return OWZX.Core.BSPData.RDBS.GetUserBett(type, uid);
       }
        #endregion
    }
}
