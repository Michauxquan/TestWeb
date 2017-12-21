using Newtonsoft.Json;
using OWZX.Core;
using OWZX.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace OWZX.Services
{
    public class Lottery
    {
        private static object lkbj = new object();
        private static object lkcan = new object();
        private static object lkinit = new object();
        private static object lkconfig = new object();
        Configuration cfa = WebConfigurationManager.OpenWebConfiguration("~");

        #region 处理彩票开奖

        /// <summary>
        /// 添加初始记录
        /// </summary>
        /// <returns></returns>
        public static void InitLottery()
        {

            string root = ConfigurationManager.AppSettings["lk28"];
            lock (lkinit)
            {
                string url = root + "?token=7904a63f2fc9c2d2&code=bjkl8&rows=5&format=json&date=" + DateTime.Now.ToString("yyyy-MM-dd");
                string bjjson = HttpUtils.HttpGet(url, "");
                MD_28Json bj28json = JsonConvert.DeserializeObject<MD_28Json>(bjjson);
                if (bj28json.data.Count > 0)
                {
                    LK28Item item = bj28json.data[0];
                    //已经开奖的记录是否存在，存在则不添加，不存在则添加
                    bool extbj = Lottery.ExistsBJ28(" and a.expect='" + (int.Parse(item.expect + 1)).ToString() + "'");
                    if (!extbj)
                    {
                        MD_Lottery lottery = new MD_Lottery
                        {
                            Type = 10,
                            Expect = (int.Parse(item.expect) + 1).ToString(),
                            Opentime = DateTime.Parse(DateTime.Parse(item.opentime).ToString("yyyy-MM-dd HH:mm") + ":00").AddMinutes(5),
                            Status = 0
                        };
                        bool result = Lottery.AddBJRecord(lottery);
                    }
                }
            }
        }
        /// <summary>
        /// 添加初始记录
        /// </summary>
        /// <returns></returns>
        public static void InitCanadaLottery()
        {
            string root = ConfigurationManager.AppSettings["lk28"];
            lock (lkinit)
            {
                string url = root + "?token=7904a63f2fc9c2d2&code=cakeno&rows=5&format=json&date=" + DateTime.Now.ToString("yyyy-MM-dd");
                string bjjson = HttpUtils.HttpGet(url, "");
                MD_28Json bj28json = JsonConvert.DeserializeObject<MD_28Json>(bjjson);
                if (bj28json.data.Count > 0)
                {
                    LK28Item item = bj28json.data[0];
                    //已经开奖的记录是否存在，存在则不添加，不存在则添加
                    bool extbj = Lottery.ExistsCanada28(" and a.expect='" + (int.Parse(item.expect + 1)).ToString() + "'");
                    if (!extbj)
                    {
                        MD_Lottery lottery = new MD_Lottery
                        {
                            Type = 11,
                            Expect = (int.Parse(item.expect) + 1).ToString(),
                            Opentime = DateTime.Parse(item.opentime).AddMinutes(3).AddSeconds(30),
                            Status = 0
                        };
                        bool result = Lottery.AddCanadaRecord(lottery);
                    }

                }
            }
        }

        /// <summary>
        /// 根据北京快乐8结果，计算28结果
        /// </summary>
        /// <returns></returns>
        public static string AddLottery()
        {
            lock (lkbj)
            {
                //是否有投注或封盘记录
                bool listtoady = Lottery.ExistsBJ28(" and a.status in (0,1) and convert(varchar(10),a.opentime,120)='" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
                if (!listtoady)
                {
                    return "";
                }
                string root = ConfigurationManager.AppSettings["lk28"];
                string url = root + "?token=7904a63f2fc9c2d2&code=bjkl8&rows=5&format=json&date=" + DateTime.Now.ToString("yyyy-MM-dd");
                string bjjson = HttpUtils.HttpGet(url, "");
                MD_28Json bj28json = JsonConvert.DeserializeObject<MD_28Json>(bjjson);
                if (bj28json.data.Count > 0)
                {
                    LK28Item item = bj28json.data[0];
                    //10 北京28  11 加拿大28

                    //更新开奖信息
                    item.opencode = item.opencode.Substring(0, item.opencode.Length - 3);
                    MD_Lottery lottery = new MD_Lottery
                    {
                        Type = 10,
                        Expect = item.expect,
                        Opencode = item.opencode,
                        Opentime = DateTime.Parse(item.opentime),
                        Status = 2
                    };
                    //获取的开奖记录 是否是当前正在开奖的记录
                    bool listltty = Lottery.ExistsBJ28(" and a.expect='" + item.expect + "' and a.status<>2");
                    if (listltty)
                    {
                        GetBJResult(lottery, item);
                        string result = OWZX.Data.Lottery.UpdateLottery(lottery);
                        if (result.EndsWith("成功"))
                        {
                            return lottery.Expect;
                        }
                        else
                            Logs.Write("北京28更新结果异常：" + result);
                        return "";
                    }

                }
            }
            return "";
        }

        /// <summary>
        /// 根据加拿大卑斯快乐8结果，计算28结果
        /// </summary>
        /// <returns></returns>
        public static string AddCanadaLottery()
        {
            lock (lkcan)
            {
                bool listtoady = Lottery.ExistsCanada28(" and a.status in (0,1)");
                if (!listtoady)
                {
                    return "";
                }
                string root = ConfigurationManager.AppSettings["lk28"];
                string url = root + "?token=7904a63f2fc9c2d2&code=cakeno&rows=5&format=json&date=" + DateTime.Now.ToString("yyyy-MM-dd");
                string bjjson = HttpUtils.HttpGet(url, "");
                MD_28Json bj28json = JsonConvert.DeserializeObject<MD_28Json>(bjjson);
                if (bj28json.data.Count > 0)
                {
                    LK28Item item = bj28json.data[0];
                    //10 北京28  11 加拿大28

                    //更新开奖信息
                    MD_Lottery lottery = new MD_Lottery
                    {
                        Type = 11,
                        Expect = item.expect,
                        Opencode = item.opencode,
                        Opentime = DateTime.Parse(item.opentime),
                        Status = 2
                    };
                    //获取的开奖记录 是否是当前正在开奖的记录
                    bool listltty = Lottery.ExistsCanada28(" and a.expect='" + item.expect + "' and a.status<>2");
                    if (listltty)
                    {
                        GetCanaResult(lottery, item);

                        string result = OWZX.Data.Lottery.UpdateLottery(lottery);
                        if (result.EndsWith("成功"))
                        {
                            return lottery.Expect;
                        }
                        else
                            Logs.Write("加拿大28更新结果异常：" + result);
                        return "";
                    }
                }
            }
            return "";
        }
        /// <summary>
        /// 计算北京28玩法 结果
        /// </summary>
        /// <param name="lottery"></param>
        /// <param name="item"></param>
        private static void GetBJResult(MD_Lottery lottery, LK28Item item)
        {
            string[] lucky = item.opencode.Split(',');
            Array.Sort(lucky);
            lottery.Orderresult = String.Join(",", lucky);
            int sumRes = 0;
            for (int i = 0; i < lucky.Length; i++)
            {
                sumRes += int.Parse(lucky[i]);
                switch (i)
                {
                    case 5:
                        lottery.First = sumRes.ToString().Substring(sumRes.ToString().Length - 1, 1);
                        sumRes = 0;
                        break;
                    case 11:
                        lottery.Second = sumRes.ToString().Substring(sumRes.ToString().Length - 1, 1);
                        sumRes = 0;
                        break;
                    case 17:
                        lottery.Three = sumRes.ToString().Substring(sumRes.ToString().Length - 1, 1);
                        sumRes = 0;
                        break;
                }

            }
            lottery.Resultnum = (int.Parse(lottery.First) + int.Parse(lottery.Second) + int.Parse(lottery.Three)).ToString();
            lottery.Result = lottery.First + "+" + lottery.Second + "+" + lottery.Three + "=" + lottery.Resultnum;
            string restype = string.Empty;
            int resnum = int.Parse(lottery.Resultnum);
            if (resnum >= 14 && resnum <= 27)
                restype += "大,";
            else if (resnum >= 0 && resnum <= 13)
                restype += "小,";

            if (resnum % 2 == 0)
                restype += "双";
            else
                restype += "单";

            lottery.ResultType = restype;
        }

        /// <summary>
        /// 计算加拿大28玩法 结果
        /// </summary>
        /// <param name="lottery"></param>
        /// <param name="item"></param>
        private static void GetCanaResult(MD_Lottery lottery, LK28Item item)
        {
            string[] lucky = item.opencode.Split(',');
            Array.Sort(lucky);
            lottery.Orderresult = String.Join(",", lucky);
            int first = 0; int second = 0; int three = 0;
            for (int i = 0; i < lucky.Length; i++)
            {
                switch (i)
                {
                    case 1:
                    case 4:
                    case 7:
                    case 10:
                    case 13:
                    case 16:
                        first += int.Parse(lucky[i]);
                        break;
                    case 2:
                    case 5:
                    case 8:
                    case 11:
                    case 14:
                    case 17:
                        second += int.Parse(lucky[i]);
                        break;
                    case 3:
                    case 6:
                    case 9:
                    case 12:
                    case 15:
                    case 18:
                        three += int.Parse(lucky[i]);
                        break;
                }

            }

            lottery.First = first.ToString().Substring(first.ToString().Length - 1, 1);

            lottery.Second = second.ToString().Substring(second.ToString().Length - 1, 1);

            lottery.Three = three.ToString().Substring(three.ToString().Length - 1, 1);

            lottery.Resultnum = (int.Parse(lottery.First) + int.Parse(lottery.Second) + int.Parse(lottery.Three)).ToString();
            lottery.Result = lottery.First + "+" + lottery.Second + "+" + lottery.Three + "=" + lottery.Resultnum;
            string restype = string.Empty;
            int resnum = int.Parse(lottery.Resultnum);
            if (resnum >= 14 && resnum <= 27)
                restype += "大,";
            else if (resnum >= 0 && resnum <= 13)
                restype += "小,";

            if (resnum % 2 == 0)
                restype += "双";
            else
                restype += "单";

            lottery.ResultType = restype;
        }

        /// <summary>
        /// 根据北京快乐8结果，计算28结果
        /// </summary>
        /// <returns></returns>
        public static void TrapBJ()
        {
            lock (lkbj)
            {
                string root = ConfigurationManager.AppSettings["lk28"];
                string url = root + "?token=7904a63f2fc9c2d2&code=bjkl8&rows=15&format=json&date=" + DateTime.Now.ToString("yyyy-MM-dd");
                string bjjson = HttpUtils.HttpGet(url, "");
                MD_28Json bj28json = JsonConvert.DeserializeObject<MD_28Json>(bjjson);
                if (bj28json.data.Count > 0)
                {
                    string allnums = string.Empty;
                    bj28json.data.ForEach((x) =>
                    {
                        allnums += "'" + x.expect + "',";
                    });
                    allnums = allnums.Remove(allnums.Length - 1, 1);


                    //开奖时间超过一分钟未开奖，进行补漏
                    //10 北京28  11 加拿大28
                    List<MD_Lottery> listlott = Lottery.GetLotteryList(1, -1, " where a.type=10 and a.expect in (" + allnums + ") and a.status in (0,1) and DATEDIFF(minute,a.opentime,getdate())>1");
                    if (listlott.Count > 0)
                    {
                        foreach (MD_Lottery lot in listlott)
                        {
                            LK28Item mdlt = bj28json.data.Find(x => x.expect == lot.Expect);

                            //更新开奖信息
                            mdlt.opencode = mdlt.opencode.Substring(0, mdlt.opencode.Length - 3);
                            MD_Lottery lottery = new MD_Lottery
                            {
                                Type = 10,
                                Expect = mdlt.expect,
                                Opencode = mdlt.opencode,
                                Opentime = DateTime.Parse(mdlt.opentime),
                                Status = 2
                            };

                            GetBJResult(lottery, mdlt);

                            string result = OWZX.Data.Lottery.UpdateLottery(lottery);
                            if (result.EndsWith("成功"))
                            {
                                if (mdlt.expect != string.Empty)
                                {
                                    MD_WaitPayBonus pay = new MD_WaitPayBonus { Expect = mdlt.expect, Isread = false };
                                    Lottery.AddWaitPay(pay);
                                }
                            }
                            else
                                Logs.Write("北京28Trap更新结果异常：" + result);

                        }
                    }
                    else
                    {
                        //没有数据
                    }
                }
            }
        }

        /// <summary>
        /// 根据加拿大卑斯快乐8结果，计算28结果
        /// </summary>
        /// <returns></returns>
        public static void TrapCanada()
        {
            lock (lkcan)
            {
                string root = ConfigurationManager.AppSettings["lk28"];
                string url = root + "?token=7904a63f2fc9c2d2&code=cakeno&rows=15&format=json&date=" + DateTime.Now.ToString("yyyy-MM-dd");
                string bjjson = HttpUtils.HttpGet(url, "");
                MD_28Json cakeno28json = JsonConvert.DeserializeObject<MD_28Json>(bjjson);
                if (cakeno28json.data.Count > 0)
                {
                    string allnums = string.Empty;
                    cakeno28json.data.ForEach((x) =>
                    {
                        allnums += "'" + x.expect + "',";
                    });
                    allnums = allnums.Remove(allnums.Length - 1, 1);
                    //10 北京28  11 加拿大28
                    List<MD_Lottery> listlott = Lottery.GetLotteryList(1, -1, " where a.type=11 and a.expect in (" + allnums + ") and a.status in (0,1) and DATEDIFF(minute,a.opentime,getdate())>1");
                    if (listlott.Count > 0)
                    {
                        foreach (MD_Lottery lot in listlott)
                        {
                            LK28Item mdlt = cakeno28json.data.Find(x => x.expect == lot.Expect);

                            //更新开奖信息
                            MD_Lottery lottery = new MD_Lottery
                            {
                                Type = 11,
                                Expect = mdlt.expect,
                                Opencode = mdlt.opencode,
                                Opentime = DateTime.Parse(mdlt.opentime),
                                Status = 2
                            };

                            GetCanaResult(lottery, mdlt);

                            string result = OWZX.Data.Lottery.UpdateLottery(lottery);
                            if (result.EndsWith("成功"))
                            {
                                if (mdlt.expect != string.Empty)
                                {
                                    MD_WaitPayBonus pay = new MD_WaitPayBonus { Expect = mdlt.expect, Isread = false };
                                    Lottery.AddWaitPay(pay);
                                }
                            }
                            else
                                Logs.Write("加拿大28Trap更新结果异常：" + result);
                        }
                    }
                }
            }
        }

        #endregion

        #region 聊天室
        /// <summary>
        /// 获取每次操作微信API的Token访问令牌
        /// </summary>
        /// <returns></returns>
        public static MD_AccessTokenResult GetAccessToken()
        {
            string url = ConfigurationManager.AppSettings["hxurl"] + "/token";
            //正常情况下access_token有效期为7200秒,这里使用缓存设置短于这个时间即可
            MD_AccessTokenResult access_token = MemoryCacheHelper.GetCacheItem<MD_AccessTokenResult>("hxaccess_token", delegate()
            {
                MD_AccessTokenResult result = new MD_AccessTokenResult();
                string data = "{\"grant_type\": \"client_credentials\",\"client_id\":\"YXA6zGdmQMJmEeanBPMFN36Rqg\",\"client_secret\":\"YXA6xRVkFVyFkYNNMJQxzRPbZTwAusw\"}";
                string jsonStr = WebHelper.GetHXRequestData(url, "post", "", false, data);
                if (jsonStr.Contains("error"))
                {
                    ErrorMsg errorResult = new ErrorMsg();
                    errorResult = JsonConvert.DeserializeObject<ErrorMsg>(jsonStr);
                    result.ErrorResult = errorResult;
                    result.Result = false;
                }
                else
                {
                    AccessTokenModel model = new AccessTokenModel();
                    model = JsonConvert.DeserializeObject<AccessTokenModel>(jsonStr);
                    result.SuccessResult = model;
                    result.Result = true;
                }
                return result;
            },
                new TimeSpan(0, 0, 5100000)//过期
            );

            return access_token;
        }
        #endregion

        #region 彩票
        /// <summary>
        /// 获取最新彩票记录
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DataTable LastLottery(string type)
        {
            return OWZX.Data.Lottery.LastLottery(type);
        }

        /// <summary>
        /// 添加新的竞猜记录
        /// </summary>
        /// <param name="lottery"></param>
        /// <returns></returns>
        public static bool AddBJRecord(MD_Lottery lottery)
        {
            return OWZX.Data.Lottery.AddBJRecord(lottery);
        }

        /// <summary>
        /// 添加新的竞猜记录
        /// </summary>
        /// <param name="lottery"></param>
        /// <returns></returns>
        public static bool AddCanadaRecord(MD_Lottery lottery)
        {
            return OWZX.Data.Lottery.AddCanadaRecord(lottery);
        }
        /// <summary>
        /// 添加彩票记录 (记录彩票开奖时间和期号)
        /// </summary>
        /// <param name="lot"></param>
        /// <returns></returns>
        public static bool AddLottery(MD_Lottery lot)
        {
            string result = OWZX.Data.Lottery.AddLottery(lot);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加彩票记录 (上期结束，添加新的记录)
        /// </summary>
        /// <param name="type">北京10 加拿大11</param>
        /// <param name="starttime">彩票开始时间</param>
        /// <param name="endtime">彩票截止时间</param>
        /// <returns></returns>
        public static bool AddLottery(int type, string starttime, string endtime)
        {
            string result = OWZX.Data.Lottery.AddLottery(type, starttime, endtime);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新彩票记录 (更新开奖信息)
        /// </summary>
        /// <param name="lot"></param>
        /// <returns></returns>
        public static bool UpdateLottery(MD_Lottery lot)
        {
            string result = OWZX.Data.Lottery.UpdateLottery(lot);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新竞猜记录为 封盘状态
        /// </summary>
        /// <returns></returns>
        public static bool UpdateLotteryStatus()
        {
            string result = OWZX.Data.Lottery.UpdateLotteryStatus();
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除彩票记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteLottery(string id)
        {
            string result = OWZX.Data.Lottery.DeleteLottery(id);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static List<MD_Lottery> GetLotteryList(int pageIndex, int pageSize, string condition = "", string orderby = "desc")
        {
            DataTable dt = OWZX.Data.Lottery.GetLotteryList(pageIndex, pageSize, condition,orderby);
            List<MD_Lottery> list = (List<MD_Lottery>)ModelConvertHelper<MD_Lottery>.ConvertToModel(dt);
            return list;
        }

        /// <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="type">彩票类型</param>
        /// <returns></returns>
        public static List<MD_Lottery> LastLotteryList(int type)
        {
            DataTable dt = OWZX.Data.Lottery.LastLotteryList(type);
            List<MD_Lottery> list = (List<MD_Lottery>)ModelConvertHelper<MD_Lottery>.ConvertToModel(dt);
            return list;
        }
        /// <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">有where 条件需要and</param>
        /// <returns></returns>
        public static List<MD_Lottery> GetBJ28LotteryList(int pageIndex, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.Lottery.GetBJ28LotteryList(pageIndex, pageSize, condition);
            List<MD_Lottery> list = (List<MD_Lottery>)ModelConvertHelper<MD_Lottery>.ConvertToModel(dt);
            return list;
        }


        /// <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">有where 条件需要and</param>
        /// <returns></returns>
        public static List<MD_Lottery> GetCanada28LotteryList(int pageIndex, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.Lottery.GetCanada28LotteryList(pageIndex, pageSize, condition);
            List<MD_Lottery> list = (List<MD_Lottery>)ModelConvertHelper<MD_Lottery>.ConvertToModel(dt);
            return list;
        }

        /// <summary>
        /// 获取彩票走势图
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="type">彩票类型id</param>
        /// <returns></returns>
        public static DataTable LotteryTrend(int pageNumber, int pageSize, string type)
        {
            return OWZX.Data.Lottery.LotteryTrend(pageNumber, pageSize, type);
        }
        /// <summary>
        ///是否存在北京28彩票记录
        /// </summary>
        /// <param name="condition">有where 条件需要and</param>
        /// <returns></returns>
        public static bool ExistsBJ28(string condition = "")
        {
            return OWZX.Data.Lottery.ExistsBJ28(condition);
        }


        /// <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="condition">有where 条件需要and</param>
        /// <returns></returns>
        public static bool ExistsCanada28(string condition = "")
        {
            return OWZX.Data.Lottery.ExistsCanada28(condition);
        }
        /// <summary>
        /// bj28最新竞猜记录是否已经超时
        /// </summary>
        /// <returns></returns>
        public static bool ExistsBjTimeOut()
        {
            return OWZX.Data.Lottery.ExistsBjTimeOut();
        }
        /// <summary>
        /// canada28最新竞猜记录是否已经超时
        /// </summary>
        /// <returns></returns>
        public static bool ExistsCanTimeOut()
        {
            return OWZX.Data.Lottery.ExistsCanTimeOut();
        }

        /// <summary>
        /// 验证投注信息是否正确
        /// </summary>
        /// <param name="account"></param>
        /// <param name="expect"></param>
        /// <param name="money"></param>
        /// <param name="room"></param>
        /// <param name="vip"></param>
        /// <param name="bttypeid"></param>
        /// <returns></returns>
        public static string ValidateBett(string account, string expect, string money, string room, string vip, int bttypeid)
        {
            return OWZX.Data.Lottery.ValidateBett(account, expect, money, room, vip, bttypeid);
        }
        #endregion

        #region 投注
        /// <summary>
        /// 添加投注记录
        /// </summary>
        /// <param name="bett"></param>
        /// <returns></returns>
        public static string AddNewBett(MD_Bett bett)
        {
            string result = OWZX.Data.Lottery.AddNewBett(bett);
            return result;
        }
        /// <summary>
        /// 添加投注记录
        /// </summary>
        /// <param name="bett"></param>
        /// <returns></returns>
        public static bool AddBett(MD_Bett bett)
        {
            string result = OWZX.Data.Lottery.AddBett(bett);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新投注记录 
        /// </summary>
        /// <param name="draw"></param>
        /// <returns></returns>
        public static bool UpdateBett(MD_Bett draw)
        {
            string result = OWZX.Data.Lottery.UpdateBett(draw);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除投注记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteBett(string id)
        {
            string result = OWZX.Data.Lottery.DeleteBett(id);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///  获取投注记录(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static List<MD_Bett> GetBettList(int pageIndex, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.Lottery.GetBettList(pageIndex, pageSize, condition);
            List<MD_Bett> list = (List<MD_Bett>)ModelConvertHelper<MD_Bett>.ConvertToModel(dt);
            return list;
        }
        /// <summary>
        /// 获取当前竞猜的投注总注数
        /// </summary>
        /// <param name="expect"></param>
        /// <returns></returns>
        public static DataTable GetBettTotal(string expect)
        {
            return OWZX.Data.Lottery.GetBettTotal(expect);
        }

        /// <summary>
        /// 验证投注操作是否异常
        /// </summary>
        /// <param name="expect">投注期号</param>
        /// <param name="bttypeid">投注类型id</param>
        /// <param name="money">投注金额</param>
        /// <param name="type">房间类型</param>
        /// <returns></returns>
        public static string ValidateBetMoney(string expect, int bttypeid, int money, string type)
        {
            return OWZX.Data.Lottery.ValidateBetMoney(expect, bttypeid, money, type);
        }
        #endregion



        #region 彩票投注赔率
        /// <summary>
        /// 赔率说明
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DataTable SetRemark(string type)
        {
            return OWZX.Data.Lottery.SetRemark(type);
        }
        /// <summary>
        /// 添加投注赔率
        /// </summary>
        /// <param name="lotset"></param>
        /// <returns></returns>
        public static bool AddLotterySet(MD_LotterySet lotset)
        {
            string result = OWZX.Data.Lottery.AddLotterySet(lotset);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新投注赔率
        /// </summary>
        /// <param name="lotset"></param>
        /// <returns></returns>
        public static bool UpdateLotterySet(MD_LotterySet lotset)
        {
            string result = OWZX.Data.Lottery.UpdateLotterySet(lotset);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除投注赔率
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteLotterySet(string id)
        {
            string result = OWZX.Data.Lottery.DeleteLotterySet(id);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///  获取投注赔率(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static List<MD_LotterySet> GetLotterySetList(int pageIndex, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.Lottery.GetLotterySetList(pageIndex, pageSize, condition);
            List<MD_LotterySet> list = (List<MD_LotterySet>)ModelConvertHelper<MD_LotterySet>.ConvertToModel(dt);
            return list;
        }
        /// <summary>
        ///  获取投注赔率
        /// </summary>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static DataSet GetLotterySetList(string condition = "")
        {
            return OWZX.Data.Lottery.GetLotterySetList(condition);
        }
        #endregion

        #region 记录等待计算用户奖金彩票
        /// <summary>
        /// 添加等待计算奖金记录
        /// </summary>
        /// <param name="bett"></param>
        /// <returns></returns>
        public static bool AddWaitPay(MD_WaitPayBonus pay)
        {
            string result = OWZX.Data.Lottery.AddWaitPay(pay);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新等待计算奖金记录 
        /// </summary>
        /// <param name="draw"></param>
        /// <returns></returns>
        public static bool UpdateWaitPay(MD_WaitPayBonus pay)
        {
            string result = OWZX.Data.Lottery.UpdateWaitPay(pay);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除等待计算奖金记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteWaitPay(string id)
        {
            string result = OWZX.Data.Lottery.DeleteWaitPay(id);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///  获取等待计算奖金记录(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static DataTable GetWaitPayList(int pageIndex, int pageSize, string condition = "")
        {
            return OWZX.Data.Lottery.GetWaitPayList(pageIndex, pageSize, condition);
        }

        #endregion

        #region 发放奖金计算竞猜投注信息
        /// <summary>
        /// 计算竞猜投注结果
        /// </summary>
        /// <returns></returns>
        public static string ExcuteBettResult()
        {
            return OWZX.Data.Lottery.ExcuteBettResult();
        }
        /// <summary>
        /// 计算北京28竞猜投注结果
        /// </summary>
        /// <param name="lotterynum"></param>
        /// <returns></returns>
        public static string ExcuteBJBettResult(string lotterynum)
        {
            return OWZX.Data.Lottery.ExcuteBJBettResult(lotterynum);
        }
        /// <summary>
        /// 计算加拿大28竞猜投注结果
        /// </summary>
        /// <param name="lotterynum"></param>
        /// <returns></returns>
        public static string ExcuteCanadaBettResult(string lotterynum)
        {
            return OWZX.Data.Lottery.ExcuteCanadaBettResult(lotterynum);
        }
        #endregion

        #region 房间信息

        /// <summary>
        /// 添加投注记录
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static bool AddRoom(MD_LotteryRoom room)
        {
            string result = OWZX.Data.Lottery.AddRoom(room);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新投注记录 
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static bool UpdateRoom(MD_LotteryRoom room)
        {
            string result = OWZX.Data.Lottery.UpdateRoom(room);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除投注记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteRoom(string id)
        {
            string result = OWZX.Data.Lottery.DeleteRoom(id);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///  获取投注记录(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where 主表 a  彩票类型表 b 房间类型表c</param>
        /// <returns></returns>
        public static List<MD_LotteryRoom> GetRoomList(int pageIndex, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.Lottery.GetRoomList(pageIndex, pageSize, condition);
            List<MD_LotteryRoom> list = (List<MD_LotteryRoom>)ModelConvertHelper<MD_LotteryRoom>.ConvertToModel(dt);
            return list;
        }
        #endregion

        #region 回水规则

        /// <summary>
        /// 添加回水规则
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static bool AddRateRule(MD_BackRate rate)
        {
            string result = OWZX.Data.Lottery.AddRateRule(rate);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新回水规则
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static bool UpdateRateRule(MD_BackRate rate)
        {
            string result = OWZX.Data.Lottery.UpdateRateRule(rate);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除回水规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteRateRule(string id)
        {
            string result = OWZX.Data.Lottery.DeleteRateRule(id);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///  获取回水规则(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where 主表 a  房间类型表 b </param>
        /// <returns></returns>
        public static List<MD_BackRate> GetRateRuleList(int pageIndex, int pageSize, string condition = "")
        {

            DataTable dt = OWZX.Data.Lottery.GetRateRuleList(pageIndex, pageSize, condition);
            List<MD_BackRate> list = (List<MD_BackRate>)ModelConvertHelper<MD_BackRate>.ConvertToModel(dt);
            return list;
        }
        #endregion

        #region 报表
        /// <summary>
        /// 盈利报表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="pageNumber"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable GetProfitList(string type, int pageSize, int pageNumber, string condition = "")
        {
            return OWZX.Data.Lottery.GetProfitList(type, pageSize, pageNumber, condition);
        }

        /// <summary>
        /// 盈利报表 彩票类型不参数分组,包含回水
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="pageNumber"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable GetProfitListNoLottery(string type, int pageSize, int pageNumber, string condition = "")
        {
            return OWZX.Data.Lottery.GetProfitListNoLottery(type, pageSize, pageNumber, condition);
        }
        #endregion

        #region 特殊赔率

        /// <summary>
        /// 添加投注记录
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static bool AddOddsRoom(MD_OddsRoom room)
        {
            string result = OWZX.Data.Lottery.AddOddsRoom(room);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新投注记录 
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static bool UpdateOddsRoom(MD_OddsRoom room)
        {
            string result = OWZX.Data.Lottery.UpdateOddsRoom(room);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除投注记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteOddsRoom(string id)
        {
            string result = OWZX.Data.Lottery.DeleteOddsRoom(id);
            if (result.EndsWith("成功"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///  获取投注记录(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where 主表 a  彩票类型表 b 房间类型表c</param>
        /// <returns></returns>
        public static List<MD_OddsRoom> GetOddsRoomList(int pageIndex, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Data.Lottery.GetOddsRoomList(pageIndex, pageSize, condition);
            List<MD_OddsRoom> list = (List<MD_OddsRoom>)ModelConvertHelper<MD_OddsRoom>.ConvertToModel(dt);
            return list;
        }
        #endregion

        #region 急速28 

        public static List<MD_LotteryAllMoney> GetAllMoneyByLotteryNum(string lotterynum = "", int type = 3)
        {
            DataTable dt = OWZX.Data.Lottery.GetAllMoneyByLotteryNum(lotterynum, type);
            List<MD_LotteryAllMoney> list = (List<MD_LotteryAllMoney>)ModelConvertHelper<MD_LotteryAllMoney>.ConvertToModel(dt);
            return list;
        }
        public static List<MD_LotteryOpenSet> GetLotteryOpenSetList(int type = 3)
        {
            DataTable dt = OWZX.Data.Lottery.GetLotteryOpenSetList(type);
            List<MD_LotteryOpenSet> list = (List<MD_LotteryOpenSet>)ModelConvertHelper<MD_LotteryOpenSet>.ConvertToModel(dt);
            return list;
        }

        public static string UpdateSetStaus(int lotteryid, int status, string result, string lotterynum)
        {
            return OWZX.Data.Lottery.UpdateSetStaus(lotteryid, status, result, lotterynum);
        }
        public static string UpdateSetDetailStaus(int lotteryid, int detailid, int isdefault, string result = "", string lotterynum = "")
        {
            return OWZX.Data.Lottery.UpdateSetDetailStaus(lotteryid, detailid, isdefault, result, lotterynum);
        }

        #endregion

        #region app
        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="chag"></param>
        /// <returns></returns>
        public static string AddLimit(MD_AppLimit mode)
        {
            return OWZX.Data.Lottery.AddLimit(mode);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string UpdateLimit(MD_AppLimit mode)
        {
            return OWZX.Data.Lottery.UpdateLimit(mode);
        }

        /// <summary>
        ///获取信息
        /// </summary>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static List<MD_AppLimit> GetLimitList(string condition = "")
        {
            DataTable dt= OWZX.Data.Lottery.GetLimitList(condition);
            List<MD_AppLimit> list = (List<MD_AppLimit>)ModelConvertHelper<MD_AppLimit>.ConvertToModel(dt);
            return list;
        }
        #endregion

        #region 盈利报表
        public static DataTable GetWeekProfit(int uid)
        {
            return OWZX.Data.Lottery.GetWeekProfit(uid);
        }

        #endregion
    }
}
