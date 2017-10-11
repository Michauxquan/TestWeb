using OWZX.Model;
using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Web.Admin.Models
{
    public class UserChangeList
    {
        public string Account { get; set; }
        public string Operater { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }

        /// <summary>
        /// 账变记录列表
        /// </summary>
        public List<MD_Change> ChangeList { get; set; }
    }
}
