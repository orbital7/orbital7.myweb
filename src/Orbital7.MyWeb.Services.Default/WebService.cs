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

            var blob = container.GetBlockBlobReference(WEB_BLOB_NAME);
            await blob.UploadTextAsync(web.Serialize());

            return web;
        }
    }
}
