using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace OWZX.Web.Models
{
    public class LotteryRecord
    {
        /// <summary>
        /// 彩票类型
        /// </summary>
        public int LotteryType { get; set; }
        public PageModel PageModel { get; set; }

        public DataTable Records { get; set; }
    }
}