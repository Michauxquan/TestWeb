using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    /// <summary>
    /// 用户投注结果
    /// </summary>
    public class MD_BettProfitLoss
    {
        private int btresid;
        public int Btresid
        {
            get { return btresid; }
            set { btresid = value; }
        }

        private string account;
        /// <summary>
        /// 用户账号
        /// </summary>
        public string Account
        {
            get { return account; }
            set { account = value.TrimEnd(); }
        }

        private int lotteryid;
        /// <summary>
        /// 投注彩票id
        /// </summary>
        public int Lotteryid
        {
            get { return lotteryid; }
            set { lotteryid = value; }
        }
        private string lotterynum;
        /// <summary>
        /// 投注彩票期号
        /// </summary>
        public string LotteryNum
        {
            get { return lotterynum; }
            set { lotterynum = value.TrimEnd(); }
        }
        private decimal luckresult;
        /// <summary>
        ///获得奖金 
        /// </summary>
        public decimal Luckresult
        {
            get { return luckresult; }
            set { luckresult = value; }
        }


        private DateTime addtime;
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }

    }
}
