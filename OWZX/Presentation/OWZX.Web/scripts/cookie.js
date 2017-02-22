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