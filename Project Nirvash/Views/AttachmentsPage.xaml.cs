using Project_Nirvash.Core.Helpers;
using Project_Nirvash.Dialogs;
using Project_Nirvash.Helpers;
using Project_Nirvash.Models;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace Project_Nirvash.Views
{
    /// <summary>
    /// AttachmentsPage class
    /// </summary>
    public sealed partial class AttachmentsPage : Page
    {
        internal Profile AccountDetails = Singleton<ClientExtensions>.Instance.AccountDetails;
        internal Didactict DidactictItem;
        internal Content ContentItem;

        private readonly DataTransferManager dataTransferManager;
        private DataPackage dataPackage;

        /// <summary>
        /// Parameterless constructor of AttachmentsPage class.
        /// Initializes DataTransferManager for Content sharing.
        /// </summary>
        public AttachmentsPage()
        {
            InitializeComponent();

            dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
        }

        /// <summary>
        /// Method which occurs when a share operation starts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            args.Request.Data = dataPackage;
            args.Request.Data.Properties.Title = ContentItem.Name;
        }

        /// <summary>
        /// Method which occurs when the page is laid out, rendered, and ready for interaction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (DidactictItem == null && AccountDetails.DidacticsItems.Count > 0)
            {
                DidactictItem = AccountDetails.DidacticsItems[0];
                MasterListView.SelectedIndex = 0;
            }
            // Start Project Nirvash
            // TODO: Implement Narrow State
            //// If the app starts in narrow mode - showing only the Master listView - 
            //// it is necessary to set the commands and the selection mode.
            //if (PageSizeStatesGroup.CurrentState == NarrowState)
            //{
            //    VisualStateManager.GoToState(this, MasterState.Name, true);
            //}
            //else
            //{
            // Stop Project Nirvash
            if (PageSizeStatesGroup.CurrentState == WideState)
            {
                // In this case, the app starts is wide mode, Master/Details view, 
                // so it is necessary to set the commands and the selection mode.
                VisualStateManager.GoToState(this, MasterDetailsState.Name, true);
                MasterListView.SelectionMode = ListViewSelectionMode.Extended;
                MasterListView.SelectedItem = DidactictItem;
            }
            // Start Project Nirvash
            //else
            //{
            //    new InvalidOperationException();
            //}
            ContentAutoSuggestBox.Focus(FocusState.Keyboard);
            // Stop Project Nirvash
        }

        /// <summary>
        /// Method which handles the CurrentStateChanged event of VisualStateManager.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisualStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            // Start Project Nirvash
            // TODO: Implement Narrow State
            //bool isNarrow = e.NewState == NarrowState;
            //if (isNarrow)
            //{
            //    NavigationService.Navigate(typeof(AttachmentsNarrowPage), DidactictItem, new SuppressNavigationTransitionInfo());
            //}
            //else
            //{
            // Stop Project Nirvash
            VisualStateManager.GoToState(this, MasterDetailsState.Name, true);
            MasterListView.SelectionMode = ListViewSelectionMode.Extended;
            MasterListView.SelectedItem = DidactictItem;
            // Start Project Nirvash
            //}

            //EntranceNavigationTransitionInfo.SetIsTargetElement(MasterListView, isNarrow);
            // Stop Project Nirvash
            if (DetailContentPresenter != null)
            {
                // Start Project Nirvash
                // ORG: EntranceNavigationTransitionInfo.SetIsTargetElement(DetailContentPresenter, !isNarrow);
                EntranceNavigationTransitionInfo.SetIsTargetElement(DetailContentPresenter, false);
                // Stop Project Nirvash
            }
        }

        /// <summary>
        /// Method which occurs when the currently selected item in MasterListView changes.
        /// Loads DetailList content.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MasterListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PageSizeStatesGroup.CurrentState == WideState)
            {
                if (MasterListView.SelectedItems.Count == 1)
                {
                    DidactictItem = MasterListView.SelectedItem as Didactict;
                    DetailContentPresenter.ContentTransitions.Clear();
                    DetailContentPresenter.ContentTransitions.Add(new EntranceThemeTransition());
                }
                // Entering in Extended selection
                else
                {
                    if (MasterListView.SelectedItems.Count > 1 && MasterDetailsStatesGroup.CurrentState == MasterDetailsState)
                    {
                        VisualStateManager.GoToState(this, ExtendedSelectionState.Name, true);
                    }
                }
            }
            // Exiting Extended selection
            if (MasterDetailsStatesGroup.CurrentState == ExtendedSelectionState && MasterListView.SelectedItems.Count == 1)
            {
                VisualStateManager.GoToState(this, MasterDetailsState.Name, true);
            }
        }

        /// <summary>
        /// ItemClick event only happens when user is a Master state. In this state, 
        /// selection mode is none and click event navigates to the details view. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            DidactictItem = e.ClickedItem as Didactict;
            // Start Project Nirvash
            // TODO: Implement Narrow State
            //if (PageSizeStatesGroup.CurrentState == NarrowState)
            //{
            //    NavigationService.Navigate(typeof(AttachmentsNarrowPage), DidactictItem, new DrillInNavigationTransitionInfo());
            //}
            // Stop Project Nirvash
        }

        /// <summary>
        /// Method which occurs when the currently selected item in DetailListView changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView senderListView = (ListView)sender;
            if (senderListView.SelectedItem != null)
            {
                ExecuteContentAction(senderListView.SelectedItem as Content, false);
            }
        }

        /// <summary>
        /// Method invoked when the user clicks Download.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadContentButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Button senderButton = (Button)sender;
            if (senderButton.DataContext != null)
            {
                ExecuteContentAction(((Button)sender).DataContext as Content, false);
            }
        }

        /// <summary>
        /// Method invoked when the user clicks Share.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShareContentButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Button senderButton = (Button)sender;
            if (senderButton.DataContext != null)
            {
                ExecuteContentAction(((Button)sender).DataContext as Content, true);
            }
        }

        /// <summary>
        /// Method invoked when the poiter enters on the UserControl.
        /// Shows Download and Share button.
        /// </summary>
        private void UserControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender != null && (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse || e.Pointer.PointerDeviceType == PointerDeviceType.Pen))
            {
                VisualStateManager.GoToState(sender as Control, "HoverButtonsShown", true);
            }
        }

        /// <summary>
        /// Method invoked when the poiter exits from the UserControl.
        /// Hides Download and Share button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_PointerExited(object sender, PointerRoutedEventArgs e) => VisualStateManager.GoToState(sender as Control, "HoverButtonsHidden", true);

        /// <summary>
        /// Method invoked when the text in the ContentAutoSuggestBox changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                List<Content> contentSuggestions = Models.Content.SearchContentByName(sender.Text);

                if (contentSuggestions.Count > 0)
                {
                    sender.ItemsSource = contentSuggestions;
                }
            }
        }

        /// <summary>
        /// Method invoked when the user keys through the list, or taps on a suggestion in ContentAutoSuggestBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Content selectedContentSuggestion = args.SelectedItem is Content content ? content : null;
            
            if (selectedContentSuggestion != null)
            {
                sender.Text = selectedContentSuggestion.Name;

                ExecuteContentAction(selectedContentSuggestion, false);
            }
        }

        /// <summary>
        /// Method which occurs when a keyboard shortcut (or accelerator) is pressed.
        /// Avoids unwanted AutoSuggestBox_SuggestionChosen invocations when pressing Up and Down keys.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void AutoSuggestBox_ProcessKeyboardAccelerators(UIElement sender, ProcessKeyboardAcceleratorEventArgs args)
        {
            if (args.Key == VirtualKey.Down || args.Key == VirtualKey.Up)
            {
                args.Handled = true;
            }
        }

        /// <summary>
        /// Method which downloads, shows or opens the passed Content.
        /// </summary>
        /// <param name="content">The Content to process</param>
        /// <param name="share">True to share the content, false otherwise</param>
        private async void ExecuteContentAction(Content content, bool share)
        {
            ContentItem = content;
            if (content != null)
            {
                switch (content.ObjectType)
                {
                    case Models.Content.FILE:
                        {
                            DownloadTeachingTip.IsOpen = true;
                            StorageFile storageFile = await Singleton<ClientExtensions>.Instance.GetDidacticsItemAttachmentAsync(content);
                            DownloadTeachingTip.IsOpen = false;
                            if (share)
                            {
                                dataPackage = new DataPackage();
                                dataPackage.SetStorageItems(new List<StorageFile> { storageFile });
                                DataTransferManager.ShowShareUI();
                            }
                            else
                            {
                                await Launcher.LaunchFileAsync(storageFile);
                            }

                            break;
                        }
                    case Models.Content.LINK:
                        {
                            Uri uri = await Singleton<ClientExtensions>.Instance.GetDidacticsItemLinkAsync(content);
                            if (share)
                            {
                                dataPackage = new DataPackage();
                                dataPackage.SetWebLink(uri);
                                DataTransferManager.ShowShareUI();
                            }
                            else
                            {
                                await Launcher.LaunchUriAsync(uri);
                            }

                            break;
                        }
                    case Models.Content.TEXT:
                        {
                            string message = await Singleton<ClientExtensions>.Instance.GetDidacticsItemTextAsync(content);
                            if (share)
                            {
                                dataPackage = new DataPackage();
                                dataPackage.SetText(message);
                                DataTransferManager.ShowShareUI();
                            }
                            else
                            {
                                await new ContentMessageDialog(message, ContentItem.Name).ShowAsync();
                            }

                            break;
                        }
                }
            }
        }
    }
}
