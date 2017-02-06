using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace OWZX.Model
{
    /// <summary>
    /// 部门列表模型类
    /// </summary>
    public class DepartmentListModel
    {
        /// <summary>
        /// 部门列表
        /// </summary>
        public IList<DepartmentModel> DepartList { get; set; }
    }
    /// <summary>
    /// 部门列表
    /// </summary>
    public class DepartmentModel
    {
        private int depid;
        private string name;
        private string depnum;
        private int displayorder;
        private int parentid;

        /// <summary>
        /// 部门id
        /// </summary>
        public int Depid
        {
            get { return depid; }
            set { depid = value; }
        }

        
        /// <summary>
        /// 部门名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(30, ErrorMessage = "名称长度不能大于30")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 部门简称
        /// </summary>
        [StringLength(20, ErrorMessage = "名称长度不能大于20")]
        public string Depnum
        {
            get { return depnum; }
            set { depnum = value; }
        }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int Displayorder
        {
            get { return displayorder; }
            set { displayorder = value; }
        }
        /// <summary>
        /// 父级id
        /// </summary>
        public int Parentid
        {
            get { return parentid; }
            set { parentid = value; }
        }

        private int layer;
        /// <summary>
        /// 层级
        /// </summary>
        public int Layer
        {
            get { return layer; }
            set { layer = value; }
        }

        private int haschild;
        /// <summary>
        /// 是否有子级
        /// </summary>
        public int Haschild
        {
            get { return haschild; }
            set { haschild = value; }
        }

        private string path;
        /// <summary>
        /// 路径
        /// </summary>
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        private DateTime addtime;
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }
        private int adduser;
        /// <summary>
        /// 添加人
        /// </summary>
        public int Adduser
        {
            get { return adduser; }
            set { adduser = value; }
        }

        private DateTime updatetime;
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updatetime
        {
            get { return updatetime; }
            set { updatetime = value; }
        }

        private int updateuser;
        /// <summary>
        /// 更新用户
        /// </summary>
        public int Updateuser
        {
            get { return updateuser; }
            set { updateuser = value; }
        }
    }
}
