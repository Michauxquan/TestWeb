using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    /// <summary>
    /// 用户消息
    /// </summary>
   public class MD_Message
    {
        private int msgid;
        public int Msgid
        {
            get { return msgid; }
            set { msgid = value; }
        }
        private string account;
        /// <summary>
        /// 用户账号(手机号)
        /// </summary>
        public string Account
        {
            get { return account; }
            set { account = value.TrimEnd(); }
        }
        private int uid;
        /// <summary>
        /// 用户id
        /// </summary>
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        private string title;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value.TrimEnd(); }
        }

        private string body;
        /// <summary>
        /// 内容
        /// </summary>
        public string Body
        {
            get { return body; }
            set { body = value.TrimEnd(); }
        }

        private DateTime addtime;
        [JsonProperty(PropertyName="Time")]
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }

    }
}
