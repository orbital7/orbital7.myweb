using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage.Blob;
using Orbital7.Extensions.Models;
using Orbital7.MyWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Orbital7.MyWeb.Services.Default
{
    public class WebService : StorageServiceBase, IWebService
    {
        private const string WEB_BLOB_NAME = "web";

        public WebService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            
        }

        public async Task<Web> CreateAsync(
            string webKey)
        {
            var web = new Web(webKey);
            return await UpdateAsync(web);
        }

        public async Task<Web> GetAsync(
            string webKey)
        {
            var container = GetWebContainer(webKey);
            return await ReadAsync(container);
        }

        private async Task<Web> ReadAsync(
            CloudBlobContainer container)
        {
            var blob = container.GetBlockBlobReference(WEB_BLOB_NAME);
            var json = await blob.DownloadTextAsync();
            return Web.Load(json);
        }

        public async Task<Web> UpdateAsync(
            Web web)
        {
            var container = GetWebContainer(web.Key);
            var created = await container.CreateIfNotExistsAsync();
            if (created)
            {
                await container.SetPermissionsAsync(new BlobContainerPermissions()
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob,
                });
            }

            return await WriteAsync(container, web);
        }

        private async Task<Web> WriteAsync(
            CloudBlobContainer container,
            Web web)
        {
            var blob = container.GetBlockBlobReference(WEB_BLOB_NAME);
            await blob.UploadTextAsync(web.Serialize());

            return web;
        }

        public async Task<Web> UpdateThumbnailsIfDueAsync(
            Web web,
            bool forceUpdate = false)
        {
            var siteService = this.ServiceProvider.GetRequiredService<ISiteService>();

            foreach (var site in web.GatherAllSites())
                await siteService.UpdateThumbnailIfDueAsync(site, forceUpdate);

            return await UpdateAsync(web);
        }

        public async Task UpdateAllThumbnailsIfDueAsync(
            bool forceUpdate = false)
        {
            var client = CreateCloudBlobClient();
            var webContainers = client.ListContainers(WEB_CONTAINER_PREFIX);
            var siteService = this.ServiceProvider.GetRequiredService<ISiteService>();

            foreach (var webContainer in webContainers)
            {
                var web = await ReadAsync(webContainer);
                foreach (var site in web.GatherAllSites())
                    await siteService.UpdateThumbnailIfDueAsync(site, forceUpdate);
                await WriteAsync(webContainer, web);
            }
        }

        public async Task<Web> AddCategoryAsync(
            string webKey,
            Category category)
        {
            var container = GetWebContainer(webKey);
            var web = await ReadAsync(container);

            var existingCategory = web.Categories.Get(category.Id);
            if (existingCategory == null)
            {
                web.Categories.Add(category);
                return await WriteAsync(container, web);
            }
            else
            {
                throw new Exception("The specified category already exists");
            }
        }

        public async Task<Web> UpdateCategoryAsync(
            string webKey,
            Category category)
        {
            var container = GetWebContainer(webKey);
            var web = await ReadAsync(container);

            var existingCategory = web.Categories.Get(category.Id);
            if (existingCategory == null)
            {
                var index = web.Categories.IndexOf(existingCategory);
                web.Categories[index] = category;
                return await WriteAsync(container, web);
            }
            else
            {
                throw new Exception("The specified category could not be found");
            }
        }

        public async Task<Web> DeleteCategoryAsync(
            string webKey,
            Guid categoryId)
        {
            var container = GetWebContainer(webKey);
            var web = await ReadAsync(container);

            var category = web.GetCategory(categoryId);
            if (category == null)
                throw new Exception("The specified category could not be found");

            var siteService = this.ServiceProvider.GetRequiredService<ISiteService>();
            foreach (var site in category.GatherAllSites())
                await siteService.DeleteThumbnailAsync(site);
            category.Web.Categories.Remove(category);

            return await WriteAsync(container, web);
        }

        public async Task<Web> AddGroupAsync(
            string webKey,
            Guid categoryId,
            Group group)
        {
            var container = GetWebContainer(webKey);
            var web = await ReadAsync(container);

            var existingGroup = web.GetGroup(group.Id);
            if (existingGroup == null)
            {
                var category = web.GetCategory(categoryId);
                if (group != null)
                {
                    category.Groups.Add(group);
                    return await WriteAsync(container, web);
                }
                else
                {
                    throw new Exception("The specified group could not be found");
                }
            }
            else
            {
                throw new Exception("The specified group already exists");
            }
        }

        public async Task<Web> UpdateGroupAsync(
            string webKey,
            Group group)
        {
            var container = GetWebContainer(webKey);
            var web = await ReadAsync(container);

            var existingGroup = web.GetGroup(group.Id);
            if (existingGroup == null)
            {
                var index = existingGroup.Category.Groups.IndexOf(existingGroup);
                existingGroup.Category.Groups[index] = group;
                return await WriteAsync(container, web);
            }
            else
            {
                throw new Exception("The specified group could not be found");
            }
        }

        public async Task<Web> DeleteGroupAsync(
            string webKey,
            Guid groupId)
        {
            var container = GetWebContainer(webKey);
            var web = await ReadAsync(container);

            var group = web.GetGroup(groupId);
            if (group == null)
                throw new Exception("The specified group could not be found");

            var siteService = this.ServiceProvider.GetRequiredService<ISiteService>();
            foreach (var site in group.Sites)
                await siteService.DeleteThumbnailAsync(site);
            group.Category.Groups.Remove(group);

            return await WriteAsync(container, web);
        }

        public async Task<Web> AddSiteAsync(
            string webKey,
            Guid groupId,
            Site site)
        {
            var container = GetWebContainer(webKey);
            var web = await ReadAsync(container);

            var existingSite = web.GetSite(site.Id);
            if (existingSite == null)
            {
                var group = web.GetGroup(groupId);
                if (group != null)
                {
                    group.Sites.Add(site);

                    await this.ServiceProvider.GetRequiredService<ISiteService>()
                        .UpdateThumbnailNowAsync(site);
                    return await WriteAsync(container, web);
                }
                else
                {
                    throw new Exception("The specified group could not be found");
                }
            }
            else
            {
                throw new Exception("The specified site already exists");
            }
        }

        public async Task<Web> UpdateSiteAsync(
            string webKey,
            Site site)
        {
            var container = GetWebContainer(webKey);
            var web = await ReadAsync(container);

            var existingSite = web.GetSite(site.Id);
            if (existingSite == null)
            {
                var index = existingSite.Group.Sites.IndexOf(existingSite);
                existingSite.Group.Sites[index] = site;

                await this.ServiceProvider.GetRequiredService<ISiteService>()
                        .UpdateThumbnailNowAsync(site);
                return await WriteAsync(container, web);
            }
            else
            {
                throw new Exception("The specified site could not be found");
            }
        }

        public async Task<Web> DeleteSiteAsync(
            string webKey,
            Guid siteId)
        {
            var container = GetWebContainer(webKey);
            var web = await ReadAsync(container);

            var site = web.GetSite(siteId);
            if (site == null)
                throw new Exception("The specified site could not be found");

            await this.ServiceProvider.GetRequiredService<ISiteService>()
                .DeleteThumbnailAsync(site);
            site.Group.Sites.Remove(site);

            return await WriteAsync(container, web);
        }
    }
}
