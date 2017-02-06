using OWZX.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Data
{
   public class Advice
    {
        #region 意见
        /// <summary>
        /// 添加意见
        /// </summary>
        /// <param name="account"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static bool AddUserAdvice(string account, string remark)
        {
            return OWZX.Core.BSPData.RDBS.AddUserAdvice(account, remark);
        }
        /// <summary>
        /// 回复意见
        /// </summary>
        /// <param name="advice"></param>
        /// <returns></returns>
        public static string UpdateUserAdvice(AdviceInfoModel advice)
        {
            return OWZX.Core.BSPData.RDBS.UpdateUserAdvice(advice);
        }
        /// <summary>
        /// 获取意见列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable GetUserAdvice(int pageSize, int pageNumber, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetUserAdvice(pageSize, pageNumber, condition);
        }
        #endregion
    }
}
