using System;
using System.ComponentModel.DataAnnotations;

namespace OWZX.Web.Framework
{
    /// <summary>
    /// 身份证验证属性
    /// </summary>
    public class IdCardAttribute : ValidationAttribute
    {
        public IdCardAttribute()
        {
            ErrorMessage = "不是有效的身份证号";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            else
            {
                return OWZX.Core.ValidateHelper.IsIdCard(value.ToString());
            }

        }
    }
}
