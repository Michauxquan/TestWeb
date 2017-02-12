using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    public class MD_UserOrder
    {
        /// <summary>
        /// 订单编码
        /// </summary>
        private int orderid;
        public int OrderID
        {
            get { return orderid; }
            set { orderid = value; }
        }
        /// <summary>
        /// 手机号
        /// </summary>
        private string account;
        public string Account
        {
            get { return account; }
            set { account = value; }
        }
        /// <summary>
        /// 夺宝编号
        /// </summary>
        private int changeid;
        public int ChangeID
        {
            get { return changeid; }
            set { changeid = value; }
        }
        /// <summary>
        /// 订单编码
        /// </summary>
        private string ordercode;
        public string OrderCode
        {
            get { return ordercode; }
            set { ordercode = value; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        private int userid;
        public int UserID
        {
            get { return userid; }
            set { userid = value; }
        }
        /// <summary>
        /// 商品代码
        /// </summary>
        private string warecode;
        public string WareCode
        {
            get { return warecode; }
            set { warecode = value; }
        }

        private string warename;
        public string WareName
        {
            get { return warename; }
            set { warename = value; }
        }

        private string speccode;
        public string SpecCode
        {
            get { return speccode; }
            set { speccode = value; }
        }
        private string specname;
        public string SpecName
        {
            get { return specname; }
            set { specname = value; }
        }
        /// <summary>
        /// 期号 类型兑换时允许为空
        /// </summary>
        private string issuenum;
        public string IssueNum
        {
            get { return issuenum; }
            set { issuenum = value; }
        }
        //购买数量
        private int num;
        public int Num
        {
            get { return num; }
            set { num = value; }
        }
        /// <summary>
        /// 投注内容
        /// </summary>
        private string content;
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        /// <summary>
        /// 0 未开奖 1 开奖中 2 已中奖 3未中奖
        /// </summary>
        private int status;
        public int Status
        {
            get { return status; }
            set { status = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime createtime;
        public DateTime CreateTime
        {
            get { return createtime; }
            set { createtime = value; }
        }

        /// <summary>
        /// 1夺宝  0 兑换
        /// </summary>
        private int type;
        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        /// <summary>
        /// 单价
        /// </summary>
        private decimal price;
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }
        /// <summary>
        /// 总价
        /// </summary>
        private decimal totalfee;
        public decimal TotalFee
        {
            get { return totalfee; }
            set { totalfee = value; }
        }
    }
}
