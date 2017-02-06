using System;

using OWZX.Core;

namespace OWZX.Services
{
    /// <summary>
    /// 友情链接操作管理类
    /// </summary>
    public partial class FriendLinks
    {
        /// <summary>
        /// 获得友情链接列表
        /// </summary>
        public static FriendLinkInfo[] GetFriendLinkList()
        {
            FriendLinkInfo[] friendLinkList = OWZX.Core.BSPCache.Get(CacheKeys.SHOP_FRIENDLINK_LIST) as FriendLinkInfo[];
            if (friendLinkList == null)
            {
                friendLinkList = OWZX.Data.FriendLinks.GetFriendLinkList();
                OWZX.Core.BSPCache.Insert(CacheKeys.SHOP_FRIENDLINK_LIST, friendLinkList);
            }
            return friendLinkList;
        }

        /// <summary>
        /// 获得友情链接
        /// </summary>
        /// <param name="id">友情链接id</param>
        /// <returns></returns>
        public static FriendLinkInfo GetFriendLinkById(int id)
        {
            foreach (FriendLinkInfo friendLinkInfo in GetFriendLinkList())
            {
                if (friendLinkInfo.Id == id)
                    return friendLinkInfo;
            }

            return null;
        }
    }
}
