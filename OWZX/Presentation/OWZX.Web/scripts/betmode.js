var tzmscount = 10;//投注模式最大数设置
//投注模式 号码的乐豆数
var ModelDatas = new Array(
    new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
    new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
    new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
    new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
    new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
    new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
    new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
    new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
    new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
    new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
var ModeNames = new Array("", "", "", "", "", "", "", "", "", "");
var CurrentMode = 0;
var ModeCount = 0;
var modelbetttype = new Array("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
    "", "", "", "", "", "", "", "", "", "", "", "","", "", "", "", "", "", "", "", "");



//不同的模式不同的投注金额
function Change_Modes(ModeId) {
    //console.log(ModeId);
    CurrentMode = 0;
    var MName = "";
    if (ModeId < 1 || ModeId > tzmscount) return;
    for (var i = 1; i <= tzmscount; i++)
    {
        var o = document.getElementById("ModeName_" + i);
        if (o == null) continue;
        if (i == ModeId)
        {
            o.style.color = "#FF6600";
            CurrentMode = i;
            MName = o.innerHTML;
        } else
        {
            o.style.color = "";
        }
    }
    if (CurrentMode < 1) return;

    for (var i = 0; i < 28; i++)
    {
        document.getElementById("SMONEY_"+i).value = ModelDatas[CurrentMode - 1][i];
    }
    $("#bttmode").val(ModeNames[CurrentMode - 1]);


    $(".img_bt1").css("background", "url(../../images/img_bt1.png) left no-repeat");

    //$('.img_bt1').each(function(i, v) {
    //    if ($(v).html() == bttype) {
    //        $(v).css("background", "url(../../images/xy28_bg.gif)"); betttype = i;
    //        return false;
    //    }
    //});
    //switch (bttype)
    //{
    //    case "单":
    //        $(".img_bt1").eq(0).css("background", "url(../img/xy28_bg.gif)"); betttype = "1";
    //        break;
    //    case "双":
    //        $(".img_bt1").eq(1).css("background", "url(../img/xy28_bg.gif)"); betttype = "2";
    //        break;
    //    case "大": $(".img_bt1").eq(2).css("background", "url(../img/xy28_bg.gif)"); betttype = "3";
    //        break;
    //    case "小": $(".img_bt1").eq(3).css("background", "url(../img/xy28_bg.gif)"); betttype = "4";
    //        break;
    //}

    document.getElementById("SMONEYSUM").value = ModelDatas[CurrentMode - 1][27];;
    document.getElementById("SMONEYSUM2").value = ModelDatas[CurrentMode - 1][27];
    document.getElementById("m_info").innerHTML = "模式“" + MName + "”的详细情况：";
}

function inputsb(val, num)
{
    var regex = /^[1-9]\d{0,}$/;
    var oldsum, sum, thismoney, sm;
    sum = 0;

    for (loop = 0 ; loop < 28 ; loop++)
    {
        sm = document.getElementById("SMONEY_"+loop).value;
        if (sm == null || sm > 20000000 || sm == "")
        {
            sm = 0;
            document.getElementById("SMONEY_"+loop).value = 0;

        }
        sum = sum + parseInt(sm);
    }
    document.getElementById("SMONEYSUM").value = sum;
    document.getElementById("SMONEYSUM2").value = sum;
}

//加法
function AddMONEY(val)
{
    var obj = document.getElementById('SMONEY_'+val);
    if ((obj.value == "") || (obj.value == null))
    {
        document.getElementById('SMONEY_'+val).value = "0";
    }
    //组合不允许增加数字
    if (obj.value == '0')
        return;
    var ciobj = document.getElementsByName('CI')[val];
    var SMONEYSUM = document.getElementById("SMONEYSUM");
        

    if (obj.value == '0')
    {
        obj.value = 10;
    }
    else
    {
        var result=parseInt(obj.value) + 10;
        if(result.toString().length>9)
        {
            return;
        }
        obj.value =result;
    }
    var sum = parseInt(SMONEYSUM.value) + 10;
    SMONEYSUM.value = sum;
    document.getElementById("SMONEYSUM2").value = sum;

}

//减法
function DecreaseMONEY(val)
{
    var obj = document.getElementById('SMONEY_'+val);
    if ((obj.value == "") || (obj.value == null))
    {
        document.getElementById('SMONEY_'+val).value = "0";
    }

    var ciobj = document.getElementsByName('CI')[val];
    var SMONEYSUM = document.getElementById("SMONEYSUM");
    var sum = parseInt(SMONEYSUM.value) - 10;

    if (obj.value != '0')
    {
        if (obj.value <= 10)
        {
            obj.value = '0';
        }
        else
        {
            obj.value = parseInt(obj.value) - 10;
        }
        SMONEYSUM.value = sum;
        document.getElementById("SMONEYSUM2").value = sum;
    }
}

//投注倍数算法
function chgbbb(bbb)
{
    var abc = 0;
    var smval, cival;
    for (loop = 0 ; loop < 28 ; loop++)
    {

        smval = document.getElementById("SMONEY_"+loop).value;
        if ((smval == "") || (smval == null))
        {
            smval == "0";
        }
        else
        {
            smval = parseInt(parseInt(smval) * bbb);
            if(smval.toString().length>9)
            {
                return;
            }
            document.getElementById("SMONEY_"+loop).value = smval;
            abc = parseInt(abc) + parseInt(smval);
        }

    }
    document.getElementById("SMONEYSUM").value = abc;
    document.getElementById("SMONEYSUM2").value = abc;
}

var stdMode = new Array(
new Array(),
new Array(1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 63, 69, 73, 75, 75, 73, 69, 63, 55, 45, 36, 28, 21, 15, 10, 6, 3, 1),//全包 1
new Array(0, 3, 0, 10, 0, 21, 0, 36, 0, 55, 0, 69, 0, 75, 0, 73, 0, 63, 0, 45, 0, 28, 0, 15, 0, 6, 0, 1),//单 2
new Array(1, 0, 6, 0, 15, 0, 28, 0, 45, 0, 63, 0, 73, 0, 75, 0, 69, 0, 55, 0, 36, 0, 21, 0, 10, 0, 3, 0),//双 3
new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 75, 73, 69, 63, 55, 45, 36, 28, 21, 15, 10, 6, 3, 1),//大 4
new Array(1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 63, 69, 73, 75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),//小 5
new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 63, 69, 73, 75, 75, 73, 69, 63, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),//中 6
new Array(1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 0, 0, 0, 0, 0, 0, 0, 0, 55, 45, 36, 28, 21, 15, 10, 6, 3, 1),//边 7
new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 73, 0, 63, 0, 45, 0, 28, 0, 15, 0, 6, 0, 1),//大单 8
new Array(0, 3, 0, 10, 0, 21, 0, 36, 0, 55, 0, 69, 0, 75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),//小单 9
new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 75, 0, 69, 0, 55, 0, 36, 0, 21, 0, 10, 0, 3, 0),//大双 10
new Array(1, 0, 6, 0, 15, 0, 28, 0, 45, 0, 63, 0, 73, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),//小双 11
new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 55, 45, 36, 28, 21, 15, 10, 6, 3, 1),//大边 12
new Array(1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),//小边 13
new Array(0, 3, 0, 10, 0, 21, 0, 36, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 0, 28, 0, 15, 0, 6, 0, 1),//单边 14
new Array(1, 0, 6, 0, 15, 0, 28, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 55, 0, 36, 0, 21, 0, 10, 0, 3, 0),//双边 15
new Array(1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 63, 0, 0, 0, 0, 0, 0, 0, 0, 0, 36, 0, 0, 0, 0, 0, 0, 0),//0尾 16
new Array(0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 69, 0, 0, 0, 0, 0, 0, 0, 0, 0, 28, 0, 0, 0, 0, 0, 0),//1尾 17
new Array(0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 73, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 0, 0, 0, 0, 0),//2尾 18
new Array(0, 0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0),//3尾 19
new Array(0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0),//4尾 20
new Array(1, 3, 6, 10, 15, 0, 0, 0, 0, 0, 63, 69, 73, 75, 75, 0, 0, 0, 0, 0, 36, 28, 21, 15, 10, 0, 0, 0),//小尾 21
new Array(0, 0, 0, 0, 0, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 73, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0),//5尾 22
new Array(0, 0, 0, 0, 0, 0, 28, 0, 0, 0, 0, 0, 0, 0, 0, 0, 69, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0),//6尾 23
new Array(0, 0, 0, 0, 0, 0, 0, 36, 0, 0, 0, 0, 0, 0, 0, 0, 0, 63, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1),//7尾 24
new Array(0, 0, 0, 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0),//8尾 25
new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0),//9尾 26
new Array(0, 0, 0, 0, 0, 21, 28, 36, 45, 55, 0, 0, 0, 0, 0, 73, 69, 63, 55, 45, 0, 0, 0, 0, 0, 6, 3, 1),//大尾 27
new Array(1, 0, 0, 10, 0, 0, 28, 0, 0, 55, 0, 0, 73, 0, 0, 73, 0, 0, 55, 0, 0, 28, 0, 0, 10, 0, 0, 1),//3余0 28
new Array(0, 3, 0, 0, 15, 0, 0, 36, 0, 0, 63, 0, 0, 75, 0, 0, 69, 0, 0, 45, 0, 0, 21, 0, 0, 6, 0, 0),//3余1 29
new Array(0, 0, 6, 0, 0, 21, 0, 0, 45, 0, 0, 69, 0, 0, 75, 0, 0, 63, 0, 0, 36, 0, 0, 15, 0, 0, 3, 0),//3余2 30
new Array(1, 0, 0, 0, 15, 0, 0, 0, 45, 0, 0, 0, 73, 0, 0, 0, 69, 0, 0, 0, 36, 0, 0, 0, 10, 0, 0, 0),//4余0 31
new Array(0, 3, 0, 0, 0, 21, 0, 0, 0, 55, 0, 0, 0, 75, 0, 0, 0, 63, 0, 0, 0, 28, 0, 0, 0, 6, 0, 0),//4余1 32
new Array(0, 0, 6, 0, 0, 0, 28, 0, 0, 0, 63, 0, 0, 0, 75, 0, 0, 0, 55, 0, 0, 0, 21, 0, 0, 0, 3, 0),//4余2 33
new Array(0, 0, 0, 10, 0, 0, 0, 36, 0, 0, 0, 69, 0, 0, 0, 73, 0, 0, 0, 45, 0, 0, 0, 15, 0, 0, 0, 1),//4余3 34
new Array(1, 0, 0, 0, 0, 21, 0, 0, 0, 0, 63, 0, 0, 0, 0, 73, 0, 0, 0, 0, 36, 0, 0, 0, 0, 6, 0, 0),//5余0 35
new Array(0, 3, 0, 0, 0, 0, 28, 0, 0, 0, 0, 69, 0, 0, 0, 0, 69, 0, 0, 0, 0, 28, 0, 0, 0, 0, 3, 0),//5余1 36
new Array(0, 0, 6, 0, 0, 0, 0, 36, 0, 0, 0, 0, 73, 0, 0, 0, 0, 63, 0, 0, 0, 0, 21, 0, 0, 0, 0, 1),//5余2 37
new Array(0, 0, 0, 10, 0, 0, 0, 0, 45, 0, 0, 0, 0, 75, 0, 0, 0, 0, 55, 0, 0, 0, 0, 15, 0, 0, 0, 0),//5余3 38
new Array(0, 0, 0, 0, 15, 0, 0, 0, 0, 55, 0, 0, 0, 0, 75, 0, 0, 0, 0, 45, 0, 0, 0, 0, 10, 0, 0, 0)//5余4 39
    );


//标准模式
function FirstModes(modes)
{
    $(".img_bt1").css("background", "url(../../images/img_bt1.png) left no-repeat");
    $(".img_bt1").eq(modes-1).css("background", "url(../../images/xy28_bg.gif)");
    var SMONEYS;
    var sum = 0;
    document.getElementById("SMONEYSUM").value = 0;
    document.getElementById("SMONEYSUM2").value = 0;

    if (modes < 1 || modes > stdMode.length) modes = 1;

    //if ((modes - 1) == 1 || (modes - 1) == 2 || (modes - 1) == 3 || (modes - 1) == 4)
    //{
    //    betttype = (modes - 1).toString();
    //}
    //else if ((modes - 1) == 8 || (modes - 1) == 9 || (modes - 1) == 10 || (modes - 1) == 11)
    //{
    //    betttype = (modes - 1).toString();
    //}

    for (loop = 0 ; loop < 28 ; loop++)
    {
        var n = stdMode[modes][loop];
        document.getElementById("SMONEY_"+loop).value = n;//投注金额
        sum = sum + n;  //总投注金额

    }
    document.getElementById("SMONEYSUM").value = sum;
    document.getElementById("SMONEYSUM2").value = sum;
}

function AddMode()
{
    //     if(ModeCount >= 10) {
    //        var message = "您当前已经有10个投注模式，无法再添加！";
    //        var btn = "<div><a onclick='return CleanMessage()'  style='width:72px;height:22px; background:url(img/popup_btn.png) no-repeat;display:block;cursor:pointer;'></a></div>";
    //        showMessage("添加新模式", message, btn);
    //        return;
    //     }
    $(".img_bt1").css("background", "url(../../images/img_bt1.png) left no-repeat");
    betttype = "0";
    for (var i = 1; i <= tzmscount; i++)
    {
        var o = document.getElementById("ModeName_" + i);
        if (o == null) continue;
        o.style.color = ""
    }


    CurrentMode = 0;
    for (var i = 1; i <= tzmscount; i++)
    {
        if (ModeNames[i - 1] == "")
        {
            CurrentMode = i;
            break;
        }
    }

    for (var i = 0; i < 28; i++)
    {
        document.getElementById("SMONEY_"+i).value = "0";
    }
    document.getElementById("SMONEYSUM").value = "0";
    document.getElementById("SMONEYSUM2").value = "0";
    document.getElementById("m_info").innerHTML = "新投注模式的详细情况：";
}

function trim(data)
{
    if (data == null) return "";
    return data.replace(/(^\s*)|(\s*$)/g, "");
}

function OnSave()
{
    var sumgoldeggs = document.getElementById("SMONEYSUM").value;
    if (sumgoldeggs <= 0 || sumgoldeggs > 20000000)
    {
        m = "<div class='content1' id='Notice_content'>保存失败，投注总额必须为1-2000万乐豆</div>";
        layer.alert(m,{icon:2,title:"保存模式"});
        return;
    }

    var message;
    if (CurrentMode != 0)
    {
        message = "<div class='content1' id='Notice_content'>保存模式名称&nbsp;&nbsp;<input type='text' class='input3 layui-layer-input' id='SaveModename' maxlength='8' value='" + ModeNames[CurrentMode - 1] + "'></div>";
    } else
    {
        message = "<div class='content1' id='Notice_content'>保存模式名称&nbsp;&nbsp;<input type='text' class='input3 layui-layer-input' id='SaveModename' maxlength='8'></div>";
    }

        
    layer.prompt({title: '保存模式', formType: 2,content:message}, 
            function(val, index){
                layer.close(index);
                DoSave(val);
            });
}

function DoSave(NewName)
{
    if (ModeCount >= tzmscount && CurrentMode == 0)
    {
        var message = "<div class='content1' id='Notice_content'>您当前已经有" + tzmscount + "个投注模式，无法再添加！</div>";
        layer.alert(message,{icon:2,title:"保存模式"});
        return;
    }
    var m = "";
    var sumgoldeggs = document.getElementById("SMONEYSUM").value;
    if (sumgoldeggs <= 0 || sumgoldeggs > 20000000)
    {
        m = "<div class='content1' id='Notice_content'>保存失败，投注总额必须为1-2000万乐豆</div>";
        layer.alert(m,{icon:2,title:"保存模式"});
        return;
    }


    for (var i = 1; i <= tzmscount; i++)
    {
        if (i == CurrentMode) continue;
        if (NewName == ModeNames[i - 1])
        {
            var message = "<div class='content1' id='Notice_content'>投注模式名:“" + NewName + "”已经存在，无法保存</div>";
            layer.alert(message,{icon:2,title:"保存模式"});
            return;
        }
    }

    if (NewName.indexOf("<") > -1 || NewName.indexOf(">") > -1 || NewName.indexOf("'") > -1 || NewName.indexOf('"') > -1)
    {
        var message = "<div class='content1' id='Notice_content'>包含非法字符，无法保存</div>";
        layer.alert(message,{icon:2,title:"保存模式"});
        return;
    }
    var arrbettnew = "";
    var arrbettnum = "";
        
    $("input[name='SMONEY']").each(function ()
    {
        var ipval = $(this).val();
        ipval = $.trim(ipval).replace(/,/gi, "");

        if (ipval != "0" && ipval.trim() != "")
        {
            var src = $(this).parent().parent().find("td").eq(0).find("span").text();
            var num =src;// src.split('_')[1].split('.')[0];
            arrbettnew += (num.length == 1 ? ("0" + num) : num) + ":" + ipval + ";";
            arrbettnum += (num.length == 1 ? ("0" + num) : num) + ";";
        }
    });
        
        
    $.post("/nwlottery/addbettmode",
        {
           "lotterytype":lotterytype, "list": arrbettnew.substr(0, arrbettnew.length - 1), "listnum": arrbettnum.substr(0, arrbettnum.length - 1),
            "mdname": NewName, "sum": document.getElementById("SMONEYSUM").value
        },
        function (data)
        {
            if(data=="1")
            {
                AddMode();
                Init("save");
                    
            }else 
            {
                var message = "<div class='content1' id='Notice_content'>保存投注模式[" + NewName + "]失败！</div>";
                layer.alert(message,{icon:2,title:"保存模式"});
            }
        });
}

function OnDelete()
{
    var message;
    var btn;

    if (CurrentMode == 0 || ModeNames[CurrentMode - 1] == "")
    {
        message = "<div class='content1' id='Notice_content'>请选择要删除的模式！</div>";
        layer.alert(message, { icon: 2, title: "保存模式" });
    } else
    {
        message = "您确定要删除模式“" + ModeNames[CurrentMode - 1] + "”？";
        var message = "<div class='content1' id='Notice_content'>" + message + "</div>";
        layer.confirm(message, function ()
        {
            DoDelete();
            layer.closeAll('dialog');
        }, function ()
        { }, {title:"提示"});
    }
}
function DoDelete()
{
    $.post("/nwlottery/delbettmode",
        { "mdname": ModeNames[CurrentMode - 1], "lotterytype": lotterytype },
        function (data)
        {
            if (data == "1")
            {
                AddMode();
                Init("save");
            }else
            {
                var message = "<div class='content1' id='Notice_content'>删除投注模式[" + ModeNames[CurrentMode - 1] + "]失败！</div>";
                layer.alert(message, { icon: 2, title: "删除模式" });
            }
        });
    
}