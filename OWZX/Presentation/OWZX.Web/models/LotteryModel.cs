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
        /// <summary>
        /// 彩票总时间
        /// </summary>
        public int TotalS { get; set; }
        /// <summary>
        /// 彩票封盘时间
        /// </summary>
        public int StopTime { get; set; }
        /// <summary>
        /// 分页
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        ///竞猜记录
        /// </summary>
        public MD_LotteryList lotterylist { get; set; }
    }

    
}