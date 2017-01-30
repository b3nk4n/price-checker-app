﻿using AmaScan.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UWPCore.Framework.Common;
using UWPCore.Framework.Controls;
using UWPCore.Framework.Data;
using UWPCore.Framework.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace AmaScan.App.ViewModels
{
    /// <summary>
    /// The view model of the settings page.
    /// </summary>
    public class SettingsViewModel : ViewModelBase
    {
        /// <summary>
        /// The theme source for data binding to an enum.
        /// </summary>
        public EnumSource<ElementTheme> ThemeEnumSource { get; private set; } = new EnumSource<ElementTheme>();

        public EnumSource<AmazonRegion> RegionEnumSource { get; private set; } = new EnumSource<AmazonRegion>();

        public SettingsViewModel()
        {
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            base.OnNavigatedTo(parameter, mode, state);

            // setup selections
            ThemeEnumSource.SelectedValue = UniversalPage.PageTheme.Value;
            RegionEnumSource.SelectedValue = AppSettings.Region.Value;
        }

        public async override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            await base.OnNavigatedFromAsync(state, suspending);

            // save settings
            AppSettings.Region.Value = RegionEnumSource.SelectedValue;

            // apply theme changes
            UniversalPage.PageTheme.Value = ThemeEnumSource.SelectedValue;
            UniversalApp.Current.UpdateTheme();
        }
    }
}
