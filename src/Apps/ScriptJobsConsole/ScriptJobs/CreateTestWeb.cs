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
    public class CreateTestWeb : ScriptJob
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
                                    new Site("Twitter", "https://twitter.com"),
                                    new Site("Instagram", "https://instagram.com"),
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
                                    new Site("CNN", "https://cnn.com"),
                                    new Site("ESPN NBA", "https://espn.com/nba"),
                                }
                            },
                            new Group("Technology")
                            {
                                Sites = new List<Site>()
                                {
                                    new Site("Engadget", "https://engadget.com"),
                                    new Site("Gizmodo", "https://gizmodo.com"),
                                    new Site("ArsTechnica", "https://arstechnica.com"),
                                }
                            },
                            new Group("Automotive")
                            {
                                Sites = new List<Site>()
                                {
                                    new Site("AutoBlog", "https://www.autoblog.com"),
                                    new Site("Jalopnik", "https://jalopnik.com"),
                                }
                            }
                        }
                    }
                }
            };

            string serializedWeb = web.Serialize();
            var deserializedWeb = Web.Load(serializedWeb);

            string serializedWeb2 = web.Serialize();
            var deserializedWeb2 = Web.Load(serializedWeb2);

            var webService = serviceProvider.GetRequiredService<IWebService>();
            await webService.UpdateAsync(web);

            var updatedWeb = await webService.GetAsync(SAMPLE_WEB_KEY);
        }
    }
}
