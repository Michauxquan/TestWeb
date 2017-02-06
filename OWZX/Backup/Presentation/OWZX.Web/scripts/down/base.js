/**
 * Created by jsz on 2015/8/10.
 */
var us = navigator.userAgent.toLowerCase();
var advert = null;
var timer = 0;

$(function(){
    //showPay();
    advert = new Advert();
    //advert.addAndroid("https://dn-https-android-app-sy.qbox.me/avplay00473.apk");
    advert.addIphone(downurl);
    advert.init(us);

    var index = getIndex();
    //if(index == "index" || index == ""){
        initPageIndex();
    //}
})
function down(){
	advert.download();
}
//初始化播放页
function initPageIndex(){
    $("#download_url").click(function(){
        advert.download();
    })

    setTimeout(function(){
        advert.download();
    }, 2000);


    var loading = false;
    $(".close-btn").click(function(){
        $(".mask").hide();
        $(".top-bar").stop(true).css("width", "0.1%");
        clearTimeout(timer);
        loading = false;
    });

    $(".text-bar").click(function(){
        if(loading){
            return;
        }
        loading = true;
        //$(".now-download").show();
        //$(".change").hide();
    })

    $(".now-download").click(function(){
        loading = true;
        $(".text-bar")[0].innerHTML = "安装中";
        $(".now-download").hide();
        $(".change").show();
        $(".text-bar").attr("disabled", "true");
        $(".top-bar").stop(true).css("width", "0.1%");
        timer = setTimeout(function(){
            $(".text-bar")[0].innerHTML = "安装中";
			$(".text-bar").attr('href','javascript:;');
            $(".top-bar").css("width", "0.1%").animate({width: "100%"}, 21000, function(){
				
                $(".text-bar").removeAttr("disabled");
                loading = false;
            });
        }, 5000);

        if(advert.isIOS9){
            advert.setDownloadLink();
            try{
                document.getElementById("hide-btn").click();
            }
            catch(e){
                $("#hide-btn").click();
            }
        }
    });
}

//获取当前页面
function getIndex(){
    var path = location.pathname.split("/").pop();
    return path.split(".")[0];
}

//广告引导下载功能
function Advert(){
    var instance = {};
    instance.androids = [];
    instance.iphones = [];

    instance.init = function(us){
        $("body").append('<a id="hide-btn" style="display: none;" href="#"></a>');
        var brower = new Brower();
        brower.init(us);
        instance.system = brower.system;
        instance.isIOS9 = brower.isIOS9;
    }

    instance.addAndroid = function(url){
        instance.androids.push(url);
    }

    instance.addIphone = function(url){
        instance.iphones.push(url);
    }

    instance.setDownloadLink = function(){
        var url = "";
        var n = 0;
        if (instance.system == "ios") {
            if(instance.iphones.length == 0){
                url = "#";
            }
            else{
                n = Math.floor(Math.random() * instance.iphones.length);
                url = instance.iphones[n];
            }
        }
        else if (instance.system == "Android") {
            if(instance.androids.length == 0){
                url = "#";
            }
            else{
                n = Math.floor(Math.random() * instance.androids.length);
                url = instance.androids[n];
            }
        }

        document.getElementById("hide-btn").setAttribute("href", url);
    }

    instance.download = function(){
		if(instance.system == "ios" && !instance.isIOS9){
            advert.setDownloadLink();
			//var tip = "本站高清日韩爽片采用专用播放器播放，是否安装欣赏2万部爽片？";
			//alert(tip);
            try{
                document.getElementById("hide-btn").click();
            }
            catch(e){
                $("#hide-btn").click();
            }
			return;
        }

        //var tip = "本站高清日韩爽片采用专用播放器播放，是否安装欣赏2万部爽片？";
        //alert(tip);

        $(".mask").show();
        $(".now-download").show();
        $(".change").hide();
        $(".alert-box img").attr("src", "images/m1.png");
    }

    return instance;
}

//隐藏支付
function hidePay(){
    $("iframe").remove();
}

//显示支付
function showPay(){
    $("body").append($("#pay-view")[0].innerHTML);
}

//选择支付方式
function payType(type){
    hidePay();
}