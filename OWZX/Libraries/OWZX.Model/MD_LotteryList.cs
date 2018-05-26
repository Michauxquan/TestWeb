using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    public class MD_LotteryList
    {
        /// <summary>
        /// 最新竞猜期号
        /// </summary>
        public string LastNumber { get; set; }
        /// <summary>
        /// 剩余时间(S)
        /// </summary>
        public int RemainS { get; set; }
        /// <summary>
        /// 今日盈亏
        /// </summary>
        public decimal TdProf { get; set; }
        /// <summary>
        /// 今日参与期数
        /// </summary>
        public int TdBettNum { get; set; }
        /// <summary>
        /// 胜率
        /// </summary>
        public decimal WinPercent { get; set; } 
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 上一期结果
        /// </summary>
        public MD_Lottery Prev_Lottery { get; set; }
        /// <summary>
        /// 往期开奖数据
        /// </summary>
        public List<MD_LotteryUser> LotteryList { get; set; }
    }
    /// <summary>
    /// 彩票类型枚举
    /// </summary>
    public enum LotType
    {
        /// <summary>
        /// 蛋蛋28
        /// </summary>
        dd28 = 1,
        /// <summary>
        /// 加拿大28
        /// </summary>
        cakeno28 = 2,
        /// <summary>
        /// 急速28
        /// </summary>
        js28 = 3,
        /// <summary>
        /// 蛋蛋36
        /// </summary>
        dd36 = 4,
        /// <summary>
        /// 加拿大36
        /// </summary>
        cakeno36 = 5,
        /// <summary>
        /// 韩国28
        /// </summary>
        hg28 = 6,
        /// <summary>
        /// PK冠军
        /// </summary>
        pkgj = 7,
        /// <summary>
        /// PK冠亚军
        /// </summary>
        pkgyj = 8,
        /// <summary>
        /// 蛋蛋龙虎豹
        /// </summary>
        ddlhb = 9,
        /// <summary>
        /// 蛋蛋龙虎豹
        /// </summary>
        js10 = 10,
        /// <summary>
        /// 蛋蛋龙虎豹
        /// </summary>
        js11 = 11,
        /// <summary>
        /// 蛋蛋龙虎豹
        /// </summary>
        js16 = 12,
        /// <summary>
        /// 六合彩
        /// </summary>
        hk6 = 13,
        /// <summary>
        /// 蛋蛋28浮动
        /// </summary>
        dd28fd = 14,
        /// <summary>
        /// 加拿大28浮动
        /// </summary>
        cakeno28fd = 15,
        /// <summary>
        /// 北京28
        /// </summary>
        bj28 = 16,
        /// <summary>
        /// 北京36
        /// </summary>
        bj36 = 17,
        /// <summary>
        /// 广西快3
        /// </summary>
        gxk3 = 18,
        /// <summary>
        /// 广西快乐十分
        /// </summary>
        gxklsf = 19,
        /// <summary>
        /// 湖南快乐十分
        /// </summary>
        hnklsf = 20,
        /// <summary>
        /// 江苏骰宝
        /// </summary>
        jsgb = 21,
        /// <summary>
        /// 江西时时彩
        /// </summary>
        jxssc = 22,
        /// <summary>
        /// 天津时时彩
        /// </summary>
        tjssc = 23,
        /// <summary>
        /// 新疆时时彩
        /// </summary>
        xjssc = 24,
        /// <summary>
        /// 重庆时时彩
        /// </summary>
        cqssc = 25,
        /// <summary>
        /// 重庆幸运农场
        /// </summary>
        cqxync = 26 ,
        /// <summary>
        /// PK赛车
        /// </summary>
        pksc = 27,
        /// <summary>
        /// PK10
        /// </summary>
        pk22 = 28,
        /// <summary>
        /// PK22
        /// </summary>
         pk10= 29,
        /// <summary>
        /// PK龙虎
        /// </summary>
        pklh = 30,
    }
    /// <summary>
    /// 投注类型
    /// </summary>
    public enum LotterySetType
    {
        /// <summary>
        /// 大小单双 龙虎豹
        /// </summary>
        lhb = 12,
        /// <summary>
        /// 猜数字
        /// </summary>
        number = 13,
        /// <summary>
        /// 顺对杂
        /// </summary>
        sdz = 15
    }
}
