using Newtonsoft.Json;
using Orbital7.Extensions;
using System;

namespace Orbital7.MyWeb.Models
{
    public class Site : WebObjectBase
    {
        [JsonIgnore]
        public Group Group { get; internal set; }

        public ThumbnailUpdateFrequency ThumbnailUpdateFrequency { get; set; }

        public DateTime? ThumbnailLastUpdatedDateUtc { get; set; }

        public bool ThumbnailLastUpdatedSuccess { get; set; }

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
            string url)
            : base(url)
        {

        }

        public Site(
            string url, 
            ThumbnailUpdateFrequency thumbnailUpdateFrequency = ThumbnailUpdateFrequency.Every4Weeks)
            : this(url)
        {
            this.ThumbnailUpdateFrequency = thumbnailUpdateFrequency;
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
