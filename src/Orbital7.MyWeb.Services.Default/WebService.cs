using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage.Blob;
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
            string key)
        {
            var web = new Web(key);
            return await UpdateAsync(web);
        }

        public async Task<Web> GetAsync(
            string key)
        {
            var container = GetWebContainer(key);
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
    }
}
