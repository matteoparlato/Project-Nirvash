using Project_Nirvash.Core.Helpers;
using Project_Nirvash.Dialogs;
using Project_Nirvash.Helpers;
using Project_Nirvash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Windows.ApplicationModel.Appointments;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Project_Nirvash.Views
{
    /// <summary>
    /// AgendaPage class
    /// </summary>
    public sealed partial class AgendaPage : Page
    {
        internal Profile AccountDetails = Singleton<ClientExtensions>.Instance.AccountDetails;

        /// <summary>
        /// Parameterless constructor of AgendaPage class.
        /// </summary>
        public AgendaPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method which occurs when CalendarView is loading.
        /// Adds density to a CalendarViewDayItem if an agenda event is scheduled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CalendarView_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            IEnumerable<Agendum> agendum = Singleton<ClientExtensions>.Instance.AccountDetails.AgendaEvents.Where(item => args.Item != null && item.BeginDateTime.Date.Date.Equals(args.Item.Date.Date));

            List<Color> densityList = new List<Color>();
            foreach (Agendum agenda in agendum)
            {
                densityList.Add((Color)Application.Current.Resources["SystemAccentColor"]);
            }

            args.Item?.SetDensityColors(densityList);
        }

        /// <summary>
        /// Method which occurs when the collection returned by the SelectedDates property is changed.
        /// Opens AgendaDetailsDialog to display detailed data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void CalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            try
            {
                List<Agendum> agendaEvents = Singleton<ClientExtensions>.Instance.AccountDetails.AgendaEvents.Where(item => item.BeginDateTime.Date.Date.Equals(sender.SelectedDates[0].Date.Date)).ToList();
                if (agendaEvents.Count > 0)
                {
                    sender.SelectedDates.Clear();
                    await new AgendaDetailsDialog(agendaEvents).ShowAsync();
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
        private void AppBarButton_Tapped(object sender, TappedRoutedEventArgs e) => AgendaCalendarView.SetDisplayDate(DateTimeOffset.Now);

        /// <summary>
        /// Method invoked when the user clicks on Open Outlook Calendar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AppBarButton_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            await AppointmentManager.ShowTimeFrameAsync(DateTime.Now, TimeSpan.FromDays(7));
        }
    }
}
