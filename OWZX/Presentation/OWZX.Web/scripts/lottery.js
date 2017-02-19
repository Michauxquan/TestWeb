﻿/*竞猜*/
/*倒计时 剩余总时间，期号，总时间，封盘时间,结果期数*/
var tiner;
var traptime;
function GetRTime(type, ctime, fcnum, totalime, stoptime,prevnum)
{
    //处理上期未开奖，重启计时处理
    if (prevnum != undefined)
    {
        trap(type, fcnum - 1);
    }

    num = fcnum;
    //ns 开奖时间
    var nS = ctime;
    if (nS > 0)
    {
        nS = nS - 1;
        if (nS > stoptime && nS <= totalime)
        {
            //bett
            $(".remains").html(
                '第 <i class="bold">' + fcnum + '</i>期  还有<span class="ltwarn">' + (nS - 30).toString() + '</span>秒停止下注!');
            StrTimeOut = 1;
        }
        else if (nS > 0 && nS <= stoptime)
        {
            //stop
            if (nS != 0)
            {
                $(".remains").html(
                    '第 <i class="bold">' + fcnum + '</i>期  停止下注,还有<span class="ltwarn">' + nS + '</span>秒开奖!');
            } else
            {
                $(".remains").html(
                                '第 <i class="bold">' + fcnum + '</i>期  正在开奖,请稍后!');
            }
            if ($(".lot_btn_" + fcnum + " .ltoperate").text().toString().trim() != "正在开奖")
            {
                $(".lot_btn_" + fcnum).html("").html('<a href="#"><div class="ltoperate">正在开奖</div></a>');
            }
            StrTimeOut = -1;
        }

    }
    else
    {
        nS = nS - 1;
        StrTimeOut = -1;
        if (nS > -30)
        {
            $(".remains").html(
                '第 <i class="bold">' + fcnum + '</i>期  正在开奖,请稍后!');
        }

        if (nS == -5 || nS == -10 || nS == -15 || nS == -20 || nS == -25 || nS == -30)
        {
            
            $(".remains").html('Loading......');
            $.post("/nwlottery/lotteryopen", { "type": lotterytype, "expect": num }, function (data)
            {
                //是否开奖，开奖且是首页则自动刷新，不是首页则提示刷新
                //未开奖 显示新一期的倒计时，另起一个计时器，获取未开奖信息直到获取到结果
                if (data == "1")
                {
                    clearTimeout(tiner);
                    clearTimeout(traptime);

                    if ($(".sec_head a:eq(0)").hasClass("hot"))
                    {
                        $(".lot_content").load("/nwlottery/_index", { "type": lotterytype });
                    } else
                    {
                        $(".remains").html('第 <i class="bold">' + fcnum + '</i>期  已开奖,请刷新!');
                        //$(".temp_content").load("/nwlottery/_content", { "type": lotterytype, "page": 1 });
                    }
                    return;
                } else
                {
                    if (nS == -30)
                    {
                        if ($(".sec_head a:eq(0)").hasClass("hot"))
                        {
                            $(".lot_content").load("/nwlottery/_index", { "type": lotterytype });
                            trap(lotterytype, fcnum);
                        }else
                        {
                            $(".remains").html('第 <i class="bold">' + fcnum + '</i>期  已开奖,请刷新!');
                            //$(".temp_content").load("/nwlottery/_content", { "type": lotterytype, "page": 1 });
                        }
                    }
                }
            });
        }

        if (nS <= -30)
            clearTimeout(tiner);
    }
    if (nS > -30)
        tiner = setTimeout("GetRTime(" + type + "," + nS + "," + fcnum + "," + totalime + "," + stoptime +")", 1000);
}
function trap(type,fcnum)
{
    $.post("/nwlottery/lotteryopen", { "type": lotterytype, "expect": fcnum }, function (data)
    {
        if (data == "1")
        {
            clearTimeout(tiner);
            clearTimeout(traptime);
            $(".lot_content").load("/nwlottery/_index", { "type": lotterytype });
        }else
        {
            traptime = setTimeout("trap(" + type + "," + fcnum + ")", 2000);
        }
    });
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
//暂停时间
var samplingRate = function (interval)
{
    var mark;
    mark = 0;
    return function ()
    {
        var now;
        now = Date.now();
        if (now - mark < interval)
        {
            return false;
        }
        return mark = now;
    };
};

var sampling = samplingRate(1000);


function chgTimes(numID, times)
{
    //if (sampling())
    {
        var numIDx = "#" + numID;
        $(numIDx).attr("unable", "false");
        if (parseInt($(numIDx).val() * times) > 0)
        {
            var result = parseInt($(numIDx).val() * times);
            if (result < 1)
            {
                $("#totalvalue").text(parseInt($("#totalvalue").text()) + parseInt("1") - parseInt($(numIDx).val()));
                $(numIDx).val("1");
            } else if (result.toString().length > 9)
            {
                var res = result.toString().substr(0, 9);
                $("#totalvalue").text(parseInt($("#totalvalue").text()) + parseInt(res) - parseInt($(numIDx).val()));
                $(numIDx).val(res);
            } else
            {
                $("#totalvalue").text(parseInt($("#totalvalue").text()) + result - parseInt($(numIDx).val()));
                $(numIDx).val(result);
            }
        }
    }
}



