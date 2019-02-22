using Microsoft.Extensions.Configuration;
using Orbital7.MyWeb.Services;
using Orbital7.MyWeb.Services.Default;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddMyWebDefaultServices(
            this IServiceCollection services)
        {
            services.AddScoped<ISiteService, SiteService>();
            services.AddScoped<IWebService, WebService>();

            return services;
        }
        
    }
}
