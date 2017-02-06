using System;
using System.ComponentModel.DataAnnotations;

using OWZX.Core;
using OWZX.Services;
using System.ComponentModel;

namespace OWZX.Web.Admin.Models
{
    /// <summary>
    /// 管理员组列表模型类
    /// </summary>
    public class AdminGroupListModel
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepName
        {
            get;
            set;
        }
        /// <summary>
        /// 部门ID
        /// </summary>
        public int DepId
        {
            get ;
            set ;
        }
        public AdminGroupListModel()
        {
            DepId = -1;
            DepName = "选择部门";
        }
        /// <summary>
        /// 管理员组列表
        /// </summary>
        public AdminGroupInfo[] AdminGroupList { get; set; }
    }

    /// <summary>
    /// 管理员组模型类
    /// </summary>
    public class AdminGroupModel
    {
        /// <summary>
        /// 管理员组标题
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(25, ErrorMessage = "名称长度不能大于25")]
        public string AdminGroupTitle { get; set; }

        /// <summary>
        /// 动作列表
        /// </summary>
        public string[] ActionList { get; set; }

        /// <summary>
        /// 前端动作列表
        /// </summary>
        public string[] CusActionList { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        //[Required(ErrorMessage = "请选择部门")]
        //[Range(1, int.MaxValue, ErrorMessage = "请选择部门")]
        //[DisplayName("部门分类")]
        public int DepId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepName { get; set; }
    }
}
