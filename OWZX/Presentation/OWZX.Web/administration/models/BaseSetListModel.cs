using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using OWZX.Core;

namespace OWZX.Web.Admin.Models
{
    /// <summary>
    /// 基础配置列表模型类
    /// </summary>
    public class BaseSetListModel
    {
        /// <summary>
        /// 列表
        /// </summary>
        public List<BaseInfo> BaseSetList { get; set; }

       
    }
    
}
