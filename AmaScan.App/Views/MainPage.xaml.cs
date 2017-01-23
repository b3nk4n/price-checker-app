using UWPCore.Framework.Controls;
using AmaScan.App.ViewModels;

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
        public MainViewModel ViewModel { get; private set; }


        public MainPage()
        {
            InitializeComponent();

            ViewModel = new MainViewModel();
            DataContext = ViewModel;
        }
    }
}
