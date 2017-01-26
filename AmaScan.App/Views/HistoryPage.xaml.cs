using AmaScan.App.Models;
using AmaScan.App.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Universal.UI.Xaml.Controls;
using UWPCore.Framework.Common;
using UWPCore.Framework.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace AmaScan.App.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class HistoryPage : UniversalPage
    {
        /// <summary>
        /// The view model instance.
        /// </summary>
        public static IHistoryViewModel ViewModel { get; private set; }

        private Localizer _localizer = new Localizer();

        public HistoryPage()
        {
            InitializeComponent();

            ViewModel = Injector.Get<IHistoryViewModel>();
            DataContext = ViewModel;
        }

        private void SwipeListView_ItemSwipe(object sender, ItemSwipeEventArgs e)
        {
            var item = e.SwipedItem as HistoryItem;
            if (item != null)
            {
                if (e.Direction == SwipeListDirection.Right)
                {
                    ViewModel.RemoveCommand.Execute(item);
                }
            }
        }

        private void HistoryItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).Tag as HistoryItem;
            if (item != null)
            {
                NavigationService.Navigate(typeof(MainPage), string.Format("{0}{1}", AppConstants.NAV_LINK, item.Link.AbsoluteUri));
            }
        }

        #region Context menu

        private void HistoryItem_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if(e.PointerDeviceType != Windows.Devices.Input.PointerDeviceType.Mouse)
                return;

            var item = (sender as FrameworkElement).Tag as HistoryItem;
            var position = e.GetPosition(null);
            ShowContextMenu(item, null, position);
            e.Handled = true;
        }

        private void NoteListItem_Holding(object sender, HoldingRoutedEventArgs e)
        {
            if (e.PointerDeviceType != Windows.Devices.Input.PointerDeviceType.Touch ||
                e.HoldingState != Windows.UI.Input.HoldingState.Started)
                return;

            var item = (sender as FrameworkElement).Tag as HistoryItem;
            var position = e.GetPosition(null);
            ShowContextMenu(item, null, position);
            e.Handled = true;
        }

        private void ShowContextMenu(HistoryItem item, UIElement target, Point offset)
        {
            var contextMenu = CreateContextMenu(item);
            contextMenu.ShowAt(target, offset);
        }

        /// <summary>
        /// Creates the context menu.
        /// </summary>
        /// <param name="item">The associated history item.</param>
        /// <returns>The context menu popup.</returns>
        private MenuFlyout CreateContextMenu(HistoryItem item)
        {
            var style = (Style)Application.Current.Resources["MenuFlyoutItemIconTemplate"];
            var menu = new MenuFlyout();
            menu.Items.Add(new MenuFlyoutItem()
            {
                Text = _localizer.Get("OpenInExternalBrowser.Label"),
                Command = ViewModel.OpenInExternalBrowserCommand,
                CommandParameter = item,
                Tag = Application.Current.Resources["NewWindow"],
                Style = style
            });
            menu.Items.Add(new MenuFlyoutItem()
            {
                Text = _localizer.Get("Delete.Label"),
                Command = ViewModel.RemoveCommand,
                CommandParameter = item,
                Tag = Application.Current.Resources["Delete"],
                Style = style
            });
            return menu;
        }

        #endregion
    }
}
