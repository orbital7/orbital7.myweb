using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orbital7.Extensions.ScriptJobs;
using Orbital7.MyWeb.Models;
using Orbital7.MyWeb.Services;
using Orbital7.MyWeb.Services.Default;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScriptJobsConsole.ScriptJobs
{
    public class CreateTestWeb : ScriptJobBase
    {
        private const string SAMPLE_WEB_KEY = "sampleweb";

        public override async Task ExecuteAsync()
        {
            var serviceProvider = DefaultServicesFactory.CreateDefault<Program>();

            var web = new Web(SAMPLE_WEB_KEY)
            {
                Categories = new List<Category>()
                {
                    new Category("Me")
                    {
                        Groups = new List<Group>()
                        {
                            new Group("Social Media")
                            {
                                Sites = new List<Site>()
                                {
                                    new Site("https://twitter.com"),
                                    new Site("https://instagram.com"),
                                }
                            }
                        }
                    },
                    new Category("News")
                    {
                        Groups = new List<Group>()
                        {
                            new Group("World")
                            {
                                Sites = new List<Site>()
                                {
                                    new Site("https://www.cnn.com"),
                                    new Site("https://espn.com/nba"),
                                }
                            },
                            new Group("Technology")
                            {
                                Sites = new List<Site>()
                                {
                                    new Site("https://engadget.com"),
                                    new Site("https://gizmodo.com"),
                                    new Site("https://arstechnica.com"),
                                }
                            },
                            new Group("Automotive")
                            {
                                Sites = new List<Site>()
                                {
                                    new Site("https://www.autoblog.com"),
                                    new Site("https://jalopnik.com"),
                                }
                            }
                        }
                    }
                }
            };

            var webService = serviceProvider.GetRequiredService<IWebService>();
            await webService.UpdateAsync(web);
            await webService.UpdateAllThumbnailsIfDueAsync();
        }
    }
}
