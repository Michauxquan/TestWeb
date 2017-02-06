using OWZX.Model;
using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Web.Admin.Models
{
   public class DrawListModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户手机
        /// </summary>
        public string Mobile { get; set; }
       /// <summary>
       /// 支付宝账号
       /// </summary>
        public string AlipayAccount { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }

        /// <summary>
        /// 提现列表
        /// </summary>
        public List<DrawInfoModel> DrawList { get; set; }
    }
}
