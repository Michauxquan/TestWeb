﻿using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Collections.Concurrent;

using OWZX.Core;
using OWZX.Model;

namespace OWZX.RDBSStrategy.SqlServer
{
    /// <summary>
    /// SqlServer策略之商城分部类
    /// </summary>
    public partial class RDBSStrategy : IRDBSStrategy
    {
        #region PV统计

        private static object _pvstatlocker = new object();//PV统计锁

        private static int _lastupdatepvstatstime = 0;//最后一次更新pv统计的时间

        private static int _userpvstat = 0;//用户访问数
        private static int _memberpvstat = 0;//会员访问数
        private static int _guestpvstat = 0;//游客访问数
        private static int _yearpvstat = 0;//今年访问数
        private static int _monthpvstat = 0;//本月访问数
        private static int _daypvstat = 0;//今天访问数
        private static int _hourpvstat = 0;//当前小时访问数
        private static int _weekpvstat = 0;//本周访问数
        private static volatile ConcurrentDictionary<string, int> _browserpvstat = new ConcurrentDictionary<string, int>();//浏览器统计
        private static volatile ConcurrentDictionary<string, int> _ospvstat = new ConcurrentDictionary<string, int>();//操作系统统计
        private static volatile ConcurrentDictionary<int, int> _regionpvstat = new ConcurrentDictionary<int, int>();//区域统计


        /// <summary>
        /// 获取访问IP统计
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetWebFlow(int pageNumber, int pageSize, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };
            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
  drop table #list

SELECT ROW_NUMBER() over(order by a.visitid desc) id
      ,a.[visitid]
      ,a.[uid]
      ,a.[ip]
      ,b.[email] account
      ,a.[addtime]
  into  #list
  FROM (select distinct * from owzx_visitip) a
  left join owzx_users b on a.uid=b.uid 
  {0}


if(@pagesize=-1)
begin
select * from #list
end
else
begin
select * from #list where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end

end try
begin catch
select ERROR_MESSAGE() state
end catch

", condition);

            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, parms)[0];
        }

        /// <summary>
        /// 更新PV统计
        /// </summary>
        /// <param name="updatePVStatState">更新PV统计状态</param>
        public void UpdatePVStat(UpdatePVStatState updatePVStatState)
        {
            lock (_pvstatlocker)
            {
                ShopConfigInfo shopConfig = BSPConfig.ShopConfig;

                ++_userpvstat;
                ++_yearpvstat;
                ++_monthpvstat;
                ++_daypvstat;
                ++_hourpvstat;
                ++_weekpvstat;
                if (updatePVStatState.IsMember)
                    ++_memberpvstat;
                else
                    ++_guestpvstat;

                //更新浏览器统计
                if (shopConfig.IsStatBrowser == 1)
                {
                    if (_browserpvstat.ContainsKey(updatePVStatState.Browser))
                        _browserpvstat[updatePVStatState.Browser]++;
                    else
                        _browserpvstat.TryAdd(updatePVStatState.Browser, 1);
                }

                //更新操作系统统计
                if (shopConfig.IsStatOS == 1)
                {
                    if (_ospvstat.ContainsKey(updatePVStatState.OS))
                        _ospvstat[updatePVStatState.OS]++;
                    else
                        _ospvstat.TryAdd(updatePVStatState.OS, 1);
                }

                //更新位置统计
                if (shopConfig.IsStatRegion == 1)
                {
                    if (_regionpvstat.ContainsKey(updatePVStatState.RegionId))
                        _regionpvstat[updatePVStatState.RegionId]++;
                    else
                        _regionpvstat.TryAdd(updatePVStatState.RegionId, 1);
                }

                if (_lastupdatepvstatstime < (Environment.TickCount - BSPConfig.ShopConfig.UpdatePVStatTimespan * 1000 * 60))
                {
                    UpdatePVStat("week", DateTime.Now.ToString("yyyy-MM-dd") + DateTime.Now.Month.ToString("00") + ((int)DateTime.Now.DayOfWeek).ToString(), _weekpvstat);
                    UpdatePVStat("hour", DateTime.Now.ToString("yyyy-MM-dd") + DateTime.Now.Hour.ToString("00"), _hourpvstat);
                    UpdatePVStat("day", DateTime.Now.ToString("yyyy-MM-dd"), _daypvstat);
                    UpdatePVStat("month", DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00"), _monthpvstat);
                    UpdatePVStat("year", DateTime.Now.Year.ToString(), _yearpvstat);
                    UpdatePVStat("total", "guest", _guestpvstat);
                    UpdatePVStat("total", "member", _memberpvstat);
                    UpdatePVStat("total", "user", _userpvstat);

                    foreach (KeyValuePair<string, int> item in _browserpvstat)
                        UpdatePVStat("browser", item.Key, item.Value);
                    foreach (KeyValuePair<string, int> item in _ospvstat)
                        UpdatePVStat("os", item.Key, item.Value);
                    foreach (KeyValuePair<int, int> item in _regionpvstat)
                        UpdatePVStat("region", item.Key.ToString(), item.Value);

                    ResetPVStats();

                    _lastupdatepvstatstime = Environment.TickCount;
                }
            }
        }

        /// <summary>
        /// 重置pv统计
        /// </summary>
        private void ResetPVStats()
        {
            _userpvstat = _memberpvstat = _guestpvstat = _yearpvstat = _monthpvstat = _daypvstat = _hourpvstat = _weekpvstat = 0;
            _browserpvstat = new ConcurrentDictionary<string, int>();
            _ospvstat = new ConcurrentDictionary<string, int>();
            _regionpvstat = new ConcurrentDictionary<int, int>();
        }

        /// <summary>
        /// 更新pv统计
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="value">值</param>
        /// <param name="count">数量</param>
        private void UpdatePVStat(string category, string value, int count)
        {
            DbParameter[] parms = {
									GenerateInParam("@category",SqlDbType.Char,10, category),
									GenerateInParam("@value",SqlDbType.Char,20, value),
									GenerateInParam("@count",SqlDbType.Int,4, count)
			                       };
            string commandText = string.Format("UPDATE [{0}pvstats] SET [count]=[count]+@count WHERE [category]=@category AND [value]=@value",
                                                RDBSHelper.RDBSTablePre);
            if (RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms) < 1)
            {
                commandText = string.Format("INSERT INTO [{0}pvstats]([category],[value],[count]) VALUES(@category, @value, @count)",
                                             RDBSHelper.RDBSTablePre);
                RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            }
        }

        /// <summary>
        /// 获得省级区域统计
        /// </summary>
        /// <returns></returns>
        public DataTable GetProvinceRegionStat()
        {
            string commandText = string.Format("SELECT [temp3].[provinceid],[temp4].[name] AS [provincename],[temp3].[totalcount] FROM (SELECT [temp2].[parentid] AS [provinceid],SUM([temp1].[count]) AS [totalcount] FROM (SELECT [category],[value],[count] FROM [{0}pvstats] WHERE [category]='region') AS [temp1] LEFT JOIN [{0}regions] AS [temp2] ON [temp1].[value]=[temp2].[regionid] GROUP BY [temp2].[parentid]) AS [temp3] LEFT JOIN [{0}regions] AS [temp4] ON [temp4].[regionid]=[temp3].[provinceid]",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 获得市级区域统计
        /// </summary>
        /// <param name="provinceId">省id</param>
        /// <returns></returns>
        public DataTable GetCityRegionStat(int provinceId)
        {
            string commandText = string.Format("SELECT [temp3].[cityid],[temp4].[name] AS [cityname],[temp3].[totalcount] FROM (SELECT [temp2].[cityid],SUM([temp1].[count]) AS [totalcount] FROM (SELECT [category],[value],[count] FROM [{0}pvstats] WHERE [category]='region') AS [temp1] LEFT JOIN [{0}regions] AS [temp2] ON [temp1].[value]=[temp2].[regionid] WHERE [temp2].[provinceid]={1} GROUP BY [temp2].[cityid]) AS [temp3] LEFT JOIN [{0}regions] AS [temp4] ON [temp4].[regionid]=[temp3].[cityid]",
                                                RDBSHelper.RDBSTablePre,
                                                provinceId);
            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 获得区/县级区域统计
        /// </summary>
        /// <param name="cityId">市id</param>
        /// <returns></returns>
        public DataTable GetCountyRegionStat(int cityId)
        {
            string commandText = string.Format("SELECT [temp3].[regionid] AS [countyid],[temp4].[name] AS [countyname],[temp3].[totalcount] FROM (SELECT [temp2].[regionid],SUM([temp1].[count]) AS [totalcount] FROM (SELECT [category],[value],[count] FROM [{0}pvstats] WHERE [category]='region') AS [temp1] LEFT JOIN [{0}regions] AS [temp2] ON [temp1].[value]=[temp2].[regionid] WHERE [temp2].[cityid]={1} GROUP BY [temp2].[regionid]) AS [temp3] LEFT JOIN [{0}regions] AS [temp4] ON [temp4].[regionid]=[temp3].[regionid]",
                                                RDBSHelper.RDBSTablePre,
                                                cityId);
            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 获得PV统计列表
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public IDataReader GetPVStatList(string condition)
        {
            string commandText = string.Format("SELECT {1} FROM [{0}pvstats] WHERE {2}",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.PVSTATS,
                                                condition);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获得PV统计
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public IDataReader GetPVStatByCategoryAndValue(string category, string value)
        {
            DbParameter[] parms = {
									GenerateInParam("@category",SqlDbType.Char,10, category),
									GenerateInParam("@value",SqlDbType.Char,20, value)
			                       };
            string commandText = string.Format("SELECT {1} FROM [{0}pvstats] WHERE [category]=@category AND [value]=@value",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.PVSTATS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        #endregion

        #region 禁止IP

        /// <summary>
        /// 获得禁止的ip列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetBannedIPList()
        {
            DbParameter[] parms = {
									GenerateInParam("@nowtime",SqlDbType.DateTime,8, DateTime.Now)
			                       };
            string commandText = string.Format("SELECT [ip] FROM [{0}bannedips] WHERE [liftbantime]>@nowtime",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得禁止的ip
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public IDataReader GetBannedIPById(int id)
        {
            DbParameter[] parms = {
                                     GenerateInParam("@id", SqlDbType.Int, 4, id)
                                   };
            string commandText = string.Format("SELECT {1} FROM [{0}bannedips] WHERE [id]=@id",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.BANNEDIPS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得禁止IP的id
        /// </summary>
        /// <param name="ip">ip</param>
        /// <returns></returns>
        public int GetBannedIPIdByIP(string ip)
        {
            string commandText = string.Format("SELECT TOP 1 [id] FROM [{0}bannedips] WHERE [ip] LIKE '{1}%'",
                                                RDBSHelper.RDBSTablePre,
                                                SecureHelper.IsSafeSqlString(ip) ? ip : "");
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText), -1);
        }

        /// <summary>
        /// 添加禁止的ip
        /// </summary>
        public void AddBannedIP(BannedIPInfo bannedIPInfo)
        {
            DbParameter[] parms = {
									GenerateInParam("@ip",SqlDbType.VarChar,15, bannedIPInfo.IP),
									GenerateInParam("@liftbantime",SqlDbType.DateTime,8, bannedIPInfo.LiftBanTime)
			                       };
            string commandText = string.Format("INSERT INTO [{0}bannedips]([ip],[liftbantime]) VALUES(@ip,@liftbantime)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新禁止的ip
        /// </summary>
        public void UpdateBannedIP(BannedIPInfo bannedIPInfo)
        {
            DbParameter[] parms = {
									GenerateInParam("@ip",SqlDbType.VarChar,15, bannedIPInfo.IP),
									GenerateInParam("@liftbantime",SqlDbType.DateTime,8, bannedIPInfo.LiftBanTime),
									GenerateInParam("@id",SqlDbType.Int,4, bannedIPInfo.Id)
			                       };
            string commandText = string.Format("UPDATE [{0}bannedips] SET [ip]=@ip,[liftbantime]=@liftbantime WHERE [id]=@id",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除禁止的ip
        /// </summary>
        /// <param name="idList">id列表</param>
        public void DeleteBannedIPById(string idList)
        {
            string commandText = String.Format("DELETE FROM [{0}bannedips] WHERE [id] IN ({1})",
                                                RDBSHelper.RDBSTablePre,
                                                idList);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 后台获得禁止的ip列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="ip">ip</param>
        /// <returns></returns>
        public DataTable AdminGetBannedIPList(int pageSize, int pageNumber, string ip)
        {
            string commandText;
            if (pageNumber == 1)
            {
                if (string.IsNullOrWhiteSpace(ip) || !SecureHelper.IsSafeSqlString(ip))
                    commandText = string.Format("SELECT TOP {0} {2} FROM [{1}bannedips] ORDER BY [id] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.BANNEDIPS);

                else
                    commandText = string.Format("SELECT TOP {0} {3} FROM [{1}bannedips] WHERE [ip] LIKE '{2}%' ORDER BY [id] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                ip,
                                                RDBSFields.BANNEDIPS);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(ip) || !SecureHelper.IsSafeSqlString(ip))
                    commandText = string.Format("SELECT TOP {0} {3} FROM [{1}bannedips] WHERE [id] NOT IN (SELECT TOP {2} [id] FROM [{1}bannedips] ORDER BY [id] DESC) ORDER BY [id] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize,
                                                RDBSFields.BANNEDIPS);
                else
                    commandText = string.Format("SELECT TOP {0} {4} FROM [{1}bannedips] WHERE [id] NOT IN (SELECT TOP {2} [id] FROM [{1}bannedips] WHERE [ip] LIKE '{3}%' ORDER BY [id] DESC) AND [ip] LIKE '{3}%' ORDER BY [id] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize,
                                                ip,
                                                RDBSFields.BANNEDIPS);
            }

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 后台获得禁止的ip数量
        /// </summary>
        /// <param name="ip">ip</param>
        /// <returns></returns>
        public int AdminGetBannedIPCount(string ip)
        {
            string commandText;
            if (string.IsNullOrWhiteSpace(ip) || !SecureHelper.IsSafeSqlString(ip))
                commandText = string.Format("SELECT COUNT([id]) FROM [{0}bannedips]", RDBSHelper.RDBSTablePre);
            else
                commandText = string.Format("SELECT COUNT([id]) FROM [{0}bannedips] WHERE [ip] LIKE '{1}%'", RDBSHelper.RDBSTablePre, ip);

            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        #endregion

        #region 筛选词

        /// <summary>
        /// 获得筛选词列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetFilterWordList()
        {
            string commandText = string.Format("SELECT {1} FROM [{0}filterwords]",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.FILTERWORDS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 添加筛选词
        /// </summary>
        public void AddFilterWord(FilterWordInfo filterWordInfo)
        {
            DbParameter[] parms = { 
                                    GenerateInParam("@match", SqlDbType.NVarChar, 250, filterWordInfo.Match),
                                    GenerateInParam("@replace", SqlDbType.NVarChar, 250, filterWordInfo.Replace)
                                   };
            string commandText = string.Format("INSERT INTO [{0}filterwords]([match],[replace]) VALUES(@match,@replace)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新筛选词
        /// </summary>
        public void UpdateFilterWord(FilterWordInfo filterWordInfo)
        {
            DbParameter[] parms = { 
                                    GenerateInParam("@match", SqlDbType.NVarChar, 250, filterWordInfo.Match),
                                    GenerateInParam("@replace", SqlDbType.NVarChar, 250, filterWordInfo.Replace),
                                    GenerateInParam("@id", SqlDbType.Int, 4, filterWordInfo.Id)
                                   };
            string commandText = string.Format("UPDATE [{0}filterwords] SET [match]=@match,[replace]=@replace WHERE [id]=@id",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除筛选词
        /// </summary>
        /// <param name="idList">id列表</param>
        public void DeleteFilterWordById(string idList)
        {
            string commandText = string.Format("DELETE FROM [{0}filterwords] WHERE [id] IN ({1}) ",
                                                RDBSHelper.RDBSTablePre,
                                                idList);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        #endregion

        #region 登陆失败日志

        /// <summary>
        /// 获得登陆失败日志
        /// </summary>
        /// <param name="loginIP">登陆IP</param>
        /// <returns></returns>
        public IDataReader GetLoginFailLogByIP(long loginIP)
        {
            DbParameter[] parms = {
									GenerateInParam("@loginip",SqlDbType.BigInt,8, loginIP)
			                      };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getloginfaillogbyip", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 增加登陆失败次数
        /// </summary>
        /// <param name="loginIP">登陆IP</param>
        /// <param name="loginTime">登陆时间</param>
        public void AddLoginFailTimes(long loginIP, DateTime loginTime)
        {
            DbParameter[] parms = {
									GenerateInParam("@loginip",SqlDbType.BigInt,8, loginIP),
									GenerateInParam("@lastlogintime",SqlDbType.DateTime,8, loginTime)
			                        };
            string commandText = string.Format("UPDATE [{0}loginfaillogs] SET [failtimes]=[failtimes]+1,[lastlogintime]=@lastlogintime WHERE [loginip]=@loginip",
                                                RDBSHelper.RDBSTablePre);
            if (RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms) < 1)
            {
                commandText = string.Format("INSERT INTO [{0}loginfaillogs]([loginip],[failtimes],[lastlogintime]) VALUES(@loginip,1,@lastlogintime)",
                                             RDBSHelper.RDBSTablePre);
                RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
            }
        }
       
        /// <summary>
        /// 增加登陆日志
        /// </summary>
        /// <param name="loginIP">登陆IP</param>
        /// <param name="loginTime">登陆时间</param>
        public void AddLogin(string loginIP, int uid, DateTime loginTime, string ipName, int type, string remark)
        { 
            string commandText = string.Format("INSERT INTO [{0}userslog]([uid],[createtime],[remark],[type],[ip] ,[ipname]) VALUES({1},'{2}','{3}',{4},'{5}','{6}')",
                                             RDBSHelper.RDBSTablePre,uid,loginTime.ToString("yyyy-MM-dd HH:mm:ss:fff"),remark,type,loginIP,ipName);
                RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, null); 
        }
        /// <summary>
        /// 删除登陆失败日志
        /// </summary>
        /// <param name="loginIP">登陆IP</param>
        public void DeleteLoginFailLogByIP(long loginIP)
        {
            DbParameter[] parms = {
									GenerateInParam("@loginip",SqlDbType.BigInt,8, loginIP)
			                       };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}deleteloginfaillogbyip", RDBSHelper.RDBSTablePre),
                                       parms);
        } 
        public DataTable GetLoginList(int pageindex, int pageSize, string sqlwhere = "")
        {
            DbParameter[] parms =
             {
                GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                GenerateInParam("@pageindex", SqlDbType.Int, 4, pageindex)
            };
            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
  drop table #list

SELECT ROW_NUMBER() over(order by autoid desc) id ,a.*
  into  #list
  FROM owzx_userslog a where  1=1 
  {0}

declare @total int
select @total=(select count(1)  from #list)

if(@pagesize=-1)
begin
    select *,@total TotalCount from #list
end
else
begin
    select *,@total TotalCount from #list where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end

end try
begin catch
select ERROR_MESSAGE() state
end catch

", sqlwhere
            );
            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, parms)[0];
        }
        #endregion

        #region 管理员操作日志

        /// <summary>
        /// 创建管理员操作日志
        /// </summary>
        public void CreateAdminOperateLog(AdminOperateLogInfo adminOperateLogInfo)
        {
            DbParameter[] parms = { 
                                        GenerateInParam("@uid", SqlDbType.Int, 4, adminOperateLogInfo.Uid),
                                        GenerateInParam("@nickname", SqlDbType.NVarChar, 20, adminOperateLogInfo.NickName),
                                        GenerateInParam("@admingid", SqlDbType.SmallInt, 2, adminOperateLogInfo.AdminGid),
                                        GenerateInParam("@admingtitle", SqlDbType.NVarChar, 50, adminOperateLogInfo.AdminGTitle),
                                        GenerateInParam("@operation", SqlDbType.NVarChar, 100, adminOperateLogInfo.Operation),
                                        GenerateInParam("@description", SqlDbType.NVarChar, 200, adminOperateLogInfo.Description),
                                        GenerateInParam("@ip", SqlDbType.VarChar, 15, adminOperateLogInfo.IP),
                                        GenerateInParam("@operatetime", SqlDbType.DateTime, 8, adminOperateLogInfo.OperateTime)
                                    };
            string commandText = string.Format("INSERT INTO [{0}adminoperatelogs]([uid],[nickname],[admingid],[admingtitle],[operation],[description],[ip],[operatetime]) VALUES(@uid,@nickname,@admingid,@admingtitle,@operation,@description,@ip,@operatetime)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得管理员操作日志列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public IDataReader GetAdminOperateLogList(int pageSize, int pageNumber, string condition)
        {
            bool noCondition = string.IsNullOrWhiteSpace(condition);
            string commandText;
            if (pageNumber == 1)
            {
                if (noCondition)
                    commandText = string.Format("SELECT TOP {0} {2} FROM [{1}adminoperatelogs] ORDER BY [logid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.ADMIN_OPERATELOGS);

                else
                    commandText = string.Format("SELECT TOP {0} {3} FROM [{1}adminoperatelogs] WHERE {2} ORDER BY [logid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                condition,
                                                RDBSFields.ADMIN_OPERATELOGS);
            }
            else
            {
                if (noCondition)
                    commandText = string.Format("SELECT TOP {0} {3} FROM [{1}adminoperatelogs] WHERE [logid] < (SELECT MIN([logid]) FROM (SELECT TOP {2} [logid] FROM [{1}adminoperatelogs] ORDER BY [logid] DESC) AS [temp]) ORDER BY [logid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize,
                                                RDBSFields.ADMIN_OPERATELOGS);
                else
                    commandText = string.Format("SELECT TOP {0} {4} FROM [{1}adminoperatelogs] WHERE [logid] < (SELECT MIN([logid]) FROM (SELECT TOP {2} [logid] FROM [{1}adminoperatelogs] WHERE {3} ORDER BY [logid] DESC) AS [temp]) AND {3} ORDER BY [logid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize,
                                                condition,
                                                RDBSFields.ADMIN_OPERATELOGS);
            }

            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获得管理员操作日志列表条件
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="operation">操作行为</param>
        /// <param name="startTime">操作开始时间</param>
        /// <param name="endTime">操作结束时间</param>
        /// <returns></returns>
        public string GetAdminOperateLogListCondition(int uid, string operation, string startTime, string endTime)
        {
            StringBuilder condition = new StringBuilder();
            if (uid > 0)
                condition.AppendFormat(" AND [uid] = {0} ", uid);
            if (!string.IsNullOrWhiteSpace(operation) && SecureHelper.IsSafeSqlString(operation))
                condition.AppendFormat(" AND [operation] like '{0}%' ", operation);
            if (!string.IsNullOrEmpty(startTime))
                condition.AppendFormat(" AND [operatetime] >= '{0}' ", TypeHelper.StringToDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss"));
            if (!string.IsNullOrEmpty(endTime))
                condition.AppendFormat(" AND [operatetime] <= '{0}' ", TypeHelper.StringToDateTime(endTime).ToString("yyyy-MM-dd HH:mm:ss"));

            return condition.Length > 0 ? condition.Remove(0, 4).ToString() : "";
        }

        /// <summary>
        /// 获得管理员操作日志数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public int GetAdminOperateLogCount(string condition)
        {
            string commandText;
            if (string.IsNullOrWhiteSpace(condition))
                commandText = string.Format("SELECT COUNT(logid) FROM [{0}adminoperatelogs]", RDBSHelper.RDBSTablePre);
            else
                commandText = string.Format("SELECT COUNT(logid) FROM [{0}adminoperatelogs] WHERE {1}", RDBSHelper.RDBSTablePre, condition);

            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText), 0);
        }

        /// <summary>
        /// 删除管理员操作日志
        /// </summary>
        /// <param name="logIdList">日志id</param>
        public void DeleteAdminOperateLogById(string logIdList)
        {
            string commandText = string.Format("DELETE FROM [{0}adminoperatelogs] WHERE [logid] IN ({1}) ",
                                               RDBSHelper.RDBSTablePre,
                                               logIdList);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        #endregion

        #region 导航栏

        /// <summary>
        /// 获得导航栏列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetNavList()
        {
            string commandText = string.Format("SELECT {1} FROM [{0}navs] ORDER BY [displayorder] DESC",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.NAVS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 创建导航栏
        /// </summary>
        public void CreateNav(NavInfo navInfo)
        {
            DbParameter[] param = {
                                         GenerateInParam("@pid",SqlDbType.Int, 4, navInfo.Pid),
                                         GenerateInParam("@layer",SqlDbType.Int, 4, navInfo.Layer),
                                         GenerateInParam("@name",SqlDbType.NChar, 50, navInfo.Name),
                                         GenerateInParam("@title",SqlDbType.NChar, 250, navInfo.Title),
                                         GenerateInParam("@url",SqlDbType.NChar, 250, navInfo.Url),
                                         GenerateInParam("@target",SqlDbType.Int, 4, navInfo.Target),
                                         GenerateInParam("@displayorder",SqlDbType.Int,4,navInfo.DisplayOrder)
                                       };
            string commandText = String.Format("INSERT INTO [{0}navs]([pid],[layer],[name],[title],[url],[target],[displayorder]) VALUES(@pid,@layer,@name,@title,@url,@target,@displayorder)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, param);
        }

        /// <summary>
        /// 删除导航栏
        /// </summary>
        /// <param name="id">导航栏id</param>
        public void DeleteNavById(int id)
        {
            DbParameter[] param = {
                                    GenerateInParam("@id",SqlDbType.Int, 4, id)
                                   };
            string commandText = String.Format("DELETE FROM [{0}navs] WHERE [id]=@id",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, param);
        }

        /// <summary>
        /// 更新导航栏
        /// </summary>
        public void UpdateNav(NavInfo navInfo)
        {
            DbParameter[] param = {
                                         GenerateInParam("@pid",SqlDbType.Int, 4, navInfo.Pid),
                                         GenerateInParam("@layer",SqlDbType.Int, 4, navInfo.Layer),
                                         GenerateInParam("@name",SqlDbType.NChar, 50, navInfo.Name),
                                         GenerateInParam("@title",SqlDbType.NChar, 250, navInfo.Title),
                                         GenerateInParam("@url",SqlDbType.NChar, 250, navInfo.Url),
                                         GenerateInParam("@target",SqlDbType.Int, 4, navInfo.Target),
                                         GenerateInParam("@displayorder",SqlDbType.Int,4,navInfo.DisplayOrder),
                                         GenerateInParam("@id",SqlDbType.Int, 4, navInfo.Id)
                                       };
            string commandText = String.Format("UPDATE [{0}navs] SET [pid]=@pid,[layer]=@layer,[name]=@name,[title]=@title,[url]=@url,[target]=@target,[displayorder]=@displayorder WHERE [id]=@id",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, param);
        }

        #endregion

        #region 广告位置

        /// <summary>
        /// 获得广告位置列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        public IDataReader GetAdvertPositionList(int pageSize, int pageNumber)
        {
            string commandText;
            if (pageNumber == 1)
            {
                commandText = string.Format("SELECT TOP {2} {1} FROM [{0}advertpositions] ORDER BY [displayorder] DESC,[adposid] DESC",
                                             RDBSHelper.RDBSTablePre,
                                             RDBSFields.ADVERT_POSITIONS,
                                             pageSize);
            }
            else
            {
                commandText = string.Format("SELECT TOP {2} {1} FROM [{0}advertpositions] WHERE [adposid] < (SELECT MIN([adposid]) FROM (SELECT TOP {3} [adposid] FROM [{0}advertpositions] ORDER BY [displayorder] DESC,[adposid] DESC) AS [temp]) ORDER BY [displayorder] DESC,[adposid] DESC",
                                             RDBSHelper.RDBSTablePre,
                                             RDBSFields.ADVERT_POSITIONS,
                                             pageSize,
                                             (pageNumber - 1) * pageSize);
            }

            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获得广告位置数量
        /// </summary>
        /// <returns></returns>
        public int GetAdvertPositionCount()
        {
            string commandText = string.Format("SELECT COUNT(adposid) FROM [{0}advertpositions]", RDBSHelper.RDBSTablePre);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        /// <summary>
        /// 获得全部广告位置
        /// </summary>
        /// <returns></returns>
        public IDataReader GetAllAdvertPosition()
        {
            string commandText = string.Format("SELECT {1} FROM [{0}advertpositions] ORDER BY [displayorder] DESC,[adposid] DESC",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.ADVERT_POSITIONS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 创建广告位置
        /// </summary>
        public void CreateAdvertPosition(AdvertPositionInfo advertPositionInfo)
        {
            DbParameter[] param = {
                                     GenerateInParam("@displayorder",SqlDbType.Int, 4, advertPositionInfo.DisplayOrder),
                                     GenerateInParam("@title",SqlDbType.NChar, 50, advertPositionInfo.Title),
                                     GenerateInParam("@description",SqlDbType.NChar, 150, advertPositionInfo.Description)
                                   };
            string commandText = String.Format("INSERT INTO [{0}advertpositions]([displayorder],[title],[description]) VALUES(@displayorder,@title,@description)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, param);
        }

        /// <summary>
        /// 更新广告位置
        /// </summary>
        public void UpdateAdvertPosition(AdvertPositionInfo advertPositionInfo)
        {
            DbParameter[] param = {
                                     GenerateInParam("@displayorder",SqlDbType.Int, 4, advertPositionInfo.DisplayOrder),
                                     GenerateInParam("@title",SqlDbType.NChar, 50, advertPositionInfo.Title),
                                     GenerateInParam("@description",SqlDbType.NChar, 150, advertPositionInfo.Description),
                                     GenerateInParam("@adposid",SqlDbType.SmallInt, 2, advertPositionInfo.AdPosId)
                                   };
            string commandText = String.Format("UPDATE [{0}advertpositions] SET [displayorder]=@displayorder,[title]=@title,[description]=@description WHERE [adposid]=@adposid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, param);
        }

        /// <summary>
        /// 获得广告位置
        /// </summary>
        /// <param name="adPosId">广告位置id</param>
        /// <returns></returns>
        public IDataReader GetAdvertPositionById(int adPosId)
        {
            DbParameter[] param = {
                                    GenerateInParam("@adposid",SqlDbType.SmallInt, 2, adPosId)
                                   };
            string commandText = string.Format("SELECT {1} FROM [{0}advertpositions] WHERE [adposid]=@adposid",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.ADVERT_POSITIONS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText, param);
        }

        /// <summary>
        /// 删除广告位置
        /// </summary>
        /// <param name="adPosId">广告位置id</param>
        public void DeleteAdvertPositionById(int adPosId)
        {
            DbParameter[] param = {
                                    GenerateInParam("@adposid",SqlDbType.SmallInt, 2, adPosId)
                                   };
            string commandText = String.Format("DELETE FROM [{0}adverts] WHERE [adposid]=@adposid;DELETE FROM [{0}advertpositions] WHERE [adposid]=@adposid;",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, param);
        }

        #endregion

        #region 广告

        /// <summary>
        /// 创建广告
        /// </summary>
        public void CreateAdvert(AdvertInfo advertInfo)
        {
            DbParameter[] param = {
                                     GenerateInParam("@clickcount",SqlDbType.Int, 4, advertInfo.ClickCount),
                                     GenerateInParam("@adposid",SqlDbType.SmallInt, 2, advertInfo.AdPosId),
                                     GenerateInParam("@state",SqlDbType.TinyInt, 1, advertInfo.State),
                                     GenerateInParam("@starttime",SqlDbType.DateTime, 8, advertInfo.StartTime),
                                     GenerateInParam("@endtime",SqlDbType.DateTime, 8, advertInfo.EndTime),
                                     GenerateInParam("@displayorder",SqlDbType.Int, 4, advertInfo.DisplayOrder),
                                     GenerateInParam("@title",SqlDbType.NVarChar, 50, advertInfo.Title),
                                     GenerateInParam("@image",SqlDbType.NVarChar, 100, advertInfo.Image),
                                     GenerateInParam("@url",SqlDbType.NVarChar, 200, advertInfo.Url),
                                     GenerateInParam("@extfield1",SqlDbType.NVarChar, 500, advertInfo.ExtField1),
                                     GenerateInParam("@extfield2",SqlDbType.NVarChar, 500, advertInfo.ExtField2),
                                     GenerateInParam("@extfield3",SqlDbType.NVarChar, 500, advertInfo.ExtField3),
                                     GenerateInParam("@extfield4",SqlDbType.NVarChar, 500, advertInfo.ExtField4),
                                     GenerateInParam("@extfield5",SqlDbType.NVarChar, 500, advertInfo.ExtField5)
                                   };
            string commandText = String.Format("INSERT INTO [{0}adverts]([clickcount],[adposid],[state],[starttime],[endtime],[displayorder],[title],[image],[url],[extfield1],[extfield2],[extfield3],[extfield4],[extfield5]) VALUES(@clickcount,@adposid,@state,@starttime,@endtime,@displayorder,@title,@image,@url,@extfield1,@extfield2,@extfield3,@extfield4,@extfield5)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, param);
        }

        /// <summary>
        /// 更新广告
        /// </summary>
        public void UpdateAdvert(AdvertInfo advertInfo)
        {
            DbParameter[] param = {
                                     GenerateInParam("@clickcount",SqlDbType.Int, 4, advertInfo.ClickCount),
                                     GenerateInParam("@adposid",SqlDbType.SmallInt, 2, advertInfo.AdPosId),
                                     GenerateInParam("@state",SqlDbType.TinyInt, 1, advertInfo.State),
                                     GenerateInParam("@starttime",SqlDbType.DateTime, 8, advertInfo.StartTime),
                                     GenerateInParam("@endtime",SqlDbType.DateTime, 8, advertInfo.EndTime),
                                     GenerateInParam("@displayorder",SqlDbType.Int, 4, advertInfo.DisplayOrder),
                                     GenerateInParam("@title",SqlDbType.NVarChar, 50, advertInfo.Title),
                                     GenerateInParam("@image",SqlDbType.NVarChar, 100, advertInfo.Image),
                                     GenerateInParam("@url",SqlDbType.NVarChar, 200, advertInfo.Url),
                                     GenerateInParam("@extfield1",SqlDbType.NVarChar, 500, advertInfo.ExtField1),
                                     GenerateInParam("@extfield2",SqlDbType.NVarChar, 500, advertInfo.ExtField2),
                                     GenerateInParam("@extfield3",SqlDbType.NVarChar, 500, advertInfo.ExtField3),
                                     GenerateInParam("@extfield4",SqlDbType.NVarChar, 500, advertInfo.ExtField4),
                                     GenerateInParam("@extfield5",SqlDbType.NVarChar, 500, advertInfo.ExtField5),
                                     GenerateInParam("@adid",SqlDbType.Int, 4, advertInfo.AdId)
                                   };
            string commandText = String.Format("UPDATE [{0}adverts] SET [clickcount]=@clickcount,[adposid]=@adposid,[state]=@state,[starttime]=@starttime,[endtime]=@endtime,[displayorder]=@displayorder,[title]=@title,[image]=@image,[url]=@url,[extfield1]=@extfield1,[extfield2]=@extfield2,[extfield3]=@extfield3,[extfield4]=@extfield4,[extfield5]=@extfield5 WHERE [adid]=@adid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, param);
        }

        /// <summary>
        /// 删除广告
        /// </summary>
        /// <param name="adId">广告id</param>
        public void DeleteAdvertById(int adId)
        {
            DbParameter[] param = {
                                     GenerateInParam("@adid",SqlDbType.Int, 4, adId)
                                   };
            string commandText = String.Format("DELETE FROM [{0}adverts] WHERE [adid]=@adid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, param);
        }

        /// <summary>
        /// 后台获得广告
        /// </summary>
        /// <param name="adId">广告id</param>
        /// <returns></returns>
        public IDataReader AdminGetAdvertById(int adId)
        {
            DbParameter[] param = {
                                     GenerateInParam("@adid",SqlDbType.Int, 4, adId)
                                   };
            string commandText = string.Format("SELECT {1} FROM [{0}adverts] WHERE [adid]=@adid",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.ADVERTS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText, param);
        }

        /// <summary>
        /// 后台获得广告列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="adPosId">广告位置id</param>
        /// <returns></returns>
        public DataTable AdminGetAdvertList(int pageSize, int pageNumber, int adPosId)
        {
            string commandText;
            if (pageNumber == 1)
            {
                if (adPosId < 1)
                    commandText = string.Format(@"SELECT [temp1].[adid],[temp1].[clickcount],[temp1].[adposid],[temp1].[title] AS [atitle],
[temp1].[starttime],[temp1].[endtime],[temp1].[state],[temp1].[displayorder],[temp2].[title] AS [aptitle] ,image ,url
FROM (SELECT TOP {0} [adid],[clickcount],[adposid],[title],[starttime],[endtime],[state],[displayorder],image,url 
FROM [{1}adverts] ORDER BY [adposid] DESC,[displayorder] DESC,[adid] DESC) AS [temp1] 
LEFT JOIN [{1}advertpositions] AS [temp2] ON [temp1].[adposid]=[temp2].[adposid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre);

                else
                    commandText = string.Format(@"SELECT [temp1].[adid],[temp1].[clickcount],[temp1].[adposid],[temp1].[title] AS [atitle],[temp1].[starttime],
[temp1].[endtime],[temp1].[state],[temp1].[displayorder],[temp2].[title] AS [aptitle] ,image ,url
FROM (SELECT TOP {0} [adid],[clickcount],[adposid],[title],[starttime],[endtime],[state],[displayorder] ,image ,url
FROM [{1}adverts] WHERE [adposid]={2} ORDER BY [adposid] DESC,[displayorder] DESC,[adid] DESC) AS [temp1] LEFT JOIN [{1}advertpositions] AS [temp2] ON [temp1].[adposid]=[temp2].[adposid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                adPosId);
            }
            else
            {
                if (adPosId < 1)
                    commandText = string.Format(@"SELECT [temp1].[adid],[temp1].[clickcount],[temp1].[adposid],[temp1].[title] AS [atitle],[temp1].[starttime],
[temp1].[endtime],[temp1].[state],[temp1].[displayorder],[temp2].[title] AS [aptitle] ,image ,url
FROM (SELECT TOP {0} [adid],[clickcount],[adposid],[title],[starttime],[endtime],[state],[displayorder],image,url  
FROM [{1}adverts] WHERE [adid] NOT IN (SELECT TOP {2} [adid] FROM [{1}adverts] ORDER BY [adposid] DESC,[displayorder] DESC,[adid] DESC) ORDER BY [adposid] DESC,[displayorder] DESC,[adid] DESC) AS [temp1] LEFT JOIN [{1}advertpositions] AS [temp2] ON [temp1].[adposid]=[temp2].[adposid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize);
                else
                    commandText = string.Format(@"SELECT [temp1].[adid],[temp1].[clickcount],[temp1].[adposid],[temp1].[title] AS [atitle],[temp1].[starttime],
[temp1].[endtime],[temp1].[state],[temp1].[displayorder],[temp2].[title] AS [aptitle] ,image ,url
FROM (SELECT TOP {0} [adid],[clickcount],[adposid],[title],[starttime],[endtime],[state],[displayorder] ,image ,url
FROM [{1}adverts] WHERE [adposid]={3} AND [adid] NOT IN (SELECT TOP {2} [adid] FROM [{1}adverts] WHERE [adposid]={3} ORDER BY [adposid] DESC,[displayorder] DESC,[adid] DESC) ORDER BY [adposid] DESC,[displayorder] DESC,[adid] DESC) AS [temp1] LEFT JOIN [{1}advertpositions] AS [temp2] ON [temp1].[adposid]=[temp2].[adposid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize,
                                                adPosId);
            }

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 后台获得广告数量
        /// </summary>
        /// <param name="adPosId">广告位置id</param>
        /// <returns></returns>
        public int AdminGetAdvertCount(int adPosId)
        {
            string commandText;
            if (adPosId < 1)
                commandText = string.Format("SELECT COUNT(adid) FROM [{0}adverts]", RDBSHelper.RDBSTablePre);
            else
                commandText = string.Format("SELECT COUNT(adid) FROM [{0}adverts] WHERE [adposid]={1}", RDBSHelper.RDBSTablePre, adPosId);

            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        /// <summary>
        /// 获得广告列表
        /// </summary>
        /// <param name="adPosId">广告位置id</param>
        /// <param name="nowTime">当前时间</param>
        /// <returns></returns>
        public IDataReader GetAdvertList(int adPosId, DateTime nowTime)
        {
            DbParameter[] param = {
                                     GenerateInParam("@adposid",SqlDbType.SmallInt, 2, adPosId),
                                     GenerateInParam("@nowtime",SqlDbType.DateTime, 8, nowTime)
                                   };
            string commandText = string.Format("SELECT {0} FROM [{1}adverts] WHERE [adposid]=@adposid AND [state]=1 AND [starttime]<=@nowtime AND [endtime]>@nowtime ORDER BY [displayorder] DESC,[adid] DESC",
                                                RDBSFields.ADVERTS,
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText, param);
        }

        #endregion

        #region 新闻类型

        /// <summary>
        /// 创建新闻类型
        /// </summary>
        public void CreateNewsType(NewsTypeInfo newsTypeInfo)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@name", SqlDbType.NChar, 60, newsTypeInfo.Name),
                                    GenerateInParam("@displayorder", SqlDbType.Int,4,newsTypeInfo.DisplayOrder)
                                    };
            string commandText = string.Format("INSERT INTO [{0}newstypes]([name],[displayorder]) VALUES(@name,@displayorder)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除新闻类型
        /// </summary>
        /// <param name="newsTypeId">新闻类型id</param>
        public void DeleteNewsTypeById(int newsTypeId)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@newstypeid", SqlDbType.SmallInt,2,newsTypeId)
                                   };
            string commandText = string.Format("DELETE FROM [{0}news] WHERE [newstypeid]=@newstypeid;DELETE FROM [{0}newstypes] WHERE [newstypeid]=@newstypeid;",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新新闻类型
        /// </summary>
        public void UpdateNewsType(NewsTypeInfo newsTypeInfo)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@name", SqlDbType.NChar, 60, newsTypeInfo.Name),
                                    GenerateInParam("@displayorder", SqlDbType.Int,4,newsTypeInfo.DisplayOrder),
                                    GenerateInParam("@newstypeid", SqlDbType.SmallInt,2,newsTypeInfo.NewsTypeId)
                                    };
            string commandText = string.Format("UPDATE [{0}newstypes] SET [name]=@name,[displayorder]=@displayorder WHERE [newstypeid]=@newstypeid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得新闻类型列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetNewsTypeList()
        {
            string commandText = string.Format("SELECT {1} FROM [{0}newstypes] ORDER BY [displayorder] DESC",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.NEWS_TYPES);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        #endregion

        #region 新闻

        /// <summary>
        /// 创建新闻
        /// </summary>
        public void CreateNews(NewsInfo newsInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@newstypeid", SqlDbType.SmallInt, 2, newsInfo.NewsTypeId),
                                        GenerateInParam("@isshow", SqlDbType.TinyInt, 1, newsInfo.IsShow),
                                        GenerateInParam("@istop", SqlDbType.TinyInt, 1, newsInfo.IsTop),
                                        GenerateInParam("@ishome", SqlDbType.TinyInt, 1, newsInfo.IsHome),
                                        GenerateInParam("@displayorder", SqlDbType.Int,4,newsInfo.DisplayOrder),
                                        GenerateInParam("@addtime", SqlDbType.DateTime,8,newsInfo.AddTime),
                                        GenerateInParam("@title", SqlDbType.NVarChar,100,newsInfo.Title),
                                        GenerateInParam("@url", SqlDbType.NVarChar,200,newsInfo.Url),
                                        GenerateInParam("@body", SqlDbType.NText, 0, newsInfo.Body)
                                    };
            string commandText = string.Format("INSERT INTO [{0}news]([newstypeid],[isshow],[istop],[ishome],[displayorder],[addtime],[title],[url],[body]) VALUES(@newstypeid,@isshow,@istop,@ishome,@displayorder,@addtime,@title,@url,@body)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除新闻
        /// </summary>
        /// <param name="newsIdList">新闻id列表</param>
        public void DeleteNewsById(string newsIdList)
        {
            string commandText = string.Format("DELETE FROM [{0}news] WHERE [newsid] IN ({1})",
                                                RDBSHelper.RDBSTablePre,
                                                newsIdList);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        /// <summary>
        /// 更新新闻
        /// </summary>
        public bool UpdateNews(NewsInfo newsInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@newstypeid", SqlDbType.SmallInt, 2, newsInfo.NewsTypeId),
                                        GenerateInParam("@isshow", SqlDbType.TinyInt, 1, newsInfo.IsShow),
                                        GenerateInParam("@istop", SqlDbType.TinyInt, 1, newsInfo.IsTop),
                                        GenerateInParam("@ishome", SqlDbType.TinyInt, 1, newsInfo.IsHome),
                                        GenerateInParam("@displayorder", SqlDbType.Int,4,newsInfo.DisplayOrder),
                                        GenerateInParam("@addtime", SqlDbType.DateTime,8,newsInfo.AddTime),
                                        GenerateInParam("@title", SqlDbType.NVarChar,100,newsInfo.Title),
                                        GenerateInParam("@url", SqlDbType.NVarChar,200,newsInfo.Url),
                                        GenerateInParam("@body", SqlDbType.NText, 0, newsInfo.Body),
                                        GenerateInParam("@newsid", SqlDbType.Int, 4, newsInfo.NewsId),
                                    };
            string commandText = string.Format("UPDATE [{0}news] SET [newstypeid]=@newstypeid,[isshow]=@isshow,[istop]=@istop,[ishome]=@ishome,[displayorder]=@displayorder,[addtime]=@addtime,[title]=@title,[url]=@url,[body]=@body WHERE [newsid]=@newsid",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms) == 1 ? true : false;
        }

        /// <summary>
        /// 获得新闻
        /// </summary>
        /// <param name="newsId">新闻id</param>
        /// <returns></returns>
        public IDataReader GetNewsById(int newsId)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@newsid", SqlDbType.Int, 4, newsId)    
                                   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getnewsbyid", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 后台获得新闻
        /// </summary>
        /// <param name="newsId">新闻id</param>
        /// <returns></returns>
        public IDataReader AdminGetNewsById(int newsId)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@newsid", SqlDbType.Int, 4, newsId)    
                                   };
            string commandText = string.Format("SELECT {1} FROM [{0}news] WHERE [newsid]=@newsid",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.NEWS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 后台获得新闻列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public DataTable AdminGetNewsList(int pageSize, int pageNumber, string condition)
        {
            bool noCondition = string.IsNullOrWhiteSpace(condition);
            string commandText;
            if (pageNumber == 1)
            {
                if (noCondition)
                    commandText = string.Format("SELECT [temp1].[newsid],[temp1].[newstypeid],[temp1].[isshow],[temp1].[istop],[temp1].[ishome],[temp1].[displayorder],[temp1].[addtime],[temp1].[title],[temp2].[name] FROM (SELECT TOP {0} [newsid],[newstypeid],[isshow],[istop],[ishome],[displayorder],[addtime],[title] FROM [{1}news] ORDER BY [displayorder] DESC,[newsid] DESC) AS [temp1] LEFT JOIN [{1}newstypes] AS [temp2] ON [temp1].[newstypeid]=[temp2].[newstypeid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre);

                else
                    commandText = string.Format("SELECT [temp1].[newsid],[temp1].[newstypeid],[temp1].[isshow],[temp1].[istop],[temp1].[ishome],[temp1].[displayorder],[temp1].[addtime],[temp1].[title],[temp2].[name] FROM (SELECT TOP {0} [newsid],[newstypeid],[isshow],[istop],[ishome],[displayorder],[addtime],[title] FROM [{1}news] WHERE {2} ORDER BY [displayorder] DESC,[newsid] DESC) AS [temp1] LEFT JOIN [{1}newstypes] AS [temp2] ON [temp1].[newstypeid]=[temp2].[newstypeid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                condition);
            }
            else
            {
                if (noCondition)
                    commandText = string.Format("SELECT [temp1].[newsid],[temp1].[newstypeid],[temp1].[isshow],[temp1].[istop],[temp1].[ishome],[temp1].[displayorder],[temp1].[addtime],[temp1].[title],[temp2].[name] FROM (SELECT TOP {0} [newsid],[newstypeid],[isshow],[istop],[ishome],[displayorder],[addtime],[title] FROM [{1}news] WHERE [newsid] NOT IN (SELECT TOP {2} [newsid] FROM [{1}news] ORDER BY [displayorder] DESC,[newsid] DESC) ORDER BY [displayorder] DESC,[newsid] DESC) AS [temp1] LEFT JOIN [{1}newstypes] AS [temp2] ON [temp1].[newstypeid]=[temp2].[newstypeid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize);
                else
                    commandText = string.Format("SELECT [temp1].[newsid],[temp1].[newstypeid],[temp1].[isshow],[temp1].[istop],[temp1].[ishome],[temp1].[displayorder],[temp1].[addtime],[temp1].[title],[temp2].[name] FROM (SELECT TOP {0} [newsid],[newstypeid],[isshow],[istop],[ishome],[displayorder],[addtime],[title] FROM [{1}news] WHERE {3} AND [newsid] NOT IN (SELECT TOP {2} [newsid] FROM [{1}news] WHERE {3} ORDER BY [displayorder] DESC,[newsid] DESC) ORDER BY [displayorder] DESC,[newsid] DESC) AS [temp1] LEFT JOIN [{1}newstypes] AS [temp2] ON [temp1].[newstypeid]=[temp2].[newstypeid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize,
                                                condition);
            }

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 后台获得新闻列表搜索条件
        /// </summary>
        /// <param name="newsTypeId">新闻类型id</param>
        /// <param name="title">新闻标题</param>
        /// <returns></returns>
        public string AdminGetNewsListCondition(int newsTypeId, string title)
        {
            StringBuilder condition = new StringBuilder();

            if (newsTypeId > 0)
                condition.AppendFormat(" AND [newstypeid] = {0} ", newsTypeId);
            if (!string.IsNullOrWhiteSpace(title) && SecureHelper.IsSafeSqlString(title))
                condition.AppendFormat(" AND [title] like '{0}%' ", title);

            return condition.Length > 0 ? condition.Remove(0, 4).ToString() : "";
        }

        /// <summary>
        /// 后台获得新闻数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public int AdminGetNewsCount(string condition)
        {
            string commandText;
            if (string.IsNullOrWhiteSpace(condition))
                commandText = string.Format("SELECT COUNT(newsid) FROM [{0}news]", RDBSHelper.RDBSTablePre);
            else
                commandText = string.Format("SELECT COUNT(newsid) FROM [{0}news] WHERE {1}", RDBSHelper.RDBSTablePre, condition);

            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText), 0);
        }

        /// <summary>
        /// 后台根据新闻标题得到新闻id
        /// </summary>
        /// <param name="title">新闻标题</param>
        /// <returns></returns>
        public int AdminGetNewsIdByTitle(string title)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@title", SqlDbType.NVarChar, 100, title)    
                                   };
            string commandText = string.Format("SELECT [newsid] FROM [{0}news] WHERE [title]=@title",
                                                RDBSHelper.RDBSTablePre);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms));
        }

        /// <summary>
        /// 获得置首的新闻列表
        /// </summary>
        /// <param name="newsTypeId">新闻类型id(0代表全部类型)</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public DataTable GetHomeNewsList(int newsTypeId, int count)
        {
            string commandText;
            if (newsTypeId == 0)
                commandText = string.Format("SELECT [temp2].[name],[temp1].[newsid],[temp1].[newstypeid],[temp1].[addtime],[temp1].[title],[temp1].[url] FROM (SELECT TOP {1} [newsid],[newstypeid],[addtime],[title],[url] FROM [{0}news] WHERE [isshow]=1 AND [ishome]=1 ORDER BY [displayorder] DESC) AS [temp1] LEFT JOIN [{0}newstypes] AS [temp2] ON [temp1].[newstypeid]=[temp2].[newstypeid]", RDBSHelper.RDBSTablePre, count);
            else
                commandText = string.Format("SELECT [temp2].[name],[temp1].[newsid],[temp1].[newstypeid],[temp1].[addtime],[temp1].[title],[temp1].[url] FROM (SELECT TOP {2} [newsid],[newstypeid],[addtime],[title],[url] FROM [{0}news] WHERE [newstypeid]={1} AND [isshow]=1 AND [ishome]=1 ORDER BY [displayorder] DESC) AS [temp1] LEFT JOIN [{0}newstypes] AS [temp2] ON [temp1].[newstypeid]=[temp2].[newstypeid]", RDBSHelper.RDBSTablePre, newsTypeId, count);
            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 获得新闻列表条件
        /// </summary>
        /// <param name="newsTypeId">新闻类型id(0代表全部类型)</param>
        /// <param name="title">新闻标题</param>
        /// <returns></returns>
        public string GetNewsListCondition(int newsTypeId, string title)
        {
            StringBuilder condition = new StringBuilder();

            if (newsTypeId > 0)
                condition.AppendFormat(" AND [newstypeid] = {0} ", newsTypeId);
            if (!string.IsNullOrWhiteSpace(title) && SecureHelper.IsSafeSqlString(title))
                condition.AppendFormat(" AND [title] like '{0}%' ", title);

            return condition.Length > 0 ? condition.Remove(0, 4).ToString() : "";
        }

        /// <summary>
        /// 获得新闻列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public DataTable GetNewsList(int pageSize, int pageNumber, string condition)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                    GenerateInParam("@pagenumber", SqlDbType.Int, 4, pageNumber),
                                    GenerateInParam("@condition", SqlDbType.NVarChar, 1000, condition)    
                                   };
            return RDBSHelper.ExecuteDataset(CommandType.StoredProcedure,
                                             string.Format("{0}getnewslist", RDBSHelper.RDBSTablePre),
                                             parms).Tables[0];
        }

        /// <summary>
        /// 获得新闻数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public int GetNewsCount(string condition)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@condition", SqlDbType.NVarChar, 1000, condition)   
                                   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}getnewscount", RDBSHelper.RDBSTablePre),
                                                                   parms));
        }

        #endregion

        #region 帮助

        /// <summary>
        /// 创建帮助
        /// </summary>
        public void CreateHelp(HelpInfo helpInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@pid", SqlDbType.Int, 4, helpInfo.Pid),
                                        GenerateInParam("@title", SqlDbType.NChar,60,helpInfo.Title),
                                        GenerateInParam("@url", SqlDbType.NChar,200,helpInfo.Url),
                                        GenerateInParam("@description", SqlDbType.NText, 0, helpInfo.Description),
                                        GenerateInParam("@displayorder", SqlDbType.Int,4,helpInfo.DisplayOrder)
                                    };
            string commandText = string.Format("INSERT INTO [{0}helps]([pid],[title],[url],[description],[displayorder]) VALUES(@pid,@title,@url,@description,@displayorder)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除帮助
        /// </summary>
        /// <param name="id">帮助id</param>
        public void DeleteHelpById(int id)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@id", SqlDbType.Int, 4, id)    
                                    };
            string commandText = string.Format("DELETE FROM [{0}helps] WHERE [id]=@id",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新帮助
        /// </summary>
        public void UpdateHelp(HelpInfo helpInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@pid", SqlDbType.Int, 4, helpInfo.Pid),
                                        GenerateInParam("@title", SqlDbType.NChar,60,helpInfo.Title),
                                        GenerateInParam("@url", SqlDbType.NChar,200,helpInfo.Url),
                                        GenerateInParam("@description", SqlDbType.NText, 0, helpInfo.Description),
                                        GenerateInParam("@displayorder", SqlDbType.Int,4,helpInfo.DisplayOrder),
                                        GenerateInParam("@id", SqlDbType.Int, 4, helpInfo.Id)    
                                    };

            string commandText = string.Format("UPDATE [{0}helps] SET [pid]=@pid,[title]=@title,[url]=@url,[description]=@description,[displayorder]=@displayorder WHERE [id]=@id",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得帮助列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetHelpList()
        {
            string commandText = string.Format("SELECT {1} FROM [{0}helps] ORDER BY [displayorder] DESC",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.HELPS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 更新帮助排序
        /// </summary>
        /// <param name="id">帮助id</param>
        /// <param name="displayOrder">排序</param>
        /// <returns></returns>
        public bool UpdateHelpDisplayOrder(int id, int displayOrder)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@id", SqlDbType.Int, 4, id),
                                        GenerateInParam("@displayorder", SqlDbType.Int,4,displayOrder)
                                    };
            string commandText = string.Format("UPDATE [{0}helps] SET [displayorder]=@displayorder WHERE [id]=@id",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms) > 0;
        }

        #endregion

        #region 友情链接

        /// <summary>
        /// 获得友情链接列表
        /// </summary>
        public DataTable GetFriendLinkList()
        {
            string commandText = string.Format("SELECT {1} FROM [{0}friendlinks] ORDER BY [displayorder] DESC",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.FRIENDLINKS);
            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 创建友情链接
        /// </summary>
        public void CreateFriendLink(FriendLinkInfo friendLinkInfo)
        {
            DbParameter[] param = {
                                         GenerateInParam("@name",SqlDbType.NChar, 50, friendLinkInfo.Name),
                                         GenerateInParam("@title",SqlDbType.NChar, 100, friendLinkInfo.Title),
                                         GenerateInParam("@logo",SqlDbType.NChar, 250, friendLinkInfo.Logo),
                                         GenerateInParam("@url",SqlDbType.NChar, 250, friendLinkInfo.Url),
                                         GenerateInParam("@target",SqlDbType.Int, 4, friendLinkInfo.Target),
                                         GenerateInParam("@displayorder",SqlDbType.Int,4,friendLinkInfo.DisplayOrder)
                                       };
            string commandText = String.Format("INSERT INTO [{0}friendlinks] ([name],[title],[logo],[url],[target],[displayorder]) VALUES(@name,@title,@logo,@url,@target,@displayorder)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, param);
        }

        /// <summary>
        /// 更新友情链接
        /// </summary>
        public void UpdateFriendLink(FriendLinkInfo friendLinkInfo)
        {
            DbParameter[] param = {
                                         GenerateInParam("@name",SqlDbType.NChar, 50, friendLinkInfo.Name),
                                         GenerateInParam("@title",SqlDbType.NChar, 100, friendLinkInfo.Title),
                                         GenerateInParam("@logo",SqlDbType.NChar, 250, friendLinkInfo.Logo),
                                         GenerateInParam("@url",SqlDbType.NChar, 250, friendLinkInfo.Url),
                                         GenerateInParam("@target",SqlDbType.Int, 4, friendLinkInfo.Target),
                                         GenerateInParam("@displayorder",SqlDbType.Int,4,friendLinkInfo.DisplayOrder),
                                         GenerateInParam("@id",SqlDbType.Int, 4, friendLinkInfo.Id)
                                       };
            string commandText = String.Format("UPDATE [{0}friendlinks] SET [name]=@name,[title]=@title,[logo]=@logo,[url]=@url,[target]=@target,[displayorder]=@displayorder WHERE [id]=@id",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, param);
        }

        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="idList">友情链接id</param>
        public void DeleteFriendLinkById(string idList)
        {
            string commandText = String.Format("DELETE FROM [{0}friendlinks] WHERE [id] IN ({1})",
                                                RDBSHelper.RDBSTablePre,
                                                idList);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText);
        }

        #endregion

        #region 全国行政区域

        /// <summary>
        /// 获得全部区域
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllRegion()
        {
            string commandText = string.Format("SELECT {1} FROM [{0}regions] ORDER BY [displayorder] DESC",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.REGIONS);
            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 获得区域列表
        /// </summary>
        /// <param name="parentId">父id</param>
        /// <returns></returns>
        public IDataReader GetRegionList(int parentId)
        {
            DbParameter[] param = {
                                    GenerateInParam("@parentid",SqlDbType.SmallInt, 2, parentId)
                                   };
            string commandText = string.Format("SELECT {1} FROM [{0}regions] WHERE [parentid]=@parentid ORDER BY [displayorder] DESC",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.REGIONS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText, param);
        }

        /// <summary>
        /// 获得区域
        /// </summary>
        /// <param name="regionId">区域id</param>
        /// <returns></returns>
        public IDataReader GetRegionById(int regionId)
        {
            DbParameter[] param = {
                                    GenerateInParam("@regionid",SqlDbType.SmallInt, 2, regionId)
                                   };
            string commandText = string.Format("SELECT {1} FROM [{0}regions] WHERE [regionid]=@regionid",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.REGIONS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText, param);
        }

        /// <summary>
        /// 获得区域
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="layer">级别</param>
        /// <returns></returns>
        public IDataReader GetRegionByNameAndLayer(string name, int layer)
        {
            DbParameter[] param = {
                                    GenerateInParam("@name",SqlDbType.NChar, 50, name),
                                    GenerateInParam("@layer",SqlDbType.TinyInt, 1, layer)
                                   };
            string commandText = string.Format("SELECT {1} FROM [{0}regions] WHERE [name]=@name AND [layer]=@layer",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.REGIONS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText, param);
        }

        #endregion

        #region 积分日志

        /// <summary>
        /// 后台获得积分日志列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public DataTable AdminGetCreditLogList(int pageSize, int pageNumber, string condition)
        {
            bool noCondition = string.IsNullOrWhiteSpace(condition);
            string commandText;
            if (pageNumber == 1)
            {
                if (noCondition)
                    commandText = string.Format("SELECT [temp1].[logid],[temp1].[uid],[temp1].[paycredits],[temp1].[rankcredits],[temp1].[action],[temp1].[actioncode],[temp1].[actiontime],[temp1].[actiondes],[temp1].[operator],[temp2].[username] AS [rusername],[temp3].[username] AS [ousername] FROM (SELECT TOP {0} {2} FROM [{1}creditlogs] ORDER BY [logid] DESC) AS [temp1] LEFT JOIN [{1}users] AS [temp2] ON [temp1].[uid]=[temp2].[uid] LEFT JOIN [{1}users] AS [temp3] ON [temp1].[operator]=[temp3].[uid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.CREDITLOGS);

                else
                    commandText = string.Format("SELECT [temp1].[logid],[temp1].[uid],[temp1].[paycredits],[temp1].[rankcredits],[temp1].[action],[temp1].[actioncode],[temp1].[actiontime],[temp1].[actiondes],[temp1].[operator],[temp2].[username] AS [rusername],[temp3].[username] AS [ousername] FROM (SELECT TOP {0} {2} FROM [{1}creditlogs] WHERE {3} ORDER BY [logid] DESC) AS [temp1] LEFT JOIN [{1}users] AS [temp2] ON [temp1].[uid]=[temp2].[uid] LEFT JOIN [{1}users] AS [temp3] ON [temp1].[operator]=[temp3].[uid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.CREDITLOGS,
                                                condition);
            }
            else
            {
                if (noCondition)
                    commandText = string.Format("SELECT [temp1].[logid],[temp1].[uid],[temp1].[paycredits],[temp1].[rankcredits],[temp1].[action],[temp1].[actioncode],[temp1].[actiontime],[temp1].[actiondes],[temp1].[operator],[temp2].[username] AS [rusername],[temp3].[username] AS [ousername] FROM (SELECT TOP {0} {2} FROM [{1}creditlogs] WHERE [logid] < (SELECT MIN([logid]) FROM (SELECT TOP {3} [logid] FROM [{1}creditlogs] ORDER BY [logid] DESC) AS [t]) ORDER BY [logid] DESC) AS [temp1] LEFT JOIN [{1}users] AS [temp2] ON [temp1].[uid]=[temp2].[uid] LEFT JOIN [{1}users] AS [temp3] ON [temp1].[operator]=[temp3].[uid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.CREDITLOGS,
                                                (pageNumber - 1) * pageSize);
                else
                    commandText = string.Format("SELECT [temp1].[logid],[temp1].[uid],[temp1].[paycredits],[temp1].[rankcredits],[temp1].[action],[temp1].[actioncode],[temp1].[actiontime],[temp1].[actiondes],[temp1].[operator],[temp2].[username] AS [rusername],[temp3].[username] AS [ousername] FROM (SELECT TOP {0} {2} FROM [{1}creditlogs] WHERE [logid] < (SELECT MIN([logid]) FROM (SELECT TOP {3} [logid] FROM [{1}creditlogs] WHERE {4} ORDER BY [logid] DESC) AS [t]) AND {4} ORDER BY [logid] DESC) AS [temp1] LEFT JOIN [{1}users] AS [temp2] ON [temp1].[uid]=[temp2].[uid] LEFT JOIN [{1}users] AS [temp3] ON [temp1].[operator]=[temp3].[uid]",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.CREDITLOGS,
                                                (pageNumber - 1) * pageSize,
                                                condition);
            }

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 后台获得积分日志列表条件
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public string AdminGetCreditLogListCondition(int uid, string startTime, string endTime)
        {
            StringBuilder condition = new StringBuilder();

            if (uid > 0)
                condition.AppendFormat(" AND [uid]={0} ", uid);
            if (!string.IsNullOrEmpty(startTime))
                condition.AppendFormat(" AND [actiontime]>='{0}' ", TypeHelper.StringToDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss"));
            if (!string.IsNullOrEmpty(endTime))
                condition.AppendFormat(" AND [actiontime]<='{0}' ", TypeHelper.StringToDateTime(endTime).ToString("yyyy-MM-dd HH:mm:ss"));

            return condition.Length > 0 ? condition.Remove(0, 4).ToString() : "";
        }

        /// <summary>
        /// 后台获得积分日志数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public int AdminGetCreditLogCount(string condition)
        {
            string commandText;
            if (string.IsNullOrWhiteSpace(condition))
                commandText = string.Format("SELECT COUNT(logid) FROM [{0}creditlogs]", RDBSHelper.RDBSTablePre);
            else
                commandText = string.Format("SELECT COUNT(logid) FROM [{0}creditlogs] WHERE {1}", RDBSHelper.RDBSTablePre, condition);

            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText), 0);
        }

        /// <summary>
        /// 发放积分
        /// </summary>
        /// <param name="userRid">用户等级id</param>
        /// <param name="creditLogInfo">积分日志信息</param>
        public void SendCredits(int userRid, CreditLogInfo creditLogInfo)
        {
            DbParameter[] parms = {
									   GenerateInParam("@userrid",SqlDbType.SmallInt,2,userRid),
									   GenerateInParam("@uid",SqlDbType.Int,4,creditLogInfo.Uid),
									   GenerateInParam("@paycredits",SqlDbType.Int,4,creditLogInfo.PayCredits),
									   GenerateInParam("@rankcredits",SqlDbType.Int,4,creditLogInfo.RankCredits),
									   GenerateInParam("@action",SqlDbType.TinyInt,1,creditLogInfo.Action),
                                       GenerateInParam("@actioncode",SqlDbType.Int,4,creditLogInfo.ActionCode),
									   GenerateInParam("@actiontime",SqlDbType.DateTime,8,creditLogInfo.ActionTime),
                                       GenerateInParam("@actiondes",SqlDbType.NVarChar,300,creditLogInfo.ActionDes),
									   GenerateInParam("@operator",SqlDbType.Int,4,creditLogInfo.Operator)
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}sendcredits", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 获得今天发放的支付积分
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="today">今天日期</param>
        /// <returns></returns>
        public int GetTodaySendPayCredits(int uid, DateTime today)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4,uid),
									   GenerateInParam("@today",SqlDbType.DateTime,8,today.Date)
								   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}gettodaysendpaycredits", RDBSHelper.RDBSTablePre),
                                                                   parms), 0);
        }

        /// <summary>
        /// 获得今天发放的等级积分
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="today">今天日期</param>
        /// <returns></returns>
        public int GetTodaySendRankCredits(int uid, DateTime today)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4,uid),
									   GenerateInParam("@today",SqlDbType.DateTime,8,today.Date)
								   };

            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}gettodaysendrankcredits", RDBSHelper.RDBSTablePre),
                                                                   parms), 0);
        }

        /// <summary>
        /// 是否发放了今天的登陆积分
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="today">今天日期</param>
        /// <returns></returns>
        public bool IsSendTodayLoginCredit(int uid, DateTime today)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4,uid),
									   GenerateInParam("@today",SqlDbType.DateTime,8,today.Date)
								   };
            string commandText = string.Format("SELECT [logid] FROM [{0}creditlogs] WHERE [uid]=@uid AND [actiontime]>=@today AND [action]={1}",
                                                RDBSHelper.RDBSTablePre,
                                                (int)CreditAction.Login);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms)) > 0;
        }

        /// <summary>
        /// 是否发放了完成用户信息积分
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public bool IsSendCompleteUserInfoCredit(int uid)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4,uid)
								   };
            string commandText = string.Format("SELECT [logid] FROM [{0}creditlogs] WHERE [uid]=@uid AND [action]={1}",
                                                RDBSHelper.RDBSTablePre,
                                                (int)CreditAction.CompleteUserInfo);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms)) > 0;
        }

        /// <summary>
        /// 获得支付积分日志列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="type">类型(2代表全部类型，0代表收入，1代表支出)</param>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        public IDataReader GetPayCreditLogList(int uid, int type, int pageSize, int pageNumber)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4,uid),
									   GenerateInParam("@type",SqlDbType.TinyInt,1,type),
									   GenerateInParam("@pagesize",SqlDbType.Int,4,pageSize),
									   GenerateInParam("@pagenumber",SqlDbType.Int,4,pageNumber)
								   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getpaycreditloglist", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得支付积分日志数量
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="type">类型(2代表全部类型，0代表收入，1代表支出)</param>
        /// <returns></returns>
        public int GetPayCreditLogCount(int uid, int type)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4,uid),
									   GenerateInParam("@type",SqlDbType.TinyInt,1,type)
								   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}getpaycreditlogcount", RDBSHelper.RDBSTablePre),
                                                                   parms));
        }

        /// <summary>
        /// 获得用户订单发放的积分
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <returns></returns>
        public DataTable GetUserOrderSendCredits(int oid)
        {
            DbParameter[] parms = {
									GenerateInParam("@oid",SqlDbType.Int,4,oid)
								   };
            return RDBSHelper.ExecuteDataset(CommandType.StoredProcedure,
                                             string.Format("{0}getuserordersendcredits", RDBSHelper.RDBSTablePre),
                                             parms).Tables[0];
        }

        #endregion

        #region 事件日志

        /// <summary>
        /// 创建事件日志
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="title">标题</param>
        /// <param name="server">服务器名称</param>
        /// <param name="executeTime">执行时间</param>
        public void CreateEventLog(string key, string title, string server, DateTime executeTime)
        {
            DbParameter[] parms = {
									   GenerateInParam("@key",SqlDbType.NVarChar,100,key),
									   GenerateInParam("@title",SqlDbType.NVarChar,200,title),
									   GenerateInParam("@server",SqlDbType.NVarChar,100,server),
									   GenerateInParam("@executetime",SqlDbType.DateTime,8,executeTime)
								   };
            string commandText = string.Format("INSERT INTO [{0}eventlogs]([key],[title],[server],[executetime]) VALUES(@key,@title,@server,@executetime)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得事件的最后执行时间
        /// </summary>
        /// <param name="key">事件key</param>
        /// <returns></returns>
        public DateTime GetEventLastExecuteTimeByKey(string key)
        {
            DbParameter[] parms =  {
                                     GenerateInParam("@key", SqlDbType.NVarChar, 100, key)
                                   };
            string commandText = string.Format("SELECT TOP 1 [executetime] FROM [{0}eventlogs] WHERE [key]=@key ORDER BY [id] DESC",
                                                RDBSHelper.RDBSTablePre);
            return TypeHelper.ObjectToDateTime(RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms), new DateTime(1900, 1, 1));
        }

        #endregion

        #region 基本资料
        /// <summary>
        /// 获取基本资料
        /// </summary>
        /// <returns></returns>
        public DataTable GetBaseInfoList(int baseid = -1, string condition = "")
        {
            string where = " where 1=1 ";
            if (baseid != -1)
                where += " and baseid=" + baseid;
            if (condition != "")
                where += condition;
            string sql = string.Format(@"SELECT [baseid]
      ,[title]
      ,[content],outid
      ,[addtime]
  FROM {0}baseinfo {1}", RDBSHelper.RDBSTablePre, where);
            DataSet ds = RDBSHelper.ExecuteDataset(CommandType.Text, sql);
            DataTable dt = new DataTable();
            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];
            return dt;
        }

        /// <summary>
        /// 修改基本资料
        /// </summary>
        /// <param name="baseid"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool UpdateBaseInfo(int baseid, string title, string content)
        {
            DbParameter[] parms = {
									   GenerateInParam("@baseid",SqlDbType.Int,10,baseid),
									   GenerateInParam("@title",SqlDbType.NVarChar,50,title),
									   GenerateInParam("@content",SqlDbType.NText,3000,content)
								   };
            string sql = string.Format(@"
if exists(select 1 from {0}baseinfo a where a.[baseid]=@baseid)
begin
update a set title=@title,content=@content from {0}baseinfo a where a.[baseid]=@baseid 
end
else 
begin
insert into {0}baseinfo(title,content) values(@title,@content)
end", RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, sql, parms) > 0 ? true : false;
        }


        /// <summary>
        /// 获取基础类型
        /// </summary>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetBaseTypeList(string condition = "")
        {

            string sql = string.Format(@"SELECT [systypeid],outtypeid
      ,[type]
      ,[parentid]
      ,(select type from {0}sys_basetype where systypeid=parentid) parenttype
      ,[remark],[displayorder]
  FROM {0}sys_basetype  {1}  order by displayorder", RDBSHelper.RDBSTablePre, condition);
            return RDBSHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }
        /// <summary>
        /// 添加基础类型
        /// </summary>
        /// <param name="basetype"></param>
        /// <returns></returns>
        public bool AddBaseType(BaseTypeModel basetype)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@type",SqlDbType.NVarChar,50,basetype.Type),
									   GenerateInParam("@parentid",SqlDbType.Int,10,basetype.Parentid),
									   GenerateInParam("@remark",SqlDbType.NVarChar,1000,basetype.Remark)
								   };
            string sql = string.Format(@"
insert into {0}sys_basetype([type] ,[parentid],[remark]) values(@type,@parentid,@remark)
", RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, sql, parms) > 0 ? true : false;
        }

        /// <summary>
        /// 修改基础类型
        /// </summary>
        /// <param name="basetype"></param>
        /// <returns></returns>
        public bool UpdateBaseType(BaseTypeModel basetype)
        {
            DbParameter[] parms = {
									   GenerateInParam("@systypeid",SqlDbType.Int,10,basetype.Systypeid),
                                        GenerateInParam("@type",SqlDbType.NVarChar,50,basetype.Type),
									   GenerateInParam("@parentid",SqlDbType.Int,10,basetype.Parentid),
									   GenerateInParam("@remark",SqlDbType.NVarChar,1000,basetype.Remark)
								   };
            string sql = string.Format(@"
if exists(select 1 from {0}sys_basetype a where a.[systypeid]=@systypeid)
begin
update a set type=@type,parentid=@parentid,remark=@remark from {0}sys_basetype a where a.[systypeid]=@systypeid 
end
else 
begin
insert into {0}sys_basetype([type] ,[parentid],[remark]) values(@type,@parentid,@remark)
end", RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, sql, parms) > 0 ? true : false;
        }

        /// <summary>
        /// 删除基础类型
        /// </summary>
        /// <param name="basetype"></param>
        /// <returns></returns>
        public bool DeleteBaseType(int systypeid)
        {

            string sql = string.Format(@"
delete a from {0}sys_basetype a where a.[systypeid]={1}
", RDBSHelper.RDBSTablePre, systypeid);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, sql) > 0 ? true : false;
        }
        #endregion

        #region 活动专题

        /// <summary>
        /// 创建活动专题
        /// </summary>
        /// <param name="topicInfo">活动专题信息</param>
        public void CreateTopic(TopicInfo topicInfo)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@starttime", SqlDbType.DateTime, 8, topicInfo.StartTime),
                                    GenerateInParam("@endtime", SqlDbType.DateTime,8,topicInfo.EndTime),
                                    GenerateInParam("@isshow", SqlDbType.TinyInt,1,topicInfo.IsShow),
                                    GenerateInParam("@sn", SqlDbType.Char,16,topicInfo.SN),
                                    GenerateInParam("@title", SqlDbType.NVarChar,100,topicInfo.Title),
                                    GenerateInParam("@headhtml", SqlDbType.NText,0,topicInfo.HeadHtml),
                                    GenerateInParam("@bodyhtml", SqlDbType.NText,0,topicInfo.BodyHtml)
                                    };
            string commandText = string.Format("INSERT INTO [{0}topics]([starttime],[endtime],[isshow],[sn],[title],[headhtml],[bodyhtml]) VALUES(@starttime,@endtime,@isshow,@sn,@title,@headhtml,@bodyhtml)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新活动专题
        /// </summary>
        /// <param name="topicInfo">活动专题信息</param>
        public void UpdateTopic(TopicInfo topicInfo)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@starttime", SqlDbType.DateTime, 8, topicInfo.StartTime),
                                    GenerateInParam("@endtime", SqlDbType.DateTime,8,topicInfo.EndTime),
                                    GenerateInParam("@isshow", SqlDbType.TinyInt,1,topicInfo.IsShow),
                                    GenerateInParam("@sn", SqlDbType.Char,16,topicInfo.SN),
                                    GenerateInParam("@title", SqlDbType.NVarChar,100,topicInfo.Title),
                                    GenerateInParam("@headhtml", SqlDbType.NText,0,topicInfo.HeadHtml),
                                    GenerateInParam("@bodyhtml", SqlDbType.NText,0,topicInfo.BodyHtml),
                                    GenerateInParam("@topicid", SqlDbType.Int,4,topicInfo.TopicId)
                                    };
            string commandText = string.Format("UPDATE [{0}topics] SET [starttime]=@starttime,[endtime]=@endtime,[isshow]=@isshow,[sn]=@sn,[title]=@title,[headhtml]=@headhtml,[bodyhtml]=@bodyhtml WHERE [topicid]=@topicid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除活动专题
        /// </summary>
        /// <param name="topicId">活动专题id</param>
        public void DeleteTopicById(int topicId)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@topicid", SqlDbType.Int,4,topicId)
                                    };
            string commandText = string.Format("DELETE FROM [{0}topics] WHERE [topicid]=@topicid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 后台获得活动专题
        /// </summary>
        /// <param name="topicId">活动专题id</param>
        /// <returns></returns>
        public IDataReader AdminGetTopicById(int topicId)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@topicid", SqlDbType.Int,4,topicId)
                                    };
            string commandText = string.Format("SELECT {1} FROM [{0}topics] WHERE [topicid]=@topicid",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.TOPICS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 后台获得活动专题列表条件
        /// </summary>
        /// <param name="sn">编号</param>
        /// <param name="title">标题</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public string AdminGetTopicListCondition(string sn, string title, string startTime, string endTime)
        {
            StringBuilder condition = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(sn) && SecureHelper.IsSafeSqlString(sn))
                condition.AppendFormat(" AND [sn] like '{0}%' ", sn);
            if (!string.IsNullOrWhiteSpace(title) && SecureHelper.IsSafeSqlString(title))
                condition.AppendFormat(" AND [title] like '{0}%' ", title);
            if (!string.IsNullOrEmpty(startTime))
                condition.AppendFormat(" AND [starttime] >= '{0}' ", TypeHelper.StringToDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss"));
            if (!string.IsNullOrEmpty(endTime))
                condition.AppendFormat(" AND [endtime] <= '{0}' ", TypeHelper.StringToDateTime(endTime).ToString("yyyy-MM-dd HH:mm:ss"));

            return condition.Length > 0 ? condition.Remove(0, 4).ToString() : "";
        }

        /// <summary>
        /// 后台获得活动专题列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public DataTable AdminGetTopicList(int pageSize, int pageNumber, string condition)
        {
            bool noCondition = string.IsNullOrWhiteSpace(condition);
            string commandText;
            if (pageNumber == 1)
            {
                if (noCondition)
                    commandText = string.Format("SELECT TOP {0} [topicid],[starttime],[endtime],[isshow],[sn],[title] FROM [{1}topics] ORDER BY [topicid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre);

                else
                    commandText = string.Format("SELECT TOP {0} [topicid],[starttime],[endtime],[isshow],[sn],[title] FROM [{1}topics] WHERE {2} ORDER BY [topicid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                condition);
            }
            else
            {
                if (noCondition)
                    commandText = string.Format("SELECT TOP {0} [topicid],[starttime],[endtime],[isshow],[sn],[title] FROM [{1}topics] WHERE [topicid] NOT IN (SELECT TOP {2} [topicid] FROM [{1}topics] ORDER BY [topicid] DESC) ORDER BY [topicid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize);
                else
                    commandText = string.Format("SELECT TOP {0} [topicid],[starttime],[endtime],[isshow],[sn],[title] FROM [{1}topics] WHERE [topicid] NOT IN (SELECT TOP {2} [topicid] FROM [{1}topics] WHERE {3} ORDER BY [topicid] DESC) AND {3} ORDER BY [topicid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize,
                                                condition);
            }

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 后台获得活动专题数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public int AdminGetTopicCount(string condition)
        {
            string commandText;
            if (string.IsNullOrWhiteSpace(condition))
                commandText = string.Format("SELECT COUNT(topicid) FROM [{0}topics]", RDBSHelper.RDBSTablePre);
            else
                commandText = string.Format("SELECT COUNT(topicid) FROM [{0}topics] WHERE {1}", RDBSHelper.RDBSTablePre, condition);

            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        /// <summary>
        /// 判断活动专题编号是否存在
        /// </summary>
        /// <param name="topicSN">活动专题编号</param>
        /// <returns></returns>
        public bool IsExistTopicSN(string topicSN)
        {
            DbParameter[] parms = {
                                     GenerateInParam("@topicsn", SqlDbType.Char, 16, topicSN)
                                   };
            string commandText = string.Format("SELECT TOP 1 topicid FROM [{0}topics] WHERE [sn]=@topicsn", RDBSHelper.RDBSTablePre);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms)) > 0;
        }

        /// <summary>
        /// 获得活动专题
        /// </summary>
        /// <param name="topicId">活动专题id</param>
        /// <param name="nowTime">当前时间</param>
        /// <returns></returns>
        public IDataReader GetTopicByIdAndTime(int topicId, DateTime nowTime)
        {
            DbParameter[] parms = {
                                     GenerateInParam("@topicid", SqlDbType.Int, 4, topicId),
                                     GenerateInParam("@nowtime", SqlDbType.DateTime, 8, nowTime)
                                    };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}gettopicbyidandtime", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得活动专题
        /// </summary>
        /// <param name="topicSN">活动专题编号</param>
        /// <param name="nowTime">当前时间</param>
        /// <returns></returns>
        public IDataReader GetTopicBySNAndTime(string topicSN, DateTime nowTime)
        {
            DbParameter[] parms = {
                                     GenerateInParam("@topicsn", SqlDbType.Char, 16, topicSN),
                                     GenerateInParam("@nowtime", SqlDbType.DateTime, 8, nowTime)
                                   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}gettopicbysnandtime", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        #endregion

        #region 意见

        /// <summary>
        /// 添加意见
        /// </summary>
        /// <param name="account"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool AddUserAdvice(string account, string remark)
        {
            string sql = string.Format(@"INSERT INTO [owzx_adviceinfo]
           ([account]
           ,[remark])
     VALUES('{0}','{1}')", account, remark);

            return RDBSHelper.ExecuteNonQuery(sql) == 1 ? true : false;
        }
        /// <summary>
        /// 回复意见
        /// </summary>
        /// <param name="advice"></param>
        /// <returns></returns>
        public string UpdateUserAdvice(AdviceInfoModel advice)
        {
            DbParameter[] parms = {
                                      GenerateInParam("@adviceid", SqlDbType.Int, 4, advice.Adviceid),
                                      GenerateInParam("@reply", SqlDbType.VarChar, 300, advice.reply),
                                      GenerateInParam("@replyuid", SqlDbType.Int, 4, advice.replyuid),
                                        
                                    };
            string commandText = string.Format(@"
begin try
if exists(select 1 from [{0}adviceinfo] where [adviceid]=@adviceid)
begin
UPDATE [{0}adviceinfo]
   SET reply=@reply,replyuid=@replyuid
where [adviceid]=@adviceid

select '修改成功' state
end
else
begin
select '意见已被删除' state
end
end try
begin catch
select ERROR_MESSAGE() state
end catch
",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();

        }
        /// <summary>
        /// 获取意见列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable GetUserAdvice(int pageSize, int pageNumber, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };

            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
  drop table #list

   select ROW_NUMBER() over(order by adviceid desc) id, adviceid,account,remark,addtime,isnull(reply,'')reply,isnull(replyuid,-1) replyuid,isnull(replytime,'') replytime
   into #list
   from {0}adviceinfo   {1}

if(@pagesize=-1)
begin
select cast(id as int) id,adviceid,account,remark,addtime,reply,
(select count(1)  from #list) TotalCount from #list
end
else
begin
select cast(id as int) id,adviceid,account,remark,addtime ,reply,
(select count(1)  from #list) TotalCount from #list where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end
 end try
begin catch
select ERROR_MESSAGE() state
end catch

", RDBSHelper.RDBSTablePre, condition);

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }
        #endregion
    }
}