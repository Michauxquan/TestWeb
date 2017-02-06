using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    /// <summary>
    /// 彩票
    /// </summary>
    public class MD_Lottery
    {
        private int lotteryid;
        public int Lotteryid
        {
            get { return lotteryid; }
            set { lotteryid = value; }
        }

        private int type;
        /// <summary>
        /// 类型（1代表北京28，2代表加拿大28）
        /// </summary>
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        private string expect;
        /// <summary>
        /// 开奖期号
        /// </summary>
        public string Expect
        {
            get { return expect; }
            set { expect = value.TrimEnd(); }
        }

        private string opencode;
        /// <summary>
        /// 开奖号码
        /// </summary>
        public string Opencode
        {
            get { return opencode; }
            set { opencode = value.TrimEnd(); }
        }

        private DateTime opentime;
        /// <summary>
        /// 开奖时间
        /// </summary>
        public DateTime Opentime
        {
            get { return opentime; }
            set { opentime = value; }
        }

        private string orderresult;
        /// <summary>
        /// 排序后号码
        /// </summary>
        public string Orderresult
        {
            get { return orderresult; }
            set { orderresult = value.TrimEnd(); }
        }

        private string first;
        /// <summary>
        ///计算第一次取的6个数字
        /// </summary>
        public string First
        {
            get { return first; }
            set { first = value.TrimEnd(); }
        }

        private string second;
        /// <summary>
        /// 第二次取的6个数字
        /// </summary>
        public string Second
        {
            get { return second; }
            set { second = value.TrimEnd(); }
        }

        private string three;
        /// <summary>
        /// 第三次取的6个数字
        /// </summary>
        public string Three
        {
            get { return three; }
            set { three = value.TrimEnd(); }
        }

        private string result;
        /// <summary>
        /// 结果：0+9+0=9
        /// </summary>
        public string Result
        {
            get { return result; }
            set { result = value.TrimEnd(); }
        }

        private string resultnum;
        /// <summary>
        /// 最终结果：9
        /// </summary>
        public string Resultnum
        {
            get { return resultnum; }
            set { resultnum = value.TrimEnd(); }
        }
        private string resulttype;
        /// <summary>
        /// 中奖类型(只统计 大，小，单，双)
        /// </summary>
        public string ResultType
        {
            get { return resulttype; }
            set { resulttype = value.TrimEnd(); }
        }

        private short status;
        /// <summary>
        /// 状态 0：投注中 1：封盘 2：已开奖
        /// </summary>
        public short Status
        {
            get { return status; }
            set { status = value; }
        }

        private DateTime addtime;
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }

    }
}
