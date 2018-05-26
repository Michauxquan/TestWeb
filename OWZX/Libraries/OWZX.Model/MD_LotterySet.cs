using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    /// <summary>
    /// 投注类型及赔率
    /// </summary>
    public class MD_LotterySet
    {
        public Int64 Id { get; set; }
        private int bttypeid;
        public int Bttypeid
        {
            get { return bttypeid; }
            set { bttypeid = value; }
        }

        private int lotterytype;
        /// <summary>
        /// 彩票类型id
        /// </summary>
        public int Lotterytype
        {
            get { return lotterytype; }
            set { lotterytype = value; }
        }
        private string lottery;
        /// <summary>
        /// 彩票类型
        /// </summary>
        public string Lottery
        {
            get { return lottery; }
            set { lottery = value.TrimEnd(); }
        }
        /// <summary>
        /// 房间类型id
        /// </summary>
        public int RoomType { get; set; }

        /// <summary>
        /// 房间类型
        /// </summary>
        public string Room { get; set; }

        private string settype;
        /// <summary>
        /// 投注类型
        /// </summary>
        public string Settype
        {
            get { return settype; }
            set { settype = value.TrimEnd(); }
        }
        private int type;
        /// <summary>
        /// 投注类型id
        /// </summary>
        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        
        private string item;
        /// <summary>
        ///投注项 
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(100, ErrorMessage = "名称长度不能大于50")]
        public string Item
        {
            get { return item; }
            set { item = value.TrimEnd(); }
        }
        public string PItem { get; set; }
        private string odds;
        /// <summary>
        /// 项赔率
        /// </summary>
        [Required(ErrorMessage = "赔率不能为空")]
        [StringLength(100, ErrorMessage = "赔率不能大于100000")]
        public string Odds
        {
            get { return odds; }
            set { odds = value.TrimEnd(); }
        }

        private string nums;
        /// <summary>
        ///中奖数字 
        /// </summary>
         [Required(ErrorMessage = "和值不能为空")]
         [StringLength(100, ErrorMessage = "和值长度不能大于50")]
        public string Nums
        {
            get { return nums; }
            set { nums = value.TrimEnd(); }
        }

        private DateTime addtime;
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }

        public int TotalCount { get; set; }
    }
    
}
