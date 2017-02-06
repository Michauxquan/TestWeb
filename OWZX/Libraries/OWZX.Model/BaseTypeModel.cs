using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace OWZX.Model
{
    public class BaseTypeListModel
    {
        public List<BaseTypeModel> basetypelist { get; set; }
    }
    public class BaseTypeModel
    {
        private int systypeid;
        /// <summary>
        /// id
        /// </summary>
        public int Systypeid
        {
            get { return systypeid; }
            set { systypeid = value; }
        }

        private int outtypeid;
        /// <summary>
        /// 外部类型id
        /// </summary>
        public int Outtypeid {
            get { return outtypeid; }
            set { outtypeid = value; }
        }

        private string type;
        /// <summary>
        /// 类型
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(30, ErrorMessage = "名称长度不能大于30")]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private int parentid;
        /// <summary>
        /// 父级id
        /// </summary>
        public int Parentid
        {
            get { return parentid; }
            set { parentid = value; }
        }

        private string parenttype;
        /// <summary>
        /// 父级类型
        /// </summary>
        public string ParentType
        {
            get { return parenttype; }
            set { parenttype = value; }
        }

        private string remark;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        private int displayorder;
        /// <summary>
        /// 排序
        /// </summary>
        public int DisplayOrder
        {
            get { return displayorder; }
            set { displayorder = value; }
        }

    }
}
