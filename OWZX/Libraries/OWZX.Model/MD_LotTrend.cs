using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    public class MD_LotTrend
    {
        public Int64 id { get; set; }
        public string Expect { get; set; }
        public string ResultNum { get; set; }
        public string Big { get; set; }
        public string Small { get; set; }
        public string Single { get; set; }
        public string Dble { get; set; }
        public string LSingle { get; set; }
        public string SSingle { get; set; }
        public string LDble { get; set; }
        public string SDble { get; set; }
        public int TotalCount { get; set; }
    }

}
