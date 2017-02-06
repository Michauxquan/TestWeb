using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    public class AdviceInfoModel
    {
        public Int64 Id { get; set; }
        private int adviceid;
        public int Adviceid
        {
            get { return adviceid; }
            set { adviceid = value; }
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

        private string remark;
        /// <summary>
        /// 内容
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        private DateTime addtime;
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }
        private string _reply;
        /// <summary>
        /// 回复
        /// </summary>
        public string reply { get { return _reply == null ? "" : _reply; } set { _reply = value; } }
        /// <summary>
        /// 回复用户id
        /// </summary>
        public int replyuid { get; set; }
        /// <summary>
        /// 回复时间
        /// </summary>
        public DateTime replytime { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount { get; set; }

    }
}
