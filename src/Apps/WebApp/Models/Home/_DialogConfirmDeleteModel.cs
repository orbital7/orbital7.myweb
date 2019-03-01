using Microsoft.Extensions.DependencyInjection;
using Orbital7.MyWeb.Models;
using Orbital7.MyWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Home
{
    public class _DialogConfirmDeleteModel
    {
        public string WebKey { get; set; }

        public Guid WebObjectId { get; set; }

        public WebObjectType WebObjectType { get; set; }

        public string Value { get; set; }

        public static async Task<_DialogConfirmDeleteModel> CreateAsync(
            IServiceProvider serviceProvider,
            string webKey,
            WebObjectType webObjectType,
            Guid webObjectId)
        {
            var web = await serviceProvider.GetRequiredService<IWebService>()
                .GetAsync(webKey);
            var webObject = web.GetWebObject(webObjectType, webObjectId);

            return new _DialogConfirmDeleteModel()
            {
                WebKey = webKey,
                WebObjectId = webObject.Id,
                WebObjectType = webObject.Type,
                Value = webObject.Value,
            };
        }
    }
}
