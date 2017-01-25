using System;
using UWPCore.Framework.Mvvm;

namespace AmaScan.App.Models
{
    public class HistoryItem : BindableBase
    {
        public string Title { get; set; }

        public Uri Link { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public string ThumbnailPath { get; set; } // try to cache the image locally in a folder. And simply save the path.
    }
}
