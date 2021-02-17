using Microsoft.Toolkit.Uwp.Helpers;
using Project_Nirvash.Helpers;
using Project_Nirvash.Models;
using System;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Security.Credentials;
using Windows.UI.Notifications;

namespace Project_Nirvash.BackgroundTasks
{
    /// <summary>
    /// UpdaterTask class
    /// </summary>
    public sealed class UpdaterTask : IBackgroundTask
    {
        private readonly RoamingObjectStorageHelper _roamingObjectStorage = new RoamingObjectStorageHelper();

        /// <summary>
        /// Performs the work of a background task.
        /// The system calls this method when the associated background task has been triggered.
        /// </summary>
        /// <param name="taskInstance">An interface to an instance of the background task</param>
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            taskInstance.GetDeferral();

            Update(taskInstance);
        }

        /// <summary>
        /// Method which performs the work of the background task.
        /// </summary>
        /// <param name="taskInstance">The interface to an instance of the background task</param>
        private async void Update(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

#if !DEBUG
            if (!(DateTime.Today.Month == 7 || DateTime.Today.Month == 8))
#else
            if (true)
#endif
            {
                PasswordCredential credential = SecurityExtensions.RetrieveCredentials();
                if (credential != null)
                {
                    credential.RetrievePassword();

                    try
                    {
                        TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                        ToastNotificationManager.History.Clear();

                        using (ClientExtensions client = new ClientExtensions())
                        {
                            await client.AuthenticateAndInitDataAsync(new UserCredentials(credential.UserName, credential.Password), false);

                            CheckAndSendNotification(BackgroundTaskExtensions.EVENTS, client.AccountDetails.AbsenceEvents.Count(item => !item.IsJustified), "AbsenceNotification".GetLocalized());
                            CheckAndSendNotification(BackgroundTaskExtensions.APPOINTMENTS, client.AccountDetails.AgendaEvents.Count, "AgendaNotification".GetLocalized());
                            CheckAndSendNotification(BackgroundTaskExtensions.DIDACTICS, client.AccountDetails.DidacticsItems.Count, "DidactictNotification".GetLocalized());
                            CheckAndSendNotification(BackgroundTaskExtensions.GRADES, client.AccountDetails.Grades.Count, "GradeNotification".GetLocalized());
                            CheckAndSendNotification(BackgroundTaskExtensions.NOTES, client.AccountDetails.Notes.Count, "NoteNotification".GetLocalized());
                            CheckAndSendNotification(BackgroundTaskExtensions.ITEMS, client.AccountDetails.NoticeboardItems.Count, "NoticeboardNotification".GetLocalized());
                        }
                    }
                    catch (Exception)
                    {
                        //
                    }
                }
            }
            else
            {
                ResourceExtensions.StoreRoamingObject(BackgroundTaskExtensions.EVENTS, 0);
                ResourceExtensions.StoreRoamingObject(BackgroundTaskExtensions.APPOINTMENTS, 0);
                ResourceExtensions.StoreRoamingObject(BackgroundTaskExtensions.DIDACTICS, 0);
                ResourceExtensions.StoreRoamingObject(BackgroundTaskExtensions.GRADES, 0);
                ResourceExtensions.StoreRoamingObject(BackgroundTaskExtensions.NOTES, 0);
                ResourceExtensions.StoreRoamingObject(BackgroundTaskExtensions.ITEMS, 0);
            }

            deferral.Complete();
        }

        private void CheckAndSendNotification(string key, int newValue, string text)
        {
            if (_roamingObjectStorage.KeyExists(key))
            {
                int oldValue = _roamingObjectStorage.Read<int>(key);

                if (newValue > oldValue)
                {
                    NotificationExtensions.SendToastNotification(text);
                    NotificationExtensions.SendTileNotification(text);

                    ResourceExtensions.StoreRoamingObject(key, newValue);
                }
            }
        }
    }
}