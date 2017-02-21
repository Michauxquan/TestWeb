using OWZX.Core;
using OWZX.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.RDBSStrategy.SqlServer
{
    /// <summary>
    /// SqlServer策略之新的竞猜
    /// </summary>
    public partial class RDBSStrategy : IRDBSStrategy
    {
        #region 竞猜数据
        /// <summary>
        /// 根据类型获取竞猜首页数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataSet GetLotteryByType(int type, int pageindex, int pagesize, int uid = -1)
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pagesize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageindex)
                                  };

            string sql = string.Format(@"
declare @uid int={1},@type int={0}

if OBJECT_ID('tempdb..#last') is not null
drop table #last

select top 1  type,expect,orderresult,first,second,three,result,resultnum,resulttype,status 
into #last
from owzx_lotteryrecord 
where type=@type and status=2 
and DATEDIFF(minute,opentime,GETDATE()) between 0 and 5

if not exists(select 1 from #last)
begin
select top 1 type,expect,orderresult,first,second,three,result,resultnum,resulttype,status 
from owzx_lotteryrecord 
where type=@type and status=2 order by lotteryid desc
end
else
begin
select * from #last
end

--用户投注盈亏
if OBJECT_ID('tempdb..#temp') is not null
drop table #temp

select a.lotteryid type,a.lotterynum expect,a.money,b.luckresult
into #temp
from owzx_bett a
join owzx_bettprofitloss b on a.bettid=b.bettid where a.uid=@uid 
and datediff(day,a.addtime,GETDATE())=0


declare @total int=(select COUNT(1) from owzx_lotteryrecord where type=@type)


if OBJECT_ID('tempdb..#now') is not null
drop table #now

declare @temptotal int=0
set @temptotal=(select COUNT(1) from #temp)


select top 1  type,expect lastnumber,opentime,status,DATEDIFF(SECOND,GETDATE(),opentime) remains, 
(select COUNT(1) from owzx_bett where uid=@uid and 
 datediff(day,addtime,GETDATE())=0) tdbettnum,
 (case when @temptotal=0 then 0 
 else (select isnull(SUM(luckresult),0) from #temp) end) tdprof,
 case when @temptotal=0 then 0 
 else (cast((select COUNT(1) from #temp where luckresult>0) /(select COUNT(1) from #temp)as decimal(18,2))) end winpercent,
 @total totalcount
into #now
from owzx_lotteryrecord
where type=@type and status in (0,1)
and DATEDIFF(SECOND,GETDATE(),opentime) between 0 and 300

if not exists(select 1 from #now)
begin
select top 1 type,expect lastnumber,opentime,status,DATEDIFF(SECOND,GETDATE(),opentime) remains, 
(select COUNT(1) from owzx_bett where uid=@uid and 
 datediff(day,addtime,GETDATE())=0) tdbettnum,
(case when @temptotal=0 then 0 
 else (select isnull(SUM(luckresult),0) from #temp) end) tdprof,
 case when (select COUNT(1) from #temp)=0 then 0 
 else (cast((select COUNT(1) from #temp where luckresult>0) /(select COUNT(1) from #temp)as decimal(18,2))) end winpercent,
 @total totalcount
 from owzx_lotteryrecord
where type=@type and status in (0,1) order by lotteryid desc
end
else
begin
select * from #now
end


 
if OBJECT_ID('tempdb..#lottery') is not null
 drop table #lottery

select * 
into #lottery
from (
select ROW_NUMBER() over(order by lotteryid desc) id,type,expect,orderresult,first,second,three,result,opentime,
resultnum,resulttype,status
from owzx_lotteryrecord where type=@type 
) a  where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex


if OBJECT_ID('tempdb..#bettprof') is not null
 drop table #bettprof

 select a.*,b.luckresult
 into #bettprof
 from owzx_bett a
 join  #lottery c on a.lotteryid=c.type and a.lotterynum=c.expect
 left join owzx_bettprofitloss b on a.bettid=b.bettid


select a.*,isnull(b.totalbett,0)totalbett,isnull(c.winperson,0) winperson,isnull(d.bettperson,0)bettperson,isnull(e.luckresult,0)luckresult,
isnull(e.money,0) money
from #lottery a
left join 
(select a.lotteryid type,a.lotterynum expect,sum(a.money) totalbett 
from #bettprof a join  #lottery b on a.lotteryid=b.type and a.lotterynum=b.expect 
group by a.lotteryid,a.lotterynum)  b on a.type=b.type and a.expect=b.expect
left join (select lotteryid type,lotterynum expect,count(1) winperson from #bettprof 
where luckresult>=0 group by lotteryid ,lotterynum) c
on a.type=c.type and a.expect=c.expect
left join (select lotteryid type,lotterynum expect,count(1) bettperson from #bettprof 
group by lotteryid ,lotterynum)  d
on a.type=d.type and a.expect=d.expect
left join #temp e 
on a.type=e.type and a.expect=e.expect


", type,uid);

            return RDBSHelper.ExecuteDataset(CommandType.Text, sql, parms);
        }
        /// <summary>
        /// 是否已开奖
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expect"></param>
        /// <returns></returns>
        public bool ExistsLotteryOpen(string type, string expect)
        {
            string sql = string.Format(@"
select 1 from owzx_lotteryrecord where type={0} and expect={1} and status=2
",type,expect);

            return RDBSHelper.Exists(sql);
        }
        /// <summary>
        /// 获取投注记录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public DataTable GetUserBett(int type, int uid, int pageindex, int pagesize)
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pagesize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageindex)
                                  };

            string sql = string.Format(@"
if OBJECT_ID('tempdb..#lotrecord') is not null
 drop table #lotrecord

select  ROW_NUMBER() over(order by a.bettid desc) id,a.lotterynum,a.addtime,c.resultnum,a.money,b.luckresult,
case when b.luckresult>=0 then b.luckresult-a.money else b.luckresult end win,a.bettid
into #lotrecord  
from owzx_bett a
left join owzx_bettprofitloss b on a.bettid=b.bettid
join owzx_lotteryrecord c on a.lotteryid=c.type and a.lotterynum=c.expect and c.type={0} and a.uid={1}

declare @total int=(select count(1) from #lotrecord )

if(@pagesize=-1)
begin
select * ,@total totalcount from #lotrecord
end
else
begin
select * ,@total totalcount from #lotrecord where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end

", type,uid);
            return RDBSHelper.ExecuteTable(sql,parms)[0];
        }

        /// <summary>
        /// 获取最新彩票记录
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable NewestLottery(string type)
        {
            lock (lkstate)
            {
                string sql = string.Format(@" 
                --逻辑：北京 投注4:30s 封盘30s ,到开奖时间加载新的一期；
                --加拿大 投注3分，封盘30s,到开奖时间加载新的一期；
                declare @type int ={0}
declare @min varchar(5), @sec varchar(5),@expect varchar(50),@totalsec varchar(5)

if(@type in (1,4,9)) 
begin

if exists(select top 1 1 from owzx_lotteryrecord where type=@type and status !=2 and 
(DATEDIFF(second,opentime,getdate()) >= -270 and DATEDIFF(second,opentime,getdate())<0) order by lotteryid )
begin
select top 1 @expect=expect, 
--@min=CONVERT(VARCHAR(10),DATEDIFF(SECOND,getdate(),dateadd(SECOND,-30,opentime))/60),
--@sec=CONVERT(VARCHAR(10),DATEDIFF(SECOND,getdate(),dateadd(SECOND,-30,opentime))%60),
@totalsec= CONVERT(VARCHAR(10),DATEDIFF(SECOND,getdate(),opentime))
from owzx_lotteryrecord where type=@type and status  !=2  and (DATEDIFF(second,opentime,getdate()) >= -270 and DATEDIFF(second,opentime,getdate())<0)
order by lotteryid

select @expect expect,@totalsec time
--select @expect expect,(replicate('0',2-len(@min))+rtrim(@min)) +'分'+(replicate('0',2-len(@sec))+rtrim(@sec)) +'秒' time
end
else
begin
select '?' expect,'维护中' time
end

end
else if(@type in (2,3)) 
begin

if exists(select top 1 1 from owzx_lotteryrecord where type=@type and status  !=2  and 
(DATEDIFF(second,opentime,getdate()) >= -180 and DATEDIFF(second,opentime,getdate())<0) order by lotteryid )
begin
select top 1 @expect=expect, 
--@min=CONVERT(VARCHAR(10),DATEDIFF(SECOND,getdate(),dateadd(SECOND,-30,opentime))/60),
--@sec=CONVERT(VARCHAR(10),DATEDIFF(SECOND,getdate(),dateadd(SECOND,-30,opentime))%60),
@totalsec= CONVERT(VARCHAR(10),DATEDIFF(SECOND,getdate(),opentime))
from owzx_lotteryrecord where type=@type and status  !=2 and (DATEDIFF(second,opentime,getdate()) >= -180 and DATEDIFF(second,opentime,getdate())<0)
order by lotteryid

select @expect expect,@totalsec time
--select @expect expect,(replicate('0',2-len(@min))+rtrim(@min)) +'分'+(replicate('0',2-len(@sec))+rtrim(@sec)) +'秒' time
end
else
begin

select '?' expect,'维护中' time
end

end
                ", type);
                return RDBSHelper.ExecuteTable(sql, null)[0];
            }
        }
        #endregion

        #region 赔率
        /// <summary>
        /// 获取赔率信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataSet GetLotterySet(int type)
        {
            string sql = string.Format(@"
if OBJECT_ID('tempdb..#list') is not null
drop table #list

SELECT ROW_NUMBER() over(order by a.bttypeid ) id,
a.[bttypeid]
,a.[type],c.type as settype,replicate('0',2-len(a.item))+rtrim(a.item) item,a.odds odds,a.[nums],a.[addtime],a.roomtype
into  #list
FROM owzx_lotteryset a
join owzx_sys_basetype c on a.type=c.systypeid
where roomtype=20 and a.type={0}

if({0}=13)
begin
select top 14 * from  #list

select top 14 * from  #list order by [bttypeid] desc 
end
else if({0}=15)
begin
--顺对杂
select * from  #list
end
else if({0}=12)
begin
--大小单双龙虎豹
select * from  #list
end
", type);
            return RDBSHelper.ExecuteDataset(sql);
        }

        /// <summary>
        /// 添加彩票赔率
        /// </summary>
        /// <param name="chag"></param>
        /// <returns></returns>
        public string AddLotSet(MD_LotSetOdds mode)
        {
            try
            {
                DbParameter[] parms = {
                                        GenerateInParam("@lotterytype", SqlDbType.Int,4, mode.Lotterytype),
                                        GenerateInParam("@bttypeid", SqlDbType.Int,4, mode.Bttypeid),
                                        GenerateInParam("@odds", SqlDbType.VarChar,20, mode.Odds)
                                       
                                    };
                string commandText = string.Format(@"
begin try
begin tran t1

INSERT INTO [owzx_lotsetodds]
           ([lotterytype]
           ,[bttypeid]
           ,[odds]
           )
VALUES (@lotterytype,@bttypeid,@settype,@odds)


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
        /// 更新彩票赔率
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string UpdateLotSet(MD_LotSetOdds mode)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@setid", SqlDbType.Int,4, mode.Bttypeid),
                                        GenerateInParam("@odds", SqlDbType.VarChar,20, mode.Odds)
                                       
                                    };
            string commandText = string.Format(@"
begin try
begin tran t1

if exists(select 1 from owzx_lotsetodds where setid=@setid)
begin
update a set a.[odds]=@odds
from [owzx_lotsetodds] a where  setid=@setid
       
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
        /// 删除彩票赔率
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DelLotSet(int setid)
        {
            string commandText = string.Format(@"
            begin try
            if exists(select 1 from  owzx_lotsetodds where setid={0})
            begin
            delete from owzx_lotsetodds wheresetid={0} 
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
            ", setid);
            return RDBSHelper.ExecuteScalar(commandText).ToString();
        }

        /// <summary>
        ///获取彩票赔率
        /// </summary>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataSet GetLotSetList(string type, string condition = "")
        {
            string commandText = string.Format(@"

declare @type int
set @type={0}

begin try
SELECT ROW_NUMBER() over(order by a.setid ) id,a.[setid]
      ,a.[lotterytype]
      ,a.[bttypeid]
      ,a.[odds]
      ,a.[addtime]
      ,b.item,b.type
into  #list
  FROM owzx_lotsetodds a
  join owzx_lotteryset b on a.bttypeid= b.bttypeid and a.lotterytype= @type
  {1}

if( @type in (1,2,6))
begin
select * from  #list where id>=1 and id<=14

select * from  #list where id>=15 and id<=28
end
else if( @type in (9))
begin
select * from  #list where type=12

select * from  #list where type=16
end
else if( @type in (4,5))
begin
select * from  #list where id>=1 and id<=3

select * from  #list where id>=4 and id<=5
end
else if( @type in (7))
begin
select * from  #list where id>=1 and id<=5

select * from  #list where id>=6 and id<=10
end
else if( @type in (8))
begin
select * from  #list where id>=1 and id<=9

select * from  #list where id>=10 and id<=17
end
else if( @type in (3))
begin
select * from  #list --where id>=1 and id<=3

select * from  #list --where id>=4 and id<=5
end

end try
begin catch
select ERROR_MESSAGE() state
end catch

", type,condition);

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText, null);
        }
        
        #endregion

        #region 自动投注
        /// <summary>
        /// 添加自动投注
        /// </summary>
        /// <param name="chag"></param>
        /// <returns></returns>
        public string AddAutoBett(MD_AutoBett mode)
        {
            try
            {
                DbParameter[] parms = {
                                        GenerateInParam("@uid", SqlDbType.Int,4, mode.Uid),
                                        GenerateInParam("@lotteryid", SqlDbType.Int,4, mode.LotteryId),
                                        GenerateInParam("@selmodeid", SqlDbType.Int,4, mode.SelModeId),
                                        GenerateInParam("@startexpect", SqlDbType.VarChar,50, mode.StartExpect),
                                        GenerateInParam("@maxbettnum", SqlDbType.Int,4, mode.MaxBettNum),
                                        GenerateInParam("@mingold", SqlDbType.Int,4, mode.MinGold),
                                        GenerateInParam("@autobettnum", SqlDbType.Int,4, mode.AutoBettNum)
                                       
                                    };
                string commandText = string.Format(@"
begin try
begin tran t1
if exists(select 1 from owzx_userbettmodel where uid=@uid and lotterytype=@lotterytype)
begin


end
else
begin
INSERT INTO [owzx_userautobett]
           ([uid]
           ,[lotteryid]
           ,[selmodeid]
           ,[startexpect]
           ,[maxbettnum]
           ,[mingold]
           ,[autobettnum]
           ,[isstart]
          )
VALUES (@uid,@lotteryid,@selmodeid,@startexpect,@maxbettnum,@mingold,@autobettnum,@isstart)
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
        public string UpdateAutoBett(MD_AutoBett mode)
        {
            return "";
        }

        /// <summary>
        /// 停止自动投注
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string StopAutoBett(int uid, int lotterytype)
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
            ", uid,lotterytype);
            return RDBSHelper.ExecuteScalar(commandText).ToString();
        }

        /// <summary>
        ///获取自动投注
        /// </summary>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataTable GetAutoBett(string condition = "")
        {
            string commandText = string.Format(@"
begin try
if OBJECT_ID('tempdb..#list') is not null
  drop table #list

SELECT ROW_NUMBER() over(order by a.modeid desc) id
      ,a.[name]
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

            return RDBSHelper.ExecuteTable(CommandType.Text, commandText,null)[0];
        }
        
        #endregion
    }
}
