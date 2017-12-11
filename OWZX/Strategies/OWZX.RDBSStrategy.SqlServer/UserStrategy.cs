using System;
using System.Text;
using System.Data;
using System.Data.Common;

using OWZX.Core;
using OWZX.Model;

namespace OWZX.RDBSStrategy.SqlServer
{
    /// <summary>
    /// SqlServer策略之用户分部类
    /// </summary>
    public partial class RDBSStrategy : IRDBSStrategy
    {
        #region 在线用户

        /// <summary>
        /// 创建在线用户
        /// </summary>
        public int CreateOnlineUser(OnlineUserInfo onlineUserInfo)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4,onlineUserInfo.Uid),
									   GenerateInParam("@sid",SqlDbType.Char,16,onlineUserInfo.Sid),
                                       GenerateInParam("@nickname",SqlDbType.NChar,20,onlineUserInfo.NickName),	
                                       GenerateInParam("@ip",SqlDbType.Char,15,onlineUserInfo.IP),	
                                       GenerateInParam("@regionid",SqlDbType.SmallInt,2,onlineUserInfo.RegionId),	
									   GenerateInParam("@updatetime",SqlDbType.DateTime,8,onlineUserInfo.UpdateTime)
								   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}createonlineuser", RDBSHelper.RDBSTablePre),
                                                                   parms));
        }

        /// <summary>
        /// 更新在线用户ip
        /// </summary>
        /// <param name="olId">在线用户id</param>
        /// <param name="ip">ip</param>
        public void UpdateOnlineUserIP(int olId, string ip)
        {
            DbParameter[] parms = {
									   GenerateInParam("@ip",SqlDbType.Char,15,ip),
									   GenerateInParam("@olid",SqlDbType.Int,4,olId)
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateonlineuserip", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 更新在线用户uid
        /// </summary>
        /// <param name="olId">在线用户id</param>
        /// <param name="ip">uid</param>
        public void UpdateOnlineUserUid(int olId, int uid)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4,uid),
									   GenerateInParam("@olid",SqlDbType.Int,4,olId)
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateonlineuseruid", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 获得在线用户
        /// </summary>
        /// <param name="sid">sessionId</param>
        /// <returns></returns>
        public IDataReader GetOnlineUserBySid(string sid)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@sid",SqlDbType.Char,16,sid)
                                  };

            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getonlineuserbysid", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得在线用户数量
        /// </summary>
        /// <param name="userType">在线用户类型</param>
        /// <returns></returns>
        public int GetOnlineUserCount(int userType)
        {
            DbParameter[] parms = {
                                      GenerateInParam("@usertype",SqlDbType.Int,4,userType)
                                   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}getonlineuercount", RDBSHelper.RDBSTablePre),
                                                                   parms));
        }

        /// <summary>
        /// 删除在线用户
        /// </summary>
        /// <param name="sid">sessionId</param>
        public void DeleteOnlineUserBySid(string sid)
        {
            DbParameter[] parms = { 
                                        GenerateInParam("@sid", SqlDbType.Char, 16, sid)
                                    };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}deleteonlineuserbysid", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 删除过期在线用户
        /// </summary>
        /// <param name="onlineUserExpire">过期时间</param>
        public void DeleteExpiredOnlineUser(int onlineUserExpire)
        {
            DbParameter[] parms = { 
                                    GenerateInParam("@expiretime", SqlDbType.DateTime, 8, DateTime.Now.AddMinutes(onlineUserExpire * -1))
                                  };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}deleteexpiredonlineuser", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 重置在线用户表
        /// </summary>
        public void ResetOnlineUserTable()
        {
            RDBSHelper.ExecuteNonQuery(CommandType.Text,
                                       string.Format("TRUNCATE TABLE [{0}onlineusers]",
                                       RDBSHelper.RDBSTablePre));
        }

        /// <summary>
        /// 获得在线用户列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="locationType">位置类型(0代表省,1代表市,2代表区或县)</param>
        /// <param name="locationId">位置id</param>
        /// <returns></returns>
        public IDataReader GetOnlineUserList(int pageSize, int pageNumber, int locationType, int locationId)
        {
            string condition = GetOnlineUserListCondition(locationType, locationId);
            bool noCondition = string.IsNullOrWhiteSpace(condition);
            string commandText;
            if (pageNumber == 1)
            {
                if (noCondition)
                    commandText = string.Format("SELECT TOP {0} {2} FROM [{1}onlineusers] ORDER BY [olid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.ONLINE_USERS);

                else
                    commandText = string.Format("SELECT TOP {0} {3} FROM [{1}onlineusers] WHERE {2} ORDER BY [olid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                condition,
                                                RDBSFields.ONLINE_USERS);
            }
            else
            {
                if (noCondition)
                    commandText = string.Format("SELECT TOP {0} {3} FROM [{1}onlineusers] WHERE [olid] NOT IN (SELECT TOP {2} [olid] FROM [{1}onlineusers] ORDER BY [olid] DESC) ORDER BY [olid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize,
                                                RDBSFields.ONLINE_USERS);
                else
                    commandText = string.Format("SELECT TOP {0} {4} FROM [{1}onlineusers] WHERE [olid] NOT IN (SELECT TOP {2} [olid] FROM [{1}onlineusers] WHERE {3} ORDER BY [olid] DESC) AND {3} ORDER BY [olid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize,
                                                condition,
                                                RDBSFields.ONLINE_USERS);
            }

            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 获得在线用户数量
        /// </summary>
        /// <param name="locationType">位置类型(0代表省,1代表市,2代表区或县)</param>
        /// <param name="locationId">位置id</param>
        /// <returns></returns>
        public int GetOnlineUserCount(int locationType, int locationId)
        {
            string condition = GetOnlineUserListCondition(locationType, locationId);
            string commandText;
            if (string.IsNullOrWhiteSpace(condition))
                commandText = string.Format("SELECT COUNT(olid) FROM [{0}onlineusers]", RDBSHelper.RDBSTablePre);
            else
                commandText = string.Format("SELECT COUNT(olid) FROM [{0}onlineusers] WHERE {1}", RDBSHelper.RDBSTablePre, condition);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        /// <summary>
        /// 获得在线用户列表条件
        /// </summary>
        /// <param name="locationType">位置类型(0代表省,1代表市,2代表区或县)</param>
        /// <param name="locationId">位置id</param>
        /// <returns></returns>
        private string GetOnlineUserListCondition(int locationType, int locationId)
        {
            if (locationId > 0)
            {
                if (locationType == 0)
                {
                    return string.Format(" [regionid] IN (SELECT [regionid] FROM [{0}regions] WHERE [provinceid]={1})", RDBSHelper.RDBSTablePre, locationId);
                }
                else if (locationType == 1)
                {
                    return string.Format(" [regionid] IN (SELECT [regionid] FROM [{0}regions] WHERE [cityid]={1})", RDBSHelper.RDBSTablePre, locationId);
                }
                else if (locationType == 2)
                {
                    return string.Format(" [regionid]={0}", locationId);
                }
            }

            return "";
        }

        #endregion

        #region 开放授权

        /// <summary>
        /// 创建开放授权用户
        /// </summary>
        /// <param name="oauthInfo">开放授权信息</param>
        public void CreateOAuthUser(OAuthInfo oauthInfo)
        {
            DbParameter[] parms = {
									GenerateInParam("@uid",SqlDbType.Int,4,oauthInfo.Uid),
									GenerateInParam("@openid",SqlDbType.Char,50,oauthInfo.OpenId),
                                    GenerateInParam("@unionid",SqlDbType.Char,100,oauthInfo.UnionId),
                                    GenerateInParam("@server",SqlDbType.Char,10,oauthInfo.Server)	
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}createoauthuser", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 获得用户id
        /// </summary>
        /// <param name="openId">开放id</param>
        /// <param name="server">服务商</param>
        /// <returns></returns>
        public int GetUidByOpenIdAndServer(string openId, string server)
        {
            DbParameter[] parms = {
									GenerateInParam("@openid",SqlDbType.Char,50,openId),
                                    GenerateInParam("@server",SqlDbType.Char,10,server)	
								   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}getuidbyopenidandserver", RDBSHelper.RDBSTablePre),
                                                                   parms));
        }

        /// <summary>
        /// 获得开放授权用户
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public IDataReader GetOAuthUserByUid(int uid)
        {
            DbParameter[] parms = {
									GenerateInParam("@uid",SqlDbType.Int,4,uid)	
								   };
            string commandText = string.Format("SELECT {1} FROM [{0}oauth] WHERE [uid]=@uid",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.OAUTH);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 获得开放授权用户列表
        /// </summary>
        /// <param name="uidList">用户id列表</param>
        /// <returns></returns>
        public IDataReader GetOAuthUserList(string uidList)
        {
            string commandText = string.Format("SELECT {1} FROM [{0}oauth] WHERE [uid] IN ({2})",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.OAUTH,
                                                uidList);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        #endregion

        #region 用户

        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public IDataReader GetPartUserById(int uid)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4, uid)
								   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getpartuserbyid", RDBSHelper.RDBSTablePre),
                                            parms);
        }
        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="invitecode">邀请码</param>
        /// <returns></returns>
        public IDataReader GetPartUserByInviteCode(int invitecode)
        {

            string sql = string.Format(@"select b.* from {0}userinvite a 
  join {0}users b on a.uid=b.uid 
  where a.invitecode={1}", RDBSHelper.RDBSTablePre, invitecode);
            return RDBSHelper.ExecuteReader(CommandType.Text, sql);
        }
        /// <summary>
        /// 添加邀请码信息（推广页面）
        /// </summary>
        /// <param name="paccount"></param>
        /// <param name="account"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool AddInviteInfo(string paccount, string account, string code)
        {
            string sql = string.Format(@"
           if not exists(select 1 from owzx_userinvite where uid=(select uid from owzx_users where mobile ='{0}') and chaildaccount='{1}')
            begin
                INSERT INTO [owzx_userinvite]
                       ([uid]
                       ,[chaildaccount]
                       ,[invitecode]
                       )
                 VALUES
                       ((select uid from owzx_users where mobile ='{0}'),'{1}','{2}')
            end
            else
begin
update a set a.invitecode='{2}' from owzx_userinvite a where a.uid=(select uid from owzx_users where mobile ='{0}') and a.chaildaccount='{1}'
end", paccount, account, code);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, sql) == 1 ? true : false;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool DelInviteInfo(string account)
        {
            string sql = string.Format(@"
           if exists(select 1 from owzx_userinvite where chaildaccount='{0}')
            begin
                delete from owzx_userinvite where chaildaccount='{0}'
            end
            ", account);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, sql) == 1 ? true : false;
        }
        /// <summary>
        /// 验证码是否正确
        /// </summary>
        /// <param name="account"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool ValidateCode(string account, string code)
        {
            string sql = string.Format(@"
select count(1) from owzx_userinvite a where a.chaildaccount='{0}' and a.invitecode='{1}'
", account, code);
            return RDBSHelper.ExecuteScalar(CommandType.Text, sql).ToString() == "1" ? true : false;
        }
        /// <summary>
        /// 验证用户是否通过推广注册
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public DataTable ValidateUser(string account)
        {
            string sql = string.Format(@"
select * from owzx_userinvite a where a.chaildaccount='{0}'
", account);
            DataSet ds = RDBSHelper.ExecuteDataset(CommandType.Text, sql);
            if (ds.Tables.Count > 0)
            {
                return RDBSHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            }
            else
                return new DataTable();
        }

        /// <summary>
        /// 获得用户
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public DataTable GetUserById(int uid)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4, uid)
								   };
            DataSet ds = RDBSHelper.ExecuteDataset(CommandType.StoredProcedure,
                                            string.Format("{0}getuserbyid", RDBSHelper.RDBSTablePre),
                                            parms);
            if (ds == null || ds.Tables.Count == 0)
                return new DataTable();
            else
                return ds.Tables[0];
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool DelUserByUid(int uid)
        {
            string sql = string.Format(@"delete from {0}users where uid={1};
delete from {0}userdetails where uid={1};
delete from {0}onlinetime where uid={1}
delete from owzx_onlineusers where uid={1}", RDBSHelper.RDBSTablePre, uid);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, sql) == 4 ? true : false;
        }

        /// <summary>
        /// 获得用户细节
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public IDataReader GetUserDetailById(int uid)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4, uid)
								   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getuserdetailbyid", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public IDataReader GetPartUserByName(string userName)
        {
            DbParameter[] parms = {
									   GenerateInParam("@username",SqlDbType.NChar,20, userName)
								   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getpartuserbyname", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="email">用户邮箱</param>
        /// <returns></returns>
        public IDataReader GetPartUserByEmail(string email)
        {
            DbParameter[] parms = {
									   GenerateInParam("@email",SqlDbType.Char,50, email)
								   };
            return RDBSHelper.ExecuteReader(CommandType.StoredProcedure,
                                            string.Format("{0}getpartuserbyemail", RDBSHelper.RDBSTablePre),
                                            parms);
        }

        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="mobile">用户手机</param>
        /// <returns></returns>
        public DataTable GetPartUserByMobile(string mobile)
        {
            DbParameter[] parms = {
									   GenerateInParam("@mobile",SqlDbType.Char,15, mobile)
								   };
            return RDBSHelper.ExecuteTable(CommandType.StoredProcedure,
                                            string.Format("{0}getpartuserbymobile", RDBSHelper.RDBSTablePre),
                                            parms)[0];
        }

        /// <summary>
        /// 获得用户id
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public int GetUidByUserName(string userName)
        {
            DbParameter[] parms = {
									   GenerateInParam("@username",SqlDbType.NChar,20, userName)
								   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}getuidbyusername", RDBSHelper.RDBSTablePre),
                                                                   parms), -1);
        }

        /// <summary>
        /// 获得用户id
        /// </summary>
        /// <param name="email">用户邮箱</param>
        /// <returns></returns>
        public int GetUidByEmail(string email)
        {
            DbParameter[] parms = {
									   GenerateInParam("@email",SqlDbType.Char,50, email)
								   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}getuidbyemail", RDBSHelper.RDBSTablePre),
                                                                   parms), -1);
        }

        /// <summary>
        /// 获得用户id
        /// </summary>
        /// <param name="mobile">用户手机</param>
        /// <returns></returns>
        public int GetUidByMobile(string mobile)
        {
            DbParameter[] parms = {
									   GenerateInParam("@mobile",SqlDbType.Char,15, mobile)
								   };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}getuidbymobile", RDBSHelper.RDBSTablePre),
                                                                   parms), -1);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        public int CreateUser(UserInfo userInfo)
        {
            DbParameter[] parms = {
									   GenerateInParam("@username",SqlDbType.NChar,20,userInfo.UserName),
                                       GenerateInParam("@userid",SqlDbType.NChar,20,userInfo.UserId),
									   GenerateInParam("@email",SqlDbType.Char,50,userInfo.Email),
                                       GenerateInParam("@mobile",SqlDbType.Char,15,userInfo.Mobile),
									   GenerateInParam("@password",SqlDbType.Char,32,userInfo.Password),
									   GenerateInParam("@userrid",SqlDbType.Int,4,userInfo.UserRid),
                                       GenerateInParam("@admingid",SqlDbType.SmallInt,2,userInfo.AdminGid),
									   GenerateInParam("@nickname",SqlDbType.NChar,20,userInfo.NickName),
									   GenerateInParam("@avatar",SqlDbType.Char,40,userInfo.Avatar),
									   GenerateInParam("@paycredits",SqlDbType.Int,4,userInfo.PayCredits),
									   GenerateInParam("@rankcredits",SqlDbType.Int,4,userInfo.RankCredits),
									   GenerateInParam("@verifyemail",SqlDbType.TinyInt,1,userInfo.VerifyEmail),
									   GenerateInParam("@verifymobile",SqlDbType.TinyInt,1,userInfo.VerifyMobile),
									   GenerateInParam("@liftbantime",SqlDbType.DateTime,8,userInfo.LiftBanTime),
                                       GenerateInParam("@salt",SqlDbType.NChar,6,userInfo.Salt),
									   GenerateInParam("@lastvisittime",SqlDbType.DateTime,8,userInfo.LastVisitTime),
                                       GenerateInParam("@lastvisitip",SqlDbType.Char,15,userInfo.LastVisitIP),
                                       GenerateInParam("@lastvisitrgid",SqlDbType.SmallInt,2,userInfo.LastVisitRgId),
									   GenerateInParam("@registertime",SqlDbType.DateTime,8,userInfo.RegisterTime),
                                       GenerateInParam("@registerip",SqlDbType.Char,15,userInfo.RegisterIP),
                                       GenerateInParam("@registerrgid",SqlDbType.SmallInt,2,userInfo.RegisterRgId),
									   GenerateInParam("@gender",SqlDbType.TinyInt,1,userInfo.Gender),
                                       GenerateInParam("@realname",SqlDbType.NVarChar,10,userInfo.RealName),
									   GenerateInParam("@bday",SqlDbType.DateTime,8,userInfo.Bday),
                                       GenerateInParam("@idcard",SqlDbType.VarChar,18,userInfo.IdCard),
									   GenerateInParam("@regionid",SqlDbType.SmallInt,2,userInfo.RegionId),
									   GenerateInParam("@address",SqlDbType.NVarChar,150,userInfo.Address),
									   GenerateInParam("@bio",SqlDbType.NVarChar,300,userInfo.Bio),
                                       GenerateInParam("@invitecode",SqlDbType.Int,4,userInfo.InviteCode),
                                       GenerateInParam("@imei",SqlDbType.VarChar,50,userInfo.IMEI)
                                       
								   };
            object res = RDBSHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}createuser", RDBSHelper.RDBSTablePre), parms);
            //BSPLog.Instance.Write(res.ToString());
            return TypeHelper.ObjectToInt(res, -1);

        }

        ///// <summary>
        ///// 更新用户
        ///// </summary>
        ///// <returns></returns>
        //public bool UpdateUser(UserInfo userInfo)
        //{
        //    DbParameter[] parms = {
        //                               GenerateInParam("@username",SqlDbType.NChar,20,userInfo.UserName),
        //                               GenerateInParam("@email",SqlDbType.Char,50,userInfo.Email),
        //                               GenerateInParam("@mobile",SqlDbType.Char,15,userInfo.Mobile),
        //                               GenerateInParam("@password",SqlDbType.Char,32,userInfo.Password),
        //                               GenerateInParam("@userrid",SqlDbType.SmallInt,2,userInfo.UserRid),
        //                               GenerateInParam("@admingid",SqlDbType.SmallInt,2,userInfo.AdminGid),
        //                               GenerateInParam("@nickname",SqlDbType.NChar,20,userInfo.NickName),
        //                               GenerateInParam("@avatar",SqlDbType.Char,40,userInfo.Avatar),
        //                               GenerateInParam("@paycredits",SqlDbType.Int,4,userInfo.PayCredits),
        //                               GenerateInParam("@rankcredits",SqlDbType.Int,4,userInfo.RankCredits),
        //                               GenerateInParam("@verifyemail",SqlDbType.TinyInt,1,userInfo.VerifyEmail),
        //                               GenerateInParam("@verifymobile",SqlDbType.TinyInt,1,userInfo.VerifyMobile),
        //                               GenerateInParam("@liftbantime",SqlDbType.DateTime,8,userInfo.LiftBanTime),
        //                               GenerateInParam("@salt",SqlDbType.NChar,6,userInfo.Salt),
        //                               GenerateInParam("@lastvisittime",SqlDbType.DateTime,8,userInfo.LastVisitTime),
        //                               GenerateInParam("@lastvisitip",SqlDbType.Char,15,userInfo.LastVisitIP),
        //                               GenerateInParam("@lastvisitrgid",SqlDbType.SmallInt,2,userInfo.LastVisitRgId),
        //                               GenerateInParam("@registertime",SqlDbType.DateTime,8,userInfo.RegisterTime),
        //                               GenerateInParam("@registerip",SqlDbType.Char,15,userInfo.RegisterIP),
        //                               GenerateInParam("@registerrgid",SqlDbType.SmallInt,2,userInfo.RegisterRgId),
        //                               GenerateInParam("@gender",SqlDbType.TinyInt,1,userInfo.Gender),
        //                               GenerateInParam("@realname",SqlDbType.NVarChar,10,userInfo.RealName),
        //                               GenerateInParam("@bday",SqlDbType.DateTime,8,userInfo.Bday),
        //                               GenerateInParam("@idcard",SqlDbType.VarChar,18,userInfo.IdCard),
        //                               GenerateInParam("@regionid",SqlDbType.SmallInt,2,userInfo.RegionId),
        //                               GenerateInParam("@address",SqlDbType.NVarChar,150,userInfo.Address),
        //                               GenerateInParam("@bio",SqlDbType.NVarChar,300,userInfo.Bio),
        //                               GenerateInParam("@uid",SqlDbType.Int,4,userInfo.Uid),
        //                               GenerateInParam("@compnum",SqlDbType.Int,4,userInfo.CompNum),
        //                               GenerateInParam("@shareratio",SqlDbType.Int,4,userInfo.Shareratio)
        //                           };

        //    return RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
        //                               string.Format("{0}updateuser", RDBSHelper.RDBSTablePre),
        //                               parms) == 2 ? true : false; ;
        //}

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <returns></returns>
        public bool UpdateUser(UserInfo userInfo)
        {
            DbParameter[] parms = {
									   GenerateInParam("@username",SqlDbType.NChar,20,userInfo.UserName),
                                       GenerateInParam("@mobile",SqlDbType.Char,15,userInfo.Mobile),
									   GenerateInParam("@password",SqlDbType.Char,32,userInfo.Password),
									   GenerateInParam("@userrid",SqlDbType.SmallInt,2,userInfo.UserRid),
                                       GenerateInParam("@admingid",SqlDbType.SmallInt,2,userInfo.AdminGid),
									   GenerateInParam("@nickname",SqlDbType.NChar,20,userInfo.NickName),
									   GenerateInParam("@uid",SqlDbType.Int,4,userInfo.Uid), 
                                       GenerateInParam("@qq",SqlDbType.VarChar,50,userInfo.QQ), 
                                       GenerateInParam("@imei",SqlDbType.VarChar,50,userInfo.IMEI),
                                       GenerateInParam("@usertype",SqlDbType.SmallInt,2,userInfo.UserType)
                                   };

            string result = RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                       string.Format("{0}updateuserpartinfo", RDBSHelper.RDBSTablePre),
                                       parms).ToString();
            if (result.EndsWith("成功"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 更新部分用户
        /// </summary>
        /// <returns></returns>
        public bool UpdatePartUser(PartUserInfo partUserInfo)
        {
            DbParameter[] parms = {
									   GenerateInParam("@username",SqlDbType.NChar,20,partUserInfo.UserName),
									   GenerateInParam("@email",SqlDbType.Char,50,partUserInfo.Email),
                                       GenerateInParam("@mobile",SqlDbType.Char,15,partUserInfo.Mobile),
									   GenerateInParam("@password",SqlDbType.Char,32,partUserInfo.Password),
									   GenerateInParam("@userrid",SqlDbType.SmallInt,2,partUserInfo.UserRid),
                                       GenerateInParam("@admingid",SqlDbType.SmallInt,2,partUserInfo.AdminGid),
									   GenerateInParam("@nickname",SqlDbType.NChar,20,partUserInfo.NickName),
									   GenerateInParam("@avatar",SqlDbType.Char,40,partUserInfo.Avatar),
									   GenerateInParam("@paycredits",SqlDbType.Int,4,partUserInfo.PayCredits),
									   GenerateInParam("@rankcredits",SqlDbType.Int,4,partUserInfo.RankCredits),
									   GenerateInParam("@verifyemail",SqlDbType.TinyInt,1,partUserInfo.VerifyEmail),
									   GenerateInParam("@verifymobile",SqlDbType.TinyInt,1,partUserInfo.VerifyMobile),
									   GenerateInParam("@liftbantime",SqlDbType.DateTime,8,partUserInfo.LiftBanTime),
                                       GenerateInParam("@salt",SqlDbType.NChar,6,partUserInfo.Salt),
									   GenerateInParam("@uid",SqlDbType.Int,4,partUserInfo.Uid),
                                       GenerateInParam("@imei",SqlDbType.VarChar,50,partUserInfo.IMEI)
								   };

            string result = RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                      string.Format("{0}updatepartuser", RDBSHelper.RDBSTablePre),
                                      parms).ToString();
            if (result.EndsWith("成功"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 更新用户细节
        /// </summary>
        /// <returns></returns>
        public bool UpdateUserDetail(UserDetailInfo userDetailInfo)
        {
            DbParameter[] parms = {
                                       GenerateInParam("@lastvisittime",SqlDbType.DateTime,8,userDetailInfo.LastVisitTime),
                                       GenerateInParam("@lastvisitip",SqlDbType.Char,15,userDetailInfo.LastVisitIP),
                                       GenerateInParam("@lastvisitrgid",SqlDbType.SmallInt,2,userDetailInfo.LastVisitRgId),
									   GenerateInParam("@registertime",SqlDbType.DateTime,8,userDetailInfo.RegisterTime),
                                       GenerateInParam("@registerip",SqlDbType.Char,15,userDetailInfo.RegisterIP),
                                       GenerateInParam("@registerrgid",SqlDbType.SmallInt,2,userDetailInfo.RegisterRgId),
									   GenerateInParam("@gender",SqlDbType.TinyInt,1,userDetailInfo.Gender),
                                       GenerateInParam("@realname",SqlDbType.NVarChar,10,userDetailInfo.RealName),
									   GenerateInParam("@bday",SqlDbType.DateTime,8,userDetailInfo.Bday),
                                       GenerateInParam("@idcard",SqlDbType.VarChar,18,userDetailInfo.IdCard),
									   GenerateInParam("@regionid",SqlDbType.SmallInt,2,userDetailInfo.RegionId),
									   GenerateInParam("@address",SqlDbType.NVarChar,150,userDetailInfo.Address),
									   GenerateInParam("@bio",SqlDbType.NVarChar,300,userDetailInfo.Bio),
									   GenerateInParam("@uid",SqlDbType.Int,4,userDetailInfo.Uid),
                                       GenerateInParam("@signname",SqlDbType.VarChar,100,userDetailInfo.SignName)
								   };

            string result = RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                      string.Format("{0}updateuserdetail", RDBSHelper.RDBSTablePre),
                                      parms).ToString();
            if (result.EndsWith("成功"))
                return true;
            else
                return false;
        }



        /// <summary>
        /// 更新用户细节
        /// </summary>
        /// <returns></returns>
        public bool UpdateUserVerifyLog(int uid = -1, int isveritylog = 0, int regionid = 0, int regionidtwo = 0)
        {
            string commandText = string.Format(@" begin try 
 update owzx_users set verifyloginrg={1} where uid={0}
 update owzx_userdetails set regionid={2},regionidtwo={3}   where uid={0}
select @@rowcount state
end try
begin catch select -1 state end catch ", uid, isveritylog, regionid, regionidtwo);

            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText), 0)>0;  
        }

        public string GetRegionName(int uid)
        {
            string commandText = string.Format(@" begin try 

   declare  @allname varchar(50)='' ,@res varchar(50)='',@i int=0 ,@kk varchar(30)
if({0}>0)
begin
    select top 1 @res= case when regionid>0 then  cast( regionid as varchar) else ' ' end +','+
    case when regionidtwo>0 then  cast( regionidtwo as varchar) else ' ' end +',' 
    from  owzx_userdetails where uid={0}
    if(@res!=' , ,')
    set @i=charindex(',',@res)
    while @i>0
    begin
        declare @name varchar(50)=''
        set @kk= SUBSTRING(@res,0,@i)
        select  @name=ISNULL(name,'')  from owzx_regions where layer=3 and regionid=cast(@kk as int)
        if(@name='')
        begin 
            select @name=ISNULL(name,'')  from owzx_regions where layer=2 and regionid=cast(@kk as int)
	        if(@name='')
	        begin 
		        select @name=ISNULL(name,'')  from owzx_regions where layer=1 and regionid=cast(@kk as int)
	        end
        end
        if(@name!='')  set @allname= ltrim(rtrim(@name))+','+@allname  
        set @res=substring(@res,@i+1,LEN(@res))	
        set @i=charindex(',',@res,0)	
    end
end
select @allname state
end try
begin catch select '查询错误' state end catch ", uid);

            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText).ToString();
        }

        /// <summary>
        /// 更新用户最后访问
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="visitTime">访问时间</param>
        /// <param name="ip">ip</param>
        /// <param name="regionId">区域id</param>
        public void UpdateUserLastVisit(int uid, DateTime visitTime, string ip, int regionId)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4,uid),
									   GenerateInParam("@visittime",SqlDbType.DateTime,8,visitTime),
                                       GenerateInParam("@ip",SqlDbType.Char,15,ip),
									   GenerateInParam("@regionid",SqlDbType.SmallInt,2,regionId)
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateuserlastvisit", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 后台获得用户列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public DataTable AdminGetUserList(int pageSize, int pageNumber, string condition)
        {
            bool noCondition = string.IsNullOrWhiteSpace(condition);
            string commandText;
            if (pageNumber == 1)
            {
                if (noCondition)
                    commandText = string.Format(@"SELECT TOP {0} [{1}users].[uid],[{1}users].[username],[{1}users].[email],[{1}users].[mobile],[{1}users].[userrid],
[{1}users].[admingid],[{1}users].[nickname],[{1}users].[depid],c.name depname,[{1}users].[paycredits],[{1}users].[rankcredits],[{1}userdetails].[lastvisittime],[{1}userdetails].[lastvisitip],
[{1}userdetails].[registertime],[{1}userdetails].[gender],[{1}userdetails].[realname],[{1}userranks].[title] AS [utitle],[{1}admingroups].[title] AS [atitle] 
FROM [{1}users] LEFT JOIN [{1}userdetails] ON [{1}userdetails].[uid] = [{1}users].[uid]  
LEFT JOIN [{1}userranks] ON [{1}userranks].[userrid]=[{1}users].[userrid] 
LEFT JOIN [{1}admingroups] ON [{1}admingroups].[admingid]=[{1}users].[admingid]
left join {1}sys_department c on [{1}users].depid=c.depid
ORDER BY [{1}users].[uid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre);

                else
                    commandText = string.Format(@"SELECT TOP {0} [{1}users].[uid],[{1}users].[username],[{1}users].[email],[{1}users].[mobile],[{1}users].[userrid],
[{1}users].[admingid],[{1}users].[nickname],[{1}users].[depid],c.name depname,[{1}users].[paycredits],[{1}users].[rankcredits],[{1}userdetails].[lastvisittime],[{1}userdetails].[lastvisitip],
[{1}userdetails].[registertime],[{1}userdetails].[gender],[{1}userdetails].[realname],[{1}userranks].[title] AS [utitle],
[{1}admingroups].[title] AS [atitle] 
FROM [{1}users] LEFT JOIN [{1}userdetails] ON [{1}userdetails].[uid] = [{1}users].[uid]  
LEFT JOIN [{1}userranks] ON [{1}userranks].[userrid]=[{1}users].[userrid]  
LEFT JOIN [{1}admingroups] ON [{1}admingroups].[admingid]=[{1}users].[admingid]
left join {1}sys_department c on [{1}users].depid=c.depid
WHERE {2} ORDER BY [{1}users].[uid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre, condition);
            }
            else
            {
                if (noCondition)
                    commandText = string.Format(@"SELECT TOP {0} [{1}users].[uid],[{1}users].[username],[{1}users].[email],[{1}users].[mobile],[{1}users].[userrid],
[{1}users].[admingid],[{1}users].[nickname],[{1}users].[depid],c.name depname,[{1}users].[paycredits],[{1}users].[rankcredits],[{1}userdetails].[lastvisittime],[{1}userdetails].[lastvisitip],
[{1}userdetails].[registertime],[{1}userdetails].[gender],[{1}userdetails].[realname],[{1}userranks].[title] AS [utitle],[{1}admingroups].[title] AS [atitle] 
FROM [{1}users],[{1}userdetails],[{1}userranks],[{1}admingroups] 
left join {1}sys_department c on [{1}users].depid=c.depid
WHERE [{1}userdetails].[uid] = [{1}users].[uid] AND  [{1}userranks].[userrid]=[{1}users].[userrid] AND  
[{1}admingroups].[admingid]=[{1}users].[admingid] AND 
[{1}users].[uid] < (SELECT min([uid])  FROM (SELECT TOP {2} [uid] FROM [{1}users] ORDER BY [uid] DESC) AS temp ) ORDER BY [{1}users].[uid] DESC",
                                                pageSize,
                                                RDBSHelper.RDBSTablePre,
                                                (pageNumber - 1) * pageSize);
                else
                    commandText = string.Format(@"
select * from (
SELECT ROW_NUMBER() over(order by [{1}users].[uid] desc) id, [{1}users].[uid],[{1}users].[username],[{1}users].[email],[{1}users].[mobile],[{1}users].[userrid],[{1}users].[storeid],
[{1}users].[admingid],[{1}users].[nickname],[{1}users].[depid],c.name depname,[{1}users].[paycredits],[{1}users].[rankcredits],[{1}userdetails].[lastvisittime],
[{1}userdetails].[lastvisitip],[{1}userdetails].[registertime],[{1}userdetails].[gender],[{1}userdetails].[realname],
[{1}userranks].[title] AS [utitle],[{1}admingroups].[title] AS [atitle],
[{1}userdetails].[qqnum],[{1}userdetails].[wxnum] 
FROM [{1}users],[{1}userdetails],[{1}userranks],[{1}admingroups]  
left join {1}sys_department c on [{1}users].depid=c.depid
WHERE {3} and [{1}userdetails].[uid] = [{1}users].[uid] 
AND  [{1}userranks].[userrid]=[{1}users].[userrid] 
AND  [{1}admingroups].[admingid]=[{1}users].[admingid] ) temp
where  id>{0}*({2}-1) and id<={0}*({2}) ",
                                               pageSize,
                                               RDBSHelper.RDBSTablePre,
                                               pageNumber,
                                               condition);
            }

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 后台获得用户列表条件
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="email">邮箱</param>
        /// <param name="mobile">手机</param>
        /// <param name="userRid">用户等级</param>
        /// <param name="adminGid">管理员组</param>
        /// <returns></returns>
        public string AdminGetUserListCondition(string userName, string email, string mobile, int userRid, int adminGid)
        {
            StringBuilder condition = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(userName) && SecureHelper.IsSafeSqlString(userName))
                condition.AppendFormat(" AND [{1}users].[username] like '{0}%' ", userName, RDBSHelper.RDBSTablePre);

            if (!string.IsNullOrWhiteSpace(email) && SecureHelper.IsSafeSqlString(email, false))
                condition.AppendFormat(" AND [{1}users].[email] like '{0}%' ", email, RDBSHelper.RDBSTablePre);

            if (!string.IsNullOrWhiteSpace(mobile) && SecureHelper.IsSafeSqlString(mobile))
                condition.AppendFormat(" AND [{1}users].[mobile] like '{0}%' ", mobile, RDBSHelper.RDBSTablePre);

            if (userRid > 0)
                condition.AppendFormat(" AND [{1}users].[userrid] >= '{0}' ", userRid, RDBSHelper.RDBSTablePre);

            if (adminGid > 0)
                condition.AppendFormat(" AND [{1}users].[admingid] <= '{0}' ", adminGid, RDBSHelper.RDBSTablePre);

            return condition.Length > 0 ? condition.Remove(0, 4).ToString() : "";
        }

        /// <summary>
        /// 后台获得用户列表数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public int AdminGetUserCount(string condition)
        {
            string commandText;
            if (string.IsNullOrWhiteSpace(condition))
                commandText = string.Format(@"SELECT COUNT([{0}users].[uid]) FROM [{0}users],[{0}userdetails],[{0}userranks],[{0}admingroups]  
WHERE  [{0}userdetails].[uid] = [{0}users].[uid] 
AND  [{0}userranks].[userrid]=[{0}users].[userrid] 
AND  [{0}admingroups].[admingid]=[{0}users].[admingid]", RDBSHelper.RDBSTablePre);
            else
                commandText = string.Format(@"SELECT COUNT([{0}users].[uid]) FROM [{0}users],[{0}userdetails],[{0}userranks],[{0}admingroups]  
WHERE  {1} and [{0}userdetails].[uid] = [{0}users].[uid] 
AND  [{0}userranks].[userrid]=[{0}users].[userrid] 
AND  [{0}admingroups].[admingid]=[{0}users].[admingid] ", RDBSHelper.RDBSTablePre, condition);

            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText), 0);
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pageSize">-1时 获取全部数据</param>
        /// <param name="pageNumber"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable GetUserList(int pageSize, int pageNumber, ref long SumFee, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };
            string sql = string.Format(@"begin try 


if OBJECT_ID('tempdb..#list') is not null
  drop table #list

select ROW_NUMBER() over(order by a.totalmoney desc,a.uid desc) id, a.uid,a.username,a.mobile,a.nickname,a.totalmoney,a.qq ,a.email,a.usertype,a.bankmoney,
convert(varchar(25),b.registertime,120) registertime,convert(varchar(25),b.lastvisittime,120) lastvisittime,c.title AS admingtitle
into #list
from owzx_users a
join owzx_userdetails b on a.uid=b.uid 
LEFT JOIN [owzx_admingroups] c ON c.[admingid]=a.[admingid] {0}


if(@pagesize=-1)
begin
select * ,(select count(1) from #list) TotalCount from #list 
end
else
begin
select * ,(select count(1) from #list) TotalCount from #list where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end

select  sum(isnull(totalmoney,0))+ sum(isnull(bankmoney,0)) SumFee from  owzx_users  a  {0}

end try
begin catch

select ERROR_MESSAGE() error
end catch



", condition);

            DataSet ds = RDBSHelper.ExecuteDataset(CommandType.Text, sql, parms);
            if (ds == null || ds.Tables.Count < 1)
            {
                SumFee = 0;
                return new DataTable();
            }
            else
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    SumFee = Convert.ToInt64(ds.Tables[1].Rows[0][0]);
                }
                else { SumFee = 0; }
                return ds.Tables[0];
            }
        }

        /// <summary>
        /// 获取用户和父级列表
        /// </summary>
        /// <param name="pageSize">-1时 获取全部数据</param>
        /// <param name="pageNumber"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable GetUserParentList(int pageSize, int pageNumber, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };
            string sql = string.Format(@"begin try 


if OBJECT_ID('tempdb..#list') is not null
  drop table #list

select ROW_NUMBER() over(order by a.uid desc) id, a.uid,a.username,a.mobile,a.nickname,a.totalmoney,a.qq ,a.email,a.usertype,a.bankmoney,
convert(varchar(25),b.registertime,120) registertime,convert(varchar(25),b.lastvisittime,120) lastvisittime,c.title AS admingtitle,d.Email as Pemail,d.nickname as Pnickname 
into #list
from owzx_users a
join owzx_users d on a.ParentID=d.Uid  
join owzx_userdetails b on a.uid=b.uid 
LEFT JOIN [owzx_admingroups] c ON c.[admingid]=a.[admingid] {0}


if(@pagesize=-1)
begin
select * ,(select count(1) from #list) TotalCount from #list 
end
else
begin
select * ,(select count(1) from #list) TotalCount from #list where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end


end try
begin catch

select ERROR_MESSAGE() error
end catch



", condition);

            DataSet ds = RDBSHelper.ExecuteDataset(CommandType.Text, sql, parms);
            if (ds == null || ds.Tables.Count == 0)
                return new DataTable();
            else
                return ds.Tables[0];
        }
        /// <summary>
        /// 获取团队列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable GetTeam(int pageSize, int pageNumber, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };

            string sql = string.Format(@"
if OBJECT_ID('tempdb..#list') is not null
drop table #list
--用户信息
select ROW_NUMBER() over(order by a.uid desc) id, a.uid,a.mobile,a.nickname,c.title,0 ctotal,0 cctotal
into #list
from owzx_users a
join owzx_userdetails b on a.uid=b.uid and isnull(a.mobile,'')!='' and a.admingid=1
left join owzx_userranks c on a.userrid=c.userrid
{0}

--一级好友
if OBJECT_ID('tempdb..#listchild') is not null
drop table #listchild

select tt.uid puid,a.uid,a.mobile,c.title
into #listchild
from #list tt
join owzx_userdetails b on b.level=2 and charindex(','+cast(ltrim(rtrim(tt.uid)) as varchar(10))+',',',' + cast(ltrim(rtrim(b.puid)) as varchar(10))+ ',')>0 
join owzx_users a on a.uid=b.uid 
left join owzx_userranks c on a.userrid=c.userrid 

--二级好友
if OBJECT_ID('tempdb..#listthlevel') is not null
drop table #listthlevel

select tt.uid puid,aa.uid ,aa.mobile ,cc.title
into #listthlevel
from #listchild tt
join owzx_userdetails bb on bb.level=3 and charindex(','+cast(ltrim(rtrim(tt.uid)) as varchar(10))+',',',' + cast(ltrim(rtrim(bb.puid)) as varchar(10))+ ',')>0 
join owzx_users aa on aa.uid=bb.uid 
left join owzx_userranks cc on aa.userrid=cc.userrid

--一，二级合并
if OBJECT_ID('tempdb..#level') is not null
drop table #level


select a.puid,a.uid,a.mobile,a.title ,b.uid cuid,b.mobile cmobile,b.title ctitle
into #level
from #listchild a
left join #listthlevel b on a.uid=b.puid

--去除 #listchild表中在#level 中已存在的记录
delete a from #listchild a
join #level b on a.uid=b.uid and a.puid=b.puid 


if exists(select 1 from #listchild)
begin
update a
set a.ctotal=(select COUNT(1) from #listchild)
from #list a
end
if exists(select 1 from #listthlevel)
begin
update a
set a.cctotal=(select COUNT(1) from #listthlevel)
from #list a
end

if OBJECT_ID('tempdb..#result') is not null
drop table #result

select a.id,a.uid,a.mobile,a.nickname,a.title, isnull(b.uid,0) cuid,isnull(b.mobile,'') cmobile,isnull(b.title,'') cuserrank,isnull(b.cuid,0) ccuid,
isnull(b.cmobile,'') ccmobile,isnull(b.ctitle,'') ccuserrank
,ctotal,cctotal,(select count(1) from #list) TotalCount
into #result
from #list a
left join #level b on a.uid=b.puid
where not exists(select 1 from #listchild c where a.uid=c.puid)--一级子级
and not exists(select 1 from #listthlevel d where a.uid=d.puid) --二级子级
union all
select a.id,a.uid,a.mobile,a.nickname,a.title, b.uid cuid,b.mobile cmobile,b.title cuserrank,0 ccuid,'' ccmobile,'' ccuserrank
,ctotal,cctotal,(select count(1) from #list) TotalCount
from #list a
join #listchild b on a.uid=b.puid
union all
select a.id,a.uid,a.mobile,a.nickname,a.title, b.uid cuid,b.mobile cmobile,b.title cuserrank,0 ccuid,'' ccmobile,'' ccuserrank
,ctotal,cctotal,(select count(1) from #list) TotalCount
from #list a
join #listthlevel b on a.uid=b.puid



if(@pagesize=-1)
begin
select * from #result order by id 
end
else
begin
select * from #result where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex  order by id 
end", condition);
            DataSet ds = RDBSHelper.ExecuteDataset(CommandType.Text, sql, parms);
            if (ds == null || ds.Tables.Count == 0)
                return new DataTable();
            else
                return ds.Tables[0];
        }
        /// <summary>
        /// 获得用户等级下用户的数量
        /// </summary>
        /// <param name="userRid">用户等级id</param>
        /// <returns></returns>
        public int GetUserCountByUserRid(int userRid)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@userrid", SqlDbType.SmallInt, 2, userRid)    
                                    };
            string commandText = string.Format("SELECT COUNT([uid]) FROM [{0}users] WHERE [userrid]=@userrid",
                                                RDBSHelper.RDBSTablePre);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms));
        }

        /// <summary>
        /// 获得管理员组下用户的数量
        /// </summary>
        /// <param name="adminGid">管理员组id</param>
        /// <returns></returns>
        public int GetUserCountByAdminGid(int adminGid)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@admingid", SqlDbType.SmallInt, 2, adminGid)    
                                    };
            string commandText = string.Format("SELECT COUNT([uid]) FROM [{0}users] WHERE [admingid]=@admingid",
                                                RDBSHelper.RDBSTablePre);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms));
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="userName">用户名</param>
        /// <param name="nickName">昵称</param>
        /// <param name="avatar">头像</param>
        /// <param name="gender">性别</param>
        /// <param name="realName">真实名称</param>
        /// <param name="bday">出生日期</param>
        /// <param name="idCard">The id card.</param>
        /// <param name="regionId">区域id</param>
        /// <param name="address">所在地</param>
        /// <param name="bio">简介</param>
        /// <returns></returns>
        public bool UpdateUser(int uid,  string nickName, string avatar, int gender, string realName,  string mobile, string qq )
        {
            DbParameter[] parms = { 
									   GenerateInParam("@nickname",SqlDbType.NChar,20,nickName),
									   GenerateInParam("@avatar",SqlDbType.Char,40,avatar),
									   GenerateInParam("@gender",SqlDbType.TinyInt,1,gender),
                                       GenerateInParam("@realname",SqlDbType.NVarChar,10,realName),
									   GenerateInParam("@qq",SqlDbType.VarChar,18,qq),
									   GenerateInParam("@mobile",SqlDbType.VarChar,18,mobile),  
									   GenerateInParam("@uid",SqlDbType.Int,4,uid),
								   };

            return RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                              string.Format("{0}updateucenteruser", RDBSHelper.RDBSTablePre),
                                              parms) > 0;
        }

        /// <summary>
        /// 更新用户邮箱
        /// </summary>
        /// <param name="uid">用户id.</param>
        /// <param name="email">邮箱</param>
        public void UpdateUserEmailByUid(int uid, string email)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4, uid),
									   GenerateInParam("@email",SqlDbType.Char,50, email)
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateuseremailbyuid", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 更新用户手机
        /// </summary>
        /// <param name="uid">用户id.</param>
        /// <param name="mobile">手机</param>
        public void UpdateUserMobileByUid(int uid, string mobile)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4, uid),
									   GenerateInParam("@mobile",SqlDbType.Char,15, mobile)
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateusermobilebyuid", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="uid">用户id.</param>
        /// <param name="password">密码</param>
        public void UpdateUserPasswordByUid(int uid, string password)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4, uid),
									   GenerateInParam("@password",SqlDbType.Char,32, password)
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateuserpasswordbyuid", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        public void UpdateUserSafePasswordByUid(int uid, string password)
        {
            DbParameter[] parms = {
                                       GenerateInParam("@uid",SqlDbType.Int,4, uid),
                                       GenerateInParam("@safepassword",SqlDbType.Char,32, password)
                                   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateusersafepasswordbyuid", RDBSHelper.RDBSTablePre),
                                       parms);
        }
        public string BankChange(int uid, decimal changefee, int type = 0)
        { 
            DbParameter[] parms = {
                                       GenerateInParam("@uid",SqlDbType.Int,4, uid),
                                       GenerateInParam("@type",SqlDbType.Int,4, type),
                                       GenerateInParam("@changefee",SqlDbType.Decimal,32, changefee),
                                       GenerateOutParam("@msg",SqlDbType.VarChar, 500)
                                   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                string.Format("{0}updateuserchange", RDBSHelper.RDBSTablePre),
                parms);
            return parms[3].Value.ToString();
        }
        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="mobile">用户手机号</param>
        /// <param name="password">密码</param>
        public bool UpdateUserPasswordByMobile(string mobile, string password)
        {
            string sql = string.Format(@"update owzx_users set password='{1}' where mobile='{0}'", mobile, password);
            return RDBSHelper.ExecuteNonQuery(sql) == 1 ? true : false;
        }

        /// <summary>
        /// 更新用户解禁时间
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="liftBanTime">解禁时间</param>
        public void UpdateUserLiftBanTimeByUid(int uid, DateTime liftBanTime)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4, uid),
									   GenerateInParam("@liftbantime",SqlDbType.DateTime,8, liftBanTime)
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateuserliftbantimebyuid", RDBSHelper.RDBSTablePre),
                                       parms);
        }

        /// <summary>
        /// 更新用户等级
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="userRid">用户等级id</param>
        public void UpdateUserRankByUid(int uid, int userRid)
        {
            DbParameter[] parms = {
									   GenerateInParam("@uid",SqlDbType.Int,4, uid),
									   GenerateInParam("@userrid",SqlDbType.SmallInt,2, userRid)
								   };
            RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                       string.Format("{0}updateuserrankbyuid", RDBSHelper.RDBSTablePre),
                                       parms);
        }
        /// <summary>
        /// 更新用户等级
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="userRid">用户等级id</param>
        public bool UpdateUserAccount(int uid, decimal fee)
        {
            string commandText = string.Format(" UPDATE [{0}users] SET totalmoney=totalmoney+{1} WHERE [uid]={2} ",
                                               RDBSHelper.RDBSTablePre, fee, uid);
           return  RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText)>0;
        }

        public bool UpdateUserLower(int uid, string logaccount)
        {
            string commandText = string.Format(@"
begin try
 begin tran
declare @pid int =0,@pids varchar(100)='',@pids2 varchar(100)=''

select @pid=uid from owzx_users where rtrim(email)='{1}'
if(@pid=0)
begin
    select @pid=uid from owzx_users where uid='{1}'
end
if(@pid=0)
select 0
else
begin

UPDATE [{0}users] SET parentid=@pid WHERE [uid]={2} 
select @pids=ltrim(rtrim(puid)) from [{0}userdetails] WHERE [uid]={2} 
declare @tempp varchar(100)='',@str varchar(100)=''
select  @str=ltrim(rtrim(puid)) from [{0}userdetails] where uid= @pid

if( charindex(',' ,@pids)>0)
begin
--下级代理
    if(@str='0')  begin  set @str=cast(@pid as varchar)  end 
    else begin  set @str=@str+','+cast(@pid as varchar) end  
    UPDATE [{0}userdetails] SET puid=@str WHERE [uid]={2}  
    set @tempp=','+@pids+','+ ltrim(rtrim(cast({2} as varchar)))+','
    update [{0}userdetails] set puid=replace(puid,','+@pids+','+ltrim(rtrim(cast({2} as varchar)))+',' , @tempp) WHERE puid like '%'+@tempp
end
else 
begin
--总代   
     if(@str='0')  begin  set @str=cast(@pid as varchar)  end 
    else begin  set @str=@str+','+cast(@pid as varchar) end  
    UPDATE [{0}userdetails] SET puid=@str WHERE [uid]={2}  
    set @str=@str+','+ltrim(rtrim(cast({2} as varchar))) 
    update [{0}userdetails] set puid=@str WHERE [uid] in (select uid from [{0}users] where parentid={2})
    update [{0}userdetails] set puid=replace(puid,@pids+','+ltrim(rtrim(cast({2} as varchar)))+',',@str+',') WHERE puid like '%'+ltrim(rtrim(cast(@pid as varchar)))+','+ltrim(rtrim(cast({2} as varchar)))+','
end 
end 
  if (@@error<>0 )
    begin
        select 0
        Rollback Tran
    end
    else
    begin 
        select 1
        commit tran    
    end
end try
begin catch
	select 0
	Rollback Tran
end catch
",
                                               RDBSHelper.RDBSTablePre, logaccount, uid);
            return RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText) > 0;
        }
        /// <summary>
        /// 更新用户在线时间
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="onlineTime">在线时间</param>
        /// <param name="updateTime">更新时间</param>
        public void UpdateUserOnlineTime(int uid, int onlineTime, DateTime updateTime)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@uid", SqlDbType.Int, 4, uid),
                                    GenerateInParam("@onlinetime", SqlDbType.Int, 4, onlineTime),
                                    GenerateInParam("@updatetime", SqlDbType.DateTime, 8, updateTime)
                                   };
            string commandText = string.Format("UPDATE [{0}onlinetime] SET [total]=[total]+@onlinetime,[year]=[year]+@onlinetime,[month]=[month]+@onlinetime,[week]=[week]+@onlinetime,[day]=[day]+@onlinetime,[updatetime]=@updatetime WHERE [uid]=@uid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 通过注册ip获得注册时间
        /// </summary>
        /// <param name="registerIP">注册ip</param>
        /// <returns></returns>
        public DateTime GetRegisterTimeByRegisterIP(string registerIP)
        {
            DbParameter[] parms = {
									GenerateInParam("@registerip",SqlDbType.Char,15, registerIP)
								   };
            return TypeHelper.ObjectToDateTime(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                        string.Format("{0}getregistertimebyregisterip", RDBSHelper.RDBSTablePre),
                                                                        parms), DateTime.Now.AddDays(-1));
        }

        /// <summary>
        /// 获得用户最后访问时间
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public DateTime GetUserLastVisitTimeByUid(int uid)
        {
            DbParameter[] parms = {
									GenerateInParam("@uid",SqlDbType.Int,4, uid)
								   };
            return TypeHelper.ObjectToDateTime(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                        string.Format("{0}getuserlastvisittimebyuid", RDBSHelper.RDBSTablePre),
                                                                        parms));
        }

        #endregion

        #region 用户等级

        /// <summary>
        /// 获得用户等级列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetUserRankList()
        {
            string commandText = string.Format("SELECT {1} FROM [{0}userranks] ORDER BY [system] DESC",
                                                RDBSHelper.RDBSTablePre,
                                                RDBSFields.USER_RANKS);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }

        /// <summary>
        /// 创建用户等级
        /// </summary>
        public void CreateUserRank(UserRankInfo userRankInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@system", SqlDbType.Int, 4, userRankInfo.System),
                                        GenerateInParam("@title", SqlDbType.NChar,50,userRankInfo.Title),
                                        GenerateInParam("@avatar", SqlDbType.Char,50,userRankInfo.Avatar),
                                        GenerateInParam("@creditslower", SqlDbType.Int, 4, userRankInfo.CreditsLower),
                                        GenerateInParam("@creditsupper", SqlDbType.Int,4,userRankInfo.CreditsUpper),
                                        GenerateInParam("@limitdays", SqlDbType.Int,4,userRankInfo.LimitDays)
                                    };
            string commandText = string.Format("INSERT INTO [{0}userranks]([system],[title],[avatar],[creditslower],[creditsupper],[limitdays]) VALUES(@system,@title,@avatar,@creditslower,@creditsupper,@limitdays)",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 删除用户等级
        /// </summary>
        /// <param name="userRid">用户等级id</param>
        public void DeleteUserRankById(int userRid)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@userrid", SqlDbType.SmallInt, 2, userRid)    
                                    };
            string commandText = string.Format("DELETE FROM [{0}userranks] WHERE [userrid]=@userrid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新用户等级
        /// </summary>
        public void UpdateUserRank(UserRankInfo userRankInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@system", SqlDbType.Int, 4, userRankInfo.System),
                                        GenerateInParam("@title", SqlDbType.NChar,50,userRankInfo.Title),
                                        GenerateInParam("@avatar", SqlDbType.Char,50,userRankInfo.Avatar),
                                        GenerateInParam("@creditslower", SqlDbType.Int, 4, userRankInfo.CreditsLower),
                                        GenerateInParam("@creditsupper", SqlDbType.Int,4,userRankInfo.CreditsUpper),
                                        GenerateInParam("@limitdays", SqlDbType.Int,4,userRankInfo.LimitDays),
                                        GenerateInParam("@userrid", SqlDbType.SmallInt, 2, userRankInfo.UserRid)    
                                    };

            string commandText = string.Format("UPDATE [{0}userranks] SET [system]=@system,[title]=@title,[avatar]=@avatar,[creditslower]=@creditslower,[creditsupper]=@creditsupper,[limitdays]=@limitdays WHERE [userrid]=@userrid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        #endregion

        #region 管理员组

        /// <summary>
        /// 获得管理员组列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetAdminGroupList()
        {
            //string commandText = string.Format("SELECT {0},isnull(b.name,'选择部门') DepName FROM [{1}admingroups] a left join {1}sys_department b on a.depid=b.depid {2}",
            //                                    RDBSFields.ADMIN_GROUPS,
            //                                    RDBSHelper.RDBSTablePre,(DepId==-1?"" :" where a.depid="+DepId));
            string commandText = string.Format("SELECT {0} FROM [{1}admingroups] a ",
                                                RDBSFields.ADMIN_GROUPS,
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText).Tables[0];
        }

        /// <summary>
        /// 创建管理员组
        /// </summary>
        /// <param name="adminGroupInfo">管理员组信息</param>
        /// <returns></returns>
        public int CreateAdminGroup(AdminGroupInfo adminGroupInfo)
        {
            //DbParameter[] parms = {
            //                            GenerateInParam("@title", SqlDbType.NChar,50,adminGroupInfo.Title),
            //                            GenerateInParam("@actionlist", SqlDbType.Text, 0, adminGroupInfo.ActionList),
            //                            GenerateInParam("@cusactionlist", SqlDbType.Text, 0, adminGroupInfo.CusActionList),
            //                            GenerateInParam("@depid", SqlDbType.Int, 4, adminGroupInfo.DepId)
            //                        };
            //string commandText = string.Format("INSERT INTO [{0}admingroups]([title],[actionlist],[depid],[cusactionlist]) VALUES(@title,@actionlist,@depid,@cusactionlist);SELECT SCOPE_IDENTITY();",
            //                                    RDBSHelper.RDBSTablePre);
            DbParameter[] parms = {
                                        GenerateInParam("@title", SqlDbType.NChar,50,adminGroupInfo.Title),
                                        GenerateInParam("@actionlist", SqlDbType.Text, 0, adminGroupInfo.ActionList)
                                    };
            string commandText = string.Format("INSERT INTO [{0}admingroups]([title],[actionlist]) VALUES(@title,@actionlist);SELECT SCOPE_IDENTITY();",
                                                RDBSHelper.RDBSTablePre);
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms));
        }

        /// <summary>
        /// 删除管理员组
        /// </summary>
        /// <param name="adminGid">管理员组id</param>
        public void DeleteAdminGroupById(int adminGid)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@admingid", SqlDbType.SmallInt, 2, adminGid)    
                                    };
            string commandText = string.Format("DELETE FROM [{0}admingroups] WHERE [admingid]=@admingid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 更新管理员组
        /// </summary>
        public void UpdateAdminGroup(AdminGroupInfo adminGroupInfo)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@title", SqlDbType.NChar,50,adminGroupInfo.Title),
                                        GenerateInParam("@actionlist", SqlDbType.Text, 0, adminGroupInfo.ActionList),
                                        GenerateInParam("@admingid", SqlDbType.SmallInt, 2, adminGroupInfo.AdminGid)
                                    };
            string commandText = string.Format(@"UPDATE [{0}admingroups] SET [title]=@title,[actionlist]=@actionlist
                                                 WHERE [admingid]=@admingid",
                                                RDBSHelper.RDBSTablePre);
            RDBSHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
        }


        #endregion

        #region  操作管理

        /// <summary>
        /// 获得后台操作列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetAdminActionList()
        {
            string commandText = string.Format("SELECT {0} FROM [{1}adminactions] ORDER BY [displayorder] ",
                                                RDBSFields.ADMIN_ACTIONS,
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteReader(CommandType.Text, commandText);
        }
        /// <summary>
        /// 获得后台操作列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetAdminActions()
        {
            string commandText = string.Format("SELECT * FROM [owzx_adminactions] ORDER BY [displayorder]");
            return RDBSHelper.ExecuteTable(commandText, null)[0];
        }
        #endregion

        #region 收藏夹

        /// <summary>
        /// 将商品添加到收藏夹
        /// </summary>
        /// <returns></returns>
        public bool AddToFavorite(int uid, int pid, int state, DateTime addTime)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@uid", SqlDbType.Int, 4, uid),    
                                        GenerateInParam("@pid", SqlDbType.Int, 4, pid),    
                                        GenerateInParam("@state", SqlDbType.TinyInt, 1, state),
                                        GenerateInParam("@addtime", SqlDbType.DateTime, 8, addTime)  
                                    };
            return RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                              string.Format("{0}addtofavorite", RDBSHelper.RDBSTablePre),
                                              parms) > 0;
        }

        /// <summary>
        /// 删除收藏夹的商品
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="pid">商品id</param>
        /// <returns></returns>
        public bool DeleteFavoriteProductByUidAndPid(int uid, int pid)
        {
            DbParameter[] parms = {
                                     GenerateInParam("@uid", SqlDbType.Int, 4, uid),
                                     GenerateInParam("@pid", SqlDbType.Int, 4, pid)
                                    };
            return RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                              string.Format("{0}deletefavoriteproductbyuidandpid", RDBSHelper.RDBSTablePre),
                                              parms) > 0;
        }

        /// <summary>
        /// 商品是否已经收藏
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="pid">商品id</param>
        public bool IsExistFavoriteProduct(int uid, int pid)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@uid", SqlDbType.Int, 4, uid),    
                                        GenerateInParam("@pid", SqlDbType.Int, 4, pid)
                                    };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}isexistfavoriteproduct", RDBSHelper.RDBSTablePre),
                                                                   parms)) > 0;
        }

        /// <summary>
        /// 获得收藏夹商品列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="uid">用户id</param>
        /// <param name="productName">商品名称</param>
        /// <returns></returns>
        public DataTable GetFavoriteProductList(int pageSize, int pageNumber, int uid, string productName)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),    
                                        GenerateInParam("@pagenumber", SqlDbType.Int, 4, pageNumber),
                                        GenerateInParam("@uid", SqlDbType.Int, 4, uid),
                                        GenerateInParam("@productname", SqlDbType.NVarChar, 200, productName)
                                    };
            return RDBSHelper.ExecuteDataset(CommandType.StoredProcedure,
                                             string.Format("{0}getfilterfavoriteproductlist", RDBSHelper.RDBSTablePre),
                                             parms).Tables[0];
        }

        /// <summary>
        /// 获得收藏夹商品列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public DataTable GetFavoriteProductList(int pageSize, int pageNumber, int uid)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),    
                                        GenerateInParam("@pagenumber", SqlDbType.Int, 4, pageNumber),
                                        GenerateInParam("@uid", SqlDbType.Int, 4, uid)
                                    };
            return RDBSHelper.ExecuteDataset(CommandType.StoredProcedure,
                                             string.Format("{0}getfavoriteproductlist", RDBSHelper.RDBSTablePre),
                                             parms).Tables[0];
        }

        /// <summary>
        /// 获得收藏夹商品数量
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="productName">商品名称</param>
        /// <returns></returns>
        public int GetFavoriteProductCount(int uid, string productName)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@uid", SqlDbType.Int, 4, uid),
                                        GenerateInParam("@productname", SqlDbType.NVarChar, 200, productName)
                                    };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}getfilterfavoriteproductcount", RDBSHelper.RDBSTablePre),
                                                                   parms));
        }

        /// <summary>
        /// 获得收藏夹商品数量
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public int GetFavoriteProductCount(int uid)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@uid", SqlDbType.Int, 4, uid)
                                    };
            return TypeHelper.ObjectToInt(RDBSHelper.ExecuteScalar(CommandType.StoredProcedure,
                                                                   string.Format("{0}getfavoriteproductcount", RDBSHelper.RDBSTablePre),
                                                                   parms));
        }

        /// <summary>
        /// 设置收藏夹商品状态
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="pid">商品id</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public bool SetFavoriteProductState(int uid, int pid, int state)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@uid", SqlDbType.Int, 4, uid),    
                                        GenerateInParam("@pid", SqlDbType.Int, 4, pid),    
                                        GenerateInParam("@state", SqlDbType.TinyInt, 1, state)
                                    };
            return RDBSHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                              string.Format("{0}setfavoriteproductstate", RDBSHelper.RDBSTablePre),
                                              parms) > 0;
        }

        #endregion

        #region app
        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="chag"></param>
        /// <returns></returns>
        public string AddLimit(MD_AppLimit mode)
        {
            try
            {
                DbParameter[] parms = {
                                        GenerateInParam("@ip", SqlDbType.VarChar,50, mode.Ip),
                                        GenerateInParam("@domin", SqlDbType.VarChar,50, mode.Domin),
                                        GenerateInParam("@port", SqlDbType.VarChar,50, mode.Port),
                                        GenerateInParam("@limittime", SqlDbType.DateTime,25, mode.Limittime),
                                        GenerateInParam("@remark", SqlDbType.VarChar,50, mode.Remark)
                                       
                                    };
                string commandText = string.Format(@"
begin try
begin tran t1
if not exists(select 1 from owzx_applimit where ip=@ip and domin=@domin and port=@port)
begin
INSERT INTO [owzx_applimit]([ip] ,[domin],[port],[limittime],[remark],[addtime])
VALUES (@ip,@domin,@port,@limittime,@remark,convert(varchar(25),getdate(),120))

select '添加成功' state
commit tran t1
end
else
begin
select '已存在' state
commit tran t1
end

end try
begin catch
rollback tran t1
select ERROR_MESSAGE() state
end catch

");
                return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
            }
            catch (Exception er)
            {

                throw;
            }
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string UpdateLimit(MD_AppLimit mode)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@ip", SqlDbType.VarChar,50, mode.Ip),
                                        GenerateInParam("@domin", SqlDbType.VarChar,50, mode.Domin),
                                        GenerateInParam("@port", SqlDbType.VarChar,50, mode.Port),
                                        GenerateInParam("@limittime", SqlDbType.DateTime,25, mode.Limittime),
                                        GenerateInParam("@remark", SqlDbType.VarChar,50, mode.Remark)
                                    };
            string commandText = string.Format(@"
begin try
begin tran t1

if exists(select 1 from owzx_applimit where ip=@ip and domin=@domin and port=@port)
begin
UPDATE a
   SET --[ip] = @ip
      --,[domin] = @domin
      --,[port] = @port
      --,
       [limittime] = @limittime
      ,[remark] = @remark
      ,[updatetime] =convert(varchar(25),getdate(),120)
from [owzx_applimit] a where ip=@ip and domin=@domin and port=@port
       
select '修改成功' state
commit tran t1

end
else
begin
select '记录已被删除' state
commit tran t1
end

end try
begin catch
rollback tran t1
select ERROR_MESSAGE() state
end catch

");
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }

        

        /// <summary>
        ///获取信息
        /// </summary>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetLimitList(string condition = "")
        {
            string commandText = string.Format(@"

begin try
if OBJECT_ID('tempdb..#list') is not null
drop table #list

SELECT ROW_NUMBER() over(order by limitid ) id,
      [limitid]
      ,[ip]
      ,[domin]
      ,[port]
      ,[limittime]
      ,[remark]
      , addtime
      ,updatetime 
  into #list
  FROM [owzx_applimit]
 {0}

select * from #list
 
end try
begin catch
select ERROR_MESSAGE() state
end catch

", condition);

            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, null)[0];
        }
        #endregion

    }
}
