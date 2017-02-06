using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    public class DrawInfoModel
    {
        [JsonIgnore]
        public Int64 Id { get; set; }
        private int drawid;
        [JsonIgnore]
        public int Drawid
        {
            get { return drawid; }
            set { drawid = value; }
        }
        private string account;
        /// <summary>
        /// 登录账号
        /// </summary>
        [JsonIgnore]
        public string Account
        {
            get { return account; }
            set { account = value; }
        }
        private int uid;
        /// <summary>
        /// 用户id
        /// </summary>
        [JsonIgnore]
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        private string mobile;
        /// <summary>
        /// 提现手机号
        /// </summary>
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }
        private string compnum;
        /// <summary>
        /// 短号
        /// </summary>
        public string CompNum {
            get { return compnum; }
            set { compnum = value; }
        }
        private string username;
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        private string card;
        /// <summary>
        /// 开户银行
        /// </summary>
        public string Card
        {
            get { return card; }
            set { card = value; }
        }
        private string cardnum;
        /// <summary>
        /// 开户卡号
        /// </summary>
        public string CardNum
        {
            get { return cardnum; }
            set { cardnum = value; }
        }
        private string cardaddress;
        /// <summary>
        /// 开户银行地址
        /// </summary>
        public string CardAddress
        {
            get { return cardaddress; }
            set { cardaddress = value; }
        }
        private decimal totalmoney;
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal TotalMoney
        {
            get { return totalmoney; }
            set { totalmoney = value; }
        }

        private int money;
        /// <summary>
        /// 提现金额
        /// </summary>
        public int Money
        {
            get { return money; }
            set { money = value; }
        }

        private string state;
        /// <summary>
        /// 状态(0：待审核 1：审核中 2：审核完成 3:审核失败)
        /// </summary>
        public string State
        {
            get { return state; }
            set { state = value; }
        }

        private string exception;
        /// <summary>
        /// 异常信息
        /// </summary>
        public string Exception
        {
            get { return exception; }
            set { exception = value; }
        }

        private DateTime addtime;
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }
        private string updateuser;
        /// <summary>
        /// 操作用户
        /// </summary>
        [JsonIgnore]
        public string Updateuser {
            get { return updateuser; }
            set { updateuser = value; }
        }
        private DateTime updatetime;
        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonIgnore]
        public DateTime Updatetime
        {
            get { return updatetime; }
            set { updatetime = value; }
        }
        /// <summary>
        /// 总数量
        /// </summary>
        [JsonIgnore]
        public int TotalCount { get; set; }

    }
}
