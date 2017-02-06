using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    /// <summary>
    /// 系统设置
    /// </summary>
    public class MD_SysSet
    {
        private int setid;
        public int Setid
        {
            get { return setid; }
            set { setid = value; }
        }

        private int parentid;
        public int Parentid
        {
            get { return parentid; }
            set { parentid = value; }
        }

        private string name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string invalue;
        /// <summary>
        /// 值
        /// </summary>
        public string InValue
        {
            get { return invalue; }
            set { value = invalue; }
        }

        private DateTime addtiime;
        public DateTime Addtiime
        {
            get { return addtiime; }
            set { addtiime = value; }
        }

    }
}
