using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MikrosClient.UI
{
    /// <summary>
    /// Base abstract class for app to show in UI.
    /// </summary>
    internal abstract class AppBase : MonoBehaviour
    {
        /// <summary>
        /// RawImage component that holds app icon image.
        /// </summary>
        [SerializeField]
        private RawImage imageIcon;

        /// <summary>
        /// Text field to show initials of the app name.
        /// </summary>
        [SerializeField]
        private Text txtAppInitial;

        /// <summary>
        /// Text component that holds app name.
        /// </summary>
        [SerializeField]
        private Text txtAppName;

        /// <summary>
        /// Google play store link.
        /// </summary>
        private string googlePlayStoreLink;

        /// <summary>
        /// Apple app store link.
        /// </summary>
        private string appleAppStoreLink;

        /// <summary>
        /// Preliminary initialization of the app view UI.
        /// </summary>
        /// <param name="appDetailsContent">Details of app received in response.</param>
        internal virtual void Initialize(AppDetailsContent appDetailsContent)
        {
            txtAppName.text = appDetailsContent.AppName;
            txtAppInitial.text = Utils.GetInitials(appDetailsContent.AppName);
            googlePlayStoreLink = appDetailsContent.GooglePlayStoreLink;
            appleAppStoreLink = appDetailsContent.AppleAppStoreLink;
            SetupView(appDetailsContent);
        }

        /// <summary>
        /// Populate the app view UI.
        /// </summary>
        /// <param name="appDetailsContent">Details of app received in response.</param>
        private void SetupView(AppDetailsContent appDetailsContent)
        {
            string imageUrl = "";
            if (typeof(FeaturedApp).IsAssignableFrom(GetType()) && appDetailsContent.Images.FeatureGraphics.Count > 0 && !appDetailsContent.Images.FeatureGraphics.Any(image => string.IsNullOrEmpty(image.ImageUrl)))
            {
                imageUrl = appDetailsContent.Images.FeatureGraphics.FirstOrDefault(image => !string.IsNullOrEmpty(image.ImageUrl)).ImageUrl;
            }
            else if (typeof(NormalApp).IsAssignableFrom(GetType()) && appDetailsContent.Images.HiResIcons.Count > 0 && !appDetailsContent.Images.HiResIcons.Any(image => string.IsNullOrEmpty(image.ImageUrl)))
            {
                imageUrl = appDetailsContent.Images.HiResIcons.FirstOrDefault(image => !string.IsNullOrEmpty(image.ImageUrl)).ImageUrl;
            }

            if (!string.IsNullOrEmpty(imageUrl))
            {
                WebRequest<Texture2D>.Builder()
                    .Url(imageUrl)
                    .CreateDownloadTextureRequest((Texture2D texture2D) =>
                    {
                        if (texture2D != null)
                        {
                            imageIcon.texture = texture2D;
                            txtAppInitial.text = "";
                        }
                    });
            }
        }

        /// <summary>
        /// Operation when an app is clicked.
        /// </summary>
        public void OnClickApp()
        {
            string storeLink = Utils.GetRelevantStoreLink(googlePlayStoreLink, appleAppStoreLink);
            if (!string.IsNullOrEmpty(storeLink))
            {
                Application.OpenURL(storeLink);
            }
        }
    }
}
