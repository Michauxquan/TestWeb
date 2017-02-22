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

var fc36 = new Array(10, 60, 270, 360, 300, 0)//豹 顺 对 半 杂
var pkgj = 10;
var pkgyj = new Array(2, 2, 4, 4, 6, 6, 8, 8, 10, 8, 8, 6, 6, 4, 4, 2, 2)

var mode36 = new Array([1, 2, 3, 4, 5], //全包 0
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