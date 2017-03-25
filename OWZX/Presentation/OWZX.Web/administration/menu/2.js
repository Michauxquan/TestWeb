var menuList = [
     {
         "title": "事务处理",
         "subMenuList": [
                {
                    "title": "提现记录",
                    "url": "/admin/draw/drawlist"
                },
            {
                "title": "充值记录",
                "url": "/admin/newuser/remitlist"
            }, {
                "title": "回水记录",
                "url": "/admin/newuser/backlist"
            }
         ]
     },
     {
         "title": "竞猜管理",
     "subMenuList": [
            {
                "title": "下注记录",
                "url": "/admin/lottery/lotterylist"
            },
            {
                "title": "急速28开奖设置",
                "url": "/admin/lottery/openset"
            },
            {
                "title": "急速10开奖设置",
                "url": "/admin/lottery/openset?type=10"
            },
            {
                "title": "急速11开奖设置",
                "url": "/admin/lottery/openset?type=11"
            },
            {
                "title": "急速16开奖设置",
                "url": "/admin/lottery/openset?type=12"
            }
     ]
     },
       {
           "title": "财务报表",
           "subMenuList": [
               {
                   "title": "盈利报表",
                   "url": "/admin/lottery/profitlist"
               }
           ]
       },
         {
             "title": "公告管理",
             "subMenuList": [
                 {
                     "title": "公告列表",
                     "url": "/admin/news/newslist"
                 },
                 {
                     "title": "广告列表",
                     "url": "/admin/advert/advertlist"
                 }
             ]
         },
	{
	    "title": "用户管理",
	    "subMenuList": [

            {
                "title": "用户列表",
                "url": "/admin/user/list"
            },
            {
                "title": "管理员组",
                "url": "/admin/admingroup/list"
            },
            //{
            //    "title": "银行卡列表",
            //    "url": "/admin/newuser/userbanklist"
            //},
             {
                 "title": "代理信息",
                 "url": "/admin/user/lowerlist"
             },
            {
                "title": "访问IP列表",
                "url": "/admin/stat/visitiplist"
            },
            {
                "title": "账变记录",
                "url": "/admin/newuser/changelist"
            }

	    ]
	},
    {
        "title": "导航管理",
        "subMenuList": [
            {
                "title": "导航菜单",
                "url": "/admin/nav/list"
            }
        ]
    },
	{
	    "title": "系统设置",
	    "subMenuList": [
            {
                "title": "基础类型",
                "url": "/admin/baseinfo/basetypelist"
            }, {
                "title": "客服回复",
                "url": "/admin/baseinfo/baseinfolist"
            },
			{
			    "title": "房间信息",
			    "url": "/admin/baseinfo/roomlist"
			},
			{
			    "title": "回水规则",
			    "url": "/admin/baseinfo/backrulelist"
			},
			{
			    "title": "竞猜赔率",
			    "url": "/admin/baseinfo/lotterysetlist"
			},
			{
			    "title": "基础设置",
			    "url": "/admin/baseset/list"
			}


	    ]
	},
    	{
    	    "title": "商品管理",
    	    "subMenuList": [
                {
                    "title": "商品列表",
                    "url": "/admin/ware/list"
                },
                {
                    "title": "订单列表",
                    "url": "/admin/ware/orderlist"
                }


    	    ]
    	}
]