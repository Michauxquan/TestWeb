using OWZX.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Core
{
    // 充值
    public partial interface IRDBSStrategy
    {
        /// <summary>
        /// 添加充值记录
        /// </summary>
        /// <param name="rech"></param>
        /// <returns></returns>
        string AddRecharge(RechargeModel rech);
        /// <summary>
        /// 更新充值记录 
        /// </summary>
        /// <param name="rech"></param>
        /// <returns></returns>
        string UpdateRecharge(RechargeModel rech);

        /// <summary>
        /// 充值成功更新充值记录
        /// </summary>
        /// <param name="rech"></param>
        /// <returns></returns>
        string UpdateRechargeForPay(RechargeModel rech);
        /// <summary>
        /// 删除充值记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string DeleteRecharge(string id);


        /// <summary>
        ///  获取充值记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataTable GetRechargeList(int pageNumber, int pageSize, string condition = "");



        /// <summary>
        /// 获取账户余额
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        DataTable GetUserMoney(string account);

        #region 银行卡和提现密码
        /// <summary>
        /// 更新绑定银行卡信息
        /// </summary>
        /// <param name="drawa"></param>
        /// <returns></returns>
        string UpdateDrawCardInfo(MD_DrawAccount drawa);
        /// <summary>
        /// 更新提现密码
        /// </summary>
        /// <param name="drawa"></param>
        /// <returns></returns>
        string UpdateDrawPWD(MD_DrawAccount drawa);

        /// <summary>
        /// 验证提现密码是否正确
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        string ValidateDrawPwd(string account, string pwd);
        /// <summary>
        /// 删除提现账号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string DeleteDrawAccount(string id);

        /// <summary>
        /// 是否设置提现密码
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        string ValidateDrawPwd(string account);
        /// <summary>
        ///  获取提现账号(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataTable GetDrawAccountList(int pageNumber, int pageSize, string condition = "");
        #endregion

        #region 提现
        string AddChangeWare(MD_DrawAccount draw);
        /// <summary>
        /// 添加提现记录
        /// </summary>
        /// <param name="draw"></param>
        /// <returns></returns>
        string AddDraw(DrawInfoModel draw);
        /// <summary>
        /// 更新提现记录 
        /// </summary>
        /// <param name="draw"></param>
        /// <returns></returns>
        string UpdateDraw(DrawInfoModel draw);


        /// <summary>
        /// 删除提现记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string DeleteDraw(string id);


        /// <summary>
        ///  获取提现记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataTable GetDrawList(int pageNumber, int pageSize, string condition = "");


       
        #endregion


    }
}
