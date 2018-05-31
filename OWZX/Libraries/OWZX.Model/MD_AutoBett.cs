using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    public class MD_AutoBett
    {
        private int autobtid;
        public int Autobtid
        {
            get { return autobtid; }
            set { autobtid = value; }
        }

        private int uid;
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        private int lotteryid;
        /// <summary>
        /// 彩票类型id
        /// </summary>
        public int LotteryId
        {
            get { return lotteryid; }
            set { lotteryid = value; }
        }
        private int selmodeid;
        /// <summary>
        /// 当前模式id
        /// </summary>
        public int SelModeId
        {
            get { return selmodeid; }
            set { selmodeid = value; }
        }

        private string startexpect;
        /// <summary>
        /// 开始期号
        /// </summary>
        public string StartExpect
        {
            get { return startexpect; }
            set { startexpect = value; }
        }

        private string maxbettnum;
        /// <summary>
        /// 结束期号
        /// </summary>
        public string MaxBettNum
        {
            get { return maxbettnum; }
            set { maxbettnum = value; }
        }

        private int mingold;
        /// <summary>
        /// 剩余最小乐豆数
        /// </summary>
        public int MinGold
        {
            get { return mingold; }
            set { mingold = value; }
        }

        private int autobettnum;
        /// <summary>
        /// 自动投注期数
        /// </summary>
        public int AutoBettNum
        {
            get { return autobettnum; }
            set { autobettnum = value; }
        }

        private bool isstart;
        /// <summary>
        /// 是否开始自动投注
        /// </summary>
        public bool IsStart
        {
            get { return isstart; }
            set { isstart = value; }
        }
        /// <summary>
        /// 设置的投注模式列表
        /// </summary>
        public string AllSelMode { get; set; }

        private DateTime addtime;
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }

        private DateTime updatetime;
        public DateTime Updatetime
        {
            get { return updatetime; }
            set { updatetime = value; }
        }

        private string updateuser;
        public string Updateuser
        {
            get { return updateuser; }
            set { updateuser = value; }
        }

    }
}
