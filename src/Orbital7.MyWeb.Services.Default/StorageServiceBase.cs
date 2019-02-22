using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.MyWeb.Services.Default
{
    public abstract class StorageServiceBase
    {
        protected IServiceProvider ServiceProvider { get; private set; }

        public StorageServiceBase(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        protected CloudBlobContainer GetWebContainer(
            string webKey)
        {
            var configuration = this.ServiceProvider.GetRequiredService<IConfiguration>();
            var storageAccount = CloudStorageAccount.Parse(configuration["StorageConnectionString"]);
            var client = storageAccount.CreateCloudBlobClient();
            var containerName = "myweb-" + webKey;
            var container = client.GetContainerReference(containerName);

            return container;
        }
    }
}
