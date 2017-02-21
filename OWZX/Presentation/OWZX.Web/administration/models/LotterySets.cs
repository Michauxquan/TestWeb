using OWZX.Model;
using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Web.Admin.Models
{
    public class LotterySets
    {
        /// <summary>
        /// 房间类型
        /// </summary>
        public int roomtype { get; set; }
        /// <summary>
        /// 彩票类型
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 投注类型
        /// </summary>
        public int bttype { get; set; }
         

        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }

        /// <summary>
        /// 赔率列表
        /// </summary>
        public List<MD_LotterySet> SetList { get; set; }
    }
}
