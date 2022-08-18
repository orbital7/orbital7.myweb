using Newtonsoft.Json;
using Orbital7.Extensions.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbital7.MyWeb.Models
{
    public abstract class WebObjectBase : IIdObject
    {
        [RequiredGuid]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Web Web { get; internal set; }

        public abstract WebObjectType Type { get; }

        public WebObjectBase()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
