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
            string key);

        Task<Web> GetAsync(
            string key);

        Task<Web> UpdateAsync(
            Web web);

        Task<Web> UpdateThumbnailsIfDueAsync(
            Web web,
            bool forceUpdate = false);

        Task UpdateAllThumbnailsIfDueAsync(
            bool forceUpdate = false);
    }
}
