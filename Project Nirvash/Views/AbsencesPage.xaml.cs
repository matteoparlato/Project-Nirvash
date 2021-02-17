using Project_Nirvash.Core.Helpers;
using Project_Nirvash.Dialogs;
using Project_Nirvash.Helpers;
using Project_Nirvash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Project_Nirvash.Views
{
    /// <summary>
    /// AbsencesPage class
    /// </summary>
    public sealed partial class AbsencesPage : Page
    {
        internal Profile AccountDetails = Singleton<ClientExtensions>.Instance.AccountDetails;

        /// <summary>
        /// Parameterless constructor of AbsencesPage class.
        /// </summary>
        public AbsencesPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method which occurs when the page is laid out, rendered, and ready for interaction.
        /// Opens AbsenceToJustifyDetailsDialog if there are events to justify.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<Event> eventsToJustify = Singleton<ClientExtensions>.Instance.AccountDetails.AbsenceEvents.Where(item => !item.IsJustified).ToList();
            if (eventsToJustify.Count > 0)
            {
                await new AbsenceToJustifyDetailsDialog(eventsToJustify).ShowAsync();
            }
        }

        /// <summary>
        /// Method which occurs when CalendarView is loading.
        /// Adds density to a CalendarViewDayItem if an event is scheduled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CalendarView_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            IEnumerable<Event> absences = Singleton<ClientExtensions>.Instance.AccountDetails.AbsenceEvents.Where(item => args.Item != null && item.Date.Date.Equals(args.Item.Date.Date));

            List<Color> densityList = new List<Color>();
            foreach (Event absence in absences)
            {
                if (absence.IsJustified)
                {
                    densityList.Add((Color)Application.Current.Resources["SystemAccentColor"]);
                }
                else
                {
                    densityList.Add((Color)Application.Current.Resources["HighlightColor"]);
                }
            }

            args.Item?.SetDensityColors(densityList);
        }

        /// <summary>
        /// Method which occurs when the collection returned by the SelectedDates property is changed.
        /// Opens AbsenceDetailsDialog to display detailed data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void CalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            try
            {
                List<Event> events = Singleton<ClientExtensions>.Instance.AccountDetails.AbsenceEvents.Where(item => item.Date.Date.Equals(sender.SelectedDates[0].Date.Date)).ToList();
                if (events.Count > 0)
                {
                    sender.SelectedDates.Clear();
                    await new AbsenceDetailsDialog(events).ShowAsync();
                }
            }
            catch (Exception)
            {
                // Exception thrown every time the user selects many times the same day in CalendarView.
            }
        }

        /// <summary>
        /// Method invoked when the user clicks on Go to today.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarButton_Tapped(object sender, TappedRoutedEventArgs e) => AbsencesCalendarView.SetDisplayDate(DateTimeOffset.Now);

        /// <summary>
        /// Method invoked when the user clicks on Events to justify.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AppBarButton_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            List<Event> absences = Singleton<ClientExtensions>.Instance.AccountDetails.AbsenceEvents.Where(item => !item.IsJustified).ToList();
            await new AbsenceToJustifyDetailsDialog(Singleton<ClientExtensions>.Instance.AccountDetails.AbsenceEvents.Where(item => !item.IsJustified).ToList()).ShowAsync();
        }
    }
}
