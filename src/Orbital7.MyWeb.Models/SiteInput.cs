using Orbital7.Extensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbital7.MyWeb.Models
{
    public class SiteInput : WebObjectInputBase<Site>, ISite
    {
        [RequiredGuid]
        public Guid GroupId { get; set; }

        [Required]
        public string Url { get; set; } = "https://";

        [Display(Name = "Thumbnail Update Frequency")]
        public ThumbnailUpdateFrequency ThumbnailUpdateFrequency { get; set; }

        public SiteInput()
            : base()
        {

        }

        public SiteInput(
            string webKey,
            Guid groupId)
            : base(webKey)
        {
            this.GroupId = groupId;
        }

        public SiteInput(
            string webKey,
            Site site)
            : base(webKey, site)
        {
            this.GroupId = site.Group.Id;
            UpdateProperties(site, this);
        }

        protected override void UpdateProperties(
            Site site)
        {
            UpdateProperties(this, site);
        }

        private static void UpdateProperties(
            ISite source,
            ISite destination)
        {
            destination.Url = source.Url;
            destination.ThumbnailUpdateFrequency = source.ThumbnailUpdateFrequency;
        }
    }
}
