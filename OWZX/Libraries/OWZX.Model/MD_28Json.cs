using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    /// <summary>
    /// 彩票开奖信息
    /// </summary>
    public class MD_28Json
    {
        public int rows { get; set; }
        public string code { get; set; }
        public string info { get; set; }
        public List<LK28Item> data { get; set; }
    }
    public class LK28Item
    {
        /// <summary>
        /// 彩票代码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 期号
        /// </summary>
        public string expect { get; set; }
        /// <summary>
        /// 开奖号码
        /// </summary>
        public string opencode { get; set; }
        /// <summary>
        /// 开奖时间
        /// </summary>
        public string opentime { get; set; }
        /// <summary>
        /// 开奖时间戳
        /// </summary>
        public string opentimestamp { get; set; }
    }
    //{"Error":null,"Data":{"total":196,"rows":[{"Id":173805,"IssueNo":2085109,"AwardDate":"2016-12-25 11:25:30",
    //"Result":"07,12,18,21,22,23,26,32,34,44,47,51,53,55,58,61,65,66,72,77","CreateDate":"2016-12-25 11:25:54","KenoLotteryNo":0}
    public class MD_CanadaJson
    {
        public CanadaItem Data { get; set; }
    }

    public class CanadaItem
    {
        public int Total { get; set; }
        public List<CanadaRows> Rows { get; set; }
    }

    public class CanadaRows
    {
        public int Id { get; set; }
        public string IssueNo { get; set; }
        public string AwardDate { get; set; }
        public string Result { get; set; }
        public string CreateDate { get; set; }
        public int KenoLotteryNo { get; set; }
    }

    //{"time":"2017/02/27 16:43:28","period":1783670,"awardTime":"2017-02-27 16:42:00","result":"1,2,7,17,24,25,29,30,32,35,39,44,48,51,52,59,61,62,65,77",
    //"next_period":1783671,"next_awardTime":"2017/02/27 16:43:30","awardTimeInterval":"2","mode":"760,小,双,上,奇,木","msg":"","state":"0"}
    public class MD_HGJson
    {
        public string period { get; set; }
        public string awardTime { get; set; }
        public string result { get; set; }
   }
}
