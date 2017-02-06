/**
 * Created by jiangsizhi on 2016/1/12.
 */
function Brower(){
    var instance = {};

    instance.init = function(us){
        if(!us){
            us = navigator.userAgent;
        }
        us = us.toLowerCase();
        instance.system = instance.getSystem(us);
        instance.isIOS9 = instance.checkIOS9(us);
    }

    instance.getSystem = function(us){
        if(us.indexOf("android") != -1 || us.indexOf("linux") != -1){
            return "Android";
        }
        if(us.indexOf("safari") != -1){
            if(us.indexOf("windows") != -1){
                return "pc";
            }
            else{
                if(us.indexOf("mac") != -1){
                    return "ios";
                }
                else{
                    return "Android";
                }
            }
        }
        if(us.indexOf("iphone") != -1 || us.indexOf("ipad") != -1 || us.indexOf("ios") != -1){
            if(us.indexOf("mac") != -1){
                return "ios";
            }
        }
        if(us.indexOf("iuc") != -1 && us.indexOf("mac") != -1){
            return "ios";
        }
        return "pc";
    }

    instance.checkIOS9 = function(us) {
        if(instance.system == "ios"){
            var n = us.match(/OS [9]_\d[_\d]* like Mac OS X/i);
            if(n == null){
                return false;
            }
            return true;
        }
        return false;
    }

    return instance;
}