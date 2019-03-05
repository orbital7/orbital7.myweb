using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Orbital7.MyWeb.Models;
using Orbital7.MyWeb.Services;
using WebApp.Models;
using WebApp.Models.Home;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private IServiceProvider ServiceProvider { get; set; }

        public HomeController(
            IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index()
        {
            return View(IndexModel.Create());
        }

        public async Task<IActionResult> GetWeb(
            string webKey)
        {
            try
            {
                var web = await this.ServiceProvider.GetRequiredService<IWebService>()
                    .GetAsync(webKey);
                return Json(web);
            }
            catch (Exception)
            {
                this.RAFailure();
                return NoContent();
            }
        }

        public IActionResult _DialogSetWeb(
            string webKey)
        {
            return PartialView(_DialogSetWebModel.Create(webKey));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DialogSetWeb(
            _DialogSetWebModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    var web = await this.ServiceProvider.GetRequiredService<IWebService>()
                        .GetAsync(model.WebKey);
                    return Json(web);
                }
                catch (Exception ex)
                {
                    this.HandleException(ex);
                }
            }

            return this.RAFailurePartialView(model);
        }

        public IActionResult _DialogAddCategory(
            string webKey)
        {
            return PartialView("_DialogEditCategory", 
                _DialogEditCategoryModel.CreateToAdd(webKey));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DialogAddCategory(
            _DialogEditCategoryModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    var web = await this.ServiceProvider.GetRequiredService<IWebService>()
                        .AddCategoryAsync(model.Input);
                    return Json(web);
                }
                catch (Exception ex)
                {
                    this.HandleException(nameof(model.Input), ex);
                }
            }

            return this.RAFailurePartialView("_DialogEditCategory", model);
        }

        public async Task<IActionResult> _DialogUpdateCategory(
            string webKey,
            Guid categoryId)
        {
            return PartialView(
                "_DialogEditCategory", 
                await _DialogEditCategoryModel.CreateToUpdateAsync(
                    this.ServiceProvider,
                    webKey,
                    categoryId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DialogUpdateCategory(
            _DialogEditCategoryModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    var web = await this.ServiceProvider.GetRequiredService<IWebService>()
                        .UpdateCategoryAsync(model.Input);
                    return Json(web);
                }
                catch (Exception ex)
                {
                    this.HandleException(nameof(model.Input), ex);
                }
            }

            return this.RAFailurePartialView("_DialogEditCategory", model);
        }

        public async Task<IActionResult> _DialogDeleteCategory(
            string webKey,
            Guid categoryId)
        {
            return PartialView("_DialogConfirmDelete",
                await _DialogConfirmDeleteModel.CreateAsync(
                    this.ServiceProvider,
                    webKey,
                    WebObjectType.Category,
                    categoryId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DialogDeleteCategory(
            _DialogConfirmDeleteModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    var web = await this.ServiceProvider.GetRequiredService<IWebService>()
                        .DeleteCategoryAsync(model.WebKey, model.WebObjectId);
                    return Json(web);
                }
                catch (Exception ex)
                {
                    this.HandleException(ex);
                }
            }

            return this.RAFailurePartialView("_DialogConfirmDelete", model);
        }

        public IActionResult _DialogAddGroup(
            string webKey,
            Guid categoryId)
        {
            return PartialView("_DialogEditGroup",
                _DialogEditGroupModel.CreateToAdd(webKey, categoryId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DialogAddGroup(
            _DialogEditGroupModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    var web = await this.ServiceProvider.GetRequiredService<IWebService>()
                        .AddGroupAsync(model.Input);
                    return Json(web);
                }
                catch (Exception ex)
                {
                    this.HandleException(nameof(model.Input), ex);
                }
            }

            return this.RAFailurePartialView("_DialogEditGroup", model);
        }

        public async Task<IActionResult> _DialogUpdateGroup(
            string webKey,
            Guid groupId)
        {
            return PartialView(
                "_DialogEditGroup", 
                await _DialogEditGroupModel.CreateToUpdateAsync(
                    this.ServiceProvider,
                    webKey,
                    groupId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DialogUpdateGroup(
            _DialogEditGroupModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    var web = await this.ServiceProvider.GetRequiredService<IWebService>()
                        .UpdateGroupAsync(model.Input);
                    return Json(web);
                }
                catch (Exception ex)
                {
                    this.HandleException(nameof(model.Input), ex);
                }
            }

            return this.RAFailurePartialView("_DialogEditGroup", model);
        }

        public async Task<IActionResult> _DialogDeleteGroup(
            string webKey,
            Guid groupId)
        {
            return PartialView("_DialogConfirmDelete",
                await _DialogConfirmDeleteModel.CreateAsync(
                    this.ServiceProvider,
                    webKey,
                    WebObjectType.Group,
                    groupId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DialogDeleteGroup(
            _DialogConfirmDeleteModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    var web = await this.ServiceProvider.GetRequiredService<IWebService>()
                        .DeleteGroupAsync(model.WebKey, model.WebObjectId);
                    return Json(web);
                }
                catch (Exception ex)
                {
                    this.HandleException(ex);
                }
            }

            return this.RAFailurePartialView("_DialogConfirmDelete", model);
        }

        public IActionResult _DialogAddSite(
            string webKey,
            Guid groupId)
        {
            return PartialView("_DialogEditSite", 
                _DialogEditSiteModel.CreateToAdd(webKey, groupId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DialogAddSite(
            _DialogEditSiteModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    var web = await this.ServiceProvider.GetRequiredService<IWebService>()
                        .AddSiteAsync(model.Input);
                    return Json(web);
                }
                catch (Exception ex)
                {
                    this.HandleException(nameof(model.Input), ex);
                }
            }

            return this.RAFailurePartialView("_DialogEditSite", model);
        }

        public async Task<IActionResult> _DialogUpdateSite(
            string webKey,
            Guid siteId)
        {
            return PartialView(
                "_DialogEditSite", 
                await _DialogEditSiteModel.CreateToUpdateAsync(
                    this.ServiceProvider,
                    webKey,
                    siteId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DialogUpdateSite(
            _DialogEditSiteModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    var web = await this.ServiceProvider.GetRequiredService<IWebService>()
                        .UpdateSiteAsync(model.Input);
                    return Json(web);
                }
                catch (Exception ex)
                {
                    this.HandleException(nameof(model.Site), ex);
                }
            }

            return this.RAFailurePartialView("_DialogEditSite", 
                await model.UpdateReferencesAsync(this.ServiceProvider));
        }

        public async Task<IActionResult> _DialogDeleteSite(
            string webKey,
            Guid siteId)
        {
            return PartialView("_DialogConfirmDelete",
                await _DialogConfirmDeleteModel.CreateAsync(
                    this.ServiceProvider,
                    webKey,
                    WebObjectType.Site,
                    siteId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DialogDeleteSite(
            _DialogConfirmDeleteModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    var web = await this.ServiceProvider.GetRequiredService<IWebService>()
                        .DeleteSiteAsync(model.WebKey, model.WebObjectId);
                    return Json(web);
                }
                catch (Exception ex)
                {
                    this.HandleException(ex);
                }
            }

            return this.RAFailurePartialView("_DialogConfirmDelete", model);
        }
    }
}
