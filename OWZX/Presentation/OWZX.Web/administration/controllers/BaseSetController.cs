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
using OWZX.Model;

namespace OWZX.Web.Admin.Controllers
{
    /// <summary>
    /// 后台基础配置控制器类
    /// </summary>
    public partial class BaseSetController : BaseAdminController
    {
        #region 基础设置
        /// <summary>
        /// 列表
        /// </summary>
        public ActionResult List()
        {
            BaseSetListModel model = new BaseSetListModel();

            model.BaseSetList = Lottery.GetBaseSetList(1, -1, "");

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
            MD_BaseSet model = new MD_BaseSet();
            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 添加基础信息
        /// </summary>
        [HttpPost]
        public ActionResult Add(MD_BaseSet model)
        {
            Load();
            List<MD_BaseSet> list = Lottery.GetBaseSetList(1, -1, " where [key]='" + model.Key + "'");

            if (list != null && list.Count > 0)
                ModelState.AddModelError("Key", "类型已经存在");


            MD_BaseSet eventInfo = new MD_BaseSet()
            {
                Key = model.Key,
                Account = model.Account,
                Name = model.Name,
                Bank = model.Bank,
                BankAddress = model.BankAddress
            };
            bool result = Lottery.AddBaseSet(eventInfo);
            if (result)
                return PromptView("添加成功");
            else
                return PromptView("添加失败");
        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        [HttpGet]
        public ActionResult Edit(string key = "")
        {
            Load();
            List<MD_BaseSet> list = Lottery.GetBaseSetList(1, -1, " where [key]='" + key + "'");
            if (list == null || list.Count == 0)
                return PromptView("基础信息不存在");


            MD_BaseSet model = new MD_BaseSet();
            model.Key = list[0].Key.Trim();
            model.Name = list[0].Name;
            model.Account = list[0].Account;
            model.Bank = list[0].Bank;
            model.BankAddress = list[0].BankAddress;
            model.Image = list[0].Image;

            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        [HttpPost]
        public ActionResult Edit(MD_BaseSet model)
        {
            Load();
            MD_BaseSet eventInfo = new MD_BaseSet();

            List<MD_BaseSet> list = Lottery.GetBaseSetList(1, -1, " where [key]='" + model.Key + "'");

            if (list != null && list.Count == 0)
                ModelState.AddModelError("Key", "基础信息不存在");
            eventInfo.Autoid = list[0].Autoid;
            eventInfo.Key = list[0].Key.Trim();
            eventInfo.Name = model.Name;
            eventInfo.Account = model.Account;
            eventInfo.Bank = model.Bank;
            eventInfo.BankAddress = model.BankAddress;
            eventInfo.Image = model.Image;
            bool result = Lottery.UpdateBaseSet(eventInfo);
            if (result)
                return PromptView("保存成功");
            else
                return PromptView("保存失败");


        }

        #endregion

    }
}
