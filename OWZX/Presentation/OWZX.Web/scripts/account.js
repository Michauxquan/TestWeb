var returnUrl = "/"; //返回地址
var shadowName = ""; //影子账号名

$(function ()
{
    //设计案例切换
    $('.title-list li').mouseover(function ()
    {
        var liindex = $('.title-list li').index(this);
        $(this).addClass('on').siblings().removeClass('on');
        $('.product-wrap div.product').eq(liindex).fadeIn(150).siblings('div.product').hide();
        var liWidth = $('.title-list li').width();
        $('.lanrenzhijia .title-list p').stop(false, true).animate({ 'left': liindex * liWidth + 'px' }, 300);
    });

    //设计案例hover效果
    $('.product-wrap .product li').hover(function ()
    {
        $(this).css("border-color", "#ff6600");
        $(this).find('p > a').css('color', '#ff6600');
    }, function ()
    {
        $(this).css("border-color", "#fafafa");
        $(this).find('p > a').css('color', '#666666');
    });

    $("#chkservice").on("change", function ()
    {
        var chk = $("#chkservice").prop("checked");
        if (chk)
            $(".btnreg").removeAttr("disabled");
        else
            $(".btnreg").prop("disabled", "disabled");

    });

    $(".b1,.b2").on("focus", function (data)
    {
        $(this).css("border-color", "#96D5B9");
        $(this).next().next().html("");
        if($(this).attr("name")=="verifyCode")
            $(this).next().next().next().html("");
    });
    $(".b1,.b2").on("blur", function (data)
    {
        $(this).css("border-color", "#cdcdcd");
        if(shadowName==$(this).attr("name"))
             verifyBlur(shadowName, $(this).val());
    });

    $(".send_auto").on("click", function ()
    {
        var result = verifyBlur(shadowName, $("#" + shadowName).val());
        if (result)
        {
            $(".send_auto").attr("disabled", "disabled");
            settime();
            tt = setInterval(settime, 1000);
        }
        else
            $(".send_auto").removeAttr("disabled");
    });
})

var countdown = 60;
var tt;
function settime()
{
    if (countdown == 0)
    {
        $(".send_auto").removeAttr("disabled");
        $(".send_auto").val("发送验证码到手机");
        countdown = 60;
        clearTimeout(tt);
    } else
    {
        $(".send_auto").attr("disabled", "disabled");
        $(".send_auto").val("重新发送(" + countdown + "s)");
        countdown--;
        
    }
    
}


function css(obj, attr, value)
{
    if (arguments.length == 2)
    {
        if (attr != 'opacity')
        {
            return parseInt(obj.currentStyle ? obj.currentStyle[attr] : document.defaultView.getComputedStyle(obj, false)[attr]);
        } else
        {
            return Math.round(100 * parseFloat(obj.currentStyle ? obj.currentStyle[attr] : document.defaultView.getComputedStyle(obj, false)[attr]));
        }
    } else if (arguments.length == 3) switch (attr)
    {
        case 'width':
        case 'height':
        case 'paddingLeft':
        case 'paddingTop':
        case 'paddingRight':
        case 'paddingBottom':
            value = Math.max(value, 0);
        case 'left':
        case 'top':
        case 'right':
        case 'bottom':
        case 'marginLeft':
        case 'marginTop':
        case 'marginRight':
        case 'marginBottom':
            obj.style[attr] = value + 'px';
            break;
        case 'opacity':
            obj.style.filter = "alpha(opacity:" + value + ")";
            obj.style.opacity = value / 100;
            break;
        default:
            obj.style[attr] = value;
    }
    return function (attr_in, value_in)
    {
        css(obj, attr_in, value_in)
    };
}


//obj是指要运动的物体
//itype是要采取哪种类型的运动move_type.buffer为缓冲运动，move_type.flex弹性运动。
//oTarget是目标要运行到多少来.默认是px所以不需要带单位。
//fnCallBack运动结束要做些什么。
//fnduring在运动中要进行什么
function startMove(obj, oTarget, iType, fnCallBack, fnDuring)
{
    var bStop = true;
    var attr = '';
    var speed = 0;
    var cur = 0;
    if (obj.timer)
    {
        clearInterval(obj.timer);
    }
    obj.timer = setInterval(function ()
    {
        startMove(obj, oTarget, iType, fnCallBack, fnDuring);
    }, 30);
    for (attr in oTarget)
    {
        if (iType == 'buffer')
        {
            cur = css(obj, attr);
            if (oTarget[attr] != cur)
            {
                bStop = false;
                speed = (oTarget[attr] - cur) / 5;
                speed = speed > 0 ? Math.ceil(speed) : Math.floor(speed);
                css(obj, attr, cur + speed);
            }
        } else if (iType = 'flex')
        {
            if (!obj.oSpeed) obj.oSpeed = {};
            if (!obj.oSpeed[attr]) obj.oSpeed[attr] = 0;
            cur = css(obj, attr);
            if (Math.abs(oTarget[attr] - cur) >= 1 || Math.abs(obj.oSpeed[attr]) >= 1)
            {
                bStop = false;
                obj.oSpeed[attr] += (oTarget[attr] - cur) / 5;
                obj.oSpeed[attr] *= 0.7;
                css(obj, attr, cur + obj.oSpeed[attr]);
            }
        }
    }
    if (fnDuring) fnDuring.call(obj);
    if (bStop)
    {
        clearInterval(obj.timer);
        obj.timer = null;
        if (fnCallBack) fnCallBack.call(obj);
    }
}
//展示验证错误
function showVerifyError(verifyErrorList)
{
    if (verifyErrorList != undefined && verifyErrorList != null && verifyErrorList.length > 0)
    {
        var msg = "";
        for (var i = 0; i < verifyErrorList.length; i++)
        {
            msg += verifyErrorList[i].msg + "\n";
        }
        alert(msg)
    }
}

//用户登录
function login()
{
    var loginForm = document.forms["loginForm"];

    var accountName = loginForm.elements[shadowName].value;
    var password = loginForm.elements["password"].value;
    //var verifyCode = loginForm.elements["verifyCode"] ? loginForm.elements["verifyCode"].value : undefined;
    var isRemember = loginForm.elements["isRemember"] ? loginForm.elements["isRemember"].checked ? 1 : 0 : 0;

    if (!verifyLogin(accountName, password))
    {
        return;
    }

    var parms = new Object();
    parms[shadowName] = accountName;
    parms["password"] = password;
    //parms["verifyCode"] = verifyCode;
    parms["isRemember"] = isRemember;
    $.post("/account/login", parms, loginResponse)
}

//验证登录
function verifyLogin(accountName, password)
{
    if (accountName.trim().length == 0)
    {
        $("#" + shadowName).css("border-color", "red");
        $(".phone").html("<em></em>请输入帐户名");
        return false;
    }
    if (password.trim().length == 0)
    {
        $("#password").css("border-color", "red");
        $(".password").html("<em></em>请输入密码");
        return false;
    }
    //if (verifyCode != undefined && verifyCode.length == 0) {
    //    alert("请输入验证码");
    //    return false;
    //}
    return true;
}

//处理登录的反馈信息
function loginResponse(data)
{
    var result = eval("(" + data + ")");
    if (result.state == "success")
    {
        window.location.href = returnUrl;
    }
    else
    {
        showVerifyError(result.content);
    }
}

//用户注册
function register()
{
    var registerForm = document.forms["registerForm"];

    var accountName = registerForm.elements[shadowName].value;
    var loginname = registerForm.elements["loginname"].value;
    var password = registerForm.elements["password"].value;
    var confirmPwd = registerForm.elements["confirmPwd"].value;
    var verifyCode = registerForm.elements["verifyCode"] ? registerForm.elements["verifyCode"].value : undefined;
    var chkservice = registerForm.elements["chkservice"].value;
    if (!verifyRegister(accountName, loginname, password, confirmPwd, verifyCode))
    {
        return;
    }

    var parms = new Object();
    parms["loginname"] = loginname;
    parms[shadowName] = accountName;
    parms["password"] = password;
    parms["confirmPwd"] = confirmPwd;
    parms["verifyCode"] = verifyCode;
    $.post("/account/register", parms, registerResponse)
}

//验证注册
function verifyRegister(accountName, loginname, password, confirmPwd, verifyCode)
{
    if (accountName.length == 0)
    {
        $("#" + shadowName).css("border-color", "red");
        $(".a1").html("<em></em>手机号码不能为空");
        return false;
    }
    else if (!isMobile(accountName))
    {
        $("#" + shadowName).css("border-color", "red");
        $(".a1").html("<em></em>手机号码格式错误");
        return false;
    }
    if (loginname.length == 0)
    {
        $("#loginname").css("border-color", "red");
        $(".a2").html("<em></em>用户名不能为空");
        return false;
    }
    else if (!isRegisterUserName(loginname))
    {
        $("#loginname").css("border-color", "red");
        $(".a2").html("<em></em>用户名由英文、数字及'_'组成，5-20位字符");
        return false;
    }

    if (password.length == 0)
    {
        $("#password").css("border-color", "red");
        $(".a3").html("<em></em>密码不能为空");
        return false;
    }
    else if (!isPwd(password))
    {
        $("#password").css("border-color", "red");
        $(".a3").html("<em></em>密码长度需在6到16位间，只含数字 字母 _");
        return false;
    }

    if (password != confirmPwd)
    {
        $("#confirmPwd").css("border-color", "red");
        $(".a4").html("<em></em>两次输入的密码不一样");
        return false;
    }
    if (verifyCode != undefined && verifyCode.length == 0)
    {
        $("#verifyCode").css("border-color", "red");
        $(".a5").html("<em></em>验证码不能为空");
        return false;
    }
    return true;
}

function verifyBlur(type, value)
{
    switch (type)
    {
        case shadowName:
            if (value.length == 0)
            {
                $("#" + shadowName).css("border-color", "red");
                $(".phone").html("<em></em>请输入帐户名");
                $(".a1").html("<em></em>手机号码不能为空");
                return false;
            }
            else if (!isMobile(value))
            {
                $("#" + shadowName).css("border-color", "red");
                $(".a1").html("<em></em>手机号码格式错误");
                return false;
            }
            break;
        case "loginname":
            if (value.length == 0)
            {
                $("#loginname").css("border-color", "red");
                $(".a2").html("<em></em>用户名不能为空");
                return false;
            }
            else if (!isRegisterUserName(value))
            {
                $("#loginname").css("border-color", "red");
                $(".a2").html("<em></em>用户名由英文、数字及'_'组成，以英文开到头,5-20位字符");
                return false;
            }
            break;
        case "password":
            if (value.length == 0)
            {
                $("#password").css("border-color", "red");
                $(".a3").html("<em></em>密码不能为空");
                return false;
            }
            else if (!isPwd(value))
            {
                $("#password").css("border-color", "red");
                $(".a3").html("<em></em>密码长度需在6到16位间，只含数字 字母 _");
                return false;
            }
            break;
        case "confirmPwd":
            var pwd = $("#password").val().trim();
            if (pwd.length == 0)
            {
                $("#password").css("border-color", "red");
                $(".a3").html("<em></em>密码不能为空");
                return false;
            }
            else if (!isPwd(pwd))
            {
                $("#password").css("border-color", "red");
                $(".a3").html("<em></em>密码长度需在6到16位间，只含数字 字母 _");
                return false;
            }
            else if (pwd != confirmPwd)
            {
                $("#confirmPwd").css("border-color", "red");
                $(".a4").html("<em></em>两次输入的密码不一样");
                return false;
            }
            break;
        case "verifyCode":
            if (verifyCode != undefined && verifyCode.length == 0)
            {
                $("#verifyCode").css("border-color", "red");
                $(".a5").html("<em></em>验证码不能为空");
                return false;
            }
            break;
    }
    return true;
}

//处理注册的反馈信息
function registerResponse(data)
{
    var result = eval("(" + data + ")");
    if (result.state == "success")
    {
        window.location.href = returnUrl;
    }
    else if (result.state == "exception")
    {
        alert(result.content);
    }
    else if (result.state == "error")
    {
        showVerifyError(result.content);
    }
}

//找回密码
function findPwd()
{
    var findPwdForm = document.forms["findPwdForm"];

    var accountName = findPwdForm.elements[shadowName].value;
    var verifyCode = findPwdForm.elements["verifyCode"].value;

    if (!verifyFindPwd(accountName, verifyCode))
    {
        return;
    }

    var parms = new Object();
    parms[shadowName] = accountName;
    parms["verifyCode"] = verifyCode;
    $.post("/account/findpwd", parms, findPwdResponse)
}

//验证找回密码
function verifyFindPwd(accountName, verifyCode)
{
    if (accountName.length == 0)
    {
        alert("请输入帐号名");
        return false;
    }
    if (verifyCode != undefined && verifyCode.length == 0)
    {
        alert("请输入验证码");
        return false;
    }
    return true;
}

//处理找回密码的反馈信息
function findPwdResponse(data)
{
    var result = eval("(" + data + ")");
    if (result.state == "success")
    {
        window.location.href = result.content;
    }
    else if (result.state == "nocanfind")
    {
        alert(result.content);
    }
    else if (result.state == "error")
    {
        showVerifyError(result.content);
    }
}

//发送找回密码短信
function sendFindPwdMobile(uid)
{
    $.get("/account/sendfindpwdmobile?uid=" + uid, function (data)
    {
        var result = eval("(" + data + ")");
        alert(result.content)
    })
}

//验证找回密码短信
function verifyFindPwdMobile(uid, mobileCode)
{
    if (mobileCode.length == 0)
    {
        alert("请输入短信验证码");
        return;
    }
    $.post("/account/verifyfindpwdmobile?uid=" + uid, { 'mobileCode': mobileCode }, function (data)
    {
        var result = eval("(" + data + ")");
        if (result.state == "success")
        {
            window.location.href = result.content;
        }
        else
        {
            alert(result.content)
        }
    })
}

//发送找回密码邮件
function sendFindPwdEmail(uid)
{
    $.get("/account/sendfindpwdemail?uid=" + uid, function (data)
    {
        var result = eval("(" + data + ")");
        alert(result.content)
    })
}

//重置用户密码
function resetPwd(v)
{
    var resetPwdForm = document.forms["resetPwdForm"];

    var password = resetPwdForm.elements["password"].value;
    var confirmPwd = resetPwdForm.elements["confirmPwd"].value;

    if (!verifyResetPwd(password, confirmPwd))
    {
        return;
    }

    var parms = new Object();
    parms["password"] = password;
    parms["confirmPwd"] = confirmPwd;
    $.post("/account/resetpwd?v=" + v, parms, resetPwdResponse)
}

//验证重置密码
function verifyResetPwd(password, confirmPwd)
{
    if (password.length == 0)
    {
        alert("请输入密码");
        return false;
    }
    if (password != confirmPwd)
    {
        alert("两次输入的密码不一样");
        return false;
    }
    return true;
}

//处理验证重置密码的反馈信息
function resetPwdResponse(data)
{
    var result = eval("(" + data + ")");
    if (result.state == "success")
    {
        alert("密码修改成功,请重新登录");
        window.location.href = result.content;
    }
    else if (result.state == "error")
    {
        showVerifyError(result.content);
    }
}