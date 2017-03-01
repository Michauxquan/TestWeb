/// <reference path="D:\我的\项目\蛋蛋竞猜\Qg_EggQuiz\Qg_EggQuiz\qcgeass.aspx" />
var PRESSNUM = '1,3,6,10,15,21,28,36,45,55,63,69,73,75,75,73,69,63,55,45,36,28,21,15,10,6,3,1';
var nub1 = new Array(1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 63, 69, 73, 75, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 63, 69, 73, 75);
var maxnum = 20000000; //最大投注金额
var minnum = 10;
var BeforePeriods;
$(document).ready(function ()
{
     
    //点击投注模式
    $(".img_bt1").click(function ()
    {
        clear();
        var i = $(this).attr("attr");
        //if (i == 1|| i == 2 || i == 3 || i == 4)
        //{
        //    betttype = "1";
        //}
        //else if (i == 7 || i == 8|| i == 9 || i == 10)
        //{
        //    betttype = i;
        //}
        $(this).css("background", "url(../../images/xy28_bg.gif)");//img_bt2.gif
        setValue(i);
        //getAllpceggs();
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


    //点击每个栏目倍数

    //$("input[name='checkboxd']").each(function (i)
    //{
    //    $(this).click(function ()
    //    {
    //        return false;
    //    });

    //})

    $('input[type="checkbox"]').each(function (i, v) {
        $(v).click(function () { 
            if (!$(v).prop("checked")) {
                $(v).prop("checked", true);
                $(v).parent().next("td").children("input").val(nub1[i - 1]);
            } else {
                $(v).prop("checked", false);
                $(v).parent().next("td").children("input").val("");
            }
            var total = 0;
            $("#panel").find("input[name='SMONEY']").each(function () {
                if (!$(this).attr("readonly")) {
                    var txt_value = $.trim($(this).val()).replace(/,/gi, "");
                    if (txt_value && !isNaN(txt_value)) {
                        total += parseInt(txt_value);
                    }
                }
            });
            $("#totalvalue").text(ver(total + ""));
            $("#totalvalue").attr("");
        });
    });
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
            var dom = $(this).parent().next("td").next("td").next("td").next("td").children("input");
            if (!dom.attr("disabled"))
            {
                if (!dom.prop("checked"))
                {
                    dom.parent().next("td").children("input").val(nub1[i]); //改了
                    dom.prop("checked", true);
                } else
                {
                    dom.parent().next("td").children("input").val("");
                    dom.prop("checked", false);
                }
                getAllpceggs()
            }
        })
    }).css("cursor", "pointer");

    ////点击反选按钮
    //$(".touzhu2").eq(0).click(function () { ani_select(); })
    //点击清除按钮
    $(".touzhu2").eq(1).click(function () { bettmodel = ""; clear(); })
    ////刷新赔率
    //$(".touzhu1").eq(0).click(function () { refreshd(Periods); })
    ////上期投注
    $("#provbett_btn").click(function () {
        getprovbettinfo(1);
    });
    //setTimeout(function() { getprovbettinfo(0) },800);
    getprovbettinfo(0);
    //点击整体的倍数
    $("#border_out1_l").find("span").click(function ()
    {
        var peilv = $(this).text().replace("倍", "");
        if ($('#betsLeft').val() != "") {
            var s = parseFloat($('#betsLeft').val()) * peilv
            if (s == 0) {
                $('#betsLeft').val('');
            } else {
                $('#betsLeft').val(s.toFixed(0));
            }
        } else {
            setAllvalue(peilv);
        }
        getAllpceggs();
        //var peilv = $(this).text().replace("倍", "");
        //setAllvalue(peilv);
        //getAllpceggs();
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
        } else {
            console.log(1);
            $(this).parent().prev("td").children("input").attr("checked", "checked");
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

function getprovbettinfo(type) {
    $.post('/nwlottery/getprovbettinfo', { 'type': $('#provbett_btn').data("id"), lotterynum: type ==0 ? expect:"" }, function (data) {
        var dt = JSON.parse(data); 
        for (var i = 0; i < dt.length; i++) {
            if (dt[i].lotterynum == expect) {
                var arr = dt[i].bettinfo.split(';'); 
                $.each(arr, function (i) {
                    var arritem = arr[i].split(':');  
                    if (parseInt(arritem[0]) + "" != "NaN") {
                        $("#span_" + (parseInt(arritem[0])).toString()).html(ver(arritem[1]));
                    } else {
                        $("[id='span_" + arritem[0] + "']").html(ver(arritem[1]));
                    }
                });
            }
            if (dt[i].lotterynum != expect && type == 1 ) {
                UserMode(dt[i].bettinfo.split(';'), false, true);
                break;
            }
        } 
    });
}


function is_new_game() {
    var arr = [22, 23, 24, 25, 26, 27, 43, 45, 46, 47, 48, 49, 50, 51, 52, 28];
    var is_self = false;
    //for (var i in arr) {
    //    if (game_id == arr[i]) {
    //        is_self = true;
    //    }
    //}
    return is_self;
}
//判断正整数 
function checkRate(val) {
    var re = /^[0-9]*[1-9][0-9]*$/;
    if (!re.test(val)) {
        return false;
    } else {
        return true;
    }
}
//定格梭分配
function usefenpei() {
    var totalScore = 0;
    var perScore = 0;
    var totalPressScore = 0;
    var data = PRESSNUM.split(",");
    var Input_Score = $('#betsLeft').val();
    if (isNaN(Input_Score)) {
        $('#betsLeft').val('');
        alert('分配分必须为数字!');
        return;
    } else {
        if (Input_Score < 1) {
            $('#betsLeft').val('');
            alert('分配分必须大于0!');
            return;
        }
        if (checkRate(Input_Score) == false) {
            $('#betsLeft').val('');
            alert('分配分必须正整数!');
            return;
        }
        var checked_num = 0;
        for (var i = 0; i < data.length; i++) {
            if ($("#txt_" + i).parent().prev("td").children("input").prop("checked")) {
                checked_num = checked_num + 1;
            }
        }
        if (Input_Score < checked_num) {
            alert('分配分不够!');
            return;
        }
        if (Input_Score > maxnum) {
            $('#betsLeft').val(maxnum);
            return;
        }
    }

    for (var i = 0; i < data.length; i++) {
        if ($("#txt_" + i).parent().prev("td").children("input").prop("checked")) {
            var vval = $("#txt_" + i).val();
            perScore = 0;
            if (vval != null && vval != "" && !isNaN(parseInt(vval))) {
                totalScore += parseInt(vval);
            }
        }
    }
    for (var i = 0; i < data.length; i++) {
        if ($("#txt_" + i).parent().prev("td").children("input").prop("checked")) {
            var vval = $("#txt_" + i).val();
            perScore = 0;
            if (vval != null && vval != "" &&!isNaN(parseInt(vval))) {
                if (Input_Score <= maxnum) {
                    perScore = Input_Score / totalScore * parseInt(vval);
                } else {
                    perScore = mymoney / totalScore * parseInt(vval);
                }
                $("#txt_" + i).val(parseInt(perScore));
                totalPressScore += parseInt(perScore);
            }
        }
    }
    $("#totalvalue").html(totalPressScore);
}
function chips(num) { 
    $("#betsLeft").val(num);
    if (!is_new_game()) { 
        usefenpei();
    }
}
 
//梭哈
function useSuoha() {
    var totalScore = 0;
    var perScore = 0;
    var totalPressScore = 0;
    var data = PRESSNUM.split(",");
    for (var i = 0; i < data.length; i++) {
        if ($("#txt_" + i).parent().prev("td").children("input").prop("checked")) {
            totalScore += parseInt($("#txt_" + i).val());
        }
    }
    for (var i = 0; i < data.length; i++) {
        if ($("#txt_" + i).parent().prev("td").children("input").prop("checked")) {
            if (mymoney <= maxnum) {
                perScore = mymoney / totalScore * parseInt($("#txt_" + i).val());
            }
            else {
                perScore = maxnum / totalScore * parseInt($("#txt_" + i).val());
            }
            $("#txt_" + i).val(parseInt(perScore));
            totalPressScore += parseInt(perScore);
        }
    }
    $("#totalvalue").html(totalPressScore);
}
//标准投注模式设定方法
function setValue(num)
{
    if (lotterytype == 1 || lotterytype == 2 || lotterytype == 6)
    {
        for (var i = 0; i < mode[num].length; i++)
        {
            var id_num = mode[num][i];
            var id_name = "#txt_" + mode[num][i]; 
            if (!$(id_name).attr("readonly"))
            {
                $(id_name).val(nub[id_num]);
                $(id_name).parent().prev("td").children("input").prop("checked", true);
            }
        }
    } else if (lotterytype == 4 || lotterytype == 5)
    {
        for (var i = 0; i < mode36[num].length; i++) {
            var id_num = mode36[num][i];
            var id_name = "#txt_" + mode36[num][i];
            if (!$(id_name).attr("readonly")) {
                $(id_name).val(fc36[id_num-1]);
                $(id_name).parent().prev("td").children("input").prop("checked", true);
            }
        }
    }
    else if (lotterytype == 7)
    {
        for (var i = 0; i < modegj[num].length; i++) {
            var id_num = modegj[num][i];
            var id_name = "#txt_" + modegj[num][i];
            if (!$(id_name).attr("readonly")) {
                $(id_name).val('10');
                $(id_name).parent().prev("td").children("input").prop("checked", true);
            }
        }
    } else if (lotterytype == 8)
    {
        for (var i = 0; i < modegyj[num].length; i++) {
            var id_num = modegyj[num][i];
            var id_name = "#txt_" + modegyj[num][i];
            if (!$(id_name).attr("readonly")) {
                $(id_name).val(pkgyj[id_num-3]);
                $(id_name).parent().prev("td").children("input").prop("checked", true);
            }
        }
    }
    getAllpceggs();
}
//清除方法
function clear()
{
    $(".img_bt1").css("background", "url(../../images/img_bt1.png) left no-repeat");
    $("#panel").find("input[name='SMONEY']").each(function (i)
    {
        if (!$(this).attr("readonly"))
        {
            $(this).val("");
        }
    });
    $("#panel").find("input[name='checkboxd']").prop("checked", false);
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
function ani_select() {
    $("input[name='checkboxd']").each(function(i) {
        if (!$(this).attr("disabled")) {
            if (!$(this).prop("checked")) {
                $(this).parent().next("td").children("input").val(nub1[i]);
                $(this).prop("checked", true);
            } else {
                $(this).parent().next("td").children("input").val("");
                $(this).prop("checked", false);
            }
        }
    });
    getAllpceggs();
}


//选择自定义模式 
function personmode(id)
{
    bettmodel = id;
    $.ajax({
        type: "get",
        url: "/nwlottery/getbettmode",
        data:{"id":id},
        success: function (data)
        {
            var dt = JSON.parse(data);

            UserMode(dt.Bettinfo.split(';'));

        }
    });
}

//自定义投注模式
function UserMode(arr,flag,isprev)
{
    if (StrTimeOut == "-1" && typeof (isprev)==undefined && !isprev)
    {
        layer.alert("该期已经截止投注！", { icon: 2, title: "提示" });
        return false;
    }
    clear();
    $.each(arr, function (i)
    {
        var arritem = arr[i].split(':'); 
        if (this != "")
        {
            //不可选的号，不处理
            if ($("#txt_" + (parseInt(arritem[0])).toString()).attr("readonly"))
            {
                return;
            }
            if (flag)
            {
                $("#txt_" + (parseInt(arritem[0])).toString()).parent().prev("td").children("input").attr("disabled", true);
                $("#txt_" + (parseInt(arritem[0])).toString()).attr("readonly", true).attr("disabled", true);
            } else
            {
                $("#txt_" + (parseInt(arritem[0])).toString()).parent().prev("td").children("input").prop("checked", true);
            } 
            if (parseInt(arritem[0]) + "" != "NaN") {
                $("#txt_" + (parseInt(arritem[0])).toString()).val(ver(arritem[1]));
            } else {
                $("[id='txt_" +arritem[0]+"']").val(ver(arritem[1]));
            }
        }
    });
    getAllpceggs();
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


var first = 0;
//取总的投注金币
function getAllpceggs()
{
    if ($('#betsLeft').val() != "") {
        usefenpei();
    }
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
    })

    $("#totalvalue").text(ver(total + ""));
    $("#totalvalue").attr("")

}
function setpeilv(a_cis, a_cis1)
{
    if (a_cis != "")
    {
        $.each(a_cis, function (i)
        {
            var v = this + "";
            //		$("#txt"+i).parent().prev("td").prev("td").text(v);	//上期赔率
            $("#txt_" + i).parent().prev("td").prev("td").prev("td").text(v); //上期赔率
        })
    }
    if (a_cis1 != "")
    {
        $.each(a_cis1, function (i)
        {
            var v = this + "";
            //		$("#txt"+i).parent().prev("td").prev("td").prev("td").text(v);	//当前赔率
            $("#txt_" + i).parent().prev("td").prev("td").text(v); //当前赔率
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

    //if (StrTimeOut <= 0)
    //{
    //    layer.alert("第<span  style='color :Red;'>" + expect + "</span>期投注已截止！", { icon: 2, title: "提示" });
    //    isconfirmenable = true
    //    return false;
    //}
    //else
    if (t < minnum)
    {
        layer.alert("最小投注额[" + minnum + "]乐豆！", { icon: 2, title: "提示" });
        isconfirmenable = true
        return false;
    }
    else if (t > maxnum)
    {
        layer.alert("对不起，总投注额不能超过投注上限[" + maxnum + "]！", { icon: 2, title: "提示" });
        isconfirmenable = true
        return false;
    } else if (t == 0)
    {
        layer.alert("请投注！", { icon: 2, title: "提示" });
        isconfirmenable = true
        return false;
    } else if (t > mymoney)
    {
        layer.alert("您的乐豆不足,请充值！", { icon: 2, title: "提示" });
        isconfirmenable = true
        return false;

    }
    else
    {
        var str = [];
        for (var i = 0; i < 28; i++)
        {
            var txt_value = $.trim($("#txt_" + i).val()).replace(/,/gi, "");
            str.push(txt_value);
        }
        $("#ALLSMONEY").val(str.join(","));
        var message = "确认你投注？将扣除你<span id='postgoldeggs' style='color :Red;font-weight:bold'>" + t + "</span>乐豆！"

        layer.confirm(message, {
            btn: ['确定', '取消'] //按钮
        }, function ()
        {
            datapost();
            layer.closeAll('dialog');
        }, function ()
        {
            //必须加上，否则取消不成功
        }, { icon: 3, title: "提示" });
        t = ver(String(t)); //将数字转字符串后千分位 
        $("#postgoldeggs").html(t);
        $("#SMONEYSUM").val(t);


        isconfirmenable = true;
    }
}

//执行投注
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
                var src = $(this).parent().parent().find("td").eq(0).find("span").text();
                var num = src;
                if (isNaN(num))
                {
                    arrbettnew += (num) + ":" + ipval + ";";
                    arrbettnum += (num) + ";";
                    
                } else
                {
                    arrbettnew += (num.length == 1 ? ("0" + num) : num) + ":" + ipval + ";";
                    arrbettnum += (num.length == 1 ? ("0" + num) : num) + ";";
                }
            }
        }
    });

    $.post("/nwlottery/addbettinfo",
               {
                   "lotterytype": lotterytype, "fcnum": expect, "bettTotalEggs": $("#totalvalue").text().replace(/,/gi, ""),
                   "cusbettinfo": arrbettnew.substr(0, arrbettnew.length - 1), "bettnumber": arrbettnum.substr(0, arrbettnum.length - 1),
                   "bettmodel": bettmodel
               },
               function (data)
               {
                   if (data == "1")
                   {
                       //成功
                       layer.alert("第<span  style='color :Red;'>" + expect + "</span>期投注成功！", { icon: 1, title: "提示" })
                       if (lotterytype == 1 || lotterytype == 2 || lotterytype == 4 || lotterytype == 5 || lotterytype == 6)
                       {
                           $(".temp_content").load("/nwlottery/_content", { "type": lotterytype, "page": 1 });
                       } else if (lotterytype == 3)
                       {
                           $(".temp_content").load("/nwlottery/", { "type": lotterytype, "page": 1 });
                       }
                       else if (lotterytype == 9)
                       {
                           $(".temp_content").load("/nwlottery/_contentlhb", { "type": lotterytype, "page": 1 });
                       }
                       else if (lotterytype == 7 || lotterytype == 8)
                       {
                           $(".temp_content").load("/nwlottery/_contentpk", { "type": lotterytype, "page": 1 });
                       }
                   } else
                   {
                       var msg = "投注失败";
                       if (data == "2")
                           msg = "投注失败，第<span  style='color :Red;'>" + expect + "</span>期投注已截止！";
                       else if (data == "3")
                           msg = "您的乐豆不足,请充值！";

                       layer.alert(msg, { icon: 2, title: "提示" })
                   }

               });
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
    var bodyheight = $('#bb_body').height();
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


function otherMode(num)
{
    //全
    if (o == 0)
    {
        $("[name = 'tbChk']:checkbox").attr("checked", "checked");
        for (var i = 0; i < cc; i++)
        {
            $("#tbNum" + i).val(parseInt(data[i]));
        }
    }
    //双
    if (o == 1)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 2 == 0)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }
        }
    }
    //单
    if (o == 2)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 2 == 1)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }
        }
    }
    //小
    if (o == 3)
    {
        var num = data.length / 2;
        for (var i = 0; i < cc; i++)
        {
            if (GTYPE == 11 || GTYPE == 7)
            {
                if (i < num - 1)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");

                }
            }
            else
            {
                if (i < num)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");
                }
            }
        }
    }
    //大
    if (o == 4)
    {
        var num = data.length / 2;
        for (var i = 0; i < cc; i++)
        {
            if (GTYPE == 11 || GTYPE == 7)
            {
                if (i >= num - 1)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");

                }
            }
            else
            {
                if (i >= num)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");

                }
            }

        }
    }
    //中
    if (o == 5)
    {
        var num = data.length / 3;
        for (var i = 0; i < cc; i++)
        {
            if (GTYPE == 17)
            {
                if (i >= num - 1 & i < 2 * num)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");
                }
            }
            else if (GTYPE == 7)
            {
                if (i >= num - 1 & i <= 2 * num)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");
                }

            }

            else if (GTYPE == 11)
            {
                if (i > num - 1 && i < 2 * num)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");
                }
            }
            else
            {
                if (i >= num & i < 2 * num - 1)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");
                }
            }
        }
    }
    //边
    if (o == 6)
    {
        var num = data.length / 4;
        for (var i = 0; i < cc; i++)
        {
            if (GTYPE == 7)
            {
                if (i < num || i >= 3 * num - 1)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");
                }
            }

            else if (GTYPE == 17)
            {
                if (i <= num || i >= 3 * num - 1)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");
                }
            }
            else if (GTYPE == 11)
            {
                if (i <= num || i > 2 * num + 2)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");
                }
            }

            else
            {
                if (i < num + 3 || i > 3 * num - 4)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");
                }
            }

        }
    }
    //大单
    if (o == 7)
    {
        var num = (data.length + imore) / 2;
        for (var i = 0; i < cc; i++)
        {

            if ((i + istart) > num && (i + istart) % 2 == 1)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }


        }
    }
    //小单
    if (o == 8)
    {
        var num = (data.length + imore) / 2;
        for (var i = 0; i < cc; i++)
        {


            if ((i + istart) < num && (i + istart) % 2 == 1)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }


        }
    }
    //大双
    if (o == 9)
    {
        var num = (data.length + imore) / 2;
        for (var i = 0; i < cc; i++)
        {

            if ((i + istart) >= num && (i + istart) % 2 == 0)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }


        }
    }
    //小双
    if (o == 10)
    {
        var num = (data.length + imore) / 2;
        for (var i = 0; i < cc; i++)
        {

            if ((i + istart) < num && (i + istart) % 2 == 0)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }


        }
    }
    //大边
    if (o == 11)
    {
        var num = (data.length + imore) / 3;
        if (GTYPE == 7)
        {
            istart--;
        }
        for (var i = 0; i < cc; i++)
        {
            if (GTYPE == 11)
            {
                if ((i + istart) > 2 * num + 1)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");
                }
            }
            else
            {
                if ((i + istart) > 2 * num - 1)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");
                }
            }

        }
    }
    //小边
    if (o == 12)
    {
        var num = (data.length + imore) / 3;
        for (var i = 0; i < cc; i++)
        {

            if ((i + istart) <= num)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }


        }
    }
    //单边
    if (o == 13)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 10 == 0)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //双边
    if (o == 14)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 10 == 1)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }

    //0尾
    if (o == 15)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 10 == 0)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //1尾
    if (o == 16)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 10 == 1)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //2尾
    if (o == 17)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 10 == 2)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //3尾
    if (o == 18)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 10 == 3)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //4尾
    if (o == 19)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 10 == 4)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //小尾
    if (o == 20)
    {
        if (GTYPE == 10)
        {
            for (var i = 0; i < cc; i++)
            {
                if ((i + istart) % 10 < 5 && (i + istart) % 10 >= 0)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");
                }

            }
        } else
        {
            for (var i = 0; i < cc; i++)
            {
                if ((i + istart) % 10 < 5)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");
                }

            }
        }
    }
    //5尾
    if (o == 21)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 10 == 5)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //6尾
    if (o == 22)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 10 == 6)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //7尾
    if (o == 23)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 10 == 7)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //8尾
    if (o == 24)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 10 == 8)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //9尾
    if (o == 25)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 10 == 9)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //大尾
    if (o == 26)
    {
        if (GTYPE == 10)
        {
            for (var i = 0; i < cc; i++)
            {
                if ((i + istart) % 10 >= 5)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");
                }

            }
        }
        else
        {
            for (var i = 0; i < cc; i++)
            {
                if ((i + istart) % 10 >= 5)
                {
                    $("#tbNum" + i).val(parseInt(data[i]));
                    $("#tbChk" + i).attr("checked", "checked");
                }

            }
        }
    }
    //3余0
    if (o == 27)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 3 == 0)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //3余1
    if (o == 28)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 3 == 1)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //3余2
    if (o == 29)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 3 == 2)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //4余0
    if (o == 30)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 4 == 0)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //4余1
    if (o == 31)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 4 == 1)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //4余2
    if (o == 32)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 4 == 2)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //4余3
    if (o == 33)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 4 == 3)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //5余0
    if (o == 34)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 5 == 0)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //5余1
    if (o == 35)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 5 == 1)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //5余2
    if (o == 36)
    {

        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 5 == 2)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //5余3
    if (o == 37)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 5 == 3)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
    //5余4
    if (o == 38)
    {
        for (var i = 0; i < cc; i++)
        {
            if ((i + istart) % 5 == 4)
            {
                $("#tbNum" + i).val(parseInt(data[i]));
                $("#tbChk" + i).attr("checked", "checked");
            }

        }
    }
}