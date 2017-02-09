using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Core
{
    /// <summary>
    /// 新的竞猜数据接口
    /// </summary>
    public partial interface IRDBSStrategy
    {
        #region 竞猜数据
        /// <summary>
        /// 根据类型获取竞猜首页数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DataSet GetLotteryByType(string type, string pageindex, string pagesize);

        #endregion
    }
}
