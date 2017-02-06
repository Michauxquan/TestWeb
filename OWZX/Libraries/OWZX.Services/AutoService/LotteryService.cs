using OWZX.Core;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OWZX.Services
{
   public class LotteryService
    {
        ISchedulerFactory schedulerFactory = new StdSchedulerFactory(); //内存调度
        IScheduler scheduler;
        static LotteryService _instance = null;
        static object lockObj = new object();
        static string root = HttpRuntime.AppDomainAppPath + "Logs";// ConfigurationManager.AppSettings["log"];
        List<TaskList> joblist = null;
        /// <summary>
        /// 线程安全的单例对象
        /// </summary>
        public static LotteryService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new LotteryService();
                        }
                    }
                }
                return _instance;
            }
        }
        public void Start()
        {
            LogClass.Log.WriteLogs("", "", "", "启动服务", root + @"\service\", false);
            try
            {
                joblist = new List<TaskList>() { 
                            new TaskList() { ClassName = "添加北京28", TaskName = "AddBJEvent",TaskIntervalTime=10},
                            new TaskList() { ClassName = "添加加拿大28", TaskName = "AddCanadaEvent",TaskIntervalTime=10},
                            new TaskList() { ClassName = "获取北京28", TaskName = "BJLuckEvent",TaskIntervalTime=5},
                            new TaskList() { ClassName = "处理开奖异常北京28", TaskName = "BJTrapEvent",TaskIntervalTime=60},
                            new TaskList() { ClassName = "获取加拿大28", TaskName = "CanadaEvent",TaskIntervalTime=5},
                            new TaskList() { ClassName = "处理开奖异常加拿大28", TaskName = "CanadaTrapEvent",TaskIntervalTime=60},
                            //new TaskList() { ClassName = "发放用户奖励", TaskName = "WaitPayBonusEvent",TaskIntervalTime=60},
                            new TaskList() { ClassName = "更新竞猜为封盘", TaskName = "CloseLuckEvent",TaskIntervalTime=10}
                };

                TaskJobCreate();
            }
            catch (Exception er)
            {
                LogClass.Log.WriteLogs("", "", "", "启动服务异常：" + er.Message, root + @"\service\", false);
            }
        }

        private void TaskJobCreate()
        {
            foreach (TaskList task in joblist)
            {
                CreateJob(task.TaskIntervalTime, task.TaskName);
            }
        }

        private void CreateJob(int time, string type)
        {
            try
            {
                scheduler = schedulerFactory.GetScheduler();
                //创建一个Job来执行特定的任务
                IJobDetail synchronousData = null;
                ITrigger trigger = null;
                switch (type)
                {
                    case "BJLuckEvent":
                        synchronousData = JobBuilder.Create<BJLuckEvent>()
                                               .WithIdentity(type).Build();
                        trigger = TriggerBuilder.Create().WithCalendarIntervalSchedule(e => e.WithIntervalInSeconds(time)).Build();
                        break;
                    case "BJTrapEvent":
                        synchronousData = JobBuilder.Create<BJTrapEvent>()
                                               .WithIdentity(type).Build();
                        trigger = TriggerBuilder.Create().WithCalendarIntervalSchedule(e => e.WithIntervalInSeconds(time)).Build();
                        break;
                    case "CanadaEvent":
                        synchronousData = JobBuilder.Create<CanadaEvent>()
                                                .WithIdentity(type).Build();
                        trigger = TriggerBuilder.Create().WithDailyTimeIntervalSchedule(a => a.WithIntervalInSeconds(time)).Build();
                        break;
                    case "CanadaTrapEvent":
                        synchronousData = JobBuilder.Create<CanadaTrapEvent>()
                                                .WithIdentity(type).Build();
                        trigger = TriggerBuilder.Create().WithDailyTimeIntervalSchedule(a => a.WithIntervalInSeconds(time)).Build();
                        break;
                    case "WaitPayBonusEvent":
                        synchronousData = JobBuilder.Create<WaitPayBonusEvent>()
                                                .WithIdentity(type).Build();
                        trigger = TriggerBuilder.Create().WithDailyTimeIntervalSchedule(a => a.WithIntervalInSeconds(time)).Build();
                        break;
                    case "CloseLuckEvent":
                        synchronousData = JobBuilder.Create<CloseLuckEvent>()
                                                .WithIdentity(type).Build();
                        trigger = TriggerBuilder.Create().WithDailyTimeIntervalSchedule(a => a.WithIntervalInSeconds(time)).Build();
                        break;
                }

                synchronousData.JobDataMap["type"] = type;
                //将创建好的任务和触发规则加入到Quartz中
                scheduler.ScheduleJob(synchronousData, trigger);
                ////创建并定义触发器的规则（每天执行一次时间为：时：分）
                //ITrigger trigger = TriggerBuilder.Create()
                //                  .WithDailyTimeIntervalSchedule(
                //                  a => a.WithIntervalInHours(24).OnEveryDay().StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(14, 02))).Build();
                //创建并定义触发器的规则(执行间隔为：分)
                //ITrigger trigger = TriggerBuilder.Create().WithDailyTimeIntervalSchedule(a => a.WithIntervalInMinutes(time)).Build();

                //开始
                scheduler.Start();
            }
            catch (Exception er)
            {
                LogClass.Log.WriteLogs("", "", "", "创建Job异常：" + er.Message, root + @"\service\", false);
            }
        }
        public void Stop()
        {
            scheduler.Shutdown(false);
        }
    }
}
