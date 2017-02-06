using OWZX.Core;
using OWZX.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Services
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
            return OWZX.Data.Advice.AddUserAdvice(account, remark);
        }
        /// <summary>
        /// 回复意见
        /// </summary>
        /// <param name="advice"></param>
        /// <returns></returns>
        public static bool UpdateUserAdvice(AdviceInfoModel advice)
        {
            string result = OWZX.Data.Advice.UpdateUserAdvice(advice);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取意见列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static List<AdviceInfoModel> GetUserAdvice(int pageSize, int pageNumber, string condition = "")
        {
            DataTable dt = OWZX.Data.Advice.GetUserAdvice(pageNumber, pageSize, condition);
            List<AdviceInfoModel> suitelist = (List<AdviceInfoModel>)ModelConvertHelper<AdviceInfoModel>.ConvertToModel(dt);
            return suitelist;
        }
        #endregion
    }
}
