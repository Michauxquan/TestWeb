using System;
using System.Data;

using OWZX.Core;

namespace OWZX.Services
{
    /// <summary>
    /// 后台用户操作管理类
    /// </summary>
    public partial class AdminUsers : Users
    {
        /// <summary>
        /// 后台获得用户列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable AdminGetUserList(int pageSize, int pageNumber, string condition)
        {
            return OWZX.Data.Users.AdminGetUserList(pageSize, pageNumber, condition);
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
            return OWZX.Data.Users.AdminGetUserListCondition(userName, email, mobile, userRid, adminGid);
        }

        /// <summary>
        /// 后台获得用户列表数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int AdminGetUserCount(string condition)
        {
            return OWZX.Data.Users.AdminGetUserCount(condition);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pageSize">-1 获取全部数据</param>
        /// <param name="pageNumber"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable GetUserList(int pageSize, int pageNumber,ref long SumFee, string condition = "")
        {
            return OWZX.Data.Users.GetUserList(pageSize, pageNumber, ref  SumFee, condition);
        }
        public static DataTable GetUserParentList(int pageSize, int pageNumber, string condition = "")
        {
            return OWZX.Data.Users.GetUserParentList(pageSize, pageNumber, condition);
        }
        /// <summary>
        /// 获取团队列表
        /// </summary>
        /// <param name="pageSize">-1 获取全部数据</param>
        /// <param name="pageNumber"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable GetTeam(int pageSize, int pageNumber, string condition = "")
        {
            return OWZX.Data.Users.GetTeam(pageSize, pageNumber, condition);
        }

    }
}
