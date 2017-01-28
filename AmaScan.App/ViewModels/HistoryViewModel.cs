using AmaScan.App.Models;
using AmaScan.App.Services;
using Ninject;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UWPCore.Framework.Launcher;
using UWPCore.Framework.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace AmaScan.App.ViewModels
{
    public class HistoryViewModel : ViewModelBase, IHistoryViewModel
    {
        public IHistoryService HistoryService { get; private set; }

        public ICommand RemoveCommand { get; private set; }
        public ICommand OpenInExternalBrowserCommand { get; private set; }

        public ObservableCollection<HistoryItem> Items { get; private set; }

        [Inject]
        public HistoryViewModel(IHistoryService historyService)
        {
            HistoryService = historyService;

            RemoveCommand = new DelegateCommand<HistoryItem>((item) =>
            {
                Items.Remove(item);
                HistoryService.Items.Remove(item);
                HistoryService.Save();
            });
            OpenInExternalBrowserCommand = new DelegateCommand<HistoryItem>(async (item) =>
            {
                await SystemLauncher.LaunchUriAsync(item.Link);
            });
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            base.OnNavigatedTo(parameter, mode, state);

            // load items
            Items = new ObservableCollection<HistoryItem>(HistoryService.Items);
            RaisePropertyChanged("Items");
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
