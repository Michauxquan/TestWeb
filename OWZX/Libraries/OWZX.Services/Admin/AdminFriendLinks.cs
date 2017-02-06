using System;

using OWZX.Core;

namespace OWZX.Services
{
    /// <summary>
    /// 后台友情链接操作管理类
    /// </summary>
    public partial class AdminFriendLinks : FriendLinks
    {
        /// <summary>
        /// 创建友情链接
        /// </summary>
        public static void CreateFriendLink(FriendLinkInfo friendLinkInfo)
        {
            OWZX.Data.FriendLinks.CreateFriendLink(friendLinkInfo);
            OWZX.Core.BSPCache.Remove(CacheKeys.SHOP_FRIENDLINK_LIST);
        }

        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="idList">友情链接id</param>
        public static void DeleteFriendLinkById(int[] idList)
        {
            if (idList != null && idList.Length > 0)
            {
                OWZX.Data.FriendLinks.DeleteFriendLinkById(CommonHelper.IntArrayToString(idList));
                OWZX.Core.BSPCache.Remove(CacheKeys.SHOP_FRIENDLINK_LIST);
            }
        }

        /// <summary>
        /// 更新友情链接
        /// </summary>
        public static void UpdateFriendLink(FriendLinkInfo friendLinkInfo)
        {
            OWZX.Data.FriendLinks.UpdateFriendLink(friendLinkInfo);
            OWZX.Core.BSPCache.Remove(CacheKeys.SHOP_FRIENDLINK_LIST);
        }
    }
}
