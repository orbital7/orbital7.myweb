using Microsoft.Extensions.DependencyInjection;
using Orbital7.MyWeb.Models;
using Orbital7.MyWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Home
{
    public class _DialogEditCategoryModel
    {
        public CategoryInput Input { get; set; }

        public static _DialogEditCategoryModel CreateToAdd(
            string webKey)
        {
            return new _DialogEditCategoryModel()
            {
                Input = new CategoryInput(webKey)
            };
        }

        public static async Task<_DialogEditCategoryModel> CreateToUpdateAsync(
            IServiceProvider serviceProvider,
            string webKey,
            Guid categoryId)
        {
            var web = await serviceProvider.GetRequiredService<IWebService>()
                .GetAsync(webKey);

            return new _DialogEditCategoryModel()
            {
                Input = new CategoryInput(webKey,
                    web.GetCategory(categoryId))
            };
        }
    }
}
