function init()
{
    // 初始化插件
    $("#updiv").html("").zyUpload({
        width: "750px",                 // 宽度
        height: "200px",                 // 宽度
        itemWidth: "100px",                 // 文件项的宽度
        itemHeight: "75px",                 // 文件项的高度
        url: "/admin/tool/upload?operation=uploadprjimg",  // 上传文件的路径
        multiple: true,                    // 是否可以多个文件上传
        dragDrop: false,                    // 是否可以拖动上传文件
        del: true,                    // 是否可以删除文件
        finishDel: false,  				  // 是否在上传文件完成后删除预览
    });
}

function init(width, height)
{
    // 初始化插件
    $("#updiv").html("").zyUpload({
        width: width,                 // 宽度
        height: height,                 // 高度
        itemWidth: "70px",                 // 文件项的宽度
        itemHeight: "55px",                 // 文件项的高度
        url: "/admin/tool/upload?operation=uploadimage&imgsize="+imgsize,  // 上传文件的路径
        multiple: true,                    // 是否可以多个文件上传
        dragDrop: false,                    // 是否可以拖动上传文件
        del: true,                    // 是否可以删除文件
        finishDel: false,  				  // 是否在上传文件完成后删除预览
    });
}
function buildimgs(imgs)
{
    var ttimg = imgs.split(';');
    var html = "";
    for (var i = 0; i < ttimg.length; i++)
    {
        html += '<div style="display:inline-block;" id="proimgs_list' + i + '"><a href="javascript:delimg(\'' + ttimg[i].trim() + '\',' + i + ');" style="float:right;text-align:right;color:#0A6BA0">删除</a><br/><a style="padding:5px;" href="/upload/imgs/thumb350_350/' + ttimg[i] + '" target="_blank">' +
               '<img src="/upload/imgs/thumb100_100/' + ttimg[i].trim() + '" width="50px" height="50px">' +
               '</a></div>';
    }

    $("#prodiv").html(html);
}

function delimg(imgurl, index)
{
    $.jBox.confirm("确定要删除该图片吗？", "提示", function (v, h, f)
    {
        if (v == 'ok')
        {
            $("#upimgurl").val($("#upimgurl").val().replace(imgurl + ";", "").replace(imgurl, ""));
            $("#proimgs_list" + index).remove();
        }
        else if (v == 'cancel')
        {
            // 取消
        }
        return true; //close
    });

}

//提交按钮
$(".submit").click(function ()
{
    //if ($("#upimgurl").val() == "")
    //{
    //    $.jBox.alert("请上传图纸", "提示");
    //    return false
    //}
    
    $("form:first").submit();
    return false;
})