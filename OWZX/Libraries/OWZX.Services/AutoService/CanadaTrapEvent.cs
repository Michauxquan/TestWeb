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
    public class CanadaTrapEvent : IJob
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
         * 彩票逻辑：处理超过开奖时间一分钟 还没有开奖的记录
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
                        Lottery.TrapCanada();
                    }
                }
                catch (Exception ex)
                {
                    LogClass.Log.WriteLogs("", "", "", "执行加拿大28补漏异常：" + ex.Message, root + @"\service\can28\", false);
                }
            }
        }
    }
}
