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



