using Newtonsoft.Json;
using OWZX.Core;
using OWZX.Core.Alipay;
using OWZX.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Services
{
    public partial class Recharge
    {
        #region 充值记录
        /// <summary>
        /// 添加充值记录
        /// </summary>
        /// <param name="rech"></param>
        /// <returns></returns>
        public static bool AddRecharge(RechargeModel rech)
        {
            string result = OWZX.Data.Recharge.AddRecharge(rech);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新充值记录 
        /// </summary>
        /// <param name="rech"></param>
        /// <returns></returns>
        public static bool UpdateRecharge(RechargeModel rech)
        {
            string result = OWZX.Data.Recharge.UpdateRecharge(rech);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 充值成功更新充值记录
        /// </summary>
        /// <param name="rech"></param>
        /// <returns></returns>
        public static bool UpdateRechargeForPay(RechargeModel rech)
        {
            string result = OWZX.Data.Recharge.UpdateRechargeForPay(rech);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 删除充值记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteRecharge(string id)
        {
            string result = OWZX.Data.Recharge.DeleteRecharge(id);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        ///获取充值记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static List<RechargeModel> GetRechargeList(int pageNumber, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.Recharge.GetRechargeList(pageNumber, pageSize, condition);
            List<RechargeModel> suitelist = new List<RechargeModel>();
            if (dt.Rows.Count > 0)
                suitelist = (List<RechargeModel>)ModelConvertHelper<RechargeModel>.ConvertToModel(dt);
            return suitelist;
        }

        /// <summary>
        ///获取充值记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1取全部</param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable  GetRechargeListForDt(int pageNumber, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.Recharge.GetRechargeList(pageNumber, pageSize, condition);
            return dt;
        }

       
        #endregion

        #region 账户余额

        /// <summary>
        /// 获取账户余额
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static DataTable GetUserMoney(string account)
        {
            return OWZX.Data.Recharge.GetUserMoney(account);
        }


        
        #endregion

        #region 银行卡和提现密码
        /// <summary>
        /// 更新绑定银行卡信息
        /// </summary>
        /// <param name="drawa"></param>
        /// <returns></returns>
        public static bool UpdateDrawCardInfo(MD_DrawAccount drawa)
        {
            string result = OWZX.Data.Recharge.UpdateDrawCardInfo(drawa);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新提现密码
        /// </summary>
        /// <param name="drawa"></param>
        /// <returns></returns>
        public static bool UpdateDrawPWD(MD_DrawAccount drawa)
        {
            string result = OWZX.Data.Recharge.UpdateDrawPWD(drawa);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 验证提现密码是否正确
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static bool ValidateDrawPwd(string account, string pwd)
        {
            string result = BSPData.RDBS.ValidateDrawPwd(account, pwd);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }



        /// <summary>
        /// 删除提现账号
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static bool DeleteDrawAccount(string account)
        {
            string result = OWZX.Data.Recharge.DeleteDrawAccount(account);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否设置提现密码
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static bool ValidateDrawPwd(string account)
        {
            string result = OWZX.Data.Recharge.ValidateDrawPwd(account);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        ///  获取提现账号(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition">提现账户表别名 a;用户user表别名 b</param>
        /// <returns></returns>
        public static List<MD_DrawAccount> GetDrawAccountList(int pageNumber, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.Recharge.GetDrawAccountList(pageNumber, pageSize, condition);

            List<MD_DrawAccount> list = new List<MD_DrawAccount>();
            if (dt.Rows.Count > 0)
                list = (List<MD_DrawAccount>)ModelConvertHelper<MD_DrawAccount>.ConvertToModel(dt);
            return list;
        }
        #endregion

        #region 提现
        /// <summary>
        /// 添加提现记录
        /// </summary>
        /// <param name="draw"></param>
        /// <returns></returns>
        public static string AddDraw(DrawInfoModel draw)
        {
            return OWZX.Data.Recharge.AddDraw(draw);

        }
        /// <summary>
        /// 更新提现记录 
        /// </summary>
        /// <param name="draw"></param>
        /// <returns></returns>
        public static bool UpdateDraw(DrawInfoModel draw)
        {
            string result = OWZX.Data.Recharge.UpdateDraw(draw);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 删除提现记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteDraw(string id)
        {
            string result = OWZX.Data.Recharge.DeleteDraw(id);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        ///  获取提现记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">提现表别名 a;用户user表别名 b</param>
        /// <returns></returns>
        public static List<DrawInfoModel> GetDrawList(int pageNumber, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.Recharge.GetDrawList(pageNumber, pageSize, condition);
            List<DrawInfoModel> suitelist = new List<DrawInfoModel>();
            if (dt.Rows.Count > 0)
                suitelist = (List<DrawInfoModel>)ModelConvertHelper<DrawInfoModel>.ConvertToModel(dt);
            return suitelist;

        }
        /// <summary>
        ///  获取提现记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">提现表别名 a;用户user表别名 b</param>
        /// <returns></returns>
        public static DataTable GetDrawListForDT(int pageNumber, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.Recharge.GetDrawList(pageNumber, pageSize, condition);
            return dt;

        }

        #endregion

        /// <summary>
        /// 充值成功后续处理
        /// </summary>
        /// <param name="total_fee">总金额</param>
        /// <param name="out_trade_no">系统订单号（标识）</param>
        /// <param name="time_end">支付完成时间</param>
        /// <param name="trade_no">支付宝/微信交易号</param>
        /// <returns></returns>
        public static string UpdateRecharge(string total_fee, string out_trade_no, string time_end, string trade_no)
        {
            /*
             * 1、充值成功，更新充值记录信息
             */
            //更新充值记录
            OWZX.Model.RechargeModel rech = new OWZX.Model.RechargeModel { Out_trade_no = out_trade_no, Paytime = time_end, Total_fee = decimal.Parse(total_fee), Trade_no = trade_no };
            bool recres = Recharge.UpdateRechargeForPay(rech);
            if (recres)
            {
                //string type = string.Empty;（1:充话费 2:升级充值）
                List<RechargeModel> rchlist = Recharge.GetRechargeList(1, 1, " where out_trade_no='" + out_trade_no + "'");
                if (rchlist.Count == 0)
                {
                    return "fail";
                }
                RechargeModel rch = rchlist[0];
                return "success";
                    
            }
            else
            {
                Logs.Write("更新充值记录失败!");
                return "error";
            }
        }
    }
}
