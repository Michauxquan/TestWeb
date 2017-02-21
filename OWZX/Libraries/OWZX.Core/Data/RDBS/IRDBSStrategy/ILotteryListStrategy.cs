using OWZX.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Core
{
    /// <summary>
    /// 新的竞猜数据接口
    /// </summary>
    public partial interface IRDBSStrategy
    {
        #region 竞猜数据
        /// <summary>
        /// 根据类型获取竞猜首页数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DataSet GetLotteryByType(int type, int pageindex, int pagesize, int uid = -1);

        /// <summary>
        /// 是否已开奖
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expect"></param>
        /// <returns></returns>
        bool ExistsLotteryOpen(string type, string expect);

        /// <summary>
        /// 获取投注记录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        DataTable GetUserBett(int type, int uid, int pageindex, int pagesize);

        /// <summary>
        /// 获取最新彩票记录
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DataTable NewestLottery(string type);
        #endregion

        #region 赔率
        /// <summary>
        /// 获取赔率信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DataSet GetLotterySet(int type);

        /// <summary>
        /// 添加彩票赔率
        /// </summary>
        /// <param name="chag"></param>
        /// <returns></returns>
        string AddLotSet(MD_LotSetOdds mode);

        /// <summary>
        /// 更新彩票赔率
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        string UpdateLotSet(MD_LotSetOdds mode);

        /// <summary>
        /// 删除彩票赔率
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string DelLotSet(int setid);

        /// <summary>
        ///获取彩票赔率
        /// </summary>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        DataSet GetLotSetList(string type, string condition = "");
        #endregion
    }
}
