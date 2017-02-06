using System;
using System.Data;
using System.Collections.Generic;

using OWZX.Core;

namespace OWZX.Services
{
    /// <summary>
    /// 后台广告操作管理类
    /// </summary>
    public partial class AdminAdverts : Adverts
    {
        /// <summary>
        /// 创建广告位置
        /// </summary>
        public static void CreateAdvertPosition(AdvertPositionInfo advertPositionInfo)
        {
            OWZX.Data.Adverts.CreateAdvertPosition(advertPositionInfo);
        }

        /// <summary>
        /// 更新广告位置
        /// </summary>
        public static void UpdateAdvertPosition(AdvertPositionInfo advertPositionInfo)
        {
            OWZX.Data.Adverts.UpdateAdvertPosition(advertPositionInfo);
        }

        /// <summary>
        /// 删除广告位置
        /// </summary>
        /// <param name="adPosId">广告位置id</param>
        public static void DeleteAdvertPositionById(int adPosId)
        {
            OWZX.Data.Adverts.DeleteAdvertPositionById(adPosId);
            OWZX.Core.BSPCache.Remove(CacheKeys.SHOP_ADVERT_LIST + adPosId);

        }




        /// <summary>
        /// 创建广告
        /// </summary>
        public static void CreateAdvert(AdvertInfo advertInfo)
        {
            OWZX.Data.Adverts.CreateAdvert(advertInfo);
            OWZX.Core.BSPCache.Remove(CacheKeys.SHOP_ADVERT_LIST + advertInfo.AdPosId);
        }

        /// <summary>
        /// 更新广告
        /// </summary>
        public static void UpdateAdvert(int oldAdPosId, AdvertInfo advertInfo)
        {
            OWZX.Data.Adverts.UpdateAdvert(advertInfo);
            if (oldAdPosId == advertInfo.AdPosId)
            {
                OWZX.Core.BSPCache.Remove(CacheKeys.SHOP_ADVERT_LIST + advertInfo.AdPosId);
            }
            else
            {
                OWZX.Core.BSPCache.Remove(CacheKeys.SHOP_ADVERT_LIST + oldAdPosId);
                OWZX.Core.BSPCache.Remove(CacheKeys.SHOP_ADVERT_LIST + advertInfo.AdPosId);
            }
        }

        /// <summary>
        /// 删除广告
        /// </summary>
        /// <param name="adId">广告id</param>
        public static void DeleteAdvertById(int adId)
        {
            AdvertInfo advertInfo = AdminGetAdvertById(adId);
            if (advertInfo != null)
            {
                OWZX.Data.Adverts.DeleteAdvertById(adId);
                OWZX.Core.BSPCache.Remove(CacheKeys.SHOP_ADVERT_LIST + advertInfo.AdPosId);
            }
        }

        /// <summary>
        /// 后台获得广告
        /// </summary>
        /// <param name="adId">广告id</param>
        /// <returns></returns>
        public static AdvertInfo AdminGetAdvertById(int adId)
        {
            return OWZX.Data.Adverts.AdminGetAdvertById(adId);
        }

        /// <summary>
        /// 后台获得广告列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="adPosId">广告位置id</param>
        /// <returns></returns>
        public static DataTable AdminGetAdvertList(int pageSize, int pageNumber, int adPosId)
        {
            return OWZX.Data.Adverts.AdminGetAdvertList(pageSize, pageNumber, adPosId);
        }

        /// <summary>
        /// 后台获得广告数量
        /// </summary>
        /// <param name="adPosId">广告位置id</param>
        /// <returns></returns>
        public static int AdminGetAdvertCount(int adPosId)
        {
            return OWZX.Data.Adverts.AdminGetAdvertCount(adPosId);
        }
    }
}
