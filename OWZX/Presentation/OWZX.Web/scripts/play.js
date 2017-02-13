/// <reference path="D:\我的\项目\蛋蛋竞猜\Qg_EggQuiz\Qg_EggQuiz\qcgeass.aspx" />
var nub = new Array(1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 63, 69, 73, 75, 75, 73, 69, 63, 55, 45, 36, 28, 21, 15, 10, 6, 3, 1);
var nub1 = new Array(1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 63, 69, 73, 75, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 63, 69, 73, 75);
var mode = new Array([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27], //全包 0
					 [1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27], //单 1
					 [0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26], //双 2
					 [14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27], //大 3
					 [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13], //小 4
					 [10, 11, 12, 13, 14, 15, 16, 17], //中 5
					 [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27], //边 6
					 [15, 17, 19, 21, 23, 25, 27], //大单 7 
					 [1, 3, 5, 7, 9, 11, 13], //小单 8
					 [14, 16, 18, 20, 22, 24, 26], //大双 9
					 [0, 2, 4, 6, 8, 10, 12], //小双 10
					 [18, 19, 20, 21, 22, 23, 24, 25, 26, 27], //大边
					 [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], //小边
					 [1, 3, 5, 7, 9, 19, 21, 23, 25, 27], //单边
					 [0, 2, 4, 6, 8, 18, 20, 22, 24, 26], //双边
					 [0, 10, 20], //0尾
					 [1, 11, 21], //1尾
					 [2, 12, 22], //2尾
					 [3, 13, 23], //3尾
					 [4, 14, 24], //4尾
					 [0, 1, 2, 3, 4, 10, 11, 12, 13, 14, 20, 21, 22, 23, 24], //小尾
					 [5, 15, 25], //5尾
					 [6, 16, 26], //6尾
					 [7, 17, 27], //7尾
					 [8, 18], //8尾
					 [9, 19], //9尾
					 [5, 6, 7, 8, 9, 15, 16, 17, 18, 19, 25, 26, 27], //大尾
					 [0, 3, 6, 9, 12, 15, 18, 21, 24, 27], //3余0
					 [1, 4, 7, 10, 13, 16, 19, 22, 25], //3余1
					 [2, 5, 8, 11, 14, 17, 20, 23, 26], //3余2
					 [0, 4, 8, 12, 16, 20, 24], //4余0
					 [1, 5, 9, 13, 17, 21, 25], //4余1
					 [2, 6, 10, 14, 18, 22, 26], //4余2
					 [3, 7, 11, 15, 19, 23, 27], //4余3
					 [0, 5, 10, 15, 20, 25], //5余0
					 [1, 6, 11, 16, 21, 26], //5余1
					 [2, 7, 12, 17, 22, 27], //5余2
					 [3, 8, 13, 18, 23], //5余3
					 [4, 9, 14, 19, 24]//5余4
					 );

var maxnum = 100000000; //最大投注金额
var minnum = 10;
$(document).ready(function ()
{
    //点击投注模式
    $(".img_bt1").click(function ()
    {
        clear();
        var i = $(this).attr("attr");
        if (i == 1|| i == 2 || i == 3 || i == 4)
        {
            betttype = "1";
        }
        else if (i == 7 || i == 8|| i == 9 || i == 10)
        {
            betttype = i;
        }
        $(this).css("background", "url(../img/xy28_bg.gif)");//img_bt2.gif
        setValue(i);
        getAllpceggs();
    }).hover(
  function ()
  {
      $(this).css("color", "#FF6600");
  },
  function ()
  {
      $(this).css("color", "#515151"); //鼠标移过后样式
  }

);

    var btps = new Array("10", "100", "500", "1000");
    //点击每个栏目倍数
    //$("#panel").find("input[name='Add']").click(function () {
    //    var peilv = $(this).val();
    //    var txt = $(this).parent().prev("td").children("input");
    //    if (!txt.attr("readonly")) {
    //        var txt_value = txt.val().replace(/,/gi, "");
    //        if (!txt_value) { return; }
    //        var new_value = Math.floor(txt_value * peilv);
    //        txt.val(ver(new_value + ""));
    //        getAllpceggs();
    //    }
    //})
    $("#panel").find("input[name='Add']").click(function ()
    {
        var peilv = $(this).val();
        var peiimg = $(this).parent().children("img")[0];
        var pei = peiimg.src;
        var imgNum = parseInt(pei.substr(pei.lastIndexOf('/') + 3, 1));
        if (imgNum <= 4)
        {
            var txt = $(this).parent().prev("td").children("input");
            var txt_value = txt.val().replace(/,/gi, "");
            if (txt_value.length > 8 || txt_value == 0 || txt_value == "") { return; }
            if (imgNum == 4)
            {
                peiimg.src = "../img/number/dt" + imgNum + ".png";
            }
            else
            {
                peiimg.src = "../img/number/dt" + (imgNum + 1) + ".png";
            }
            peilv = btps[(imgNum - 1)];


            if (!txt.attr("readonly"))
            {
                var new_value = Math.floor(txt_value * peilv);
                if (new_value.toString().length > 8)
                    txt.val(ver(new_value.toString().substr(0, 8) + ""));
                else
                    txt.val(ver(new_value.toString() + ""));
                getAllpceggs();
            }
        }

    })
    $("#panel").find("input[name='Loss']").click(function ()
    {
        var peilv = $(this).val();
        var peiimg = $(this).parent().children("img")[0];
        var pei = peiimg.src;
        var imgNum = parseInt(pei.substr(pei.lastIndexOf('/') + 3, 1));
        if (imgNum >= 1)
        {
            var txt = $(this).parent().prev("td").children("input");
            var txt_value = txt.val().replace(/,/gi, "");
            if (!txt_value || txt_value == 0 || txt_value == "") { return; }
            if (imgNum == 1)
            {
                peiimg.src = "../img/number/dt" + imgNum + ".png";
            } else
            {
                peiimg.src = "../img/number/dt" + (imgNum - 2) + ".png";

            }
            peilv = btps[(imgNum - 1)];

            if (!txt.attr("readonly"))
            {
                var new_value = Math.floor(txt_value / peilv);
                txt.val(ver(new_value.toString() + ""));
                getAllpceggs();
            }
        } //else if (imgnum = 5)
        //{
        //    peiimg.src = "../img/number/dt" + ('1') + ".png";
        //    peilv = "1";
        //};

    })
    $("input[name='checkboxd']").each(function (i)
    {
        $(this).click(function ()
        {
            return false;
        });

    })
    ////点击checkbox,反向选择     不能修改
    //$("input[name='checkboxd']").each(function (i)
    //{
    //    $(this).click(function ()
    //    {
    //        if ($(this).attr("checked"))
    //        {
    //            $(this).parent().next("td").children("input").val(nub1[i]);
    //        } else
    //        { $(this).parent().next("td").children("input").val(""); }
    //        getAllpceggs()
    //    })
    //})
    //点击号码 
    $(".btnumber").each(function (i)
    {
        $(this).click(function ()
        {
            if (betttype != "0")
            {
                return;
            }
            var dom = $(this).parent().next("td").next("td").next("td").children("input");
            if (!dom.attr("disabled"))
            {
                if (!dom.attr("checked"))
                {
                    dom.parent().next("td").children("input").val(nub1[i]); //改了
                    dom.attr("checked", true);
                } else
                {
                    dom.parent().next("td").children("input").val("");
                    dom.attr("checked", false);
                }
                getAllpceggs()
            }
        })
    }).css("cursor", "pointer");

    ////点击反选按钮
    //$(".touzhu2").eq(0).click(function () { ani_select(); })
    //点击清除按钮
    $(".touzhu2").eq(1).click(function () { clear(); })
    ////刷新赔率
    //$(".touzhu1").eq(0).click(function () { refreshd(Periods); })
    ////上期投注
    //$(".touzhu1").eq(1).click(function () { personmode(BeforePeriods); })
    //点击整体的倍数
    $("#border_out1_l").find("span").click(function ()
    {
        var peilv = $(this).text().replace("倍", "");
        setAllvalue(peilv);
        getAllpceggs();
    }).hover(
  function ()
  {
      $(this).css("color", "#FF6600");
  },
  function ()
  {
      $(this).css("color", "#8d5000"); //鼠标移过后样式
  }
);

    //输入投注数据
    $("#panel").find("input[name='SMONEY']").keyup(function ()
    {
        var regex = /^[1-9]\d{0,}$/;
        var val = $(this).val();
        if (!regex.test(val))
        {
            val = val.replace(/\D/g, '');
            $(this).val(val);
        }
        if (!regex.test(val))
        {
            $(this).val(val.substring(1));
            getAllpceggs();
        } else
        {
            $(this).parent().prev("td").children("input").attr("checked", true);
            getAllpceggs();
        }
    }).blur(function ()
    {
        $(this).val(ver($(this).val()));
    }).focus(function ()
    {
        if ($(this).val().indexOf(",") > -1)
        {
            domvalue = $(this).val().replace(/,/gi, "");
            $(this).val(domvalue);
        }
        try
        {
            var obj = event.srcElement;
            var txt = obj.createTextRange();
            txt.moveStart('character', obj.value.length);
            txt.collapse(true);
            txt.select();
        } catch (e)
        {
        }
    });
});


//标准投注模式设定方法
function setValue(num)
{
    for (var i = 0; i < mode[num].length; i++)
    {
        var id_num = mode[num][i];
        var id_name = "#txt" + mode[num][i];
        if (!$(id_name).attr("readonly"))
        {
            $(id_name).val(nub[id_num]);
            $(id_name).parent().prev("td").children("input").attr("checked", true);
        }
    }
}
//清除方法
function clear()
{
    betttype = "0";
    $(".img_bt1").css("background", "url(../img/img_bt1.png) left no-repeat");
    $("#panel").find("input[name='SMONEY']").each(function (i)
    {
        if (!$(this).attr("readonly"))
        {
            $(this).val("");
        }
    });
    $("#panel").find("input[name='checkboxd']").attr("checked", false);
    $("#totalvalue").text("0");
}
//数字加千分符号
function ver(n)
{
    re = /(\d{1,3})(?=(\d{3})+(?:$|\.))/g
    return n.replace(re, "$1,")
}

//设置所有赔率
function setAllvalue(peilv)
{
    $("#panel").find("input[name='SMONEY']").each(function ()
    {
        if (!$(this).attr("readonly"))
        {
            var txt_value = $.trim($(this).val()).replace(/,/gi, "");
            if (txt_value && !isNaN(txt_value))
            {
                var new_value = Math.floor(txt_value * peilv);
                if (new_value.toString().length > 8)
                    $(this).val(ver(new_value.toString().substr(0, 8) + ""));
                else
                    $(this).val(ver(new_value.toString() + ""));
            }
        }
    });
}
//反选		  
function ani_select()
{
    $("input[name='checkboxd']").each(function (i)
    {
        if (!$(this).attr("disabled"))
        {
            if (!$(this).attr("checked"))
            {
                $(this).parent().next("td").children("input").val(nub1[i]);
                $(this).attr("checked", true);
            } else
            {
                $(this).parent().next("td").children("input").val("");
                $(this).attr("checked", false);
            }
        }
    }
									  )
    getAllpceggs();
}

//获取用户模式
function InitMd(id)
{
    $.ajax({
        type: "get",
        url: "../Data/CusBettOperater.ashx?qctype=bettmd&mdID=" + id,
        success: function (data)
        {
            var dt = JSON.parse(data);
            $("#usbettmode").html("");
            if (dt.Msg != "" && dt.Result)
            {
                var jsdt = JSON.parse(dt.Msg);
                var strb = "";
                $.each(jsdt, function (i, item)
                {
                    strb +=
            "<a href='javascript:personmode(" + item.ObjName + ")' style='margin-left:15px;'>" + item.Objjc + "</a>";
                })
                $("#usbettmode").html(strb);
            }

        }
    });
}
//自定义模式 
function personmode(id)
{
    $.ajax({
        type: "get",
        url: "../Data/CusBettOperater.ashx?qctype=bettmd&mdID=" + id,
        success: function (data)
        {
            var dt = JSON.parse(data);

            if (dt.Msg != "" && dt.Result)
            {
                var dtmsg = JSON.parse(dt.Msg)[0];
                var bttype = dtmsg.CheckBettType;

                UserMode(dtmsg.BettInfo.split(';'));

                $(".img_bt1").css("background", "url(../img/img_bt1.png) left no-repeat");
                if (bttype == "数字")
                    betttype = "0";
                switch (bttype)
                {
                    case "单":
                        $(".img_bt1").eq(0).css("background", "url(../img/xy28_bg.gif)"); betttype = "1";
                        break;
                    case "双":
                        $(".img_bt1").eq(1).css("background", "url(../img/xy28_bg.gif)"); betttype = "2";
                        break;
                    case "大": $(".img_bt1").eq(2).css("background", "url(../img/xy28_bg.gif)"); betttype = "3";
                        break;
                    case "小": $(".img_bt1").eq(3).css("background", "url(../img/xy28_bg.gif)"); betttype = "4";
                        break;
                }
            }

        }
    });
}

//刷新赔率 
function refreshd(id)
{
    $.ajax({
        type: "get",
        url: "pg28mode.aspx?refresh=" + id,
        error: function ()
        {
            //	alert("操作错误");
            //showmessage("10","操作错误！",LastIssue);
        },
        success: function (data, textStatus)
        {
            setpeilv("", data.split(",")); //当前赔率
        }
    });
}


//开始 由首页传递期号和开奖时间

////获取选择竞猜期号，是否已经开奖
//function getfcnumTime(fcnum)
//{
//    $.get("../Data/CusBettOperater.ashx?qctype=vdfctmot&fcnum=" + fcnum, function (data)
//    {
//        if (data)
//            StrTimeOut = "-1";
//        else
//            StrTimeOut = "1";
//    });
//}


//用户金币
//获取用户金币
function VadUsGold()
{
    var result = false;
    $.ajax({
        url: "../Data/CusBettOperater.ashx?qctype=vdusgold",
        type: "get",
        async: "false",
        success: function (data)
        {
            if (data != "")
            {
                mypceggs = data;
                if (parseInt(mypceggs) < 500)
                {
                    //验证是否能够领取
                    $.ajax({
                        type: "post",
                        url: '../Data/CusBettOperater.ashx',
                        data: { "qctype": "getgold", "type": "get" },
                        async: false,
                        success: function (data)
                        {
                            var dt = JSON.parse(data);
                            if (dt.Result)
                            {
                                showmessage("8", "目前您的账户上金币少于500，是否需要免费获取金币？", LastIssue);
                            }
                        }
                    });

                }
            }
        }
    });
    return result;

}

function getgoldop()
{
    $.ajax({
        type: "post",
        url: '../Data/CusBettOperater.ashx',
        data: { "qctype": "getgold", "type": "add" },
        async: false,
        success: function (data)
        {
            rm();
        }
    });
}

//结束

//页面载入时执行
function showvalue(arr, flag)
{
    if (StrTimeOut == "-1")
    {
        showmessage("3", num + "期已经截止投注！", LastIssue);
        return false;
    }
    //else if (IsGetEggs == "1")
    //{
    //    //$("#div_ad").css("display",""); 

    //    showmessage("8", "目前您的账户上金币少于5000，是否需要免费获取金币？", LastIssue);

    //}

    $.each(arr, function (i)
    {
        var arritem = arr[i].split(':');
        var selnum = (parseInt(arritem[0])).toString();
        if ($("#txt" + selnum).attr("readOnly"))
        {
            return;
        }
        if (flag)
        {
            $("#txt" + selnum).parent().prev("td").children("input").attr("disabled", true);
            $("#txt" + selnum).attr("readOnly", true).attr("disabled", true);
        } else
        {
            $("#txt" + selnum).parent().prev("td").children("input").attr("checked", true);
        }
        $("#txt" + selnum).val(ver(arritem[1]));
    });
    getAllpceggs();
}

//自定义投注模式
function UserMode(arr, flag)
{
    if (StrTimeOut == "-1")
    {
        showmessage("3", "该期已经截止投注！", LastIssue);
        return false;
        // $("#div_ad").css("display","");
        return;
    }
    clear();
    $.each(arr, function (i)
    {
        var arritem = arr[i].split(':');
        if (this != "")
        {
            //不可选的号，不处理
            if ($("#txt" + (parseInt(arritem[0])).toString()).attr("readonly"))
            {
                return;
            }
            if (flag)
            {
                $("#txt" + (parseInt(arritem[0])).toString()).parent().prev("td").children("input").attr("disabled", true);
                $("#txt" + (parseInt(arritem[0])).toString()).attr("readonly", true).attr("disabled", true);
            } else
            {
                $("#txt" + (parseInt(arritem[0])).toString()).parent().prev("td").children("input").attr("checked", true);
            }
            $("#txt" + (parseInt(arritem[0])).toString()).val(ver(arritem[1]));
        }
    });
    getAllpceggs();
}

var first = 0;
//取总的投注金币
function getAllpceggs()
{
    var total = 0;
    $("#panel").find("input[name='SMONEY']").each(function ()
    {
        if (!$(this).attr("readonly"))
        {
            var txt_value = $.trim($(this).val()).replace(/,/gi, "");
            if (txt_value && !isNaN(txt_value))
            {
                total += parseInt(txt_value);
            }
        }
    }
															   )

    $("#totalvalue").text(ver(total + ""));
    $("#totalvalue").attr("")


    if (readcookie("handflag") == "1")
    {
        if ($("#totalvalue").text() != 0 && first == 0)
        {
            first = 1;
            // $("#help_show1").css("display",""); 
            // $("#help_show").css("display","none"); 
        }
    }
    //if(total>maxnum){
    //alert("对不起，总投注金额不能超过投注上限！");}
    // if(total>mypceggs){
    //alert("您的余额不足！");}
}
function setpeilv(a_cis, a_cis1)
{
    if (a_cis != "")
    {
        $.each(a_cis, function (i)
        {
            var v = this + "";
            //		$("#txt"+i).parent().prev("td").prev("td").text(v);	//上期赔率
            $("#txt" + i).parent().prev("td").prev("td").prev("td").text(v); //上期赔率
        })
    }
    if (a_cis1 != "")
    {
        $.each(a_cis1, function (i)
        {
            var v = this + "";
            //		$("#txt"+i).parent().prev("td").prev("td").prev("td").text(v);	//当前赔率
            $("#txt" + i).parent().prev("td").prev("td").text(v); //当前赔率
        })

    }
}
var isconfirmenable = true;
//确认投注	
function comform()
{
    if (!isconfirmenable)
        return;
    t = $("#totalvalue").text().replace(/,/gi, "");
    t = parseInt(t);
    var str = [];
    if (StrTimeOut < 0)
    {
        showmessage("3", num + "期投注投注已截止！", LastIssue); isconfirmenable = true
        return false;
    }
    else if (t < minnum)
    {
        showmessage("11", "投注金额[" + minnum + "]积分起！", LastIssue); isconfirmenable = true
        return false;
    }
    else if (t > maxnum)
    {
        showmessage("11", "对不起，总投注金额不能超过投注上限[" + maxnum + "]！", LastIssue); isconfirmenable = true
        return false;
    } else if (t == 0)
    {
        showmessage("13", "请先投注！", LastIssue); isconfirmenable = true
        return false;
    } else if (t > mypceggs)
    {
        showmessage("12", "您的金币不足！", LastIssue); isconfirmenable = true
        return false;

    }
    else
    {
        var str = [];
        for (var i = 0; i < 28; i++)
        {
            var txt_value = $.trim($("#txt" + i).val()).replace(/,/gi, "");
            str.push(txt_value);
        }
        $("#ALLSMONEY").val(str.join(","));
        //$("#div_ad").css("display","");
        showmessage("9", "确认你投注？将扣除你<span id='postgoldeggs' style='color :Red;font-weight:bold'>" + t + "</span>个金币！<br/><input type='hidden' name='isdb' id='isdb' value='1'  style='margin-right:2px' />", LastIssue);
        t = ver(String(t)); //将数字转字符串后千分位 
        $("#postgoldeggs").html(t);
        $("#SMONEYSUM").val(t);
        //document.documentElement.clientHeight - document.getElementById("div_ad").offsetHeight  document.documentElement.clientWidth - document.getElementById("div_ad").offsetWidth
        document.getElementById("div_ad").style.top = (document.documentElement.scrollTop + document.body.scrollTop + ($(window).height() - $('#div_ad').height()) / 2) + "px";
        document.getElementById("div_ad").style.left = (document.documentElement.scrollLeft + ($(window).width() - $('#div_ad').width()) / 2) + "px";

        isconfirmenable = true;
    }
}



//是否按现模式自动投注
function ischecked()
{
    var isdb = document.getElementById("isdb")//子层
    var isdb_p = ""; //父层


    if (isdb.checked == true)
    {
        document.getElementById("isdb_p").value = "1";
    } else
    {
        document.getElementById("isdb_p").value = "0";
    }

}

//取消投注
function rm()
{
    //document.getElementById("isdb_p").value = "0";
    document.getElementById("div_ad").style.display = 'none';
    document.getElementById("parent_div").style.display = 'none';
}

//确认投注
function datapost()
{
    //chgsubmit();
    var arrbettnew = "";
    var arrbettnum = "";
    $("input[name='SMONEY']").attr("disabled", false);
    $("input[name='SMONEY']").each(function (i)
    {
        if (!$(this).attr("readOnly"))
        {
            var ipval = $(this).val();
            ipval = $.trim(ipval).replace(/,/gi, "");//去掉数字分割符
            if (ipval != "0" && ipval.trim() != "")
            {
                var src = $(this).parent().parent().find("td").eq(0).find("img").attr("src");
                var num = src.split('_')[1].split('.')[0];
                arrbettnew += (num.length == 1 ? ("0" + num) : num) + ":" + ipval + ";";
                arrbettnum += (num.length == 1 ? ("0" + num) : num) + ";";
            }
        }
    });
    $.post(
               "../Data/CusBettOperater.ashx?qctype=addbett",
               {
                   "fcnum": num, "bettTotalEggs": $("#totalvalue").text().replace(/,/gi, ""),
                   "cusbettinfo": arrbettnew.substr(0, arrbettnew.length - 1), "bettnumber": arrbettnum.substr(0, arrbettnum.length - 1),
                   "betttype": betttype
               },
               function (data)
               {
                   if (data != "")
                   {
                       var dt = JSON.parse(data);
                       if (dt.Result)
                       {
                           //成功
                           showmessage1(num);
                       } else
                       {
                           showmessage("1", dt.Msg, "");
                       }

                   }

               });
}

//滚动
function sc1()
{
    var top = (document.documentElement.scrollTop + document.body.scrollTop + ($(window).height() - $('#div_ad').height()) / 2) + "px";
    var left = (document.documentElement.scrollLeft + ($(window).width() - $('#div_ad').width()) / 2) + "px";

    $("#div_ad").css("top", top);
    $("#div_ad").css("left", left);
}
function scall()
{
    sc1();
}

window.onscroll = sc1;
function CleanMessage()
{
    document.getElementById("div_ad").style.display = 'none';
    document.getElementById("parent_div").style.display = 'none';
}
//投注后信息返回
function showmessage(flag, msg, NLid)
{

    ////弹出浮层
    $("#div_ad").css("display", "");
    sc1();
    switch (flag)
    {
        case "0":
            if (readcookie("handflag") == "1")
            {
                setcookie("handflag", "2");
            }
            window.location.href = "/qcgeass.aspx";
            break;
        case "1": //投注失败
            $(".content1").html(msg);
            $(".titleclose").html('<span class="title">投注失败</span><a onclick="rm()" class="close"><span>X</span></a>');
            $(".btnpane").html('<div style="text-align:center;"><a onclick="return rm();" style="width:72px;height:22px;background:url(../img/popup_btn.png) no-repeat;display:block;cursor:pointer; margin:0 auto"> </a></div>');
            break;
        case "2": //投注金币少于现有金币
            $(".content1").html(msg);
            $(".titleclose").html('<span class="title">投注失败</span><a onclick="rm()"   class="close"><span>X</span></a>');
            $(".btnpane").html('<div style="text-align:center;"><a onclick="return rm();" style="width:72px;height:22px;background:url(../img/popup_btn.png) no-repeat;display:block;cursor:pointer; margin:0 auto"> </a></div>');
            break;
        case "3": //已截至投注
            $(".content1").html(msg);
            $(".titleclose").html('<span class="title">投注失败</span><a onclick="bettsucc()"   class="close"><span>X</span></a>');
            $(".btnpane").html('<div style="float:left; "><a onclick="return CleanMessage()" style="width:90px;height:22px; background:url(../img/popup_btn.png) no-repeat;display:block;cursor:pointer;"> </a></div> ');
            break;
        case "4": //重复投注
            $(".content1").html(msg);
            $(".titleclose").html('<span class="title">投注失败</span><a onclick="bettsucc()"   class="close"><span>X</span></a>');
            $(".btnpane").html('<div style="text-align:center;"><a href="pgl.aspx" style="width:72px;height:22px;background:url(../img/popup_btn3.png) no-repeat;display:block;cursor:pointer; margin:0 auto"> </a></div>');
            break;
        case "5": //总投注金额超过上限
            $(".content1").html(msg);
            $(".titleclose").html('<span class="title">投注失败</span><a onclick="return rm();" class="close"><span>X</span></a>');
            $(".btnpane").html('<div style="text-align:center;"><a onclick="return rm();" style="width:72px;height:22px;background:url(../img/popup_btn.png) no-repeat;display:block;cursor:pointer; margin:0 auto"> </a></div>');
            break;
        case "6": //系统异常（数据库返回）
            $(".content1").html(msg);
            $(".titleclose").html('<span class="title">投注失败</span><a onclick="bettsucc()"   class="close"><span>X</span></a>');
            $(".btnpane").html('<div style="text-align:center;"><a href="pgl.aspx" style="width:72px;height:22px;background:url(../img/popup_btn3.png) no-repeat;display:block;cursor:pointer; margin:0 auto"> </a></div>');
            break;
        case "7": //系统异常（程序）
            $(".content1").html(msg);
            $(".titleclose").html('<span class="title">投注失败</span><a onclick="bettsucc()"   class="close"><span>X</span></a>');
            $(".btnpane").html('<div style="text-align:center;"><a href="pgl.aspx" style="width:72px;height:22px;background:url(../img/popup_btn3.png) no-repeat;display:block;cursor:pointer; margin:0 auto"> </a></div>');
            break;

        case "8": //金币少于500
            //$("#help_show").css("display","none");
            $(".content1").html(msg);
            $(".titleclose").html('<span class="title">金币少于500</span><a onclick="return rm();"  class="close"><span>X</span></a>');
            $(".btnpane").html('<div style="float:left; "><a href="javascript:getgoldop()" style="width:72px;height:22px;background:url(../img/popup_btn.png) no-repeat;display:block;cursor:pointer;"> </a></div> <div style="float:right;"><a onclick="return rm();"  style="width:72px;height:22px;background:url(../img/popup_btn2.png) no-repeat;display:block;cursor:pointer;"> </a></div> ');
            break;

        case "9": //确认投注
            $(".content1").html(msg);
            $(".titleclose").html('<span class="title">确认投注</span><a href="#" onclick="rm()" class="close"><span>X</span></a>');
            $(".btnpane").html('<div style="float:left; "><a onclick="return datapost()"  style="width:72px;height:22px; background:url(../img/popup_btn.png) no-repeat;display:block;cursor:pointer;"></a></div> <div style="float:right;"><a onclick="return rm();"  style="width:72px;height:22px; background:url(../img/popup_btn2.png) no-repeat;display:block;cursor:pointer;"></a></div> ');
            break;

        case "10": //系统异常
            $(".content1").html(msg);
            $(".titleclose").html('<span class="title">投注失败</span><a onclick="bettsucc()"  class="close"><span>X</span></a>');
            $(".btnpane").html('<div style="text-align:center;"><a href="*" style="width:72px;height:22px;background:url(../img/popup_btn3.png) no-repeat;display:block;cursor:pointer; margin:0 auto"> </a></div>');
            break;

        case "11": //投注金额超过上限
            $(".content1").html(msg);
            $(".titleclose").html('<span class="title">投注金额超过上限</span><a onclick="return rm();" class="close"><span>X</span></a>');
            $(".btnpane").html('<div style="text-align:center;"><a onclick="return rm();" style="width:72px;height:22px;background:url(../img/popup_btn.png) no-repeat;display:block;cursor:pointer; margin:0 auto"> </a></div>');
            break;

        case "12": //金币投余额不足
            $(".content1").html(msg);
            $(".titleclose").html('<span class="title">您的金币不足！</span><a onclick="return rm();" class="close"><span>X</span></a>');
            $(".btnpane").html('<div style="text-align:center;"><a onclick="return rm();" style="width:72px;height:22px;background:url(../img/popup_btn.png) no-repeat;display:block;cursor:pointer; margin:0 auto"> </a></div>');
            break;

        case "13": //没有投注
            $(".content1").html(msg);
            $(".titleclose").html('<span class="title">请先投注！</span><a onclick="return rm();" class="close"><span>X</span></a>');
            $(".btnpane").html('<div style="text-align:center;"><a onclick="return rm();" style="width:72px;height:22px;background:url(../img/popup_btn.png) no-repeat;display:block;cursor:pointer; margin:0 auto"> </a></div>');
            break;

        case "14": //投注邀请分享浮层
            $(".content1").html(msg);
            $(".titleclose").html('<span class="title">投注成功</span><a onclick="return rm();" class="close"><span>X</span></a>');
            $(".btnpane").html('<div style="text-align:center;"><ul><li><a href="javascript:void(0)" onclick="postToXL()"><img src="img/fc_img/fxdxl.gif" border="0"></a></li><li><a href="javascript:void(0)" onclick="postToWb()"><img src="img/fc_img/fxdtx.gif" border="0"></a></li><li><a href="javascript:void(0)" onclick="return rm();"><img src="img/fc_img/zbfx.gif" border="0"></a></li></ul></div>');
            break;
        case "15": //暂停投注
            rm();
            $("#stopsubtip").html(msg);
            $("#stoptip").css("display", "");
            $("#savetip").css("display", "none");
            return;
            break;
        case "16": //验证码错误
            $(".content1").html(msg);
            $(".titleclose").html('<span class="title">提示</span><a onclick="return rm();" class="close"><span>X</span></a>');
            $(".btnpane").html('<div style="text-align:center;"><a onclick="return rm();" style="width:72px;height:22px;background:url(../img/popup_btn.png) no-repeat;display:block;cursor:pointer; margin:0 auto"> </a></div>');
            return;
            break;
    }


    //弹出笼罩层
    var bodyheight =$('#bb_body').height();
    var parent_div = document.getElementById("parent_div");
    parent_div.style.display = 'block';
    parent_div.style.height = parseInt(bodyheight) + 'px';

}

//投注后信息返回
function showmessage1(issue)
{
    $(".content1").html('<div class="content_zc">第<span  style="color :Red;">' + issue + '</span>期投注成功！</div>');
    $(".titleclose").html('<span class="title">投注成功</span><a onclick="return bettsucc();" class="close"><span>X</span></a>');
    $(".btnpane").html('<div style="float:left; "><a onclick="return bettsucc();"  style="width:72px;height:22px; background:url(../img/popup_btn.png) no-repeat;display:block;cursor:pointer;"></a></div>');


    //弹出笼罩层
    var bodyheight = $('#bb_body').height();
    var parent_div = document.getElementById("parent_div");
    parent_div.style.display = 'block';
    parent_div.style.height = parseInt(bodyheight) + 'px';

}
function bettsucc()
{
    document.getElementById("div_ad").style.display = 'none';
    document.getElementById("parent_div").style.display = 'none';
    window.location = "/qcgeass.aspx"
}

//取消投注
function rm1()
{
    document.getElementById("isdb_p").value = "0";
    document.getElementById("div_ad").style.display = 'none';
    document.getElementById("parent_div").style.display = 'none';
}

//取消投注
function rm()
{
    isconfirmenable = true;
    document.getElementById("div_ad").style.display = 'none';
    document.getElementById("parent_div").style.display = 'none';
}

function getgoldeggs()
{
    //$("#help_show").css("display","none");
    ShowMsgo.show("/adcomate/pggetgoldeggsnew.aspx", 503, 518);
    document.getElementById("div_ad").style.display = 'none';
    document.getElementById("parent_div").style.display = 'none';


}
function closelinqu()
{
    ShowMsgo.cancel();
    window.location.reload(true);
}