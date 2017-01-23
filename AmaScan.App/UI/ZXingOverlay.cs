using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPCore.Framework.Common;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace AmaScan.App.UI
{
    public static class ZXingOverlay
    {
        public static UIElement CreateCustomOverlay(bool showCancelButton, Action closeClick)
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
                    closeClick();
                };
                root.Children.Add(button);
            }

            return root;
        }
    }
}
