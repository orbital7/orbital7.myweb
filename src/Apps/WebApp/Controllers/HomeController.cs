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

        public HomeController(IServiceProvider serviceProvider)
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
            var web = await this.ServiceProvider.GetRequiredService<IWebService>()
                .GetAsync(webKey);
            return Json(web);
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
