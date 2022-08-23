using UnityEngine;
using MikrosClient.NativeFramework;

namespace MikrosClient.Config
{
    /// <summary>
    /// Client's configurations data structure.
    /// </summary>
    public sealed class Configuration
    {
        #region Parameters

        internal bool isTrackUserSession = true;
        internal bool isTrackUserMetadata = true;
        internal bool isEventLogging = true;
        internal bool isAllTrackingEnabled = true;
        internal bool isCrashReportingEnabled = true;
        internal bool isTrackDeviceMemory = true;

        #endregion Parameters

        private PRIVACY_LEVEL privacyLevel = PRIVACY_LEVEL.DEFAULT;

        /// <summary>
        /// UserPrivacyConfiguration private constructor to restrict object creation of the class from outside.
        /// </summary>
        private Configuration()
        {
        }

        /// <summary>
        /// Builder function return a new object of the Configuration class.
        /// </summary>
        /// <returns>New Configuration object.</returns>
        public static Configuration Builder()
        {
            return new Configuration();
        }

        /// <summary>
        /// Set privacy level.
        /// </summary>
        /// <param name="privacyLevel">Privacy level type.</param>
        /// <returns>Configuration object.</returns>
        public Configuration SetPrivacyLevel(PRIVACY_LEVEL privacyLevel)
        {
            this.privacyLevel = privacyLevel;
            isTrackUserSession = this.privacyLevel != PRIVACY_LEVEL.EXTREME;
            isTrackUserMetadata = this.privacyLevel == PRIVACY_LEVEL.DEFAULT;
            isEventLogging = this.privacyLevel != PRIVACY_LEVEL.EXTREME;
            isCrashReportingEnabled = this.privacyLevel != PRIVACY_LEVEL.EXTREME;
            isTrackDeviceMemory = this.privacyLevel != PRIVACY_LEVEL.EXTREME;
            return this;
        }

        /// <summary>
        /// Set session tracking status of user.
        /// </summary>
        /// <param name="isTrackUserSession">Status of user's session tracking.</param>
        /// <returns>Configuration object.</returns>
        public Configuration SetSessionTracking(bool isTrackUserSession)
        {
            this.isTrackUserSession = isTrackUserSession;
            return this;
        }

        /// <summary>
        /// Set metaData tracking status of user.
        /// </summary>
        /// <param name="isTrackUserMetadata">Status of user's metaData tracking.</param>
        /// <returns>Configuration object.</returns>
        public Configuration SetMetadataTracking(bool isTrackUserMetadata)
        {
            this.isTrackUserMetadata = isTrackUserMetadata;
            return this;
        }

        /// <summary>
        /// Set event logging status of user.
        /// </summary>
        /// <param name="isEventLogging">Status of user's event logging.</param>
        /// <returns>Configuration object.</returns>
        public Configuration SetEventLogging(bool isEventLogging)
        {
            this.isEventLogging = isEventLogging;
            return this;
        }

        /// <summary>
        /// Set crash reporting status of user.
        /// </summary>
        /// <param name="isCrashReporting">Status of user's crash reporting.</param>
        /// <returns>Configuration object.</returns>
        public Configuration SetCrashReporting(bool isCrashReporting)
        {
            this.isCrashReportingEnabled = isCrashReporting;
            return this;
        }

        /// <summary>
        /// Set trackDeviceMemory status of user.
        /// </summary>
        /// <param name="isTrackDeviceMemory">Status of user's track device memory.</param>
        /// <returns>Configuration object.</returns>
        public Configuration SetTrackDeviceMemory(bool isTrackDeviceMemory)
        {
            this.isTrackDeviceMemory = isTrackDeviceMemory;
            return this;
        }


        /// <summary>
        /// Create function is used for validation of variables of the UserPrivacyConfiguration class.
        /// </summary>
        /// <returns>Configuration object.</returns>
        public Configuration Create()
        {
            return this;
        }

        /// <summary>
        /// Sets auto-tracking status of user device memory
        /// </summary>
        /// <param name="isTrackDeviceMemory">User device memory auto-track status.</param>
        internal void SetAutoTrackDeviceMemory(bool isTrackDeviceMemory)
        {
            this.isTrackDeviceMemory = isTrackDeviceMemory;
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject mikrosApiClient = MikrosManager.Instance.mikrosApiClientProvider.CallStaticMethod<AndroidJavaObject>(Constants.GetInstance);
			mikrosApiClient.Call(Constants.UpdateMemoryLogging, isTrackDeviceMemory);

#elif UNITY_IOS && !UNITY_EDITOR
			SwiftForUnity.UpdateMemoryLoggingNatively(isTrackDeviceMemory);
#endif
        }

        /// <summary>
        /// Sets auto-tracking status of user session
        /// </summary>
        /// <param name="isTrackUserSession">User session data auto-track status.</param>
        internal void SetAutoTrackUserSession(bool isTrackUserSession)
        {
            this.isTrackUserSession = isTrackUserSession;
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject mikrosApiClient = MikrosManager.Instance.mikrosApiClientProvider.CallStaticMethod<AndroidJavaObject>(Constants.GetInstance);
			mikrosApiClient.Call(Constants.UpdateSessionLogging, isTrackUserSession);

#elif UNITY_IOS && !UNITY_EDITOR
			SwiftForUnity.UpdateSessionLoggingNatively(isTrackUserSession);
#endif
        }

        /// <summary>
        /// Sets auto-tracking status of user metadata.
        /// </summary>
        /// <param name="isTrackUserMetadata">User metadata auto-track status.</param>
        internal void SetAutoTrackUserMetadata(bool isTrackUserMetadata)
        {
            this.isTrackUserMetadata = isTrackUserMetadata;
        }

        /// <summary>
        /// Customize the event logging status of user.
        /// </summary>
        /// <param name="isEventLogging">Status of user's event logging.</param>
        internal void CustomizeEventLoggingStatus(bool isEventLogging)
        {
            this.isEventLogging = isEventLogging;
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaObject mikrosApiClient = MikrosManager.Instance.mikrosApiClientProvider.CallStaticMethod<AndroidJavaObject>(Constants.GetInstance);
            mikrosApiClient.Call(Constants.UpdateEventLogging, isEventLogging);

#elif UNITY_IOS && !UNITY_EDITOR
            SwiftForUnity.UpdateEventLoggingNatively(isEventLogging);
#endif
        }

        /// <summary>
        /// Sets crash reporting status.
        /// </summary>
        /// <param name="isCrashReporting">Crash reporting status.</param>
        internal void SetAutoCrashReporting(bool isCrashReporting)
        {
            this.isCrashReportingEnabled = isCrashReporting;
#if UNITY_ANDROID && !UNITY_EDITOR

#elif UNITY_IOS && !UNITY_EDITOR
            SwiftForUnity.UpdateCrashReportingNatively(isCrashReporting);
#endif
        }

        /// <summary>
        /// Set whether to enable tracking of user's every activity.
        /// </summary>
        /// <param name="isAllTrackingEnabled">Status of user's every activity tracking.</param>
        internal void SetAllTrackingEnabled(bool isAllTrackingEnabled)
        {
            this.isAllTrackingEnabled = isAllTrackingEnabled;
            SetAutoTrackUserMetadata(isAllTrackingEnabled);
            SetAutoTrackUserSession(isAllTrackingEnabled);
            SetAutoTrackDeviceMemory(isAllTrackingEnabled);
            CustomizeEventLoggingStatus(isAllTrackingEnabled);
            SetAutoCrashReporting(isAllTrackingEnabled);
        }
    }
}
