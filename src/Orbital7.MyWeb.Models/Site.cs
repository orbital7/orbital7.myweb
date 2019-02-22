using System;

namespace Orbital7.MyWeb.Models
{
    public class Site : WebObjectBase
    {
        public string Url { get; set; }

        public string ThumbnailFilename => this.Id + ".png";

        public Site()
            : base()
        {
            
        }

        public Site(string name)
            : base(name)
        {

        }

        public Site(string name, string url)
            : this(name)
        {
            this.Url = url;
        }
    }
}
