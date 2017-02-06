using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
   public class MD_VisitIP
    {
       public Int64 Id { get; set; }
        private int visitid;
        public int Visitid
        {
            get { return visitid; }
            set { visitid = value; }
        }

        private int uid;
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        private string account;
        public string Account {
            get { return account; }
            set { account = value; }
        }

        private string ip;
        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }
        public int TotalCount { get; set; }
        private DateTime addtime;
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }

    }
}
