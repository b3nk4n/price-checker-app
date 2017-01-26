using AmaScan.App.Models;
using AmaScan.App.Services;
using Ninject;
using System.Collections.Generic;
using UWPCore.Framework.Launcher;
using UWPCore.Framework.Mvvm;

namespace AmaScan.App.ViewModels
{
    public class HistoryViewModel : ViewModelBase, IHistoryViewModel
    {
        public IHistoryService HistoryService { get; private set; }

        public ICommand RemoveCommand { get; private set; }
        public ICommand OpenInExternalBrowserCommand { get; private set; }

        [Inject]
        public HistoryViewModel(IHistoryService historyService)
        {
            HistoryService = historyService;

            RemoveCommand = new DelegateCommand<HistoryItem>((item) =>
            {
                HistoryService.Items.Remove(item);
                HistoryService.Save();
            });
            OpenInExternalBrowserCommand = new DelegateCommand<HistoryItem>(async (item) =>
            {
                await SystemLauncher.LaunchUriAsync(item.Link);
            });
        }

        /// <summary>
        /// Gets all history items.
        /// </summary>
        public IList<HistoryItem> Items
        {
            get { return HistoryService.Items; }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public HistoryItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                Set(ref _selectedItem, value);
            }
        }
        private HistoryItem _selectedItem;
    }
}
