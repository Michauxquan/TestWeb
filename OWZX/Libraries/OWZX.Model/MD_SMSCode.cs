using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    public class MD_SMSCode
    {
        private int codeid;
        public int Codeid
        {
            get { return codeid; }
            set { codeid = value; }
        }

        private string account;
        /// <summary>
        ///账号 
        /// </summary>
        public string Account
        {
            get { return account; }
            set { account = value; }
        }

        private string code;
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        private DateTime expiretime;
        /// <summary>
        ///有效期 
        /// </summary>
        public DateTime Expiretime
        {
            get { return expiretime; }
            set { expiretime = value; }
        }

        private DateTime addtime;
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }

    }
}
