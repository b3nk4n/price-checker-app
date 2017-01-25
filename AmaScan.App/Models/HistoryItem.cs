using System;
using UWPCore.Framework.Mvvm;

namespace AmaScan.App.Models
{
    public class HistoryItem : BindableBase
    {
        public string Title { get; set; }

        public Uri Link { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public Uri Thumbnail { get; set; } // TODO: cache image offline, and only store the file path?
    }
}
