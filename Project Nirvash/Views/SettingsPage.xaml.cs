using Microsoft.AppCenter.Analytics;
using Microsoft.Services.Store.Engagement;
using Project_Nirvash.Core.Helpers;
using Project_Nirvash.Dialogs;
using Project_Nirvash.Helpers;
using Project_Nirvash.Services;
using System;
using System.Collections.Generic;
using Windows.Devices.Enumeration;
using Windows.Devices.Power;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Project_Nirvash.Views
{
    /// <summary>
    /// SettingsPage class
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        /// <summary>
        /// Parameterless constructor of SettingsPage class.
        /// </summary>
        public SettingsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method which occurs when the page is laid out, rendered, and ready for interaction.
        /// Loads app settings state and checks if Windows Hello is available in the system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UseWindowsHelloCheckBox.IsChecked = ApplicationData.Current.LocalSettings.Values.ContainsKey("HelloAuthenticationEnabled");

            if (!await KeyCredentialManager.IsSupportedAsync())
            {
                UseWindowsHelloCheckBox.IsEnabled = false;
                WindowsHelloStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                WindowsHelloStackPanel.Visibility = Visibility.Collapsed;
            }

            DeviceInformationCollection batteryCollection = await DeviceInformation.FindAllAsync(Battery.GetDeviceSelector());
            BatterySaverStackPanel.Visibility = batteryCollection.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            SendDiagnosticDataCheckBox.IsChecked = !ApplicationData.Current.LocalSettings.Values.ContainsKey("AnalyticsDisabled");
        }

        /// <summary>
        /// Method invoked when the user clicks on Send a feedback.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e) => await StoreServicesFeedbackLauncher.GetDefault().LaunchAsync();

        /// <summary>
        /// Method invoked when the user clicks on About this app.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void HyperlinkButton_Click_1(object sender, RoutedEventArgs e) => await new AboutDialog().ShowAsync();

        /// <summary>
        /// Method invoked when the user clicks on Change user.
        /// Unregister the background task and removes all user data in order to allow
        /// another user to use the app.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                // TODO: Fix this monstrosity
                Singleton<ClientExtensions>.Instance.DisconnectUser();

                SecurityExtensions.RemoveCredentials();

                NavigationService.Navigate(typeof(LockedPage));

                ShellPage.Current.Loader.IsLoading = true;
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent(ex.Message, new Dictionary<string, string> { { "exception", ex.ToString() } });
            }
        }

        #region Windows Hello

        /// <summary>
        /// Method invoked when the user clicks on Use Windows Hello.
        /// Checks user's trustworthiness and enable Windows Hello.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (UseWindowsHelloCheckBox.IsChecked == true)
            {
                if (await SecurityExtensions.Authenticate())
                {
                    UseWindowsHelloCheckBox.IsChecked = true;

                    SecurityExtensions.RegisterKey();
                }
                else
                {
                    UseWindowsHelloCheckBox.IsChecked = false;

                    SecurityExtensions.RemoveKey();
                }
            }
            else
            {
                SecurityExtensions.RemoveKey();
            }
        }

        /// <summary>
        /// Method invoked when the user clicks on Open Windows Settings.
        /// Opens the Settings page for managing sign-in options.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:signinoptions"));
        }

        #endregion

        #region Notifications and live tile

        /// <summary>
        /// Method called when the user clicks on Add Exception.
        /// Opens the Settings page for battery save mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click_1(object sender, RoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri("ms-settings:batterysaver"));

        /// <summary>
        /// Method invoked when the user clicks on Manage background apps.
        /// Opens the Settings page for managing background apps.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click_2(object sender, RoutedEventArgs e) => await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-backgroundapps"));

        #endregion

        #region Privacy and Diagnostic Data

        /// <summary>
        /// Method called when the user clicks on Send diagnostic data.
        /// Enables or disables AppCenter Analytics.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Click_1(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("AnalyticsDisabled"))
            {
                ApplicationData.Current.LocalSettings.Values.Remove("AnalyticsDisabled");
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values.Add("AnalyticsDisabled", true);
            }
        }

        #endregion
    }
}
