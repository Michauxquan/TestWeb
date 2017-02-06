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
    public class BJLuckEvent : IJob
    {
        private object lkbjlk = new object();
        string start = "09:00";
        string end = "23:57";
        static string root = HttpRuntime.AppDomainAppPath + "Logs";
        /*
         * 彩票逻辑：投注4:30s 封盘 30s ,到时（每期规定的开奖时间）增加新的投注记录，开奖
         */
        public void Execute(IJobExecutionContext context)
        {
            Execute();
        }

        public void Execute()
        {
            //执行获取北京快乐8 开奖结果并计算出北京28开奖
            lock (lkbjlk)
            {
                try
                {
                    TimeSpan startTime = DateTime.Parse(start).TimeOfDay;
                    TimeSpan endTime = DateTime.Parse(end).TimeOfDay;
                    TimeSpan tmNow = DateTime.Now.TimeOfDay;

                    if (tmNow >= startTime && tmNow <= endTime)
                    {
                        if (tmNow.Minutes % 5 == 0)
                        {
                            //获取开奖结果
                            string expect = Lottery.AddLottery();
                            if (expect != string.Empty)
                            {
                                MD_WaitPayBonus pay = new MD_WaitPayBonus { Expect = expect, Isread = false };
                                Lottery.AddWaitPay(pay);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogClass.Log.WriteLogs("", "", "", "执行北京28异常：" + ex.Message, root + @"\service\bj28\", false);
                }
            }

        }
    }
}
