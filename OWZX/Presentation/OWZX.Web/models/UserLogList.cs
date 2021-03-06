﻿using OWZX.Model;
using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWZX.Core;

namespace OWZX.Web
{
    public class UserLogList
    {
        public int uid { get; set; }
        public int type { get; set; }

        public PartUserInfo PartUser { get; set; }

        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }

        /// <summary>
        /// 账变记录列表
        /// </summary>
        public List<MD_UsersLog> LogList { get; set; }
    }
}
