using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Orbital7.MyWeb.Models
{
    public class Web
    {
        public string Key { get; set; }

        public List<Category> Categories { get; set; }
            = new List<Category>();

        public Web()
        {

        }

        public Web(
            string key)
            : this()
        {
            this.Key = key;
        }

        public static Web Load(
            string serializedJson)
        {
            var web = JsonConvert.DeserializeObject<Web>(serializedJson);

            foreach (var category in web.Categories)
            {
                category.Web = web;
                foreach (var group in category.Groups)
                {
                    group.Web = web;
                    foreach (var site in group.Sites)
                        site.Web = web;
                }
            }

            return web;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public Category GetCategory(
            Guid categoryId)
        {
            return (from x in this.Categories
                    where x.Id == categoryId
                    select x).FirstOrDefault();
        }

        public Group GetGroup(
            Guid groupId)
        {
            return (from x in this.Categories
                    from y in x.Groups
                    where y.Id == groupId
                    select y).FirstOrDefault();
        }

        public List<Group> GatherAllGroups(
            Guid groupId)
        {
            return (from x in this.Categories
                    from y in x.Groups
                    select y).ToList();
        }

        public Site GetSite(
            Guid siteId)
        {
            return (from x in this.Categories
                    from y in x.Groups
                    from z in y.Sites
                    where z.Id == siteId
                    select z).FirstOrDefault();
        }

        public List<Site> GatherAllSites()
        {
            return (from x in this.Categories
                    from y in x.Groups
                    from z in y.Sites
                    select z).ToList();
        }
    }
}
