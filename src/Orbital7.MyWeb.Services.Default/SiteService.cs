﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            Console.WriteLine("Updating Thumbnail for {0}", site.Url);

            // Generate the thumbnail.
            try
            {
                var configuration = this.ServiceProvider.GetRequiredService<IConfiguration>();
                var thumIOAuthCode = configuration["ThumIOAuthCode"];
                var url = "https://image.thum.io/get/width/640/crop/762/maxAge/0/noanimate/png/";
                if (!string.IsNullOrEmpty(thumIOAuthCode))
                    url += "auth/" + thumIOAuthCode + "/";
                url += site.Url + "?mywebnow=" + DateTime.UtcNow.FormatAsFileSystemSafeDateTime();

                var httpClient = new HttpClient();
                thumbnail = await httpClient.GetByteArrayAsync(url);
            }
            catch (Exception ex)
            {
                // TODO: Log eventually.
                Console.WriteLine("Error generating thumbnail for {0}: {1} {2}",
                    site.Url, ex.Message, ex.StackTrace);
            }

            // Record the thumbnail.
            if (thumbnail != null && thumbnail.Length > 0)
            {
                try
                {
                    var container = GetWebContainer(site.Web.Key);
                    var blob = container.GetBlockBlobReference(site.ThumbnailFilename);
                    await blob.UploadFromByteArrayAsync(thumbnail, 0, thumbnail.Length);

                    site.ThumbnailLastUpdatedSuccess = true;
                }
                catch(Exception ex)
                {
                    // TODO: Log eventually.
                    Console.WriteLine("Error recording thumbnail for {0}: {1} {2}",
                        site.Url, ex.Message, ex.StackTrace);
                }
            }

            return site;
        }
    }
}
