using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    /// <summary>
    /// 用户投注模式
    /// </summary>
    public class MD_BettMode
    {
        private int modeid;
        /// <summary>
        /// id
        /// </summary>
        public int Modeid
        {
            get { return modeid; }
            set { modeid = value; }
        }

        private string name;
        /// <summary>
        ///模式名称 
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int uid;

        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        private int lotterytype;
        /// <summary>
        /// 彩票类型
        /// </summary>
        public int LotteryType { get; set; }
        private string bettnum;
        /// <summary>
        ///号码集合;分割 
        /// </summary>
        public string Bettnum
        {
            get { return bettnum; }
            set { bettnum = value; }
        }

        private string bettinfo;
        /// <summary>
        ///号码:乐豆数量  ;分割 
        /// </summary>
        public string Bettinfo
        {
            get { return bettinfo; }
            set { bettinfo = value; }
        }

        private int betttotal;
        /// <summary>
        ///乐豆总数
        /// </summary>
        public int Betttotal
        {
            get { return betttotal; }
            set { betttotal = value; }
        }

        private int wintype;
        /// <summary>
        ///赢采用模式
        /// </summary>
        public int Wintype
        {
            get { return wintype; }
            set { wintype = value; }
        }

        private int losstype;
        /// <summary>
        /// 输采用模式
        /// </summary>
        public int Losstype
        {
            get { return losstype; }
            set { losstype = value; }
        }

        private DateTime addtime;
        /// <summary>
        /// 
        /// </summary>
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }

        private DateTime updatetime;
        /// <summary>
        /// 
        /// </summary>
        public DateTime Updatetime
        {
            get { return updatetime; }
            set { updatetime = value; }
        }

        private int updateuid;
        /// <summary>
        /// 
        /// </summary>
        public int Updateuid
        {
            get { return updateuid; }
            set { updateuid = value; }
        }

    }
}
