using MikrosClient.Analytics;
using System;

namespace MikrosClient.Ad
{
    /// <summary>
    /// Handles all marketing and advertising features.
    /// </summary>
    public sealed class AdController
    {
        private AppStoreListener appStoreListener;

        public AppStoreListener StoreListener => appStoreListener;

        /// <summary>
        /// Object creation of this class only allowed internally.
        /// </summary>
        internal AdController()
        {
            appStoreListener = new AppStoreListener();
        }
    }

    /// <summary>
    /// Event listeners for Mikros App Store.
    /// </summary>
    public sealed class AppStoreListener
    {
        /// <summary>
        /// Event for Mikros App Store open.
        /// </summary>
        public event Action OnOpened;

        /// <summary>
        /// Event for Mikros App Store close.
        /// </summary>
        public event Action OnClosed;

        /// <summary>
        /// Event for Mikros App Store related error.
        /// </summary>
        public event Action<MikrosException> OnError;

        /// <summary>
        /// Object creation of this class only allowed internally.
        /// </summary>
        internal AppStoreListener()
        { }

        /// <summary>
        /// Invoke subscribed operations for Mikros App Store open.
        /// </summary>
        internal void InvokeStoreOpened()
        {
            MikrosSDKPortalEvent.Builder(SDK_PORTAL_EVENT_TYPE.OPEN).Create();
            OnOpened?.Invoke();
        }

        /// <summary>
        /// Invoke subscribed operations for Mikros App Store close.
        /// </summary>
        internal void InvokeStoreClosed()
        {
            MikrosSDKPortalEvent.Builder(SDK_PORTAL_EVENT_TYPE.CLOSE).Create();
            OnClosed?.Invoke();
        }

        /// <summary>
        /// Invoke subscribed operations for Mikros App Store error.
        /// </summary>
        /// <param name="mikrosException"></param>
        internal void InvokeStoreError(MikrosException mikrosException)
        {
            OnError?.Invoke(mikrosException);
        }
    }
}
