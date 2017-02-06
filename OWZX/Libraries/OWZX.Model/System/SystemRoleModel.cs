using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace OWZX.Model
{
    /// <summary>
    /// 角色列表模型类
    /// </summary>
    public class SystemRoleListModel
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
        /// <summary>
        /// 角色列表
        /// </summary>
        public IList<SystemRoleModel> RoleList { get; set; }

        public SystemRoleListModel()
        {
            DepId = -1;
            DepName = "选择部门";
        }
    }
   public class SystemRoleModel
    {
        private int roleid;
        /// <summary>
        /// id
        /// </summary>
        public int RoleId
        {
            get { return roleid; }
            set { roleid = value; }
        }

        private string name;
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(30, ErrorMessage = "名称长度不能大于30")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string roleremark;
       /// <summary>
       /// 备注
       /// </summary>
        public string RoleRemark
        {
            get { return roleremark; }
            set { roleremark = value; }
        }

        private int displayorder;
       /// <summary>
       /// 显示顺序
       /// </summary>
        public int DisplayOrder
        {
            get { return displayorder; }
            set { displayorder = value; }
        }

        private int depid;
       /// <summary>
       /// 部门id
       /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "请选择部门")]
        public int DepId
        {
            get { return depid; }
            set { depid = value; }
        }

        private string depname;
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepName
        {
            get { return depname; }
            set { depname = value; }
        }
        /// <summary>
        /// 动作列表
        /// </summary>
        public string[] ActionList { get; set; }

        private DateTime addtime;
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }

        private int adduser;
        public int Adduser
        {
            get { return adduser; }
            set { adduser = value; }
        }

        private DateTime updatetime;
        public DateTime Updatetime
        {
            get { return updatetime; }
            set { updatetime = value; }
        }

        private int updateuser;
        public int Updateuser
        {
            get { return updateuser; }
            set { updateuser = value; }
        }

        public SystemRoleModel()
        {
            DepId = -1;
            DepName = "选择部门";
        }

    }
}
