using System;

namespace MikrosClient
{
    /// <summary>
    /// Url Type enum for different types of url path.
    /// </summary>
    [Serializable]
    internal enum UrlType
    {
        BaseURL,
        SDKInitialization,
        PresetAnalytics,
        CustomAnalytics,
        SignIn,
        SignUp,
        SignOut,
        UpdateMetaData,
        GetAllApps,
        SessionTrack,
        PlayerRating
    }

    /// <summary>
    /// This enum is for various status types of any API call.
    /// </summary>
    [Serializable]
    public enum STATUS_TYPE
    {
        SUCCESS = 2,
        ERROR = 4,
        UNKNOWN
    }

    /// <summary>
    /// Enum for API key type.
    /// </summary>
    [Serializable]
    public enum API_KEY_TYPE
    {
        PRODUCTION,
        QA,
        DEVELOPMENT
    }

    /// <summary>
    /// Logging Tag for a log.
    /// </summary>
    [Serializable]
    internal enum LogTag
    {
        REQ_TAG,
        RES_TAG,
        URL_TAG
    }

    /// <summary>
    /// Enum for the auth type.
    /// </summary>
    [Serializable]
    internal enum AUTH_TYPE
    {
        SIGNUP,
        SIGNIN,
        SIGNOUT
    }

    /// <summary>
    /// Enum for type of Preset Event.
    /// </summary>
    [Serializable]
    public enum EVENT
    {
        APP_OPEN,
        GAME_OVER,
        HANDLED_EXCEPTION,
        HTTP_FAILURE,
        HTTP_SUCCESS,
        LEVEL_END,
        LEVEL_START,
        LEVEL_UP,
        POST_SCORE,
        SHARE,
        SIGNIN,
        SIGNUP,
        START_TIMER,
        STOP_TIMER,
        TUTORIAL_BEGIN,
        TUTORIAL_COMPLETE,
        UNHANDLED_EXCEPTION,
        UNLOCK_ACHIEVEMENT,
        TRACK_SCREEN,
        TRACK_PURCHASE
    }

    /// <summary>
    /// Enumerations of all possible behaviour type.
    /// </summary>
    [Serializable]

    public enum PlayerBehavior
    {
        POOR_SPORTSMANSHIP,
        TROLLING,
        CONSTANT_PINGING,
        AFK,
        COMPLAINING,
        OFFENSIVE_LANGUAGE,
        CHEATING,
        GOOD_SPORTSMANSHIP,
        GREAT_LEADERSHIP,
        EXCELLENT_TEAMMATE,
        MVP
    }

    /// <summary>
    /// ExceptionType enum for custom exception logging.
    /// </summary>
    [Serializable]
    public enum ExceptionType
    {
        INAPPROPRIATE_INPUT,
        INVALID_PARAMETER,
        INVALID_USERNAME,
        INVALID_EMAIL,
        INVALID_PASSWORD,
        INVALID_ACCESS_TOKEN,
        INITIALIZE_MIKROS_SDK,
        OBJECT_NOT_CREATED,
        INVALID_APP_GAME_ID,
        INVALID_API_KEY,
        SERIALIZATION,
        DESERIALIZATION,
        DISABLED_EVENT_LOGGING,
        DISABLED_ALL_TRACKING,
        OTHER
    }

    /// <summary>
    /// Enum for Mikros users' score type.
    /// </summary>
    [Serializable]
    internal enum PROFILE_SCORE_TYPE
    {
        SPENDING,
        ACTIVITY,
        TENDENCY,
        REPUTATION
    }

    /// <summary>
    /// Enum for internal event types for navigating Mikros custom UI.
    /// </summary>
    [Serializable]
    internal enum SDK_PORTAL_EVENT_TYPE
    {
        OPEN,
        CLOSE,
        APP_CLICK_DETAILS,
        APP_REDIRECT,
        APP_DOWNLOAD,
        NAVIGATE_FEATURED,
        NAVIGATE_GAMES,
        NAVIGATE_PROFILE,
        NAVIGATE_OFFERWALL,
        NAVIGATE_FRIENDS,
        NAVIGATE_SUBSCREEN_JUSTTOOFUN,
        NAVIGATE_SUBSCREEN_TATUMGAMES_FAVORITES,
        NAVIGATE_SUBSCREEN_APPS_DEVELOPMENT,
        NAVIGATE_SUBSCREEN_CASUAL_GAMER,
        NAVIGATE_SUBSCREEN_CORE_GAMER
    }

    /// <summary>
    /// Enumeration for sub-type of exception.
    /// </summary>
    [Serializable]
    public enum EXCEPTION_SUB_TYPE
    {
        HANDLED,
        UNHANDLED
    }

    /// <summary>
    /// Enum for Mikros button placement style.
    /// </summary>
    [Serializable]
    public enum MIKROS_BUTTON_STYLE
    {
        STATIC,
        FLOATING
    }

    /// <summary>
    /// Enumeration of all privacy levels.
    /// </summary>
    [Serializable]
    public enum PRIVACY_LEVEL
    {
        /// <summary>
        /// (Recommended) Mikros tracks user metadata and session activity in the background.
        /// This is GDPR & CCPA compliant.
        /// </summary>
        DEFAULT = 0,

        /// <summary>
        /// Mikros no longer tracks any metadata information in the background; only user session activity is tracked.
        /// This is GDPR & CCPA compliant.
        /// </summary>
        HIGH = 1,

        /// <summary>
        /// Mikros no longer tracks any metadata or user session activity in the background. Integrators will have to track manually.
        /// This is GDPR & CCPA compliant.
        /// </summary>
        EXTREME = 2
    }
}