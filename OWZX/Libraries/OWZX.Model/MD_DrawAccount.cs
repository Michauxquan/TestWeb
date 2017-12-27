using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    /// <summary>
    /// 银行卡
    /// </summary>
    public class MD_DrawAccount
    {
        public Int64 Id { get; set; }
        private int drawaccid;
        public int Drawaccid
        {
            get { return drawaccid; }
            set { drawaccid = value; }
        }

        private int uid;
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }
        private string account;
        /// <summary>
        /// 手机号
        /// </summary>
        public string Account
        {
            get { return account; }
            set { account = value.TrimEnd(); }
        }
        private string username;
        /// <summary>
        /// 开户名称
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value.TrimEnd(); }
        }

        private string card;
        /// <summary>
        /// 开户银行
        /// </summary>
        public string Card
        {
            get { return card; }
            set { card = value.TrimEnd(); }
        }

        private string cardnum;
        /// <summary>
        /// 卡号
        /// </summary>
        public string Cardnum
        {
            get { return cardnum; }
            set { cardnum = value.TrimEnd(); }
        }

        private string cardaddress;
        /// <summary>
        /// 开户地址
        /// </summary>
        public string Cardaddress
        {
            get { return cardaddress; }
            set { cardaddress = value.TrimEnd(); }
        }

        private string drawpwd;
        /// <summary>
        /// 提现密码
        /// </summary>
        [Required(ErrorMessage = "提现密码不能为空")]
        [StringLength(20,MinimumLength=6, ErrorMessage = "提现密码长度为6至20位")]
        public string Drawpwd
        {
            get { return drawpwd; }
            set { drawpwd = value.TrimEnd(); }
        }

        public decimal Money { get; set; }
        
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

        public int TotalCount { get; set; }

    }
}
