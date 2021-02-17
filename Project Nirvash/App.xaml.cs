using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Project_Nirvash.Services;
using Project_Nirvash.Views;
#if !DEBUG
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
#endif
using Project_Nirvash.BackgroundTasks;
using Windows.Devices.Input;
#if !DEBUG
using Windows.Storage;
using Microsoft.Toolkit.Uwp.Helpers;
#endif

namespace Project_Nirvash
{
    sealed partial class App : Application
    {
        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public bool IsTouchPresent { get; }

        private const string AppCenterAPIKey = "8cb56210-6786-4a7b-87de-85fe424e48a8";

        public App()
        {
#if !DEBUG
            if (!(ApplicationData.Current.LocalSettings.Values.ContainsKey("AnalyticsDisabled") || SystemInformation.IsFirstRun))
            {
                AppCenter.Start(AppCenterAPIKey, typeof(Analytics), typeof(Crashes), typeof(Push));
            }
            else
            {
                AppCenter.Start(AppCenterAPIKey, typeof(Push));
            }
#endif

            InitializeComponent();

            try
            {
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();

                ToastNotificationManager.History.Clear();

                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            }
            catch (Exception)
            {
                //
            }

            IsTouchPresent = new TouchCapabilities().TouchPresent == 0;

            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
            NavigationService.Navigate(typeof(LockedPage));
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(LockedPage), new Lazy<UIElement>(CreateShell));
        }

        private UIElement CreateShell()
        {
            return ShellPage.Current = new ShellPage();
        }

        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);
            new UpdaterTask().Run(args.TaskInstance);
        }
    }
}
