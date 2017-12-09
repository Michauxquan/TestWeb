using OWZX.Model;
using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OWZX.Web.models
{
    public class LotteryTrend
    {
        public int Type { get; set; }
        public int Page { get; set; }
        public List<MD_LotTrend> List { get; set; }

        /// <summary>
        /// 分页
        /// </summary>
        public PageModel PageModel { get; set; }
    }
}