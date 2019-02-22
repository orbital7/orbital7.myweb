using System;

namespace Orbital7.MyWeb.Models
{
    public class Site : WebObjectBase
    {
        public string Url { get; set; }

        public ThumbnailUpdateFrequency ThumbnailUpdateFrequency { get; set; }

        public DateTime? ThumbnailLastUpdatedDateUtc { get; set; }

        public bool ThumbnailLastUpdatedSuccess { get; set; }

        public string ThumbnailUrl { get; set; }

        public string ThumbnailFilename => this.Id + ".png";

        public bool IsThumbnailUpdateDue => DetermineIsThumbnailUpdateDue();

        public Site()
            : base()
        {
            
        }

        public Site(
            string name)
            : base(name)
        {

        }

        public Site(
            string name, 
            string url, 
            ThumbnailUpdateFrequency thumbnailUpdateFrequency = ThumbnailUpdateFrequency.Every4Weeks)
            : this(name)
        {
            this.Url = url;
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
