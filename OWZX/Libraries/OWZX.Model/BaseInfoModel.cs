using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OWZX.Model
{
    /// <summary>
    /// 基础资料
    /// </summary>
    public class BaseInfoModel
    {
        private int baseid;
        public int BaseId
        {
            get { return baseid; }
            set { baseid = value; }
        }

        private string title;
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(100, ErrorMessage = "名称长度不能大于50")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string content;
        /// <summary>
        /// 内容
        /// </summary>
        [AllowHtml]
        [Required(ErrorMessage = "简介不能为空")]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private int outid;
        /// <summary>
        /// 外部id
        /// </summary>
        public int Outid {
            get { return outid; }
            set { outid = value; }
        }

        private DateTime addtime;
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }

    }
}
