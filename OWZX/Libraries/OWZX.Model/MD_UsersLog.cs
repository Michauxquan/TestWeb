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
    public class MD_UsersLog
    {

        public Int64 Id { get; set; } 

        private int uid;
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }
        private int type;
        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        private string ip;
        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }
        private string remark; 
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        private string ipname; 
        public string IpName
        {
            get { return ipname; }
            set { ipname = value; }
        }

        private DateTime createtime;
        [JsonProperty(PropertyName="Time")]
        public DateTime CreateTime
        {
            get { return createtime; }
            set { createtime = value; }
        }
       
        public int TotalCount { get; set; }
    }
}
