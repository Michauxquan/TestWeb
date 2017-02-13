using OWZX.Model;
using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OWZX.Web.Models
{
    public class LotteryModel
    {
        /// <summary>
        /// 彩票类型
        /// </summary>
        public int LotteryType { get; set; }
        public PageModel PageModel { get; set; }
        public MD_LotteryList lotterylist { get; set; }
    }

    
}