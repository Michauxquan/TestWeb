using System;
using System.Web;
using System.Data;
using System.Text;
using System.Web.Mvc;
using System.Collections.Generic;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.Web.Admin.Models;
using OWZX.Model;

namespace OWZX.Web.Admin.Controllers
{
    /// <summary>
    /// 后台报表统计控制器类
    /// </summary>
    public partial class StatController : BaseAdminController
    {
        /// <summary>
        /// 在线用户列表
        /// </summary>
        /// <param name="provinceId">省id</param>
        /// <param name="cityId">市id</param>
        /// <param name="regionId">区/县id</param>
        /// <param name="pageNumber">当前页数</param>
        /// <param name="pageSize">每页数</param>
        /// <returns></returns>
        public ActionResult OnlineUserList(int provinceId = -1, int cityId = -1, int regionId = -1, int pageNumber = 1, int pageSize = 15)
        {
            int locationType = 0, locationId = 0;
            if (regionId > 0)
            {
                locationType = 2;
                locationId = regionId;
            }
            else if (cityId > 0)
            {
                locationType = 1;
                locationId = cityId;
            }
            else if (provinceId > 0)
            {
                locationType = 0;
                locationId = provinceId;
            }

            PageModel pageModel = new PageModel(pageSize, pageNumber, OnlineUsers.GetOnlineUserCount(locationType, locationId));

            OnlineUserListModel model = new OnlineUserListModel()
            {
                PageModel = pageModel,
                OnlineUserList = OnlineUsers.GetOnlineUserList(pageModel.PageSize, pageModel.PageNumber, locationType, locationId),
                ProvinceId = provinceId,
                CityId = cityId,
                RegionId = regionId
            };

            ShopUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&provinceId={3}&cityId={4}&regionId={5}",
                                                          Url.Action("onlineuserlist"),
                                                          pageModel.PageNumber, pageModel.PageSize,
                                                          provinceId, cityId, regionId));
            return View(model);
        }

        /// <summary>
        /// 在线用户趋势
        /// </summary>
        /// <returns></returns>
        public ActionResult OnlineUserTrend()
        {
            OnlineUserTrendModel model = new OnlineUserTrendModel();

            model.PVStatList = PVStats.GetTodayHourPVStatList();

            return View(model);
        }

        

        /// <summary>
        /// 客户端统计
        /// </summary>
        /// <returns></returns>
        public ActionResult ClientStat()
        {
            ClientStatModel model = new ClientStatModel();

            model.BrowserStat = PVStats.GetBrowserStat();
            model.OSStat = PVStats.GetOSStat();

            return View(model);
        }

        /// <summary>
        /// 地区统计
        /// </summary>
        /// <returns></returns>
        public ActionResult RegionStat()
        {
            RegionStatModel model = new RegionStatModel();

            model.RegionStat = PVStats.GetProvinceRegionStat();

            return View(model);
        }
        /// <summary>
        /// 访问ip统计
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult VisitIpList(int pageSize = 15, int pageNumber = 1)
        {
            List<MD_VisitIP> iplist = PVStats.GetWebFlow(pageNumber, pageSize, " where a.uid>0");
            VisitIPList list = new VisitIPList
            {
                PageModel = new PageModel(pageSize, pageNumber, iplist.Count > 0 ? iplist[0].TotalCount : 0),
                visitipList = iplist
            };
            return View(list);
        }
    }
}
