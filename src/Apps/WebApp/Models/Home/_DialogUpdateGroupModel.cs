using Microsoft.Extensions.DependencyInjection;
using Orbital7.MyWeb.Models;
using Orbital7.MyWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Home
{
    public class _DialogUpdateGroupModel
    {
        public string WebKey { get; set; }

        public Group Group { get; set; }

        public static async Task<_DialogUpdateGroupModel> CreateAsync(
            IServiceProvider serviceProvider,
            string webKey,
            Guid groupId)
        {
            var web = await serviceProvider.GetRequiredService<IWebService>()
                .GetAsync(webKey);

            return new _DialogUpdateGroupModel()
            {
                WebKey = webKey,
                Group = web.GetGroup(groupId),
            };
        }
    }
}
