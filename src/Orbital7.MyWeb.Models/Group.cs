using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.MyWeb.Models
{
    public class Group : WebObjectBase
    {
        public List<Site> Sites { get; set; } = new List<Site>();

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
