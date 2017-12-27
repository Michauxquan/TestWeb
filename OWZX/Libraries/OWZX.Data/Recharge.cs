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
    public partial class Recharge
    {

        /// <summary>
        /// 添加充值记录
        /// </summary>
        /// <param name="rech"></param>
        /// <returns></returns>
        public static string AddRecharge(RechargeModel rech)
        {
            return BSPData.RDBS.AddRecharge(rech);
        }
        /// <summary>
        /// 更新充值记录 
        /// </summary>
        /// <param name="rech"></param>
        /// <returns></returns>
        public static string UpdateRecharge(RechargeModel rech)
        {
            return BSPData.RDBS.UpdateRecharge(rech);
        }

        /// <summary>
        /// 充值成功更新充值记录
        /// </summary>
        /// <param name="rech"></param>
        /// <returns></returns>
        public static string UpdateRechargeForPay(RechargeModel rech)
        {
            return BSPData.RDBS.UpdateRechargeForPay(rech);
        }
        /// <summary>
        /// 删除充值记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteRecharge(string id)
        {
            return BSPData.RDBS.DeleteRecharge(id);
        }


        /// <summary>
        ///获取充值记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable GetRechargeList(int pageNumber, int pageSize, string condition = "")
        {
            return BSPData.RDBS.GetRechargeList(pageNumber, pageSize, condition);
        }

        /// <summary>
        /// 获取用户账户余额
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static DataTable GetUserMoney(string account)
        {
            return BSPData.RDBS.GetUserMoney(account);
        }


        #region 银行卡和提现密码
        /// <summary>
        /// 更新绑定银行卡信息
        /// </summary>
        /// <param name="drawa"></param>
        /// <returns></returns>
        public static string UpdateDrawCardInfo(MD_DrawAccount drawa)
        {
            return BSPData.RDBS.UpdateDrawCardInfo(drawa);
        }
        /// <summary>
        /// 更新提现密码
        /// </summary>
        /// <param name="drawa"></param>
        /// <returns></returns>
        public static string UpdateDrawPWD(MD_DrawAccount drawa)
        {
            return BSPData.RDBS.UpdateDrawPWD(drawa);
        }
        /// <summary>
        /// 验证提现密码是否正确
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string ValidateDrawPwd(string account, string pwd)
        {
            return BSPData.RDBS.ValidateDrawPwd(account, pwd);
        }
        /// <summary>
        /// 删除提现账号
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static string DeleteDrawAccount(string account)
        {
            return BSPData.RDBS.DeleteDrawAccount(account);
        }

        /// <summary>
        /// 是否设置提现密码
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static string ValidateDrawPwd(string account)
        {
            return BSPData.RDBS.ValidateDrawPwd(account);
        }
        /// <summary>
        ///  获取提现账号(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable GetDrawAccountList(int pageNumber, int pageSize, string condition = "")
        {
            return BSPData.RDBS.GetDrawAccountList(pageNumber, pageSize, condition);
        }
        #endregion
       

       
        #region 提现
        public static  string AddChangeWare(MD_DrawAccount draw)
        {
            return BSPData.RDBS.AddChangeWare(draw);
        }
        /// <summary>
        /// 添加提现记录
        /// </summary>
        /// <param name="draw"></param>
        /// <returns></returns>
        public static string AddDraw(DrawInfoModel draw)
        {
            return BSPData.RDBS.AddDraw(draw);
        }
        /// <summary>
        /// 更新提现记录 
        /// </summary>
        /// <param name="draw"></param>
        /// <returns></returns>
        public static string UpdateDraw(DrawInfoModel draw)
        {
            return BSPData.RDBS.UpdateDraw(draw);
        }


        /// <summary>
        /// 删除提现记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string DeleteDraw(string id)
        {
            return BSPData.RDBS.DeleteDraw(id);
        }


        /// <summary>
        ///  获取提现记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable GetDrawList(int pageNumber, int pageSize, string condition = "")
        {
            return BSPData.RDBS.GetDrawList(pageNumber,pageSize,condition);
        }

        
        #endregion

       
    }
}
