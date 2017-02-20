using System;
using System.Data;
using System.Web.Mvc;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;

namespace OWZX.Web
{
    /// <summary>
    /// 用户列表模型类
    /// </summary>
    public class UserListModel
    {
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        /// 用户列表
        /// </summary>
        public DataTable UserList { get; set; }
        
    } 

}
