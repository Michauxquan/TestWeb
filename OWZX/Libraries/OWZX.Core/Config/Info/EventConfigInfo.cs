using System;
using System.Collections.Generic;

namespace OWZX.Core
{
    /// <summary>
    /// 事件配置信息类
    /// </summary>
    [Serializable]
    public class EventConfigInfo : IConfigInfo
    {
        private int _bspeventstate;//OWZX事件状态
        private int _bspeventperiod;//OWZX事件执行间隔(单位为分钟)
        private List<EventInfo> _bspeventlist;//OWZX事件列表

        /// <summary>
        /// OWZX事件状态
        /// </summary>
        public int BSPEventState
        {
            get { return _bspeventstate; }
            set { _bspeventstate = value; }
        }
        /// <summary>
        /// OWZX事件执行间隔(单位为分钟)
        /// </summary>
        public int BSPEventPeriod
        {
            get { return _bspeventperiod; }
            set { _bspeventperiod = value; }
        }
        /// <summary>
        /// OWZX事件列表
        /// </summary>
        public List<EventInfo> BSPEventList
        {
            get { return _bspeventlist; }
            set { _bspeventlist = value; }
        }
    }
}
