using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace Project_Nirvash.Helpers
{
    /// <summary>
    /// NotificationExtensions class
    /// </summary>
    public static class NotificationExtensions
    {
        internal const string NOTIFICATION_GROUP_ID = "88E0B7C4-6691-419A-A4E8-B1407B9A7B56";

        /// <summary>
        /// Method which sends a toast notification.
        /// </summary>
        /// <param name="text">The text of the notification</param>
        public static void SendToastNotification(string text)
        {
            ToastContentBuilder builder = new ToastContentBuilder().SetToastScenario(ToastScenario.Default)
                .AddToastActivationInfo("", ToastActivationType.Background)
                .AddHeader(NOTIFICATION_GROUP_ID, "", "")
                .AddText(text);

            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(builder.Content.GetXml()));
        }

        /// <summary>
        /// Method which sends a tile update.
        /// </summary>
        /// <param name="text">The text of the notification</param>
        public static void SendTileNotification(string text)
        {
            TileContentBuilder builder = new TileContentBuilder();

            builder.AddTile(TileSize.Medium)
                .SetBranding(TileBranding.Logo)
                .SetTextStacking(TileTextStacking.Center, TileSize.Medium)
                .AddText(text, TileSize.Medium, AdaptiveTextStyle.Default, hintWrap: true, hintAlign: AdaptiveTextAlign.Center);
    
            builder.AddTile(TileSize.Wide)
                .SetBranding(TileBranding.Logo)
                .SetTextStacking(TileTextStacking.Center, TileSize.Wide)
                .AddText(text, TileSize.Wide, AdaptiveTextStyle.Body, hintWrap: true, hintAlign: AdaptiveTextAlign.Center);
 
            builder.AddTile(TileSize.Large)
                .SetBranding(TileBranding.Logo)
                .SetTextStacking(TileTextStacking.Center, TileSize.Large)
                .AddText(text, TileSize.Large, AdaptiveTextStyle.Body, hintWrap: true, hintAlign: AdaptiveTextAlign.Center);

            TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(builder.Content.GetXml()));
        }
    }
}
