using OWZX.Model;
using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Web.Admin.Models
{
   public class RechargeListModel
    {
        /// <summary>
        /// 用户手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 充值套餐
        /// </summary>
        public string Suite { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PlatForm { get; set; }
        /// <summary>
        /// 充值类型 1:充话费 2:升级充值 3:充流量
        /// </summary>
        public string ChargeType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }

        /// <summary>
        /// 充值列表
        /// </summary>
        public List<RechargeModel> RechargeList { get; set; }
    }
}
