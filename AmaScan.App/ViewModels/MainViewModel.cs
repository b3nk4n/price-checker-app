using AmaScan.App.UI;
using System;
using System.Collections.Generic;
using UWPCore.Framework.Devices;
using UWPCore.Framework.Mvvm;
using Windows.UI.Xaml.Navigation;
using ZXing;
using ZXing.Mobile;
using Windows.UI.Xaml.Controls;
using AmaScan.Common.Tools;

namespace AmaScan.App.ViewModels
{
    /// <summary>
    /// The view model of the main page.
    /// </summary>
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private static MobileBarcodeScanner scanner;

        private IDeviceInfoService _deviceInfoService;

        public static string LastScannedCode { get; private set; }

        public string BaseUri { get; private set; } = "https://www.amazon.de/";

        public string SearchPath { get; private set; } = "s/field-keywords=";

        public WebView WebViewer { get; private set; }

        /// <summary>
        /// Gets or sets the URI page.
        /// </summary>
        public Uri Uri { get { return _uri; } set { Set(ref _uri, value); } }
        private Uri _uri;

        /// <summary>
        /// Gets or sets the scan command.
        /// </summary>
        public ICommand ScanCommand { get; private set; }

        public MainViewModel()
        {
            _deviceInfoService = Injector.Get<IDeviceInfoService>();

            ScanCommand = new DelegateCommand(async () =>
            {
                scanner = new MobileBarcodeScanner(Dispatcher.CoreDispatcher);
                scanner.UseCustomOverlay = true;
                scanner.CustomOverlay = ZXingOverlay.CreateCustomOverlay(_deviceInfoService.IsWindows, () =>
                {
                    NavigationService.GoBack();
                });
                var options = new MobileBarcodeScanningOptions()
                {
                    // set auto rotate to false, becuase it was causing some crashed + (green) camera-deadlock
                    AutoRotate = false,
                    PossibleFormats = new List<BarcodeFormat>()
                    {
                        BarcodeFormat.EAN_8,
                        BarcodeFormat.EAN_13,
                    }
                };

                // LOCK screen rotation
                var currentOrientation = Windows.Graphics.Display.DisplayInformation.GetForCurrentView().CurrentOrientation;
                Windows.Graphics.Display.DisplayInformation.AutoRotationPreferences = currentOrientation;

                var result = await scanner.Scan(options);
                scanner.AutoFocus();

                if (result != null)
                {
                    await Dispatcher.DispatchAsync(() =>
                    {
                        var parsed = ZXing.Client.Result.ResultParser.parseResult(result);

                        // delayed navigation to the product, since this is not running in the new page
                        LastScannedCode = parsed.DisplayResult;
                        Uri = new Uri(string.Format("{0}{1}{2}", BaseUri, SearchPath, LastScannedCode), UriKind.Absolute);
                        WebViewer.NavigationCompleted += WebViewer_NavigationCompleted;
                    });
                }
            },
            () =>
            {
                return true;
            });
        }

        private async void WebViewer_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            WebViewer.NavigationCompleted -= WebViewer_NavigationCompleted;

            if (args.IsSuccess)
            {
                string htmlContent = await WebViewer.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });
                string title = HtmlTools.ExtractTextFromHtml(htmlContent, "productTitle"); 

                //var historyItem = new HistoryItem()
                //{
                //    Link = args.Uri,
                //    Timestamp = DateTimeOffset.Now,
                //};

                // save to history
            }
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            base.OnNavigatedTo(parameter, mode, state);

            if (LastScannedCode != null)
            {
                Uri = new Uri(string.Format("{0}{1}{2}", BaseUri, SearchPath, LastScannedCode), UriKind.Absolute);
            }
            else
            {
                Uri = new Uri(BaseUri, UriKind.Absolute);
            }
        }

        public void RegisterWebView(WebView webViewer)
        {
            WebViewer = webViewer;
        }
    }
}
