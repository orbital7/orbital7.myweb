using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Home
{
    public class _DialogSetWebModel
    {
        [Required]
        [Display(Name = "Web Key")]
        public string WebKey { get; set; }

        public static _DialogSetWebModel Create(
            string webKey)
        {
            return new _DialogSetWebModel()
            {
                WebKey = webKey,
            };
        }
    }
}
