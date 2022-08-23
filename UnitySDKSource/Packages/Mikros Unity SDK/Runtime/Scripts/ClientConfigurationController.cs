using UnityEngine;

namespace MikrosClient.Config
{
    /// <summary>
    /// Handles all configuration operation of Mikros.
    /// </summary>
    public sealed class ConfigurationController
    {
        /// <summary>
        /// Reference of the Configuration.
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// Mikros Settings data that contains app game ID path and api keys.
        /// </summary>
        public MikrosSettings MikrosSettings { get; private set; }

        /// <summary>
        /// Returns true if auto-tracking of user session is enabled, else false.
        /// </summary>
        public bool IsTrackUserSession => configuration.isTrackUserSession;

        /// <summary>
        /// Returns true if auto-tracking of user metadata is enabled, else false.
        /// </summary>
        public bool IsTrackUserMetadata => configuration.isTrackUserMetadata;

        /// <summary>
        /// Returns true if event logging is enabled, else false.
        /// </summary>
        public bool IsEventLogging => configuration.isEventLogging;

        /// <summary>
        /// Returns true if crash reporting is enabled, else false.
        /// </summary>
        public bool IsCrashReporting => configuration.isCrashReportingEnabled;

        /// <summary>
        /// Returns true if Track Device Memory is enabled, else false.
        /// </summary>
        public bool IsTrackDeviceMemory => configuration.isTrackDeviceMemory;
        /// <summary>
        /// Returns true if user's all activity tracking is enabled, else false.
        /// </summary>
        public bool IsAllTrackingEnabled => configuration.isAllTrackingEnabled;

        /// <summary>
        /// Object creation of this class only allowed internally.
        /// </summary>
        internal ConfigurationController()
        { }

        /// <summary>
        /// Load the Mikros Settings from resources file.
        /// </summary>
        internal void LoadMikrosSettings()
        {
            MikrosSettings = Resources.Load<MikrosSettings>(Constants.MikrosSettingsAssetName);
            if (MikrosSettings == null)
            {
                throw new MikrosException(ExceptionType.OTHER, "Initial Mikros setup file missing");
            }
            if (string.IsNullOrEmpty(MikrosSettings.AppGameID) || string.IsNullOrEmpty(MikrosSettings.GetCurrentApiKey()))
            {
                throw new MikrosException(ExceptionType.OTHER, "Mikros is not set up properly");
            }
        }

        /// <summary>
        /// Set up the configuration.
        /// </summary>
        /// <param name="configuration">Client configuration object.</param>
        internal void SetConfiguration(Configuration configuration)
        {
            if (configuration != null)
            {
                this.configuration = configuration;
            }
            else
            {
                // initialize the Configuration object with configurations setup from MikrosSettings.
                this.configuration = Configuration.Builder();               
                this.configuration
                    .SetSessionTracking(MikrosSettings.IsAutoTrackSession)
                    .SetMetadataTracking(MikrosSettings.IsAutoTrackMetadata)
                    .SetEventLogging(MikrosSettings.IsEventLogging)
                    .SetCrashReporting(MikrosSettings.IsCrashReporting)
                    .SetTrackDeviceMemory(MikrosSettings.IsTrackDeviceMemory)
                    .Create();
            }
        }

        /// <summary>
        /// Sets auto-tracking status of user session.
        /// </summary>
        /// <param name="isTrackUserSession">User session data auto-track status.</param>
        public void SetAutoTrackUserSession(bool isAutoTrackSession)
        {
            configuration.SetAutoTrackUserSession(isAutoTrackSession);
        }

        /// <summary>
        /// Sets auto-tracking status of user metadata.
        /// </summary>
        /// <param name="isTrackUserMetadata">User metadata auto-track status.</param>
        public void SetAutoTrackUserMetadata(bool isAutoTrackMetadata)
        {
            configuration.SetAutoTrackUserMetadata(isAutoTrackMetadata);
        }

        /// <summary>
        /// Sets crash reporting status.
        /// </summary>
        /// <param name="isCrashReporting">Crash reporting status.</param>
        public void SetAutoCrashReporting(bool isCrashReporting)
        {
            configuration.SetAutoCrashReporting(isCrashReporting);
        }

        /// <summary>
        /// Set event logging status of user.
        /// </summary>
        /// <param name="isEventLogging">Status of user's event logging.</param>
        public void SetEventLogging(bool isEventLogging)
        {
            configuration.CustomizeEventLoggingStatus(isEventLogging);
        }

        /// <summary>
        /// Sets track device memory status.
        /// </summary>
        /// <param name="isTrackDeviceMemory">Device Memory tracking status.</param>
        public void SetAutoTrackDeviceMemory(bool isTrackDeviceMemory)
        {
            configuration.SetAutoTrackDeviceMemory(isTrackDeviceMemory);
        }

        /// <summary>
        /// Set whether to enable tracking of user's every activity.
        /// </summary>
        /// <param name="isAllTrackingEnabled">Status of user's every activity tracking.</param>
        public void SetAllTrackingEnabled(bool isAllTrackingEnabled)
        {
            configuration.SetAllTrackingEnabled(isAllTrackingEnabled);
        }
    }
}
