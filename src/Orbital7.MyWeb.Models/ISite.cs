using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.MyWeb.Models
{
    public interface ISite
    {
        string Url { get; set; }

        ThumbnailUpdateFrequency ThumbnailUpdateFrequency { get; set; }
    }
}
