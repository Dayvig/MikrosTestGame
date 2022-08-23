using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MikrosClient.UI
{
    /// <summary>
    /// Class for a featured app to show in UI.
    /// </summary>
    internal sealed class FeaturedApp : AppBase
    {
        /// <summary>
        /// Text component that holds company name.
        /// </summary>
        [SerializeField]
        private Text txtCompanyName;

        /// <summary>
        /// Text component that holds app description.
        /// </summary>
        [SerializeField]
        private Text txtAppDescription;

        /// <summary>
        /// Preliminary initialization of the featured app view UI.
        /// </summary>
        /// <param name="appDetailsContent">Details of app received in response.</param>
        internal override void Initialize(AppDetailsContent appDetailsContent)
        {
            base.Initialize(appDetailsContent);
            txtAppDescription.text = appDetailsContent.ShortDesc;
            txtCompanyName.text = appDetailsContent.CompanyName;
        }
    }
}
