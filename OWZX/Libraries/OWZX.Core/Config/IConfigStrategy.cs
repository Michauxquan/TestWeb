using System;

namespace OWZX.Core
{
    /// <summary>
    /// OWZX配置策略接口
    /// </summary>
    public partial interface IConfigStrategy
    {
        /// <summary>
        /// 获得关系数据库配置
        /// </summary>
        RDBSConfigInfo GetRDBSConfig();
        /// <summary>
        /// 获取App更新信息
        /// </summary>
        /// <returns></returns>
        AppUpdateConfigInfo GetAppUpdateConfig();
        /// <summary>
        /// 保存商城基本配置
        /// </summary>
        /// <param name="configInfo">商城基本配置信息</param>
        /// <returns>是否保存成功</returns>
        bool SaveShopConfig(ShopConfigInfo configInfo);

        /// <summary>
        /// 获得商城基本配置
        /// </summary>
        ShopConfigInfo GetShopConfig();

        /// <summary>
        /// 保存邮件配置
        /// </summary>
        /// <param name="configInfo">邮件配置信息</param>
        /// <returns>是否保存成功</returns>
        bool SaveEmailConfig(EmailConfigInfo configInfo);

        /// <summary>
        /// 获得邮件配置
        /// </summary>
        EmailConfigInfo GetEmailConfig();
        /// <summary>
        /// 保存基础信息配置
        /// </summary>
        /// <param name="configInfo"></param>
        /// <returns></returns>
        bool SaveBaseConfig(BaseConfigInfo configInfo);
        /// <summary>
        /// 获得基础信息配置
        /// </summary>
        BaseConfigInfo GetBaseConfig();

        /// <summary>
        /// 保存短信配置
        /// </summary>
        /// <param name="configInfo">短信配置信息</param>
        /// <returns>是否保存成功</returns>
        bool SaveSMSConfig(SMSConfigInfo configInfo);

        /// <summary>
        /// 获得短信配置
        /// </summary>
        SMSConfigInfo GetSMSConfig();

       

        /// <summary>
        /// 保存事件配置
        /// </summary>
        /// <param name="configInfo">事件配置信息</param>
        /// <returns>是否保存成功</returns>
        bool SaveEventConfig(EventConfigInfo configInfo);

        /// <summary>
        /// 获得事件配置
        /// </summary>
        EventConfigInfo GetEventConfig();

       

        /// <summary>
        /// 获得Memcached缓存配置
        /// </summary>
        MemcachedCacheConfigInfo GetMemcachedCacheConfig();

        /// <summary>
        /// 获得Memcached会话状态配置
        /// </summary>
        MemcachedSessionConfigInfo GetMemcachedSessionConfig();



    }
}
