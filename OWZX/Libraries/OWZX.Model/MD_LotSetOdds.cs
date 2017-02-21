using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    /// <summary>
    /// 设置彩票赔率表
    /// </summary>
    public class MD_LotSetOdds:MD_LotterySet
    {
        private int setid;
        public int Setid
        {
            get { return setid; }
            set { setid = value; }
        }

        //private int lotterytype;
        //public  int Lotterytype
        //{
        //    get { return lotterytype; }
        //    set { lotterytype = value; }
        //}

        //private int bttypeid;
        //public int Bttypeid
        //{
        //    get { return bttypeid; }
        //    set { bttypeid = value; }
        //}

        //private  string odds;
        //public  string Odds
        //{
        //    get { return odds; }
        //    set { odds = value; }
        //}


        //private DateTime addtime;
        //public DateTime Addtime
        //{
        //    get { return addtime; }
        //    set { addtime = value; }
        //}

    }
}
