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
            DataSet ds = OWZX.Data.LotteryList.GetLotteryByType(type, pageindex, pagesize, uid);
            MD_LotteryList list = ModelConvertHelper<MD_LotteryList>.DataTableToModel(ds.Tables[1]);
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
        public static DataTable GetUserBett(int type, int uid, int pageindex, int pagesize)
        {
            return OWZX.Data.LotteryList.GetUserBett(type, uid, pageindex, pagesize);
        }
        /// <summary>
        /// 获取最新彩票记录
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DataTable NewestLottery(string type)
        {
            return OWZX.Data.LotteryList.NewestLottery(type);
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
            return OWZX.Data.LotteryList.GetLotterySet(type);
        }

        /// <summary>
        /// 添加彩票赔率
        /// </summary>
        /// <param name="chag"></param>
        /// <returns></returns>
        public static string AddLotSet(MD_LotSetOdds mode)
        {
            return OWZX.Data.LotteryList.AddLotSet(mode);
        }

        /// <summary>
        /// 更新彩票赔率
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string UpdateLotSet(MD_LotSetOdds mode)
        {
            return OWZX.Data.LotteryList.UpdateLotSet(mode);
        }

        /// <summary>
        /// 删除彩票赔率
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DelLotSet(int setid)
        {
            return OWZX.Data.LotteryList.DelLotSet(setid);
        }

        /// <summary>
        ///获取彩票赔率
        /// </summary>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static DataSet GetLotSetList(string type, string condition = "")
        {
            return OWZX.Data.LotteryList.GetLotSetList(type, condition);
        }

        #endregion

        #region 自动投注
        /// <summary>
        /// 添加自动投注
        /// </summary>
        /// <param name="chag"></param>
        /// <returns></returns>
        public static string AddAutoBett(MD_AutoBett mode)
        {
            return OWZX.Data.LotteryList.AddAutoBett(mode);
        }

        /// <summary>
        /// 更新模式信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string UpdateAutoBett(MD_AutoBett mode)
        {
            return OWZX.Data.LotteryList.UpdateAutoBett(mode);
        }

        /// <summary>
        /// 停止自动投注
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string StopAutoBett(int uid, int lotterytype)
        {
            return OWZX.Data.LotteryList.StopAutoBett(uid, lotterytype);
        }

        /// <summary>
        ///获取自动投注
        /// </summary>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static DataTable GetAutoBett(string condition = "")
        {
            return OWZX.Data.LotteryList.GetAutoBett(condition);
        }

        #endregion
    }
}
