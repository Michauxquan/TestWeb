using OWZX.Core;
using OWZX.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.EventStrategy.Timer
{
    public class DelLogEvent : IEvent
    {
        public void Execute(object eventInfo)
        {
            //执行日志删除
            EventInfo e = (EventInfo)eventInfo;
            DelLog(e.LogUrl);
            EventLogs.CreateEventLog(e.Key, e.Title, Environment.MachineName, DateTime.Now);
        }
        /// <summary>
        ///清除事件日志
        /// </summary>
        private void DelEventLog()
        {
            string sql = string.Format(" delete from owzx_eventlogs where executetime<CONVERT(varchar(25),dateadd(MINUTE,-10,getdate()),120)");
            RDBSHelper.ExecuteScalar(sql);
        }
        private void DelLog(string path)
        {
            DelEventLog();
            foreach (string name in path.Split(';'))
            {
                DeleteFile(name, "txt");
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="filetype">删除文件类型</param>
        public static void DeleteFile(string path, string filetype)
        {

            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] fi;
            if (di.Exists == false)
                return;
            if (!string.IsNullOrEmpty(filetype))
            {
                fi = di.GetFiles("*." + filetype);
            }
            else
            {
                fi = di.GetFiles();
            }
            try
            {
                if (fi.Length > 0)
                    Delete_File(fi);

                foreach (DirectoryInfo die in di.GetDirectories())
                {
                    DeleteFile(die.FullName, "txt");
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// 对比时间，删除与当前日期相差指定的天数的文件
        /// </summary>
        /// <param name="fi">文件集合</param>
        private static void Delete_File(FileInfo[] fi)
        {
            foreach (FileInfo tmpfi in fi)
            {
                TimeSpan ts = DateTime.Today.Subtract(tmpfi.LastWriteTime);
                if (ts.TotalDays > 7) //删除7天前的文件
                {
                    tmpfi.Delete();
                }
            }
        }
    }
}
