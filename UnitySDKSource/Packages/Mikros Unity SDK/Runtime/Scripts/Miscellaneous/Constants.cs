using UnityEngine;

namespace MikrosClient
{
    internal sealed class Constants
    {
        internal const string Mikros = "MIKROS";

        internal static readonly string SDKVersion = "1.2.0";

        internal const string MikrosManager = "Mikros Manager";

        internal const string FileEncryptKey = "WoGIrrX6GEA9fHaD5cahucLG88OHWlOK";

        internal const string AuthDataFileName = "5UHAHr19h6iv7js2";

        internal const string MikrosSavedAccountsFileName = "zKf8PICWw4wyyIi7";

        internal const string AppPauseInfoKey = "APP_PAUSE_INFO";

        internal const string SuccessMessageGeneric = "SUCCESS";

        internal const string ErrorMessageGeneric = "Unrecognized Error Occured";

        internal const string StackTraceErrorHeader = "Stack trace error: ";

        internal const string SSOErrorHeader = "SSO Exception: ";

        internal const string ApiKeyHeaderKey = "X-Apikey";

        internal static readonly string DeviceID = SystemInfo.deviceUniqueIdentifier;

        internal static readonly string DeviceModel = SystemInfo.deviceName;

        internal static readonly string DeviceOSVersion = SystemInfo.operatingSystem;

        internal static readonly string ScreenDPI = Screen.dpi.ToString();

        internal static readonly string ScreenWidth = Screen.width.ToString();

        internal static readonly string ScreenHeight = Screen.height.ToString();

        internal static readonly bool IsNetworkAvailable = !Equals(Application.internetReachability, NetworkReachability.NotReachable);

        internal static readonly string IsWifi = IsNetworkAvailable ? "1" : "0";

        internal static readonly string Handled = "handled";

        internal static readonly string Unhandled = "unhandled";

        internal static readonly string MainThread = "main";

        internal const string NoNetworkError = "No network connection. We are unable to process your request.";

        internal const string MikrosAppStoreError = "There must be an active user session to access the Mikros community app store. Go to the following link to learn more details:\n<color=blue>https://developer.tatumgames.com/docs/market</color>";

        internal const string MetadataTrackingDisableError = "User Metadata tracking is disabled.";

        internal const string MetadataExistsMessage = "User Metadata has already been uploaded.";

        internal const string MetadataSuccessMessage = "Metadata Upload Success.";

        internal const string MetadataFailureMessage = "Metadata Upload Failure.";

        #region AuthenticationPanel

        internal const string AuthenticationPanelPrefabName = "AuthenticationPanel";

        internal const string CroutonPrefabName = "Crouton";

        internal const string ValueRequiredMessage = "This value is required";

        internal const string InappropriateLanguageError = "Inappropriate language used";

        internal const string ExistingSessionSignoutError = "Unable to sign out from existing session";

        internal const string AuthGeneralError = "Sorry, an error occurred. Please email <color=blue>support@tatumgames.com</color> or <color=blue>https://tatumgames.com/contact-us/</color>";

        internal static readonly string LegalDescriptionText = "To continue, Tatum Games will share your profile information such as your name, email address, profile picture with " + Application.productName + ".";

        internal static readonly string ChooseAccountSubtitleText = "to continue to " + Application.productName;

        #endregion AuthenticationPanel

        internal const string LoadingPanelPrefabName = "LoadingPanel";

        internal const string PopupPanelPrefabName = "PopupPanel";

        internal const string MikrosUiCanvasPrefabName = "MikrosUiCanvas";

        internal const string MikrosAppStorePrefabName = "MikrosAppStore";

        internal const string MikrosSettingsAssetName = "Mikros Settings";

        internal const string MikrosLogoAssetName = "MikrosLogo";

        internal const string ContentTypeKey = "Content-Type";

        internal const string AcceptTypeKey = "Accept";

        internal const string JsonWebContent = "application/json";

        internal const string EventsKey = "events";

        internal const string EventNameKey = "eventName";

        internal const string TimestampKey = "timestamp";

        internal const string MikrosInitializeWarning = "Canceling initialize process, since already initialized successfully.";

        #region EditorConstants

        internal const string EditorHashTable = "EditorHashTable";

        internal const int EditorCustomEventsBatchCount = 10;

        internal const string EditorCustomEventAddMessage = "Custom Events added from Editor.";

        #endregion EditorConstants

        #region NativeFrameworkMethods

        internal const string GetInstance = "getInstance";

        internal const string Initialize = "initialize";

        internal const string SetApiKey = "setApiKey";

        internal const string SetBaseUrl = "setBaseUrl";

        internal const string SetAppGameId = "setAppGameId";

        internal const string SetApiKeyType = "setApiKeyType";

        internal const string SetDeviceId = "setDeviceId";

        internal const string SetAppVersion = "setAppVersion";

        internal const string SetSdkVersion = "setSdkVersion";

        internal const string SetLatitude = "setLatitude";

        internal const string SetLongitude = "setLongitude";

        internal const string SetDeviceModel = "setDeviceModel";

        internal const string SetDeviceOS = "setDeviceOS";

        internal const string SetDeviceOSVersion = "setDeviceOSVersion";

        internal const string SetDeviceScreenDpi = "setDeviceScreenDpi";

        internal const string SetDeviceScreenHeight = "setDeviceScreenHeight";

        internal const string SetDeviceScreenWidth = "setDeviceScreenWidth";

        internal const string SetSdkType = "setSdkType";

        internal const string SetIsWifi = "setIsWifi";

        internal const string SetEventLogging = "setEventLogging";

        internal const string UpdateSessionLogging = "updateSessionLogging";

        internal const string UpdateMemoryLogging = "updateMemoryLogging";

        internal const string UpdateEventLogging = "updateEventLogging";

        internal const string UpdateUserMetadata = "updateUserMetadata";

        internal const string Create = "create";

        internal const string SetEvent = "setEvent";

        internal const string LogEvent = "logEvent";

        internal const string FlushEvents = "flushEvents";

        internal const string OnMotionEvent = "onMotionEvent";

        internal const string StageAppGameId = "tg-test";

        #endregion NativeFrameworkMethods

        #region CustomUI

        internal const string JustForFunCategoryDescription = "Guaranteed fun to play";

        internal const string FavoritesCategoryDescription = "Recommended by Mikros";

        internal const string CasualCategoryDescription = "Hyper-casual, casual games";

        internal const string CompetitiveCategoryDescription = "Competitive games";

        internal const string InDevelopmentCategoryDescription = "Look out for these games in future";

        #endregion CustomUI

        #region MikrosException

        internal const string MikrosExceptionHeader = "Exception occured:\n";

        internal const string InappropriateInputError = "Input contains profanity or inappropriate words.";

        internal const string InvalidParameterError = "One or more mandatory parameters has null or invalid values.";

        internal const string InvalidUsernameError = "Provided username is invalid.";

        internal const string InvalidEmailError = "Provided email is invalid.";

        internal const string InvalidPasswordError = "Provided password is invalid.";

        internal const string InvalidAccessTokenError = "Invalid Access Token.";

        internal const string MikrosInitializeSDKError = "Mikros SDK not initialized.";

        internal const string ObjectNotCreatedError = "Request parameter object not created.";

        internal const string InvalidGameIDError = "Invalid App Game ID.";

        internal const string InvalidAPIKeyError = "Invalid API Key passed.";

        internal const string SerializationError = "Error occurred during serialization process.";

        internal const string DeserializationError = "Error occurred during de-serialization process.";

        internal const string EventLoggingDisableError = "Event logging disabled.";

        internal const string AllTrackingDisableError = "All tracking is currently disabled. This includes event tracking, session tracking, and metadata tracking.";

        internal const string DefaultError = "Error Occurred!";

        #endregion MikrosException

        #region MikrosSettings

        internal const string AppGameID = "App Game ID";
        internal const string ApiKeyProduction = "API Key for Production";
        internal const string ApiKeyQA = "API Key for QA (Optional)";
        internal const string ApiKeyDev = "API Key for Development (Optional)";
        internal const string ApiKeyMikrosSettings = "Set which API Key to use";
        internal const string AutoInitializeMikros = "Set if you want to initialize Mikros automatically at start or handle it manually\nIts recommended to keep this Enabled";
        internal const string AutoTrackSessionMikros = "Set if you want to track session of end-user\nIts recommended to keep this Enabled";
        internal const string AutoTrackMetadataMikros = "Set if you want to enable Metadata tracking for improved user insights\nIts recommended to keep this Enabled";
        internal const string EventLogging = "Set if you want to enable Event Logging\nIts recommended to keep this Enabled";
        internal const string CrashReporting = "Set if you want to enable Crash Reporting\nIts recommended to keep this Enabled";
        internal const string TrackDeviceMemory = "Set if you want to enable Device Memory Tracking\nIts recommended to keep this Enabled";
        internal const string EnableUserNameSpecialCharacters = "Set if you want to enable Enable Username Special Characters.";

        internal const string Production = "prod";
        internal const string Qa = "qa";
        internal const string Development = "dev";

        #endregion MikrosSettings

        #region AnalyticsControllers

        internal const string FlushedEventSuccessful = "Flushed events successful.";
        internal const string CustomEventSuccessful = "Custom Event logging successful.";
        internal const string CustomEventData = "Custom event data";
        internal const string ExceptionCustom = "Exception custom";
        internal const string CustomJson = "Custom JSON";

        #endregion AnalyticsControllers
    }
}
