using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using MikrosClient;

namespace MikrosClient.NativeFramework
{
    internal class SwiftForUnity
    {
#if UNITY_IOS && !UNITY_EDITOR

        #region Declare external C interface

        [DllImport("__Internal")]
        private static extern void ClientConfigurationSetup(string apiKey, string baseUrl, string appGameId, string apiKeyType, string deviceId, string appVersion, string sdkVersion, string eventLogging, string sessionTracking, string crashReporting);

        [DllImport("__Internal")]
        private static extern void UpdateUserMetadata(string latitude, string longitude, string deviceModel, string deviceOSVersion, string deviceOperatingSystem, string deviceScreenDpi, string deviceScreenHeight, string deviceScreenWidth, string sdkVersion, string sdkType, string isWifi);

        [DllImport("__Internal")]
        private static extern void LogEvent(string eventData);

        [DllImport("__Internal")]
        private static extern void FlushEvents();

        [DllImport("__Internal")]
        private static extern void UpdateSessionLogging(bool isTrackUserSession);

        [DllImport("__Internal")]
        private static extern void UpdateEventLogging(bool isEventLogging);

        [DllImport("__Internal")]
        private static extern void UpdateCrashReporting(bool isCrashReporting);

         [DllImport("__Internal")]
        private static extern void UpdateMemoryLogging(bool isTrackDeviceMemory);

        [DllImport("__Internal")]
        private static extern void OnMotionEvent();

        #endregion Declare external C interface

        #region Wrapped methods and properties

        /// <summary>
        /// Setup client configuration for native iOS.
        /// </summary>
        internal static void PerformClientConfiguration(bool isEventLogging, bool isTrackUserSession, bool isCrashReporting)
        {
            string eventLogging = isEventLogging ? "1" : "0";
            string sessionTracking = isTrackUserSession ? "1" : "0";
            string crashReporting = isCrashReporting ? "1" : "0";
            ClientConfigurationSetup(MikrosManager.Instance.ConfigurationController.MikrosSettings.GetCurrentApiKey(),
                ServerData.GetUrl(UrlType.BaseURL),
                MikrosManager.Instance.ConfigurationController.MikrosSettings.AppGameID,
                MikrosManager.Instance.ConfigurationController.MikrosSettings.GetApiKeyTypeString(),
                Constants.DeviceID,
                Application.version,
                Constants.SDKVersion,
                eventLogging,
                sessionTracking,
                crashReporting);
        }

        /// <summary>
        /// Update user metadata via native iOS.
        /// </summary>
        /// <param name="metadata">Metadata to uploaded.</param>
        internal static void UpdateUserMetadataNatively(Metadata metadata)
        {
            UpdateUserMetadata(metadata.latitude,
                metadata.longitude,
                metadata.deviceModel,
                metadata.deviceOSVersion,
                metadata.deviceOperatingSystem,
                metadata.deviceScreenDpi,
                metadata.deviceScreenHeight,
                metadata.deviceScreenWidth,
                metadata.sdkVersion,
                metadata.sdkType,
                metadata.isWifi);
        }

        /// <summary>
        /// Custom log event via native iOS.
        /// </summary>
        /// <param name="eventData">Custom event data.</param>
        internal static void LogEventNatively(string eventData)
        {
            LogEvent(eventData);
        }

        /// <summary>
        /// Flush all session and events via native iOS.
        /// </summary>
        internal static void FlushEventsNatively()
        {
            FlushEvents();
        }

        /// <summary>
        /// Update user session tracking status via native iOS.
        /// </summary>
        /// <param name="isTrackUserSession">Status of user session tracking.</param>
        internal static void UpdateSessionLoggingNatively(bool isTrackUserSession)
        {
            UpdateSessionLogging(isTrackUserSession);
        }

        /// <summary>
        /// Update user event tracking status via native iOS.
        /// </summary>
        /// <param name="isEventLogging">Status of user event tracking.</param>
        internal static void UpdateEventLoggingNatively(bool isEventLogging)
        {
            UpdateEventLogging(isEventLogging);
        }

        /// <summary>
        /// Set Crash Reporting status via native iOS.
        /// </summary>
        /// <param name="isCrashReportingEnabled">Status of crash report.</param>
        internal static void UpdateCrashReportingNatively(bool isCrashReportingEnabled)
        {
            UpdateCrashReporting(isCrashReportingEnabled);
        }

        /// <summary>
        /// Update user device memory tracking status via native iOS.
        /// </summary>
        /// <param name="isTrackDeviceMemory">Status of user device memory tracking.</param>
        internal static void UpdateMemoryLoggingNatively(bool isTrackDeviceMemory)
        {
            UpdateMemoryLogging(isTrackDeviceMemory);
        }

        /// <summary>
        /// Touch event detection in native iOS.
        /// </summary>
        internal static void OnMotionEventNative()
        {
            OnMotionEvent();
        }

        #endregion Wrapped methods and properties

#endif
    }
}