using Project_Nirvash.Helpers;
using Project_Nirvash.Models;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Project_Nirvash.Dialogs
{
    /// <summary>
    /// AbsenceDetailsDialog class
    /// </summary>
    public sealed partial class AbsenceDetailsDialog : ContentDialog
    {
        internal List<Event> Absences { get; }

        /// <summary>
        /// Constructor which initializes a AbsenceDetailsDialog object with passed information.
        /// </summary>
        /// <param name="events">The list of events to display</param>
        public AbsenceDetailsDialog(List<Event> events)
        {
            this.InitializeComponent();

            Title = string.Format("AbsenceDetailsTitle".GetLocalized(), events[0].Date.ToShortDateString());
            Absences = events;
        }
    }
}
