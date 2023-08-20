using Project_Nirvash.Helpers;
using Project_Nirvash.Models;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System;
using Project_Nirvash.Dialogs;
using System.Linq;
using System.Collections.Generic;
using Project_Nirvash.Core.Helpers;

namespace Project_Nirvash.Views
{
    /// <summary>
    /// MainPage class
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Profile AccountDetails = Singleton<ClientExtensions>.Instance.AccountDetails;

        internal List<Lesson> Lessons { get; }
        internal List<Event> Events { get; }
        internal List<Agendum> Agenda { get; }
        internal List<Item> Noticeboard { get; }

        /// <summary>
        /// Parameterless constructor of MainPage class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            LessonsListView.ItemsSource = Lessons = AccountDetails.Lessons.Where(item => item.LessonDate.Date.Equals(DateTime.Today)).ToList();
            EventsListView.ItemsSource = Events = AccountDetails.AbsenceEvents.Where(item => !item.IsJustified).ToList();
            AgendaListView.ItemsSource = Agenda = AccountDetails.AgendaEvents.Where(item => item.BeginDateTime.Date.Equals(DateTime.Today.AddDays(1))).ToList();
            NoticeboardListView.ItemsSource = Noticeboard = AccountDetails.NoticeboardItems.Take(10).ToList();
        }

        /// <summary>
        /// Method which occurs when the page is laid out, rendered, and ready for interaction.
        /// Clears the navigation back stack and shows AdDialog on first MainPage navigation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            Frame.BackStack.Clear();
        }

        /// <summary>
        /// Method which occurs when the currently selected item changes.
        /// Opens ItemDetailsDialog to show detailed data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView senderListView = (ListView)sender;
            if (senderListView.SelectedItem != null)
            {
                await new ItemDetailsDialog(((ListView)sender).SelectedItem as Item).ShowAsync();
            }
        }

        /// <summary>
        /// Method which occurs before a ListView loses focus.
        /// Deselects all selected elements of a ListView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ListView_LosingFocus(UIElement sender, LosingFocusEventArgs args) => ((ListView)sender).SelectedItem = null;

        /// <summary>
        /// Method invoked when the user clicks on Add to Outlook Calendar (SwipeControl).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void SwipeItem_Invoked(SwipeItem sender, SwipeItemInvokedEventArgs args) => AppointmentExtensions.CreateAppointmentFromAgendum((Agendum)args.SwipeControl.DataContext);

        /// <summary>
        /// Method invoked when the user clicks on Add to Outlook Calendar (Button).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Tapped(object sender, TappedRoutedEventArgs e) => AppointmentExtensions.CreateAppointmentFromAgendum((Agendum)((Button)sender).DataContext);

        /// <summary>
        /// Method invoked when the poiter enters on the UserControl.
        /// Shows Add to Outlook Calendar button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_PointerEntered(object sender, PointerRoutedEventArgs e) => VisualStateManager.GoToState(sender as Control, "HoverButtonsShown", true);

        /// <summary>
        /// Method invoked when the poiter exits from the UserControl.
        /// Hides Add to Outlook Calendar button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_PointerExited(object sender, PointerRoutedEventArgs e) => VisualStateManager.GoToState(sender as Control, "HoverButtonsHidden", true);
    }
}
