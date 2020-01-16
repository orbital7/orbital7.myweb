using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Orbital7.MyWeb.Services.Default
{
    public static class DefaultServicesFactory
    {
        public static IServiceCollection CreateDefaultServiceCollection<TStartup>(
            string environmentName = "ASPNETCORE_ENVIRONMENT")
            where TStartup : class
        {
            var services = new ServiceCollection();
            services.AddMyWebDefaultServices();
            services.AddAppSettingsConfigurationWithUserSecrets<TStartup>(environmentName);
            return services;
        }

        public static IServiceCollection CreateDefaultServiceCollection(
            string environmentName = "ASPNETCORE_ENVIRONMENT")
        {
            var services = new ServiceCollection();
            services.AddMyWebDefaultServices();
            services.AddAppSettingsConfiguration(environmentName);
            return services;
        }

        public static IServiceProvider CreateDefault<TStartup>(
            string environmentName = "ASPNETCORE_ENVIRONMENT")
            where TStartup : class
        {
            return CreateDefaultServiceCollection<TStartup>(environmentName)
                .BuildServiceProvider();
        }

        public static IServiceProvider CreateDefault(
            string environmentName = "ASPNETCORE_ENVIRONMENT")
        {
            return CreateDefaultServiceCollection(environmentName)
                .BuildServiceProvider();
        }
    }
}
