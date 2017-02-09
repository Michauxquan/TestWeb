using OWZX.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.RDBSStrategy.SqlServer
{
    /// <summary>
    /// SqlServer策略之新的竞猜
    /// </summary>
    public partial class RDBSStrategy : IRDBSStrategy
    {
        #region 竞猜数据
        /// <summary>
        /// 根据类型获取竞猜首页数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataSet GetLotteryByType(string type, string pageindex, string pagesize)
        {
            string sql = string.Format(@"");
            return RDBSHelper.ExecuteDataset(sql);
        }
        #endregion
    }
}
