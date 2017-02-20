using System;
using System.Data;
using System.Text;
using System.Web.Mvc;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.Web.Models;

namespace OWZX.Web.Controllers
{
    /// <summary>
    /// 新闻控制器类
    /// </summary>
    public partial class AgentController : BaseWebController
    { 
        /// <summary>
        /// 代理商列表
        /// </summary>
        public ActionResult List(int page=1)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(" where a.usertype=1  and a.isfreeze=0 ");  
            strb.Append("order by a.uid desc"); 
            DataTable dt = AdminUsers.GetUserList(15, page, strb.ToString());
            UserListModel model = new UserListModel()
            {
                PageModel = new PageModel(15, page, dt.Rows.Count),
                UserList = dt 
            };
            return View(model);
        }
    }
}
