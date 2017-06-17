using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    public class MD_AppLimit
    {
        private int limitid;
        public int Limitid
        {
            get { return limitid; }
            set { limitid = value; }
        }

        private string ip;
        /// <summary>
        /// ip
        /// </summary>
        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }

        private string domin;
        /// <summary>
        /// 域名
        /// </summary>
        public string Domin
        {
            get { return domin; }
            set { domin = value; }
        }

        private string port;
        /// <summary>
        /// 端口
        /// </summary>
        public string Port
        {
            get { return port; }
            set { port = value; }
        }

        private DateTime limittime;
        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime Limittime
        {
            get { return limittime; }
            set { limittime = value; }
        }

        private string remark;
        /// <summary>
        /// 备注
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

        private DateTime updatetime;
        public DateTime Updatetime
        {
            get { return updatetime; }
            set { updatetime = value; }
        }

    }
}
