using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OWZX.Model
{
    /// <summary>
    /// 转账记录
    /// </summary>
    public class MD_Remit
    {
        public Int64 Id { get; set; }
        private int remitid;
        public int Remitid
        {
            get { return remitid; }
            set { remitid = value; }
        }

        private string mobile;
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value.TrimEnd(); }
        }
        private int uid;
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        private string type;
        /// <summary>
        /// 转账方式
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value.TrimEnd(); }
        }

        private string name;
        /// <summary>
        ///汇款用户昵称 
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value.TrimEnd(); }
        }

        private string account;
        /// <summary>
        /// 汇款账号
        /// </summary>
        public string Account
        {
            get { return account; }
            set { account = value.TrimEnd(); }
        }

        private int money;
        /// <summary>
        ///汇款金额 
        /// </summary>
        public int Money
        {
            get { return money; }
            set { money = value; }
        }
        private int realmoney;
        /// <summary>
        ///实际汇款金额 
        /// </summary>
        public int RealMoney
        {
            get { return realmoney; }
            set { realmoney = value; }
        }
        
        private string bankname;
        /// <summary>
        /// 开户银行
        /// </summary>
        public string Bankname
        {
            get { return bankname; }
            set { bankname = value.TrimEnd(); }
        }

        private short status;
        /// <summary>
        /// 状态 0 待审核 1 审核中 2 已审核 3 审核失败
        /// </summary>
        public short Status
        {
            get { return status; }
            set { status = value; }
        }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string State
        {
            get;set;
        }

        private string remark;
        /// <summary>
        /// 失败原因
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value.TrimEnd(); }
        }

        private DateTime addtime;
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }
       
        private int updateuid;
        public int Updateuid
        {
            get { return updateuid; }
            set { updateuid = value; }
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
