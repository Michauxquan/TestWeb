using OWZX.Model;
using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Web.Admin.Models
{
    public class VisitIPList
    {
        
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }

        /// <summary>
        /// 访问IP列表
        /// </summary>
        public List<MD_VisitIP> visitipList { get; set; }
    }
}
