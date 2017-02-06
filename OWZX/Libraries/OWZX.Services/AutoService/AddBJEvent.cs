using OWZX.Model;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OWZX.Services
{
    [DisallowConcurrentExecution]
    public class AddBJEvent : IJob
    {
        private object lkbjlk = new object();
        string start = "09:00";
        string end = "23:57";
        static string root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Logs";//服务运行路径 
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
                        Lottery.InitLottery();
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
