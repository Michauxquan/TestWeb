using OWZX.Model;
using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Web.Admin.Models
{
    public class ProfitStatList
    {
        /// <summary>
        /// 彩票类型
        /// </summary>
        public int Lottery { get; set; }
        /// <summary>
        /// 统计类型
        /// </summary>
        public string Type { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }

        /// <summary>
        /// 盈利列表
        /// </summary>
        public DataTable ProfitList { get; set; }
    }
}
