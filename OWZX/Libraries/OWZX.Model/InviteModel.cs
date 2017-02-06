using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    public class InviteModel
    {
        private int inviteid;
        public int Inviteid
        {
            get { return inviteid; }
            set { inviteid = value; }
        }

        private int uid;
        /// <summary>
        /// 父级id
        /// </summary>
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        private string chaildaccount;
        /// <summary>
        /// 子级手机号
        /// </summary>
        public string Chaildaccount
        {
            get { return chaildaccount; }
            set { chaildaccount = value; }
        }

        private int invitecode;
        /// <summary>
        /// 邀请码
        /// </summary>
        public int Invitecode
        {
            get { return invitecode; }
            set { invitecode = value; }
        }

        private DateTime addtime;

        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }

    }
}
