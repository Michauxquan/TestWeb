using OWZX.Core;
using OWZX.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Services
{
    public class AdminBaseInfo
    {
        /// <summary>
        /// 获取基本信息
        /// </summary>
        /// <returns></returns>
        public static List<BaseInfoModel> GetBaseInfoList(int baseid = -1, string condition = "")
        {
            DataTable dt = OWZX.Data.AdminBaseInfo.GetBaseInfoList(baseid, condition);
            return (List<BaseInfoModel>)ModelConvertHelper<BaseInfoModel>.ConvertToModel(dt);
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
            return OWZX.Data.AdminBaseInfo.UpdateBaseInfo(baseid, title, content);
        }

        /// <summary>
        /// 获取基础类型
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static List<BaseTypeModel> GetBaseTypeList(string condition = "")
        {
            DataTable dt = OWZX.Data.AdminBaseInfo.GetBaseTypeList(condition);
            return (List<BaseTypeModel>)ModelConvertHelper<BaseTypeModel>.ConvertToModel(dt);
        }
        /// <summary>
        /// 验证类型是否已存在
        /// </summary>
        /// <param name="parentid"></param>
        /// <param name="type"></param>
        /// <param name="systypeid"></param>
        /// <returns></returns>
        public static bool GetBaseTypeByParentId(int parentid, string type, int systypeid = -1)
        {
            List<BaseTypeModel> baselist = GetBaseTypeList();
            foreach (BaseTypeModel basett in baselist)
            {
                if (systypeid != -1)
                {
                    if (basett.Parentid == parentid && basett.Type == type && basett.Systypeid != systypeid)
                    {
                        return true;
                    }
                }
                else
                {
                    if (basett.Parentid == parentid && basett.Type == type)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 添加基础类型
        /// </summary>
        /// <param name="basetype"></param>
        /// <returns></returns>
        public static bool AddBaseType(BaseTypeModel basetype)
        {
            return OWZX.Data.AdminBaseInfo.AddBaseType(basetype);
        }

        /// <summary>
        /// 修改基础类型
        /// </summary>
        /// <param name="basetype"></param>
        /// <returns></returns>
        public static bool UpdateBaseType(BaseTypeModel basetype)
        {
            return OWZX.Data.AdminBaseInfo.UpdateBaseType(basetype);
        }

        /// <summary>
        /// 删除基础类型
        /// </summary>
        /// <param name="basetype"></param>
        /// <returns></returns>
        public static bool DeleteBaseType(int systypeid)
        {
            return OWZX.Data.AdminBaseInfo.DeleteBaseType(systypeid);
        }
    }
}
