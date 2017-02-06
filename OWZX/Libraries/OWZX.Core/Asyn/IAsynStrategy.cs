using System;

namespace OWZX.Core
{
    /// <summary>
    /// OWZX异步策略接口
    /// </summary>
    public partial interface IAsynStrategy
    {
        /// <summary>
        /// 更新在线用户
        /// </summary>
        /// <param name="state">state</param>
        void UpdateOnlineUser(UpdateOnlineUserState state);


        /// <summary>
        /// 更新PV统计
        /// </summary>
        /// <param name="state">state</param>
        void UpdatePVStat(UpdatePVStatState state);

    }
}
