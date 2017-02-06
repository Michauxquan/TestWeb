using OWZX.Core;
using OWZX.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Data
{
    public class AdminBaseInfo
    {
        
        /// <summary>
        /// 获取基本信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetBaseInfoList(int baseid = -1, string condition = "")
       {
           return OWZX.Core.BSPData.RDBS.GetBaseInfoList(baseid,condition);
       }
        /// <summary>
       /// 修改基本信息
        /// </summary>
        /// <param name="baseid"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
       public static bool UpdateBaseInfo(int baseid, string title, string content)
       {
           return OWZX.Core.BSPData.RDBS.UpdateBaseInfo(baseid, title, content);
       }


       /// <summary>
       /// 获取基础类型
       /// </summary>
       /// <param name="condition"></param>
       /// <returns></returns>
       public static DataTable GetBaseTypeList(string condition = "") {
           return OWZX.Core.BSPData.RDBS.GetBaseTypeList(condition);
       }
       /// <summary>
       /// 添加基础类型
       /// </summary>
       /// <param name="basetype"></param>
       /// <returns></returns>
       public static bool AddBaseType(BaseTypeModel basetype)
       {
           return OWZX.Core.BSPData.RDBS.AddBaseType(basetype);
       }

       /// <summary>
       /// 修改基础类型
       /// </summary>
       /// <param name="basetype"></param>
       /// <returns></returns>
       public static bool UpdateBaseType(BaseTypeModel basetype)
       {
           return OWZX.Core.BSPData.RDBS.UpdateBaseType(basetype);
       }

       /// <summary>
       /// 删除基础类型
       /// </summary>
       /// <param name="basetype"></param>
       /// <returns></returns>
       public static bool DeleteBaseType(int systypeid)
       {
           return OWZX.Core.BSPData.RDBS.DeleteBaseType(systypeid);
       }
    }
}
