using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Services
{
    [DisallowConcurrentExecution]
    public class WaitPayBonusEvent : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Execute();
        }

        private object lkresult = new object();

        /*
         * 计算奖金
         */
        public void Execute()
        {
            lock (lkresult)
            {
                Lottery.ExcuteBettResult();
            }

        }
    }
}
