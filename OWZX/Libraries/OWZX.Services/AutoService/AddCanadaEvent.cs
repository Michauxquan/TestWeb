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
    public class AddCanadaEvent : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Execute();
        }

        private object lkcanadalk = new object();

        string start = "20:00";
        string end = "21:00";
        static string root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Logs";//服务运行路径 
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
                        Lottery.InitCanadaLottery();
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
