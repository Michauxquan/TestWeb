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

select top 1 type,expect,orderresult,first,second,three,result,resultnum,resulttype,status 
from owzx_lotteryrecord 
where type=@type and status=2 order by lotteryid desc


--用户今日投注盈亏
if OBJECT_ID('tempdb..#temp') is not null
drop table #temp

select a.lotteryid type,a.lotterynum expect,a.money
,a.winmoney as luckresult,a.isread 
into #temp
from owzx_bett a 
where a.uid=@uid and a.lotteryid=@type and  datediff(day,a.addtime,GETDATE())=0  


declare @total int=(select COUNT(1) from owzx_lotteryrecord where type=@type)


if OBJECT_ID('tempdb..#now') is not null
drop table #now

declare @temptotal int=0,@tdbettnum int=0,@tdprof bigint=0
declare @winpercent decimal(18,2)
set @temptotal=(select COUNT(1) from #temp )

select @tdbettnum=COUNT(1) from owzx_bett where uid=@uid and lotteryid=@type and 
 datediff(day,addtime,GETDATE())=0

 select @tdprof=(case when @temptotal=0 then 0 
 else (select isnull(SUM(cast(luckresult as bigint)),0)-isnull(SUM(cast(money as bigint)),0) from #temp where isread=1 ) end)

 select @winpercent= (case when @temptotal=0 then 0 
 else  (cast ((select COUNT(1) from #temp where luckresult>0) as decimal(18,2))  / 
 cast ((select COUNT(1) from #temp) as decimal(18,2))) *100 end ) 

 
select top 1  type,expect lastnumber,opentime,status,case when @type=6 then DATEDIFF(SECOND,GETDATE(),opentime)+25 else DATEDIFF(SECOND,GETDATE(),opentime) end remains, 
@tdbettnum tdbettnum,
 @tdprof tdprof,
 @winpercent winpercent,
 @total totalcount
into #now
from owzx_lotteryrecord
where type=@type and status in (0,1)
and DATEDIFF(SECOND,GETDATE(),opentime)>=0 and DATEDIFF(SECOND,GETDATE(),opentime)<=(case when type in(1,4,9,7,8,14,16,17,27,28,29,30) then 300 
when type =6 then 115
when type in(2,5,15) then 210 
when type in(3,12,10,11) then 120
else 600 end)


if not exists(select 1 from #now)
begin
select top 1 type,expect lastnumber,opentime,status,case when @type=6 then DATEDIFF(SECOND,GETDATE(),opentime)+25 else DATEDIFF(SECOND,GETDATE(),opentime) end remains, 
@tdbettnum tdbettnum,
 @tdprof tdprof,
 @winpercent winpercent,
 @total totalcount
 from owzx_lotteryrecord
where type=@type and status in (0,1) and DATEDIFF(second,getdate(),opentime)>0  order by lotteryid
end
else
begin
select * from #now
end


--添加假人
declare @jiaren varchar(50)='' , @jiaren2 varchar(50)='',@jiaren3 varchar(50)=''

select  @jiaren=isnull(remark ,'') from owzx_sys_basetype where  outtypeid=@type and parentid=47
if(@jiaren<>'')
begin
    declare @expectnum varchar(50)
    select @expectnum=lastnumber from #now where type=@type
    if((select top 1 fakeeggnum from owzx_lotteryrecord where type=@type and status in (0) and expect=@expectnum order by lotteryid)=0)
    begin
        declare @jnum int=0 ,@jnum1 int=0,@jnum2 int=0,@jenum bigint=0,@avfee decimal(18,2)=0.00,@jenum1 bigint=0

        set @jiaren2=substring(@jiaren,0,charindex('_',@jiaren))
        set @jiaren3=substring(@jiaren,charindex('_',@jiaren)+1,len(@jiaren))
        set @jnum= cast(substring(@jiaren2,0,charindex('|',@jiaren2)) as int)  
        set @jenum= cast(substring(@jiaren2,charindex('|',@jiaren2)+1,len(@jiaren2)) as bigint)
        set @jnum1=cast(isnull(substring(@jiaren3,0,charindex('|',@jiaren3)),0) as int) 
        if(@jnum1>0)
        begin
            set @jenum1= cast(isnull(substring(@jiaren3,charindex('|',@jiaren3)+1,len(@jiaren3)),0) as bigint)     
            set @avfee=cast((@jenum1/@jnum1) as decimal(18,2))
            set @jnum1=cast(ceiling(rand() * @jnum1) as int)
            set @jnum2=cast(@jnum1*6/5 as int)
            set @jnum= @jnum1+@jnum
            set @jenum=@jenum+ cast( (@jnum1*@avfee) as bigint)
        end
        update owzx_lotteryrecord set fakeuserscount=@jnum, fakeeggnum=@jenum , fakewinnum=@jnum2 where type=@type and status =0 and expect=@expectnum
    end

end



 
if OBJECT_ID('tempdb..#lottery') is not null
 drop table #lottery

select * 
into #lottery
from (
select ROW_NUMBER() over(order by lotteryid desc) id,type,expect,orderresult,first,second,three,result,opentime,
resultnum,resulttype,status,luckyuserscount as winperson,betteggnum as totalbett ,bettnum bettperson,fakeuserscount, fakeeggnum , fakewinnum 
from owzx_lotteryrecord where type=@type 
) a  
where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex


if OBJECT_ID('tempdb..#bettprof') is not null
 drop table #bettprof

 select a.*,b.luckresult
 into #bettprof
 from owzx_bett a
 join  #lottery c on a.lotteryid=c.type and a.lotterynum=c.expect
 left join owzx_bettprofitloss b on a.bettid=b.bettid


select a.*,isnull(cast(e.luckresult as decimal(18,2)),0)luckresult,
isnull(e.money,0) money
from #lottery a 
left join #temp e 
on a.type=e.type and a.expect=e.expect  
order by expect desc





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
select 1 from owzx_lotteryrecord where type={0} and expect='{1}' and status=2
",type,expect);

            return RDBSHelper.Exists(sql);
        }
        /// <summary>
        /// 获取投注记录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public DataTable GetUserBett(int type, int uid, int pageindex, int pagesize,string condition)
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pagesize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageindex)
                                  };

            string sql = string.Format(@"
if OBJECT_ID('tempdb..#lotrecord') is not null
 drop table #lotrecord

select  ROW_NUMBER() over(order by a.bettid desc) id,a.lotterynum,a.addtime,c.resultnum,a.money,a.winmoney,
case when c.status=2 then (
case when ISNULL(a.winmoney,0)>=0 then ISNULL(a.winmoney,0)-a.money else ISNULL(a.winmoney,0) end ) else 0 end win,
a.bettid,a.bettinfo
,c.status,c.opentime,a.lotteryid,isnull(c.resulttype,'') resulttype,c.orderresult
into #lotrecord  
from owzx_bett a 
join owzx_lotteryrecord c on a.lotteryid=c.type and a.lotterynum=c.expect and c.type={0} and a.uid={1}
{2}

declare @total int=(select count(1) from #lotrecord )

if(@pagesize=-1)
begin
select * ,@total totalcount from #lotrecord
end
else
begin
select * ,@total totalcount from #lotrecord where id>@pagesize*(@pageindex-1) and id <=@pagesize*@pageindex
end

", type,uid,condition);
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
                
              declare @type int ={0}
declare @min varchar(5), @sec varchar(5),@expect varchar(50),@totalsec varchar(5)

if(@type in (1,4,9,7,8,14,16,17)) 
begin

if exists(select top 1 1 from owzx_lotteryrecord where type=@type and status !=2 and 
(DATEDIFF(second,opentime,getdate()) >= -300 and DATEDIFF(second,opentime,getdate())<=0) order by lotteryid )
begin
select top 1 @expect=expect, 
@totalsec= CONVERT(VARCHAR(10),DATEDIFF(SECOND,getdate(),opentime))
from owzx_lotteryrecord where type=@type and status  !=2  and (DATEDIFF(second,opentime,getdate()) >= -300 and DATEDIFF(second,opentime,getdate())<=0)
order by lotteryid

select @expect expect,@totalsec time

end


end
else if(@type in (2,5,15)) 
begin

if exists(select top 1 1 from owzx_lotteryrecord where type=@type and status  !=2  and 
(DATEDIFF(second,opentime,getdate()) >= -210 and DATEDIFF(second,opentime,getdate())<=0) order by lotteryid )
begin
select top 1 @expect=expect, 

@totalsec= CONVERT(VARCHAR(10),DATEDIFF(SECOND,getdate(),opentime))
from owzx_lotteryrecord where type=@type and status  !=2 and (DATEDIFF(second,opentime,getdate()) >= -210 and DATEDIFF(second,opentime,getdate())<=0)
order by lotteryid

select @expect expect,@totalsec time
end
end
else if(@type in (6)) 
begin

if exists(select top 1 1 from owzx_lotteryrecord where type=@type and status !=2 and 
(DATEDIFF(second,opentime,getdate()) >= -115 and DATEDIFF(second,opentime,getdate())<=0) order by lotteryid )
begin
select top 1 @expect=expect, 

@totalsec= CONVERT(VARCHAR(10),DATEDIFF(SECOND,getdate(),opentime))
from owzx_lotteryrecord where type=@type and status  !=2  and (DATEDIFF(second,opentime,getdate()) >= -115 and DATEDIFF(second,opentime,getdate())<=0)
order by lotteryid

select @expect expect,@totalsec time
end
end
else if(@type in (3,10,11,12)) 
begin

if exists(select top 1 1 from owzx_lotteryrecord where type=@type and status !=2 and 
(DATEDIFF(second,opentime,getdate()) >= -120 and DATEDIFF(second,opentime,getdate())<=0) order by lotteryid )
begin
select top 1 @expect=expect, 
@totalsec= CONVERT(VARCHAR(10),DATEDIFF(SECOND,getdate(),opentime))
from owzx_lotteryrecord where type=@type and status !=2  and (DATEDIFF(second,opentime,getdate()) >= -120 and DATEDIFF(second,opentime,getdate())<=0)
order by lotteryid

select @expect expect,@totalsec time
end
end
else
begin
select '?' expect,'维护中' time
end           ", type);
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
        public DataSet GetLotSetList(string type, string condition = "",bool islhcbett=false)
        {
            StringBuilder strb = new StringBuilder();
            strb.AppendFormat(@"

declare @type int
set @type={0}

begin try
if OBJECT_ID('tempdb..#list') is not null
drop table #list

SELECT ROW_NUMBER() over(order by a.setid ) id,a.[setid]
      ,a.[lotterytype]
      ,a.[bttypeid]
      ,isnull(currodds,a.[odds]) [odds]
      ,ISNULL(prevodds,a.[odds])  prevodds
      ,a.[addtime]
      ,b.item,b.type,b.nums,a.pitem
into  #list
  FROM owzx_lotsetodds a
  join owzx_lotteryset b on a.bttypeid= b.bttypeid and a.lotterytype= @type
  {1}

if( @type in (1,2,3,6,14,15,16))
begin
select * from  #list where id>=1 and id<=14

select * from  #list where id>=15 and id<=28
end
else if( @type in (9))
begin
select * from  #list where type=12

select * from  #list where type=16
end
else if( @type in (4,5,17))
begin
select * from  #list where id>=1 and id<=3

select * from  #list where id>=4 and id<=5
end
else if( @type in (7,10))
begin
select * from  #list where id>=1 and id<=5

select * from  #list where id>=6 and id<=10
end
else if( @type in (8))
begin
select * from  #list where id>=1 and id<=9

select * from  #list where id>=10 and id<=17
end
else if( @type in (11))
begin
select * from  #list where id>=1 and id<=6

select * from  #list where id>=7 and id<=12
end
else if( @type in (12))
begin
select * from  #list where id>=1 and id<=8

select * from  #list where id>=9 and id<=18
end
else if(@type=27)
begin

select * from  #list where id>=1 and id<=17

select * from  #list where id>=18 and id<=21

select id,setid,lotterytype,bttypeid,odds,prevodds,item,type,
replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(pitem,'第一名','1')
,'第二名','2')
,'第三名','3')
,'第四名','4')
,'第五名','5')
,'第六名','6')
,'第七名','7')
,'第八名','8')
,'第九名','9')
,'第十名','10') pitem from  #list where id>=22 and id<=161
select * from  #list where id>=162 and id<=171
end
else if(@type=28)
begin
select * from  #list where id>=1 and id<=11
select * from  #list where id>=12 and id<=22
end
else if(@type=29)
begin
select * from  #list where id>=1 and id<=5
select * from  #list where id>=6 and id<=10
end
else if(@type=30)
begin
select * from  #list where id=1
select * from  #list where id=2
end
", type,condition);
            
                strb.AppendFormat(@"
else if( @type in (13))
begin
select * from 
(
select top 17 * from  #list where (bttypeid>=12 and bttypeid <=28 ) order by bttypeid  ) a
union all
select * from (
select top 17 * from  #list where (bttypeid>=29 and bttypeid <=36) 
 order by bttypeid) a

select * from (
select top 17 * from  #list where (bttypeid>=37 and bttypeid <=38) or (bttypeid>=135 and bttypeid <=141) order by bttypeid) a
union all
select * from 
(
select top 17 * from  #list where (bttypeid>=142 and bttypeid <=156 ) order by bttypeid  ) a

select * from (
select top 8 * from  #list where setid between 249 and 256 order by bttypeid  ) a

select * from (
select top 12 * from  #list where bttypeid between 161 and 172 order by bttypeid  ) a
end
");

            strb.AppendFormat(@"

end try
begin catch
select ERROR_MESSAGE() state
end catch
");

            return RDBSHelper.ExecuteDataset(CommandType.Text, strb.ToString(), null);
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
                                        GenerateInParam("@lotterytype", SqlDbType.Int,4, mode.LotteryId),
                                        GenerateInParam("@uid", SqlDbType.Int,4, mode.Uid),
                                        GenerateInParam("@selmodeid", SqlDbType.Int,4, mode.SelModeId),
                                        GenerateInParam("@fcnum", SqlDbType.VarChar,50, mode.StartExpect),
                                        GenerateInParam("@maxbtnum", SqlDbType.Int,4, mode.MaxBettNum),
                                        GenerateInParam("@mingoldnum", SqlDbType.Int,4, mode.MinGold),
                                        GenerateInParam("@allmd", SqlDbType.VarChar,1000, mode.AllSelMode)
                                    };

                return RDBSHelper.ExecuteScalar(CommandType.StoredProcedure, "addautobett", parms).ToString();
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
            if exists(select 1 from owzx_userautobett where uid={0} and lotteryid={1})
            begin
            update a set a.isstart=0 from owzx_userautobett a where uid={0} and lotteryid={1}
            select '停止成功' state
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
        public DataTable GetAutoBett(int pageindex, int pagesize,string condition = "")
        {
            DbParameter[] parms = {
                                      GenerateInParam("@pagesize", SqlDbType.Int, 4, pagesize),
                                      GenerateInParam("@pageindex", SqlDbType.Int, 4, pageindex)
                                  };

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

            return RDBSHelper.ExecuteTable(CommandType.Text, commandText,parms)[0];
        }

        /// <summary>
        /// 获取用户自动投注信息
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="lotterytype"></param>
        /// <returns></returns>
        public DataSet  GetUserAtBett(int uid, int lotterytype)
        {
            string sql = string.Format(@"
declare @uid int={0},@lotteryid int ={1}


select a.uid, a.selmodeid,c.name, a.startexpect, a.maxbettnum, a.mingold,
(select name from owzx_userbettmodel where modeid=c.wintype) wintype,
(select name from owzx_userbettmodel where modeid=c.losstype) losstype,c.betttotal,
a.autobettnum 
 from owzx_userautobett a 
join owzx_users b on a.uid=b.uid and b.uid=@uid and a.IsStart=1 and a.lotteryid=@lotteryid
join owzx_userbettmodel c on a.uid=c.uid and a.selmodeid=c.modeid


select a.modeid, a.name,a.uid, a.bettnum, a.bettinfo, a.betttotal, 
(select name from owzx_userbettmodel where modeid=a.wintype) wintype,
(select name from owzx_userbettmodel where modeid=a.losstype) losstype
from owzx_userbettmodel a
join owzx_users b on a.uid=b.uid  and b.uid=@uid and a.lotterytype=@lotteryid
", uid,lotterytype);

            return RDBSHelper.ExecuteDataset(sql);
        }
        
        #endregion

        #region 竞猜管理
        /// <summary>
        /// 添加彩票信息
        /// </summary>
        /// <param name="chag"></param>
        /// <returns></returns>
        public string AddLottery(MD_LotteryInfo mode)
        {
            try
            {
                DbParameter[] parms = {
                                        GenerateInParam("@lotterytype", SqlDbType.Int,4, mode.lotterytype),
                                        GenerateInParam("@lotteryname", SqlDbType.VarChar,50, mode.lotteryname),
                                        GenerateInParam("@isstart", SqlDbType.Bit,1, mode.isstart==true?1:0)
                                       
                                    };
                string commandText = string.Format(@"
begin try
begin tran t1

INSERT INTO [owzx_lotteryinfo]
           ([lotterytype]
           ,[lotteryname]
           ,[isstart]
           )
VALUES (@lotterytype,@lotteryname,@isstart)


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
        /// 更新彩票信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string UpdateLottery(MD_LotteryInfo mode)
        {
            DbParameter[] parms = {
                                        GenerateInParam("@lotid", SqlDbType.Int,4, mode.lotid),
                                        GenerateInParam("@isstart", SqlDbType.Bit,20, mode.isstart)
                                       
                                    };
            string commandText = string.Format(@"
begin try
begin tran t1

if exists(select 1 from owzx_lotteryinfo where lotid=@lotid)
begin
update a set a.[lotteryname]=@lotteryname,a.isstart=@isstart
from [owzx_lotteryinfo] a where  lotid=@lotid
       
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
        /// 删除彩票信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DelLottery(int setid)
        {
            string commandText = string.Format(@"
            begin try
            if exists(select 1 from  owzx_lotteryinfo where lotid={0})
            begin
            delete from owzx_lotteryinfo where lotid={0} 
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
        ///获取彩票信息
        /// </summary>
        /// <param name="condition">没有where</param>
        /// <returns></returns>
        public DataSet GetLotteryList(string condition = "")
        {
            string commandText = string.Format(@"

declare @type int
set @type={0}

begin try
if OBJECT_ID('tempdb..#list') is not null
drop table #list

SELECT ROW_NUMBER() over(order by a.lotid ) id,a.[lotid]
      ,a.[lotterytype]
      ,a.[lotteryname]
      ,a.[isstart]
      ,a.[addtime]
      ,a.[updatetime]
into  #list
  FROM owzx_lotteryinfo a
 
end try
begin catch
select ERROR_MESSAGE() state
end catch

", condition);

            return RDBSHelper.ExecuteDataset(CommandType.Text, commandText, null);
        }
        
        #endregion
    }
}
