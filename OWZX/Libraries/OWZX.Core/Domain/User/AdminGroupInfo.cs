using System;

namespace OWZX.Core
{
    /// <summary>
    /// 管理员组信息类
    /// </summary>
    public class AdminGroupInfo
    {
        private int _admingid;//管理员组id
        private int _depid;//部门id
        private string _depname;//部门名称
        private string _title;//管理员组标题
        private string _actionlist;//管理员组行为列表
        private string _cusactionlist;//前端行为列表

        /// <summary>
        /// 管理员组id
        /// </summary>
        public int AdminGid
        {
            get { return _admingid; }
            set { _admingid = value; }
        }

        /// <summary>
        /// 部门id
        /// </summary>
        public int DepId
        {
            get { return _depid; }
            set { _depid = value; }
        }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepName
        {
            get { return _depname; }
            set { _depname = value; }
        }
        /// <summary>
        /// 管理员组标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value.TrimEnd(); }
        }
        /// <summary>
        /// 管理员组行为列表
        /// </summary>
        public string ActionList
        {
            get { return _actionlist; }
            set { _actionlist = value; }
        }
    }
}
