using UWPCore.Framework.Controls;
using AmaScan.App.ViewModels;
using Windows.UI.Xaml.Navigation;

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
        }

    }
}
