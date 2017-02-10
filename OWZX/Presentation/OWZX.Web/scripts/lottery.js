/*竞猜*/
/*倒计时*/
function GetRTime(ctime, fcnum)
{
    //ns 开奖时间
    var nS = ctime;
    if (nS > 0)
    {
        //alert(document.getElementById('check_reload').checked);
        //alert(nS);
        nS = nS - 1;
        stopSencond = stopSencond - 1;
        if (stopSencond < 0)
        {
            document.getElementById("RemainS").innerHTML = "已经停止竞猜，还有<strong style='color:#ff0000;'>" + nS + "</strong>秒开奖 ";
            $("." + fcnum + "_status a").removeClass("a0").text("开奖中");
        }
        else
        {
            document.getElementById("RemainS").innerHTML = "还有<strong style='color:#ff0000;'>" + stopSencond + "</strong>秒停止竞猜，<strong style='color:#ff0000;'>" + nS + "</strong>秒开奖 ";
        }
        document.getElementById("RemainTitle").innerHTML = "第<strong>" + fcnum + "</strong>期";
        if (0 == 1)
        {
            document.getElementById("RemainTitle").innerHTML = "<span class='form_game'>距离第" + fcnum + "期开奖还有</span>";
            document.getElementById("RemainS").innerHTML = "<span class='form_game'>" + nS + "</span><span class='form_game'>秒</span>";
        }


    }
    else
    {
        var isReload = document.getElementById('reloadshow').value;
        document.getElementById("RemainTitle").innerHTML = "<strong>第" + fcnum + "期</strong>";
        if (isReload == 2)
        {
            document.getElementById("RemainS").innerHTML = "正在开奖中请等待！";
        }
        else
        {
            document.getElementById("RemainS").innerHTML = "已开奖 <a href='/qcgeass.aspx' style='color:red; font-size:12px;'>请刷新</a>";
        }

        nS = nS - 1;
        if (isReload == 2)
        {
            if (nS == -5 || nS == -10 || nS == -15 || nS == -20 || nS == -25)
            {
                window.location = '/qcgeass.aspx';
            }
        }

        if (nS <= -30)
            clearTimeout(tiner);
    }
    if (nS > -30)
        tiner = setTimeout("GetRTime(" + nS + "," + fcnum + ")", 1000);
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