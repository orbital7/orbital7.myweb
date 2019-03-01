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
            string webKey,
            Category category);

        Task<Web> UpdateCategoryAsync(
            string webKey,
            Category category);

        Task<Web> DeleteCategoryAsync(
            string webKey,
            Guid categoryId);

        Task<Web> AddGroupAsync(
            string webKey,
            Guid categoryId,
            Group group);

        Task<Web> UpdateGroupAsync(
            string webKey,
            Group group);

        Task<Web> DeleteGroupAsync(
            string webKey,
            Guid groupId);

        Task<Web> AddSiteAsync(
            string webKey,
            Guid groupId,
            Site site);

        Task<Web> UpdateSiteAsync(
            string webKey,
            Site site);

        Task<Web> DeleteSiteAsync(
            string webKey,
            Guid siteId);
    }
}
