using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    /// <summary>
    /// 房间信息
    /// </summary>
    public class MD_LotteryRoom
    {
        public Int64 Id { get; set; }
        private int roomid;
        public int Roomid
        {
            get { return roomid; }
            set { roomid = value; }
        }

        private int lotterytype;
        /// <summary>
        ///彩票类型10：北京28 11：加拿大28
        /// </summary>
        [Required(ErrorMessage = "请选择彩票类型")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择彩票类型")]
        public int Lotterytype
        {
            get { return lotterytype; }
            set { lotterytype = value; }
        }
        /// <summary>
        /// 彩票名称
        /// </summary>
        public string LotteryName { get; set; }
        private int room;
        /// <summary>
        /// 房间类型20：初级 21：中级 22：高级
        /// </summary>
        [Required(ErrorMessage = "请选择房间类型")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择房间类型")]
        public int Room
        {
            get { return room; }
            set { room = value; }
        }
        /// <summary>
        /// 房间类型名称
        /// </summary>
        public string RoomName { get; set; }
        private int maxuser;
        /// <summary>
        /// 房间最大人数
        /// </summary>
        //[Required(ErrorMessage = "房间最大人数")]
        //[Range(1, 500, ErrorMessage = "房间人数范围1~500")]
        public int Maxuser
        {
            get { return maxuser; }
            set { maxuser = value; }
        }
        private int vipmaxuser;
        /// <summary>
        /// 房间最大人数
        /// </summary>
        //[Required(ErrorMessage = "房间最大人数")]
        //[Range(1, 500, ErrorMessage = "房间人数范围1~500")]
        public int VipMaxuser
        {
            get { return vipmaxuser; }
            set { vipmaxuser = value; }
        }

        private int backrate;
        /// <summary>
        /// 房间回水
        /// </summary>
        //[Required(ErrorMessage = "请输入房间回水")]
        //[Range(1, 100, ErrorMessage = "房间回水范围1%~100%")]
        public int Backrate
        {
            get { return backrate; }
            set { backrate = value; }
        }

        private int enter;
        /// <summary>
        /// 房间进入条件
        /// </summary>
        //[Range(1, int.MaxValue, ErrorMessage = "请输入有效房间进入条件")]
        public int Enter
        {
            get { return enter; }
            set { enter = value; }
        }

        private DateTime addtime;
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }

    }
}
