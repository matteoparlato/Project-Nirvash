using Project_Nirvash.Core.Helpers;
using Project_Nirvash.Helpers;
using Project_Nirvash.Models;
using System;
using System.Linq;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Project_Nirvash.Dialogs
{
    /// <summary>
    /// ItemDetailsDialog class
    /// </summary>
    public sealed partial class ItemDetailsDialog : ContentDialog
    {
        Item Item { get; }

        /// <summary>
        /// Constructor which initializes a ItemDetailsDialog object with passed information.
        /// </summary>
        /// <param name="details">The item to display</param>
        public ItemDetailsDialog(Item details)
        {
            this.InitializeComponent();

            this.Item = details;
        }

        /// <summary>
        /// Method which occurs when the dialog is laid out, rendered, and ready for interaction.
        /// Updates the read status of the current item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Singleton<ClientExtensions>.Instance.SetNoticeboardReadStatusAsync(Item.EventCode, Item.ID);
                Singleton<ClientExtensions>.Instance.AccountDetails.NoticeboardItems.FirstOrDefault(item => item.ID == Item.ID).ReadStatus = true;
            }
            catch (Exception)
            {
                //
            }
        }

        /// <summary>
        /// Method which occurs when the currently selected item changes.
        /// Download the selected attachment.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView senderListView = (ListView)sender;
            if (senderListView.SelectedItem != null)
            {
                DownloadProgressBar.IsIndeterminate = true;

                Attachment attachment = senderListView.SelectedItem as Attachment;
                StorageFile storageFile = await Singleton<ClientExtensions>.Instance.GetNoticeboardItemAttachmentAsync(Item.EventCode, Item.ID, attachment.AttachmentNo);
                await Launcher.LaunchFileAsync(storageFile);

                DownloadProgressBar.IsIndeterminate = false;
            }
        }
    }
}
