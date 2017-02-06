using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Core
{
    /// <summary>    
    /// 实体转换辅助类    
    /// </summary>    
    public class ModelConvertHelper<T> where T : new()
    {
        public static IList<T> ConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<T> ts = new List<T>();

            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列    

                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

        /// <summary>
        /// DataTable转T
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T DataTableToModel(DataTable dt)
        {
            T t = new T();
            foreach (DataRow row in dt.Rows)
            {
                foreach (var item in t.GetType().GetProperties())
                {
                    if (row.Table.Columns.Contains(item.Name))
                    {
                        if (DBNull.Value != row[item.Name])
                        {
                            item.SetValue(t, Convert.ChangeType(row[item.Name], item.PropertyType), null);
                        }

                    }
                }
            }
            return t;
        }
    }    
}
