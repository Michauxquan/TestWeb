using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    /// <summary>
    /// 等待发放奖金
    /// </summary>
    public class MD_WaitPayBonus
    {
        private int bonusid;
        public int Bonusid
        {
            get { return bonusid; }
            set { bonusid = value; }
        }

        private string expect;
        /// <summary>
        /// 期号
        /// </summary>
        public string Expect
        {
            get { return expect; }
            set { expect = value; }
        }

        private bool isread;
        /// <summary>
        /// 是否处理
        /// </summary>
        public bool Isread
        {
            get { return isread; }
            set { isread = value; }
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
