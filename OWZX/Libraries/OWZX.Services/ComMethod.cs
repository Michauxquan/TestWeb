using OWZX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Services
{
   public class ComMethod
    {
       /// <summary>
       /// 验证imei
       /// </summary>
       /// <param name="account"></param>
       /// <param name="imei"></param>
       /// <returns></returns>
       public static string ValidateIMEI(string account,string imei)
       {
           PartUserInfo partUserInfo = Users.GetPartUserByMobile(account);

           if (partUserInfo.Uid <= 0)
               return "账号不存在";
           if (partUserInfo.IMEI.ToLower() != imei.TrimEnd().ToLower())
           {
               return "您的账号已在其他手机登录,您被迫下线";
           }
           return "";
       }
    }
}
