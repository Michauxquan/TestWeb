using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.Web.Admin.Models;
using System.Configuration;
using Newtonsoft.Json;
using System.Text;
using OWZX.Model;
using System.Data;
using OWZX.Core.Helper;
using System.IO;


namespace OWZX.Web.Admin.Controllers
{
    /// <summary>
    /// 财务控制器类
    /// </summary>
    public partial class DrawController : BaseAdminController
    {

        #region 提现
        /// <summary>
        /// 提现列表
        /// </summary>
        public ActionResult DrawList(string username = "", string mobile = "", string alipayaccount = "", string status = "-1", int pageSize = 15, int pageNumber = 1)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(" where 1=1 ");
            if (username != "")
                strb.Append(" and a.username like '%" + username + "%' ");
            if (mobile != "")
                strb.Append(" and a.mobile like '" + mobile + "%' ");
            if (alipayaccount != "")
                strb.Append(" and a.alipay like '" + alipayaccount + "%' ");
            if (status != "-1" && status != "")
                strb.Append(" and a.state =" + status);

            strb.Append(" order by a.[addtime] desc");

            List<DrawInfoModel> drawlist = Recharge.GetDrawList(pageNumber, pageSize, strb.ToString());
            DrawListModel model = new DrawListModel()
            {
                DrawList = drawlist,
                PageModel = new PageModel(pageSize, pageNumber, drawlist.Count > 0 ? drawlist[0].TotalCount : 0),
                Status = int.Parse(status),
                UserName = username,
                Mobile = mobile,
                AlipayAccount = alipayaccount
            };

            ShopUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&UserName={3}&Mobile={4}&Status={5}&AlipayAccount={6}",
                                                          Url.Action("DrawList"), pageNumber, pageSize,
                                                          username, mobile, status, alipayaccount));
            return View(model);
        }

        /// <summary>
        /// 完成提现转账
        /// </summary>
        public ActionResult EditDraw(int drawId, string exception, string type)
        {
            List<DrawInfoModel> listdraw = Recharge.GetDrawList(1, 15, " where drawid=" + drawId);
            DrawInfoModel draw = new DrawInfoModel();
            if (listdraw.Count > 0)
            {
                draw = listdraw[0];
            }
            else
                return AjaxResult("error", "提现信息不存在");

            if (type == "2")
            {
                //完成
                draw.Updateuser = WorkContext.UserName;
                if (exception == "")
                    draw.State = "2";
                else
                    draw.State = "3";
                draw.Exception = exception;
            }
            else if (type == "1")
            {
                //审核中
                draw.Exception = "";
                draw.Updateuser = WorkContext.UserName;
                draw.State = "1";
            }
            bool result = Recharge.UpdateDraw(draw);
            if (result)
            {
                return AjaxResult("success", "处理成功");
            }
            else
                return AjaxResult("error", "处理失败");

        }

        /// <summary>
        /// 删除提现
        /// </summary>
        public ActionResult DelDraw(int[] drawId)
        {
            List<DrawInfoModel> list = Recharge.GetDrawList(1, 15, " where drawid in (" + CommonHelper.IntArrayToString(drawId) + ")");
            if (list.Count > 0)
            {
                bool result = Recharge.DeleteDraw(CommonHelper.IntArrayToString(drawId));
                if (result)
                {
                    return PromptView("删除成功");
                }
                else
                    return PromptView("删除失败");
            }
            return PromptView("提现信息不存在");
        }

        

        #endregion

        #region 充值
        public ActionResult RechargeList(string mobile = "", string suite = "-1", string platform = "全部", string starttime = "", string endtime = "", int pageSize = 15, int pageNumber = 1)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(" where 1=1 ");
            if (mobile != "")
                strb.Append(" and a.account like '" + mobile + "%' ");
            if (suite != "-1" )
                strb.Append(" and a.vossuiteid = " + suite + " ");
            if (platform != "全部" )
                strb.Append(" and a.platform = '" + platform + "' ");
            if (starttime != "" && endtime != "")
                strb.Append(" and (convert(varchar(10),a.paytime,120) between '" + starttime + "' and '" + endtime + "')");
            strb.Append(" order by a.addtime desc ");

            List<RechargeModel> rechargelist = Recharge.GetRechargeList(pageNumber, pageSize, strb.ToString());
            RechargeListModel model = new RechargeListModel()
            {
                RechargeList = rechargelist,
                PageModel = new PageModel(pageSize, pageNumber, rechargelist.Count > 0 ? rechargelist[0].TotalCount : 0),
                Mobile = mobile,
                Suite = suite,
                PlatForm = platform,
                StartTime = starttime,
                EndTime = endtime

            };

            return View(model);
        }

        /// <summary>
        /// 充值导出excel
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public FileResult ExportExcelForRech(string mobile = "", string suite = "-1", string platform = "全部", string starttime = "", string endtime = "")
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(" where 1=1 ");
            if (mobile != "")
                strb.Append(" and a.account like '" + mobile + "%' ");
            if (suite != "-1")
                strb.Append(" and a.vossuiteid = " + suite + " ");
            if (platform != "全部")
                strb.Append(" and a.platform = '" + platform + "' ");
            if (starttime != "" && endtime != "")
                strb.Append(" and (convert(varchar(10),a.paytime,120) between '" + starttime + "' and '" + endtime + "')");
            strb.Append(" order by a.addtime desc ");
            DataTable dt = Recharge.GetRechargeListForDt(1, -1, strb.ToString());

            Dictionary<string, string> listcol = new Dictionary<string, string>() { };
            listcol["编号"] = "rechargeid"; listcol["手机号"] = "account"; listcol["姓名"] = "nickname"; listcol["职位"] = "userrank"; listcol["充值金额"] = "total_fee";
            listcol["充值套餐"] = "suitename";
            listcol["支付方式"] = "platform"; listcol["充值时间"] = "paytime";

            string html = ExcelHelper.BuildHtml(dt, listcol);


            //第一种:使用FileContentResult
            byte[] fileContents = Encoding.Default.GetBytes(html);
            return File(fileContents, "application/ms-excel", "充值信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            ////第二种:使用FileStreamResult
            //var fileStream = new MemoryStream(fileContents);
            //return File(fileStream, "application/ms-excel", "fileStream.xls");

            ////第三种:使用FilePathResult
            ////服务器上首先必须要有这个Excel文件,然会通过Server.MapPath获取路径返回.
            //var fileName = Server.MapPath("~/Files/fileName.xls");
            //return File(fileName, "application/ms-excel", "fileName.xls");
        }
        #endregion

        /// <summary>
        /// 提现导出excel
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public FileResult ExportExcel(string username = "", string mobile = "", string alipayaccount = "", string status = "")
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(" where 1=1 ");
            if (username != "")
                strb.Append(" and a.username like '%" + username + "%' ");
            if (mobile != "")
                strb.Append("  and a.mobile like '" + mobile + "%' ");
            if (alipayaccount != "")
                strb.Append(" and a.alipay like '" + alipayaccount + "%' ");
            if (status != "-1")
                strb.Append(" and a.state =" + status);

            strb.Append(" order by a.[addtime] desc");

            //编号、手机号、姓名、支付宝账号、提现金额、提现时间、状态（审核中、已完成）、异常信息(支持文本输入）
            DataTable dt = Recharge.GetDrawListForDT(1, -1, strb.ToString());

            Dictionary<string, string> listcol = new Dictionary<string, string>() { };
            listcol["编号"] = "drawid"; listcol["手机"] = "mobile"; listcol["姓名"] = "username"; listcol["支付宝账号"] = "alipay"; listcol["提现金额"] = "money";
            listcol["提现时间"] = "addtime";
            listcol["状态"] = "state"; listcol["异常信息"] = "exception";

            string html = ExcelHelper.BuildHtml(dt, listcol);


            //第一种:使用FileContentResult
            byte[] fileContents = Encoding.Default.GetBytes(html);
            return File(fileContents, "application/ms-excel", "提现信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            ////第二种:使用FileStreamResult
            //var fileStream = new MemoryStream(fileContents);
            //return File(fileStream, "application/ms-excel", "fileStream.xls");

            ////第三种:使用FilePathResult
            ////服务器上首先必须要有这个Excel文件,然会通过Server.MapPath获取路径返回.
            //var fileName = Server.MapPath("~/Files/fileName.xls");
            //return File(fileName, "application/ms-excel", "fileName.xls");
        }
        private void Load()
        {
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
        }

    }
}
