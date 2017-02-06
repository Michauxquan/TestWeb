using System;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.Threading;
using System.Collections.Generic;

using OWZX.Core;
using OWZX.Services;
using OWZX.Web.Framework;
using OWZX.Web.Admin.Models;

namespace OWZX.Web.Admin.Controllers
{
    /// <summary>
    /// 后台基础配置控制器类
    /// </summary>
    public partial class BaseSetController : BaseAdminController
    {
        /// <summary>
        /// 列表
        /// </summary>
        public ActionResult List()
        {
            BaseSetListModel model = new BaseSetListModel();

            model.BaseSetList = BSPConfig.BaseConfig.BaseList;

            ShopUtils.SetAdminRefererCookie(Url.Action("list"));

            return View(model);
        }
        private void Load()
        {
            string allowImgType = string.Empty;
            string[] imgTypeList = StringHelper.SplitString(BSPConfig.ShopConfig.UploadImgType, ",");
            foreach (string imgType in imgTypeList)
                allowImgType += string.Format("{0},", imgType.ToLower());
            allowImgType = allowImgType.Replace(".", "");
            allowImgType = allowImgType.TrimEnd(',');
            ViewData["allowImgType"] = allowImgType;
            ViewData["maxImgSize"] = BSPConfig.ShopConfig.UploadImgSize;

            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
        }
        /// <summary>
        /// 添加基础信息
        /// </summary>
        [HttpGet]
        public ActionResult Add()
        {
            Load();
            BaseInfo model = new BaseInfo();
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 添加基础信息
        /// </summary>
        [HttpPost]
        public ActionResult Add(BaseInfo model)
        {
            Load();
            if (!string.IsNullOrWhiteSpace(model.Key) && BSPConfig.BaseConfig.BaseList.Find(x => x.Key == model.Key.Trim().ToLower()) != null)
                ModelState.AddModelError("Key", "键已经存在");


            BaseInfo eventInfo = new BaseInfo()
                {
                    Key = model.Key,
                    Account = model.Account,
                    Name = model.Name,
                    Bank = model.Bank,
                    BankAddress = model.BankAddress
                };

            BSPConfig.BaseConfig.BaseList.Add(eventInfo);
            BSPConfig.SaveBaseConfig(BSPConfig.BaseConfig);

            return PromptView("添加成功");

        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        [HttpGet]
        public ActionResult Edit(string key = "")
        {
            Load();
            BaseInfo eventInfo = BSPConfig.BaseConfig.BaseList.Find(x => x.Key == key);
            if (eventInfo == null)
                return PromptView("基础信息不存在");

            BaseInfo model = new BaseInfo();
            model.Key = eventInfo.Key.Trim();
            model.Name = eventInfo.Name;
            model.Account = eventInfo.Account;
            model.Bank = eventInfo.Bank;
            model.BankAddress = eventInfo.BankAddress;
            model.Image = eventInfo.Image;

            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        [HttpPost]
        public ActionResult Edit(BaseInfo model)
        {
            Load();
            BaseInfo eventInfo = null;

            if (!string.IsNullOrWhiteSpace(model.Key))
                eventInfo = BSPConfig.BaseConfig.BaseList.Find(x => x.Key == model.Key);

            if (eventInfo == null)
                return PromptView("基础信息不存在");


            //eventInfo.Key = model.Key.Trim().ToLower(),
            eventInfo.Key = model.Key.Trim();
            eventInfo.Name = model.Name;
            eventInfo.Account = model.Account;
            eventInfo.Bank = model.Bank;
            eventInfo.BankAddress = model.BankAddress;
            eventInfo.Image = model.Image;
            BSPConfig.SaveBaseConfig(BSPConfig.BaseConfig);
            return PromptView("保存成功");


        }

    }
}
