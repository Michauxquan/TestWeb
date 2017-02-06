using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Core
{
    /// <summary>
    /// App版本配置信息类
    /// </summary>
    [Serializable]
    public class AppUpdateConfigInfo:IConfigInfo
    {
        private string _version;//版本
        private string _appfilename;//名称
        private string _appdescription;//描述
        private string _downloadurl;//下载地址

        /// <summary>
        /// 版本
        /// </summary>
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        /// 安装包名称
        /// </summary>
        public string AppFileName
        {
            get { return _appfilename; }
            set { _appfilename = value; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string AppDescription
        {
            get { return _appdescription; }
            set { _appdescription = value; }
        }
        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownLoadUrl
        {
            get { return _downloadurl; }
            set { _downloadurl = value; }
        }

        
    }
}
