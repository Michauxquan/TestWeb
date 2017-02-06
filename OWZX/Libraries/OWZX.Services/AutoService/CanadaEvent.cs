using OWZX.Model;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OWZX.Services
{
    [DisallowConcurrentExecution]
    public class CanadaEvent : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Execute();
        }

        private object lkcanadalk = new object();

        string start = "20:00";
        string end = "21:00";
        static string root = HttpRuntime.AppDomainAppPath + "Logs";
        /*
         * 彩票逻辑：投注3:30s 封盘 30s ,到时（每期规定的开奖时间）增加新的投注记录，开奖
         */
        public void Execute()
        {
            //执行获取加拿大卑斯快乐8 开奖结果并计算出加拿大28开奖
            lock (lkcanadalk)
            {
                try
                {
                    TimeSpan startTime = DateTime.Parse(start).TimeOfDay;
                    TimeSpan endTime = DateTime.Parse(end).TimeOfDay;
                    TimeSpan tmNow = DateTime.Now.TimeOfDay;

                    if (tmNow < startTime || tmNow > endTime)
                    {
                        //每3:30一次
                        string expect = Lottery.AddCanadaLottery();
                        if (expect != string.Empty)
                        {
                            MD_WaitPayBonus pay = new MD_WaitPayBonus { Expect = expect, Isread = false };
                            Lottery.AddWaitPay(pay);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogClass.Log.WriteLogs("", "", "", "执行加拿大28异常：" + ex.Message, root + @"\service\can28\", false);
                }
            }
        }
    }
}
