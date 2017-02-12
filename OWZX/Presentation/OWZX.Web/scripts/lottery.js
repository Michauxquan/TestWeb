/*竞猜*/
/*倒计时 剩余总时间，期号，下注时间，封盘时间*/
function GetRTime(type,ctime, fcnum,bettime,stoptime)
{
    //ns 开奖时间
    var nS = ctime;
    if (nS > 0)
    {
        //alert(document.getElementById('check_reload').checked);
        //alert(nS);
        nS = nS - 1;
        if (nS >= stoptime && nS <= bettime)
        {
            //bett
            $(".remains").html(
                '第 <i class="bold">' + fcnum + '</i>期  还有<span class="ltwarn">' + (nS - 30).toString() + '</span>秒停止下注!');
        } else
        {
            //stop
            $(".remains").html(
                '第 <i class="bold">' + fcnum + '</i>期  停止下注,还有<span class="ltwarn">' + nS + '</span>秒开奖!');
            if ($(".lot_" + fcnum + "_btn .ltoperate").text() != "正在开奖")
            {
                $(".lot_" + fcnum + "_btn").html("").html('<a href="#"><div class="ltoperate">正在开奖</div></a>');
            }
        }

    }
    else
    {
        if(nS>-30)
        {
            $(".remains").html(
                '第 <i class="bold">' + fcnum + '</i>期  正在开奖,请稍后!');
        }

        nS = nS - 1;
        
        if (nS == -5 || nS == -10 || nS == -15 || nS == -20 || nS == -25)
        {
            $(".remains").html('Loading......');
            $(".lot_content").load("/nwlottery/lt28data", { "type": type, "pageindex": 1 })
        }

        if (nS <= -30)
            clearTimeout(tiner);
    }
    if (nS > -30)
        tiner = setTimeout("GetRTime("+type+"," + nS + "," + fcnum + "," + bettime + "," + stoptime + ")", 1000);
}
/*数字增加，分割*/
function transStr(str)
{
    str = str.toString()
    var begin = "";
    var after = "";
    var l;
    var str2 = "";
    if (str.indexOf(".") > 0)
    {
        begin = str.substring(0, str.indexOf("."));
        after = str.substring(str.indexOf("."), str.length);
    }
    else
    {
        begin = str;
    }

    l = begin.length / 3;
    if (l > 1)
    {
        for (var i = 0; i < l;)
        {
            str2 = "," + begin.substring(begin.length - 3, begin.length) + str2;
            begin = begin.substring(0, begin.length - 3);
            l = begin.length / 3;
        }
        if (after.length < 3)
        {
            str2 = begin + str2 + after;
        } else
        {
            str2 = begin + str2 + after
        }
        return str2.substring(1);
    }
    else
    {
        if (after.length < 3)
        {
            return str;

        } else
        {
            return str;
        }
    }
}