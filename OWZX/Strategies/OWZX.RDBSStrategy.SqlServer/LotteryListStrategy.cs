using OWZX.Core;
using System;
using System.Collections.Generic;
using System.Data;
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
        public DataSet GetLotteryByType(string type, string pageindex, string pagesize, int uid=-1)
        {
            string sql = string.Format(@"
declare @uid int=19,@type int=10

select  type,expect,orderresult,first,second,three,result,resultnum,resulttype,status 
from owzx_lotteryrecord 
where type=@type and status=2 
and DATEDIFF(minute,opentime,GETDATE()) between 0 and 5

select type,expect,opentime,status,DATEDIFF(SECOND,GETDATE(),opentime) remains 
from owzx_lotteryrecord
where type=@type and status in (0,1)
and DATEDIFF(SECOND,GETDATE(),opentime) between 0 and 300

--用户投注盈亏
if OBJECT_ID('tempdb..#temp') is not null
drop table #temp

select a.lotteryid type,a.lotterynum expect,a.money,b.luckresult
into #temp
from owzx_bett a
join owzx_bettprofitloss b on a.bettid=b.bettid where a.uid=@uid and 
 datediff(day,a.addtime,GETDATE())=0

select  (select COUNT(1) from owzx_bett where uid=@uid and 
 datediff(day,addtime,GETDATE())=0) totalbett,
(select isnull(SUM(luckresult),0) from #temp) totalwin,
 case when (select COUNT(1) from #temp)=0 then 0 else (cast((select COUNT(1) from #temp where luckresult>0) /(select COUNT(1) from #temp)as decimal(18,2))) end winpert
 

declare @total int=(select COUNT(1) from owzx_lotteryrecord where type=@type)
 
if OBJECT_ID('tempdb..#lottery') is not null
 drop table #lottery
select top 20 type,expect,orderresult,first,second,three,result,resultnum,resulttype,status
into #lottery 
from owzx_lotteryrecord 
where type=@type order by lotteryid desc


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



");

            return RDBSHelper.ExecuteDataset(sql);
        }
        #endregion
    }
}
