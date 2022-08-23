using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MikrosClient.Analytics
{
    /// <summary>
    /// Mikros internal event for initialization.
    /// </summary>
    internal sealed class MikrosSDKInitializeEvent
    {
        private string eventName = "mikros_sdk_initialize";
        private Hashtable eventData;

        /// <summary>
        /// Constructor for MikrosSDKInitializeEvent object creation.
        /// </summary>
        private MikrosSDKInitializeEvent()
        {
            eventData = new Hashtable();
        }

        /// <summary>
        /// Builder function is used for creating an object of MikrosSDKInitializeEvent class.
        /// </summary>
        /// <returns>MikrosSDKInitializeEvent object.</returns>
        internal static MikrosSDKInitializeEvent Builder()
        {
            return new MikrosSDKInitializeEvent();
        }

        /// <summary>
        /// Create function is used for validation of variables of the MikrosSDKInitializeEvent class.
        /// </summary>
        internal void Create()
        {
            MikrosManager.Instance.AnalyticsController.LogInternalEvent(eventName, eventData, data => { }, exception => { });
        }
    }

    /// <summary>
    /// Log auth related internal events.
    /// </summary>
    internal sealed class MikrosSDKAuthEvent
    {
        private List<string> eventNames = new List<string>()
        {
            "mikros_sdk_signup",
            "mikros_sdk_signin",
            "mikros_sdk_signout"
        };

        private List<string> parameterKeys = new List<string>()
        {
            "app_game_id",
            "user_id", "email", "phone", "user_type_id", "name", "first_name", "last_name", "date_of_birth", "sex", "verified_email", "verified_phone",
            "spending_score", "activity_score", "tendency_score", "reputation_score",
            "app_name", "app_build_number", "app_version",
            "latitude", "longitude", "device_manufacturer", "device_model", "device_os_version", "device_operating_system", "device_screen_dpi", "device_screen_height", "device_screen_width", "sdk_version", "is_wifi"
        };

        private string eventName;
        private Hashtable eventData;

        /// <summary>
        /// MikrosSDKAuthEvent private default constructor to restrict object creation of the class.
        /// </summary>
        private MikrosSDKAuthEvent()
        {
        }

        /// <summary>
        /// Parameterised constructor for MikrosSDKAuthEvent object creation.
        /// </summary>
        /// <param name="authType">Set the auth type.</param>
        private MikrosSDKAuthEvent(AUTH_TYPE authType)
        {
            eventName = eventNames[(int)authType];

            eventData = new Hashtable();
        }

        /// <summary>
        /// Builder function is used for creating an object of MikrosSDKAuthEvent class.
        /// </summary>
        /// <param name="authType">Set the auth type.</param>
        /// <returns>MikrosSDKAuthEvent object.</returns>
        internal static MikrosSDKAuthEvent Builder(AUTH_TYPE authType)
        {
            return new MikrosSDKAuthEvent(authType);
        }

        /// <summary>
        /// Set App Game ID.
        /// </summary>
        private void SetAppGameId()
        {
            string appGameId = Utils.ModifyStringIfEmpty(MikrosManager.Instance.ConfigurationController.MikrosSettings.AppGameID);
            eventData.Add(parameterKeys[0], appGameId);
        }

        /// <summary>
        /// Set Mikros user details.
        /// </summary>
        /// <param name="mikrosUserData">Mikros auth user full data.</param>
        /// <returns>MikrosSDKAuthEvent object.</returns>
        internal MikrosSDKAuthEvent SetUser(MikrosUserData mikrosUserData)
        {
            if (mikrosUserData == null)
            {
                return this;
            }

            string key = parameterKeys[1];
            string value = Utils.ModifyStringIfEmpty(mikrosUserData.Id);
            eventData.Add(key, value);

            key = parameterKeys[2];
            value = Utils.ModifyStringIfEmpty(mikrosUserData.Email);
            eventData.Add(key, value);

            key = parameterKeys[3];
            value = Utils.ModifyStringIfEmpty(mikrosUserData.Phone);
            eventData.Add(key, value);

            key = parameterKeys[4];
            value = Utils.ModifyStringIfEmpty(mikrosUserData.UserTypeId);
            eventData.Add(key, value);

            key = parameterKeys[5];
            value = Utils.ModifyStringIfEmpty(mikrosUserData.Name);
            eventData.Add(key, value);

            key = parameterKeys[6];
            value = Utils.ModifyStringIfEmpty(mikrosUserData.FirstName);
            eventData.Add(key, value);

            key = parameterKeys[7];
            value = Utils.ModifyStringIfEmpty(mikrosUserData.LastName);
            eventData.Add(key, value);

            key = parameterKeys[8];
            value = Utils.ModifyStringIfEmpty(mikrosUserData.DateOfBirth);
            eventData.Add(key, value);

            key = parameterKeys[9];
            value = Utils.ModifyStringIfEmpty(mikrosUserData.Sex);
            eventData.Add(key, value);

            key = parameterKeys[10];
            value = Utils.ModifyStringIfEmpty(mikrosUserData.VerifiedEmail);
            eventData.Add(key, value);

            key = parameterKeys[11];
            value = Utils.ModifyStringIfEmpty(mikrosUserData.VerifiedPhone);
            eventData.Add(key, value);

            key = parameterKeys[12];
            value = Utils.ModifyStringIfEmpty(mikrosUserData.SpendingScore);
            eventData.Add(key, value);

            key = parameterKeys[13];
            value = Utils.ModifyStringIfEmpty(mikrosUserData.ActivityScore);
            eventData.Add(key, value);

            key = parameterKeys[15];
            value = Utils.ModifyStringIfEmpty(mikrosUserData.ReputationScore);
            eventData.Add(key, value);

            return this;
        }

        /// <summary>
        /// Set details of app.
        /// </summary>
        private void SetAppDetails()
        {
            string key = parameterKeys[16];
            string value = Utils.ModifyStringIfEmpty(Application.productName);
            eventData.Add(key, value);

            key = parameterKeys[17];
            value = Utils.ModifyStringIfEmpty("");
            eventData.Add(key, value);

            key = parameterKeys[18];
            value = Utils.ModifyStringIfEmpty(Application.version);
            eventData.Add(key, value);
        }

        /// <summary>
        /// Set Metadata.
        /// </summary>
        private void SetMetadata()
        {
            Metadata metadata = Metadata.Builder().Create();

            string key = parameterKeys[19];
            string value = Utils.ModifyStringIfEmpty(metadata.latitude);
            eventData.Add(key, value);

            key = parameterKeys[20];
            value = Utils.ModifyStringIfEmpty(metadata.longitude);
            eventData.Add(key, value);

            key = parameterKeys[22];
            value = Utils.ModifyStringIfEmpty(metadata.deviceModel);
            eventData.Add(key, value);

            key = parameterKeys[23];
            value = Utils.ModifyStringIfEmpty(metadata.deviceOSVersion);
            eventData.Add(key, value);

            key = parameterKeys[24];
            value = Utils.ModifyStringIfEmpty(metadata.deviceOperatingSystem);
            eventData.Add(key, value);

            key = parameterKeys[25];
            value = Utils.ModifyStringIfEmpty(metadata.deviceScreenDpi);
            eventData.Add(key, value);

            key = parameterKeys[26];
            value = Utils.ModifyStringIfEmpty(metadata.deviceScreenHeight);
            eventData.Add(key, value);

            key = parameterKeys[27];
            value = Utils.ModifyStringIfEmpty(metadata.deviceScreenWidth);
            eventData.Add(key, value);

            key = parameterKeys[28];
            value = Utils.ModifyStringIfEmpty(metadata.sdkVersion);
            eventData.Add(key, value);

            key = parameterKeys[29];
            value = Utils.ModifyStringIfEmpty(metadata.isWifi);
            eventData.Add(key, value);
        }

        /// <summary>
        /// Create function is used for validation of variables of the MikrosSDKAuthEvent class.
        /// </summary>
        internal void Create()
        {
            SetAppGameId();
            SetAppDetails();
            SetMetadata();
            MikrosManager.Instance.AnalyticsController.LogInternalEvent(eventName, eventData, data => { }, exception => { });
        }
    }

    /// <summary>
    /// Log Mikros user profile scoring internal events.
    /// </summary>
    internal class MikrosSDKProfileScoreEvent
    {
        private List<KeyValuePair<string, string>> eventParameterPairs = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("mikros_sdk_spending_score", "spending_score"),
            new KeyValuePair<string, string>("mikros_sdk_activity_score", "activity_score"),
            new KeyValuePair<string, string>("mikros_sdk_tendency_score", "tendency_score"),
            new KeyValuePair<string, string>("mikros_sdk_reputation_score", "reputation_score")
        };

        private KeyValuePair<string, string> eventParameterPair;
        private Hashtable eventData;

        /// <summary>
        /// MikrosSDKProfileScoreEvent private default constructor to restrict object creation of the class.
        /// </summary>
        private MikrosSDKProfileScoreEvent()
        {
        }

        /// <summary>
        /// Parameterised constructor for MikrosSDKProfileScoreEvent object creation.
        /// </summary>
        /// <param name="profileScoreType">Profile score type.</param>
        private MikrosSDKProfileScoreEvent(PROFILE_SCORE_TYPE profileScoreType)
        {
            eventParameterPair = new KeyValuePair<string, string>();
            eventParameterPair = eventParameterPairs[(int)profileScoreType];

            eventData = new Hashtable();
        }

        /// <summary>
        /// Builder function is used for creating an object of MikrosSDKProfileScoreEvent class.
        /// </summary>
        /// <param name="profileScoreType">Profile score type.</param>
        /// <returns>MikrosSDKProfileScoreEvent object.</returns>
        internal static MikrosSDKProfileScoreEvent Builder(PROFILE_SCORE_TYPE profileScoreType)
        {
            return new MikrosSDKProfileScoreEvent(profileScoreType);
        }

        /// <summary>
        /// Set the score.
        /// </summary>
        /// <param name="score">Score of a specific type.</param>
        /// <returns>MikrosSDKProfileScoreEvent object.</returns>
        internal MikrosSDKProfileScoreEvent SetScore(int score)
        {
            eventData.Add(eventParameterPair.Value, score);

            return this;
        }

        /// <summary>
        /// Create function is used for validation of variables of the MikrosSDKProfileScoreEvent class.
        /// </summary>
        internal void Create()
        {
            MikrosManager.Instance.AnalyticsController.LogInternalEvent(eventParameterPair.Key, eventData, data => { }, exception => { });
        }
    }

    /// <summary>
    /// Log custom UI navigation related internal events.
    /// </summary>
    internal class MikrosSDKPortalEvent
    {
        private List<string> eventNames = new List<string>()
        {
            "mikros_sdk_portal_open",
            "mikros_sdk_portal_close",
            "mikros_sdk_portal_app_click_details",
            "mikros_sdk_portal_app_redirect",
            "mikros_sdk_portal_app_download",
            "mikros_sdk_portal_nagivate_featured",
            "mikros_sdk_portal_navigate_games",
            "mikros_sdk_portal_navigate_profile",
            "mikros_sdk_portal_navigate_offerwall",
            "mikros_sdk_portal_navigate_friends",
            "mikros_sdk_portal_navigate_subscreen_justtoofun",
            "mikros_sdk_portal_navigate_subscreen_tatumgames_favorites",
            "mikros_sdk_portal_navigate_subscreen_apps_development",
            "mikros_sdk_portal_navigate_subscreen_casual_gamer",
            "mikros_sdk_portal_navigate_subscreen_core_gamer"
        };

        private List<string> parameterKeys = new List<string>()
        {
            "app_game_id", "time_on_screen"
        };

        private string eventName;
        private SDK_PORTAL_EVENT_TYPE portalEventType;
        private Hashtable eventData;

        /// <summary>
        /// MikrosSDKPortalEvent private default constructor to restrict object creation of the class.
        /// </summary>
        private MikrosSDKPortalEvent()
        {
        }

        /// <summary>
        /// Parameterised constructor for MikrosSDKPortalEvent object creation.
        /// </summary>
        /// <param name="portalEventType">Custom UI portal event type.</param>
        private MikrosSDKPortalEvent(SDK_PORTAL_EVENT_TYPE portalEventType)
        {
            eventName = eventNames[(int)portalEventType];
            this.portalEventType = portalEventType;

            eventData = new Hashtable();
        }

        /// <summary>
        /// Builder function is used for creating an object of MikrosSDKPortalEvent class.
        /// </summary>
        /// <param name="portalEventType">Custom UI portal event type.</param>
        /// <returns></returns>
        internal static MikrosSDKPortalEvent Builder(SDK_PORTAL_EVENT_TYPE portalEventType)
        {
            return new MikrosSDKPortalEvent(portalEventType);
        }

        /// <summary>
        /// Set App Game ID.
        /// </summary>
        private void SetAppGameId()
        {
            string appGameId = Utils.ModifyStringIfEmpty(MikrosManager.Instance.ConfigurationController.MikrosSettings.AppGameID);
            eventData.Add(parameterKeys[0], appGameId);
        }

        /// <summary>
        /// Set total time spent on custom UI.
        /// </summary>
        /// <param name="timeInSeconds">Time in seconds spent on custom UI.</param>
        /// <returns>MikrosSDKPortalEvent object.</returns>
        internal MikrosSDKPortalEvent SetTimeOnScreen(int timeInSeconds)
        {
            if ((int)portalEventType < 4)
                return this;

            eventData.Add(parameterKeys[1], timeInSeconds);

            return this;
        }

        /// <summary>
        /// Create function is used for validation of variables of the MikrosSDKPortalEvent class.
        /// </summary>
        internal void Create()
        {
            SetAppGameId();
            MikrosManager.Instance.AnalyticsController.LogInternalEvent(eventName, eventData, data => { }, exception => { });
        }
    }

    /// <summary>
    /// Log internal exception events.
    /// </summary>
    internal class MikrosSDKExceptionEvent
    {
        private string eventName = "mikros_sdk_handled_exception";

        private List<string> parameterKeys = new List<string>()
        {
            "app_game_id",
            "api_key_type",
            "app_version",
            "sdk_version",
            "platform",
            "exception_type",
            "exception_message",
            "timestamp"
        };

        private Hashtable eventData;

        /// <summary>
        /// Constructor for MikrosSDKExceptionEvent object creation.
        /// </summary>
        private MikrosSDKExceptionEvent()
        {
            eventData = new Hashtable();
        }

        /// <summary>
        /// Builder function is used for creating an object of MikrosSDKExceptionEvent class.
        /// </summary>
        /// <returns>MikrosSDKExceptionEvent object.</returns>
        internal static MikrosSDKExceptionEvent Builder()
        {
            return new MikrosSDKExceptionEvent();
        }

        /// <summary>
        /// Set Metadata.
        /// </summary>
        private void SetMetadata()
        {
            string appGameId = Utils.ModifyStringIfEmpty(MikrosManager.Instance.ConfigurationController.MikrosSettings.AppGameID);
            eventData.Add(parameterKeys[0], appGameId);

            string apiKeyType = Utils.ModifyStringIfEmpty(MikrosManager.Instance.ConfigurationController.MikrosSettings.GetApiKeyTypeString());
            eventData.Add(parameterKeys[1], apiKeyType);

            string appVersion = Utils.ModifyStringIfEmpty(Application.version);
            eventData.Add(parameterKeys[2], appVersion);

            string sdkVersion = Utils.ModifyStringIfEmpty(Constants.SDKVersion);
            eventData.Add(parameterKeys[3], sdkVersion);

            string platform = Utils.ModifyStringIfEmpty(Utils.GetCurrentPlatform());
            eventData.Add(parameterKeys[4], platform);
        }

        /// <summary>
        /// Set details of the exception.
        /// </summary>
        /// <param name="errorMessage">Error message of the exception.</param>
        /// <returns>MikrosSDKExceptionEvent object.</returns>
        internal MikrosSDKExceptionEvent SetErrorDetails(string errorMessage)
        {
            string errorType = Utils.ModifyStringIfEmpty("caught");
            eventData.Add(parameterKeys[5], errorType);

            string message = Utils.ModifyStringIfEmpty(errorMessage);
            eventData.Add(parameterKeys[6], message);

            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            return this;
        }

        /// <summary>
        /// Create function is used for validation of variables of the MikrosSDKExceptionEvent class.
        /// </summary>
        internal void Create()
        {
            SetMetadata();
            MikrosManager.Instance.AnalyticsController.LogInternalEvent(eventName, eventData, data => { }, exception => { });
        }
    }

    /// <summary>
    /// Log internal event for API request failure.
    /// </summary>
    internal class MikrosSDKApiFailureEvent
    {
        private string eventName = "mikros_sdk_api_failure";

        private List<string> parameterKeys = new List<string>()
        {
            "app_game_id",
            "url",
            "status_code",
            "error_type",
            "error_message",
            "response_time"
        };

        private Hashtable eventData;

        /// <summary>
        /// Constructor for MikrosSDKApiFailureEvent object creation.
        /// </summary>
        private MikrosSDKApiFailureEvent()
        {
            eventData = new Hashtable();
        }

        /// <summary>
        /// Builder function is used for creating an object of MikrosSDKApiFailureEvent class.
        /// </summary>
        /// <returns>MikrosSDKApiFailureEvent object.</returns>
        internal static MikrosSDKApiFailureEvent Builder()
        {
            return new MikrosSDKApiFailureEvent();
        }

        /// <summary>
        /// Set App Game ID.
        /// </summary>
        private void SetAppGameId()
        {
            string appGameId = Utils.ModifyStringIfEmpty(MikrosManager.Instance.ConfigurationController.MikrosSettings.AppGameID);
            eventData.Add(parameterKeys[0], appGameId);
        }

        /// <summary>
        /// Set URL of the web request.
        /// </summary>
        /// <param name="url">URL of the web request.</param>
        /// <returns>MikrosSDKApiFailureEvent object.</returns>
        internal MikrosSDKApiFailureEvent SetUrl(string url)
        {
            eventData.Add(parameterKeys[1], url);

            return this;
        }

        /// <summary>
        /// Set status code of the web request.
        /// </summary>
        /// <param name="statusCode">Status code of the web request.</param>
        /// <returns>MikrosSDKApiFailureEvent object.</returns>
        internal MikrosSDKApiFailureEvent SetStatusCode(int statusCode)
        {
            eventData.Add(parameterKeys[2], statusCode);

            return this;
        }

        /// <summary>
        /// Set details of the web request error.
        /// </summary>
        /// <param name="errorMessage">Error message of the web request.</param>
        /// <returns>MikrosSDKApiFailureEvent object.</returns>
        internal MikrosSDKApiFailureEvent SetErrorDetails(string errorMessage)
        {
            string errorType = Utils.ModifyStringIfEmpty(Constants.Unhandled);
            eventData.Add(parameterKeys[3], errorType);

            string message = Utils.ModifyStringIfEmpty(errorMessage);
            eventData.Add(parameterKeys[4], message);

            return this;
        }

        /// <summary>
        /// Set response time of the web request.
        /// </summary>
        /// <param name="timeInSeconds">Response time of the web request in seconds.</param>
        /// <returns>MikrosSDKApiFailureEvent object.</returns>
        internal MikrosSDKApiFailureEvent SetResponseTime(float timeInSeconds)
        {
            eventData.Add(parameterKeys[5], timeInSeconds);

            return this;
        }

        /// <summary>
        /// Create function is used for validation of variables of the MikrosSDKApiFailureEvent class.
        /// </summary>
        internal void Create()
        {
            SetAppGameId();
            MikrosManager.Instance.AnalyticsController.LogInternalEvent(eventName, eventData, data => { }, exception => { });
        }
    }

    /// <summary>
    /// Log internal event for API request success.
    /// </summary>
    internal class MikrosSDKApiSuccessEvent
    {
        private string eventName = "mikros_sdk_api_success";

        private List<string> parameterKeys = new List<string>()
        {
            "app_game_id",
            "url",
            "status_code",
            "status_message",
            "response_time"
        };

        private Hashtable eventData;

        /// <summary>
        /// Constructor for MikrosSDKApiSuccessEvent object creation.
        /// </summary>
        private MikrosSDKApiSuccessEvent()
        {
            eventData = new Hashtable();
        }

        /// <summary>
        /// Builder function is used for creating an object of MikrosSDKApiSuccessEvent class.
        /// </summary>
        /// <returns>MikrosSDKApiSuccessEvent object.</returns>
        internal static MikrosSDKApiSuccessEvent Builder()
        {
            return new MikrosSDKApiSuccessEvent();
        }

        /// <summary>
        /// Set App Game ID.
        /// </summary>
        private void SetAppGameId()
        {
            string appGameId = Utils.ModifyStringIfEmpty(MikrosManager.Instance.ConfigurationController.MikrosSettings.AppGameID);
            eventData.Add(parameterKeys[0], appGameId);
        }

        /// <summary>
        /// Set URL of the web request.
        /// </summary>
        /// <param name="url">URL of the web request.</param>
        /// <returns>MikrosSDKApiSuccessEvent object.</returns>
        internal MikrosSDKApiSuccessEvent SetUrl(string url)
        {
            eventData.Add(parameterKeys[1], url);

            return this;
        }

        /// <summary>
        /// Set status code of the web request.
        /// </summary>
        /// <param name="statusCode">Status code of the web request.</param>
        /// <returns>MikrosSDKApiSuccessEvent object.</returns>
        internal MikrosSDKApiSuccessEvent SetStatusCode(int statusCode)
        {
            eventData.Add(parameterKeys[2], statusCode);

            return this;
        }

        /// <summary>
        /// Set message of the status of the web request.
        /// </summary>
        /// <param name="statusMessage">Message of the status of the web request.</param>
        /// <returns>MikrosSDKApiSuccessEvent object.</returns>
        internal MikrosSDKApiSuccessEvent SetStatusMessage(string statusMessage)
        {
            eventData.Add(parameterKeys[3], statusMessage);

            return this;
        }

        /// <summary>
        /// Set response time of the web request.
        /// </summary>
        /// <param name="timeInSeconds">Response time of the web request in seconds.</param>
        /// <returns>MikrosSDKApiSuccessEvent object.</returns>
        internal MikrosSDKApiSuccessEvent SetResponseTime(float timeInSeconds)
        {
            eventData.Add(parameterKeys[4], timeInSeconds);

            return this;
        }

        /// <summary>
        /// Create function is used for validation of variables of the MikrosSDKApiSuccessEvent class.
        /// </summary>
        internal void Create()
        {
            SetAppGameId();
            MikrosManager.Instance.AnalyticsController.LogInternalEvent(eventName, eventData, data => { }, exception => { });
        }
    }
}