using System;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.Collections.Generic;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.Web.Admin.Models;
using System.Text;
using Newtonsoft.Json;
using OWZX.Core.Helper;
using OWZX.Model;
using OWZX.Web.Admin.Models;

namespace OWZX.Web.Admin.Controllers
{
    /// <summary>
    /// 后台商品控制器类
    /// </summary>
    public partial class WareController : BaseAdminController
    {
        #region 商品管理
        /// <summary>
        /// 商品列表
        /// </summary>
        public ActionResult List(string wareName = "", string wareCode = "", int type = -1, int pageNumber = 1, int pageSize = 15)
        { 
            ShopUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&warename={3}&warecode={4}&type={5}",
                                                          Url.Action("list"), pageNumber, pageSize,
                                                          wareName, wareCode, type));

            StringBuilder strb = new StringBuilder();
            strb.Append(" ");
            if (wareName != "")
                strb.Append(" and wareName like '%" + wareName + "%' ");

            if (wareCode != "")
                strb.Append(" and Warecode='" + wareCode + "' ");
            if (type > -1)
                strb.Append(" and Type=" + type);
            strb.Append(" order by wareid desc");


            DataTable dt = ChangeWare.GetWareList(pageNumber, pageSize, strb.ToString());
            if (dt.Columns[0].ColumnName == "error")
                return PromptView("商品获取失败");

            WareListModel model = new WareListModel()
            {
                PageModel = new PageModel(pageSize, pageNumber, dt.Rows.Count),
                WareList = dt
            };


            return View(model);
        }

        public ActionResult OrderList(string email = "", string warename = "", string warecode = "", string btime = "", string etime = "",string content="",int status = -1,
            int pageNumber = 1, int pageSize = 15)
        { 
            ShopUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&warename={3}&warecode={4}&status={5}&email={6}&btime={7}&etime={8}&content={9}",
                                                          Url.Action("orderlist"), pageNumber, pageSize,
                                                          warename, warecode, status, email, btime, etime,content));

            StringBuilder strb = new StringBuilder();
            strb.Append(" ");
            if (warename != "")
                strb.Append(" and a.wareName like '%" + warename.Trim() + "%' ");

            if (warecode != "")
                strb.Append(" and a.Warecode='" + warecode.Trim() + "' ");
            if (status > -1)
                strb.Append(" and a.status=" + status); 
            if (btime != "")
                strb.Append(" and a.createtime>='" + btime + "' ");
            if(etime!="")
                strb.Append(" and a.createtime<'" + etime+"' ");
            if (content != "")
                strb.Append(" and cast(a.[content] as varchar(max))='" + content.Trim() + "' ");
            if (email != "")
                strb.Append(" and rtrim(b.email)='" + email + "'");
            strb.Append(" order by a.ordercode desc");


            DataTable dt = ChangeWare.GetUserOrder(pageNumber, pageSize, strb.ToString());
            if (dt.Columns[0].ColumnName == "error")
                return PromptView("订单获取失败");
            List<SelectListItem> statusGroupList = new List<SelectListItem>();
            statusGroupList.Add(new SelectListItem() { Text = "全部", Value = "-1" });
            statusGroupList.Add(new SelectListItem() { Text = "未兑换", Value = "0" });
            statusGroupList.Add(new SelectListItem() { Text = "已兑换", Value = "2" });
            statusGroupList.Add(new SelectListItem() { Text = "删除", Value = "0" });
            statusGroupList.Add(new SelectListItem() { Text = "作废", Value = "9" });
            OrderListModel model = new OrderListModel()
            {
                PageModel = new PageModel(pageSize, pageNumber, (dt != null && dt.Rows != null && dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["TotalCount"]) : 0)),
                OrderList = dt,
                WareCode = warecode,
                WareName = warename,
                Btime = btime,
                Etime = etime,
                Status = status,
                Email = email,
                Content = content,
                StatusList = statusGroupList
            };
          

            return View(model);
        }

        public ActionResult SkuList(string warecode = "")
        {
            var dt = ChangeWare.GetWareSkuList(" and a.warecode=" + warecode + " ");
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            string data = JsonConvert.SerializeObject(dt, jsetting).ToLower();
            return new JsonResult
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        [HttpGet]
        public ActionResult Add()
        {
            WareModel model = new WareModel();
            Load();
            return View(model);
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        [HttpPost]
        private ActionResult Add(WareModel model)
        {
            if (string.IsNullOrWhiteSpace(model.WareName))
                ModelState.AddModelError("WareName", "商品名称不能为空");
            if (string.IsNullOrWhiteSpace(model.WareCode))
                ModelState.AddModelError("WareCode", "商品编码不能为空");

            if (ModelState.IsValid)
            {
                Ware wareInfo = new Ware()
                {
                    WareCode = model.WareCode,
                    WareName = model.WareName,
                    Status = 0,
                    Type = model.Type,
                    ImgSrc = model.ImgSrc,
                    Price = model.Price
                };

                int wareid = ChangeWare.CreateWare(wareInfo);
                if (wareid > 0)
                {
                    return PromptView("商品添加成功");
                }
                else
                    return PromptView("商品添加失败");
            }
            return View(model);
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        [HttpPost]
        public ActionResult Add(string warecode = "")
        {
            string form = Request.Form.ToString();
            form = HttpUtility.UrlDecode(form);
            Dictionary<string, string> parms = CommonHelper.ParmsToDic(form);
            WareModel wareInfo = new WareModel();
            if (ModelState.IsValid)
            {
                Ware ware = new Ware()
                {
                    WareCode = parms["WareCode"],
                    WareName = parms["WareName"],
                    Status = int.Parse(parms["Status"]),
                    Type = int.Parse(parms["Type"]),
                    ImgSrc = parms.ContainsKey("ImgSrc") ? parms["ImgSrc"] : "",
                    Price = decimal.Parse("0")
                };
                int wareid = ChangeWare.CreateWare(ware);
                if (wareid > 0)
                {
                    return PromptView("商品添加成功");
                }
                else
                    return PromptView("商品添加失败");
            }
            else
            {
                wareInfo = new WareModel()
                {
                    WareCode = parms["WareCode"],
                    WareName = parms["WareName"],
                    Status = int.Parse(parms["Status"]),
                    Type = int.Parse(parms["Type"]),
                    ImgSrc = parms.ContainsKey("ImgSrc") ? parms["ImgSrc"] : "",
                    Price = decimal.Parse("0")

                };
            }
            Load();
            return View(wareInfo);
        }

        /// <summary>
        /// 编辑商品
        /// </summary>
        [HttpGet]
        public ActionResult Edit(string warecode = "")
        {
            Ware wareInfo = ChangeWare.GetWareByCode(warecode);
            if (wareInfo == null)
                return PromptView("商品不存在");

            WareModel model = new WareModel();
            model.WareCode = wareInfo.WareCode;
            model.WareName = wareInfo.WareName;
            model.Status = wareInfo.Status;
            model.Type = wareInfo.Type;
            model.ImgSrc = wareInfo.ImgSrc;
            model.Price = wareInfo.Price;
            Load();
            return View(model);
        }

        /// <summary>
        /// 编辑商品
        /// </summary>
        [HttpPost]
        public ActionResult Edit(WareModel model, string warecode = "")
        {
            Ware wareInfo = ChangeWare.GetWareByCode(warecode);
            if (wareInfo == null)
                return PromptView("商品不存在");

            if (ModelState.IsValid)
            {
                string warename;
                if (string.IsNullOrWhiteSpace(model.WareName))
                    warename = wareInfo.WareName;
                else
                    warename = model.WareName;

                wareInfo.WareName = warename;
                wareInfo.ImgSrc = model.ImgSrc;
                wareInfo.Status = model.Status;
                wareInfo.Type = model.Type;
                wareInfo.Price = model.Price;
                bool result = false;


                result = ChangeWare.UpdateWare(wareInfo);
                if (result)
                {
                    return PromptView("修改商品成功");
                }
                else
                {
                    return PromptView("修改商品失败");
                }
            }

            Load();

            return View(model);
        }
        /// <summary>
        /// 编辑订单状态
        /// </summary>
        ///<param name="ordercode">编码</param>
        ///<param name="status">状态  </param>
        /// <returns></returns>
        public ActionResult EditOrder(string ordercode, int status)
        {
            bool result = ChangeWare.UpdateOrderStatus(ordercode, status,WorkContext.UserEmail);
            if (result)
                return PromptView("订单状态修改成功");
            else
                return PromptView("订单状态修改失败");
        }
        /// <summary>
        /// 添加商品
        /// </summary>
        /// [HttpPost]
        public ActionResult SkuAdd(string warecode = "")
        {
            SkuModel skuInfo = new SkuModel();
            string form = Request.Form.ToString();
            form = HttpUtility.UrlDecode(form);
            if (string.IsNullOrEmpty(form))
            {

                skuInfo = new SkuModel()
                {
                    WareCode = warecode
                };
                Load();
                return View(skuInfo);
            }
            Dictionary<string, string> parms = CommonHelper.ParmsToDic(form);
            if (ModelState.IsValid)
            {
                Sku ware = new Sku()
                {
                    WareCode = parms["WareCode"],
                    SpecName = parms["SpecName"],
                    SpecCode = parms["SpecCode"],
                    Status = int.Parse(parms["Status"]),
                    Price = decimal.Parse(parms["Price"]),
                    ImgSrc = parms.ContainsKey("ImgSrc") ? parms["ImgSrc"] : "",
                    UserNum = int.Parse(parms["UserNum"])
                };
                int wareid = ChangeWare.CreateWareSku(ware);
                if (wareid > 0)
                {
                    return PromptView("商品规格添加成功");
                }
                else
                    return PromptView("商品规格添加失败");
            }
            else
            {
                skuInfo = new SkuModel()
                {
                    WareCode = parms["WareCode"],
                    SpecName = parms["SpecName"],
                    SpecCode = parms["SpecCode"],
                    Status = int.Parse(parms["Status"]),
                    Price = decimal.Parse(parms["Price"]),
                    ImgSrc = parms.ContainsKey("ImgSrc") ? parms["ImgSrc"] : "",
                    UserNum = int.Parse(parms["UserNum"])

                };
            }
            Load();
            return View(skuInfo);
        }

        [HttpPost]
        private ActionResult SkuAdd(SkuModel model)
        {
            if (string.IsNullOrWhiteSpace(model.SpecCode))
                ModelState.AddModelError("SpecCode", "规格编码不能为空");
            if (string.IsNullOrWhiteSpace(model.SpecName))
                ModelState.AddModelError("SpecName", "规格名称不能为空");
            if (string.IsNullOrWhiteSpace(model.WareCode))
                ModelState.AddModelError("WareCode", "商品编码不能为空");

            if (ModelState.IsValid)
            {
                Sku skuInfo = new Sku()
                {
                    WareCode = model.WareCode,
                    SpecCode = model.SpecCode,
                    SpecName = model.SpecName,
                    Status = 0,
                    UserNum = model.UserNum,
                    Price = model.Price
                };

                int wareid = 0;
                wareid = ChangeWare.CreateWareSku(skuInfo);
                if (wareid > 0)
                {
                    return PromptView("商品规格添加成功");
                }
                else
                    return PromptView("商品规格添加失败");
            }
            return View(model);
        }
        /// <summary>
        /// 编辑用户Sku
        /// </summary>
        [HttpGet]
        public ActionResult EditSku(int specid = -1)
        {
            Sku wareInfo = ChangeWare.GetWareSkuByID(specid);
            if (wareInfo == null)
                return PromptView("规格信息不存在");
            Load();
            SkuModel model = new SkuModel();
            model.WareCode = wareInfo.WareCode;
            model.SpecCode = wareInfo.SpecCode;
            model.SpecName = wareInfo.SpecName;
            model.Status = wareInfo.Status;
            model.UserNum = wareInfo.UserNum;
            model.Price = wareInfo.Price;
            return View(model);
        }

        /// <summary>
        /// 编辑商品Sku
        /// </summary>
        [HttpPost]
        public ActionResult EditSku(SkuModel model, int specid = -1)
        {
            Sku skuInfo = ChangeWare.GetWareSkuByID(specid);
            if (skuInfo == null)
                return PromptView("规格信息不存在");

            if (ModelState.IsValid)
            {
                skuInfo.SpecName = model.SpecName;
                skuInfo.SpecCode = model.SpecCode;
                skuInfo.WareCode = model.WareCode;
                skuInfo.Price = model.Price;
                skuInfo.Status = model.Status;
                skuInfo.UserNum = model.UserNum;

                bool result = false;

                result = ChangeWare.UpdateWareSku(skuInfo);
                if (result)
                {
                    return PromptView("规格信息修改成功");
                }
                else
                {
                    return PromptView("规格信息修改失败");
                }
            }
            Load();

            return View(model);
        }

        /// <summary>
        /// 删除shangpin
        /// </summary>
        ///<param name="warecode">编码</param>
        ///<param name="status">状态  </param>
        /// <returns></returns>
        public ActionResult Del(string warecode, int status)
        {
            bool result = ChangeWare.UpdateWareStatus(warecode, status);
            if (result)
                return PromptView("商品状态修改成功");
            else
                return PromptView("商品状态修改失败");
        }

        /// <summary>
        /// 删除shangpin
        /// </summary>
        ///<param name="specid">编码</param>
        ///<param name="status">状态  </param>
        /// <returns></returns>
        public ActionResult DelSku(int specid, int status)
        {
            bool result = ChangeWare.UpdateWareSkuStatus(specid, status);
            if (result)
                return PromptView("规格状态修改成功");
            else
                return PromptView("规格状态修改失败");
        }
        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public FileResult ExportExcel(string username = "", string mobile = "")
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(" where 1=1");
            if (username != "")
                strb.Append(" and a.username like '" + username + "%'");

            if (mobile != "")
                strb.Append(" and a.mobile='" + mobile + "'"); 
            long SumFee = 0;

            DataTable dt = AdminUsers.GetUserList(-1, 1,ref SumFee, strb.ToString());

            Dictionary<string, string> listcol = new Dictionary<string, string>() { };
            listcol["编号"] = "uid"; listcol["用户名"] = "username"; listcol["手机"] = "mobile"; listcol["姓名"] = "nickname"; listcol["职位"] = "userrank"; listcol["推荐人"] = "recomuser";
            listcol["在用套餐"] = "hassuite"; listcol["剩余分钟数"] = "totalmin"; listcol["有效期"] = "remainmin"; listcol["充值总额"] = "recharge"; listcol["总赠送分钟数"] = "giftmin";
            listcol["收益总额"] = "totalincome"; listcol["注册时间"] = "registertime";
            listcol["访问时间"] = "lastvisittime";

            string html = ExcelHelper.BuildHtml(dt, listcol);


            //第一种:使用FileContentResult
            byte[] fileContents = Encoding.Default.GetBytes(html);
            return File(fileContents, "application/ms-excel", "用户信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

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
            List<SelectListItem> typeGroupList = new List<SelectListItem>();
            typeGroupList.Add(new SelectListItem() { Text = "兑换商品", Value = "0" });
            //typeGroupList.Add(new SelectListItem() { Text = "夺宝商品", Value = "1" });
            ViewData["typeGroupList"] = typeGroupList;
            List<SelectListItem> statusGroupList = new List<SelectListItem>();
            statusGroupList.Add(new SelectListItem() { Text = "销售中", Value = "0" });
            statusGroupList.Add(new SelectListItem() { Text = "停售", Value = "1" });
            ViewData["statusGroupList"] = statusGroupList;
            ViewData["countyId"] = -1;
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
        }
        #endregion

    }
}
