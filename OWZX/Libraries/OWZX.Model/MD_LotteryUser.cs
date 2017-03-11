using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
   public class MD_LotteryUser:MD_Lottery
    {
       /// <summary>
       /// 每期投注金额
       /// </summary>
       public Int64 TotalBett { get; set; }
       /// <summary>
        /// 每期中奖人数
       /// </summary>
        public int WinPerson { get; set; }
       /// <summary>
        /// 每期投注人数
       /// </summary>
        public int BettPerson { get; set; }
       /// <summary>
       /// 当前用户中奖金额
       /// </summary>
        public decimal LuckResult { get; set; }
        /// <summary>
        /// 当前用户投注金额
        /// </summary>
        public Int64 Money { get; set; }
    }
}
