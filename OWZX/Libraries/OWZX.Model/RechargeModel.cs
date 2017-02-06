using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OWZX.Model
{
    public class RechargeModel
    {
        public int Id { get; set; }
        private int rechargeid;

        public int RechargeId
        {
            get { return rechargeid; }
            set { rechargeid = value; }
        }
        private string account;
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string Account
        {
            get { return account; }
            set { account = value; }
        }
        private string vossuiteid;
        /// <summary>
        /// 套餐id
        /// </summary>
        public string SuiteId
        {
            get { return vossuiteid; }
            set { vossuiteid = value; }
        }
        /// <summary>
        /// 充值套餐
        /// </summary>
        public string SuiteName
        {
            get;
            set;
        }
        private string trade_no;
        /// <summary>
        ///支付系统订单编号
        /// </summary>
        public string Trade_no
        {
            get { return trade_no; }
            set { trade_no = value; }
        }

        private string out_trade_no;
        /// <summary>
        /// 系统订单号
        /// </summary>
        public string Out_trade_no
        {
            get { return out_trade_no; }
            set { out_trade_no = value; }
        }

        private string paytime;
        /// <summary>
        /// 支付时间
        /// </summary>
        public string Paytime
        {
            get { return paytime; }
            set { paytime = value; }
        }

        private decimal total_fee;
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Total_fee
        {
            get { return total_fee; }
            set { total_fee = value; }
        }
        private string platform;
        /// <summary>
        /// 支付类型
        /// </summary>
        public string PlatForm
        {
            get { return platform; }
            set { platform = value; }

        }

        private int type;
        /// <summary>
        /// 充值类型（1:充话费 2:升级充值 3:充流量）
        /// </summary>
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        private int role;
        /// <summary>
        /// 升级类型（8：队长 9：经理 ）
        /// </summary>
        public int Role
        {
            get { return role; }
            set { role = value; }
        }

        private string compnum;
        /// <summary>
        /// 短号
        /// </summary>
        public string CompNum
        {
            get { return compnum; }
            set { compnum = value; }
        }
        private string nickname;
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string NickName
        {
            get { return nickname; }
            set { nickname = value; }
        }
        /// <summary>
        /// 当日充值金额
        /// </summary>
        public decimal totalmoney { get; set; }
        /// <summary>
        /// 用户职位
        /// </summary>
        public string UserRank { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount { get; set; }

    }

}
