using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MikrosClient.UI
{
    /// <summary>
    /// User profile tab view in Mobile Custom UI.
    /// </summary>
    internal sealed class UserProfilePanel : MonoBehaviour
    {
        [SerializeField] private RawImage userImage;
        [SerializeField] private Text usernameText;
        [SerializeField] private InputField emailInputField;
        [SerializeField] private Button saveButton;

        /// <summary>
        /// Preliminary initialization for the user profile tab.
        /// </summary>
        internal void Initialize()
        {
            if (MikrosManager.Instance.AuthenticationController.MikrosUser == null)
            {
                return;
            }
            usernameText.text = MikrosManager.Instance.AuthenticationController.MikrosUser.UserName;
            emailInputField.text = MikrosManager.Instance.AuthenticationController.MikrosUser.Email;
        }
    }
}
