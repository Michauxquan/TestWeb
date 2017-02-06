using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Core.Helper
{
   public class ExcelHelper
    {
       /// <summary>
        /// 构建table
       /// </summary>
       /// <param name="dt">数据源</param>
        /// <param name="listcol">列名称及dt中字段</param>
       /// <returns></returns>
       public static string BuildHtml(DataTable dt, Dictionary<string, string> listcol)
       {
           var sbHtml = new StringBuilder();
           sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
           sbHtml.Append("<tr>");

           foreach (var item in listcol)
           {
               sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", item.Key);
           }
           sbHtml.Append("</tr>");

           foreach (DataRow row in dt.Rows)
           {
               sbHtml.Append("<tr>");
               foreach (var item in listcol)
               {
                   sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", row[item.Value].ToString());
               }
               sbHtml.Append("</tr>");
           }
           sbHtml.Append("</table>");
           return sbHtml.ToString();
       }
    }
}
