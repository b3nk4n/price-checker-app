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
using AmaScan.App.Models;
using UWPCore.Framework.Storage;
using Ninject;
using AmaScan.App.Services;

namespace AmaScan.App.ViewModels
{
    /// <summary>
    /// The view model of the main page.
    /// </summary>
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private static MobileBarcodeScanner scanner;

        public IDeviceInfoService DeviceInfoService { get; private set; }

        public IHistoryService HistoryService {get; private set; }

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

        [Inject]
        public MainViewModel(IDeviceInfoService deviceInfoService, IHistoryService historyService)
        {
            DeviceInfoService = deviceInfoService;
            HistoryService = historyService;

            ScanCommand = new DelegateCommand(async () =>
            {
                scanner = new MobileBarcodeScanner(Dispatcher.CoreDispatcher);
                scanner.UseCustomOverlay = true;
                scanner.CustomOverlay = ZXingOverlay.CreateCustomOverlay(DeviceInfoService.IsWindows, () =>
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
                var htmlContent = await WebViewer.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });
                var title = AmazonHtmlTools.ExtractTextFromHtml(htmlContent, "productTitle");
                var imgUri = AmazonHtmlTools.ExtractImageUriFromHtml(htmlContent);

                var historyItem = new HistoryItem()
                {
                    Link = args.Uri,
                    Timestamp = DateTimeOffset.Now,
                    Title = title,
                    Thumbnail = imgUri
                };

                // save to history
                HistoryService.Items.Add(historyItem);
                await HistoryService.Save();
            }
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            base.OnNavigatedTo(parameter, mode, state);

            string uri = BaseUri;

            if (parameter != null)
            {
                var paramData = parameter as string;
                if (paramData != null)
                {
                    if (paramData.StartsWith(AppConstants.NAV_LINK))
                    {
                        uri = paramData.Replace(AppConstants.NAV_LINK, string.Empty);
                    }
                }
            }
            else if (LastScannedCode != null)
            {
                uri = string.Format("{0}{1}{2}", BaseUri, SearchPath, LastScannedCode);
            }

            // browse to the specified page
            Uri = new Uri(uri, UriKind.Absolute);
        }

        public void RegisterWebView(WebView webViewer)
        {
            WebViewer = webViewer;
        }
    }
}
