using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbital7.MyWeb.Models
{
    public enum ThumbnailUpdateFrequency
    {
        [Display(Name = "Every 4 Weeks")]
        Every4Weeks,

        [Display(Name = "Every Week")]
        EveryWeek,

        [Display(Name = "Every Day")]
        EveryDay,

        [Display(Name = "Every 4 Hours")]
        Every4Hours,

        [Display(Name = "Every 2 Hours")]
        Every2Hours
    }
}
