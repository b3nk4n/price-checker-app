using System;
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
        public static IMainViewModel ViewModel { get; private set; }


        public MainPage()
        {
            InitializeComponent();

            ViewModel = Injector.Get<IMainViewModel>();
            DataContext = ViewModel;

            WebViewer.NavigationCompleted += async (s, e) =>
            {
                string htmlContent = await WebViewer.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });
                ViewModel.SaveHistory(htmlContent);
            };
        }
    }
}
