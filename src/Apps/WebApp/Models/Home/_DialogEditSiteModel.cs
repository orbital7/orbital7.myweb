using Microsoft.Extensions.DependencyInjection;
using Orbital7.MyWeb.Models;
using Orbital7.MyWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Home
{
    public class _DialogEditSiteModel
    {
        public SiteInput Input { get; set; }

        public Site Site { get; set; }

        public async Task<_DialogEditSiteModel> UpdateReferencesAsync(
            IServiceProvider serviceProvider)
        {
            var web = await serviceProvider.GetRequiredService<IWebService>()
                .GetAsync(this.Input.WebKey);
            this.Site = web.GetSite(this.Input.Id);
            
            return this;
        }

        public static _DialogEditSiteModel CreateToAdd(
            string webKey,
            Guid groupId)
        {
            return new _DialogEditSiteModel()
            {
                Input = new SiteInput(webKey, groupId),
            };
        }

        public static async Task<_DialogEditSiteModel> CreateToUpdateAsync(
            IServiceProvider serviceProvider,
            string webKey,
            Guid siteId)
        {
            var web = await serviceProvider.GetRequiredService<IWebService>()
                .GetAsync(webKey);
            var site = web.GetSite(siteId);

            return new _DialogEditSiteModel()
            {
                Input = new SiteInput(webKey, site),
                Site = site,
            };
        }
    }
}
