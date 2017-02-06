//用户登录
function login()
{
    var loginForm = document.forms["loginForm"];

    var accountName = loginForm.elements[shadowName].value;
    var password = loginForm.elements["password"].value;
    var verifyCode = loginForm.elements["verifyCode"] ? loginForm.elements["verifyCode"].value : undefined;
    var isRemember = loginForm.elements["isRemember"] ? loginForm.elements["isRemember"].checked ? 1 : 0 : 0;

    if (!verifyLogin(accountName, password, verifyCode))
    {
        return;
    }

    var parms = new Object();
    parms[shadowName] = accountName;
    parms["password"] = password;
    parms["verifyCode"] = verifyCode;
    parms["isRemember"] = isRemember;
    $.post("/admin/account/login", parms, loginResponse)
}

//验证登录
function verifyLogin(accountName, password, verifyCode)
{
    if (accountName.length == 0)
    {
        alert("请输入帐号名");
        return false;
    }
    if (password.length == 0)
    {
        alert("请输入密码");
        return false;
    }
    if (verifyCode != undefined && verifyCode.length == 0)
    {
        alert("请输入验证码");
        return false;
    }
    return true;
}

//处理登录的反馈信息
function loginResponse(data)
{
    var result = eval("(" + data + ")");
    if (result.state == "success")
    {
        window.location.href = "/admin/home/index";
    }
    else if (result.state == "404")
    {
        alert(result.content);
    }
    else
    {
        showVerifyError(result.content);
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