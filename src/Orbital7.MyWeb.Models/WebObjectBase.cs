using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.MyWeb.Models
{
    public abstract class WebObjectBase
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public Web Web { get; internal set; }

        public WebObjectBase()
        {
            this.Id = Guid.NewGuid();
        }

        public WebObjectBase(
            string name)
            : this()
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
