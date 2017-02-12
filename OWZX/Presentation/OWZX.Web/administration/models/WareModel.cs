using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using OWZX.Web.Framework;

namespace OWZX.Web.Admin.models
{
    /// <summary>
    /// 商品列表模型类
    /// </summary>
    public class WareListModel
    {
        /// <summary>
        /// 分页对象
        /// </summary>
        public PageModel PageModel { get; set; }
        /// <summary>
        /// 商品列表
        /// </summary>
        public DataTable WareList { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string WareName { get; set; }
        /// <summary>
        /// 商品代码
        /// </summary>
        public string WareCode { get; set; }
        /// <summary>
        /// 商品类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 商品类型
        /// </summary>
        public List<SelectListItem> TypeList { get; set; }
    }
    public class WareModel : IValidatableObject
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        //[Required(ErrorMessage = "商品名称不能为空")]
        //[StringLength(20, ErrorMessage = "商品名称长度不能大于20")]
        public string WareName { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary> 
        //[StringLength(50, ErrorMessage = "商品编码长度不能大于4")]
        public string WareCode { get; set; }
        /// <summary>
        /// 单价
        /// </summary> 
        public decimal Price { get; set; }

        /// <summary>
        /// 状态
        /// </summary> 
        //[Range(1, int.MaxValue, ErrorMessage = "请选择正确的商品类型")]
        //[DisplayName("状态")]
        public int Status { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        //[Range(1, int.MaxValue, ErrorMessage = "请选择正确的商品类型")]
        //[DisplayName("类型")]
        public int Type { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        [StringLength(150, ErrorMessage = "图片路径不能大于150")]
        public string ImgSrc { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errorList = new List<ValidationResult>();

            return errorList;
        }
    }

    public class SkuModel : IValidatableObject
    {
        /// <summary>
        /// 商品规格
        /// </summary>
        //[Required(ErrorMessage = "商品规格不能为空")]
        //[StringLength(20, ErrorMessage = "商品规格长度不能大于20")]
        public string WareCode { get; set; }
        /// <summary>
        /// 规格名称
        /// </summary>
        //[Required(ErrorMessage = "规格名称不能为空")]
        //[StringLength(20, ErrorMessage = "规格名称长度不能大于20")]
        public string SpecName { get; set; }
        /// <summary>
        /// 规格编码
        /// </summary> 
        //[StringLength(50, ErrorMessage = "规格编码长度不能大于4")]
        public string SpecCode { get; set; }
        /// <summary>
        /// 单价
        /// </summary> 
        public decimal Price { get; set; }

        /// <summary>
        /// 状态
        /// </summary> 
        //[Range(1, int.MaxValue, ErrorMessage = "请选择正确的商品类型")]
        //[DisplayName("状态")]
        public int Status { get; set; }
        /// <summary>
        /// 状态
        /// </summary>  
        public int UserNum { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        [StringLength(150, ErrorMessage = "图片路径不能大于150")]
        public string ImgSrc { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errorList = new List<ValidationResult>();

            return errorList;
        }
    }
}
