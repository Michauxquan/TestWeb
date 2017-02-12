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
       public static DataSet GetLotteryByType(string type, string pageindex, string pagesize, int uid = -1)
       {
           return OWZX.Core.BSPData.RDBS.GetLotteryByType(type,pageindex,pagesize,uid);
       }
        #endregion
    }
}
