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
    public class CloseLuckEvent : IJob
    {
        private object lkclose = new object();
   
        static string root = HttpRuntime.AppDomainAppPath + "Logs";
        /*
         * 修改彩票状态为封盘中
         */
        public void Execute(IJobExecutionContext context)
        {
            Execute();
        }

        public void Execute()
        {
            //执行获取北京快乐8 开奖结果并计算出北京28开奖
            lock (lkclose)
            {
                try
                {
                    Lottery.UpdateLotteryStatus();
                }
                catch (Exception ex)
                {
                   LogClass.Log.WriteLogs("", "", "", "执行修改竞猜状态异常：" + ex.Message, root + @"\service\", false);
                }
            }

        }
    }
}
