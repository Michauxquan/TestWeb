using OWZX.Model;
using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Web.Admin.Models
{
    public class UserBackList
    {
        public string Account { get; set; }
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }

        /// <summary>
        /// 回水列表
        /// </summary>
        public List<MD_UserBack> BackList { get; set; }
    }
}
