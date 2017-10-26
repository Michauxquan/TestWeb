using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OWZX.Model
{
    /// <summary>
    /// 用户回水
    /// </summary>
    public class MD_UserRptDay
    {
        public Int64 Id { get; set; }
        
        private int uid;
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }
        /// <summary>
        /// 账户
        /// </summary>
        public string Email { get; set; }

        private decimal bettfee;
        /// <summary>
        /// 变更金额
        /// </summary>
        [JsonProperty(PropertyName = "Money")]
        public decimal BettFee
        {
            get { return bettfee; }
            set { bettfee = value; }
        }
        private decimal winfee;
        public decimal WinFee
        {
            get { return winfee; }
            set { winfee = value; }
        }
        private decimal ylfee;
        /// <summary>
        /// 变更金额
        /// </summary>
        [JsonProperty(PropertyName = "YLFee")]
        public decimal YLFee
        {
            get { return ylfee; }
            set { ylfee = value; }
        }
        private decimal backfee;
        /// <summary>
        /// 变更记录
        /// </summary>
        public decimal BackFee
        {
            get { return backfee; }
            set { backfee = value; }
        }

        private DateTime addtime;
        [JsonProperty(PropertyName = "Time")]
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }

        public int TotalCount { get; set; }
    }
}
