using OWZX.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Data
{
    public class Lottery
    {
        #region 彩票

        /// <summary>
        /// 获取最新彩票记录
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DataTable LastLottery(string type)
        {
            return OWZX.Core.BSPData.RDBS.LastLottery(type);
        }

        /// <summary>
        /// 添加新的竞猜记录
        /// </summary>
        /// <param name="lottery"></param>
        /// <returns></returns>
        public static bool AddBJRecord(MD_Lottery lottery)
        {
            return OWZX.Core.BSPData.RDBS.AddBJRecord(lottery);
        }

        /// <summary>
        /// 添加新的竞猜记录
        /// </summary>
        /// <param name="lottery"></param>
        /// <returns></returns>
        public static bool AddCanadaRecord(MD_Lottery lottery)
        {
            return OWZX.Core.BSPData.RDBS.AddCanadaRecord(lottery);
        }
        /// <summary>
        /// 添加彩票记录 (记录彩票开奖时间和期号)
        /// </summary>
        /// <param name="lot"></param>
        /// <returns></returns>
        public static  string AddLottery(MD_Lottery lot)
        {
            return OWZX.Core.BSPData.RDBS.AddLottery(lot);
        }

        /// <summary>
        /// 添加彩票记录 (上期结束，添加新的记录)
        /// </summary>
        /// <param name="type">北京10 加拿大11</param>
        /// <param name="starttime">彩票开始时间</param>
        /// <param name="endtime">彩票截止时间</param>
        /// <returns></returns>
        public static string AddLottery(int type, string starttime, string endtime)
        {
            return OWZX.Core.BSPData.RDBS.AddLottery(type,starttime, endtime);
        }
        /// <summary>
        /// 更新彩票记录 (更新开奖信息)
        /// </summary>
        /// <param name="lot"></param>
        /// <returns></returns>
        public static  string UpdateLottery(MD_Lottery lot)
        {
            return OWZX.Core.BSPData.RDBS.UpdateLottery(lot);
        }
        /// <summary>
        /// 更新竞猜记录为 封盘状态
        /// </summary>
        /// <returns></returns>
        public static string UpdateLotteryStatus()
        {
            return OWZX.Core.BSPData.RDBS.UpdateLotteryStatus();
        }

        /// <summary>
        /// 删除彩票记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static  string DeleteLottery(string id)
        {
            return OWZX.Core.BSPData.RDBS.DeleteLottery(id);
        }


        /// <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static  DataTable GetLotteryList(int pageNumber, int pageSize, string condition = "",string orderby="")
        {
            return OWZX.Core.BSPData.RDBS.GetLotteryList(pageNumber, pageSize, condition,orderby);
        }

        /// <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="type">彩票类型</param>
        /// <returns></returns>
        public static DataTable LastLotteryList(int type)
        {
            return OWZX.Core.BSPData.RDBS.LastLotteryList(type);
        }

        /// <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">有where 条件需要and</param>
        /// <returns></returns>
        public static DataTable GetBJ28LotteryList(int pageNumber, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetBJ28LotteryList(pageNumber, pageSize, condition);
        }


        /// <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">有where 条件需要and</param>
        /// <returns></returns>
        public static DataTable GetCanada28LotteryList(int pageNumber, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetCanada28LotteryList(pageNumber, pageSize, condition);
        }

        /// <summary>
        /// 获取彩票走势图
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="type">彩票类型id</param>
        /// <returns></returns>
        public static DataTable LotteryTrend(int pageNumber, int pageSize, string type)
        {
            return OWZX.Core.BSPData.RDBS.LotteryTrend(pageNumber, pageSize, type);
        }

        /// <summary>
        ///是否存在北京28彩票记录
        /// </summary>
        /// <param name="condition">有where 条件需要and</param>
        /// <returns></returns>
        public static bool ExistsBJ28(string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.ExistsBJ28(condition);
        }


        /// <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="condition">有where 条件需要and</param>
        /// <returns></returns>
        public static bool ExistsCanada28(string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.ExistsCanada28(condition);
        }
        /// <summary>
        /// bj28最新竞猜记录是否已经超时
        /// </summary>
        /// <returns></returns>
        public static bool ExistsBjTimeOut()
        {
            return OWZX.Core.BSPData.RDBS.ExistsBjTimeOut();
        }
        /// <summary>
        /// canada28最新竞猜记录是否已经超时
        /// </summary>
        /// <returns></returns>
        public static bool ExistsCanTimeOut()
        {
            return OWZX.Core.BSPData.RDBS.ExistsCanTimeOut();
        }

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
        public static string ValidateBett(string account, string expect, string money, string room, string vip, int bttypeid)
        {
            return OWZX.Core.BSPData.RDBS.ValidateBett(account, expect, money, room,vip,bttypeid);
        }
        #endregion

        #region 投注
        /// <summary>
        /// 添加投注记录
        /// </summary>
        /// <param name="bett"></param>
        /// <returns></returns>
        public static string AddNewBett(MD_Bett bett)
        {
            return OWZX.Core.BSPData.RDBS.AddNewBett(bett);
        }
        /// <summary>
        /// 添加投注记录
        /// </summary>
        /// <param name="bett"></param>
        /// <returns></returns>
        public static  string AddBett(MD_Bett bett)
        {
            return OWZX.Core.BSPData.RDBS.AddBett(bett);
        }
        /// <summary>
        /// 更新投注记录 
        /// </summary>
        /// <param name="draw"></param>
        /// <returns></returns>
        public static string UpdateBett(MD_Bett draw)
        {
            return OWZX.Core.BSPData.RDBS.UpdateBett(draw);
        }

        /// <summary>
        /// 删除投注记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static  string DeleteBett(string id)
        {
            return OWZX.Core.BSPData.RDBS.DeleteBett(id);
        }

        /// <summary>
        ///  获取投注记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static  DataTable GetBettList(int pageNumber, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetBettList(pageNumber, pageSize, condition);
        }
        /// <summary>
        /// 获取当前竞猜的投注总注数
        /// </summary>
        /// <param name="expect"></param>
        /// <returns></returns>
        public static DataTable GetBettTotal(string expect)
        {
            return OWZX.Core.BSPData.RDBS.GetBettTotal(expect);
        }

        /// <summary>
        /// 验证投注操作是否异常
        /// </summary>
        /// <param name="expect">投注期号</param>
        /// <param name="bttypeid">投注类型id</param>
        /// <param name="money">投注金额</param>
        /// <param name="type">房间类型</param>
        /// <returns></returns>
        public static string ValidateBetMoney(string expect, int bttypeid, int money,string type)
        {
            return OWZX.Core.BSPData.RDBS.ValidateBetMoney(expect, bttypeid, money, type);
        }
        #endregion

        #region 彩票投注赔率
        /// <summary>
        /// 赔率说明
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DataTable SetRemark(string type)
        {
            return OWZX.Core.BSPData.RDBS.SetRemark(type);
        }
        /// <summary>
        /// 添加投注赔率
        /// </summary>
        /// <param name="lotset"></param>
        /// <returns></returns>
        public static  string AddLotterySet(MD_LotterySet lotset)
        {
            return OWZX.Core.BSPData.RDBS.AddLotterySet(lotset);
        }
        /// <summary>
        /// 更新投注赔率
        /// </summary>
        /// <param name="lotset"></param>
        /// <returns></returns>
        public static  string UpdateLotterySet(MD_LotterySet lotset)
        {
            return OWZX.Core.BSPData.RDBS.UpdateLotterySet(lotset);
        }

        /// <summary>
        /// 删除投注赔率
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static  string DeleteLotterySet(string id)
        {
            return OWZX.Core.BSPData.RDBS.DeleteLotterySet(id);
        }

        /// <summary>
        ///  获取投注赔率(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static  DataTable GetLotterySetList(int pageNumber, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetLotterySetList(pageNumber, pageSize, condition);
        }

        /// <summary>
        ///  获取投注赔率
        /// </summary>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static DataSet GetLotterySetList(string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetLotterySetList(condition);
        }
        #endregion

        #region 记录等待计算用户奖金彩票
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="bett"></param>
        /// <returns></returns>
        public static string AddWaitPay(MD_WaitPayBonus pay)
        {
            return OWZX.Core.BSPData.RDBS.AddWaitPay(pay);
        }
        /// <summary>
        /// 更新记录 
        /// </summary>
        /// <param name="draw"></param>
        /// <returns></returns>
        public static string UpdateWaitPay(MD_WaitPayBonus pay)
        {
            return OWZX.Core.BSPData.RDBS.UpdateWaitPay(pay);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteWaitPay(string id)
        {
            return OWZX.Core.BSPData.RDBS.DeleteWaitPay(id);
        }

        /// <summary>
        ///  获取记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static DataTable GetWaitPayList(int pageNumber, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetWaitPayList(pageNumber, pageSize, condition);
        }

        #endregion

        #region 发放奖金计算竞猜投注信息
        /// <summary>
        /// 计算竞猜投注结果
        /// </summary>
        /// <returns></returns>
        public static string ExcuteBettResult()
        {
            return OWZX.Core.BSPData.RDBS.ExcuteBettResult();
        }
        /// <summary>
        /// 计算北京28竞猜投注结果
        /// </summary>
        /// <param name="lotterynum"></param>
        /// <returns></returns>
        public static  string ExcuteBJBettResult(string lotterynum)
        {
            return OWZX.Core.BSPData.RDBS.ExcuteBJBettResult(lotterynum);
        }
        /// <summary>
        /// 计算加拿大28竞猜投注结果
        /// </summary>
        /// <param name="lotterynum"></param>
        /// <returns></returns>
        public static  string ExcuteCanadaBettResult(string lotterynum)
        {
            return OWZX.Core.BSPData.RDBS.ExcuteCanadaBettResult(lotterynum);
        }
        #endregion

        #region 房间信息

        /// <summary>
        /// 添加投注记录
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static string AddRoom(MD_LotteryRoom room)
        {
            return OWZX.Core.BSPData.RDBS.AddRoom(room);
        }
        /// <summary>
        /// 更新投注记录 
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static string UpdateRoom(MD_LotteryRoom room)
        {
            return OWZX.Core.BSPData.RDBS.UpdateRoom(room);
        }

        /// <summary>
        /// 删除投注记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteRoom(string id)
        {
            return OWZX.Core.BSPData.RDBS.DeleteRoom(id);
        }

        /// <summary>
        ///  获取投注记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where 主表 a  彩票类型表 b 房间类型表c</param>
        /// <returns></returns>
        public static DataTable GetRoomList(int pageNumber, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetRoomList(pageNumber,pageSize,condition);
        }
        #endregion

        #region 回水规则

        /// <summary>
        /// 添加回水规则
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static string AddRateRule(MD_BackRate rate)
        {
            return OWZX.Core.BSPData.RDBS.AddRateRule(rate);
        }
        /// <summary>
        /// 更新回水规则
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static string UpdateRateRule(MD_BackRate rate)
        {
            return OWZX.Core.BSPData.RDBS.UpdateRateRule(rate);
        }

        /// <summary>
        /// 删除回水规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteRateRule(string id)
        {
            return OWZX.Core.BSPData.RDBS.DeleteRateRule(id);
        }

        /// <summary>
        ///  获取回水规则(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where 主表 a  房间类型表 b </param>
        /// <returns></returns>
        public static DataTable GetRateRuleList(int pageNumber, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetRateRuleList(pageNumber, pageSize, condition);
        }
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
        public static DataTable GetProfitList(string type, int pageSize, int pageNumber, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetProfitList(type, pageSize, pageNumber, condition);
        }
        /// <summary>
        /// 盈利报表 彩票类型不参数分组,包含回水
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="pageNumber"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable GetProfitListNoLottery(string type, int pageSize, int pageNumber, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetProfitListNoLottery(type, pageSize, pageNumber, condition);
        }
        #endregion

        #region 急速28 

        public static DataTable GetAllMoneyByLotteryNum(string lotterynum = "", int type = 47)
        {
            return OWZX.Core.BSPData.RDBS.GetAllMoneyByLotteryNum(lotterynum, type);
        }
        public static DataTable GetLotteryOpenSetList(int type = 47)
        {
            return OWZX.Core.BSPData.RDBS.GetLotteryOpenSetList(type);
        }
        public static string UpdateSetStaus(int lotteryid, int status, string result, string lotterynum = "")
        {
            return OWZX.Core.BSPData.RDBS.UpdateSetStaus(lotteryid, status, result, lotterynum);
        }
        public static string UpdateSetDetailStaus(int lotteryid, int detailid, int isdefault, string result, string lotterynum)
        {
            return OWZX.Core.BSPData.RDBS.UpdateSetDetailStaus(lotteryid, detailid, isdefault, result, lotterynum);
        }

        #endregion
    }
}
