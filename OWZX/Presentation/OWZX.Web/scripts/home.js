$(function ()
{
    $("#reserve").on("click", function ()
    {
        addreserveoper();
    });
    $("#order .Message li>input").on("focus", function ()
    {
        $(this).css("border-color", "#96D5B9").next().next().html("");
    });

    $("#order .Message li>input").on("blur", function ()
    {
        $(this).css("border-color", "#EBEBEB");
    });
});
function addreserveoper()
{
    var name = $("#name").val();
    var password = $("#password").val();
    var code = $("#code").val();
   
    var result = verifyReserve(name, password, code);
    if (!result)
        return false;
    var parms = new Object();
    parms[shadowName] = name; parms["password"] = password; parms["verifyCode"] = code;
    hmlogin(parms);
}
//验证
function verifyReserve(name, password, code)
{
    if (name.length == 0)
    {
        layer.msg("登录邮箱不能为空");
        //$("#name").css("border-color", "red").next().next()
        //.html("<em></em>称呼不能为空");
        return false;
    }

    if (password.length == 0)
    {
        layer.msg("密码不能为空");
        //$("#time").css("border-color", "red").next().next()
        //.html("<em></em>密码不能为空");
        return false;
    }
    
    if (code.length == 0)
    {
        layer.msg("验证码不能为空");
        //$("#address").css("border-color", "red").next().next()
        //.html("<em></em>验证码不能为空");
        return false;
    }
    
    return true;
}
//登录
function hmlogin(parms)
{
    $.post("/account/login", parms, function (data)
    { 
        var result = eval("(" + data + ")");
        if (result.state == "success")
        {
            window.location.href = "/";
        }
        else {
            showVerifyError(result.content); 
        }
    });
}
//展示验证错误
function showVerifyError(verifyErrorList) {
    if (verifyErrorList != undefined && verifyErrorList != null && verifyErrorList.length > 0) {
        var msg = "";
        for (var i = 0; i < verifyErrorList.length; i++) {
            msg += verifyErrorList[i].msg + "\n";
        }
        layer.msg(msg, { icon: 2 })
    }
}
