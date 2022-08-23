using System.Collections.Generic;

namespace MikrosClient
{
    /// <summary>
    /// Class to hold all server related data.
    /// </summary>
    internal static class ServerData
    {
        private static readonly List<string> devAppGameIds = new List<string> { "tg-60a203ce5ddc1", "tg-5f7b1ea4e1d5c", "tg-607036b01893e" };

        private const string serverUrlProduction = "https://tg-api-new.uc.r.appspot.com/"; // PRODUCTION base URL

        private const string serverUrlStaging = "https://tg-api-new-stage.uc.r.appspot.com/"; // STAGING base URL

        private const string sdkInitializationMethodName = "mikros/mobile/mikrosSdkInitialization";

        private const string sessionTrackMethodName = "mikros/mobile/mikrosOverallSessionTrack";

        private const string presetAnalyticsMethodName = "mikros/mobile/preset/";

        private const string customAnalyticsMethodName = "mikros/mobile/custom/trackEvents";

        private const string signinMethodName = "auth/mikros/mobile/signin";

        private const string signupMethodName = "auth/mikros/mobile/signup";

        private const string signOutMethodName = "auth/signout";

        private const string updateMetaDataMethodName = "mikros/mobile/updateUserMetaData";

        private const string getAllAppsMethodName = "mikros/mobile/getAllApps";

        private const string playerRatingMethodName = "mikros/mobile/sendBehaviorRatings";

        private static string BaseUrl => devAppGameIds.Contains(MikrosManager.Instance.ConfigurationController.MikrosSettings.AppGameID) ||
                                         MikrosManager.Instance.ConfigurationController.MikrosSettings.AppGameID.Contains(Constants.StageAppGameId) ?
                                         serverUrlStaging : serverUrlProduction;

        /// <summary>
        /// Get the url path by Url Type.
        /// </summary>
        /// <param name="urlType">Url Type is the variable to get a particular url path.</param>
        /// <returns>Full URL for web request.</returns>
        internal static string GetUrl(UrlType urlType)
        {
            switch (urlType)
            {
                case UrlType.BaseURL:
                    return BaseUrl;

                case UrlType.SDKInitialization:
                    return BaseUrl + sdkInitializationMethodName;

                case UrlType.SignIn:
                    return BaseUrl + signinMethodName;

                case UrlType.SignUp:
                    return BaseUrl + signupMethodName;

                case UrlType.SignOut:
                    return BaseUrl + signOutMethodName;

                case UrlType.UpdateMetaData:
                    return BaseUrl + updateMetaDataMethodName;

                case UrlType.GetAllApps:
                    return BaseUrl + getAllAppsMethodName;

                case UrlType.PresetAnalytics:
                    return BaseUrl + presetAnalyticsMethodName;

                case UrlType.CustomAnalytics:
                    return BaseUrl + customAnalyticsMethodName;

                case UrlType.SessionTrack:
                    return BaseUrl + sessionTrackMethodName;

                case UrlType.PlayerRating:
                    return BaseUrl + playerRatingMethodName;

                default:
                    return "";
            }
        }
    }
}