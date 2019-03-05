using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Orbital7.MyWeb.Models
{
    public class Category : WebObjectBase, ICategory
    {
        [Required]
        public string Name { get; set; }

        public List<Group> Groups { get; set; } = new List<Group>();

        public override WebObjectType Type => WebObjectType.Category;

        public Category()
            : base()
        {
            
        }

        public Category(
            string name)
            : this()
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public List<Site> GatherAllSites()
        {
            return (from x in this.Groups
                    from y in x.Sites
                    select y).ToList();
        }
    }
}
