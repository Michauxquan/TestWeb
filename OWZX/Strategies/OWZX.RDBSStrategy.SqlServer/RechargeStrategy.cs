using OWZX.Core;
using recharge = OWZX.Model;
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
    /// SqlServer策略之充值记录分部类
    /// </summary>
    public partial class RDBSStrategy : IRDBSStrategy
    {
        #region 充值
        /// <summary>
        /// 添加充值记录
        /// </summary>
        /// <param name="rech"></param>
        /// <returns></returns>
        public string AddRecharge(recharge.RechargeModel rech)
        {
            DbParameter[] parms = {
                                       
                                        GenerateInParam("@vossuiteid", SqlDbType.Int, 4, rech.SuiteId),
                                        GenerateInParam("@out_trade_no", SqlDbType.VarChar, 50, rech.Out_trade_no),
                                        GenerateInParam("@account", SqlDbType.VarChar, 20, rech.Account),
                                        GenerateInParam("@platform", SqlDbType.VarChar, 10, rech.PlatForm),
                                        GenerateInParam("@type", SqlDbType.Int, 4, rech.Type),
                                        GenerateInParam("@role", SqlDbType.Int, 4, rech.Role)
                                    };
            string commandText = string.Format(@"
begin try

INSERT INTO [{0}rechargeinfo]([account],[vossuiteid],[out_trade_no],platform,type,role)
     VALUES (@account,@vossuiteid,@out_trade_no,@platform,@type,@role)
select '添加成功' state
end try
begin catch
select ERROR_MESSAGE() state
end catch
",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }
        /// <summary>
        /// 更新充值记录 
        /// </summary>
        /// <param name="rech"></param>
        /// <returns></returns>
        public string UpdateRecharge(recharge.RechargeModel rech)
        {

            return "";
        }

        /// <summary>
        /// 充值成功更新充值记录
        /// </summary>
        /// <param name="rech"></param>
        /// <returns></returns>
        public string UpdateRechargeForPay(recharge.RechargeModel rech)
        {
            DbParameter[] parms = {
                                      GenerateInParam("@out_trade_no", SqlDbType.VarChar, 50, rech.Out_trade_no),
                                      GenerateInParam("@trade_no", SqlDbType.VarChar, 50, rech.Trade_no),
                                      GenerateInParam("@paytime", SqlDbType.VarChar, 30, rech.Paytime),
                                      GenerateInParam("@total_fee", SqlDbType.Decimal, 25, rech.Total_fee)
                                    };
            string commandText = string.Format(@"
begin try
if exists(select 1 from [{0}rechargeinfo] where out_trade_no=@out_trade_no)
begin
UPDATE [{0}rechargeinfo]
   SET trade_no = @trade_no,paytime=@paytime,total_fee=@total_fee
where out_trade_no=@out_trade_no

select '修改成功' state
end
else
begin
select '充值记录已被删除' state
end
end try
begin catch
select ERROR_MESSAGE() state
end catch
",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }
        /// <summary>
        /// 删除充值记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteRecharge(string id)
        {
            string commandText = string.Format(@"
            begin try
            if exists(select 1 from [{0}rechargeinfo] where [id] in ({1}))
            begin
            delete from [{0}rechargeinfo] where [id] in ({1})
            select '删除成功' state
            end
            else
            begin
            select '充值记录已被删除' state
            end
            end try
            begin catch
            select ERROR_MESSAGE() state
            end catch
            ",
             RDBSHelper.RDBSTablePre, id);
            return RDBSHelper.ExecuteScalar(commandText).ToString();
        }


        /// <summary>
        ///获取充值记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetRechargeList(int pageNumber, int pageSize, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };


            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
  drop table #list

SELECT ROW_NUMBER() over(order by a.addtime desc) id
      ,a.id rechargeid
      ,a.[account]
      ,a.[vossuiteid] as suiteid
      ,a.[trade_no]
      ,a.[out_trade_no]
      ,a.[paytime]
      ,a.[total_fee]
      ,a.[platform]
      ,a.[addtime],a.type,a.role,b.nickname,c.title userrank,d.name suitename
      
  into  #list
  FROM [{0}rechargeinfo] a
  join {0}users b on a.account=b.mobile
join {0}userranks c on b.userrid=c.userrid
left  join {0}suites d on a.vossuiteid=d.suiteid
  {1}

declare @totalmoney decimal(18,2),@total int
select @totalmoney=sum(total_fee) from #list where convert(varchar(10),paytime,120)=convert(varchar(10),getdate(),120)
select @total=(select count(1)  from #list)
if(@pagesize=-1)
begin
select cast(id as int) id,rechargeid ,account,suiteid,trade_no,out_trade_no ,paytime,total_fee ,platform,addtime,type,role,nickname,userrank,
suitename,@totalmoney totalmoney,@total TotalCount from #list
end
else
begin
select cast(id as int) id,rechargeid ,account,suiteid,trade_no,out_trade_no ,paytime,total_fee ,platform,addtime,type,role,nickname,userrank,
suitename,@totalmoney totalmoney, @total TotalCount from #list where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end

 end try
begin catch
select ERROR_MESSAGE() state
end catch

", RDBSHelper.RDBSTablePre, condition);

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }


        /// <summary>
        /// 获取用户账户余额
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public DataTable GetUserMoney(string account)
        {
            string commandText = string.Format(@"
            select isnull(totalmoney,0) totalmoney
            from owzx_users a where a.mobile ='{1}'
            ",
           RDBSHelper.RDBSTablePre, account);
            return RDBSHelper.ExecuteDataset(commandText).Tables[0];
        }
        #endregion

        #region 银行卡和提现密码
        /// <summary>
        /// 更新绑定银行卡信息
        /// </summary>
        /// <param name="drawa"></param>
        /// <returns></returns>
        public string UpdateDrawCardInfo(MD_DrawAccount drawa)
        {
            DbParameter[] parms = {
                                      GenerateInParam("@account", SqlDbType.VarChar, 15, drawa.Account),
                                      GenerateInParam("@username", SqlDbType.VarChar, 20, drawa.Username),
                                      GenerateInParam("@card", SqlDbType.VarChar, 20, drawa.Card),
                                      GenerateInParam("@cardnum", SqlDbType.VarChar, 20, drawa.Cardnum),
                                      GenerateInParam("@cardaddress", SqlDbType.VarChar, 50, drawa.Cardaddress)
                                  };


            string commandText = string.Format(@"
begin try
if exists(select 1 from owzx_userdrawaccount a join owzx_users b on a.uid=b.uid and b.mobile=@account)
begin

update a set 
a.username=@username,
a.card=@card,
a.cardnum=@cardnum,
a.cardaddress=@cardaddress
from owzx_userdrawaccount a 
join owzx_users b on a.uid=b.uid and b.mobile=@account

end
else
begin
insert into owzx_userdrawaccount([uid],[username],[card],[cardnum],[cardaddress])
select uid,@username,@card,@cardnum,@cardaddress
from owzx_users where mobile=@account
end

select '更新成功'

end try
begin catch
select ERROR_MESSAGE() state
end catch

");
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }
        /// <summary>
        /// 更新提现密码
        /// </summary>
        /// <param name="drawa"></param>
        /// <returns></returns>
        public string UpdateDrawPWD(MD_DrawAccount drawa)
        {
            DbParameter[] parms = {
                                      GenerateInParam("@account", SqlDbType.VarChar, 15, drawa.Account),
                                      GenerateInParam("@drawpwd", SqlDbType.VarChar, 50, drawa.Drawpwd)
                                  };


            string commandText = string.Format(@"
begin try
if exists(select 1 from owzx_userdrawaccount a join owzx_users b on a.uid=b.uid and rtrim(b.mobile)=@account)
begin

update a set 
a.drawpwd=@drawpwd
from owzx_userdrawaccount a 
join owzx_users b on a.uid=b.uid and b.mobile=@account

end
else
begin
insert into owzx_userdrawaccount([uid],drawpwd)
select uid,@drawpwd
from owzx_users where mobile=@account
end

select '更新成功'

end try
begin catch
select ERROR_MESSAGE() state
end catch

");
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }
        /// <summary>
        /// 验证提现密码是否正确
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public string ValidateDrawPwd(string account, string pwd)
        {
            DbParameter[] parms = {
                                      GenerateInParam("@account", SqlDbType.VarChar, 15, account),
                                      GenerateInParam("@drawpwd", SqlDbType.VarChar, 50, pwd)
                                  };
            string sql = string.Format(@"

if exists(select 1 from owzx_userdrawaccount a join owzx_users b on a.uid=b.uid and rtrim(b.mobile)=@account and a.drawpwd=@drawpwd)
begin
select '成功'
end
else
begin
select '失败'
end

");
            return RDBSHelper.ExecuteScalar(CommandType.Text, sql, parms).ToString();
        }

        /// <summary>
        /// 是否设置提现密码
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string ValidateDrawPwd(string account)
        {
            DbParameter[] parms = {
                                      GenerateInParam("@account", SqlDbType.VarChar, 15, account)
                                  };
            string sql = string.Format(@"

if exists(select 1 from owzx_userdrawaccount a join owzx_users b on a.uid=b.uid and rtrim(b.mobile)=@account and isnull(a.drawpwd,'')!='' )
begin
select '成功'
end
else
begin
select '失败'
end
");
            return RDBSHelper.ExecuteScalar(CommandType.Text, sql, parms).ToString();
        }


        /// <summary>
        /// 删除提现账号
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string DeleteDrawAccount(string account)
        {
            string commandText = string.Format(@"
            begin try
            if exists(select 1 from [{0}userdrawaccount]  a join {0}users b on a.uid=b.uid  where b.mobile='{1}')
            begin
            delete a from [{0}userdrawaccount]  a join {0}users b on a.uid=b.uid  where b.mobile='{1}'
            select '删除成功' state
            end
            else
            begin
            select '提现记录已被删除' state
            end
            end try
            begin catch
            select ERROR_MESSAGE() state
            end catch
            ",
             RDBSHelper.RDBSTablePre, account);
            return RDBSHelper.ExecuteScalar(commandText).ToString();
        }
        
        /// <summary>
        ///获取提现账号(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetDrawAccountList(int pageNumber, int pageSize, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };


            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
  drop table #list

SELECT ROW_NUMBER() over(order by a.drawaccid ) id,
   a.[drawaccid]
      ,a.[uid]
      ,a.[username]
      ,a.[card]
      ,a.[cardnum]
      ,a.[cardaddress]
      ,a.[drawpwd]
      ,a.[addtime]
      ,a.[updatetime],b.mobile account
into  #list
  FROM [{0}userdrawaccount] a
 join {0}users b on a.uid=b.uid and a.card is not null
  {1}

declare @total int
select @total=count(1)  from #list
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

", RDBSHelper.RDBSTablePre, condition);

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }
        #endregion

        #region 提现
        /// <summary>
        /// 添加提现记录
        /// </summary>
        /// <param name="draw"></param>
        /// <returns></returns>
        public string AddDraw(DrawInfoModel draw)
        {
            DbParameter[] parms = {
                                       GenerateInParam("@account", SqlDbType.VarChar, 15, draw.Account),
                                       GenerateInParam("@money", SqlDbType.Int, 4, draw.Money)
                                      
                                    };
            string commandText = string.Format(@"
begin try
begin tran t1
if exists(select 1 from {0}users where totalmoney>@money)
begin

declare @uid int
set @uid=(select uid from {0}users where mobile=@account)

INSERT INTO [{0}userdraw]([uid],[money],[state])
VALUES (@uid,@money,0)


declare @count int
select @count=COUNT(1) 
from [owzx_userdraw]  a 
where a.uid=@uid
and CONVERT(varchar(10),a.addtime,120)=CONVERT(varchar(10),getdate(),120)

if(@count>3)
begin
--每天免费3次,超过收取1%手续费
update a set  a.totalmoney=a.totalmoney-b.money-b.money*1*0.01
from {0}users a 
join {0}userdraw b on a.uid=b.uid and a.uid=@uid

INSERT INTO owzx_accountchange([uid],[changemoney],[remark])
select @uid ,-(@money+@money*1*0.01),'提现'


end
else
begin
update a set  a.totalmoney=a.totalmoney-b.money
from {0}users a 
join {0}userdraw b on a.uid=b.uid and a.uid=@uid

INSERT INTO owzx_accountchange([uid],[changemoney],[remark])
select @uid ,-@money,'提现'

end




select '添加成功' state
commit tran t1
end
else
begin
select '-1' state
commit tran t1
end
end try
begin catch
rollback tran t1
select ERROR_MESSAGE() state
end catch
",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }
        /// <summary>
        /// 更新提现记录 
        /// </summary>
        /// <param name="draw"></param>
        /// <returns></returns>
        public string UpdateDraw(DrawInfoModel draw)
        {
            DbParameter[] parms = {
                                      GenerateInParam("@drawid", SqlDbType.Int, 4, draw.Drawid),
                                      GenerateInParam("@state", SqlDbType.VarChar, 1, draw.State),
                                      GenerateInParam("@exception", SqlDbType.VarChar, 100, draw.Exception),
                                      GenerateInParam("@updateuser", SqlDbType.VarChar, 30, draw.Updateuser)
                                    };
            string commandText = string.Format(@"
begin try
begin tran t1
if exists(select 1 from [{0}userdraw] where drawid=@drawid)
begin
UPDATE [{0}userdraw]
   SET state = @state,exception=@exception,updateuser=@updateuser,updatetime=convert(varchar(25),getdate(),120)
where drawid=@drawid

--提现成功 发送消息
if(@state='2')
begin

insert into owzx_message([uid],[title],[body])
select  a.uid ,'提现'+cast(b.money as varchar(20))+'元到账通知','您提现的'+cast(b.money as varchar(20))+'元已经到账,请注意查收'
from {0}users a 
join {0}userdraw b on a.uid=b.uid and b.drawid=@drawid

end
else if(@state='3')
begin


declare @count int
select @count=COUNT(1) 
from [owzx_userdraw]  a 
where a.uid=(select uid from owzx_userdraw where drawid=@drawid)
and CONVERT(varchar(10),a.addtime,120)=CONVERT(varchar(10),getdate(),120)

if(@count>3)
begin
--每天免费3次,超过收取1%手续费
update a set  a.totalmoney=a.totalmoney+b.money+b.money*1*0.01
from {0}users a 
join {0}userdraw b on a.uid=b.uid and b.drawid=@drawid

INSERT INTO owzx_accountchange([uid],[changemoney],[remark])
select uid ,money+money*1*0.01,'提现失败'
from owzx_userdraw where drawid=@drawid

end
else
begin
update a set  a.totalmoney=a.totalmoney+b.money
from {0}users a 
join {0}userdraw b on a.uid=b.uid and b.drawid=@drawid

INSERT INTO owzx_accountchange([uid],[changemoney],[remark])
select uid ,money,'提现失败'
from owzx_userdraw where drawid=@drawid

end

end



select '修改成功' state
commit tran t1
end
else
begin
select '提现记录已被删除' state
commit tran t1
end
end try
begin catch
rollback tran t1
select ERROR_MESSAGE() state
end catch
",
                                                RDBSHelper.RDBSTablePre);
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }

        /// <summary>
        /// 删除提现记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteDraw(string id)
        {
            string commandText = string.Format(@"
            begin try
            if exists(select 1 from [{0}userdraw] where [drawid] in ({1}))
            begin
            delete from [{0}userdraw] where [drawid] in ({1})
            select '删除成功' state
            end
            else
            begin
            select '提现记录已被删除' state
            end
            end try
            begin catch
            select ERROR_MESSAGE() state
            end catch
            ",
             RDBSHelper.RDBSTablePre, id);
            return RDBSHelper.ExecuteScalar(commandText).ToString();
        }

        /// <summary>
        ///  获取提现记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetDrawList(int pageNumber, int pageSize, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };


            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
drop table #list

SELECT ROW_NUMBER() over(order by a.addtime desc) id,
a.drawid,a.[uid],a.money,a.[state],a.[exception],a.[addtime] ,a.[updatetime],c.username
,c.card,c.cardnum,c.cardaddress,b.mobile,isnull(b.totalmoney,0) totalmoney
into  #list
FROM [owzx_userdraw] a
join owzx_users b on a.uid=b.uid
join owzx_userdrawaccount c on a.uid=c.uid
{0}

--(case  when [state]='0' or [state]='1' then totalmoney+100 else totalmoney end ) 
if(@pagesize=-1)
begin
select cast(id as int) id,drawid,[uid],[money],case state when '0' then '待审核' when '1' then '审核中' when '2' then '审核完成' when '3' then '审核失败' end state,
isnull([exception],'') exception,[addtime] ,[updatetime],username,card,cardnum,cardaddress,mobile,
totalmoney,
(select count(1)  from #list) TotalCount from #list
end
else
begin
select  cast(id as int) id,drawid,[uid],[money],
case state when '0' then '待审核' when '1' then '审核中' when '2' then '审核完成' when '3' then '审核失败' end state,
isnull([exception],'') exception,[addtime] ,[updatetime],username,card,cardnum,cardaddress,mobile,
totalmoney,
(select count(1)  from #list) TotalCount from #list where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end

end try
begin catch
select ERROR_MESSAGE() state
end catch

", condition);

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }
        #endregion

        #region 收益
        /// <summary>
        /// 获取用户收益
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetUserIncome(int pageNumber, int pageSize, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };

            string sql = string.Format(@"if OBJECT_ID('tempdb..#list') is not null
drop table #list

select ROW_NUMBER() over(order by addtime desc) id,uid,mobile,nickname,title,type,money,status,addtime 
into #list
from
(
select a.uid,a.mobile,a.nickname,b.title,c.type,isnull(c.money,0)money,case c.isread when 0 then '未到账' when 1 then '已到账' end status,convert(varchar(25),c.addtime,120) addtime
from owzx_users a 
join owzx_userranks b on a.userrid=b.userrid and a.admingid=1
join owzx_usercommission c on a.uid=c.uid
union all
select a.uid,a.mobile,a.nickname,b.title,'' type,isnull(d.money,0)money,'已提现' status,convert(varchar(25),isnull(d.addtime,''),120) addtime
from owzx_users a 
join owzx_userranks b on a.userrid=b.userrid and a.admingid=1
join owzx_userdraw d on a.uid=d.uid
union all
select a.uid,a.mobile,a.nickname,b.title,'' type,isnull(a.totalmoney,0)money,'未提现' status,convert(varchar(25),'',120) addtime
from owzx_users a 
join owzx_userranks b on a.userrid=b.userrid and a.admingid=1 )a

{0} 

declare @totalmoney decimal(18,2),@Total int
select @totalmoney= sum(isnull(a.money,0)) from owzx_usercommission a join owzx_users b on a.uid=b.uid and b.admingid=1 

select @Total=count(1) from #list

if(@pagesize=-1)
begin
select *,@totalmoney totalmoney,@Total TotalCount from #list 
end
else
begin
select *,@totalmoney totalmoney,@Total TotalCount from #list  where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end
", condition);


            return RDBSHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }
        #endregion

    }
}
