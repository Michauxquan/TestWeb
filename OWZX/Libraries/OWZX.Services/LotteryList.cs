using OWZX.Core;
using OWZX.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Services
{
   public class LotteryList
    {
        #region 竞猜数据
        /// <summary>
        /// 根据类型获取竞猜首页数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
       public static MD_LotteryList GetLotteryByType(int type, int pageindex, int pagesize, int uid = -1)
        {
            DataSet ds= OWZX.Data.LotteryList.GetLotteryByType(type, pageindex, pagesize, uid);
            MD_LotteryList list = ModelConvertHelper <MD_LotteryList>.DataTableToModel(ds.Tables[1]);
            list.Prev_Lottery = ModelConvertHelper<MD_Lottery>.DataTableToModel(ds.Tables[0]);
            list.LotteryList = (List<MD_LotteryUser>)ModelConvertHelper<MD_LotteryUser>.ConvertToModel(ds.Tables[2]);

            return list;
        }

       /// <summary>
        /// 是否已开奖
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expect"></param>
        /// <returns></returns>
        public static bool ExistsLotteryOpen(string type, string expect)
        {
            return OWZX.Data.LotteryList.ExistsLotteryOpen(type, expect);
        }
       /// <summary>
       /// 获取投注记录
       /// </summary>
       /// <param name="type"></param>
       /// <param name="uid"></param>
       /// <returns></returns>
        public static DataTable GetUserBett(int type, int uid)
        {
            return OWZX.Data.LotteryList.GetUserBett(type, uid);
        }
        #endregion
    }
}
