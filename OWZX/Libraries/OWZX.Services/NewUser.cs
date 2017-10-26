using OWZX.Core;
using OWZX.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Services
{
    public partial class NewUser
    {
        #region 用户消息记录
        /// <summary>
        /// 添加用户消息记录
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool AddMessage(MD_Message msg)
        {
            string result = OWZX.Data.NewUser.AddMessage(msg);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新用户消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool UpdateMessage(MD_Message msg)
        {
            string result = OWZX.Data.NewUser.UpdateMessage(msg);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除用户消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteMessage(string id)
        {
            string result = OWZX.Data.NewUser.DeleteMessage(id);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///获取用户消息记录(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static List<MD_Message> GetMessageList(int pageIndex, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.NewUser.GetMessageList(pageIndex, pageSize, condition);
            List<MD_Message> list = (List<MD_Message>)ModelConvertHelper<MD_Message>.ConvertToModel(dt);
            return list;
        }

        #endregion

        #region 用户回水
        /// <summary>
        /// 更新回水规则
        /// </summary>
        /// <param name="back"></param>
        /// <returns></returns>
        public static bool UpdateUserBack(MD_UserBack back)
        {
            string result = OWZX.Data.NewUser.UpdateUserBack(back);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新回水规则
        /// </summary>
        /// <param name="back"></param>
        /// <returns></returns>
        public static bool UpdateUserBackReport(MD_UserBack back)
        {
            string result = OWZX.Data.NewUser.UpdateUserBackReport(back);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }
        public static string UpdUserBackReport(int uid,int type=0)
        {
            return OWZX.Data.NewUser.UpdUserBackReport(uid,type);
        }
        /// <summary>
        /// 删除用户消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteUserBack(string id)
        {
            string result = OWZX.Data.NewUser.DeleteUserBack(id);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 删除用户回水
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteUserBackReport(string id)
        {
            string result = OWZX.Data.NewUser.DeleteUserBackReport(id);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        ///  获取用户回水(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static List<MD_UserBack> GetBackList(int pageIndex, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.NewUser.GetBackList(pageIndex, pageSize, condition);
            List<MD_UserBack> list = (List<MD_UserBack>)ModelConvertHelper<MD_UserBack>.ConvertToModel(dt);
            return list;
        }
        /// <summary>
        ///  获取用户回水(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static List<MD_UserBack> GetBackReportList(int pageIndex, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.NewUser.GetBackReportList(pageIndex, pageSize, condition);
            List<MD_UserBack> list = (List<MD_UserBack>)ModelConvertHelper<MD_UserBack>.ConvertToModel(dt);
            return list;
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
            return OWZX.Data.NewUser.AddAChange(chag);
        }

        /// <summary>
        /// 更新账变信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string UpdateAChange(MD_Change chag)
        {
            return OWZX.Data.NewUser.UpdateAChange(chag);
        }

        /// <summary>
        /// 删除账变信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteAChange(string id)
        {
            return OWZX.Data.NewUser.DeleteAChange(id);
        }

        /// <summary>
        ///获取用户账变信息(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static List<MD_Change> GetAChangeList(int pageIndex, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.NewUser.GetAChangeList(pageIndex, pageSize, condition);
            List<MD_Change> list = (List<MD_Change>)ModelConvertHelper<MD_Change>.ConvertToModel(dt);
            return list;
        }

        #endregion

        #region 用户转账记录
        /// <summary>
        /// 添加用户转账记录
        /// </summary>
        /// <param name="rmt"></param>
        /// <returns></returns>
        public static bool AddUserRemit(MD_Remit rmt)
        {
            string result = OWZX.Data.NewUser.AddUserRemit(rmt);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新用户转账记录
        /// </summary>
        /// <param name="rmt"></param>
        /// <returns></returns>
        public static bool UpdateUserRemit(MD_Remit rmt)
        {
            string result = OWZX.Data.NewUser.UpdateUserRemit(rmt);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除用户转账记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteUserRemit(string id)
        {
            string result = OWZX.Data.NewUser.DeleteUserRemit(id);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///获取用户转账记录记录(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static List<MD_Remit> GetUserRemitList(int pageIndex, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.NewUser.GetUserRemitList(pageIndex, pageSize, condition);
            List<MD_Remit> list = (List<MD_Remit>)ModelConvertHelper<MD_Remit>.ConvertToModel(dt);
            return list;
        }

        #endregion

        #region 用户日报表
        public static List<MD_UserRptDay> GetUserRptList(int pageIndex, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.NewUser.GetUserRptList(pageIndex, pageSize, condition);
            List<MD_UserRptDay> list = (List<MD_UserRptDay>)ModelConvertHelper<MD_UserRptDay>.ConvertToModel(dt);
            return list;
        }
        #endregion
        /// <summary>
        /// 获取首页数据
        /// </summary>
        /// <returns></returns>
        public static DataTable HomeData()
        {
            return OWZX.Data.NewUser.HomeData();
        }

        #region 验证码记录
        /// <summary>
        /// 添加验证码记录
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool AddSMSCode(MD_SMSCode code)
        {
            string result = OWZX.Data.NewUser.AddSMSCode(code);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新验证码
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool UpdateSMSCode(MD_SMSCode code)
        {
            string result = OWZX.Data.NewUser.UpdateSMSCode(code);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteSMSCode(string id)
        {
            string result = OWZX.Data.NewUser.DeleteSMSCode(id);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除过期的验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteSMSCode()
        {
            string result = OWZX.Data.NewUser.DeleteSMSCode();
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///获取验证码记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static List<MD_SMSCode> GetSMSCodeList(int pageNumber, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.NewUser.GetSMSCodeList(pageNumber, pageSize, condition);
            List<MD_SMSCode> list = (List<MD_SMSCode>)ModelConvertHelper<MD_SMSCode>.ConvertToModel(dt);
            return list;
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
            return OWZX.Data.NewUser.GetUserBettList(pageNumber, pageSize, account, condition);
        }

        #endregion

        #region 用户公告


        /// <summary>
        /// 获取系统公告
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUserSysNew(string account)
        {
            return OWZX.Data.NewUser.GetUserSysNew(account);
        }
        #endregion

        #region 用户投注模式
        /// <summary>
        /// 添加用户投注模式
        /// </summary>
        /// <param name="chag"></param>
        /// <returns></returns>
        public static bool AddMode(MD_BettMode mode)
        {
            string result = OWZX.Data.NewUser.AddMode(mode);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新模式信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool UpdateMode(MD_BettMode mode)
        {
            string result = OWZX.Data.NewUser.UpdateMode(mode);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除模式信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteMode(string name, int uid, int lotterytype)
        {
            string result = OWZX.Data.NewUser.DeleteMode(name,uid,lotterytype);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///获取模式信息(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static List<MD_BettMode> GetModeList(int pageNumber, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.NewUser.GetModeList(pageNumber, pageSize, condition);
            return (List<MD_BettMode>) ModelConvertHelper<MD_BettMode>.ConvertToModel(dt);
        }

        /// <summary>
        /// 是否设置投注模式
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static bool ExistsMode(int uid,int lotterytype)
        {
            return OWZX.Data.NewUser.ExistsMode(uid,lotterytype);
        }
        /// <summary>
        /// 添加投注模式
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bettid"></param>
        /// <returns></returns>
        public static string AddModeFromRecord(string name, int bettid)
        {
            return OWZX.Data.NewUser.AddModeFromRecord(name, bettid);
        }

        #endregion
    }
}
