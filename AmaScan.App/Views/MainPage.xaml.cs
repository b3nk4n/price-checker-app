using UWPCore.Framework.Controls;
using AmaScan.App.ViewModels;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System;

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
        /// A retry BACK timer, since a back-navigation in WebView is sometimes not working
        /// </summary>
        /// <seealso cref="http://stackoverflow.com/questions/39850284/webview-goback-not-working-at-first-time"/>
        private DispatcherTimer _retryBackNavTimer = new DispatcherTimer();


        public MainPage()
        {
            InitializeComponent();

            ViewModel = Injector.Get<IMainViewModel>();
            ViewModel.RegisterWebView(WebViewer);
            DataContext = ViewModel;

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

        private void WebViewer_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            _retryBackNavTimer.Stop();
            var s = args.Uri;
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
