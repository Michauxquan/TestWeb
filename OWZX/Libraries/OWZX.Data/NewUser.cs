using OWZX.Core;
using OWZX.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Data
{
    public partial class NewUser
    {
        #region 用户消息记录
        /// <summary>
        /// 添加用户消息记录
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string AddMessage(MD_Message msg)
        {
            return OWZX.Core.BSPData.RDBS.AddMessage(msg);
        }

        /// <summary>
        /// 更新用户消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string UpdateMessage(MD_Message msg)
        {
            return OWZX.Core.BSPData.RDBS.UpdateMessage(msg);
        }

        /// <summary>
        /// 删除用户消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteMessage(string id)
        {
            return OWZX.Core.BSPData.RDBS.DeleteMessage(id);
        }

        /// <summary>
        ///获取用户消息记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static DataTable GetMessageList(int pageNumber, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetMessageList(pageNumber, pageSize, condition);
        }

        #endregion

        #region 用户回水
        /// <summary>
        /// 更新回水规则
        /// </summary>
        /// <param name="back"></param>
        /// <returns></returns>
        public static string UpdateUserBack(MD_UserBack back)
        {
            return OWZX.Core.BSPData.RDBS.UpdateUserBack(back);

        }
        public static string UpdateUserBackReport(MD_UserBack back)
        {
            return OWZX.Core.BSPData.RDBS.UpdateUserBackReport(back);
        }

        public static string UpdUserBackReport(int uid, int type=0)
        {
            return OWZX.Core.BSPData.RDBS.UpdUserBackReport(uid, type);
        }

        /// <summary>
        /// 删除用户消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteUserBack(string id)
        {
            return OWZX.Core.BSPData.RDBS.DeleteUserBack(id);
        }
        /// <summary>
        /// 删除用户消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteUserBackReport(string id)
        {
            return OWZX.Core.BSPData.RDBS.DeleteUserBackReport(id);
        }

        /// <summary>
        ///  获取用户回水(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static DataTable GetBackList(int pageNumber, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetBackList(pageNumber, pageSize, condition);
        }
        /// <summary>
        ///  获取用户回水(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static DataTable GetBackReportList(int pageNumber, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetBackReportList(pageNumber, pageSize, condition);
        }
        #endregion

        #region 用户账变记录
        /// <summary>
        /// 添加用户账变记录
        /// </summary>
        /// <param name="chag"></param>
        /// <returns></returns>
        public static string AddAChange(MD_Change chag)
        {
            return OWZX.Core.BSPData.RDBS.AddAChange(chag);
        }

        /// <summary>
        /// 更新账变信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string UpdateAChange(MD_Change chag)
        {
            return OWZX.Core.BSPData.RDBS.UpdateAChange(chag);
        }

        /// <summary>
        /// 删除账变信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteAChange(string id)
        {
            return OWZX.Core.BSPData.RDBS.DeleteAChange(id);
        }

        /// <summary>
        ///获取用户账变信息(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static DataTable GetAChangeList(int pageNumber, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetAChangeList(pageNumber, pageSize, condition);
        }

        #endregion

        #region 用户转账记录
        /// <summary>
        /// 添加用户转账记录
        /// </summary>
        /// <param name="rmt"></param>
        /// <returns></returns>
        public static string AddUserRemit(MD_Remit rmt)
        {
            return OWZX.Core.BSPData.RDBS.AddUserRemit(rmt);
        }

        /// <summary>
        /// 更新用户转账记录
        /// </summary>
        /// <param name="rmt"></param>
        /// <returns></returns>
        public static string UpdateUserRemit(MD_Remit rmt)
        {
            return OWZX.Core.BSPData.RDBS.UpdateUserRemit(rmt);
        }

        /// <summary>
        /// 删除用户转账记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteUserRemit(string id)
        {
            return OWZX.Core.BSPData.RDBS.DeleteUserRemit(id);
        }

        /// <summary>
        ///获取用户转账记录记录(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static DataTable GetUserRemitList(int pageIndex, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetUserRemitList(pageIndex, pageSize, condition);
        }

        #endregion

        /// <summary>
        /// 获取首页数据
        /// </summary>
        /// <returns></returns>
        public static DataTable HomeData()
        {
            return OWZX.Core.BSPData.RDBS.HomeData();
        }

        #region 验证码记录
        /// <summary>
        /// 添加验证码记录
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string AddSMSCode(MD_SMSCode code)
        {
            return OWZX.Core.BSPData.RDBS.AddSMSCode(code);
        }

        /// <summary>
        /// 更新验证码
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string UpdateSMSCode(MD_SMSCode code)
        {
            return OWZX.Core.BSPData.RDBS.UpdateSMSCode(code);
        }

        /// <summary>
        /// 删除验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteSMSCode(string id)
        {
            return OWZX.Core.BSPData.RDBS.DeleteSMSCode(id);
        }

        /// <summary>
        /// 删除过期的验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteSMSCode()
        {
            return OWZX.Core.BSPData.RDBS.DeleteSMSCode();
        }

        /// <summary>
        ///获取验证码记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static DataTable GetSMSCodeList(int pageNumber, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetSMSCodeList(pageNumber, pageSize, condition);
        }

        #endregion

        #region 用户投注记录
        /// <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="pageNumber">页索引</param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="account">账号</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static DataTable GetUserBettList(int pageNumber, int pageSize, string account, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetUserBettList(pageNumber, pageSize, account, condition);
        }

        #endregion

        #region 用户公告

        /// <summary>
        /// 获取系统公告
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUserSysNew(string account)
        {
            return OWZX.Core.BSPData.RDBS.GetUserSysNew(account);
        }
        #endregion


        #region 用户投注模式
        /// <summary>
        /// 添加用户投注模式
        /// </summary>
        /// <param name="chag"></param>
        /// <returns></returns>
        public static string AddMode(MD_BettMode mode)
        {
            return OWZX.Core.BSPData.RDBS.AddMode( mode);
        }

        /// <summary>
        /// 更新模式信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string UpdateMode(MD_BettMode mode) {
            return OWZX.Core.BSPData.RDBS.UpdateMode( mode);
        }

        /// <summary>
        /// 删除模式信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteMode(string name, int uid, int lotterytype)
        {
            return OWZX.Core.BSPData.RDBS.DeleteMode(name,uid,lotterytype);
        }

        /// <summary>
        ///获取模式信息(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static DataTable GetModeList(int pageNumber, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetModeList( pageNumber,  pageSize,  condition);
        }

        /// <summary>
        /// 是否设置投注模式
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static bool ExistsMode(int uid, int lotterytype)
        {
            return OWZX.Core.BSPData.RDBS.ExistsMode(uid,lotterytype);
        }

        #endregion

    }
}
