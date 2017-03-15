using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.Web.Admin.Models;

namespace OWZX.Web.Admin.Controllers
{
    /// <summary>
    /// 后台公告控制器类
    /// </summary>
    public partial class NewsController : BaseAdminController
    {
        /// <summary>
        /// 系统公告
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditNotice()
        {
           
            NewsInfo newsInfo = AdminNews.AdminGetNewsById(1);
            if (newsInfo == null)
                return PromptView("没有系统公告不存在");
            ShopUtils.SetAdminRefererCookie(Url.Action("editnotice"));
            NewsModel model = new NewsModel();
            model.NewsTypeId = newsInfo.NewsTypeId;
            model.IsShow = newsInfo.IsShow;
            model.IsTop = newsInfo.IsTop;
            model.IsHome = newsInfo.IsHome;
            model.DisplayOrder = newsInfo.DisplayOrder;
            model.Title = newsInfo.Title;
            model.Url = newsInfo.Url;
            model.Body = newsInfo.Body;
            return View(model);
        }
        /// <summary>
        /// 系统公告
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditNotice(NewsModel model)
        {
            NewsInfo newsInfo = AdminNews.AdminGetNewsById(1);
            if (newsInfo == null)
                return PromptView("没有系统公告不存在");

            newsInfo.Body = model.Body;

            bool result = AdminNews.UpdateNews(newsInfo);
            if (result == true)
                return PromptView("更新成功");
            else
                return PromptView("更新失败");
        }
        /// <summary>
        /// 公告类型列表
        /// </summary>
        public ActionResult NewsTypeList()
        {
            NewsTypeListModel model = new NewsTypeListModel()
            {
                NewsTypeList = News.GetNewsTypeList()
            };
            ShopUtils.SetAdminRefererCookie(Url.Action("newstypelist"));
            return View(model);
        }

        /// <summary>
        /// 添加公告类型
        /// </summary>
        [HttpGet]
        public ActionResult AddNewsType()
        {
            NewsTypeModel model = new NewsTypeModel();
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 添加公告类型
        /// </summary>
        [HttpPost]
        public ActionResult AddNewsType(NewsTypeModel model)
        {
            if (AdminNews.GetNewsTypeByName(model.NewsTypeName) != null)
                ModelState.AddModelError("NewsTypeName", "名称已经存在");

            if (ModelState.IsValid)
            {
                NewsTypeInfo newsTypeInfo = new NewsTypeInfo()
                {
                    Name = model.NewsTypeName,
                    DisplayOrder = model.DisplayOrder
                };

                AdminNews.CreateNewsType(newsTypeInfo);
                AddAdminOperateLog("添加公告类型", "添加公告类型,公告类型为:" + model.NewsTypeName);
                return PromptView("公告类型添加成功");
            }
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 编辑公告类型
        /// </summary>
        [HttpGet]
        public ActionResult EditNewsType(int newsTypeId = -1)
        {
            NewsTypeInfo newsTypeInfo = AdminNews.GetNewsTypeById(newsTypeId);
            if (newsTypeInfo == null)
                return PromptView("公告类型不存在");

            NewsTypeModel model = new NewsTypeModel();
            model.NewsTypeName = newsTypeInfo.Name;
            model.DisplayOrder = newsTypeInfo.DisplayOrder;
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();

            return View(model);
        }

        /// <summary>
        /// 编辑公告类型
        /// </summary>
        [HttpPost]
        public ActionResult EditNewsType(NewsTypeModel model, int newsTypeId = -1)
        {
            NewsTypeInfo newsTypeInfo = AdminNews.GetNewsTypeById(newsTypeId);
            if (newsTypeInfo == null)
                return PromptView("公告类型不存在");

            NewsTypeInfo newsTypeInfo2 = AdminNews.GetNewsTypeByName(model.NewsTypeName);
            if (newsTypeInfo2 != null && newsTypeInfo2.NewsTypeId != newsTypeId)
                ModelState.AddModelError("NewsTypeName", "名称已经存在");

            if (ModelState.IsValid)
            {
                newsTypeInfo.Name = model.NewsTypeName;
                newsTypeInfo.DisplayOrder = model.DisplayOrder;

                AdminNews.UpdateNewsType(newsTypeInfo);
                AddAdminOperateLog("修改公告类型", "修改公告类型,公告类型ID为:" + newsTypeId);
                return PromptView("公告类型修改成功");
            }

            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 删除公告类型
        /// </summary>
        public ActionResult DelNewsType(int newsTypeId = -1)
        {
            AdminNews.DeleteNewsTypeById(newsTypeId);
            AddAdminOperateLog("删除公告类型", "删除公告类型,公告类型ID为:" + newsTypeId);
            return PromptView("公告类型删除成功");
        }



        /// <summary>
        /// 公告列表
        /// </summary>
        public ActionResult NewsList(string newsTitle, int newsTypeId = 0, int pageSize = 15, int pageNumber = 1)
        {
            string condition = AdminNews.AdminGetNewsListCondition(newsTypeId, newsTitle);

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminNews.AdminGetNewsCount(condition));

            List<SelectListItem> newsTypeList = new List<SelectListItem>();
            newsTypeList.Add(new SelectListItem() { Text = "全部类型", Value = "0" });
            foreach (NewsTypeInfo newsTypeInfo in AdminNews.GetNewsTypeList())
            {
                newsTypeList.Add(new SelectListItem() { Text = newsTypeInfo.Name, Value = newsTypeInfo.NewsTypeId.ToString() });
            }

            NewsListModel model = new NewsListModel()
            {
                NewsList = AdminNews.AdminGetNewsList(pageModel.PageSize, pageModel.PageNumber, condition),
                PageModel = pageModel,
                NewsTypeId = newsTypeId,
                NewsTypeList = newsTypeList,
                NewsTitle = newsTitle
            };
            ShopUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&newsTypeId={3}&newsTitle={4}",
                                                          Url.Action("newslist"),
                                                          pageModel.PageNumber,
                                                          pageModel.PageSize,
                                                          newsTypeId,
                                                          newsTitle));

            return View(model);
        }

        /// <summary>
        /// 添加公告
        /// </summary>
        [HttpGet]
        public ActionResult AddNews()
        {
            NewsModel model = new NewsModel();
            Load();
            return View(model);
        }

        /// <summary>
        /// 添加公告
        /// </summary>
        [HttpPost]
        public ActionResult AddNews(NewsModel model)
        {
            if (AdminNews.AdminGetNewsIdByTitle(model.Title) > 0)
                ModelState.AddModelError("Title", "标题已经存在");

            if (ModelState.IsValid)
            {
                NewsInfo newsInfo = new NewsInfo()
                {
                    NewsTypeId = model.NewsTypeId,
                    IsShow = model.IsShow,
                    IsTop = model.IsTop,
                    IsHome = model.IsHome,
                    DisplayOrder = model.DisplayOrder,
                    AddTime = DateTime.Now,
                    Title = model.Title,
                    Url = model.Url == null ? "" : model.Url,
                    Body = model.Body ?? "",
                    BTime = model.BTime ?? DateTime.Now,
                    ETime = model.ETime ?? DateTime.Now
                };

                AdminNews.CreateNews(newsInfo);
                AddAdminOperateLog("添加公告", "添加公告,公告为:" + model.Title);
                return PromptView("公告添加成功");
            }

            Load();
            return View(model);
        }

        /// <summary>
        /// 编辑公告
        /// </summary>
        [HttpGet]
        public ActionResult EditNews(int newsId = -1)
        {
            NewsInfo newsInfo = AdminNews.AdminGetNewsById(newsId);
            if (newsInfo == null)
                return PromptView("公告不存在");

            NewsModel model = new NewsModel();
            model.NewsTypeId = newsInfo.NewsTypeId;
            model.IsShow = newsInfo.IsShow;
            model.IsTop = newsInfo.IsTop;
            model.IsHome = newsInfo.IsHome;
            model.DisplayOrder = newsInfo.DisplayOrder;
            model.Title = newsInfo.Title;
            model.Url = newsInfo.Url;
            model.Body = newsInfo.Body;
            model.BTime = newsInfo.BTime;
            model.ETime = newsInfo.ETime;
            Load();
            return View(model);
        }

        /// <summary>
        /// 编辑公告
        /// </summary>
        [HttpPost]
        public ActionResult EditNews(NewsModel model, int newsId = -1)
        {
            NewsInfo newsInfo = AdminNews.AdminGetNewsById(newsId);
            if (newsInfo == null)
                return PromptView("公告不存在");

            int newsId2 = AdminNews.AdminGetNewsIdByTitle(model.Title);
            if (newsId2 > 0 && newsId2 != newsId)
                ModelState.AddModelError("Title", "名称已经存在");

            if (ModelState.IsValid)
            {
                newsInfo.NewsTypeId = model.NewsTypeId;
                newsInfo.IsShow = model.IsShow;
                newsInfo.IsTop = model.IsTop;
                newsInfo.IsHome = model.IsHome;
                newsInfo.DisplayOrder = model.DisplayOrder;
                newsInfo.Title = model.Title;
                newsInfo.Url = model.Url == null ? "" : model.Url;
                newsInfo.Body = model.Body ?? "";
                newsInfo.BTime = model.BTime ?? DateTime.Now;
                newsInfo.ETime = model.ETime ?? DateTime.Now;
                AdminNews.UpdateNews(newsInfo);
                AddAdminOperateLog("修改公告", "修改公告,公告ID为:" + newsId);
                return PromptView("公告修改成功");
            }

            Load();
            return View(model);
        }

        /// <summary>
        /// 删除公告
        /// </summary>
        public ActionResult DelNews(int[] newsIdList)
        {
            AdminNews.DeleteNewsById(newsIdList);
            AddAdminOperateLog("删除公告", "删除公告,公告ID为:" + CommonHelper.IntArrayToString(newsIdList));
            return PromptView("公告删除成功");
        }

        private void Load()
        {
            List<SelectListItem> newsTypeList = new List<SelectListItem>();
            newsTypeList.Add(new SelectListItem() { Text = "请选择类型", Value = "0" });
            foreach (NewsTypeInfo newsTypeInfo in AdminNews.GetNewsTypeList())
            {
                newsTypeList.Add(new SelectListItem() { Text = newsTypeInfo.Name, Value = newsTypeInfo.NewsTypeId.ToString() });
            }
            ViewData["newsTypeList"] = newsTypeList;
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
        }
    }
}
