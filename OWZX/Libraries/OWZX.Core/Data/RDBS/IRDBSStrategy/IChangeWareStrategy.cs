using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWZX.Model;

namespace OWZX.Core
{
    /// <summary>
    /// 兑换夺宝商品
    /// </summary>
    public partial interface IRDBSStrategy
    {
        DataTable GetWareList(int pageNumber, int pageSize, string condition = "");
        DataTable GetWareSkuList(string condition = "");
        DataTable GetWareSkuList(int pageIndex, int pageSize, string condition = "");
        DataTable GetChangeWare(int pageNumber, int pageSize, string condition = "");
        DataTable GetUserOrder(int pageNumber, int pageSize, string condition = "");
        DataTable GetUserOrderDetail(string condition = "");
        DataTable GetWareByCode(string condition = "");
        DataTable GetWareSkuByCode(string speccode = "");
        DataTable GetWareSkuByID(int specid = -1);
        string AddUserOrder(MD_UserOrder order);
        int CreateWare(Ware ware);
        int CreateWareSku(Sku sku);
        int UpdateWare(Ware ware);
        int UpdateWareSku(Sku sku);
        int UpdateWareStatus(string warecode, int status);
        int UpdateWareSkuStatus(int specid, int status);
        bool UpdateOrderStatus(string ordercode, int status);
    }
}
