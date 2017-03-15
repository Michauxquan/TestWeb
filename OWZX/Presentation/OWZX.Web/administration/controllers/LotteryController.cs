using System;
using System.Web;
using System.Web.Mvc;
using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.Web.Admin.Models;
using System.Text;
using System.Collections.Generic;
using OWZX.Model;
using System.Data;
using System.Linq;

namespace OWZX.Web.Admin.Controllers
{
    /// <summary>
    /// 后台彩票控制器类
    /// </summary>
    public partial class LotteryController : BaseAdminController
    {
        /// <summary>
        /// 投注记录
        /// </summary>
        public ActionResult LotteryList(string account = "", int lottype = -1, string expect = "",  int pageSize = 15, int pageNumber = 1)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(" where 1=1");
            if (account != "")
                strb.Append(" and rtrim(b.email)='" + account + "'");
            if (lottype != -1)
                strb.Append(" and a.lotteryid=" + lottype);
            if (expect != "")
                strb.Append(" and a.lotterynum='" + expect + "'");

            List<MD_Bett> btlist = Lottery.GetBettList(pageNumber, pageSize, strb.ToString());
            LotteryListModel list = new LotteryListModel
            {
                account = account,
                lottype = lottype,
                expect = expect, 
                PageModel = new PageModel(pageSize, pageNumber, btlist.Count > 0 ? btlist[0].TotalCount : 0),
                bettList = btlist
            };
            return View(list);
        }


        #region 财务报表
        public ActionResult ProfitList(int lottery = -1, string type = "每日盈亏", string start = "", string end = "", int pageSize = 15, int pageNumber = 1)
        {
            Load();
            StringBuilder strb = new StringBuilder();
            strb.Append(" where 1=1");
            if (lottery > 0)
                strb.Append(" and type=" + lottery);
            if (type != string.Empty && end != "")
                strb.Append(" and convert(varchar(10),b.opentime,120) between " + start+" and "+end);
            DataTable dt = Lottery.GetProfitListNoLottery(type, pageSize, pageNumber, strb.ToString());

            ProfitStatList list = new ProfitStatList
            {
                Lottery = lottery,
                Type = type,
                Start = start,
                End = end,
                ProfitList = dt,
                PageModel = new PageModel(pageSize, pageNumber, dt.Rows.Count > 0 ? int.Parse(dt.Rows[0]["TotalCount"].ToString()) : 0)
            };
            return View(list);
        }

        private void Load()
        {
            List<SelectListItem> statlist = new List<SelectListItem>();
            SelectListItem[] items = new SelectListItem[] {
                new SelectListItem() { Text = "每日盈亏", Value = "每日盈亏" } ,
                new SelectListItem() { Text = "每周盈亏", Value = "每周盈亏" } ,
                new SelectListItem() { Text = "每月盈亏", Value = "每月盈亏" } 
            };
            statlist.AddRange(items);

            ViewData["statlist"] = statlist;
        }
        #endregion

        #region 急速28后台设置

        public ActionResult OpenSet(int type = 3)
        {
            List<MD_LotteryOpenSet> openlist = Lottery.GetLotteryOpenSetList(type);
            List<MD_Lottery> lottery = Lottery.GetLotteryList(1, 1, " where a.status=0 and a.type="+type +" and a.opentime >='"+DateTime.Now.AddMinutes(-12).ToString("yyyy-MM-dd HH:mm:ss")+"'", "asc");
            LotteryOpenListModel list = new LotteryOpenListModel
            {
                type = type,
                lottery = lottery.FirstOrDefault(),
                OpenList = openlist
            };
            return View(list);
        }

        public ActionResult UpdateOpenSetStatus(int status, string result = "", string lotterynum = "", int lottery = 3)
        {
            string msg = Lottery.UpdateSetStaus(lottery, status, result, lotterynum);
            return AjaxResult("sussece", msg);
        }

        public ActionResult UpdateSetDetailStaus(int status, int detailid, int lottery = 3, string result = "", string lotterynum = "")
        {
            string msg = Lottery.UpdateSetDetailStaus(lottery, detailid, status, result, lotterynum);
            return AjaxResult("sussece", msg);
        }

        public ActionResult GetOpenResult(string lotterynum, int settype = -1, int type = 3)
        {
            var list = Lottery.GetAllMoneyByLotteryNum(lotterynum, type);
            if (settype == -1)
            {
                List<MD_LotteryOpenSet> openlist = Lottery.GetLotteryOpenSetList(3);
                settype = openlist.Where(x => x.isdefault == 1).FirstOrDefault().settype;
            }
            //庄家最大赚   0--也可能是最小赔付
            //庄家最小赚   1--也可能是最小赔付 
            //庄家最小赔   2--也可能收支平衡
            //庄家最大赔   3--最大赔付
            List<int> reusltlist = new List<int>();
            string nums = "0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,";
            if (type == 10)
            {
                nums = "1,2,3,4,5,6,7,8,9,10,";
            }
            else if (type == 12)
            {
                nums = "3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,";
            }
            else if (type == 11)
            {
                nums = "2,3,4,5,6,7,8,9,10,11,12,";
            }
            List<int> resultlist = new List<int>();
            if (list.Count > 0)
            {
                if (settype == 0 || settype == 3)
                {
                    //最大赔付不用计算是否包含投注结果
                    if (settype == 0)
                    {
                        foreach (var item in list)
                        {
                            if (!nums.Contains(item.item))
                            {
                                resultlist.Add(int.Parse(item.item));
                            }
                        }
                    }
                    if (resultlist.Count == 0)
                    {
                        var maxmoney = settype == 0
                            ? (from s in list orderby (s.totalmoney - s.lossmoney) descending select s)
                            : (from s in list orderby (s.lossmoney - s.totalmoney) descending select s);
                        foreach (var item in maxmoney)
                        {
                            if (resultlist.Count == 0)
                            {
                                resultlist.Add(int.Parse(item.item));
                            }
                            else
                            {
                                if (resultlist[0] != int.Parse(item.item))
                                {
                                    break;
                                }
                                resultlist.Add(int.Parse(item.item));
                            }
                        }

                    }
                }
                else if (settype == 1 || settype == 2)
                {
                    var maxmoney = settype == 1
                        ? (from s in list
                           where (s.totalmoney - s.lossmoney) > 0
                           orderby (s.totalmoney - s.lossmoney) ascending
                           select s)
                        : (from s in list
                           where (s.lossmoney - s.totalmoney) < 0
                           orderby (s.lossmoney - s.totalmoney) descending
                           select s);
                    if (settype == 1 && !maxmoney.Any())
                    {
                        //店家赚最小值 如果投注金额与赔付金额都是<0的 则 去 赔付-总价 最小值
                        maxmoney = from s in list
                                   where (s.lossmoney - s.totalmoney) > 0
                                   orderby (s.lossmoney - s.totalmoney) ascending
                                   select s;
                    }
                    if (settype == 2 && !maxmoney.Any())
                    {
                        //店家最小赔 如果投注的中奖数量都是>0的 则 去 总价-赔付 最小值
                        maxmoney = from s in list
                                   where (s.lossmoney - s.totalmoney) > 0
                                   orderby (s.lossmoney - s.totalmoney) ascending
                                   select s;
                    }
                    foreach (var item in maxmoney)
                    {
                        if (resultlist.Count == 0)
                        {
                            resultlist.Add(int.Parse(item.item));
                        }
                        else
                        {
                            if (resultlist[0] != int.Parse(item.item))
                            {
                                break;
                            }
                            resultlist.Add(int.Parse(item.item));
                        }
                    }
                }
            }
            else
            {
                if (type == 3)
                {
                    for (int i = 0; i <= 27; i++)
                    {
                        resultlist.Add(i);
                    }
                }
                else if (type == 10)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        resultlist.Add(i);
                    }
                }
                else if (type == 11)
                {
                    for (int i = 2; i <= 12; i++)
                    {
                        resultlist.Add(i);
                    }
                }
                else if (type == 12)
                {
                    for (int i = 3; i <= 18; i++)
                    {
                        resultlist.Add(i);
                    }
                }
            }
            string resultNumList = "";
            Random random1 = new Random();
            for (var i = 0; i < 7; i++)
            {
                if (type == 10)
                {
                    resultNumList += getJSTwoResult(resultlist[random1.Next(0, resultlist.Count)]) + ";";
                }
                else if (type == 11 || type == 12)
                {
                    resultNumList += getJSResult(resultlist[random1.Next(0, resultlist.Count)], type == 11 ? 2 : 3) +";";
                }
                else
                {
                    resultNumList += getResult(resultlist[random1.Next(0, resultlist.Count)]) + ";";
                }
            }
            return AjaxResult("sussece", resultNumList);
        }
        Random _random = new Random();
        //急速28生成随机数
        public string getResult(int resultnum)
        {
            List<string> str = new List<string>();
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    if (i + j > resultnum)
                    {
                        break;
                    }
                    else
                    {
                        for (int k = 0; k <= 9; k++)
                        {
                            if (i + j + k == resultnum)
                            {
                                str.Add(i + "+" + j + "+" + k + "=" + resultnum);
                                break;
                            }
                        }
                    }
                }
            }
            string[] result = new string[str.Count];
            for (int mm = 0; mm < str.Count; mm++)
            {
                int pos = _random.Next(str.Count);
                var temp = str[mm];
                result[mm] = str[pos];
                result[pos] = temp;
            }
            int tt = _random.Next(result.Length);
            return result[tt];
        }
        //急速11,急速16 生成随机数
        public string getJSResult(int resultnum,int layer=2)
        {
            List<string> str = new List<string>();
            for (int i = 1; i <= 6; i++)
            {
                for (int j = 1; j <= 6; j++)
                {

                    if (i + j > resultnum)
                    {
                        break;
                    }
                    else
                    {
                        if (layer == 2)
                        {
                             if (i + j  == resultnum)  
                             {
                                 str.Add(i + "+" + j+ "=" + resultnum);
                                break;
                            }
                        }
                        else
                        {
                            for (int k = 1; k <= 6; k++)
                            {
                                if (i + j + k == resultnum)
                                {
                                    str.Add(i + "+" + j + "+" + k + "=" + resultnum);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            string[] result = new string[str.Count];
            for (int mm = 0; mm < str.Count; mm++)
            {
                int pos = _random.Next(str.Count);
                var temp = str[mm];
                result[mm] = str[pos];
                result[pos] = temp;
            }
            int tt = _random.Next(result.Length);
            return result[tt];
        }
        //急速10 生成随机数
        public string getJSTwoResult(int resultnum)
        {
            List<string> str = new List<string>();
            for (int i = 1; i <= 10; i++)
            {
                if (resultnum != i)
                {
                    str.Add(i.ToString());
                }
            } 
            string[] result = new string[str.Count];
            int count = str.Count;
            int cbRandCount = 0;// 索引  
            int cbPosition = 0;// 位置  
            int k = 0;
            do
            { 
                int r = count - cbRandCount;
                cbPosition = _random.Next(r);
                result[k++] = str[cbPosition];
                cbRandCount++;
                str[cbPosition] = str[r - 1];// 将最后一位数值赋值给已经被使用的cbPosition  
            } while (cbRandCount < count);  

            //string[] result = new string[str.Count];
            //for (int mm = 0; mm < str.Count; mm++)
            //{
            //    int pos = _random.Next(str.Count);
            //    var temp = str[mm];
            //    result[mm] = str[pos];
            //    result[pos] = temp;
            //}
            string resultstr = resultnum.ToString();
            result.ToList().ForEach(x => { resultstr += "," + x.ToString(); });
            return resultstr;
        }
        #endregion

    }
}
