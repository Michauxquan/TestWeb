using System;
using System.IO;

namespace OWZX.Core
{
    /// <summary>
    /// OWZX配置管理类
    /// </summary>
    public partial class BSPConfig
    {
        private static object _locker = new object();//锁对象

        private static IConfigStrategy _iconfigstrategy = null;//配置策略

        private static RDBSConfigInfo _rdbsconfiginfo = null;//关系数据库配置信息
        private static ShopConfigInfo _shopconfiginfo = null;//商城基本配置信息
        private static EmailConfigInfo _emailconfiginfo = null;//邮件配置信息
        private static SMSConfigInfo _smsconfiginfo = null;//短信配置信息
        private static EventConfigInfo _eventconfiginfo = null;//事件配置信息
        private static MemcachedCacheConfigInfo _memcachedcacheconfiginfo = null;//Memcached缓存配置信息
        private static MemcachedSessionConfigInfo _memcachedsessionconfiginfo = null;//Memcached会话状态配置信息
        private static AppUpdateConfigInfo _appupdate = null;//app更新
        private static BaseConfigInfo _base = null;

        static BSPConfig()
        {
            try
            {
                string[] fileNameList = Directory.GetFiles(System.Web.HttpRuntime.BinDirectory, "OWZX.ConfigStrategy.*.dll", SearchOption.TopDirectoryOnly);
                _iconfigstrategy = (IConfigStrategy)Activator.CreateInstance(Type.GetType(string.Format("OWZX.ConfigStrategy.{0}.ConfigStrategy, OWZX.ConfigStrategy.{0}", fileNameList[0].Substring(fileNameList[0].LastIndexOf("ConfigStrategy.") + 15).Replace(".dll", "")),
                                                                                         false,
                                                                                         true));
            }
            catch
            {
                throw new BSPException("创建'配置策略对象'失败,可能存在的原因:未将'配置策略程序集'添加到bin目录中;'配置策略程序集'文件名不符合'OWZX.ConfigStrategy.{策略名称}.dll'格式");
            }
            _rdbsconfiginfo = _iconfigstrategy.GetRDBSConfig();
            _shopconfiginfo = _iconfigstrategy.GetShopConfig();
        }

        /// <summary>
        /// 关系数据库配置信息
        /// </summary>
        public static RDBSConfigInfo RDBSConfig
        {
            get { return _rdbsconfiginfo; }
        }
        /// <summary>
        /// App更新配置信息
        /// </summary>
        public static AppUpdateConfigInfo AppUpdateConfig
        {
            get
            {
                if (_appupdate == null)
                {
                    lock (_locker)
                    {
                        if (_appupdate == null)
                        {
                            _appupdate = _iconfigstrategy.GetAppUpdateConfig();
                        }
                    }
                }
                return _appupdate;
            }
        }
        /// <summary>
        /// 商城基本配置信息
        /// </summary>
        public static ShopConfigInfo ShopConfig
        {
            get { return _shopconfiginfo; }
        }

        /// <summary>
        /// 邮件配置信息
        /// </summary>
        public static EmailConfigInfo EmailConfig
        {
            get
            {
                if (_emailconfiginfo == null)
                {
                    lock (_locker)
                    {
                        if (_emailconfiginfo == null)
                        {
                            _emailconfiginfo = _iconfigstrategy.GetEmailConfig();
                        }
                    }
                }

                return _emailconfiginfo;
            }
        }

        /// <summary>
        /// 基础配置信息
        /// </summary>
        public static BaseConfigInfo BaseConfig
        {
            get
            {
                if (_base == null)
                {
                    lock (_locker)
                    {
                        if (_base == null)
                        {
                            _base = _iconfigstrategy.GetBaseConfig();
                        }
                    }
                }

                return _base;
            }
        }
        /// <summary>
        /// 短息配置信息
        /// </summary>
        public static SMSConfigInfo SMSConfig
        {
            get
            {
                if (_smsconfiginfo == null)
                {
                    lock (_locker)
                    {
                        if (_smsconfiginfo == null)
                        {
                            _smsconfiginfo = _iconfigstrategy.GetSMSConfig();
                        }
                    }
                }
                return _smsconfiginfo;
            }
        }


        /// <summary>
        /// 事件配置信息
        /// </summary>
        public static EventConfigInfo EventConfig
        {
            get
            {
                if (_eventconfiginfo == null)
                {
                    lock (_locker)
                    {
                        if (_eventconfiginfo == null)
                        {
                            _eventconfiginfo = _iconfigstrategy.GetEventConfig();
                        }
                    }
                }
                return _eventconfiginfo;
            }
        }

        /// <summary>
        /// Memcached缓存配置信息
        /// </summary>
        public static MemcachedCacheConfigInfo MemcachedCacheConfig
        {
            get
            {
                if (_memcachedcacheconfiginfo == null)
                {
                    lock (_locker)
                    {
                        if (_memcachedcacheconfiginfo == null)
                        {
                            _memcachedcacheconfiginfo = _iconfigstrategy.GetMemcachedCacheConfig();
                        }
                    }
                }
                return _memcachedcacheconfiginfo;
            }
        }

        /// <summary>
        /// Memcached会话状态配置信息
        /// </summary>
        public static MemcachedSessionConfigInfo MemcachedSessionConfig
        {
            get
            {
                if (_memcachedsessionconfiginfo == null)
                {
                    lock (_locker)
                    {
                        if (_memcachedsessionconfiginfo == null)
                        {
                            _memcachedsessionconfiginfo = _iconfigstrategy.GetMemcachedSessionConfig();
                        }
                    }
                }
                return _memcachedsessionconfiginfo;
            }
        }



        /// <summary>
        /// 保存网站配置信息
        /// </summary>
        public static void SaveShopConfig(ShopConfigInfo shopConfigInfo)
        {
            lock (_locker)
            {
                if (_iconfigstrategy.SaveShopConfig(shopConfigInfo))
                    _shopconfiginfo = shopConfigInfo;
            }
        }

        /// <summary>
        /// 保存邮件配置信息
        /// </summary>
        public static void SaveEmailConfig(EmailConfigInfo emailConfigInfo)
        {
            lock (_locker)
            {
                if (_iconfigstrategy.SaveEmailConfig(emailConfigInfo))
                    _emailconfiginfo = null;
            }
        }

        /// <summary>
        /// 保存短信配置信息
        /// </summary>
        public static void SaveSMSConfig(SMSConfigInfo smsConfigInfo)
        {
            lock (_locker)
            {
                if (_iconfigstrategy.SaveSMSConfig(smsConfigInfo))
                    _smsconfiginfo = null;
            }
        }
        /// <summary>
        /// 保存基础配置信息
        /// </summary>
        public static void SaveBaseConfig(BaseConfigInfo baseConfigInfo)
        {
            lock (_locker)
            {
                if (_iconfigstrategy.SaveBaseConfig(baseConfigInfo))
                    _smsconfiginfo = null;
            }
        }
      
        /// <summary>
        /// 保存事件配置信息
        /// </summary>
        public static void SaveEventConfig(EventConfigInfo eventConfigInfo)
        {
            lock (_locker)
            {
                if (_iconfigstrategy.SaveEventConfig(eventConfigInfo))
                    _eventconfiginfo = null;
            }
        }
    }
}
