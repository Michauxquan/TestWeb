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
    public class BJTrapEvent : IJob
    {
        private object lkbjlk = new object();
        string start = "09:00";
        string end = "23:57";
        static string root = HttpRuntime.AppDomainAppPath + "Logs";
        /*
         * 彩票逻辑：处理超过开奖时间一分钟 还没有开奖的记录
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
                        Lottery.TrapBJ();
                    }
                }
                catch (Exception ex)
                {
                    LogClass.Log.WriteLogs("", "", "", "执行北京28补漏异常：" + ex.Message, root + @"\service\bj28\", false);
                }
            }

        }
    }
}
