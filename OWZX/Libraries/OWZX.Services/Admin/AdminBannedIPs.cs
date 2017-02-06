using System;
using System.Data;

using OWZX.Core;

namespace OWZX.Services
{
    public partial class AdminBannedIPs : BannedIPs
    {
        /// <summary>
        /// 添加禁止的ip
        /// </summary>
        public static void AddBannedIP(BannedIPInfo bannedIPInfo)
        {
            OWZX.Data.BannedIPs.AddBannedIP(bannedIPInfo);
            OWZX.Core.BSPCache.Remove(CacheKeys.SHOP_BANNEDIP_HASHSET);
        }

        /// <summary>
        /// 更新禁止的ip
        /// </summary>
        public static void UpdateBannedIP(BannedIPInfo bannedIPInfo)
        {
            OWZX.Data.BannedIPs.UpdateBannedIP(bannedIPInfo);
            OWZX.Core.BSPCache.Remove(CacheKeys.SHOP_BANNEDIP_HASHSET);
        }

        /// <summary>
        /// 删除禁止的ip
        /// </summary>
        /// <param name="idList">id列表</param>
        public static void DeleteBannedIPById(int[] idList)
        {
            if (idList != null && idList.Length > 0)
            {
                OWZX.Data.BannedIPs.DeleteBannedIPById(CommonHelper.IntArrayToString(idList));
                OWZX.Core.BSPCache.Remove(CacheKeys.SHOP_BANNEDIP_HASHSET);
            }
        }

        /// <summary>
        /// 后台获得禁止的ip列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="ip">ip</param>
        /// <returns></returns>
        public static DataTable AdminGetBannedIPList(int pageSize, int pageNumber, string ip)
        {
            return OWZX.Data.BannedIPs.AdminGetBannedIPList(pageSize, pageNumber, ip);
        }

        /// <summary>
        /// 后台获得禁止的ip数量
        /// </summary>
        /// <param name="ip">ip</param>
        /// <returns></returns>
        public static int AdminGetBannedIPCount(string ip)
        {
            return OWZX.Data.BannedIPs.AdminGetBannedIPCount(ip);
        }
    }
}
