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
using Ninject;
using AmaScan.App.Services;
using System.Globalization;

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
                        BarcodeFormat.All_1D
                    }
                };

                var result = await scanner.Scan(options);
                scanner.AutoFocus();

                if (result != null)
                {
                    await Dispatcher.DispatchAsync(() =>
                    {
                        var parsed = ZXing.Client.Result.ResultParser.parseResult(result);

                        // delayed navigation to the product, since this is not running in the new page
                        LastScannedCode = parsed.DisplayResult;
                        Uri = AmazonUriTools.GetSearchUri(LastScannedCode);
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
                var title = AmazonHtmlTools.ExtractTextFromHtml(htmlContent);
                var imgUri = AmazonHtmlTools.ExtractImageUriFromHtml(htmlContent);

                if (title != string.Empty)
                {
                    var historyItem = new HistoryItem()
                    {
                        Link = args.Uri,
                        Timestamp = DateTimeOffset.Now,
                        Title = title,
                        Thumbnail = imgUri
                    };

                    // save to history
                    HistoryService.Items.Insert(0, historyItem);
                    await HistoryService.Save();
                }
            }
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            base.OnNavigatedTo(parameter, mode, state);

            var uri = AmazonUriTools.GetUri();

            if (parameter != null)
            {
                var paramData = parameter as string;
                if (paramData != null)
                {
                    if (paramData.StartsWith(AppConstants.NAV_LINK))
                    {
                        uri = new Uri(paramData.Replace(AppConstants.NAV_LINK, string.Empty), UriKind.Absolute);
                    }
                }
            }
            else if (LastScannedCode != null)
            {
                uri = AmazonUriTools.GetSearchUri(LastScannedCode);
            }

            // browse to the specified page
            Uri = uri;
        }

        public void RegisterWebView(WebView webViewer)
        {
            WebViewer = webViewer;
        }
    }
}
