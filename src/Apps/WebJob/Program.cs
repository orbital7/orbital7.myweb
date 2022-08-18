using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orbital7.Extensions;
using Orbital7.MyWeb.Services;
using Orbital7.MyWeb.Services.Default;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            var now = DateTime.Now;

            if (now.Hour >= 7 && now.Hour <= 21)
            {
                var serviceProvider = DefaultServicesFactory.CreateDefault();
                AsyncHelper.RunSync(() => serviceProvider.GetRequiredService<IWebService>()
                    .UpdateAllThumbnailsIfDueAsync());

                Thread.Sleep(1000 * 60 * 5);
            }
        }
    }
}
