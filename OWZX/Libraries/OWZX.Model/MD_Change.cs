using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    /// <summary>
    /// 用户账户金额变更记录
    /// </summary>
    public class MD_Change
    {

        public Int64 Id { get; set; }

        private int achangeid;
        public int Achangeid
        {
            get { return achangeid; }
            set { achangeid = value; }
        }

        private int uid;
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }
        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; set; }

        private decimal changemoney;
        /// <summary>
        /// 变更金额
        /// </summary>
        [JsonProperty(PropertyName = "Money")]
        public decimal Changemoney
        {
            get { return changemoney; }
            set { changemoney = value; }
        }

        private string remark;
        /// <summary>
        /// 变更记录
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        private DateTime addtime;
        [JsonProperty(PropertyName="Time")]
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }
       
        public int TotalCount { get; set; }
    }
}
