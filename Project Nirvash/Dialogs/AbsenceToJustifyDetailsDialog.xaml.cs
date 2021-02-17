using Project_Nirvash.Helpers;
using Project_Nirvash.Models;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Project_Nirvash.Dialogs
{
    /// <summary>
    /// AbsenceToJustifyDetailsDialog class
    /// </summary>
    public sealed partial class AbsenceToJustifyDetailsDialog : ContentDialog
    {
        internal List<Event> Absences { get; }

        /// <summary>
        /// Constructor which initializes a AbsenceToJustifyDetailsDialog object with passed information.
        /// </summary>
        /// <param name="events">The list of events to display</param>
        public AbsenceToJustifyDetailsDialog(List<Event> events)
        {
            this.InitializeComponent();

            Title = string.Format("AbsenceToJustifyDetailsTitle".GetLocalized());
            Absences = events;
        }
    }
}
