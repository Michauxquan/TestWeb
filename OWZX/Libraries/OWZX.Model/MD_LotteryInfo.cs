using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
   public class MD_LotteryInfo
    {
       public int lotid { get; set; }
       /// <summary>
       /// 彩票id
       /// </summary>
       public int lotterytype { get; set; }
       /// <summary>
       /// 彩票名称
       /// </summary>
       public string lotteryname { get; set; }
       /// <summary>
       /// 是否启动
       /// </summary>
       public bool isstart { get; set; }
       public DateTime  addtime { get; set; }
       public DateTime updatetime { get; set; }
    }
}
