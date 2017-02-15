using System;
using System.Collections.Generic;
using System.Data;
using OWZX.Core;
using OWZX.Model;

namespace OWZX.Services
{
    /// <summary>
    /// 登陆失败日志操作管理类
    /// </summary>
    public partial class LoginFailLogs
    {
        /// <summary>
        /// 获得登陆失败次数
        /// </summary>
        /// <param name="loginIP">登陆IP</param>
        /// <returns></returns>
        public static int GetLoginFailTimesByIp(string loginIP)
        {
            LoginFailLogInfo loginFailLogInfo = OWZX.Data.LoginFailLogs.GetLoginFailLogByIP(CommonHelper.ConvertIPToLong(loginIP));
            if (loginFailLogInfo == null)
                return 0;
            if (loginFailLogInfo.LastLoginTime.AddMinutes(15) < DateTime.Now)
                return 0;

            return loginFailLogInfo.FailTimes;
        }
        /// <summary>
        /// 增加登陆日志
        /// </summary>
        /// <param name="loginIP">登陆IP</param>
        /// <param name="loginTime">登陆时间</param>
        public static DataTable GetLoginList(int pageindex,int pageSize,string sqlwhere="")
        {
            return OWZX.Data.LoginFailLogs.GetLoginList(pageindex, pageSize, sqlwhere);
        }
        public static List<MD_UsersLog> GetUserLoginList(int pageIndex, int pageSize, string condition = "")
        {
            DataTable dt = GetLoginList(pageIndex, pageSize, condition);
            List<MD_UsersLog> list = (List<MD_UsersLog>)ModelConvertHelper<MD_UsersLog>.ConvertToModel(dt);
            return list;
        }
        /// <summary>
        /// 增加登陆日志
        /// </summary>
        /// <param name="loginIP">登陆IP</param>
        /// <param name="loginTime">登陆时间</param>
        public static void AddLogin(string loginIP, int uid, DateTime loginTime, string ipName, int type, string remark)
        {
            OWZX.Data.LoginFailLogs.AddLogin(loginIP, uid, loginTime, ipName, type, remark);
        }
        /// <summary>
        /// 增加登陆失败次数
        /// </summary>
        /// <param name="loginIP">登陆IP</param>
        /// <param name="loginTime">登陆时间</param>
        public static void AddLoginFailTimes(string loginIP, DateTime loginTime)
        {
            OWZX.Data.LoginFailLogs.AddLoginFailTimes(CommonHelper.ConvertIPToLong(loginIP), loginTime);
        }
        /// <summary>
        /// 删除登陆失败日志
        /// </summary>
        /// <param name="loginIP">登陆IP</param>
        public static void DeleteLoginFailLogByIP(string loginIP)
        {
            OWZX.Data.LoginFailLogs.DeleteLoginFailLogByIP(CommonHelper.ConvertIPToLong(loginIP));
        }
    }
}
