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

        #region 自动投注
        /// <summary>
        /// 添加自动投注
        /// </summary>
        /// <param name="chag"></param>
        /// <returns></returns>
        string AddAutoBett(MD_AutoBett mode);

        /// <summary>
        /// 更新模式信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        string UpdateAutoBett(MD_AutoBett mode);

        /// <summary>
        /// 停止自动投注
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string StopAutoBett(int uid, int lotterytype);


        /// <summary>
        ///获取自动投注
        /// </summary>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        DataTable GetAutoBett(int pageindex, int pagesize, string condition = "");

        /// <summary>
        /// 获取用户自动投注信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="lotterytype"></param>
        /// <returns></returns>
        DataSet GetUserAtBett(int uid, int lotterytype);
        #endregion
    }
}
