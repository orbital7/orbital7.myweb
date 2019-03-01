using System;
using System.Collections.Generic;
using System.Linq;

namespace Orbital7.MyWeb.Models
{
    public class Category : WebObjectBase
    {
        public List<Group> Groups { get; set; } = new List<Group>();

        public override WebObjectType Type => WebObjectType.Category;

        public Category()
            : base()
        {
            
        }

        public Category(
            string name)
            : base(name)
        {

        }

        public List<Site> GatherAllSites()
        {
            return (from x in this.Groups
                    from y in x.Sites
                    select y).ToList();
        }
    }
}
