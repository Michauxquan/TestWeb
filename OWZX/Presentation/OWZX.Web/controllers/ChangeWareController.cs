using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using OWZX.Model;
using OWZX.Services;
using OWZX.Web.Framework;

namespace OWZX.Web.controllers
{
    /// <summary>
    /// 兑换夺宝控制器类
    /// </summary>
    public partial class ChangeWareController : BaseWebController
    {
        private object lkchangeware = new object();




        public ActionResult ChangeList(int pageSize = 15, int pageNumber = 1,int specid=-1)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(" ");
            if (specid>-1)
            {
                strb.Append(" and  specid="+specid);
            } 
            List<MD_Ware> list = ChangeWare.GetWareSkuList(pageNumber, pageSize, strb.ToString());
            WareSkuList warelist = new WareSkuList
            {
                specid = specid, 
                PageModel = new PageModel(pageSize, pageNumber, list.Count > 0 ? list[0].TotalCount : 0),
                WareList = list
            };
            return View(warelist);
        }

        public ActionResult ChangeDetail(int specid)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(" ");
            if (specid > -1)
            {
                strb.Append(" and  specid=" + specid);
            }
            List<MD_Ware> list = ChangeWare.GetWareSkuList(1, 1, strb.ToString());
            WareSkuList warelist = new WareSkuList
            {
                specid = specid,
                PageModel = new PageModel(1, 1, list.Count > 0 ? list[0].TotalCount : 0),
                WareList = list
            };
            return View(warelist);
        }
        public ActionResult AddChangeWare()
        {
            NameValueCollection parmas = WorkContext.postparms;
            if (parmas.Keys.Count != 14)
            {
                return APIResult("error", "缺少请求参数");
            }
            int type = int.Parse(parmas["type"]);
            if (type == 0)
            {
                if (string.IsNullOrEmpty(parmas["content"]))
                {
                    return APIResult("error", "安全密码不能为空");
                }
                string oldpwd = Users.CreateUserPassword(parmas["content"], WorkContext.PartUserInfo.Salt);
                bool pwdres = Recharge.ValidateDrawPwd(parmas["account"], oldpwd);
                if (!pwdres)
                    return APIResult("error", "安全密码错误");
            }
            else if (type == 1 && string.IsNullOrEmpty(parmas["issuenum"]))
            {
                return APIResult("error", "夺宝期数不能为空");
            }
            lock (lkchangeware)
            {
                MD_UserOrder order = new MD_UserOrder
                {
                    OrderCode = parmas["ordercode"],
                    ChangeID = int.Parse(parmas["changeid"]),
                    Account = parmas["account"],
                    Status = 0,
                    Type = int.Parse(parmas["type"]),
                    WareName = parmas["warename"],
                    WareCode = parmas["warecode"],
                    SpecName = parmas["specname"],
                    SpecCode = parmas["speccode"],
                    Price = decimal.Parse(parmas["price"]),
                    TotalFee = decimal.Parse(parmas["totalfee"]),
                    Content = "",
                    Num = int.Parse(parmas["num"])
                };
                order.IssueNum = type == 1 ? parmas["issuenum"] : "";
                string betres = ChangeWare.AddUserOrder(order);
                if (string.IsNullOrEmpty(betres))
                    return APIResult("success", "投注成功");
                else
                    return APIResult("error", betres);
            }
        }

        /// <summary>
        /// 获取夺宝记录
        /// </summary>
        /// <returns></returns>
        public ActionResult GetChangeWare()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                int page = int.Parse(parmas["page"]);
                string type = parmas["type"];

                string sqlwhere = "";
                if (!string.IsNullOrEmpty(type))
                {
                    sqlwhere += " and type=" + type;
                }

                if (!string.IsNullOrEmpty(parmas["changeid"]))
                {
                    sqlwhere += " and changeid=" + parmas["changeid"];
                }
                if (!string.IsNullOrEmpty(parmas["issuenum"]))
                {
                    sqlwhere += " and issuenum='" + parmas["issuenum"] + "'";
                }
                DataTable dt = ChangeWare.GetChangeWare(page, 15, sqlwhere);
                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                string data = JsonConvert.SerializeObject(dt, jsetting).ToLower();
                return APIResult("success", data, true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }


        /// <summary>
        /// 获取商品记录
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWareList()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                int page = int.Parse(parmas["page"]);
                string type = parmas["type"];
                string sqlwhere = " and status=0 ";
                if (!string.IsNullOrEmpty(type))
                {
                    sqlwhere += " and type=" + type;
                }
                if (!string.IsNullOrEmpty(parmas["wareid"]))
                {
                    sqlwhere += " and wareid=" + parmas["wareid"];
                }
                if (!string.IsNullOrEmpty(parmas["warecode"]))
                {
                    sqlwhere += " and warecode='" + parmas["warecode"] + "'";
                }
                DataTable dt = ChangeWare.GetWareList(page, 15, sqlwhere);
                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                string data = JsonConvert.SerializeObject(dt, jsetting).ToLower();
                return APIResult("success", data, true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }

        /// <summary>
        /// 获取商品Sku记录
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWareSkuList()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                string sqlwhere = " and a.status=0  and b.status=0 ";
                if (!string.IsNullOrEmpty(parmas["warecode"]))
                {
                    sqlwhere=sqlwhere+"and b.warecode='" + parmas["warecode"] + "'";
                }
                DataTable dt = ChangeWare.GetWareSkuList(sqlwhere);
                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                string data = JsonConvert.SerializeObject(dt, jsetting).ToLower();
                return APIResult("success", data, true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }

        public ActionResult GetUserOrder()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                int page = int.Parse(parmas["page"]);
                if (string.IsNullOrEmpty(parmas["type"]))
                {
                    return APIResult("error", "订单类型不能为空");
                }
                string status = parmas["status"];
                string sqlwhere = "";
                if (!string.IsNullOrEmpty(status))
                {
                    if (status != "-1")
                    {
                        sqlwhere += " and a.status in(" + status + ") ";
                    }
                }
                if (!string.IsNullOrEmpty(parmas["ismyself"]) && parmas["ismyself"] == "1")
                {
                    sqlwhere += " and a.userid=(select top 1 uid  from owzx_users where mobile='" + parmas["account"] + "')";
                }
                DataTable dt = ChangeWare.GetUserOrder(page, 15, " and a.type=" + parmas["type"] + sqlwhere);
                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                string data = JsonConvert.SerializeObject(dt, jsetting).ToLower();
                return APIResult("success", data, true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
    }
}
