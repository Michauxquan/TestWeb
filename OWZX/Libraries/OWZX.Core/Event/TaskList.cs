using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OWZX.Core
{
    public class TaskList
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName
        {
            get;
            set;
        }
        /// <summary>
        /// 工作JOB的类名称
        /// </summary>
        public string ClassName
        {
            get;
            set;
        }
        /// <summary>
        /// 间隔时间
        /// </summary>
        public int TaskIntervalTime
        { get; set; }
    }
}
