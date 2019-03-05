using Newtonsoft.Json;
using Orbital7.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Orbital7.MyWeb.Models
{
    public class Site : WebObjectBase, ISite
    {
        [JsonIgnore]
        public Group Group { get; internal set; }

        [Required]
        public string Url { get; set; }

        [Display(Name = "Thumbnail Update Frequency")]
        public ThumbnailUpdateFrequency ThumbnailUpdateFrequency { get; set; }

        [Display(Name = "Thumbnail Last Updated")]
        public DateTime? ThumbnailLastUpdatedDateUtc { get; set; }

        [Display(Name = "Thumbnail Last Updated Successfully")]
        public bool ThumbnailLastUpdatedSuccess { get; set; }

        [Display(Name = "Thumbnail Update Error")]
        public string ThumbnailUpdateError { get; set; }

        [Display(Name = "Thumbnail Url")]
        public string ThumbnailUrl { get; set; }

        public string ThumbnailCachedUrl => this.ThumbnailUrl +
            (this.ThumbnailLastUpdatedDateUtc.HasValue ?
                "?updatedAt=" + this.ThumbnailLastUpdatedDateUtc.Value.FormatAsFileSystemSafeDateTime() :
                null);

        public string ThumbnailFilename => this.Id + MimeTypesHelper.FILE_EXT_PNG;

        public bool IsThumbnailUpdateDue => DetermineIsThumbnailUpdateDue();

        public override WebObjectType Type => WebObjectType.Site;

        public Site()
            : base()
        {
            
        }

        public Site(
            string url, 
            ThumbnailUpdateFrequency thumbnailUpdateFrequency = ThumbnailUpdateFrequency.Every4Weeks)
            : this()
        {
            this.Url = url;
            this.ThumbnailUpdateFrequency = thumbnailUpdateFrequency;
        }

        public override string ToString()
        {
            return this.Url;
        }

        private bool DetermineIsThumbnailUpdateDue()
        {
            if (this.ThumbnailLastUpdatedDateUtc.HasValue)
            {
                var sinceLastUpdated = DateTime.UtcNow.Subtract(this.ThumbnailLastUpdatedDateUtc.Value);
                return (this.ThumbnailUpdateFrequency == ThumbnailUpdateFrequency.Every4Weeks && sinceLastUpdated.Days >= 28) ||
                       (this.ThumbnailUpdateFrequency == ThumbnailUpdateFrequency.EveryWeek && sinceLastUpdated.Days >= 7) ||
                       (this.ThumbnailUpdateFrequency == ThumbnailUpdateFrequency.EveryDay && sinceLastUpdated.Days >= 1) ||
                       (this.ThumbnailUpdateFrequency == ThumbnailUpdateFrequency.Every4Hours && sinceLastUpdated.Hours >= 4) ||
                       (this.ThumbnailUpdateFrequency == ThumbnailUpdateFrequency.Every2Hours && sinceLastUpdated.Hours >= 2);
            }
            else
            {
                return true;
            }
        }
    }
}
