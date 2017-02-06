using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    public class MD_AccessTokenResult
    {
        public AccessTokenModel SuccessResult { get; set; }
        public bool Result { get; set; }
        public ErrorMsg ErrorResult { get; set; }
    }

    /// <summary>
    /// 通过code获取access_token 请求成功的实体
    /// </summary>
    public class AccessTokenModel
    {
        /// <summary>
        /// 接口调用凭证
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 当前 APP 的 UUID 值
        /// </summary>
        public string application { get; set; }

    }

    /// <summary>
    /// 错误访问的情况 
    /// </summary>
    public class ErrorMsg
    {
        /// <summary>
        /// 错误编号
        /// </summary>
        public int error { get; set; }
        /// <summary>
        /// 错误提示消息
        /// </summary>
        public string exception { get; set; }

        /// <summary>
        /// 错误提示消息
        /// </summary>
        public string error_description { get; set; }
    }
    /// <summary>
    /// 聊天室数据
    /// </summary>
    public class MD_HXRoomData
    {
        public string uri { get; set; }
        public List<MD_RoomData> data { get; set; }
        public long timestamp { get; set; }
        public int duration { get; set; }
        public int count { get; set; }

        public string organization { get; set; }


    }
    public class MD_RoomData
    {
        /// <summary>
        /// 聊天室id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 聊天室名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 管理员
        /// </summary>
        public string owner { get; set; }
        /// <summary>
        /// 聊天室内人数
        /// </summary>
        public int affiliations_count { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 删除聊天室成员状态
        /// </summary>
        public bool result { get; set; }
        /// <summary>
        /// 删除聊天室成员名称
        /// </summary>
        public string user { get; set; }
    }
}
