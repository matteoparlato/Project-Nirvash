using Project_Nirvash.Helpers;
using Project_Nirvash.Models;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Project_Nirvash.Dialogs
{
    /// <summary>
    /// AgendaDetailsDialog class
    /// </summary>
    public sealed partial class AgendaDetailsDialog : ContentDialog
    {
        internal List<Agendum> Agenda { get; }

        /// <summary>
        /// Constructor which initializes a AgendaDetailsDialog object with passed information.
        /// </summary>
        /// <param name="agenda">The list of events to display</param>
        public AgendaDetailsDialog(List<Agendum> agenda)
        {
            this.InitializeComponent();

            Title = string.Format("AgendaDetailsTitle".GetLocalized(), agenda[0].BeginDateTime.Date.ToShortDateString());
            Agenda = agenda;
        }

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
