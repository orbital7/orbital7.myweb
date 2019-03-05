using Microsoft.Extensions.DependencyInjection;
using Orbital7.MyWeb.Models;
using Orbital7.MyWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Home
{
    public class _DialogEditGroupModel
    {
        public GroupInput Input { get; set; }

        public static _DialogEditGroupModel CreateToAdd(
            string webKey,
            Guid categoryId)
        {
            return new _DialogEditGroupModel()
            {
                Input = new GroupInput(webKey, categoryId),
            };
        }

        public static async Task<_DialogEditGroupModel> CreateToUpdateAsync(
            IServiceProvider serviceProvider,
            string webKey,
            Guid groupId)
        {
            var web = await serviceProvider.GetRequiredService<IWebService>()
                .GetAsync(webKey);

            return new _DialogEditGroupModel()
            {
                Input = new GroupInput(webKey, web.GetGroup(groupId)),
            };
        }
    }
}
