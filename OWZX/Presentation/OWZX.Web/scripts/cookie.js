function setCookie(name,value,expiredays){
    var exp  = new Date();
    exp.setDate(exp.getDate() + expiredays);  
    document.cookie = name + "="+ escape (value) + ((expiredays == null) ? "" : ";expires=" + exp.toGMTString());  
}
function getCookie(name){  
    var arr = document.cookie.match(new RegExp("(^| )"+name+"=([^;]*)(;|$)"));  
    if(arr != null){  
        return (arr[2]);  
    }else{  
        return "";  
    }  
}
function delCookie(name){  
    var exp = new Date();  
    exp.setTime(exp.getTime() - 1);  
    var cval=getCookie(name);  
    if(cval!=null) document.cookie= name + "="+cval+";expires="+exp.toGMTString();  
}
/*数字增加，分割*/
function transStr(str) {
    str = str.toString()
    var begin = "";
    var after = "";
    var l;
    var str2 = "";
    if (str.indexOf(".") > 0) {
        begin = str.substring(0, str.indexOf("."));
        after = str.substring(str.indexOf("."), str.length);
    }
    else {
        begin = str;
    }

    l = begin.length / 3;
    if (l > 1) {
        for (var i = 0; i < l;) {
            str2 = "," + begin.substring(begin.length - 3, begin.length) + str2;
            begin = begin.substring(0, begin.length - 3);
            l = begin.length / 3;
        }
        if (after.length < 3) {
            str2 = begin + str2 + after;
        } else {
            str2 = begin + str2 + after
        }
        return str2.substring(1);
    }
    else {
        if (after.length < 3) {
            return str;

        } else {
            return str;
        }
    }
}