using System;
using System.Data;
using System.Collections.Generic;

using OWZX.Core;

namespace OWZX.Data
{
    /// <summary>
    /// 后台操作数据访问类
    /// </summary>
    public partial class AdminActions
    {
        /// <summary>
        /// 获得后台操作列表
        /// </summary>
        /// <returns></returns>
        public static List<AdminActionInfo> GetAdminActionList()
        {
            List<AdminActionInfo> adminActionList = new List<AdminActionInfo>();
            IDataReader reader = OWZX.Core.BSPData.RDBS.GetAdminActionList();
            while (reader.Read())
            {
                AdminActionInfo adminActionInfo = new AdminActionInfo();
                adminActionInfo.AdminAid = TypeHelper.ObjectToInt(reader["adminaid"]);
                adminActionInfo.Title = reader["title"].ToString();
                adminActionInfo.Action = reader["action"].ToString();
                adminActionInfo.ParentId = TypeHelper.ObjectToInt(reader["parentid"]);
                adminActionInfo.DisplayOrder = TypeHelper.ObjectToInt(reader["displayorder"]);
                adminActionList.Add(adminActionInfo);
            }
            reader.Close();
            return adminActionList;
        }
        /// <summary>
        /// 获得后台操作列表
        /// </summary>
        /// <returns></returns>
        public static List<AdminActionInfo> GetAdminActions()
        {
            List<AdminActionInfo> adminActionList = new List<AdminActionInfo>();
            DataTable dt = OWZX.Core.BSPData.RDBS.GetAdminActions();

            adminActionList = (List<AdminActionInfo>)ModelConvertHelper<AdminActionInfo>.ConvertToModel(dt);

            adminActionList.ForEach((x) =>
            {
                x.ChildAction = adminActionList.FindAll(y => y.ParentId == x.AdminAid);
            });

            List<AdminActionInfo> adminAction = new List<AdminActionInfo>();
            adminAction = adminActionList.FindAll(x => x.ChildAction.Count > 0);
            return adminAction;
        }
    }
}
