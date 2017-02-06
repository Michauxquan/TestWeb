using OWZX.Model;
using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Web.Admin.Models
{
    public class UserBankListModel
    {
        /// <summary>
        /// 开户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户手机
        /// </summary>
        public string Mobile { get; set; }
       
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }

        /// <summary>
        /// 用户绑定银行卡列表
        /// </summary>
        public List<MD_DrawAccount> BankList { get; set; }
    }
}
