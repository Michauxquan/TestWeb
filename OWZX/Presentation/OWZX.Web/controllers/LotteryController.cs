using Newtonsoft.Json;
using OWZX.Core;
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

namespace OWZX.Web.Controllers
{
    /// <summary>
    /// 彩票
    /// </summary>
    public class LotteryController : BaseWebController
    {
        #region 竞猜
        private object lkbtlow = new object();
        private object lkbtmin = new object();
        private object lkbthigh = new object();
        NameValueCollection parmas;
        /// <summary>
        /// 投注 （添加投注记录，扣除用户金额）
        /// </summary>
        /// <returns></returns>
        public ActionResult Bett()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                if (parmas.Keys.Count != 7)
                {
                    return APIResult("error", "缺少请求参数");
                }
                Logs.Write("请求参数："+parmas["account"] + "__" + parmas["expect"] + "__" + parmas["money"] + "__" + parmas["room"].Trim() + "__" + parmas["vip"] 
                    + "__" + int.Parse(parmas["bttypeid"]));
                string room = parmas["room"].Trim();
                string msg = Lottery.ValidateBett(parmas["account"], parmas["expect"], parmas["money"], room, parmas["vip"], int.Parse(parmas["bttypeid"]));
                if (msg != string.Empty)
                {
                    return APIResult("error", msg);
                }

                if (room == "初级")
                {
                    int btmoney = int.Parse(parmas["money"]);
                    //判断投注的最高注数 是否有效
                    if (btmoney < 10)
                    {
                        return APIResult("error", "单笔投注金额不能小于10元宝");
                    }
                    if (btmoney > 20000)
                    {
                        return APIResult("error", "单笔投注金额不能大于20000元宝");
                    }
                    return DealBettLow(parmas);
                }
                else if (room == "中级")
                {
                    int btminmoney = int.Parse(parmas["money"]);
                    if (btminmoney < 50)
                    {
                        return APIResult("error", "单笔投注金额不能小于50元宝");
                    }
                    if (btminmoney > 30000)
                    {
                        return APIResult("error", "单笔投注金额不能大于30000元宝");
                    }
                    return DealBettMid(parmas);
                }
                else if (room == "高级")
                {
                    int bthighmoney = int.Parse(parmas["money"]);
                    if (bthighmoney < 50)
                    {
                        return APIResult("error", "单笔投注金额不能小于50元宝");
                    }
                    if (bthighmoney > 30000)
                    {
                        return APIResult("error", "单笔投注金额不能大于30000元宝");
                    }
                    return DealBettHigh(parmas);
                }

              
                return APIResult("error", "投注失败");
            }
            catch (Exception ex)
            {
                return APIResult("error", "投注失败");
            }
        }

        private ActionResult DealBettLow(NameValueCollection parmas)
        {
            lock (lkbtlow)
            {
                //单注10-20000，总注80000封顶
                //大小单双20000封顶，极值5000封顶，猜数字5000封顶，组合10000封顶，红绿蓝20000封顶，豹子5000封顶

                int typeid = int.Parse(parmas["bttypeid"]);
                int money = int.Parse(parmas["money"]);
                string valres = Lottery.ValidateBetMoney(parmas["expect"], typeid, money, parmas["room"]);
                if (!valres.Contains("验证通过"))
                {
                    return APIResult("error", valres);
                }

                MD_Bett bet = new MD_Bett
                {
                    Account = parmas["account"],
                    Room = parmas["room"],
                    Vip = parmas["vip"],
                    Lotterynum = parmas["expect"],
                    Money = int.Parse(parmas["money"]),
                    Bttypeid = int.Parse(parmas["bttypeid"])
                };

                bool betres = Lottery.AddBett(bet);
                if (betres)
                    return APIResult("success", "投注成功");
                else
                    return APIResult("error", "投注失败");
            }
        }
        private ActionResult DealBettMid(NameValueCollection parmas)
        {
            lock (lkbtmin)
            {
                int typeid = int.Parse(parmas["bttypeid"]);
                int money = int.Parse(parmas["money"]);
                string valres = Lottery.ValidateBetMoney(parmas["expect"], typeid, money, parmas["room"]);
                if (!valres.Contains("验证通过"))
                {
                    return APIResult("error", valres);
                }

                MD_Bett bet = new MD_Bett
                {
                    Account = parmas["account"],
                    Room = parmas["room"],
                    Vip = parmas["vip"],
                    Lotterynum = parmas["expect"],
                    Money = int.Parse(parmas["money"]),
                    Bttypeid = int.Parse(parmas["bttypeid"])
                };

                bool betres = Lottery.AddBett(bet);
                if (betres)
                    return APIResult("success", "投注成功");
                else
                    return APIResult("error", "投注失败");
            }
        }
        private ActionResult DealBettHigh(NameValueCollection parmas)
        {
            lock (lkbthigh)
            {
                int typeid = int.Parse(parmas["bttypeid"]);
                int money = int.Parse(parmas["money"]);
                string valres = Lottery.ValidateBetMoney(parmas["expect"], typeid, money, parmas["room"]);
                if (!valres.Contains("验证通过"))
                {
                    return APIResult("error", valres);
                }

                MD_Bett bet = new MD_Bett
                {
                    Account = parmas["account"],
                    Room = parmas["room"],
                    Vip = parmas["vip"],
                    Lotterynum = parmas["expect"],
                    Money = int.Parse(parmas["money"]),
                    Bttypeid = int.Parse(parmas["bttypeid"])
                };

                bool betres = Lottery.AddBett(bet);
                if (betres)
                    return APIResult("success", "投注成功");
                else
                    return APIResult("error", "投注失败");
            }
        }
        /// <summary>
        /// 投注记录
        /// </summary>
        /// <returns></returns>
        public ActionResult BettRecord()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;

                string account = parmas["account"];
                int page = int.Parse(parmas["page"]);
                StringBuilder strb = new StringBuilder();
                string type = parmas["type"];

                string start = string.Empty;
                string end = string.Empty;
                if (parmas.AllKeys.Contains("start") && parmas.AllKeys.Contains("end"))
                {
                    start = parmas["start"];
                    end = parmas["end"];
                }
                strb.Append(" where 1=1");
                if (type != string.Empty && type != "0")
                    strb.Append("  and c.type=" + type);
                if (start != string.Empty && end != string.Empty)
                    strb.Append("  and convert(varchar(10),a.addtime,120) between '" + start + "' and '" + end + "'");
                
                DataTable list = NewUser.GetUserBettList(page, 15, account, strb.ToString());
                if (list.Rows.Count == 0)
                {
                    return APIResult("error", "暂无投注记录");
                }

                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                //jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "Expect", "Result" }, true);
                string data = JsonConvert.SerializeObject(list, jsetting).ToLower();
                return APIResult("success", data, true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        /// <summary>
        /// 最新10期开奖结果
        /// </summary>
        /// <returns></returns>
        public ActionResult LastBett()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;

                string type = parmas["type"];

                List<MD_Lottery> list = Lottery.LastLotteryList(int.Parse(type));
                if (list.Count == 0)
                {
                    return APIResult("error", "暂无开奖记录");
                }
                list.ForEach((x) =>
                {
                    if (x.Status == 1 && x.Result == null)
                    {
                        x.Expect = "第" + x.Expect + "期";
                        x.Result ="?+?+?=? (类型)";
                    }
                    else
                    {
                        x.Expect = "第" + x.Expect + "期";
                        string res = "(";
                        if (int.Parse(x.Resultnum) <= 13)
                        {
                            res += "小";
                        }
                        else
                        {
                            res += "大";
                        }

                        if (int.Parse(x.Resultnum) % 2 == 0)
                        {
                            res += ",双)";
                        }
                        else
                        {
                            res += ",单)";
                        }
                        x.Result += res;
                    }
                });

                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "Expect", "Result" }, true);
                string data = JsonConvert.SerializeObject(list, jsetting).ToLower();
                return APIResult("success", data, true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }

        /// <summary>
        /// 走势图
        /// </summary>
        /// <returns></returns>
        public ActionResult Trend()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;

                string type = parmas["type"];
                int page = int.Parse(parmas["page"]);
                DataTable list = Lottery.LotteryTrend(page, 15, type);
                if (list.Rows.Count == 0)
                {
                    return APIResult("error", "暂无开奖记录");
                }

                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                //jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "Expect", "Result" }, true);
                string data = JsonConvert.SerializeObject(list, jsetting).ToLower();
                return APIResult("success", data, true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }

        /// <summary>
        /// 最新竞猜信息
        /// </summary>
        /// <returns></returns>
        public ActionResult LastLottery()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;

                string type = parmas["type"];
                string resjson = string.Empty;


                if (type == "10")
                {
                    //游戏是否维护中
                    BaseInfo baseinfo = BSPConfig.BaseConfig.BaseList.Find(x => x.Key == "北京28");
                    if (baseinfo.Account.Trim() == "是")
                    {
                        resjson = "{\"expect\":\"?\",\"time\":\"维护中\"}";
                    }
                    else
                    {
                        TimeSpan startTime = DateTime.Parse("09:00").TimeOfDay;
                        TimeSpan endTime = DateTime.Parse("23:55").TimeOfDay;
                        TimeSpan tmNow = DateTime.Now.TimeOfDay;

                        if (tmNow <= startTime || tmNow >= endTime)
                        {
                            //禁止投注时间
                            resjson = "{\"expect\":\"?\",\"time\":\"已停售\"}";
                        }
                    }

                }
                else if (type == "11")
                {
                    BaseInfo baseinfo = BSPConfig.BaseConfig.BaseList.Find(x => x.Key == "加拿大28");
                    if (baseinfo.Account.Trim() == "是")
                    {
                        resjson = "{\"expect\":\"?\",\"time\":\"维护中\"}";
                    }
                    else
                    {
                        TimeSpan startTime = DateTime.Parse("20:00").TimeOfDay;
                        TimeSpan endTime = DateTime.Parse("21:00").TimeOfDay;
                        TimeSpan tmNow = DateTime.Now.TimeOfDay;

                        if (tmNow >= startTime && tmNow <= endTime)
                        {
                            //禁止投注时间
                            resjson = "{\"expect\":\"?\",\"time\":\"已停售\"}";
                        }
                    }
                }

                if (resjson == string.Empty)
                {
                    DataTable list = Lottery.LastLottery(type);
                    if (list.Rows.Count == 0)
                    {
                        return APIResult("error", "暂无竞猜信息");
                    }

                    JsonSerializerSettings jsetting = new JsonSerializerSettings();
                    jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "expect", "time" }, true);
                    resjson = JsonConvert.SerializeObject(list, jsetting).ToLower();

                }

                return APIResult("success", resjson.Replace("[", "").Replace("]", ""), true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        /// <summary>
        /// 赔率说明
        /// </summary>
        /// <returns></returns>
        public ActionResult SetRemark()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;

                string type = parmas["type"];

                DataTable list = Lottery.SetRemark(type);
                if (list.Rows.Count == 0)
                {
                    return APIResult("error", "获取失败");
                }

                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                string resjson = JsonConvert.SerializeObject(list, jsetting).ToLower();

                return APIResult("success", resjson, true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        /// <summary>
        /// 关于
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            try
            {

                BaseInfo info = BSPConfig.BaseConfig.BaseList.Find(x => x.Key == "关于配置");

                StringBuilder strb = new StringBuilder();
                strb.Append("{");
                string Image = BSPConfig.ShopConfig.SiteUrl + "/upload/imgs/" + info.Image;
                strb.Append("\"version\":\"" + info.BankAddress + "\",\"url\":\"" + info.Name + "\",\"qq\":\"" + info.Account + "\",\"wechat\":\"" + info.Bank + "\",\"img\":\"" + Image + "\"");
                strb.Append("}");
                return APIResult("success", strb.ToString(), true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        #endregion

        #region 聊天室
        string root = ConfigurationManager.AppSettings["hxurl"];

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        public ActionResult Token()
        {
            MD_AccessTokenResult token = Lottery.GetAccessToken();
            return Content(token.SuccessResult.access_token);
        }
        /// <summary>
        /// 创建聊天室
        /// </summary>
        /// <returns></returns>
        public ActionResult ChatRoom()
        {
            string hxurl = root + "/chatrooms";
            MD_AccessTokenResult token = Lottery.GetAccessToken();
            string[] chat = new string[]{"bj-fir-vip1","bj-fir-vip2","bj-fir-vip3","bj-fir-vip4",
                                         "bj-sec-vip1","bj-sec-vip2","bj-sec-vip3","bj-sec-vip4",
                                         "bj-thr-vip1","bj-thr-vip2","bj-thr-vip3","bj-thr-vip4",
                                         "cakeno-fir-vip1","cakeno-fir-vip2","cakeno-fir-vip3","cakeno-fir-vip4",
                                         "cakeno-sec-vip1","cakeno-sec-vip2","cakeno-sec-vip3","cakeno-sec-vip4",
                                         "cakeno-thr-vip1","cakeno-thr-vip2","cakeno-thr-vip3","cakeno-thr-vip4"};

            StringBuilder strb = new StringBuilder();
            foreach (string str in chat)
            {
                strb.Append("{\"name\": \"" + str + "\",\"description\": \"" + str + "\",\"maxusers\": 500,\"owner\": \"8001\"}");
                string result = WebHelper.GetHXRequestData(hxurl, "post", token.SuccessResult.access_token, true, strb.ToString());
                if (result.Contains("error"))
                {
                    return APIResult("error", "聊天室创建失败，返回信息 ：" + result);
                }
                strb = new StringBuilder();
            }
            //获取聊天室信息
            hxurl += "?pagenum=1&pagesize=24";
            string chats = WebHelper.GetHXRequestData(hxurl, "get", token.SuccessResult.access_token, true, "");

            if (!chats.Contains("error"))
                return APIResult("success", "创建成功", true);

            return APIResult("success", "创建失败", true);
        }

        /// <summary>
        /// 获取聊天室
        /// </summary>
        /// <returns></returns>
        public ActionResult GetChatRoom()
        {
            try
            {
                string hxurl = root + "/chatrooms";
                MD_AccessTokenResult token = Lottery.GetAccessToken();

                //获取聊天室信息
                hxurl += "?pagenum=1&pagesize=24";
                string chats = WebHelper.GetHXRequestData(hxurl, "get", token.SuccessResult.access_token, true, "");
                MD_HXRoomData room = JsonConvert.DeserializeObject<MD_HXRoomData>(chats);
                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.ContractResolver = new JsonLimitOutPut(new string[] { "id", "name", "owner", "affiliations_count" }, true);

                string data = JsonConvert.SerializeObject(room.data.OrderBy(x => x.id),jsetting);
                return APIResult("success", data, true);
            }
            catch (Exception ex)
            {
                return APIResult("success", "获取失败", true);
            }
        }
        /// <summary>
        /// 房间在线人数
        /// </summary>
        /// <returns></returns>
        public ActionResult RoomOnline()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;

                string hxurl = root + "/chatrooms";
                MD_AccessTokenResult token = Lottery.GetAccessToken();

                string type = parmas["type"];
                if (type == "10")
                {
                    type = "bj";
                }
                else if (type == "11")
                {
                    type = "cakeno";
                }

                //获取聊天室信息
                hxurl += "?pagenum=1&pagesize=24";
                string chats = WebHelper.GetHXRequestData(hxurl, "get", token.SuccessResult.access_token, true, "");
                MD_HXRoomData room = JsonConvert.DeserializeObject<MD_HXRoomData>(chats);
                List<MD_RoomData> dtlist = room.data.FindAll(x => x.name.Contains(type));

                string[] rooms = new string[] { "fir", "sec", "thr" };
                StringBuilder strb = new StringBuilder();

                int rmtotal = 0;
                strb.Append("[");
                foreach (string rmstr in rooms)
                {
                    strb.Append("{");
                    List<MD_RoomData> items = dtlist.FindAll(x => x.name.Contains(rmstr));
                    for (int i = 1; i < 5; i++)
                    {
                        MD_RoomData rmdt = items.OrderBy(x => x.id).First(x => x.name.Contains("vip" + i.ToString()));
                        strb.Append("\"vip" + i.ToString() + "\":" + rmdt.affiliations_count + ",");
                        rmtotal += rmdt.affiliations_count;
                    }
                    if (strb.Length > 1)
                        strb = strb.Remove(strb.Length - 1, 1);
                    strb.Append(",\"rmtotal\":" + rmtotal.ToString() + "},");


                    rmtotal = 0;
                }
                if (strb.Length > 1)
                    strb = strb.Remove(strb.Length - 1, 1).Append("]");

                //JsonSerializerSettings jsetting = new JsonSerializerSettings();
                //string data = JsonConvert.SerializeObject(strb.ToString()).ToLower();

                return APIResult("success", strb.ToString(), true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "获取失败");
            }
        }
        /// <summary>
        /// 删除聊天室(MVC 不支持Delete)
        /// </summary>
        /// <returns></returns>
        public ActionResult DelChatRoom()
        {
            try
            {

                string hxurl = root + "/chatrooms";
                MD_AccessTokenResult token = Lottery.GetAccessToken();

                //获取聊天室信息
                hxurl += "?pagenum=1&pagesize=24";
                string chats = WebHelper.GetHXRequestData(hxurl, "get", token.SuccessResult.access_token, true, "");
                MD_HXRoomData room = JsonConvert.DeserializeObject<MD_HXRoomData>(chats);

                foreach (MD_RoomData rd in room.data)
                {
                    hxurl = root + "/chatrooms/" + rd.id.ToString();
                    chats = WebHelper.GetHXRequestData(hxurl, "delete", token.SuccessResult.access_token, true, "");
                    room = JsonConvert.DeserializeObject<MD_HXRoomData>(chats);
                }
                return APIResult("success", "删除成功", true);
            }
            catch (Exception ex)
            {
                return APIResult("error", "删除失败", true);
            }
        }
        /// <summary>
        /// 删除聊天室成员
        /// </summary>
        /// <returns></returns>
        public ActionResult DelChatRoomUser()
        {
            try
            {
                NameValueCollection parmas = WorkContext.postparms;
                string hxurl = root + "/chatrooms/" + parmas["chatroomid"] + "/users/" + parmas["mobile"];
                MD_AccessTokenResult token = Lottery.GetAccessToken();

                string chats = WebHelper.GetHXRequestData(hxurl, "delete", token.SuccessResult.access_token, true, "");
                if (chats.Contains("error"))
                {
                    ErrorMsg errm = JsonConvert.DeserializeObject<ErrorMsg>(chats);
                    return APIResult("error", errm.error_description);
                }
                else
                {
                    MD_HXRoomData room = JsonConvert.DeserializeObject<MD_HXRoomData>(chats);

                    if (room.data[0].result)
                        return APIResult("success", "删除成功");
                }

                return APIResult("error", "删除失败");
            }
            catch (Exception ex)
            {
                return APIResult("error", "删除失败");
            }
        }
       
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <returns></returns>
        public ActionResult SendMsg()
        {

            try
            {
                string hxurl = root + "/messages";
                MD_AccessTokenResult token = Lottery.GetAccessToken();

                //获取聊天室信息
                string ptdata = "{\"target_type\":\"chatrooms\",\"target\":[\"275831248121758236\"], \"msg\":{\"type\":\"txt\",\"msg\":\"hello from rest\"},\"from\":\"8001\"}";
                string chats = WebHelper.GetHXRequestData(hxurl, "post", token.SuccessResult.access_token, true, ptdata);
                MD_HXRoomData room = JsonConvert.DeserializeObject<MD_HXRoomData>(chats);
                string data = JsonConvert.SerializeObject(room.data.OrderBy(x => x.id));
                return APIResult("success", data, true);
            }
            catch (Exception ex)
            {
                return APIResult("success", "获取失败", true);
            }
        }
        #endregion

    }
}
