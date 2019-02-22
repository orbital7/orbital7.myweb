using Microsoft.Extensions.Configuration;
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

        public async Task<Site> UpdateThumbnailAsync(
            Site site)
        {
            byte[] thumbnail = null;

            try
            {
                var configuration = this.ServiceProvider.GetRequiredService<IConfiguration>();
                var thumIOAuthCode = configuration["thumIOAuthCode"];
                var url = "https://image.thum.io/get/width/640/crop/800/noanimate/png/";
                if (!string.IsNullOrEmpty(thumIOAuthCode))
                    url += "auth/" + thumIOAuthCode + "/";
                url += site.Url;

                var httpClient = new HttpClient();
                thumbnail = await httpClient.GetByteArrayAsync(url);
            }
            catch(Exception ex)
            {
                // TODO.
            }

            if (thumbnail != null && thumbnail.Length > 0)
            {
                var container = GetWebContainer(site.Web.Key);
                var blob = container.GetBlockBlobReference(site.ThumbnailFilename);
                await blob.UploadFromByteArrayAsync(thumbnail, 0, thumbnail.Length);
            }

            return site;
        }
    }
}
