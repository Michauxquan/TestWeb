using OWZX.Model;
using OWZX.Web.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OWZX.Web.Admin.Models
{
    public class UserRemitList
    {
        public string Account { get; set; }
        public string type { get; set; }
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }

        /// <summary>
        /// 转账列表
        /// </summary>
        public List<MD_Remit> RemitList { get; set; }
    }

    public class UserRemit : IValidatableObject
    {
        private string mobile;
        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = "手机号不能为空")]
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value.TrimEnd(); }
        }
        private int uid;
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        private string type;
        /// <summary>
        /// 转账方式
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value.TrimEnd(); }
        }

        private string name;
        /// <summary>
        ///汇款用户昵称 
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value.TrimEnd(); }
        }

        private string account;
        /// <summary>
        /// 汇款账号
        /// </summary>
        public string Account
        {
            get { return account; }
            set { account = value.TrimEnd(); }
        }

        private int money;
        /// <summary>
        ///汇款金额 
        /// </summary>
        [Required(ErrorMessage = "汇款金额为空")]
        [Range(10, int.MaxValue, ErrorMessage = "汇款金额无效")]
        public int Money
        {
            get { return money; }
            set { money = value; }
        }
        private int realmoney;
        /// <summary>
        ///实际汇款金额 
        /// </summary>
        public int RealMoney
        {
            get { return realmoney; }
            set { realmoney = value; }
        }

        private string bankname;
        /// <summary>
        /// 开户银行
        /// </summary>
        public string Bankname
        {
            get { return bankname; }
            set { bankname = value.TrimEnd(); }
        }

        private short status;
        /// <summary>
        /// 状态 0 待审核 1 审核中 2 已审核 3 审核失败
        /// </summary>
        public short Status
        {
            get { return status; }
            set { status = value; }
        }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string State
        {
            get;
            set;
        }

        private string remark;
        /// <summary>
        /// 失败原因
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value.TrimEnd(); }
        }

        private DateTime addtime;
        public DateTime Addtime
        {
            get { return addtime; }
            set { addtime = value; }
        }

        private int updateuid;
        public int Updateuid
        {
            get { return updateuid; }
            set { updateuid = value; }
        }

        private DateTime updatetime;
        public DateTime Updatetime
        {
            get { return updatetime; }
            set { updatetime = value; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errorList = new List<ValidationResult>();
            Regex reg = new Regex("^(13|15|17|18)[0-9]{9}$");
            if (!reg.IsMatch(Mobile))
            {
                errorList.Add(new ValidationResult("手机号格式错误", new string[] { "Mobile" }));
            }

            //if (!SecureHelper.IsSafeSqlString(Email))
            //{
            //    errorList.Add(new ValidationResult("邮箱名中包含不安全的字符,请删除!", new string[] { "Email" }));
            //}

            return errorList;
        }
    }
}
