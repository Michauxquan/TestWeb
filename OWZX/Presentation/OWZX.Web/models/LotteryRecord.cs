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
        public PageModel PageModel { get; set; }

        public DataTable Records { get; set; }
    }
}