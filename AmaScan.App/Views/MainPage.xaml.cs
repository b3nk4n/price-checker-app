using UWPCore.Framework.Controls;
using AmaScan.App.ViewModels;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

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

            WebViewer.NavigationCompleted += (s, e) =>
            {
                if (e.IsSuccess)
                {
                    Progress.IsActive = false;
                }
            };

            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) =>
            {
                if (WebViewer.CanGoBack)
                {
                    e.Handled = true;
                    WebViewer.GoBack();
                }
            };
        }

    }
}
