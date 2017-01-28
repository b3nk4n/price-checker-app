using System;
using UWPCore.Framework.Mvvm;

namespace AmaScan.App.Models
{
    public class HistoryItem : BindableBase
    {
        public string Title { get; set; }

        public Uri Link { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public Uri Thumbnail { get; set; } // TODO: cache image offline, and only store the file path?1

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            HistoryItem o = (HistoryItem)obj;
            // sufficient here to check for the same title and link
            return Title == o.Title && Link.OriginalString == o.Link.OriginalString;
        }
    }
}
