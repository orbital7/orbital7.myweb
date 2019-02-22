using Orbital7.MyWeb.Services.Default;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = DefaultServicesFactory.CreateDefault();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var temp = configuration["StorageConnectionString"];
            Console.WriteLine("Test: " + temp);
        }
    }
}
