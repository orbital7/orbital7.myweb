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

            var builder = CreateDefaultBuilder(environmentName)
                .AddUserSecrets<TStartup>()
                .AddEnvironmentVariables();
            services.AddSingleton<IConfiguration>(builder.Build());

            return services;
        }

        public static IServiceCollection CreateDefaultServiceCollection(
            string environmentName = "ASPNETCORE_ENVIRONMENT")
        {
            var services = new ServiceCollection();
            services.AddMyWebDefaultServices();

            var builder = CreateDefaultBuilder(environmentName)
                .AddEnvironmentVariables();
            services.AddSingleton<IConfiguration>(builder.Build());

            return services;
        }

        private static IConfigurationBuilder CreateDefaultBuilder(
            string environmentName)
        {
            var environment = Environment.GetEnvironmentVariable(environmentName);
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true);

            return builder;
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
