using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWZX.Core;
using OWZX.Model;

namespace OWZX.Services
{
    public partial class ChangeWare
    {
        #region 商品记录
        /// <summary>
        ///获取商品记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="type"> </param>
        /// <param name="condition"> type=1夺宝商品 2兑换商品  and 链接</param>
        /// <returns></returns>
        public static DataTable GetWareList(int pageNumber, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetWareList(pageNumber, pageSize, condition);
        }


        /// <summary>
        ///获取商品Sku信息(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public static List<MD_Ware> GetWareSkuList(int pageIndex, int pageSize, string condition = "")
        {
            DataTable dt = OWZX.Core.BSPData.RDBS.GetWareSkuList(pageIndex, pageSize, condition);
            List<MD_Ware> list = (List<MD_Ware>)ModelConvertHelper<MD_Ware>.ConvertToModel(dt);
            return list;
        }
        /// <summary>
        ///获取商品规格带商品信息
        /// </summary> 
        /// <param name="warecode">商品code</param> 
        /// <returns></returns>
        public static DataTable GetWareSkuList(string warecode = "")
        {
            return OWZX.Core.BSPData.RDBS.GetWareSkuList(warecode);
        }
        /// <summary>
        ///根据商品编码获取商品 
        /// </summary> 
        /// <param name="warecode">商品code</param> 
        /// <returns></returns> 
        public static Ware GetWareByCode(string warecode = "")
        {
            Ware wareInfo = null;
            DataTable dt = OWZX.Core.BSPData.RDBS.GetWareByCode(warecode);
            wareInfo = OWZX.Core.ModelConvertHelper<Ware>.DataTableToModel(dt);
            return wareInfo;
        }

        /// <summary>
        ///根据商品编码获取商品 
        /// </summary> 
        /// <param name="warecode">规格code</param> 
        /// <returns></returns> 
        public static Sku GetWareSkuByCode(string speccode = "")
        {
            Sku skuInfo = null;
            DataTable dt = OWZX.Core.BSPData.RDBS.GetWareSkuByCode(speccode);
            skuInfo = OWZX.Core.ModelConvertHelper<Sku>.DataTableToModel(dt);
            return skuInfo;
        }
        /// <summary>
        ///根据ID获取规格信息
        /// </summary> 
        /// <param name="warecode">规格Id</param> 
        /// <returns></returns> 
        public static Sku GetWareSkuByID(int specid = -1)
        {
            Sku skuInfo = null;
            DataTable dt = OWZX.Core.BSPData.RDBS.GetWareSkuByID(specid);
            skuInfo = OWZX.Core.ModelConvertHelper<Sku>.DataTableToModel(dt);
            return skuInfo;
        }
        /// <summary>
        /// 获取夺宝期号
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable GetChangeWare(int pageNumber, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetChangeWare(pageNumber, pageSize, condition);
        }
        /// <summary>
        /// 用户创建多宝订单 以及兑换订单 区分 order.Type=1 夺宝 0:兑换
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string AddUserOrder(MD_UserOrder order)
        {
            return OWZX.Core.BSPData.RDBS.AddUserOrder(order);
        }
        /// <summary>
        /// 获取用户投注/兑换记录 区分 order.Type=1 夺宝 0:兑换
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static DataTable GetUserOrder(int pageNumber, int pageSize, string condition = "")
        {
            return OWZX.Core.BSPData.RDBS.GetUserOrder(pageNumber, pageSize, condition);
        }

        public static int CreateWare(Ware ware)
        {
            return OWZX.Core.BSPData.RDBS.CreateWare(ware);
        }

        public static int CreateWareSku(Sku sku)
        {
            return OWZX.Core.BSPData.RDBS.CreateWareSku(sku);
        }

        public static bool UpdateWare(Ware ware)
        {
            return OWZX.Core.BSPData.RDBS.UpdateWare(ware) > 0;
        }

        public static bool UpdateWareSku(Sku sku)
        {
            return OWZX.Core.BSPData.RDBS.UpdateWareSku(sku) > 0;
        }

        public static bool UpdateWareStatus(string warecode, int status)
        {
            return OWZX.Core.BSPData.RDBS.UpdateWareStatus(warecode, status) > 0;
        }
        public static bool UpdateWareSkuStatus(int specid, int status)
        {
            return OWZX.Core.BSPData.RDBS.UpdateWareSkuStatus(specid, status) > 0;
        }
        #endregion
    }
}
