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
        public PageModel PageModel { get; set; }
        public MD_LotteryList lotterylist { get; set; }
    }
}