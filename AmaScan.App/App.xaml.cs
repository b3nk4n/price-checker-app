using System.Collections.Generic;
using System.Threading.Tasks;
using UWPCore.Framework.Common;
using UWPCore.Framework.Controls;
using UWPCore.Framework.IoC;
using UWPCore.Framework.Store;
using UWPCore.Framework.UI;
using AmaScan.App.Views;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using AmaScan.App.Module;
using AmaScan.App.Services;

namespace AmaScan.App
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : UniversalApp
    {
        /// <summary>
        /// Gets the default localizer.
        /// </summary>
        private Localizer Localizer { get; set; }

        /// <summary>
        /// Gets the license service.
        /// </summary>
        private ILicenseService LicenseService { get; set; }

        private IHistoryService HistoryService { get; set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
            : base(typeof(MainPage), AppBackButtonBehaviour.KeepAlive, true, new DefaultModule(), new AppModule())
        {
            // initialize Microsoft Application Insights
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);

            InitializeComponent();

            // inject services
            LicenseService = Injector.Get<ILicenseService>();
            HistoryService = Injector.Get<IHistoryService>();
        }

        public async override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await base.OnInitializeAsync(args);

            // create localizer here, because the Core Windows has to be initialized first
            Localizer = new Localizer();

            // setup theme colors (mainly for title bar)
            ColorPropertiesDark = new AppColorProperties(AppConstants.COLOR_ACCENT, Colors.White, AppConstants.COLOR_ACCENT, Colors.White, AppConstants.COLOR_ACCENT, Colors.White, AppConstants.COLOR_ACCENT);
            ColorPropertiesLight = new AppColorProperties(AppConstants.COLOR_ACCENT, Colors.White, AppConstants.COLOR_ACCENT, Colors.White, AppConstants.COLOR_ACCENT, Colors.White, AppConstants.COLOR_ACCENT);

#if DEBUG
            await LicenseService.RefeshSimulator();
#endif
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            await HistoryService.Load();

            var pageType = DefaultPage;
            object parameter = null;

            // start the user experience
            NavigationService.Navigate(pageType, parameter);
        }

        public async override Task OnSuspendingAsync(SuspendingEventArgs e)
        {
            await base.OnSuspendingAsync(e);
        }

        /// <summary>
        /// Gets the navigation menu items.
        /// </summary>
        /// <returns>The navigation menu items.</returns>
        protected override IEnumerable<NavMenuItem> CreateNavigationMenuItems()
        {
            return new[]
            {
                new NavMenuItem()
                {
                    Symbol = GlyphIcons.Search,
                    Label = Localizer.Get("Nav.Main"),
                    DestinationPage = typeof(MainPage)
                },
                new NavMenuItem()
                {
                    Symbol = GlyphIcons.History,
                    Label = Localizer.Get("Nav.History"),
                    DestinationPage = typeof(HistoryPage)
                }
            };
        }

        /// <summary>
        /// Gets the navigation menu items that are docked at the bottom.
        /// </summary>
        /// <returns>The navigation menu items.</returns>
        protected override IEnumerable<NavMenuItem> CreateBottomDockedNavigationMenuItems()
        {
            return new[]
            {
                new NavMenuItem()
                {
                    Symbol = GlyphIcons.Info,
                    Label = Localizer.Get("Nav.About"),
                    DestinationPage = typeof(AboutPage)
                },
                new NavMenuItem()
                {
                    Symbol = GlyphIcons.Setting,
                    Label = Localizer.Get("Nav.Settings"),
                    DestinationPage = typeof(SettingsPage)
                }
            };
        }
    }
}
