using OWZX.Core;
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

select  type,expect,orderresult,first,second,three,result,resultnum,resulttype,status 
from owzx_lotteryrecord 
where type=@type and status=2 
and DATEDIFF(minute,opentime,GETDATE()) between 0 and 5

--用户投注盈亏
if OBJECT_ID('tempdb..#temp') is not null
drop table #temp

select a.lotteryid type,a.lotterynum expect,a.money,b.luckresult
into #temp
from owzx_bett a
join owzx_bettprofitloss b on a.bettid=b.bettid where a.uid=@uid and 
 datediff(day,a.addtime,GETDATE())=0


declare @total int=(select COUNT(1) from owzx_lotteryrecord where type=@type)

select type,expect lastnumber,opentime,status,DATEDIFF(SECOND,GETDATE(),opentime) remains, 
(select COUNT(1) from owzx_bett where uid=@uid and 
 datediff(day,addtime,GETDATE())=0) tdbettnum,
(select isnull(SUM(luckresult),0) from #temp) tdprof,
 case when (select COUNT(1) from #temp)=0 then 0 
 else (cast((select COUNT(1) from #temp where luckresult>0) /(select COUNT(1) from #temp)as decimal(18,2))) end winpercent,
 @total totalcount
from owzx_lotteryrecord
where type=@type and status in (0,1)
and DATEDIFF(SECOND,GETDATE(),opentime) between 0 and 300

 
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
 join owzx_bettprofitloss b on a.bettid=b.bettid
 join  #lottery c on a.lotteryid=c.type and a.lotterynum=c.expect


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
join owzx_bettprofitloss b on a.bettid=b.bettid and a.uid={1}
join owzx_lotteryrecord c on a.lotteryid=c.type and a.lotterynum=c.expect and c.type={0}

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
end
else if({0}=12)
begin
--大小单双龙虎豹
end
", type);
            return RDBSHelper.ExecuteDataset(sql);
        }
        #endregion
    }
}
