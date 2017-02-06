var uid = -1; //用户id
var isGuestSC = 0; //是否允许游客使用购物车(0代表不可以，1代表可以)
var scSubmitType = 0; //购物车的提交方式(0代表跳转到提示页面，1代表跳转到列表页面，2代表ajax提交)

$.ajaxSetup({
    cache: false //关闭AJAX缓存
});

$(function ()
{
    showScroll();
    function showScroll()
    {
        $(window).scroll(function ()
        {
            var scrollValue = $(window).scrollTop();
            scrollValue > 100 ? $('div[class=scroll]').fadeIn() : $('div[class=scroll]').fadeOut();
        });
        $('.top').click(function ()
        {
            $("html,body").animate({ scrollTop: 0 }, 200);
        });
    }
})
//判断是否是数字
function isNumber(val)
{
    var regex = /^[\d|\.]+$/;
    return regex.test(val);
}

//判断是否为整数
function isInt(val)
{
    var regex = /^\d+$/;
    return regex.test(val);
}

//判断是否为邮箱
function isEmail(val)
{
    var regex = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
    return regex.test(val);
}

//判断是否为手机号
function isMobile(val)
{
    var regex = /^[1][0-9][0-9]{9}$/;
    return regex.test(val);
}
//判断是否为有效的账户 用户名由英文、数字及"_"组成，5-20位字符 
function isRegisterUserName(s)
{
    var patrn = /^[a-zA-Z]{1}([a-zA-Z0-9]|[_]){4,20}$/;
    if (!patrn.exec(s)) return false
    return true
}
//判断是否为有效的密码 密码长度需在6到16位间，只含数字 字母 _
function isPwd(s)
{
    var patrn = /^(\w){6,16}$/;
    if (!patrn.exec(s)) return false
    return true
}

//搜索
function search()
{
    var word = document.getElementById('keyword').value;
    var tt = $(".header_select_sort span em").text();
    if (word == undefined || word == null || word.length < 1)
    {
        return false;
        //layer.msg("请输入关键词");
    }
    else
    {
        if (tt == "建材市场")
        {
            window.location.href = "/catalog/search?word=" + encodeURIComponent(word);
        }
        else if (key == "工程案例")
        {
            window.location.href = "/project/search?word=" + encodeURIComponent(word);
        }

    }
}

//获得购物车快照
function getCartSnap()
{
    if (isGuestSC == 0 && uid < 1)
    {
        return;
    }
    $("#cartSnap").show();
    $.get("/cart/snap", function (data)
    {
        getCartSnapResponse(data);
    })
}

//处理获得购物车快照的反馈信息
function getCartSnapResponse(data)
{
    try
    {
        var result = eval("(" + data + ")");
        alert(result.content);
    }
    catch (ex)
    {
        $("#cartSnap").html(data);
        $("#cartSnapProudctCount").html($("#csProudctCount").html());
    }
}

//关闭购物车快照
function closeCartSnap()
{
    $("#cartSnap").hide();
}

//处理添加商品到收藏夹的反馈信息
function addToFavoriteResponse(data)
{
    var result = eval("(" + data + ")");
    alert(result.content);
}

//添加商品到购物车
function addProductToCart(pid, buyCount)
{
    if (pid < 1)
    {
        layer.msg("请选择商品");
    }
    else if (isGuestSC == 0 && uid < 1)
    {
        layer.msg("请先登录");
    }
    else if (buyCount < 1)
    {
        layer.msg("请填写购买数量");
    }
    else if (scSubmitType != 2)
    {
        window.location.href = "/cart/addproduct?pid=" + pid + "&buyCount=" + buyCount;
    }
    else
    {
        $.get("/cart/addproduct?pid=" + pid + "&buyCount=" + buyCount, addProductToCartResponse)
    }
}

//处理添加商品到购物车的反馈信息
function addProductToCartResponse(data)
{
    var result = eval("(" + data + ")");
    layer.alert(result.content);
}

//购买商品
function buyProduct(pid, buyCount)
{
    if (pid < 1)
    {
        layer.msg("请选择商品");
    }
    else if (isGuestSC == 0 && uid < 1)
    {
        layer.msg("请先登录");
    }
    else if (buyCount < 1)
    {
        layer.msg("请填写购买数量");
    }
    else
    {
        $.get("/cart/buyproduct?pid=" + pid + "&buyCount=" + buyCount, buyProductResponse)
    }
}

//处理购买商品的反馈信息
function buyProductResponse(data)
{
    var result = eval("(" + data + ")");
    if (result.state == "success")
    {
        window.location.href = result.content;
    }
    else
    {
        layer.alert(result.content);
    }
}


//获得选中的购物车项键列表
function getSelectedCartItemKeyList()
{
    var valueList = new Array();
    $("#cartBody input[type=checkbox][name=cartItemCheckbox]:checked").each(function ()
    {
        valueList.push($(this).val());
    })

    if (valueList.length < 1)
    {
        //当取消全部商品时,添加一个字符防止商品全部选中
        return "_";
    }
    else
    {
        return valueList.join(',');
    }
}

//设置选择全部购物车项复选框
function setSelectAllCartItemCheckbox()
{
    var flag = true;
    $("#cartBody input[type=checkbox][name=cartItemCheckbox]:not(:checked)").each(function ()
    {
        flag = false;
        return false;
    })

    if (flag)
    {
        $("#selectAllBut_top").prop("checked", true);
        $("#selectAllBut_bottom").prop("checked", true);
    }
    else
    {
        $("#selectAllBut_top").prop("checked", false);
        $("#selectAllBut_bottom").prop("checked", false);
    }
}

//删除购物车中商品
function delCartProduct(pid, pos)
{
    if (isGuestSC == 0 && uid < 1)
    {
        layer.alert("请先登录");
        return;
    }
    if (pos == 0)
    {
        $.get("/cart/delproduct?pid=" + pid + "&pos=" + pos + "&selectedCartItemKeyList=" + getSelectedCartItemKeyList(), function (data)
        {
            try
            {
                layer.alert(val("(" + data + ")").content);
            }
            catch (ex)
            {
                $("#cartBody").html(data);
                setSelectAllCartItemCheckbox();
            }
        })
    }
    else
    {
        $.get("/cart/delproduct?pid=" + pid + "&pos=" + pos, function (data)
        {
            try
            {
                layer.alert(val("(" + data + ")").content);
            }
            catch (ex)
            {
                $("#cartSnap").html(data);
                $("#cartSnapProudctCount").html($("#csProudctCount").html());
            }
        })
    }
}

//清空购物车
function clearCart(pos)
{
    if (isGuestSC == 0 && uid < 1)
    {
        layer.alert("请先登录");
        return;
    }
    $.get("/cart/clear?pos=" + pos, function (data)
    {
        try
        {
            alert(eval("(" + data + ")").content);
        }
        catch (ex)
        {
            if (pos == 0)
            {
                $("#cartBody").html(data);
            }
            else
            {
                $("#cartSnap").html(data);
                $("#cartSnapProudctCount").html("0");
            }
        }
    })
}

//改变商品数量
function changePruductCount(pid, buyCount)
{
    if (!isInt(buyCount))
    {
        layer.msg('请输入数字', { icon: 2 });
    }
    else if (isGuestSC == 0 && uid < 1)
    {
        layer.msg("请先登录", { icon: 2 });
    }
    else
    {
        var key = "0_" + pid;
        $("#cartBody input[type=checkbox][value=" + key + "]").each(function ()
        {
            $(this).prop("checked", true);
            return false;
        })
        $.get("/cart/changeproductcount?pid=" + pid + "&buyCount=" + buyCount + "&selectedCartItemKeyList=" + getSelectedCartItemKeyList(), function (data)
        {
            try
            {
                layer.msg(eval("(" + data + ")").content, { icon: 2 });
            }
            catch (ex)
            {
                $("#cartBody").html(data);
                setSelectAllCartItemCheckbox();
            }
        })
    }
}

//取消或选中购物车项
function cancelOrSelectCartItem()
{
    if (isGuestSC == 0 && uid < 1)
    {
        layer.msg("请先登录");
        return;
    }
    $.get("/cart/cancelorselectcartitem?selectedCartItemKeyList=" + getSelectedCartItemKeyList(), function (data)
    {
        try
        {
            layer.alert(eval("(" + data + ")").content);
        }
        catch (ex)
        {
            $("#cartBody").html(data);
            setSelectAllCartItemCheckbox();
        }
    })
}

//取消或选中全部购物车项
function cancelOrSelectAllCartItem(obj)
{
    if (isGuestSC == 0 && uid < 1)
    {
        layer.msg("请先登录");
        return;
    }
    if (obj.checked)
    {
        $.get("/cart/selectallcartitem", function (data)
        {
            try
            {
                layer.alert(eval("(" + data + ")").content);
            }
            catch (ex)
            {
                $("#cartBody").html(data);
            }
        })
    }
    else
    {
        $("#cartBody input[type=checkbox]").each(function ()
        {
            $(this).prop("checked", false);
        })
        $("#totalCount").html("0");
        $("#productAmount").html("0.00");
        $("#fullCut").html("0");
        $("#orderAmount").html("0.00");
    }
}

//前往确认订单
function goConfirmOrder()
{
    if (isGuestSC == 0 && uid < 1)
    {
        layer.msg("请先登录");
        return;
    }
    var valueList = new Array();
    $("#cartBody input[type=checkbox][name=cartItemCheckbox]:checked").each(function ()
    {
        valueList.push($(this).val());
    })

    if (valueList.length < 1)
    {
        layer.msg("请先选择购物车商品");
    }
    else
    {
        $("#selectedCartItemKeyList").val(valueList.join(','));
        document.forms[0].submit();
    }
}

