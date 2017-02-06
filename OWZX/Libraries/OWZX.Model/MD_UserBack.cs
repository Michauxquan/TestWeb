using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    /// <summary>
    /// 用户回水
    /// </summary>
    public class MD_UserBack
    {
        public Int64 Id { get; set; }
        private int backid;
        public int Backid
        {
            get { return backid; }
            set { backid = value; }
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
        public string Account { get; set; }

        private decimal money;
        /// <summary>
        /// 回水金额
        /// </summary>
        public decimal Money
        {
            get { return money; }
            set { money = value; }
        }
        /// <summary>
        /// 亏损金额
        /// </summary>
        public decimal ProfitMoney { get; set; }
        /// <summary>
        /// 组合比例
        /// </summary>
        public decimal CombRatio { get; set; }

        private int roomtype;
        /// <summary>
        /// 房间类型id
        /// </summary>
        public int Roomtype
        {
            get { return roomtype; }
            set { roomtype = value; }
        }
        private string room;
        /// <summary>
        /// 房间类型
        /// </summary>
        public string Room
        {
            get { return room; }
            set { room = value; }
        }
        private short status;
        /// <summary>
        /// 状态：0 未结算 1审核中 2已结算 3结算失败
        /// </summary>
        public short Status
        {
            get { return status; }
            set { status = value; }
        }

        public int Updateuid { get; set; }

        private string addtime;
        public string Addtime
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

        public int TotalCount { get; set; }
    }
}
