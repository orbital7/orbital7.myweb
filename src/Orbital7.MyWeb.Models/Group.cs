using Newtonsoft.Json;
using Orbital7.Extensions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.MyWeb.Models
{
    public class Group : WebObjectBase
    {
        [JsonIgnore]
        public Category Category { get; internal set; }

        public List<Site> Sites { get; set; } = new List<Site>();

        public override WebObjectType Type => WebObjectType.Group;

        public Group()
            : base()
        {
            
        }

        public Group(
            string name)
            : base(name)
        {

        }
    }
}
