using OWZX.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Core
{
    // 新增用户功能
    public partial interface IRDBSStrategy
    {
        #region 用户消息记录
        /// <summary>
        /// 添加用户消息记录
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        string AddMessage(MD_Message msg);
        
        /// <summary>
        /// 更新用户消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        string UpdateMessage(MD_Message msg);

        /// <summary>
        /// 删除用户消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string DeleteMessage(string id);

        /// <summary>
        ///获取用户消息记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        DataTable GetMessageList(int pageNumber, int pageSize, string condition = "");

        #endregion


        #region 用户账变记录
        /// <summary>
        /// 添加用户账变记录
        /// </summary>
        /// <param name="chag"></param>
        /// <returns></returns>
        string AddAChange(MD_Change chag);

        /// <summary>
        /// 更新账变信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        string UpdateAChange(MD_Change chag);

        /// <summary>
        /// 删除账变信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string DeleteAChange(string id);

        /// <summary>
        ///获取用户账变信息(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        DataTable GetAChangeList(int pageNumber, int pageSize, string condition = "");

        #endregion

        #region 用户回水
        /// <summary>
        ///  获取用户回水(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        DataTable GetBackList(int pageNumber, int pageSize, string condition = "");
        DataTable GetBackReportList(int pageNumber, int pageSize, string condition = "");
        /// <summary>
        /// 更新用户回水
        /// </summary>
        /// <param name="back"></param>
        /// <returns></returns>
        string UpdateUserBack(MD_UserBack back);

        string UpdateUserBackReport(MD_UserBack back);
        string UpdUserBackReport(int uid, int type = 0);
        /// <summary>
        /// 删除用户消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string DeleteUserBack(string id);

        string DeleteUserBackReport(string id);
        #endregion
        #region 用户日报表

        DataTable GetUserRptList(int pageNumber, int pageSize, string condition = "");
 
        #endregion
        #region 用户转账记录
        /// <summary>
        /// 添加用户转账记录
        /// </summary>
        /// <param name="rmt"></param>
        /// <returns></returns>
        string AddUserRemit(MD_Remit rmt);

        /// <summary>
        /// 更新用户转账记录
        /// </summary>
        /// <param name="rmt"></param>
        /// <returns></returns>
        string UpdateUserRemit(MD_Remit rmt);

        /// <summary>
        /// 删除用户转账记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         string DeleteUserRemit(string id);

        /// <summary>
        ///获取用户转账记录记录(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
         DataTable GetUserRemitList(int pageIndex, int pageSize, string condition = "");

        #endregion

         #region 验证码记录
         /// <summary>
         /// 添加验证码记录
         /// </summary>
         /// <param name="msg"></param>
         /// <returns></returns>
         string AddSMSCode(MD_SMSCode code);

         /// <summary>
         /// 更新验证码
         /// </summary>
         /// <param name="msg"></param>
         /// <returns></returns>
         string UpdateSMSCode(MD_SMSCode code);

         /// <summary>
         /// 删除验证码
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>
         string DeleteSMSCode(string id);

         /// <summary>
         /// 删除过期的验证码
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>
         string DeleteSMSCode();

         /// <summary>
         ///获取验证码记录(分页)
         /// </summary>
         /// <param name="pageNumber"></param>
         /// <param name="pageSize">-1 取全部</param>
         /// <param name="condition">没有where</param>
         /// <returns></returns>
         DataTable GetSMSCodeList(int pageNumber, int pageSize, string condition = "");

         #endregion

        /// <summary>
        /// 获取首页数据
        /// </summary>
        /// <returns></returns>
        DataTable HomeData();

        #region 用户投注记录
        // <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="pageNumber">页索引</param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="account">账号</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        DataTable GetUserBettList(int pageNumber, int pageSize, string account, string condition = "");
        #endregion

        #region 用户公告
       

        /// <summary>
        /// 获取系统公告
        /// </summary>
        /// <returns></returns>
        DataTable GetUserSysNew(string account);
        #endregion


        #region 用户投注模式
        /// <summary>
        /// 添加用户投注模式
        /// </summary>
        /// <param name="chag"></param>
        /// <returns></returns>
        string AddMode(MD_BettMode mode);

        /// <summary>
        /// 更新模式信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
         string UpdateMode(MD_BettMode mode);

        /// <summary>
        /// 删除模式信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         string DeleteMode(string name, int uid, int lotterytype);

        /// <summary>
        ///获取模式信息(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        DataTable GetModeList(int pageNumber, int pageSize, string condition = "");

        /// <summary>
        /// 是否设置投注模式
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        bool ExistsMode(int uid, int lotterytype);

         /// <summary>
        /// 添加投注模式
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bettid"></param>
        /// <returns></returns>
        string AddModeFromRecord(string name, int bettid);
        #endregion

    }
}
