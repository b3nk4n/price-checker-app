using UWPCore.Framework.Controls;
using AmaScan.App.ViewModels;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System;
using UWPCore.Framework.Launcher;
using UWPCore.Framework.UI;
using Windows.UI.Popups;
using AmaScan.Common.Tools;
using UWPCore.Framework.Common;

namespace AmaScan.App.Views
{
    /// <summary>
    /// The apps main page.
    /// </summary>
    public partial class MainPage : UniversalPage
    {
        /// <summary>
        /// The view model instance.
        /// </summary>
        public static IMainViewModel ViewModel { get; private set; }

        /// <summary>
        /// Gets or sets the dialog service.
        /// </summary>
        public IDialogService DialogService { get; private set; }

        public Localizer Localizer { get; private set; } = new Localizer();

        /// <summary>
        /// A retry BACK timer, since a back-navigation in WebView is sometimes not working
        /// </summary>
        /// <seealso cref="http://stackoverflow.com/questions/39850284/webview-goback-not-working-at-first-time"/>
        private DispatcherTimer _retryBackNavTimer = new DispatcherTimer();

        public Uri CurrentWebViewUri { get; private set; } = new Uri(AmazonUriTools.GetBase(), UriKind.Absolute);

        public MainPage()
        {
            InitializeComponent();

            ViewModel = Injector.Get<IMainViewModel>();
            ViewModel.RegisterWebView(WebViewer);
            DataContext = ViewModel;

            DialogService = Injector.Get<IDialogService>();

            Loaded += (s, e) =>
            {
                ButtonPloppInAnimation.Begin();
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            WebViewer.NavigationCompleted += WebViewer_NavigationCompleted;
            WebViewer.NavigationStarting += WebViewer_NavigationStarting;
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;

            _retryBackNavTimer.Interval = TimeSpan.FromSeconds(0.25);
            _retryBackNavTimer.Tick += _retryBackNavTimer_Tick;
        }

        private void _retryBackNavTimer_Tick(object sender, object e)
        {
            if (WebViewer.CanGoBack)
            {
                WebViewer.GoBack();
                _retryBackNavTimer.Stop();
            }
        }

        private async void WebViewer_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            _retryBackNavTimer.Stop();

            //if (args.Uri.AbsolutePath.StartsWith("/ap/register") ||
            //    args.Uri.AbsolutePath.StartsWith("/ap/signin") ||
            //    args.Uri.AbsolutePath.Contains("/redirector.html/ref=sign-in-redirect") ||
            //    args.Uri.AbsolutePath.StartsWith("/gp/prime") ||
            //    args.Uri.AbsolutePath.StartsWith("/gp/video") ||
            //    args.Uri.AbsolutePath.StartsWith("/gp/registry") ||
            //    args.Uri.AbsolutePath.StartsWith("/gp/cobrandcard/") ||
            //    args.Uri.AbsolutePath.StartsWith("/gp/cart/") ||
            //    args.Uri.AbsolutePath.StartsWith("/gp/gc/") ||
            if (args.Uri.AbsolutePath.StartsWith("/ap/") ||
                args.Uri.AbsolutePath.StartsWith("/gp/") ||
                args.Uri.AbsolutePath.Contains("/redirector.html/ref=sign-in-redirect") ||
                args.Uri.Host == "sellercentral.amazon.de")
            {
                args.Cancel = true;

                await DialogService.ShowAsync(Localizer.Get("Dialog.ExternalBrowser"), Localizer.Get("Dialog.Information"), 0, 1, new UICommand(Localizer.Get("Dialog.OK"), async (a) => {
                    await SystemLauncher.LaunchUriAsync(CurrentWebViewUri);
                }), new UICommand(Localizer.Get("Dialog.Cancel"), (a) => {
                    // NOP
                }));

                return;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            WebViewer.NavigationCompleted -= WebViewer_NavigationCompleted;
            WebViewer.NavigationStarting -= WebViewer_NavigationStarting;
            SystemNavigationManager.GetForCurrentView().BackRequested -= MainPage_BackRequested;

            _retryBackNavTimer.Tick -= _retryBackNavTimer_Tick;
        }

        private void WebViewer_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.IsSuccess)
            {
                Progress.IsActive = false;
                CurrentWebViewUri = args.Uri;
            }
        }

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (WebViewer.CanGoBack)
            {
                e.Handled = true;
                WebViewer.GoBack();
                _retryBackNavTimer.Start();
            }
        }
    }
}
