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
    /// SqlServer策略之彩票部分类
    /// </summary>
    public partial class RDBSStrategy : IRDBSStrategy
    {
        #region 验证码记录
        /// <summary>
        /// 添加验证码记录
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string AddSMSCode(MD_SMSCode code)
        {
            DbParameter[] parms = {
                                       
                                        GenerateInParam("@account", SqlDbType.VarChar,15, code.Account),
                                        GenerateInParam("@code", SqlDbType.VarChar,10, code.Code),
                                        GenerateInParam("@expiretime", SqlDbType.DateTime, 25, code.Expiretime)
                                       
                                    };
            string commandText = string.Format(@"
begin try
if exists(select 1 from owzx_usersmscode where account=@account)
begin

update a set a.code=@code,a.expiretime=@expiretime
from owzx_usersmscode a
where account=@account

end
else
begin

INSERT INTO owzx_usersmscode([account]
           ,[code]
           ,[expiretime])
VALUES (@account,@code,@expiretime)

end

select '添加成功' state
end try
begin catch
select ERROR_MESSAGE() state
end catch
");
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }

        /// <summary>
        /// 更新验证码
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string UpdateSMSCode(MD_SMSCode code)
        {
            return "";
        }

        /// <summary>
        /// 删除验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteSMSCode(string id)
        {
            string commandText = string.Format(@"
            begin try
            if exists(select 1 from owzx_usersmscode where codeid in ({0}))
            begin
            delete from owzx_usersmscode where codeid in ({0})
            select '删除成功' state
            end
            else
            begin
            select '记录已被删除' state
            end
            end try
            begin catch
            select ERROR_MESSAGE() state
            end catch
            ",
             id);
            return RDBSHelper.ExecuteScalar(commandText).ToString();
        }

        /// <summary>
        /// 删除过期的验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteSMSCode()
        {
            string commandText = string.Format(@"
            begin try
            if exists(select 1 from owzx_usersmscode where DATEDIFF(minute,expiretime,getdate())>10)
            begin
            delete from owzx_usersmscode where DATEDIFF(minute,expiretime,getdate())>10
            select '删除成功' state
            end
            
            end try
            begin catch
            select ERROR_MESSAGE() state
            end catch
            ");
            return RDBSHelper.ExecuteScalar(commandText).ToString();
        }

        /// <summary>
        ///获取验证码记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetSMSCodeList(int pageNumber, int pageSize, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };


            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
  drop table #list

SELECT ROW_NUMBER() over(order by a.codeid desc) id
      ,a.[codeid]
      ,a.[account]
      ,a.[code]
      ,a.[expiretime]
      ,a.[addtime]
  into  #list
  FROM owzx_usersmscode a
  {0}


if(@pagesize=-1)
begin
select * from #list
end
else
begin
select * from #list where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end

end try
begin catch
select ERROR_MESSAGE() state
end catch

", condition);

            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, parms)[0];
        }

        #endregion

        #region 用户消息记录
        /// <summary>
        /// 添加用户消息记录
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string AddMessage(MD_Message msg)
        {
            DbParameter[] parms = {
                                       
                                        GenerateInParam("@account", SqlDbType.VarChar,15, msg.Account),
                                        GenerateInParam("@title", SqlDbType.VarChar,20, msg.Title),
                                        GenerateInParam("@body", SqlDbType.VarChar, 200, msg.Body)
                                       
                                    };
            string commandText = string.Format(@"
begin try

INSERT INTO owzx_message([uid],[title],[body])
VALUES ((select uid from owzx_users where rtrim(mobile)=@account),@title,@body)

select '添加成功' state
end try
begin catch
select ERROR_MESSAGE() state
end catch
");
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }

        /// <summary>
        /// 更新用户消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string UpdateMessage(MD_Message msg)
        {
            return "";
        }

        /// <summary>
        /// 删除用户消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteMessage(string id)
        {
            string commandText = string.Format(@"
            begin try
            if exists(select 1 from owzx_message where msgid in ({0}))
            begin
            delete from owzx_message where msgid in ({0})
            select '删除成功' state
            end
            else
            begin
            select '记录已被删除' state
            end
            end try
            begin catch
            select ERROR_MESSAGE() state
            end catch
            ",
             id);
            return RDBSHelper.ExecuteScalar(commandText).ToString();
        }

        /// <summary>
        ///获取用户消息记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetMessageList(int pageNumber, int pageSize, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };


            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
  drop table #list

SELECT ROW_NUMBER() over(order by a.msgid desc) id
      ,a.[msgid]
      ,a.[uid]
      ,a.[title]
      ,a.[body]
      ,a.[addtime],b.mobile account
  into  #list
  FROM owzx_message a
  join owzx_users b on a.uid=b.uid
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

", condition);

            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, parms)[0];
        }

        #endregion

        #region 用户回水

        /// <summary>
        /// 更新用户回水
        /// </summary>
        /// <param name="back"></param>
        /// <returns></returns>
        public string UpdateUserBack(MD_UserBack back)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@backid", SqlDbType.Int, 4, back.Backid),
                                    GenerateInParam("@money", SqlDbType.Decimal, 10, back.Money),
                                    GenerateInParam("@status", SqlDbType.Int, 4 ,back.Status),
                                    GenerateInParam("@updateuid", SqlDbType.Int, 4 ,back.Updateuid)
                                  };
            string commandText = string.Format(@"
            begin try
            begin tran t1
            if exists(select 1 from owzx_userback where backid=@backid)
            begin
            UPDATE owzx_userback
               SET money = @money,status=@status,updateuid=@updateuid,updatetime=convert(varchar(25),getdate(),120)
            where backid=@backid

            if(@status=2)
            begin
            INSERT INTO owzx_accountchange([uid],[changemoney],[remark])
            select (select uid from owzx_userback where backid=@backid),@money,'回水'
            end
            
            select '修改成功' state
            commit tran t1
            end
            else
            begin
            select '记录已被删除' state
            commit tran t1
            end
            end try
            begin catch
            rollback tran t1
            select ERROR_MESSAGE() state
            end catch
            ");
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }
        /// <summary>
        /// 更新用户回水
        /// </summary>
        /// <param name="back"></param>
        /// <returns></returns>
        public string UpdateUserBackReport(MD_UserBack back)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@backid", SqlDbType.Int, 4, back.Backid),
                                    GenerateInParam("@money", SqlDbType.Decimal, 10, back.Money),
                                    GenerateInParam("@status", SqlDbType.Int, 4 ,back.Status),
                                    GenerateInParam("@updateuid", SqlDbType.Int, 4 ,back.Updateuid)
                                  };
            string commandText = string.Format(@"
            begin try
            begin tran t1
            if exists(select 1 from owzx_userbackreport where backid=@backid)
            begin
            UPDATE owzx_userbackreport
               SET money = @money,status=@status,updatetime=convert(varchar(25),getdate(),120)
            where backid=@backid

            if(@status=2)
            begin

            declare @uid int=0  select @uid=uid from owzx_userbackreport where backid=@backid
            INSERT INTO owzx_accountchange([uid],[changemoney],[remark],accounted)
            select @uid,@money,'回水',(select totalmoney from owzx_users where uid=@uid)
            update owzx_users set totalmoney=totalmoney+@money where uid=@uid) 
            end
            
            select '修改成功' state
            commit tran t1
            end
            else
            begin
            select '记录已被删除' state
            commit tran t1
            end
            end try
            begin catch
            rollback tran t1
            select ERROR_MESSAGE() state
            end catch
            ");
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }
        /// <summary>
        /// 更新用户回水
        /// </summary>
        /// <param name="back"></param>
        /// <returns></returns>
        public string UpdUserBackReport(int uid, int type=0)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@uid", SqlDbType.Int, 4, uid),  
                                    GenerateInParam("@type", SqlDbType.Int, 4 ,type)
                                  };
            string commandText = string.Format(@"
            begin try
            begin tran t1
            declare @backid int =0,@money decimal(18,2)=0
            select top 1 @backid=backid,@money=money from owzx_userbackreport where uid=@uid and status=0 and backtype=@type
            if (@backid>0)
            begin  

            update  owzx_userbackreport set status=2 where backid=@backid  
            INSERT INTO owzx_accountchange([uid],[changemoney],[remark],accounted)
            select @uid,@money,'回水',(select totalmoney from owzx_users where uid=@uid)
            update owzx_users set totalmoney=totalmoney+@money where uid=@uid 
 
            
            select '领取成功' state
            commit tran t1
            end
            else
            begin
            select '不存在待回水金额' state
            commit tran t1
            end
            end try
            begin catch
            rollback tran t1
            select ERROR_MESSAGE() state
            end catch
            ");
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteUserBack(string id)
        {
            string commandText = string.Format(@"
            begin try
            if exists(select 1 from owzx_userback where backid in ({0}))
            begin
            delete from owzx_userback where backid in ({0})
            select '删除成功' state
            end
            else
            begin
            select '记录已被删除' state
            end
            end try
            begin catch
            select ERROR_MESSAGE() state
            end catch
            ",
             id);
            return RDBSHelper.ExecuteScalar(commandText).ToString();
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteUserBackReport(string id)
        {
            string commandText = string.Format(@"
            begin try
            if exists(select 1 from owzx_userbackReport where backid in ({0}))
            begin
            delete from owzx_userbackReport where backid in ({0})
            select '删除成功' state
            end
            else
            begin
            select '记录已被删除' state
            end
            end try
            begin catch
            select ERROR_MESSAGE() state
            end catch
            ",
             id);
            return RDBSHelper.ExecuteScalar(commandText).ToString();
        }
        /// <summary>
        ///  获取用户回水(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetBackList(int pageNumber, int pageSize, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };


            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
drop table #list

SELECT ROW_NUMBER() over(order by a.backid desc) id,
a.[backid] ,a.[uid],a.[money],a.[roomtype],a.[status],convert(varchar(25),a.[addtime],120) addtime,b.mobile account,a.profitmoney,a.combratio,c.type room
into  #list
FROM owzx_userback a
join owzx_users b on a.uid=b.uid 
join owzx_sys_basetype c on a.roomtype=c.systypeid
{0}

declare @total int
select @total=(select count(1)  from #list)

if(@pagesize=-1)
begin
select *,@total TotalCount from #list
end
else
begin
select  *,@total TotalCount from #list where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end

end try
begin catch
select ERROR_MESSAGE() state
end catch

", condition);

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }
        /// <summary>
        ///  获取用户回水(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetBackReportList(int pageNumber, int pageSize, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };


            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
drop table #list

SELECT ROW_NUMBER() over(order by a.backid desc) id,
a.[backid] ,a.[uid],a.[profitmoney],a.[money],a.[status],a.[backtype],a.[backrate],convert(varchar(25),a.[addtime],120) addtime,b.email account 
into  #list
FROM owzx_userbackreport a
join owzx_users b on a.uid=b.uid   
{0}

declare @total int
select @total=(select count(1)  from #list)

if(@pagesize=-1)
begin
select *,@total TotalCount from #list
end
else
begin
select  *,@total TotalCount from #list where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end

end try
begin catch
select ERROR_MESSAGE() state
end catch

", condition);

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText, parms).Tables[0];
        }
        #endregion

        #region 用户账变记录
        /// <summary>
        /// 添加用户账变记录
        /// </summary>
        /// <param name="chag"></param>
        /// <returns></returns>
        public string AddAChange(MD_Change chag)
        {
            DbParameter[] parms = {
                                       
                                        GenerateInParam("@account", SqlDbType.VarChar,15, chag.Account),
                                        GenerateInParam("@changemoney", SqlDbType.Decimal,10, chag.Changemoney),
                                        GenerateInParam("@remark", SqlDbType.VarChar, 100, chag.Remark)
                                       
                                    };
            string commandText = string.Format(@"
begin try

INSERT INTO owzx_accountchange([uid],[changemoney],[remark])
VALUES ((select uid from owzx_users where rtrim(mobile)=@account),@changemoney,@remark)

select '添加成功' state
end try
begin catch
select ERROR_MESSAGE() state
end catch
");
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }

        /// <summary>
        /// 更新账变信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string UpdateAChange(MD_Change chag)
        {
            return "";
        }

        /// <summary>
        /// 删除账变信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteAChange(string id)
        {
            string commandText = string.Format(@"
            begin try
            if exists(select 1 from owzx_accountchange where achangeid in ({0}))
            begin
            delete from owzx_accountchange where achangeid in ({0})
            select '删除成功' state
            end
            else
            begin
            select '记录已被删除' state
            end
            end try
            begin catch
            select ERROR_MESSAGE() state
            end catch
            ",
             id);
            return RDBSHelper.ExecuteScalar(commandText).ToString();
        }

        /// <summary>
        ///获取用户账变信息(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetAChangeList(int pageNumber, int pageSize, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };


            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
  drop table #list

SELECT ROW_NUMBER() over(order by a.achangeid desc) id
      ,a.[achangeid],a.[uid],a.[changemoney],a.[remark],a.[addtime],b.mobile account
  into  #list
  FROM owzx_accountchange a
  join owzx_users b on a.uid=b.uid
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

", condition);

            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, parms)[0];
        }

        #endregion

        #region 用户转账记录
        /// <summary>
        /// 添加用户转账记录
        /// </summary>
        /// <param name="rmt"></param>
        /// <returns></returns>
        public string AddUserRemit(MD_Remit rmt)
        {
            DbParameter[] parms = {
                                       
                                        GenerateInParam("@mobile", SqlDbType.VarChar,15, rmt.Mobile),
                                        GenerateInParam("@type", SqlDbType.VarChar,20, rmt.Type),
                                        GenerateInParam("@name", SqlDbType.VarChar, 50, rmt.Name),
                                        GenerateInParam("@account", SqlDbType.VarChar,50, rmt.Account),
                                        GenerateInParam("@money", SqlDbType.Int , 4, rmt.Money),
                                        GenerateInParam("@bankname", SqlDbType.VarChar, 50, rmt.Bankname),
                                        GenerateInParam("@status", SqlDbType.Int,4, rmt.Status)
                                       
                                    };
            string commandText = string.Format(@"
begin try

INSERT INTO owzx_userremit([uid],[type],[name],[account],[money],[bankname],[status])
VALUES ((select uid from owzx_users where rtrim(mobile)=@mobile),@type,@name,@account,@money,@bankname,@status)

select '添加成功' state
end try
begin catch
select ERROR_MESSAGE() state
end catch
");
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }

        /// <summary>
        /// 更新用户转账记录
        /// </summary>
        /// <param name="rmt"></param>
        /// <returns></returns>
        public string UpdateUserRemit(MD_Remit rmt)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@remitid", SqlDbType.VarChar,15, rmt.Remitid),
                                        GenerateInParam("@remark", SqlDbType.VarChar, 200, rmt.Remark),
                                        GenerateInParam("@realmoney", SqlDbType.Int , 4, rmt.RealMoney),
                                        GenerateInParam("@status", SqlDbType.Int,4, rmt.Status),
                                        GenerateInParam("@updateuid", SqlDbType.Int,4, rmt.Updateuid),
                                       
                                    };
            string commandText = string.Format(@"
begin try
if exists(select 1 from owzx_userremit where remitid=@remitid )
begin
update a set   
a.status = @status
,a.remark = @remark
,a.realmoney=@realmoney
,a.updateuid = @updateuid
,a.updatetime = convert(varchar(25),getdate(),120)

from owzx_userremit a where a.remitid=@remitid 

--添加账变记录
if(@status=2)
begin
INSERT INTO [owzx_accountchange]([uid] ,[changemoney],[remark])
select  uid ,@realmoney,'充值'
from owzx_userremit where remitid=@remitid

update a set a.totalmoney=isnull(a.totalmoney,0)+@realmoney
from owzx_users a 
join owzx_userremit b on a.uid=b.uid and b.remitid=@remitid

insert into owzx_message([uid],[title],[body])
select  uid ,'充值'+cast(@realmoney as varchar(20))+'元到账通知','您充值的'+cast(@realmoney as varchar(20))+'元已经到账,请注意查收'
from owzx_userremit where remitid=@remitid
end
end

select '更新成功' state
end try
begin catch
select ERROR_MESSAGE() state
end catch
");
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }

        /// <summary>
        /// 删除用户转账记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteUserRemit(string id)
        {
            string commandText = string.Format(@"
            begin try
            if exists(select 1 from owzx_userremit where remitid in ({0}))
            begin
            delete from owzx_userremit where remitid in ({0})
            select '删除成功' state
            end
            else
            begin
            select '记录已被删除' state
            end
            end try
            begin catch
            select ERROR_MESSAGE() state
            end catch
            ",
             id);
            return RDBSHelper.ExecuteScalar(commandText).ToString();
        }

        /// <summary>
        ///获取用户转账记录记录(分页)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetUserRemitList(int pageIndex, int pageSize, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageIndex)
                                  };


            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
  drop table #list

SELECT ROW_NUMBER() over(order by a.remitid desc) id
      ,a.[remitid]
      ,a.[uid]
      ,a.[type]
      ,a.[name]
      ,a.[account]
      ,a.[money],a.realmoney
      ,a.[bankname]
      ,a.[status]
      ,a.[remark]
      ,a.[addtime]
      ,a.[updateuid]
      ,a.[updatetime],b.mobile mobile
  into  #list
  FROM owzx_userremit a
  join owzx_users b on a.uid=b.uid
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

", condition);

            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, parms)[0];
        }

        #endregion

        /// <summary>
        /// 获取首页数据
        /// </summary>
        /// <returns></returns>
        public DataTable HomeData()
        {
            string sql = string.Format(@"
select (select COUNT(1) from owzx_users where admingid=1) users,(select isnull(sum(luckresult),0) from owzx_bettprofitloss where luckresult>0) money
");
            return RDBSHelper.ExecuteTable(sql, null)[0];
        }

        #region 系统设置
        /// <summary>
        /// 添加系统设置
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public string AddSet(MD_SysSet set)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@parentid", SqlDbType.Int,4, set.Parentid),
                                        GenerateInParam("@name", SqlDbType.VarChar,50, set.Name),
                                        GenerateInParam("@invalue", SqlDbType.VarChar, 50, set.InValue)
                                    };
            string commandText = string.Format(@"
begin try

INSERT INTO owzx_sys_set([parentid],[name],[invalue])
VALUES (@parentid,@name,@invalue)
select '添加成功' state
end try
begin catch
select ERROR_MESSAGE() state
end catch
");
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }

        /// <summary>
        /// 更新系统设置
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public string UpdateSet(MD_SysSet set)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@setid", SqlDbType.Int,4, set.Setid),
                                        GenerateInParam("@name", SqlDbType.VarChar,50, set.Name),
                                        GenerateInParam("@invalue", SqlDbType.VarChar, 50, set.InValue)
                                    };
            string commandText = string.Format(@"
begin try
begin tran t1

if exists(select 1 from owzx_sys_set where setid=@setid)
begin

update a set 
a.name=@name,a.invalue=@invalue
from owzx_sys_set a
where a.setid=@setid

select '更新成功' state
commit tran t1
end
else
begin
select '记录已被删除' state
commit tran t1
end
end try
begin catch
rollback tran t1
select ERROR_MESSAGE() state
end catch
");
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }

        /// <summary>
        /// 删除系统设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteSet(string id)
        {
            string commandText = string.Format(@"
            begin try
            if exists(select 1 from owzx_sys_set where setid in ({0}))
            begin
            delete from owzx_sys_set where setid in ({0})
            select '删除成功' state
            end
            else
            begin
            select '记录已被删除' state
            end
            end try
            begin catch
            select ERROR_MESSAGE() state
            end catch
            ",
             id);
            return RDBSHelper.ExecuteScalar(commandText).ToString();
        }

        /// <summary>
        ///获取系统设置记录(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetSetList(int pageNumber, int pageSize, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };


            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
  drop table #list

SELECT ROW_NUMBER() over(order by a.setid ) id
      ,a.[setid]
      ,a.[parentid]
      ,a.[name]
      ,a.[invalue]
      ,a.[addtime]
  into  #list
  FROM owzx_sys_set a
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

", condition);

            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, parms)[0];
        }
        #endregion

        #region 用户投注记录
        /// <summary>
        ///获取彩票记录(分页)
        /// </summary>
        /// <param name="pageNumber">页索引</param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="account">账号</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetUserBettList(int pageNumber, int pageSize, string account,string condition="")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };


            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#listpage') is not null
  drop table #listpage

if OBJECT_ID('tempdb..#listresult') is not null
drop table #listresult

if OBJECT_ID('tempdb..#listuser') is not null
drop table #listuser

select a.uid,a.bttypeid, a.bettid,a.lotterynum,a.money,c.lotteryid, c.first,c.second,c.three,c.type,c.resulttype,c.result,c.resultnum,
d.item uitem,a.addtime
into #listuser
from owzx_bett a
join owzx_users b on a.uid=b.uid and rtrim(b.mobile)='{0}'
join owzx_lotteryrecord c on a.lotterynum=c.expect
join owzx_lotteryset d on a.bttypeid=d.bttypeid
{1}


select a.*,b.item
into #listresult
from #listuser a
join  owzx_lotteryset b on a.result is not null  and b.roomtype=20 and CHARINDEX(','+rtrim(a.resultnum)+',',','+rtrim(b.nums)+',')>0
union all
select a.uid,a.bttypeid, a.bettid,a.lotterynum,a.money,a.lotteryid,a.first,a.second,a.three,a.type,a.resulttype,a.result,a.resultnum,a.uitem,a.addtime,'豹子'
from #listuser a 
where a.first=a.second and a.first=a.three


if OBJECT_ID('tempdb..#list') is not null
drop table #list

select ROW_NUMBER() over(order by bettid desc) id,* 
into #listpage
from (
select a.bettid,c.type ,a.lotterynum expect,a.result,left(d.items,LEN(d.items)-1)items,
'大小单双' as bttype,a.uitem userbttype,a.money,
case when b.luckresult>0 then b.luckresult 
else 0.00 end lkmoney,case when b.luckresult>0 then b.luckresult-a.money 
else b.luckresult end winmoney,a.addtime
FROM #listuser a
join owzx_bettprofitloss b on a.uid=b.uid and a.lotteryid=b.lotteryid  and a.bettid=b.bettid
join owzx_sys_basetype c on a.type=c.systypeid
join (
SELECT a.type,a.lotterynum,      
[items]=( SELECT item +','      
FROM #listresult AS b      
WHERE b.type = a.type and a.lotterynum=b.lotterynum      
FOR XML PATH('')  )      
FROM #listresult AS a       
GROUP BY type,lotterynum  
) d on a.type=d.type and a.lotterynum=d.lotterynum
union all
select a.bettid,c.type ,a.lotterynum expect,'等待开奖' result,'等待开奖' items,
'大小单双' as bttype,a.uitem userbttype,a.money,
0.00 lkmoney,0.00 winmoney,a.addtime
FROM #listuser a
join owzx_sys_basetype c on a.type=c.systypeid and a.result is null
) a

declare @total int
select @total=(select count(1)  from #listpage)

if(@pagesize=-1)
begin

select id,type+' -- '+expect+'期' type,winmoney,result,items,bttype,userbttype,money,lkmoney,addtime,  @total TotalCount from #listpage
end
else
begin
select id,type+' -- '+expect+'期' type,winmoney,result,items,bttype,userbttype,money,lkmoney,addtime,@total TotalCount from #listpage where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end

end try
begin catch
select ERROR_MESSAGE() state
end catch

", account,condition);

            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, parms)[0];
        }
        #endregion

        #region 用户公告

        /// <summary>
        /// 获取系统公告
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserSysNew(string account)
        {
            DbParameter[] parms = {
                                    GenerateInParam("@account", SqlDbType.VarChar, 15,account) 
                                   };
            string sql = string.Format(@"
declare @uid int
set @uid=(select uid from owzx_users where RTRIM(mobile)=@account)

if not exists(select 1 from (select top 1 * from owzx_news where newstypeid=3 order by newsid desc) a 
join owzx_newsisread b on a.newsid=b.newid and b.uid=@uid)
begin
select top 1 newsid,addtime,body from owzx_news where newstypeid=3 order by newsid desc

INSERT INTO owzx_newsisread
           ([newid],[uid])
     VALUES
           ((select top 1 newsid from owzx_news where newstypeid=3 order by newsid desc),(select uid from owzx_users where RTRIM(mobile)=@account))

end
else
begin
select '已阅读' status
end
");
            return RDBSHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }
        #endregion

        #region 用户投注模式
        /// <summary>
        /// 添加用户投注模式
        /// </summary>
        /// <param name="chag"></param>
        /// <returns></returns>
        public string AddMode(MD_BettMode mode)
        {
            try
            {
                DbParameter[] parms = {
                                        GenerateInParam("@name", SqlDbType.VarChar,50, mode.Name),
                                        GenerateInParam("@uid", SqlDbType.Int,4, mode.Uid),
                                        GenerateInParam("@lotterytype", SqlDbType.Int,4, mode.LotteryType),
                                        GenerateInParam("@bettnum", SqlDbType.VarChar, 100, mode.Bettnum),
                                        GenerateInParam("@bettinfo", SqlDbType.VarChar,500, mode.Bettinfo),
                                        GenerateInParam("@betttotal", SqlDbType.Int,4, mode.Betttotal),
                                        GenerateInParam("@wintype", SqlDbType.Int,4, mode.Wintype),
                                        GenerateInParam("@losstype", SqlDbType.Int,4, mode.Losstype)
                                        
                                       
                                    };
                string commandText = string.Format(@"
begin try
begin tran t1
if exists(select 1 from owzx_userbettmodel where uid=@uid and lotterytype=@lotterytype and name=@name)
begin
update a set 
a.bettnum=@bettnum
,a.bettinfo=@bettinfo
,a.betttotal=@betttotal
,a.wintype=@wintype
,a.losstype=@losstype
,a.updatetime=convert(varchar(25),getdate(),120)
,a.updateuid=@uid
from owzx_userbettmodel a
where  uid=@uid and name=@name

end
else
begin
INSERT INTO [owzx_userbettmodel]
           ([name]
           ,[uid],lotterytype
           ,[bettnum]
           ,[bettinfo]
           ,[betttotal]
           ,[wintype]
           ,[losstype]
           )
VALUES (@name,@uid,@lotterytype,@bettnum,@bettinfo ,@betttotal,@wintype,@losstype)
end

select '添加成功' state
commit tran t1
end try
begin catch
rollback tran t1
select ERROR_MESSAGE() state
end catch

");
                return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
            }
            catch (Exception er)
            {
                
                throw;
            }
        }

        /// <summary>
        /// 更新模式信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string UpdateMode(MD_BettMode mode)
        {
            DbParameter[] parms = {
                                      GenerateInParam("@modeid", SqlDbType.Int,4, mode.Modeid),
                                        GenerateInParam("@name", SqlDbType.VarChar,50, mode.Name),
                                        GenerateInParam("@uid", SqlDbType.Int,4, mode.Uid),
                                        GenerateInParam("@bettnum", SqlDbType.VarChar, 100, mode.Bettnum),
                                        GenerateInParam("@bettinfo", SqlDbType.VarChar,500, mode.Bettinfo),
                                        GenerateInParam("@betttotal", SqlDbType.Int,4, mode.Betttotal),
                                        GenerateInParam("@wintype", SqlDbType.Int,4, mode.Wintype),
                                        GenerateInParam("@losstype", SqlDbType.Int,4, mode.Losstype)
                                        
                                       
                                    };
            string commandText = string.Format(@"

begin try
begin tran t1

if exists(select 1 from owzx_userbettmodel where modeid=@modeid)
begin

update a set 
a.name=@name
,a.bettnum=@bettnum
,a.bettinfo=@bettinfo
,a.betttotal=@betttotal
,a.wintype=@wintype
,a.losstype=@losstype
,a.updatetime=convert(varchar(25),getdate(),120)
,a.updateuid=@uid
from owzx_userbettmodel a
where a.modeid=@modeid

select '更新成功' state
commit tran t1
end
else
begin
select '记录已被删除' state
commit tran t1
end
end try
begin catch
rollback tran t1
select ERROR_MESSAGE() state
end catch
");
            return RDBSHelper.ExecuteScalar(CommandType.Text, commandText, parms).ToString();
        }

        /// <summary>
        /// 删除模式信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteMode(string name, int uid,int lotterytype)
        {
            string commandText = string.Format(@"
            begin try
            if exists(select 1 from owzx_userbettmodel where uid={0} and name='{1}' and lotterytype={2})
            begin
            delete from owzx_userbettmodel where uid={0} and name='{1}'
            select '删除成功' state
            end
            else
            begin
            select '记录已被删除' state
            end
            end try
            begin catch
            select ERROR_MESSAGE() state
            end catch
            ", uid,
             name,lotterytype);
            return RDBSHelper.ExecuteScalar(commandText).ToString();
        }

        /// <summary>
        ///获取模式信息(分页)
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">-1 取全部</param>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetModeList(int pageNumber, int pageSize, string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pageSize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageNumber)
                                  };


            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
  drop table #list

SELECT ROW_NUMBER() over(order by a.modeid desc) id
      ,a.[name],a.modeid
           ,a.[uid],a.lotterytype
           ,a.[bettnum]
           ,a.[bettinfo]
           ,a.[betttotal]
           ,a.[wintype]
           ,a.[losstype]
           ,a.[addtime]
           ,a.[updatetime]
           ,a.[updateuid]
  into  #list
  FROM owzx_userbettmodel a
  join owzx_users b on a.uid=b.uid
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

", condition);

            return RDBSHelper.ExecuteTable(CommandType.Text, commandText, parms)[0];
        }
        /// <summary>
        /// 是否设置投注模式
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool ExistsMode(int uid, int lotterytype)
        {
            string sql = string.Format(@"
select 1 from owzx_userbettmodel where uid={0} and lotterytype={1}
",uid,lotterytype);
            return RDBSHelper.Exists(sql);
        }

        #endregion
    }
}
