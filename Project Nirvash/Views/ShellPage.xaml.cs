﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Microsoft.Services.Store.Engagement;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using Project_Nirvash.Dialogs;
using Project_Nirvash.Helpers;
using Project_Nirvash.Services;
using Windows.ApplicationModel.Core;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using WinUI = Microsoft.UI.Xaml.Controls;
using Project_Nirvash.Models;
using Project_Nirvash.Exceptions;
using Project_Nirvash.Core.Helpers;
using System.Net.Http;
using AdDealsUniversalSDKW81;
using AdDealsUniversalSDKW81.Views.UserControls;

namespace Project_Nirvash.Views
{
    public sealed partial class ShellPage : Page, INotifyPropertyChanged
    {
        private readonly KeyboardAccelerator _altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);
        private readonly KeyboardAccelerator _backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

        private bool _isBackEnabled;
        private WinUI.NavigationViewItem _selected;

        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { Set(ref _isBackEnabled, value); }
        }

        public WinUI.NavigationViewItem Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ShellPage()
        {
            InitializeComponent();
            DataContext = this;
            Initialize();
        }

        private void Initialize()
        {
            NavigationService.Frame = shellFrame;
            NavigationService.NavigationFailed += Frame_NavigationFailed;
            NavigationService.Navigated += Frame_Navigated;
            navigationView.BackRequested += OnBackRequested;

            // Start Project Nirvash
            ApplicationView view = ApplicationView.GetForCurrentView();
            view.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            navigationView.ItemInvoked += OnItemInvoked;

            AppNotification = AppNotificationTeachingTip;

            Loader = LoadingControl;

            Loader.IsLoading = true;

            AdManager.InitSDK(this.LayoutRoot, "3598", "8C2NWVZ32JIX");
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("AnalyticsDisabled"))
            {
                AdManager.SetConsent(AdManager.PrivacyPolicyConsent.GRANT);
            }
            else
            {
                AdManager.SetConsent(AdManager.PrivacyPolicyConsent.REVOKE);
            }
            // Stop Project Nirvash
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Keyboard accelerators are added here to avoid showing 'Alt + left' tooltip on the page.
            // More info on tracking issue https://github.com/Microsoft/microsoft-ui-xaml/issues/8
            KeyboardAccelerators.Add(_altLeftKeyboardAccelerator);
            KeyboardAccelerators.Add(_backKeyboardAccelerator);
            await Task.CompletedTask;

            // Start Project Nirvash
            Page_Loaded();
            // Stop Project Nirvash
        }

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw e.Exception;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
            // Start Project Nirvash
            if (e.SourcePageType == typeof(SettingsPage))
            {
                NavigationService.Navigate(typeof(SettingsPage));
                return;
            }
            // Stop Project Nirvash
            var selectedItem = GetSelectedItem(navigationView.MenuItems, e.SourcePageType);
            if (selectedItem != null)
            {
                Selected = selectedItem;
            }
        }

        private WinUI.NavigationViewItem GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
        {
            foreach (var item in menuItems.OfType<WinUI.NavigationViewItem>())
            {
                if (IsMenuItemForPageType(item, pageType))
                {
                    return item;
                }

                var selectedChild = GetSelectedItem(item.MenuItems, pageType);
                if (selectedChild != null)
                {
                    return selectedChild;
                }
            }

            return null;
        }

        private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            var pageType = menuItem.GetValue(NavHelper.NavigateToProperty) as Type;
            return pageType == sourcePageType;
        }

        private void OnItemInvoked(WinUI.NavigationView sender, WinUI.NavigationViewItemInvokedEventArgs args)
        {
            // Start Project Nirvash
            if (args.IsSettingsInvoked)
            {
                NavigationService.Navigate(typeof(SettingsPage));
                return;
            }
            // Stop Project Nirvash
            if (args.InvokedItemContainer is WinUI.NavigationViewItem selectedItem)
            {
                var pageType = selectedItem.GetValue(NavHelper.NavigateToProperty) as Type;
                NavigationService.Navigate(pageType, null, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            NavigationService.GoBack();
        }

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
            if (modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var result = NavigationService.GoBack();
            args.Handled = result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #region Project Nirvash

        public string PaneTitle
        {
            get { return "AppDisplayName".GetLocalized(); }
        }

        internal static ShellPage Current { get; set; }

        internal TeachingTip AppNotification { get; private set; }

        internal Loading Loader { get; private set; }

        private bool _avoidCheck = false;

        internal Profile AccountDetails { get; private set; } = Singleton<ClientExtensions>.Instance.AccountDetails;

        /// <summary>
        /// Method invoked once navigated to the page.
        /// Begins user login.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded()
        {
            if (SystemInformation.IsFirstRun) await new PrivacyPolicyDialog().ShowAsync();

            PasswordCredential credentials = SecurityExtensions.RetrieveCredentials();
            if (credentials != null)
            {
                _avoidCheck = true;

                credentials.RetrievePassword();

                UsernameTextBox.Text = credentials.UserName;
                PasswordPasswordBox.Password = credentials.Password;

                Button_Click(null, null);
            }
        }

        /// <summary>
        /// Method invoked when the user clicks on Accedi.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            StatusProgressBar.Opacity = 1;
            UsernameTextBox.IsEnabled = false;
            PasswordPasswordBox.IsEnabled = false;
            LoginButton.IsEnabled = false;

            bool authenticated;
            try
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("HelloAuthenticationEnabled"))
                {
                    if (await SecurityExtensions.Authenticate())
                    {
                        authenticated = await Singleton<ClientExtensions>.Instance.AuthenticateAndInitDataAsync(new UserCredentials(UsernameTextBox.Text, PasswordPasswordBox.Password), true);
                    }
                    else
                    {
                        PasswordPasswordBox.Password = "";
                        UsernameTextBox.IsEnabled = true;
                        PasswordPasswordBox.IsEnabled = true;
                        LoginButton.IsEnabled = true;
                        StatusProgressBar.Opacity = 0;
                        return;
                    }
                }
                else
                {
                    authenticated = await Singleton<ClientExtensions>.Instance.AuthenticateAndInitDataAsync(new UserCredentials(UsernameTextBox.Text, PasswordPasswordBox.Password), true);
                }

                SecurityExtensions.AddCredentials(UsernameTextBox.Text, PasswordPasswordBox.Password);
                BackgroundTaskExtensions.Register(120);
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent(ex.Message, new Dictionary<string, string> { { "exception", ex.ToString() } });

                AppNotification.IsOpen = true;
                AppNotification.Title = "Attention".GetLocalized();

                if (ex is UnauthorizedUserException)
                { 
                    AppNotification.Subtitle = ex.Message;
                    UsernameTextBox.IsEnabled = true;
                    PasswordPasswordBox.IsEnabled = true;
                    LoginButton.IsEnabled = true;
                    StatusProgressBar.Opacity = 0;
                    return;
                }
                else if (ex is HttpRequestException)
                {
                    AppNotification.Subtitle = "HttpRequestException".GetLocalized();
                    UsernameTextBox.IsEnabled = true;
                    PasswordPasswordBox.IsEnabled = true;
                    LoginButton.IsEnabled = true;
                    StatusProgressBar.Opacity = 0;
                    return;
                }
                else
                {
                    AppNotification.Subtitle = "UnhandledException".GetLocalized();
                }
            }

            NavigationService.Navigate(typeof(MainPage), new EntranceNavigationTransitionInfo());
            navigationView.SelectedItem = navigationView.MenuItems[0];
            IsBackEnabled = false;
            NavigationService.Frame.BackStack.Clear();

            Loader.IsLoading = false;

            _avoidCheck = false;

            UsernameTextBox.IsEnabled = true;
            PasswordPasswordBox.IsEnabled = true;
            LoginButton.IsEnabled = true;
            StatusProgressBar.Opacity = 0;
        }

        /// <summary>
        /// Method invoked when the text in the UsernameTextBox changed.
        /// Sets LoginButton enabled or disabled state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_avoidCheck)
            {
                LoginButton.IsEnabled = !string.IsNullOrWhiteSpace(UsernameTextBox.Text) && !string.IsNullOrWhiteSpace(PasswordPasswordBox.Password);
            }
        }

        /// <summary>
        /// Method invoked when the text in the PasswordPasswordBox changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) => TextBox_TextChanged(null, null);

        /// <summary>
        /// Method invoked when the user presses a key on the keyboard.
        /// If the Enter key is pressed begin the login process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter) Button_Click(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adKind"></param>
        public async void ShowPopupAd(AdManager.AdKind adKind)
        {
            (await AdManager.GetPopupAd(this.LayoutRoot, adKind)).ShowAd();
        }

        #endregion Project Nirvash
    }
}
