$(function(){
        var baseSize = 16; // 基数
        var baseWidth = 540; //量取值大小
        document.getElementsByTagName('html')[0].style.fontSize  =  (document.documentElement.clientWidth / baseWidth * baseSize).toFixed(2) + 'px'; //这个就是动态font-size值
    })