using Microsoft.AppCenter.Analytics;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using Windows.ApplicationModel.Resources;

namespace Project_Nirvash.Helpers
{
    internal static class ResourceExtensions
    {
        private static ResourceLoader _resLoader = new ResourceLoader();

        private static readonly RoamingObjectStorageHelper _roamingObjectStorage = new RoamingObjectStorageHelper();

        public static string GetLocalized(this string resourceKey)
        {
            return _resLoader.GetString(resourceKey);
        }

        public static void StoreRoamingObject(string key, object value)
        {
            try
            {
                _roamingObjectStorage.Save(key, value);
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent(string.Format("{0} {1}", ex.Message, key));
            }
        }
    }
}
