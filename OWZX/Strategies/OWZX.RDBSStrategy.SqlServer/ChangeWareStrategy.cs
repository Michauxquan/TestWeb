using OWZX.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWZX.Model;

namespace OWZX.RDBSStrategy.SqlServer
{
    /// <summary>
    /// SqlServer策略之新的兑换夺宝
    /// </summary>
    public partial class RDBSStrategy : IRDBSStrategy
    {
        #region
        /// <summary>
        ///获取商品记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetWareList(int pageNumber, int pageSize, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };
            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
  drop table #list

SELECT ROW_NUMBER() over(order by wareid desc) id
      ,[wareid],[warecode],[warename],[status],[type],[price],[imgsrc]
  into  #list
  FROM owzx_ware a where  1=1 
  {0}

declare @total int
select @total=(select count(1)  from #list)

if(@pagesize=-1)
begin
select *,@total TotalCount from #list
end
else
begin
select *,@total TotalCount from #list where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end

end try
begin catch
select ERROR_MESSAGE() state
end catch

", condition
            );

            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, parms)[0];
        }

        /// <summary>
        ///获取商品sku记录(分页)
        /// </summary> 
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetWareSkuList(int pageIndex, int pageSize, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageIndex)
                                  };
            string commandText = string.Format(@"
begin try
 
SELECT ROW_NUMBER() over(order by specid desc) id
      ,b.[specid],a.[warecode],a.[warename],b.[status],a.[type],
    b.[price],a.[imgsrc],b.[speccode],b.[specname],b.usernum 
   into  #list
  FROM owzx_ware a  join owzx_waresku b on a.warecode=b.warecode where 1=1 
  {0} 
declare @total int
select @total=(select count(1)  from #list)

if(@pagesize=-1)
begin
select *,@total TotalCount from #list
end
else
begin
select *,@total TotalCount from #list where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end

end try
begin catch
select ERROR_MESSAGE() state
end catch

", condition
            );

            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, parms)[0];
        }
        /// <summary>
        ///获取商品sku记录
        /// </summary> 
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetWareSkuList(string condition = "")
        {

            string commandText = string.Format(@"
begin try
 
SELECT ROW_NUMBER() over(order by specid desc) id
      ,b.[specid],a.[warecode],a.[warename],b.[status],a.[type],
    b.[price],a.[imgsrc],b.[speccode],b.[specname],b.usernum 
 
  FROM owzx_ware a  join owzx_waresku b on a.warecode=b.warecode where 1=1 
  {0} 
end try
begin catch
select ERROR_MESSAGE() state
end catch

", condition
            );

            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, null)[0];
        }

        public DataTable GetWareByCode(string condition = "")
        {
            string commandText = string.Format(@" SELECT top 1 *   FROM owzx_ware  where  warecode='{0}' ", condition);
            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, null)[0];
        }

        public DataTable GetWareSkuByID(int specid = -1)
        {
            string commandText = string.Format(@" SELECT top 1 *   FROM owzx_waresku  where  specid={0} ", specid);
            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, null)[0];
        }
        public DataTable GetWareSkuByCode(string speccode = "")
        {
            string commandText = string.Format(@" SELECT top 1 *   FROM owzx_waresku  where  speccode='{0}' ", speccode);
            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, null)[0];
        }
        public DataTable GetChangeWare(int pageNumber, int pageSize, string condition = "")
        {
            DbParameter[] parms =
            {
                GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
            };
            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
  drop table #list

SELECT ROW_NUMBER() over(order by changeid desc) id
      ,a.changeid,a.issuenum,a.warecode ,a.warename,a.speccode,a.specname,a.status,a.price,a.usernum,a.totalfee,a.playnum,a.type
  into  #list
  FROM owzx_changeware a where status=0 
  {0}

declare @total int
select @total=(select count(1)  from #list)

if(@pagesize=-1)
begin
select *,@total TotalCount from #list
end
else
begin
select *,@total TotalCount from #list where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end

end try
begin catch
select ERROR_MESSAGE() state
end catch

", condition
            );
            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, parms)[0];
        }

        public DataTable GetUserOrder(int pageNumber, int pageSize, string condition = "")
        {
            DbParameter[] parms =
            {
                GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
            };
            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
  drop table #list

SELECT ROW_NUMBER() over(order by orderid desc) id
      ,a.orderid,a.ordercode,a.warecode,a.warename,a.speccode,a.specname,a.issuenum,a.status,
       a.totalfee,a.price,a.type,a.num,a.content,a.changeid,a.createtime ,rtrim(b.nickname) nickname,
        c.winname,c.winnum,c.usernum,c.playnum 
  into  #list
  FROM owzx_userorder a join  owzx_users b  on a.userid=b.uid left join owzx_changeware c  on c.changeid =a.changeid where  1=1 
  {0}

declare @total int
select @total=(select count(1)  from #list)

if(@pagesize=-1)
begin
    select *,@total TotalCount from #list
end
else
begin
    select *,@total TotalCount from #list where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end

end try
begin catch
select ERROR_MESSAGE() state
end catch

", condition
            );
            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, parms)[0];
        }
        /// <summary>
        /// 添加投注或兑换订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public string AddUserOrder(MD_UserOrder order)
        {

            string commandText = string.Format(@"
begin try

declare @usernum int=0, @oldnum int=0,@index int=0,@status int =0,@msg varchar(300)='',@userid int=0,@error int=0

if({10}=1)
begin 
    select @usernum=usernum,@oldnum=playnum,@status=status from owzx_changeware where changeid='{1}'   
    if( @status>0)
    begin
        select '夺宝信息已变更,请刷新信息后在购买' state
    end
    else if(@usernum>=({3}+@oldnum))
    begin
        begin tran
            while @index<{3}
            begin
                update owzx_changeware set playnum=playnum+1 where changeid='{1}' 
                select @usernum=usernum,@oldnum=playnum,@status=status from owzx_changeware where changeid='{1}' 
                if(@status=0)
                begin
                    declare @content varchar(100)='',@id int=0
                    select @content='{0}'+ right('000000'+cast( @oldnum as varchar),4)+',',@id=isnull(orderid,0),@status=isnull(status,0) from owzx_userorder where ordercode='{2}' and issuenum='{0}'
                    if(@status>0)
                    begin
                        select @error=1 ,  @msg='订单状态不正确,购买失败'
                        break;
                    end
                    else if(@id=0)
                    begin
                        select @userid=uid from owzx_users where rtrim(mobile)='{11}'
                        INSERT INTO owzx_userorder ([userid],[ordercode],[warecode],[warename],[speccode],[specname],[issuenum],[totalfee]
                           ,[status] ,[price],[type],[num] ,[content],[changeid])
                        VALUES( @userid,'{2}' ,'{7}' ,'{8}','{5}','{6}','{0}',{9},0,{4},{10},{3},@content,{1})
                    end 
                    else if(@id>0)
                    begin
                            DECLARE @ptrval binary(16) SELECT @ptrval = TEXTPTR([content]) from owzx_userorder where orderid=@id  UPDATETEXT owzx_userorder.[content] @ptrval null 0 @content  
                    end
                    if(@usernum=@oldnum)
                    begin
                        update owzx_changeware set status=1 where changeid='{1}'  
                        update owzx_userorder set status=1 where changeid='{1}' and status=0 and type={10} and issuenum='{0}' 
                    end
                    if( @oldnum>@usernum)
                    begin
                        select @error=1 ,  @msg='购买人次已超,请刷新信息后在购买' 
                        break;
                    end
                end
                set @index=@index+1
            end
        if (@@error<>0 or @error>0)
        begin
            select @msg state
            Rollback Tran
        end
        else
        begin 
            select '' state
            commit tran    
        end
    end
    else
        select '购买人次已超,请刷新信息后在购买' state
end
else if({10}=0)
begin
    if((select count(1) from owzx_ware a join owzx_waresku b on a.warecode=b.warecode where a.status=0 and b.status=0)>0)
    begin
        INSERT INTO owzx_userorder ([userid],[ordercode],[warecode],[warename],[speccode],[specname],[issuenum],[totalfee]
            ,[status] ,[price],[type],[num] ,[content],[changeid])
        VALUES( @userid,'{2}' ,'{7}' ,'{8}','{5}','{6}','{0}',{9},0,{4},{10},{3},@content,{1})
     end
     else
    begin
         select '商品已下架' state
    end
end
end try
begin catch
    select ERROR_MESSAGE() state
end catch

", order.IssueNum, order.ChangeID, order.OrderCode, order.Num, order.Price, order.SpecCode, order.SpecName
, order.WareCode, order.WareName, order.TotalFee, order.Type, order.Account);
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText).ToString();
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="ware"></param>
        /// <returns></returns>
        public int CreateWare(Ware ware)
        {
            string commandText = string.Format(@" 
begin try
    if((select count(1) from owzx_ware where warecode='{1}')=0)
    begin
        INSERT INTO owzx_ware ([warecode],[warename],[Status],[price],[imgsrc],[type])
        VALUES( '{1}','{2}',{5},{3},'{0}',{4})
        select @@identity state
    end
    else
    begin
            select -1 state
    end
 
end try
begin catch
    select -1 state
end catch

", ware.ImgSrc, ware.WareCode, ware.WareName, ware.Price, ware.Type, ware.Status);
            return Convert.ToInt32(RDBSHelper.ExecuteScalar(CommandType.Text, commandText));
        }
        /// <summary>
        /// 添加规格
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        public int CreateWareSku(Sku sku)
        {
            string commandText = string.Format(@" 
begin try
    if((select count(1) from owzx_waresku where speccode='{1}')=0)
    begin
        INSERT INTO owzx_waresku ([warecode],[specname],[Status],[price],[speccode],[usernum])
        VALUES( '{1}','{2}',{6},{4},'{3}',{5})
        select @@identity state
    end
    else
    begin
            select -1 state
    end
 
end try
begin catch
    select -1 state
end catch

", sku.ImgSrc, sku.WareCode, sku.SpecName, sku.SpecCode, sku.Price, sku.UserNum, sku.Status);
            return Convert.ToInt32(RDBSHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        public int UpdateWare(Ware ware)
        {
            string commandText = string.Format(@" 
begin try
    if((select count(1) from owzx_ware where warecode='{1}')=1)
    begin
       update owzx_ware set [warename]= '{2}',[Status]={4},[imgsrc]='{0}',[type]={3} where warecode='{1}'
        select @@rowcount state
    end
    else
    begin
            select -1 state
    end
 
end try
begin catch
    select -1 state
end catch

", ware.ImgSrc, ware.WareCode, ware.WareName, ware.Type, ware.Status);
            return Convert.ToInt32(RDBSHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        public int UpdateWareSku(Sku sku)
        {
            string commandText = string.Format(@" 
begin try
    if((select count(1) from owzx_waresku where speccode='{1}')=1)
    begin
       update owzx_waresku set [specname]= '{2}',[Status]={4},[price]={0},[usernum]={3} where speccode='{1}' and warecode='{5}'
        select @@rowcount state
    end
    else
    begin
            select -1 state
    end
 
end try
begin catch
    select -1 state
end catch

", sku.Price, sku.SpecCode, sku.SpecName, sku.UserNum, sku.Status, sku.WareCode);
            return Convert.ToInt32(RDBSHelper.ExecuteScalar(CommandType.Text, commandText));
        }
        public int UpdateWareStatus(string warecode, int status)
        {
            string commandText = string.Format(@" 
begin try
    if((select count(1) from owzx_ware where warecode='{1}')=1)
    begin
       update owzx_ware  set  [Status]={0}  where   warecode='{1}'
        select @@rowcount state
    end
    else
    begin
            select -1 state
    end
end try
begin catch
    select -1 state
end catch

", status, warecode);
            return Convert.ToInt32(RDBSHelper.ExecuteScalar(CommandType.Text, commandText));
        }

        public int UpdateWareSkuStatus(int specid, int status)
        {
            string commandText = string.Format(@" 
begin try
    if((select count(1) from owzx_waresku where specid={1})=1)
    begin
       update owzx_waresku  set  [Status]={0}  where   specid={1}
        select @@rowcount state
    end
    else
    begin
            select -1 state
    end
end try
begin catch
    select -1 state
end catch

", status, specid);
            return Convert.ToInt32(RDBSHelper.ExecuteScalar(CommandType.Text, commandText));
        }
        #endregion
    }
}
