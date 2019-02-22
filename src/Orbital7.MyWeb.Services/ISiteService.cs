using Orbital7.MyWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.MyWeb.Services
{
    public interface ISiteService
    {
        Task<Site> UpdateThumbnailIfDueAsync(
            Site site,
            bool forceUpdate = false);

        Task<Site> UpdateThumbnailNowAsync(
            Site site);
    }
}
