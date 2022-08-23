using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MikrosClient.Authentication
{
    /// <summary>
    /// Custom class for input-field and inline error text setup in UI.
    /// </summary>
    [Serializable]
    internal sealed class CustomUserInputSetup
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Text inlineErrorText;

        /// <summary>
        /// Input-field reference.
        /// </summary>
        internal InputField InputField => inputField;

        /// <summary>
        /// Text field reference.
        /// </summary>
        internal Text InlineErrorText => inlineErrorText;
    }

    /// <summary>
    /// Display details of a page in Mikros SSO.
    /// </summary>
    [Serializable]
    internal sealed class MikrosSSOPage
    {
        [SerializeField] private GameObject pageGameObject;
        [SerializeField] private string title;
        [SerializeField] private bool hasBackButton;
        [SerializeField] private bool showSubtitleText;

        /// <summary>
        /// GameObject that indicates the page.
        /// </summary>
        internal GameObject PageGameObject => pageGameObject;

        /// <summary>
        /// Title of the page.
        /// </summary>
        internal string Title => title;

        /// <summary>
        /// Indicate whether back button be visible on this page.
        /// </summary>
        internal bool HasBackButton => hasBackButton;

        /// <summary>
        /// Indicate whether to show the back button or not on this page.
        /// </summary>
        internal bool ShowSubtitleText => showSubtitleText;
    }
}
