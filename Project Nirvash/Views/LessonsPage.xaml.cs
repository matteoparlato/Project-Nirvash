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
    /// LessonsPage class
    /// </summary>
    public sealed partial class LessonsPage : Page
    {
        internal Profile AccountDetails = Singleton<ClientExtensions>.Instance.AccountDetails;

        /// <summary>
        /// Parameterless constructor of LessonsPage class.
        /// </summary>
        public LessonsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method which occurs when CalendarView is loading.
        /// Adds density to a CalendarViewDayItem for each lesson.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CalendarView_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            IEnumerable<Lesson> lessons = Singleton<ClientExtensions>.Instance.AccountDetails.Lessons.Where(item => args.Item != null && item.LessonDate.Date.Date.Equals(args.Item.Date.Date));

            List<Color> densityList = new List<Color>();
            foreach (Lesson lesson in lessons)
            {
                densityList.Add((Color)Application.Current.Resources["SystemAccentColor"]);
            }

            args.Item?.SetDensityColors(densityList);
        }

        /// <summary>
        /// Method which occurs when the collection returned by the SelectedDates property is changed.
        /// Opens LessonsDetailsDialog to display detailed data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void CalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            try
            {
                List<Lesson> lessons = Singleton<ClientExtensions>.Instance.AccountDetails.Lessons.Where(item => item.LessonDate.Date.Date.Equals(sender.SelectedDates[0].Date.Date)).ToList();
                if (lessons.Count > 0)
                {
                    sender.SelectedDates.Clear();
                    await new LessonsDetailsDialog(lessons).ShowAsync();
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
        private void AppBarButton_Tapped(object sender, TappedRoutedEventArgs e) => LessonsCalendarView.SetDisplayDate(DateTimeOffset.Now);
    }
}
