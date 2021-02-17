using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.Security.Credentials.UI;
using Windows.Storage;

namespace Project_Nirvash.Helpers
{
    /// <summary>
    /// SecurityExtensions class
    /// </summary>
    internal static class SecurityExtensions
    {
        internal const string RESOURCE_NAME = "Registro Elettronico S";

        /// <summary>
        /// Method which requests user authentication with Windows Hello if any athentication device
        /// is available.
        /// </summary>
        /// <returns>The authentication status</returns>
        internal async static Task<bool> Authenticate()
        {
            UserConsentVerifierAvailability userConsentVerifierAvailability = await UserConsentVerifier.CheckAvailabilityAsync();

            switch (userConsentVerifierAvailability)
            {
                // TODO: Implement other UserConsentVerifierAvailability cases
                case UserConsentVerifierAvailability.Available:
                    return await UserConsentVerifier.RequestVerificationAsync("WindowsHelloMessage".GetLocalized()) == UserConsentVerificationResult.Verified;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Method that unregisters the HelloAuthenticationEnabled key to disable Windows Hello.
        /// </summary>
        internal static void RemoveKey()
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("HelloAuthenticationEnabled"))
            {
                ApplicationData.Current.LocalSettings.Values.Remove("HelloAuthenticationEnabled");
            }
        }

        /// <summary>
        /// Method that registers the HelloAuthenticationEnabled key to enable Windows Hello.
        /// </summary>
        internal static void RegisterKey()
        {
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("HelloAuthenticationEnabled"))
            {
                ApplicationData.Current.LocalSettings.Values.Add("HelloAuthenticationEnabled", true);
            }
        }

        /// <summary>
        /// Method which returns the credentials stored in the Windows vault by the application.
        /// </summary>
        /// <returns>User credentials</returns>
        internal static PasswordCredential RetrieveCredentials() => new PasswordVault().RetrieveAll().FirstOrDefault(item => item.Resource.Equals(RESOURCE_NAME));

        /// <summary>
        /// Method which removes the credentials from the Windows vault stored by the application.
        /// </summary>
        internal static void RemoveCredentials()
        {
            RemoveKey();

            PasswordCredential credential = RetrieveCredentials();
            if (credential != null)
            {
                new PasswordVault().Remove(credential);
            }
        }

        /// <summary>
        /// Method which stores the credentials into the Windows vault.
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        internal static void AddCredentials(string username, string password)
        {
            new PasswordVault().Add(new PasswordCredential(RESOURCE_NAME, username, password));
        }
    }
}
