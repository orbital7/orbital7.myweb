using Microsoft.Extensions.DependencyInjection;
using Orbital7.Extensions.ScriptJobs;
using Orbital7.MyWeb.Services;
using Orbital7.MyWeb.Services.Default;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScriptJobsConsole.ScriptJobs
{
    public class UpdateThumbnails : ScriptJobBase
    {
        public override async Task ExecuteAsync()
        {
            var serviceProvider = DefaultServicesFactory.CreateDefault<Program>();
            await serviceProvider.GetRequiredService<IWebService>()
                .UpdateAllThumbnailsIfDueAsync(forceUpdate: true);
        }
    }
}
