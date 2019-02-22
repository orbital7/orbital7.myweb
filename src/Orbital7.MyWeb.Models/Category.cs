using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.MyWeb.Models
{
    public class Category : WebObjectBase
    {
        public List<Group> Groups { get; set; } = new List<Group>();

        public Category()
            : base()
        {
            
        }

        public Category(string name)
            : base(name)
        {

        }
    }
}
