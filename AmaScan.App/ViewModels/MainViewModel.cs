using System;
using System.Collections.Generic;
using UWPCore.Framework.Common;
using UWPCore.Framework.Devices;
using UWPCore.Framework.Mvvm;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using ZXing.Mobile;

namespace AmaScan.App.ViewModels
{
    /// <summary>
    /// The view model of the main page.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private static MobileBarcodeScanner scanner;

        private IDeviceInfoService _deviceInfoService;

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
                scanner.CustomOverlay = CreateCustomOverlay(_deviceInfoService.IsWindows);
                var options = new MobileBarcodeScanningOptions()
                {
                    // set auto rotate to false, becuase it was causing some crashed + (green) camera-deadlock
                    AutoRotate = false,
                };

                // LOCK screen rotation
                var currentOrientation = Windows.Graphics.Display.DisplayInformation.GetForCurrentView().CurrentOrientation;
                Windows.Graphics.Display.DisplayInformation.AutoRotationPreferences = currentOrientation;

                var result = await scanner.Scan(options);

                if (result != null)
                {
                    await Dispatcher.DispatchAsync(() =>
                    {
                        var parsed = ZXing.Client.Result.ResultParser.parseResult(result);

                    });
                }
            },
            () =>
            {
                return true;
            });
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            base.OnNavigatedTo(parameter, mode, state);

            Uri = new Uri("http://www.amazon.de/", UriKind.Absolute);
        }

        private UIElement CreateCustomOverlay(bool showCancelButton)
        {
            var root = new Grid();
            var rectBottom = new Rectangle()
            {
                Fill = new SolidColorBrush(Colors.Black),
                Height = 48,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom,
                Opacity = 0.2,
            };
            root.Children.Add(rectBottom);

            var rectTop = new Rectangle()
            {
                Fill = new SolidColorBrush(Colors.Black),
                Height = 48,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top,
                Opacity = 0.2,
            };
            root.Children.Add(rectTop);

            var viewBox = new Viewbox()
            {
                Margin = new Thickness(64)
            };
            var crosshairGrid = new Grid()
            {
                Width = 240,
                Height = 240,
                Opacity = 0.2,
            };
            crosshairGrid.Children.Add(new Border()
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(2, 2, 0, 0),
                Width = 64,
                Height = 64,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            });
            crosshairGrid.Children.Add(new Border()
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(0, 2, 2, 0),
                Width = 64,
                Height = 64,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right
            });
            crosshairGrid.Children.Add(new Border()
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(0, 0, 2, 2),
                Width = 64,
                Height = 64,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right
            });
            crosshairGrid.Children.Add(new Border()
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(2, 0, 0, 2),
                Width = 64,
                Height = 64,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left
            });
            crosshairGrid.Children.Add(new Rectangle()
            {
                Fill = new SolidColorBrush(Colors.Black),
                Width = 128,
                Height = 2,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            });

            viewBox.Child = crosshairGrid;
            root.Children.Add(viewBox);

            if (showCancelButton)
            {
                var button = new Button()
                {
                    Style = (Style)Application.Current.Resources["IconButtonStyle"],
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    Content = ((char)GlyphIcons.Close).ToString()
                };
                button.Click += (s, e) => {
                    //_scanner.Cancel(); causes a crash, use navigation service instead
                    NavigationService.GoBack();
                };
                root.Children.Add(button);
            }

            return root;
        }
    }
}
