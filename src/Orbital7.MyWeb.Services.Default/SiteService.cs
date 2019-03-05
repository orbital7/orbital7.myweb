using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage.Blob;
using Orbital7.MyWeb.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.MyWeb.Services.Default
{
    public class SiteService : StorageServiceBase, ISiteService
    {
        public SiteService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        public async Task<Site> UpdateThumbnailIfDueAsync(
            Site site,
            bool forceUpdate = false)
        {
            if (forceUpdate || site.IsThumbnailUpdateDue)
                return await UpdateThumbnailNowAsync(site);
            else
                return site;
        }

        public async Task<Site> UpdateThumbnailNowAsync(
            Site site)
        {
            byte[] thumbnail = null;

            site.ThumbnailLastUpdatedDateUtc = DateTime.UtcNow;
            site.ThumbnailLastUpdatedSuccess = false;
            site.ThumbnailUpdateError = null;

            Console.WriteLine("Updating Thumbnail for {0}", site.Url);

            // Generate the thumbnail.
            try
            {
                var url = "https://image.thum.io/get/";

                // Use authorization if found.
                var configuration = this.ServiceProvider.GetRequiredService<IConfiguration>();
                var thumIOId = configuration["ThumIOId"];
                var thumIOUrlKey = configuration["ThumIOUrlKey"];
                if (!string.IsNullOrEmpty(thumIOId) && !string.IsNullOrEmpty(thumIOUrlKey))
                    url += "auth/" + thumIOId + "-" + thumIOUrlKey + "/";

                // Add the rendering options and site Url. 
                url += "width/640/crop/762/maxAge/0/noanimate/png/";
                url += site.Url;

                // Ensure we won't get a cached image by appending the mywebnow query param. 
                var delim = site.Url.Contains("?") ? "&" : "?";
                url += delim + "mywebnow=" + DateTime.UtcNow.FormatAsFileSystemSafeDateTime();

                // Download.
                var httpClient = new HttpClient()
                {
                    Timeout = new TimeSpan(0, 3, 0),
                };
                thumbnail = await httpClient.GetByteArrayAsync(url);
            }
            catch (Exception ex)
            {
                site.ThumbnailUpdateError = "Error Generating: " + ex.Message;
                Console.WriteLine("Error generating thumbnail for {0}: {1} {2}",
                    site.Url, ex.Message, ex.StackTrace);
            }

            // Record the thumbnail.
            if (thumbnail != null && thumbnail.Length > 0)
            {
                try
                {
                    var blob = GetThumbnailBlob(site);
                    await blob.UploadFromByteArrayAsync(thumbnail, 0, thumbnail.Length);

                    site.ThumbnailUrl = blob.Uri.ToString();
                    site.ThumbnailLastUpdatedSuccess = true;
                }
                catch(Exception ex)
                {
                    site.ThumbnailUpdateError = "Error Recording: " + ex.Message;
                    Console.WriteLine("Error recording thumbnail for {0}: {1} {2}",
                        site.Url, ex.Message, ex.StackTrace);
                }
            }

            return site;
        }

        private CloudBlockBlob GetThumbnailBlob(
            Site site)
        {
            var container = GetWebContainer(site.Web.Key);
            var blob = container.GetBlockBlobReference(site.ThumbnailFilename);
            return blob;
        }

        public async Task<Site> DeleteThumbnailAsync(
            Site site)
        {
            var blob = GetThumbnailBlob(site);
            await blob.DeleteIfExistsAsync();
            return site;
        }
    }
}
