using OWZX.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Core
{
    // 彩票
    public partial interface IRDBSStrategy
    {
        #region 彩票

        /// <summary>
        /// 获取最新彩票记录
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DataTable LastLottery(string type);
        /// <summary>
        /// 添加新的竞猜记录
        /// </summary>
        /// <param name="lottery"></param>
        /// <returns></returns>
        bool AddBJRecord(MD_Lottery lottery);

        /// <summary>
        /// 添加新的竞猜记录
        /// </summary>
        /// <param name="lottery"></param>
        /// <returns></returns>
        bool AddCanadaRecord(MD_Lottery lottery);


        /// <summary>
        /// 添加彩票记录 (记录彩票开奖时间和期号)
        /// </summary>
        /// <param name="lot"></param>
        /// <returns></returns>
        string AddLottery(MD_Lottery lot);


        /// <summary>
        /// 添加彩票记录 (上期结束，添加新的记录)
        /// </summary>
        /// <param name="type">北京10 加拿大11</param>
        /// <param name="starttime">彩票开始时间</param>
        /// <param name="endtime">彩票截止时间</param>
        /// <returns></returns>
        string AddLottery(int type, string starttime, string endtime);
        /// <summary>
        /// 更新彩票记录 (更新开奖信息)
        /// </summary>
        /// <param name="lot"></param>
        /// <returns></returns>
        string UpdateLottery(MD_Lottery lot);

        /// <summary>
        /// 更新竞猜记录为 封盘状态
        /// </summary>
        /// <returns></returns>
        string UpdateLotteryStatus();

        /// <summary>
        /// 删除彩票记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string DeleteLottery(string id);

        /// <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        DataTable GetLotteryList(int pageNumber, int pageSize, string condition = "");

        /// <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="type">彩票类型</param>
        /// <returns></returns>
        DataTable LastLotteryList(int type);

        /// <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">有where 条件需要and</param>
        /// <returns></returns>
        DataTable GetBJ28LotteryList(int pageNumber, int pageSize, string condition = "");


        /// <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">有where 条件需要and</param>
        /// <returns></returns>
        DataTable GetCanada28LotteryList(int pageNumber, int pageSize, string condition = "");

        /// <summary>
        /// 获取彩票走势图
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="type">彩票类型id</param>
        /// <returns></returns>
        DataTable LotteryTrend(int pageNumber, int pageSize, string type);

        /// <summary>
        ///是否存在北京28彩票记录
        /// </summary>
        /// <param name="condition">有where 条件需要and</param>
        /// <returns></returns>
        bool ExistsBJ28(string condition = "");

        /// <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="condition">有where 条件需要and</param>
        /// <returns></returns>
        bool ExistsCanada28(string condition = "");
        /// <summary>
        /// bj28最新竞猜记录是否已经超时
        /// </summary>
        /// <returns></returns>
        bool ExistsBjTimeOut();
        /// <summary>
        /// canada28最新竞猜记录是否已经超时
        /// </summary>
        /// <returns></returns>
        bool ExistsCanTimeOut();

        /// <summary>
        /// 验证投注信息是否正确
        /// </summary>
        /// <param name="account"></param>
        /// <param name="expect"></param>
        /// <param name="money"></param>
        /// <param name="room"></param>
        /// <param name="vip"></param>
        /// <param name="bttypeid"></param>
        /// <returns></returns>
        string ValidateBett(string account, string expect, string money, string room, string vip, int bttypeid);
        #endregion

        #region 投注
        /// <summary>
        /// 添加投注记录
        /// </summary>
        /// <param name="bett"></param>
        /// <returns></returns>
        string AddBett(MD_Bett bett);
        /// <summary>
        /// 更新投注记录 
        /// </summary>
        /// <param name="draw"></param>
        /// <returns></returns>
        string UpdateBett(MD_Bett bett);

        /// <summary>
        /// 删除投注记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string DeleteBett(string id);

        /// <summary>
        ///  获取投注记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        DataTable GetBettList(int pageNumber, int pageSize, string condition = "");
        /// <summary>
        /// 获取当前竞猜的投注总注数
        /// </summary>
        /// <param name="expect"></param>
        /// <returns></returns>
        DataTable GetBettTotal(string expect);

        /// <summary>
        /// 验证投注操作是否异常
        /// </summary>
        /// <param name="expect">投注期号</param>
        /// <param name="bttypeid">投注类型id</param>
        /// <param name="money">投注金额</param>
        /// <param name="type">房间类型</param>
        /// <returns></returns>
        string ValidateBetMoney(string expect, int bttypeid, int money, string type);

        #endregion

        #region 彩票投注赔率
        /// <summary>
        /// 赔率说明
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DataTable SetRemark(string type);
        /// <summary>
        /// 添加投注赔率
        /// </summary>
        /// <param name="lotset"></param>
        /// <returns></returns>
        string AddLotterySet(MD_LotterySet lotset);
        /// <summary>
        /// 更新投注赔率
        /// </summary>
        /// <param name="lotset"></param>
        /// <returns></returns>
        string UpdateLotterySet(MD_LotterySet lotset);

        /// <summary>
        /// 删除投注赔率
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string DeleteLotterySet(string id);

        /// <summary>
        ///  获取投注赔率(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        DataTable GetLotterySetList(int pageNumber, int pageSize, string condition = "");

        /// <summary>
        ///  获取投注赔率
        /// </summary>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        DataSet GetLotterySetList(string condition = "");
        #endregion

        #region 记录等待计算用户奖金彩票
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="bett"></param>
        /// <returns></returns>
        string AddWaitPay(MD_WaitPayBonus pay);
        /// <summary>
        /// 更新记录 
        /// </summary>
        /// <param name="draw"></param>
        /// <returns></returns>
        string UpdateWaitPay(MD_WaitPayBonus pay);

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string DeleteWaitPay(string id);

        /// <summary>
        ///  获取记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        DataTable GetWaitPayList(int pageNumber, int pageSize, string condition = "");

        #endregion

        #region 投注结果
        /// <summary>
        /// 计算竞猜投注结果
        /// </summary>
        /// <returns></returns>
        string ExcuteBettResult();
        /// <summary>
        /// 计算北京28竞猜投注结果
        /// </summary>
        /// <param name="lotterynum"></param>
        /// <returns></returns>
        string ExcuteBJBettResult(string lotterynum);
        /// <summary>
        /// 计算加拿大28竞猜投注结果
        /// </summary>
        /// <param name="lotterynum"></param>
        /// <returns></returns>
        string ExcuteCanadaBettResult(string lotterynum);

        #endregion

        #region 房间信息

        /// <summary>
        /// 添加投注记录
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        string AddRoom(MD_LotteryRoom room);
        /// <summary>
        /// 更新投注记录 
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        string UpdateRoom(MD_LotteryRoom room);

        /// <summary>
        /// 删除投注记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string DeleteRoom(string id);

        /// <summary>
        ///  获取投注记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where 主表 a  彩票类型表 b 房间类型表c</param>
        /// <returns></returns>
        DataTable GetRoomList(int pageNumber, int pageSize, string condition = "");
        #endregion

        #region 回水规则

        /// <summary>
        /// 添加回水规则
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        string AddRateRule(MD_BackRate rate);
        /// <summary>
        /// 更新回水规则
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        string UpdateRateRule(MD_BackRate rate);

        /// <summary>
        /// 删除回水规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string DeleteRateRule(string id);

        /// <summary>
        ///  获取回水规则(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where 主表 a  房间类型表 b </param>
        /// <returns></returns>
        DataTable GetRateRuleList(int pageNumber, int pageSize, string condition = "");
        #endregion

        #region 报表
        /// <summary>
        /// 盈利报表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="pageNumber"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataTable GetProfitList(string type, int pageSize, int pageNumber, string condition = "");

        /// <summary>
        /// 盈利报表 彩票类型不参数分组,包含回水
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="pageNumber"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataTable GetProfitListNoLottery(string type, int pageSize, int pageNumber, string condition = "");
        #endregion

    }
}
