﻿var nub = new Array(1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 63, 69, 73, 75, 75, 73, 69, 63, 55, 45, 36, 28, 21, 15, 10, 6, 3, 1);
var nub1 = new Array(1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 63, 69, 73, 75, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 63, 69, 73, 75);
var mode = new Array([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27], //全包 0
					 [1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27], //单 1
					 [0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26], //双 2
					 [14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27], //大 3
					 [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13], //小 4
					 [10, 11, 12, 13, 14, 15, 16, 17], //中 5
					 [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27], //边 6
					 [15, 17, 19, 21, 23, 25, 27], //大单 7 
					 [1, 3, 5, 7, 9, 11, 13], //小单 8
					 [14, 16, 18, 20, 22, 24, 26], //大双 9
					 [0, 2, 4, 6, 8, 10, 12], //小双 10
					 [18, 19, 20, 21, 22, 23, 24, 25, 26, 27], //大边
					 [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], //小边
					 [1, 3, 5, 7, 9, 19, 21, 23, 25, 27], //单边
					 [0, 2, 4, 6, 8, 18, 20, 22, 24, 26], //双边
					 [0, 10, 20], //0尾
					 [1, 11, 21], //1尾
					 [2, 12, 22], //2尾
					 [3, 13, 23], //3尾
					 [4, 14, 24], //4尾
					 [0, 1, 2, 3, 4, 10, 11, 12, 13, 14, 20, 21, 22, 23, 24], //小尾
					 [5, 15, 25], //5尾
					 [6, 16, 26], //6尾
					 [7, 17, 27], //7尾
					 [8, 18], //8尾
					 [9, 19], //9尾
					 [5, 6, 7, 8, 9, 15, 16, 17, 18, 19, 25, 26, 27], //大尾
					 [0, 3, 6, 9, 12, 15, 18, 21, 24, 27], //3余0
					 [1, 4, 7, 10, 13, 16, 19, 22, 25], //3余1
					 [2, 5, 8, 11, 14, 17, 20, 23, 26], //3余2
					 [0, 4, 8, 12, 16, 20, 24], //4余0
					 [1, 5, 9, 13, 17, 21, 25], //4余1
					 [2, 6, 10, 14, 18, 22, 26], //4余2
					 [3, 7, 11, 15, 19, 23, 27], //4余3
					 [0, 5, 10, 15, 20, 25], //5余0
					 [1, 6, 11, 16, 21, 26], //5余1
					 [2, 7, 12, 17, 22, 27], //5余2
					 [3, 8, 13, 18, 23], //5余3
					 [4, 9, 14, 19, 24]//5余4
					 );

var fc36 = new Array(10, 60, 270, 360, 300, 0);//豹 顺 对 半 杂
var ddlhb = new Array(10, 60, 270, 360, 620,870,1020,870,620,360,270,60,10);//大 小 单 双 极大 大单 小单 大双 小双 极小 龙 虎 豹
var pkgj = 100;
var pkgyj = new Array(20, 20, 40, 40, 60, 60, 80, 80, 100, 80, 80, 60, 60, 40, 40, 20, 20);
var js11 = new Array(20, 60, 100, 180, 360, 720, 360, 180, 100, 60, 20);
var js16 = new Array(60, 100, 180, 270, 370, 430, 518, 720, 720, 518, 430, 370, 270, 180, 100, 60);
var lhc = new Array(6, 8, 10, 14, 16, 18, 27, 32, 36, 38, 40, 43, 48,50, 51,53,55,57,58,60,62,64,66,68, 72,
    72, 68, 66, 64, 62, 60, 58, 57, 55, 53, 51, 50, 48, 43, 40, 38, 36, 32, 27, 18, 16, 14, 10, 8, 6);
var modelhb = new Array(["大","小","单","双","极大","大单","小单","大双","小双","极小","龙","虎","豹"]);

var mode36 = new Array(["豹", "顺", "对", "半", "杂"],//[1, 2, 3, 4, 5], //全包 0
                     [1, 3, 5], //单 1
					 [2, 4], //双 2
					 [4, 5], //大 3
					 [1, 2], //小 4
					 [3], //中 5
					 [1, 5], //边 6
					 [5], //大单 7 
					 [1], //小单 8
					 [4], //大双 9
					 [2], //小双 10
					 [4, 5], //大边
					 [1, 2], //小边
					 [2, 4], //单边
					 [1, 5], //双边
					 [77], //0尾
					 [1], //1尾
					 [2], //2尾
					 [3], //3尾
					 [4], //4尾
					 [1, 2, 3, 4], //小尾
					 [5], //5尾
					 [6], //6尾
					 [6], //7尾
					 [6], //8尾
					 [6], //9尾
					 [5], //大尾
					 [2], //3余0
					 [1, 4], //3余1
					 [2, 5], //3余2
					 [4], //4余0
					 [1, 5], //4余1
					 [2], //4余2
					 [3], //4余3
					 [5], //5余0
					 [1], //5余1
					 [2], //5余2
					 [3], //5余3
					 [4]//5余4 
    );
 
var modegj = new Array([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], //全包 0
					 [1, 3, 5, 7, 9], //单 1
					 [2, 4, 6, 8, 10], //双 2
					 [6, 7, 8, 9, 10], //大 3
					 [1, 2, 3, 4, 5], //小 4
					 [4, 5, 6, 7], //中 5
					 [1, 2, 3, 8, 9, 10], //边 6
					 [7, 9], //大单 7 
					 [1, 3, 5], //小单 8
					 [6, 8, 10], //大双 9
					 [2, 4], //小双 10
					 [8, 9, 10], //大边
					 [1, 2, 3], //小边
					 [2, 8, 10], //单边
					 [1, 3, 9], //双边
					 [10], //0尾
					 [1], //1尾
					 [2], //2尾
					 [3], //3尾
					 [4], //4尾
					 [1, 2, 3, 4, 10], //小尾
					 [5], //5尾
					 [6], //6尾
					 [7], //7尾
					 [8], //8尾
					 [9], //9尾
					 [5, 6, 7, 8, 9], //大尾
					 [3, 6, 9], //3余0
					 [1, 4, 7, 10], //3余1
					 [2, 5, 8], //3余2
					 [4, 8], //4余0
					 [1, 5, 9], //4余1
					 [2, 6, 10], //4余2
					 [3, 7], //4余3
					 [5, 10], //5余0
					 [1, 6], //5余1
					 [2, 7], //5余2
					 [3, 8], //5余3
					 [4, 9]//5余4
					 );
var modegyj = new Array([3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19], //全包 0
					 [3, 5, 7, 9, 11, 13, 15, 17, 19], //单 1
					 [4, 6, 8, 10, 12, 14, 16, 18], //双 2
					 [12, 13, 14, 15, 16, 17, 18, 19], //大 3
					 [3, 4, 5, 6, 7, 8, 9, 10, 11], //小 4
					 [8, 9, 10, 11, 12, 13, 14], //中 5
					 [3, 4, 5, 6, 7, 15, 16, 17, 18, 19], //边 6
					 [13, 15, 17, 19], //大单 7 
					 [3, 5, 7, 9, 11], //小单 8
					 [12, 14, 16, 18], //大双 9
					 [4, 6, 8, 10], //小双 10
					 [15, 16, 17, 18, 19], //大边
					 [3, 4, 5, 6, 7], //小边
					 [3, 5, 7, 15, 17, 19], //单边
					 [4, 6, 16, 18], //双边
					 [10], //0尾
					 [11], //1尾
					 [12], //2尾
					 [3, 13], //3尾
					 [4, 14], //4尾
					 [3, 4, 10, 11, 12, 13, 14], //小尾
					 [5, 15], //5尾
					 [6, 16], //6尾
					 [7, 17], //7尾
					 [8, 18], //8尾
					 [9, 19], //9尾
					 [5, 6, 7, 8, 9, 15, 16, 17, 18, 19], //大尾
					 [3, 6, 9, 12, 15, 18], //3余0
					 [4, 7, 10, 13, 16, 19], //3余1
					 [5, 8, 11, 14, 17], //3余2
					 [4, 8, 12, 16], //4余0
					 [5, 9, 13, 17], //4余1
					 [6, 10, 14, 18], //4余2
					 [3, 7, 11, 15, 19], //4余3
					 [5, 10, 15], //5余0
					 [6, 11, 16], //5余1
					 [7, 12, 17], //5余2
					 [3, 8, 13, 18], //5余3
					 [4, 9, 14, 19]//5余4
					 );
var modejs11 = new Array([2,3, 4, 5, 6, 7, 8, 9, 10, 11, 12], //全包 0
					 [3, 5, 7, 9, 11], //单 1
					 [2,4, 6, 8, 10, 12], //双 2
					 [7,8, 9, 10, 11, 12], //大 3
					 [2, 3, 4, 5, 6], //小 4
					 [5, 6, 7, 8, 9], //中 5
					 [2, 3, 4, 10, 11, 12], //边 6
					 [7, 9, 11], //大单 7 
					 [3, 5], //小单 8
					 [8, 10, 12], //大双 9
					 [2, 4, 6], //小双 10
					 [10, 11, 12], //大边
					 [2, 3, 4], //小边
					 [3, 5,  9, 11], //单边
					 [2, 4, 10, 12], //双边
					 [10], //0尾
					 [11], //1尾
					 [2,12], //2尾
					 [3], //3尾
					 [4], //4尾
					 [3, 4, 10, 11, 12], //小尾
					 [5], //5尾
					 [6], //6尾
					 [7], //7尾
					 [8], //8尾
					 [9], //9尾
					 [5, 6, 7, 8, 9], //大尾
					 [3, 6, 9, 12], //3余0
					 [4, 7, 10], //3余1
					 [5, 8, 11], //3余2
					 [4, 8, 12], //4余0
					 [5, 9], //4余1
					 [6, 10], //4余2
					 [3, 7, 11], //4余3
					 [5, 10], //5余0
					 [6, 11], //5余1
					 [7, 12], //5余2
					 [3, 8], //5余3
					 [4, 9]//5余4
					 );
var modejs16 = new Array([3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18], //全包 0
					 [3, 5, 7, 9, 11, 13, 15, 17], //单 1
					 [4, 6, 8, 10, 12, 14, 16, 18], //双 2
					 [12, 13, 14, 15, 16, 17, 18], //大 3
					 [3, 4, 5, 6, 7, 8, 9, 10, 11], //小 4
					 [8, 9, 10, 11, 12, 13, 14], //中 5
					 [3, 4, 5, 6, 7, 15, 16, 17, 18], //边 6
					 [13, 15, 17], //大单 7 
					 [3, 5, 7, 9, 11], //小单 8
					 [12, 14, 16, 18], //大双 9
					 [4, 6, 8, 10], //小双 10
					 [15, 16, 17, 18], //大边
					 [3, 4, 5, 6, 7], //小边
					 [3, 5, 7, 15, 17], //单边
					 [4, 6, 16, 18], //双边
					 [10], //0尾
					 [11], //1尾
					 [12], //2尾
					 [3, 13], //3尾
					 [4, 14], //4尾
					 [3, 4, 10, 11, 12, 13, 14], //小尾
					 [5, 15], //5尾
					 [6, 16], //6尾
					 [7, 17], //7尾
					 [8, 18], //8尾
					 [9], //9尾
					 [5, 6, 7, 8, 9, 15, 16, 17, 18], //大尾
					 [3, 6, 9, 12, 15, 18], //3余0
					 [4, 7, 10, 13, 16], //3余1
					 [5, 8, 11, 14, 17], //3余2
					 [4, 8, 12, 16], //4余0
					 [5, 9, 13, 17], //4余1
					 [6, 10, 14, 18], //4余2
					 [3, 7, 11, 15], //4余3
					 [5, 10, 15], //5余0
					 [6, 11, 16], //5余1
					 [7, 12, 17], //5余2
					 [3, 8, 13, 18], //5余3
					 [4, 9, 14]//5余4
					 );
var modelhc = new Array([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49], //全包 0
					 [1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35, 37, 39, 41, 43, 45, 47, 49], //单 1
					 [2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48], //双 2
					 [25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49], //大 3
					 [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24], //小 4
					 [8, 21, 22, 23, 24, 25, 26, 27, 28, 29], //中 5
					 [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49], //边 6
					 [25, 27, 29, 31, 33, 35, 37, 39, 41, 43, 45, 47, 49], //大单 7 
					 [1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23], //小单 8
					 [26, 28, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48], //大双 9
					 [2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24], //小双 10
					 [30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49], //大边
					 [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20], //小边
					 [1, 3, 5, 7, 9, 11, 13, 15, 17, 19, 31, 33, 35, 37, 39, 41, 43, 45, 47, 49], //单边
					 [2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48], //双边
					 [10, 20, 30, 40], //0尾
					 [1, 11, 21, 31, 41], //1尾
					 [2, 12, 22, 32, 42], //2尾
					 [3, 13, 23, 33, 43], //3尾
					 [4, 14, 24, 34, 44], //4尾
					 [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 20, 21, 22, 23, 24, 30, 31, 32, 33, 34, 40, 41, 42, 43, 44], //小尾
					 [5, 15, 25, 35, 45], //5尾
					 [6, 16,26,36,46], //6尾
					 [7, 17, 27, 37, 47], //7尾
					 [8, 18, 28, 38, 48], //8尾
					 [9, 19, 29, 39, 49], //9尾
					 [15, 16, 17, 18, 19, 25, 26, 27, 28, 29, 35, 36, 37, 38, 39, 45, 46, 47, 48, 49], //大尾
					 [3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36, 39, 42, 45, 48], //3余0
					 [1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31, 34, 37, 40, 43, 46, 49], //3余1
					 [2, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32, 35, 38, 41, 44, 47], //3余2
					 [4, 8, 12, 16, 20, 24, 28, 32, 36, 40, 44, 48], //4余0
					 [1, 5, 9, 13, 17, 21, 25, 29, 33, 37, 41, 45, 49], //4余1
					 [2, 6, 10, 14, 18, 22, 26, 30, 34, 38, 42, 46], //4余2
					 [3, 7, 11, 15, 19, 23, 27, 31, 35, 39, 43, 47], //4余3
					 [5, 10, 15, 20, 25, 30, 35, 40, 45], //5余0
					 [1, 6, 11, 16, 21, 26, 31, 36, 41, 46], //5余1
					 [2, 7, 12, 17, 22, 27, 32, 37, 42, 47], //5余2
					 [3, 8, 13, 18, 23, 28, 33, 38, 43, 48], //5余3
					 [4, 9, 14, 19, 24, 29, 34, 39, 44, 49]//5余4
					 );