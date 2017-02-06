using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using OWZX.Core;

namespace OWZX.Services
{
    /// <summary>
    /// 管理员组操作管理类
    /// </summary>
    public partial class AdminGroups
    {
        //后台导航菜单栏缓存文件夹
        private const string AdminNavMeunCacheFolder = "/administration/menu";

        /// <summary>
        /// 检查当前动作的授权
        /// </summary>
        /// <param name="adminGid">管理员组id</param>
        /// <param name="controller">控制器名称</param>
        /// <param name="action">动作方法名称</param>
        /// <returns></returns>
        public static bool CheckAuthority(int adminGid, string controller, string pageKey)
        {
            //非管理员
            if (adminGid == 1)
                return false;

            //系统管理员具有一切权限
            if (adminGid == 2)
                return true;

            HashSet<string> adminActionHashSet = AdminActions.GetAdminActionHashSet();
            HashSet<string> adminGroupActionHashSet = GetAdminGroupActionHashSet(adminGid);
            pageKey = pageKey.Substring(1, pageKey.Length-1);
            //动作方法的优先级大于控制器的优先级
            if ((adminActionHashSet.Contains(pageKey) && adminGroupActionHashSet.Contains(pageKey)) ||
                                    (adminActionHashSet.Contains(controller) && adminGroupActionHashSet.Contains(controller)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获得管理员组操作HashSet
        /// </summary>
        /// <param name="adminGid">管理员组id</param>
        /// <returns></returns>
        public static HashSet<string> GetAdminGroupActionHashSet(int adminGid)
        {
            HashSet<string> actionHashSet = OWZX.Core.BSPCache.Get(CacheKeys.SHOP_ADMINGROUP_ACTIONHASHSET + adminGid) as HashSet<string>;
            if (actionHashSet == null||actionHashSet.Count==0)
            {
                AdminGroupInfo adminGroupInfo = GetAdminGroupById(adminGid);
                if (adminGroupInfo != null)
                {
                    actionHashSet = new HashSet<string>();
                    string[] actionList = StringHelper.SplitString(adminGroupInfo.ActionList);//将动作列表字符串分隔成动作列表
                    foreach (string action in actionList)
                    {
                        actionHashSet.Add(action);
                    }
                    OWZX.Core.BSPCache.Insert(CacheKeys.SHOP_ADMINGROUP_ACTIONHASHSET + adminGid, actionHashSet);
                }
            }
            return actionHashSet;
        }

        /// <summary>
        /// 获得管理员组操作HashSet
        /// </summary>
        /// <param name="adminGid">管理员组id</param>
        /// <returns></returns>
        public static HashSet<string> GetAdminGroupActionHashSetNoCache(int adminGid)
        {
            HashSet<string> actionHashSet = new HashSet<string>();

            AdminGroupInfo adminGroupInfo = GetAdminGroupById(adminGid);
            if (adminGroupInfo != null)
            {
                actionHashSet = new HashSet<string>();
                string[] actionList = StringHelper.SplitString(adminGroupInfo.ActionList);//将动作列表字符串分隔成动作列表
                foreach (string action in actionList)
                {
                    actionHashSet.Add(action);
                }
            }
            return actionHashSet;
        }

        /// <summary>
        /// 获得管理员组列表
        /// </summary>
        /// <returns></returns>
        public static AdminGroupInfo[] GetAdminGroupList()
        {
            AdminGroupInfo[] adminGroupList = OWZX.Core.BSPCache.Get(CacheKeys.SHOP_ADMINGROUP_LIST) as AdminGroupInfo[];
            if (adminGroupList == null||adminGroupList.Length==0)
            {
                adminGroupList = OWZX.Data.AdminGroups.GetAdminGroupList();
                OWZX.Core.BSPCache.Insert(CacheKeys.SHOP_ADMINGROUP_LIST, adminGroupList);
            }
            return adminGroupList;
        }


        /// <summary>
        /// 获得用户级管理员组列表
        /// </summary>
        /// <returns></returns>
        public static AdminGroupInfo[] GetCustomerAdminGroupList()
        {
            AdminGroupInfo[] adminGroupList = OWZX.Data.AdminGroups.GetAdminGroupList();
            if (adminGroupList.Length == 0)
                return new AdminGroupInfo[0];
            AdminGroupInfo[] customerAdminGroupList = new AdminGroupInfo[adminGroupList.Length - 2];

            int i = 0;
            foreach (AdminGroupInfo adminGroupInfo in adminGroupList)
            {
                if (adminGroupInfo.AdminGid > 2)
                {
                    customerAdminGroupList[i] = adminGroupInfo;
                    i++;
                }
            }

            return customerAdminGroupList;
        }

        /// <summary>
        /// 获得管理员组
        /// </summary>
        /// <param name="adminGid">管理员组id</param>
        /// <returns></returns>
        public static AdminGroupInfo GetAdminGroupById(int adminGid)
        {
            foreach (AdminGroupInfo adminGroupInfo in GetAdminGroupList())
            {
                if (adminGid == adminGroupInfo.AdminGid)
                    return adminGroupInfo;
            }
            return null;
        }


        /// <summary>
        /// 获得管理员组id
        /// </summary>
        /// <param name="title">管理员组标题</param>
        /// <param name="depid">部门id</param>
        /// <returns></returns>
        public static int GetAdminGroupIdByTitle(string title)//,int depid
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                foreach (AdminGroupInfo adminGroupInfo in GetAdminGroupList())
                {
                    if (adminGroupInfo.Title == title)//&& adminGroupInfo.DepId==depid
                        return adminGroupInfo.AdminGid;
                }
            }
            return -1;
        }

        /// <summary>
        /// 创建管理员组
        /// </summary>
        /// <param name="adminGroupInfo">管理员组信息</param>
        public static void CreateAdminGroup(AdminGroupInfo adminGroupInfo)
        {
            adminGroupInfo.ActionList = adminGroupInfo.ActionList.ToLower();
            int adminGid = OWZX.Data.AdminGroups.CreateAdminGroup(adminGroupInfo);
            if (adminGid > 0)
            {
                OWZX.Core.BSPCache.Remove(CacheKeys.SHOP_ADMINGROUP_LIST);
                adminGroupInfo.AdminGid = adminGid;
                WriteAdminNavMenuCache(adminGroupInfo);
            }
        }

        /// <summary>
        /// 删除管理员组
        /// </summary>
        /// <param name="adminGid">管理员组id</param>
        /// <returns>-2代表内置管理员不能删除，-1代表此管理员组下还有会员未删除，0代表删除失败，1代表删除成功</returns>
        public static int DeleteAdminGroupById(int adminGid)
        {
            if (adminGid < 3)
                return -2;

            if (AdminUsers.GetUserCountByAdminGid(adminGid) > 0)
                return -1;

            if (adminGid > 0)
            {
                OWZX.Data.AdminGroups.DeleteAdminGroupById(adminGid);
                OWZX.Core.BSPCache.Remove(CacheKeys.SHOP_ADMINGROUP_ACTIONHASHSET + adminGid);
                OWZX.Core.BSPCache.Remove(CacheKeys.SHOP_ADMINGROUP_LIST);
                File.Delete(IOHelper.GetMapPath(AdminNavMeunCacheFolder + "/" + adminGid + ".js"));
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新管理员组
        /// </summary>
        public static void UpdateAdminGroup(AdminGroupInfo adminGroupInfo)
        {
            adminGroupInfo.ActionList = adminGroupInfo.ActionList.ToLower();
            OWZX.Data.AdminGroups.UpdateAdminGroup(adminGroupInfo);
            OWZX.Core.BSPCache.Remove(CacheKeys.SHOP_ADMINGROUP_ACTIONHASHSET + adminGroupInfo.AdminGid);
            OWZX.Core.BSPCache.Remove(CacheKeys.SHOP_ADMINGROUP_LIST);
            WriteAdminNavMenuCache(adminGroupInfo);
        }

        /// <summary>
        /// 将管理员组的导航菜单栏缓存写入到文件中
        /// </summary>
        private static void WriteAdminNavMenuCache(AdminGroupInfo adminGroupInfo)
        {
            HashSet<string> adminGroupActionHashSet = new HashSet<string>();
            string[] actionList = StringHelper.SplitString(adminGroupInfo.ActionList);//将后台操作列表字符串分隔成后台操作列表
            foreach (string action in actionList)
            {
                adminGroupActionHashSet.Add(action);
            }

            bool flag = false;
            StringBuilder menu = new StringBuilder();
            StringBuilder menuList = new StringBuilder("var menuList = [");


            #region 用户管理

            flag = false;
            menu = menu.Clear();
            menu.AppendFormat("{0}\"title\":\"用户管理\",\"subMenuList\":[", "{");
            if (adminGroupActionHashSet.Contains("user_list"))
            {
                menu.AppendFormat("{0}\"title\":\"用户列表\",\"url\":\"/admin/user/list\"{1},", "{", "}");
                flag = true;
            }
            if (adminGroupActionHashSet.Contains("user_teamlist"))
            {
                menu.AppendFormat("{0}\"title\":\"团队列表\",\"url\":\"/admin/user/teamlist\"{1},", "{", "}");
                flag = true;
            }
            if (adminGroupActionHashSet.Contains("admingroup_list"))
            {
                menu.AppendFormat("{0}\"title\":\"管理员组\",\"url\":\"/admin/admingroup/list\"{1},", "{", "}");
                flag = true;
            }
            if (flag)
            {
                menu.Remove(menu.Length - 1, 1);
                menu.Append("]},");
                menuList.Append(menu.ToString());
            }

            #endregion

            #region 信息管理
            flag = false;
            menu = menu.Clear();
            menu.AppendFormat("{0}\"title\":\"信息管理\",\"subMenuList\":[", "{");
            if (adminGroupActionHashSet.Contains("suite_suiteslist"))
            {
                menu.AppendFormat("{0}\"title\":\"套餐列表\",\"url\":\"/admin/suite/suiteslist\"{1},", "{", "}");
                flag = true;
            }
            if (adminGroupActionHashSet.Contains("draw_drawlist"))
            {
                menu.AppendFormat("{0}\"title\":\"提现列表\",\"url\":\"/admin/draw/drawlist\"{1},", "{", "}");
                flag = true;
            }
            if (adminGroupActionHashSet.Contains("advice_advicelist"))
            {
                menu.AppendFormat("{0}\"title\":\"意见列表\",\"url\":\"/admin/advice/advicelist\"{1},", "{", "}");
                flag = true;
            }
            if (adminGroupActionHashSet.Contains("draw_flowlist"))
            {
                menu.AppendFormat("{0}\"title\":\"流量套餐\",\"url\":\"/admin/draw/flowlist\"{1},", "{", "}");
                flag = true;
            }
            

            if (flag)
            {
                menu.Remove(menu.Length - 1, 1);
                menu.Append("]},");
                menuList.Append(menu.ToString());
            }

            #endregion

            #region 财务报表
            flag = false;
            menu = menu.Clear();
            menu.AppendFormat("{0}\"title\":\"财务报表\",\"subMenuList\":[", "{");
            if (adminGroupActionHashSet.Contains("draw_rechargelist"))
            {
                menu.AppendFormat("{0}\"title\":\"充值列表\",\"url\":\"/admin/draw/rechargelist\"{1},", "{", "}");
                flag = true;
            }
            if (adminGroupActionHashSet.Contains("draw_userincome"))
            {
                menu.AppendFormat("{0}\"title\":\"用户收益\",\"url\":\"/admin/draw/userincome\"{1},", "{", "}");
                flag = true;
            }
            if (adminGroupActionHashSet.Contains("draw_callinfo"))
            {
                menu.AppendFormat("{0}\"title\":\"话费清单\",\"url\":\"/admin/draw/callinfo\"{1},", "{", "}");
                flag = true;
            }
            if (flag)
            {
                menu.Remove(menu.Length - 1, 1);
                menu.Append("]},");
                menuList.Append(menu.ToString());
            }
            #endregion

            #region 基础信息
            flag = false;
            menu = menu.Clear();
            menu.AppendFormat("{0}\"title\":\"基础信息\",\"subMenuList\":[", "{");
            if (adminGroupActionHashSet.Contains("baseinfo_basetypelist"))
            {
                menu.AppendFormat("{0}\"title\":\"基础类型\",\"url\":\"/admin/baseinfo/basetypelist\"{1},", "{", "}");
                flag = true;
            }
            if (adminGroupActionHashSet.Contains("baseinfo_baseinfolist"))
            {
                menu.AppendFormat("{0}\"title\":\"基础资料\",\"url\":\"/admin/baseinfo/baseinfolist\"{1},", "{", "}");
                flag = true;
            }
            if (adminGroupActionHashSet.Contains("baseinfo_posterbaseinfo"))
            {
                menu.AppendFormat("{0}\"title\":\"海报信息\",\"url\":\"/admin/baseinfo/posterbaseinfo\"{1},", "{", "}");
                flag = true;
            }
            if (adminGroupActionHashSet.Contains("baseinfo_positionlist"))
            {
                menu.AppendFormat("{0}\"title\":\"职位列表\",\"url\":\"/admin/baseinfo/positionlist\"{1},", "{", "}");
                flag = true;
            }
                                       
            if (flag)
            {
                menu.Remove(menu.Length - 1, 1);
                menu.Append("]},");
                menuList.Append(menu.ToString());
            }
            #endregion

            if (menuList.Length > 16) //没有选择任何模块
                menuList.Remove(menuList.Length - 1, 1);
            menuList.Append("]");

            try
            {
                string fileName = IOHelper.GetMapPath(AdminNavMeunCacheFolder + "/" + adminGroupInfo.AdminGid + ".js");
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    Byte[] info = System.Text.Encoding.UTF8.GetBytes(menuList.ToString());
                    fs.Write(info, 0, info.Length);
                    fs.Flush();
                    fs.Close();
                }
            }
            catch
            { }
        }
    }
}
