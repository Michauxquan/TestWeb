using System;
using System.Data;
using System.Collections.Generic;

using OWZX.Core;
using OWZX.Model;

namespace OWZX.Data
{
    /// <summary>
    /// 用户数据访问类
    /// </summary>
    public partial class Users
    {
        #region 收益
        public static DataTable GetUserProfitBaseInfo(string account)
        {
            return new DataTable();
        }
        #endregion


        #region 辅助方法

        /// <summary>
        /// 从IDataReader创建PartUserInfo
        /// </summary>
        public static PartUserInfo BuildPartUserFromReader(IDataReader reader)
        {
            PartUserInfo partUserInfo = new PartUserInfo();

            partUserInfo.Uid = TypeHelper.ObjectToInt(reader["uid"]);
            partUserInfo.UserName = reader["username"].ToString();
            partUserInfo.Email = reader["email"].ToString();
            partUserInfo.Mobile = reader["mobile"].ToString();
            partUserInfo.Password = reader["password"].ToString();
            partUserInfo.UserRid = TypeHelper.ObjectToInt(reader["userrid"]);
            partUserInfo.AdminGid = TypeHelper.ObjectToInt(reader["admingid"]);
            partUserInfo.NickName = reader["nickname"].ToString();
            partUserInfo.Avatar = reader["avatar"].ToString();
            partUserInfo.PayCredits = TypeHelper.ObjectToInt(reader["paycredits"]);
            partUserInfo.RankCredits = TypeHelper.ObjectToInt(reader["rankcredits"]);
            partUserInfo.VerifyEmail = TypeHelper.ObjectToInt(reader["verifyemail"]);
            partUserInfo.VerifyMobile = TypeHelper.ObjectToInt(reader["verifymobile"]);
            partUserInfo.VerifyLoginRg = TypeHelper.ObjectToInt(reader["verifyloginrg"]);
            partUserInfo.LiftBanTime = TypeHelper.ObjectToDateTime(reader["liftbantime"]);
            partUserInfo.Salt = reader["salt"].ToString();
            if (reader["bankmoney"].ToString() != "")
                partUserInfo.BankMoney = Decimal.Parse(reader["bankmoney"].ToString());
            if (reader["totalmoney"].ToString() != "")
                partUserInfo.TotalMoney = Decimal.Parse(reader["totalmoney"].ToString());
            return partUserInfo;
        }

        /// <summary>
        /// 从IDataReader创建UserInfo
        /// </summary>
        public static UserInfo BuildUserFromReader(IDataReader reader)
        {
            UserInfo userInfo = new UserInfo();

            userInfo.Uid = TypeHelper.ObjectToInt(reader["uid"]);
            userInfo.UserName = reader["username"].ToString();
            userInfo.Email = reader["email"].ToString();
            userInfo.Mobile = reader["mobile"].ToString();
            userInfo.Password = reader["password"].ToString();
            userInfo.UserRid = TypeHelper.ObjectToInt(reader["userrid"]);
            userInfo.AdminGid = TypeHelper.ObjectToInt(reader["admingid"]);
            userInfo.NickName = reader["nickname"].ToString();
            userInfo.Avatar = reader["avatar"].ToString();
            userInfo.PayCredits = TypeHelper.ObjectToInt(reader["paycredits"]);
            userInfo.RankCredits = TypeHelper.ObjectToInt(reader["rankcredits"]);
            userInfo.VerifyEmail = TypeHelper.ObjectToInt(reader["verifyemail"]);
            userInfo.VerifyMobile = TypeHelper.ObjectToInt(reader["verifymobile"]);
            userInfo.VerifyLoginRg = TypeHelper.ObjectToInt(reader["verifyloginrg"]);
            userInfo.LiftBanTime = TypeHelper.ObjectToDateTime(reader["liftbantime"]);
            userInfo.Salt = reader["salt"].ToString();
            userInfo.BankMoney = Decimal.Parse(reader["bankmoney"].ToString());
            userInfo.TotalMoney = Decimal.Parse(reader["totalmoney"].ToString());

            userInfo.LastVisitTime = TypeHelper.ObjectToDateTime(reader["lastvisittime"]);
            userInfo.LastVisitIP = reader["lastvisitip"].ToString();
            userInfo.LastVisitRgId = TypeHelper.ObjectToInt(reader["lastvisitrgid"]);
            userInfo.RegisterTime = TypeHelper.ObjectToDateTime(reader["registertime"]);
            userInfo.RegisterIP = reader["registerip"].ToString();
            userInfo.RegisterRgId = TypeHelper.ObjectToInt(reader["registerrgid"]);
            userInfo.Gender = TypeHelper.ObjectToInt(reader["gender"]);
            userInfo.RealName = reader["realname"].ToString();
            userInfo.Bday = TypeHelper.ObjectToDateTime(reader["bday"]);
            userInfo.IdCard = reader["idcard"].ToString();
            userInfo.RegionId = TypeHelper.ObjectToInt(reader["regionid"]);
            userInfo.RegionIdTwo = TypeHelper.ObjectToInt(reader["regionidtwo"]);
            userInfo.Address = reader["address"].ToString();
            userInfo.Bio = reader["bio"].ToString();


            return userInfo;
        }

        /// <summary>
        /// 从IDataReader创建UserDetailInfo
        /// </summary>
        public static UserDetailInfo BuildUserDetailFromReader(IDataReader reader)
        {
            UserDetailInfo userDetailInfo = new UserDetailInfo();

            userDetailInfo.Uid = TypeHelper.ObjectToInt(reader["uid"]);
            userDetailInfo.LastVisitTime = TypeHelper.ObjectToDateTime(reader["lastvisittime"]);
            userDetailInfo.LastVisitIP = reader["lastvisitip"].ToString();
            userDetailInfo.LastVisitRgId = TypeHelper.ObjectToInt(reader["lastvisitrgid"]);
            userDetailInfo.RegisterTime = TypeHelper.ObjectToDateTime(reader["registertime"]);
            userDetailInfo.RegisterIP = reader["registerip"].ToString();
            userDetailInfo.RegisterRgId = TypeHelper.ObjectToInt(reader["registerrgid"]);
            userDetailInfo.Gender = TypeHelper.ObjectToInt(reader["gender"]);
            userDetailInfo.RealName = reader["realname"].ToString();
            userDetailInfo.Bday = TypeHelper.ObjectToDateTime(reader["bday"]);
            userDetailInfo.IdCard = reader["idcard"].ToString();
            userDetailInfo.RegionId = TypeHelper.ObjectToInt(reader["regionid"]);
            userDetailInfo.Address = reader["address"].ToString();
            userDetailInfo.Bio = reader["bio"].ToString();
            userDetailInfo.SignName = reader["signname"].ToString();
            userDetailInfo.RegionIdTwo= TypeHelper.ObjectToInt(reader["regionidtwo"]);
            return userDetailInfo;
        }

        #endregion

        ///// <summary>
        ///// 获得用户
        ///// </summary>
        ///// <param name="uid">用户id</param>
        ///// <returns></returns>
        //private static UserInfo GetUserById(int uid)
        //{
        //    UserInfo userInfo = null;

        //    IDataReader reader = OWZX.Core.BSPData.RDBS.GetUserById(uid);
        //    if (reader.Read())
        //    {
        //        userInfo = BuildUserFromReader(reader);
        //    }
        //    reader.Close();
        //    return userInfo;
        //}
        /// <summary>
        /// 获得用户
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static UserInfo GetUserById(int uid)
        {
            UserInfo userInfo = null;

            DataTable dt = OWZX.Core.BSPData.RDBS.GetUserById(uid);
            userInfo = OWZX.Core.ModelConvertHelper<UserInfo>.DataTableToModel(dt);

            return userInfo;
        }

        /// <summary>
        /// 添加邀请码信息（推广页面）
        /// </summary>
        /// <param name="paccount"></param>
        /// <param name="account"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool AddInviteInfo(string paccount, string account, string code)
        {
            return OWZX.Core.BSPData.RDBS.AddInviteInfo(paccount, account, code);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static bool DelInviteInfo(string account)
        {
            return OWZX.Core.BSPData.RDBS.DelInviteInfo(account);
        }
        /// <summary>
        /// 验证码是否正确
        /// </summary>
        /// <param name="account"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool ValidateCode(string account, string code)
        {
            return OWZX.Core.BSPData.RDBS.ValidateCode(account, code);
        }
        /// <summary>
        /// 验证用户是否通过推广注册
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static DataTable ValidateUser(string account)
        {
            return OWZX.Core.BSPData.RDBS.ValidateUser(account);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static bool DelUserByUid(int uid)
        {
            return OWZX.Core.BSPData.RDBS.DelUserByUid(uid);
        }

        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static PartUserInfo GetPartUserById(int uid)
        {
            PartUserInfo partUserInfo = null;


            IDataReader reader = OWZX.Core.BSPData.RDBS.GetPartUserById(uid);
            if (reader.Read())
            {
                partUserInfo = BuildPartUserFromReader(reader);
            }
            reader.Close();

            return partUserInfo;
        }
        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="invitecode">邀请码</param>
        /// <returns></returns>
        public static PartUserInfo GetPartUserByInviteCode(int invitecode)
        {
            PartUserInfo partUserInfo = null;


            IDataReader reader = OWZX.Core.BSPData.RDBS.GetPartUserByInviteCode(invitecode);
            if (reader.Read())
            {
                partUserInfo = BuildPartUserFromReader(reader);
            }
            reader.Close();

            return partUserInfo;
        }
        /// <summary>
        /// 获得用户细节
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static UserDetailInfo GetUserDetailById(int uid)
        {
            UserDetailInfo userDetailInfo = null;


            IDataReader reader = OWZX.Core.BSPData.RDBS.GetUserDetailById(uid);
            if (reader.Read())
            {
                userDetailInfo = BuildUserDetailFromReader(reader);
            }
            reader.Close();

            return userDetailInfo;
        }

        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static PartUserInfo GetPartUserByName(string userName)
        {
            PartUserInfo partUserInfo = null;

            IDataReader reader = OWZX.Core.BSPData.RDBS.GetPartUserByName(userName);
            if (reader.Read())
            {
                partUserInfo = BuildPartUserFromReader(reader);
            }
            reader.Close();
            return partUserInfo;
        }

        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="email">用户邮箱</param>
        /// <returns></returns>
        public static PartUserInfo GetPartUserByEmail(string email)
        {
            PartUserInfo partUserInfo = null;

            IDataReader reader = OWZX.Core.BSPData.RDBS.GetPartUserByEmail(email);
            if (reader.Read())
            {
                partUserInfo = BuildPartUserFromReader(reader);
            }
            reader.Close();
            return partUserInfo;
        }

        /// <summary>
        /// 获得部分用户
        /// </summary>
        /// <param name="mobile">用户手机</param>
        /// <returns></returns>
        public static PartUserInfo GetPartUserByMobile(string mobile)
        {
            PartUserInfo partUserInfo = null;

            DataTable reader = OWZX.Core.BSPData.RDBS.GetPartUserByMobile(mobile);
            partUserInfo = ModelConvertHelper<PartUserInfo>.DataTableToModel(reader);
            return partUserInfo;
        }

        /// <summary>
        /// 获得用户id
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static int GetUidByUserName(string userName)
        {
            return OWZX.Core.BSPData.RDBS.GetUidByUserName(userName);
        }

        /// <summary>
        /// 获得用户id
        /// </summary>
        /// <param name="email">用户邮箱</param>
        /// <returns></returns>
        public static int GetUidByEmail(string email)
        {
            return OWZX.Core.BSPData.RDBS.GetUidByEmail(email);
        }

        /// <summary>
        /// 获得用户id
        /// </summary>
        /// <param name="mobile">用户手机</param>
        /// <returns></returns>
        public static int GetUidByMobile(string mobile)
        {
            return OWZX.Core.BSPData.RDBS.GetUidByMobile(mobile);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        public static int CreateUser(UserInfo userInfo)
        {
            return OWZX.Core.BSPData.RDBS.CreateUser(userInfo);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <returns></returns>
        public static bool UpdateUser(UserInfo userInfo)
        {
            return OWZX.Core.BSPData.RDBS.UpdateUser(userInfo);

        }

        /// <summary>
        /// 更新部分用户
        /// </summary>
        /// <returns></returns>
        public static bool UpdatePartUser(PartUserInfo partUserInfo)
        {
            return OWZX.Core.BSPData.RDBS.UpdatePartUser(partUserInfo);

        }

        /// <summary>
        /// 更新用户细节
        /// </summary>
        /// <returns></returns>
        public static bool UpdateUserDetail(UserDetailInfo userDetailInfo)
        {
            return OWZX.Core.BSPData.RDBS.UpdateUserDetail(userDetailInfo);

        }

        public static bool UpdateUserVerifyLog(int uid = -1, int isveritylog = 0, int regionid = 0, int regionidtwo = 0)
        {
            return OWZX.Core.BSPData.RDBS.UpdateUserVerifyLog(uid, isveritylog, regionid, regionidtwo);
        }
        public static string GetRegionName(int uid = -1)
        {
            return OWZX.Core.BSPData.RDBS.GetRegionName(uid);
        }

        /// <summary>
        /// 更新用户最后访问
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="visitTime">访问时间</param>
        /// <param name="ip">ip</param>
        /// <param name="regionId">区域id</param>
        public static void UpdateUserLastVisit(int uid, DateTime visitTime, string ip, int regionId)
        {
            OWZX.Core.BSPData.RDBS.UpdateUserLastVisit(uid, visitTime, ip, regionId);

        }

        /// <summary>
        /// 后台获得用户列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable AdminGetUserList(int pageSize, int pageNumber, string condition)
        {
            return OWZX.Core.BSPData.RDBS.AdminGetUserList(pageSize, pageNumber, condition);
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
        public static string AdminGetUserListCondition(string userName, string email, string mobile, int userRid, int adminGid)
        {
            return OWZX.Core.BSPData.RDBS.AdminGetUserListCondition(userName, email, mobile, userRid, adminGid);
        }

        /// <summary>
        /// 后台获得用户列表数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int AdminGetUserCount(string condition)
        {
            return OWZX.Core.BSPData.RDBS.AdminGetUserCount(condition);
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable GetUserList(int pageSize, int pageNumber, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetUserList(pageSize, pageNumber, condition);
        }
        /// <summary>
        /// 获取团队列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable GetTeam(int pageSize, int pageNumber, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetTeam(pageSize, pageNumber, condition);
        }
        /// <summary>
        /// 获得用户等级下用户的数量
        /// </summary>
        /// <param name="userRid">用户等级id</param>
        /// <returns></returns>
        public static int GetUserCountByUserRid(int userRid)
        {
            return OWZX.Core.BSPData.RDBS.GetUserCountByUserRid(userRid);
        }

        /// <summary>
        /// 获得管理员组下用户的数量
        /// </summary>
        /// <param name="adminGid">管理员组id</param>
        /// <returns></returns>
        public static int GetUserCountByAdminGid(int adminGid)
        {
            return OWZX.Core.BSPData.RDBS.GetUserCountByAdminGid(adminGid);
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
        public static bool UpdateUser(int uid, string userName, string nickName, string avatar, int gender, string realName, DateTime bday, string idCard, int regionId, string address, string bio)
        {
            bool result = OWZX.Core.BSPData.RDBS.UpdateUser(uid, userName, nickName, avatar, gender, realName, bday, idCard, regionId, address, bio);

            return result;
        }

        /// <summary>
        /// 更新用户邮箱
        /// </summary>
        /// <param name="uid">用户id.</param>
        /// <param name="email">邮箱</param>
        public static void UpdateUserEmailByUid(int uid, string email)
        {
            OWZX.Core.BSPData.RDBS.UpdateUserEmailByUid(uid, email);

        }

        /// <summary>
        /// 更新用户手机
        /// </summary>
        /// <param name="uid">用户id.</param>
        /// <param name="mobile">手机</param>
        public static void UpdateUserMobileByUid(int uid, string mobile)
        {
            OWZX.Core.BSPData.RDBS.UpdateUserMobileByUid(uid, mobile);

        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="uid">用户id.</param>
        /// <param name="password">密码</param>
        public static void UpdateUserPasswordByUid(int uid, string password)
        {
            OWZX.Core.BSPData.RDBS.UpdateUserPasswordByUid(uid, password);

        }
        /// <summary>
        /// 更新用户安全密码
        /// </summary>
        /// <param name="uid">用户id.</param>
        /// <param name="password">密码</param>
        public static void UpdateUserSafePasswordByUid(int uid, string password)
        {
            OWZX.Core.BSPData.RDBS.UpdateUserSafePasswordByUid(uid, password);

        }
        public static string BankChange(int uid, decimal changefee, int type = 0)
        {
            return OWZX.Core.BSPData.RDBS.BankChange(uid, changefee, type);
        }
        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="mobile">用户手机号</param>
        /// <param name="password">密码</param>
        public static bool UpdateUserPasswordByMobile(string mobile, string password)
        {
            return OWZX.Core.BSPData.RDBS.UpdateUserPasswordByMobile(mobile, password);
        }

        /// <summary>
        /// 更新用户解禁时间
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="liftBanTime">解禁时间</param>
        public static void UpdateUserLiftBanTimeByUid(int uid, DateTime liftBanTime)
        {
            OWZX.Core.BSPData.RDBS.UpdateUserLiftBanTimeByUid(uid, liftBanTime);

        }

        /// <summary>
        /// 更新用户等级
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="userRid">用户等级id</param>
        public static void UpdateUserRankByUid(int uid, int userRid)
        {
            OWZX.Core.BSPData.RDBS.UpdateUserRankByUid(uid, userRid);

        }
        public static bool UpdateUserAccount(int uid, decimal userRid)
        {
          return  OWZX.Core.BSPData.RDBS.UpdateUserAccount(uid, userRid);
        }

        /// <summary>
        /// 更新用户在线时间
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="onlineTime">在线时间</param>
        /// <param name="updateTime">更新时间</param>
        public static void UpdateUserOnlineTime(int uid, int onlineTime, DateTime updateTime)
        {
            OWZX.Core.BSPData.RDBS.UpdateUserOnlineTime(uid, onlineTime, updateTime);
        }

        /// <summary>
        /// 通过注册ip获得注册时间
        /// </summary>
        /// <param name="registerIP">注册ip</param>
        /// <returns></returns>
        public static DateTime GetRegisterTimeByRegisterIP(string registerIP)
        {
            return OWZX.Core.BSPData.RDBS.GetRegisterTimeByRegisterIP(registerIP);
        }

        /// <summary>
        /// 获得用户最后访问时间
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static DateTime GetUserLastVisitTimeByUid(int uid)
        {
            return OWZX.Core.BSPData.RDBS.GetUserLastVisitTimeByUid(uid);
        }

    }
}
