using System;
using AmaScan.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmaScan.App.Services
{
    /// <summary>
    /// Service to manage a history of items.
    /// </summary>
    public interface IHistoryService
    {
        /// <summary>
        /// The items of the history.
        /// </summary>
        IList<HistoryItem> Items { get; }

        /// <summary>
        /// Loads the history from disk.
        /// </summary>
        Task Load();

        /// <summary>
        /// Save the history to disk.
        /// </summary>
        Task Save();
    }
}