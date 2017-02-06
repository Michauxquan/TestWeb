using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    //用户通话信息
    public class UserCallInfoModel
    {
        private int ucallid;
        public int Ucallid
        {
            get { return ucallid; }
            set { ucallid = value; }
        }
        
        private int uid;
        /// <summary>
        /// 用户id
        /// </summary>
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        private string ctdid;
        /// <summary>
        ///话务单ID 
        /// </summary>
        public string Ctdid
        {
            get { return ctdid; }
            set { ctdid = value; }
        }

        private string calling;
        /// <summary>
        ///主叫号码 
        /// </summary>
        public string Calling
        {
            get { return calling; }
            set { calling = value; }
        }

        private string called;
        /// <summary>
        ///被叫号码 
        /// </summary>
        public string Called
        {
            get { return called; }
            set { called = value; }
        }

        private string starttime;
        /// <summary>
        /// 通话开始时间 格式:yyyy-mm-dd HH:mm:ss
        /// </summary>
        public string Starttime
        {
            get { return starttime; }
            set { starttime = value; }
        }

        private string endtime;
        /// <summary>
        /// 通话结束时间
        /// </summary>
        public string Endtime
        {
            get { return endtime; }
            set { endtime = value; }
        }

        private string sencods;
        /// <summary>
        /// 通话时长，秒
        /// </summary>
        public string Sencods
        {
            get { return sencods; }
            set { sencods = value; }
        }

        private decimal spend;
        /// <summary>
        /// 费用，保留两位小数
        /// </summary>
        public decimal Spend
        {
            get { return spend; }
            set { spend = value; }
        }

        private string account;
        /// <summary>
        /// 账号
        /// </summary>
        public string Account
        {
            get { return account; }
            set { account = value; }
        }

        private DateTime addtime;

        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }

        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount { get; set; }

    }
}
