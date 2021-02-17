using Project_Nirvash.Core.Helpers;
using Project_Nirvash.Helpers;
using Project_Nirvash.Models;
using System;
using Windows.UI.Xaml.Controls;

namespace Project_Nirvash.Views
{
    /// <summary>
    /// NoticeboardPage class
    /// </summary>
    public sealed partial class NoticeboardPage : Page
    {
        internal Profile AccountDetails = Singleton<ClientExtensions>.Instance.AccountDetails;

        /// <summary>
        /// Parameterless constructor of NoticeboardPage class.
        /// </summary>
        public NoticeboardPage()
        {
            InitializeComponent();
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
                await new Dialogs.ItemDetailsDialog(senderListView.SelectedItem as Item).ShowAsync();
            }
        }
    }
}
