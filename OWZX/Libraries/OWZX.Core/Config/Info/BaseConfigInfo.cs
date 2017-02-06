using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Core
{
    /// <summary>
    /// 事件配置信息类
    /// </summary>
    [Serializable]
    public class BaseConfigInfo : IConfigInfo
    {
        public List<BaseInfo> BaseList { get; set; }
    }
    public class BaseInfo 
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 银行
        /// </summary>
        public string Bank { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string BankAddress { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string Image { get; set; }
    }
}
