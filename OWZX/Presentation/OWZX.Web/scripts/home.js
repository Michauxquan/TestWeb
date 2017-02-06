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

    $(".ind_reserve").on("click", function ()
    {
        var index = $(this).index();
        switch (index)
        {
            case 0:
                $(this).css("background-color", "#FFFFFF").next().css("background-color", "#f8f8f8");
                $(".Message").css("display", "block");
                $(".contact").css("display", "none");
                break;
            case 1:
                $(this).css("background-color", "#FFFFFF").prev().css("background-color", "#f8f8f8");
                $(".contact").css("display", "block");
                $(".Message").css("display", "none");
                break;
        }
    });

});
function addreserveoper()
{
    var name = $("#name").val();
    var time = $("#time").val();
    var address = $("#address").val();
    var phone = $("#phone").val();
    var result = verifyReserve(name, time, address, phone);
    if (!result)
        return false;
    var parms = new Object();
    parms["OwnerName"] = name; parms["ReserveTime"] = time; parms["Address"] = address; parms["OwnerPhone"] = phone;
    addReserve(parms);
}
//验证
function verifyReserve(name, time, address, phone)
{
    if (name.length == 0)
    {
        $("#name").css("border-color", "red").next().next()
        .html("<em></em>称呼不能为空");
        return false;
    }

    if (time.length == 0)
    {
        $("#time").css("border-color", "red").next().next()
        .html("<em></em>预约时间不能为空");
        return false;
    }
    
    if (address.length == 0)
    {
        $("#address").css("border-color", "red").next().next()
        .html("<em></em>地址不能为空");
        return false;
    }

    if (phone.length == 0)
    {
        $("#phone").css("border-color", "red").next().next()
        .html("<em></em>手机号码不能为空");
        return false;
    }
    else if (!isMobile(phone))
    {
        $("#phone").css("border-color", "red").next().next()
        .html("<em></em>手机号码格式错误");
        return false;
    }
    return true;
}
//添加预约
function addReserve(parms)
{
    $.post("/reserve/addreserve", parms, function (data)
    {
        if (data == "0")
            layer.alert("预约申请失败，请稍后再试", { icon: 2 });
        else if (data == "1")
        {
            layer.alert("预约申请成功，稍后客服会与您联系", { icon: 1 });
            $("#order .Message li>input").val("");
        }
    });
}
