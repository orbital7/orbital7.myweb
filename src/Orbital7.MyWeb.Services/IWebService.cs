using Orbital7.MyWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.MyWeb.Services
{
    public interface IWebService
    {
        Task<Web> CreateAsync(
            string webKey);

        Task<Web> GetAsync(
            string webKey);

        Task<Web> UpdateAsync(
            Web web);

        Task<Web> UpdateThumbnailsIfDueAsync(
            Web web,
            bool forceUpdate = false);

        Task UpdateAllThumbnailsIfDueAsync(
            bool forceUpdate = false);

        Task<Web> AddCategoryAsync(
            CategoryInput input);

        Task<Web> UpdateCategoryAsync(
            CategoryInput input);

        Task<Web> DeleteCategoryAsync(
            string webKey,
            Guid categoryId);

        Task<Web> AddGroupAsync(
            GroupInput input);

        Task<Web> UpdateGroupAsync(
            GroupInput input);

        Task<Web> DeleteGroupAsync(
            string webKey,
            Guid groupId);

        Task<Web> AddSiteAsync(
            SiteInput input);

        Task<Web> UpdateSiteAsync(
            SiteInput input);

        Task<Web> DeleteSiteAsync(
            string webKey,
            Guid siteId);
    }
}
