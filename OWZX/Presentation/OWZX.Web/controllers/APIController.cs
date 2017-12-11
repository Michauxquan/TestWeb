using log4net;
using Newtonsoft.Json;
using OWZX.Core;
using OWZX.Core.Alipay;
using OWZX.Model;
using OWZX.Services;
using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OWZX.Web.controllers
{
    /// <summary>
    /// 配置
    /// </summary>
    public class APIController : BaseWebController
    {

        private readonly static ILog logger = LogManager.GetLogger("API");

        #region 账户
        /// <summary>
        /// 获取账户余额
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMoney()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                if (parmas.Keys.Count != 2)
                {
                    return APIResult("error", "缺少请求参数");
                }


                DataTable dt = Recharge.GetUserMoney(parmas["account"]);
                return APIResult("success", "{\"money\":\"" + dt.Rows[0]["totalmoney"].ToString().Trim() + "\"}", true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        /// <summary>
        /// 用户回水
        /// </summary>
        /// <returns></returns>
        public ActionResult UserBack()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                if (parmas.Keys.Count != 3)
                {
                    return APIResult("error", "缺少请求参数");
                }
                string where = " where rtrim(b.mobile)='" + parmas["account"] + "'";
                List<MD_UserBack> list = NewUser.GetBackList(int.Parse(parmas["page"]), 15, where);
                StringBuilder strb = new StringBuilder();
                strb.Append("[");
                foreach (MD_UserBack ubk in list)
                {
                    string tt = string.Empty;
                    switch (ubk.Status)
                    {
                        case 0: tt = "未结算";
                            break;
                        case 1: tt = "审核中";
                            break;
                        case 2: tt = "已结算";
                            break;
                        case 3: tt = "结算失败";
                            break;
                    }
                    strb.Append("{\"money\":" + ubk.Money.ToString() + ",\"status\":\"" + tt + "\",\"time\":\"" + ubk.Addtime + "\"},");

                }
                if (list.Count > 0)
                    strb = strb.Remove(strb.Length - 1, 1);
                strb.Append("]");
                //JsonSerializerSettings jsetting = new JsonSerializerSettings();
                //jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "Money", "Status", "Addtime" }, true);
                //string data = JsonConvert.SerializeObject(list, jsetting).ToLower();
                string data = strb.ToString().ToLower();
                if (list.Count > 0)
                    return APIResult("success", data, true);
                else
                    return APIResult("error", "暂无回水记录");
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        /// <summary>
        /// 账变记录
        /// </summary>
        /// <returns></returns>
        public ActionResult Change()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                if (parmas.Keys.Count != 3)
                {
                    return APIResult("error", "缺少请求参数");
                }
                string where = " where rtrim(b.mobile)='" + parmas["account"] + "'";
                List<MD_Change> list = NewUser.GetAChangeList(int.Parse(parmas["page"]), 15, where);

                if (list.Count > 0)
                {
                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "Time", "Money", "Remark"}, true);
                    jsetting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    string data = JsonConvert.SerializeObject(list, jsetting).ToLower();
                    return APIResult("success", data, true);
                }
                else
                    return APIResult("error", "暂无账变记录");
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        /// <summary>
        /// 提交充值信息
        /// </summary>
        /// <returns></returns>
        public ActionResult AddRemit()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                if (parmas.Keys.Count < 6 || (parmas.Keys.Count == 7 && !parmas.AllKeys.Contains("bankname")))
                {
                    return APIResult("error", "缺少请求参数");
                }

                MD_Remit rmt = new MD_Remit
                {
                    Mobile = parmas["account"],
                    Type = parmas["type"],
                    Name = parmas["name"],
                    Account = parmas["number"],
                    Money = int.Parse(parmas["money"]),
                    Status = 0
                };
                if (parmas.AllKeys.Contains("bankname"))
                {
                    rmt.Bankname = parmas["bankname"];
                }

                bool addres = NewUser.AddUserRemit(rmt);
                if (addres)
                {
                    return APIResult("success", "提交成功");
                }
                else
                {
                    return APIResult("error", "提交失败");
                }

            }
            catch (Exception ex)
            {
                return APIResult("error", "提交失败");
            }
        }
        /// <summary>
        /// 充值记录
        /// </summary>
        /// <returns></returns>
        public ActionResult UserRemit()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                if (parmas.Keys.Count != 3)
                {
                    return APIResult("error", "缺少请求参数");
                }
                string where = " where rtrim(b.mobile)='" + parmas["account"] + "'";
                List<MD_Remit> list = NewUser.GetUserRemitList(int.Parse(parmas["page"]), 15, where);

                if (list.Count > 0)
                {
                    list.ForEach((x) =>
                    {

                        switch (x.Status)
                        {
                            case 0:
                                x.State = "待审核";
                                break;
                            case 1: x.State = "审核中";
                                break;
                            case 2: x.State = "成功";
                                break;
                            case 3: x.State = "充值失败";
                                break;
                        }
                    });

                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "Name", "Account", "RealMoney", "Bankname", "Remark", "State", "Addtime" }, true);
                    jsetting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    string data = JsonConvert.SerializeObject(list, jsetting).ToLower();
                    return APIResult("success", data, true);
                }
                else
                    return APIResult("error", "暂无充值记录");
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        /// <summary>
        ///商户账号
        /// </summary>
        /// <returns></returns>
        public ActionResult ChargeAccount()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                List<BaseInfo> list = BSPConfig.BaseConfig.BaseList;
                if (list.Count > 0)
                {
                    string type = parmas["type"];
                    string data=string.Empty ;
                    BaseInfo baseinfo = list.Find(x => x.Key == type);
                    if (type == "微信")
                    {
                        data = BSPConfig.ShopConfig.SiteUrl + "/upload/imgs/" + baseinfo.Image;
                        data = "{\"Key\":\"微信\",\"Name\":\"\",\"Account\":\"" + data + "\",\"Bank\":\"\",\"BankAddress\":\"\"}";
                        return APIResult("success",data.ToLower(),true);
                    }
                    else if (type == "支付宝" || type == "银行卡")
                    {
                        JsonSerializerSettings jsetting = new JsonSerializerSettings();
                        jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "Key", "Name", "Account", "Bank", "BankAddress"}, true);
                        data = JsonConvert.SerializeObject(baseinfo,jsetting).ToLower();
                        return APIResult("success", data,true);
                    }
                }
                return APIResult("error", "暂无账户信息");
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        /// <summary>
        /// 获取银行卡类型
        /// </summary>
        /// <returns></returns>
        public ActionResult Bank()
        {
            try
            {
                List<BaseTypeModel> list = AdminBaseInfo.GetBaseTypeList("where parentid=31  and ishide=0");

                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "Type"}, true);
                string data = JsonConvert.SerializeObject(list, jsetting).ToLower();

                return APIResult("success", data, true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }

        #endregion

        #region 提现
        /// <summary>
        /// 提现申请
        /// </summary>
        /// <returns></returns>
        public ActionResult UserDraw()
        {
            NameValueCollection parmas = WorkContext.postparms;
            if (parmas.Keys.Count != 4)
            {
                return APIResult("error", "缺少请求参数");
            }
            string account = parmas["account"];
            decimal money = decimal.Parse(parmas["money"]);

            if (money < 100)
                return APIResult("error", "最低请提现100元宝");

            PartUserInfo partUserInfo = Users.GetPartUserByMobile(account);

            if (partUserInfo.TotalMoney < money)
                return APIResult("error", "余额不足");

            string mdpwd = Users.CreateUserPassword(parmas["password"], partUserInfo.Salt);

            bool pwdres = Recharge.ValidateDrawPwd(account, mdpwd);
            if (!pwdres)
                return APIResult("error", "提现密码错误");

            DrawInfoModel draw = new DrawInfoModel
            {
                Account = account,
                Money = int.Parse(parmas["money"]),
            };
            string addres = Recharge.AddDraw(draw);
            if (addres.EndsWith("成功"))
            {
                return APIResult("success", "申请成功");
            }
            else if (addres == "-1")
            {
                return APIResult("error", "余额不足");
            }
            else
            {
                return APIResult("error", "申请失败");
            }
        }
        /// <summary>
        /// 提现记录
        /// </summary>
        /// <returns></returns>
        public ActionResult UserDrawList()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                if (parmas.Keys.Count != 3)
                {
                    return APIResult("error", "缺少请求参数");
                }
                int pageindex = int.Parse(parmas["page"]);

                List<DrawInfoModel> userdraw = Recharge.GetDrawList(pageindex, 15, " where rtrim(b.mobile)='" + parmas["account"] + "'");
                if (userdraw.Count == 0)
                {
                    return APIResult("error", "暂无提现记录");
                }
                else
                {
                    userdraw.ForEach((x) =>
                    {
                        x.State = x.State.Replace("审核完成", "成功").Replace("审核失败", "提现失败");
                    });

                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "Addtime", "Money", "State" }, true);
                    jsetting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    string data = JsonConvert.SerializeObject(userdraw, jsetting).ToLower();
                    return APIResult("success", data, true);
                }
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }

        }
        ///// <summary>
        ///// 提现条件
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult DrawSet()
        //{ 

        //}
        #endregion

        #region 海报下载
        public ActionResult hbdownload()
        {
            try
            {
                List<BaseInfoModel> infolist = AdminBaseInfo.GetBaseInfoList(4);
                if (infolist.Count == 0)
                    return APIResult("error", "没有海报信息");
                BaseInfoModel binfo = AdminBaseInfo.GetBaseInfoList(4)[0];
                string content = binfo.Content;
                string[] list = CommonHelper.GetHtmlImageUrlList(content);
                string img = list[0];
                return APIResult("success", BSPConfig.ShopConfig.SiteUrl + img);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取海报信息失败");
            }
        }
        #endregion

        #region 动态
        /// <summary>
        /// 通知公告
        /// </summary>
        /// <returns></returns>
        public ActionResult Notice()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                int page = int.Parse(parmas["page"]);
                DataTable dt = News.GetNewsList(10, page, "where newstypeid=2");
                List<MD_NewsInfo> list = (List<MD_NewsInfo>)ModelConvertHelper<MD_NewsInfo>.ConvertToModel(dt);
                if (list.Count > 0)
                {
                    list.ForEach((x) =>
                    {
                        x.Body = BSPConfig.ShopConfig.SiteUrl + "/home/notice/" + x.NewsId.ToString();
                    });
                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "Title", "Time", "Body" }, true);
                    string data = JsonConvert.SerializeObject(list, jsetting).ToLower();
                    return APIResult("success", data, true);
                }
                else
                {
                    return APIResult("success", "暂无通知公告");
                }
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        /// <summary>
        /// 系统通知公告
        /// </summary>
        /// <returns></returns>
        public ActionResult SystemNotice()
        {
            try
            {
                DataTable dt = News.GetNewsList(1, 1, "where newstypeid=3 and convert(varchar(10),addtime,120)=convert(varchar(10),getdate(),120)");
                List<MD_NewsInfo> list = (List<MD_NewsInfo>)ModelConvertHelper<MD_NewsInfo>.ConvertToModel(dt);
                if (list.Count > 0)
                {
                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "Title", "Time", "Body" }, true);
                    string data = JsonConvert.SerializeObject(list[0], jsetting).ToLower();
                    return APIResult("success", data, true);
                }
                else
                {
                    return APIResult("success", "暂无通知公告");
                }
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        
        /// <summary>
        /// 根据用户获取未读系统公告
        /// </summary>
        /// <returns></returns>
        public ActionResult GetNews()
        {
            try
            {
                string result = WebHelper.GetPostStr();
                NameValueCollection parmas = WebHelper.GetParmList(result);

                DataTable addres = NewUser.GetUserSysNew(parmas["account"]);
                if (addres != null && !addres.Columns.Contains("status"))
                {
                    JsonSerializerSettings jsset = new JsonSerializerSettings();
                    jsset.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    string data = JsonConvert.SerializeObject(addres, jsset).ToLower();
                    return AjaxResult("success", data.Replace("[", "").Replace("]", ""), true);
                }
                else
                    return AjaxResult("success", "暂无通知公告");
            }
            catch (Exception ex)
            {
                return AjaxResult("error", "获取失败");
            }
        }
        /// <summary>
        /// 我的消息
        /// </summary>
        /// <returns></returns>
        public ActionResult Message()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                if (parmas.Keys.Count != 3)
                {
                    return APIResult("error", "缺少请求参数");
                }
                string mobile = parmas["account"];
                int page = int.Parse(parmas["page"]);
                List<MD_Message> list = NewUser.GetMessageList(page, 10, " where rtrim(b.mobile)='" + mobile + "'");

                if (list.Count > 0)
                {
                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "Title", "Time", "Body" }, true);
                    string data = JsonConvert.SerializeObject(list, jsetting).ToLower();
                    return APIResult("success", data, true);
                }
                else
                {
                    return APIResult("success", "暂无信息");
                }
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        /// <summary>
        /// 轮播图
        /// </summary>
        /// <returns></returns>
        public ActionResult Advert()
        {
            try
            {
                DataTable dt = AdminAdverts.AdminGetAdvertList(4, 1, 1);
                StringBuilder strb = new StringBuilder();
                strb.Append("[");
                foreach (DataRow rw in dt.Rows)
                {
                    string img = ConfigurationManager.AppSettings["url"] + @"/upload/adv/" + rw["image"];
                    string url = string.Empty;
                    if (rw["url"].ToString() != string.Empty)
                        url = ConfigurationManager.AppSettings["url"] + rw["url"];
                    strb.Append("{\"title\":\"" + rw["atitle"].ToString().TrimEnd() + "\",\"image\":\"" + img + "\",\"url\":\"" + url + "\"},");
                }
                strb = strb.Remove(strb.Length - 1, 1);
                strb.Append("]");
                return APIResult("success", strb.ToString(), true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        #endregion

        #region 房间 回水 赔率
        /// <summary>
        /// 房间信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Room()
        {
            try
            {
                List<MD_LotteryRoom> list = Lottery.GetRoomList(1, -1, "");

                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "LotteryName", "RoomName", "Maxuser", "VipMaxuser", "Backrate", "Enter" }, true);
                string data = JsonConvert.SerializeObject(list, jsetting).ToLower();

                return APIResult("success", data, true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        
        /// <summary>
        /// 回水规则
        /// </summary>
        /// <returns></returns>
        public ActionResult RateRule()
        {
            try
            {
                List<MD_BackRate> list = Lottery.GetRateRuleList(1, -1, "");

                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "Room", "Minloss", "Maxloss", "Backrate" }, true);
                string data = JsonConvert.SerializeObject(list, jsetting).ToLower();

                return APIResult("success", data, true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        /// <summary>
        /// 赔率  分类型显示
        /// </summary>
        /// <returns></returns>
        private  ActionResult LotterySet(string type)
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                if (parmas.Keys.Count != 3)
                {
                    return APIResult("error", "缺少请求参数");
                }

                DataSet list = Lottery.GetLotterySetList("where a.roomtype=" + parmas["type"]);
                DataTable dt = list.Tables[0];
                StringBuilder strb = new StringBuilder();
                strb.Append("{\"first\":");
                strb.Append(JsonConvert.SerializeObject(list.Tables[1]));
                strb.Append(",\"sec\":");
                strb.Append(JsonConvert.SerializeObject(list.Tables[2]));
                strb.Append(",\"three\":");
                strb.Append(JsonConvert.SerializeObject(list.Tables[3]) + "}");
                //JsonSerializerSettings jsetting = new JsonSerializerSettings();
                //jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "lotterytype", "lottery", "settype", "type", "item", "odds","nums" }, true);
                //string data = JsonConvert.SerializeObject(list.Tables[0], jsetting).ToLower();

                return APIResult("success", strb.ToString(), true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }

        /// <summary>
        /// 赔率
        /// </summary>
        /// <returns></returns>
        public ActionResult LotterySet()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                if (parmas.Keys.Count != 3)
                {
                    return APIResult("error", "缺少请求参数");
                }

                DataSet list = Lottery.GetLotterySetList("where a.roomtype=" + parmas["type"]);
                DataTable dt = list.Tables[0];

                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "bttypeid", "item", "odds", "nums" }, true);
                string data = JsonConvert.SerializeObject(dt, jsetting).ToLower();

                return APIResult("success", data, true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        #endregion
    }
}
