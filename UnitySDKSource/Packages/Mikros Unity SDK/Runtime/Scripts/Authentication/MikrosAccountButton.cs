using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MikrosClient.Authentication
{
    /// <summary>
    /// Display of Mikros Account button.
    /// </summary>
    internal sealed class MikrosAccountButton : MonoBehaviour
    {
        [SerializeField] private Button mainButton;
        [SerializeField] private Text usernameText;
        [SerializeField] private Text emailText;
        [SerializeField] private Image signinStatus;

        /// <summary>
        /// Username associated with this account.
        /// </summary>
        internal string Username { get; private set; }

        /// <summary>
        /// Email associated with this account.
        /// </summary>
        internal string Email { get; private set; }

        private Action onClickTask = null;

        /// <summary>
        /// Initialize contents.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="email">Email.</param>
        /// <param name="isSignin">True if the user is currently signed in, else false.</param>
        /// <param name="onClickTask">Action to perform upon clicking this button.</param>
        internal void InitializeView(string username, string email, bool isSignin, Action onClickTask)
        {
            transform.SetAsFirstSibling();
            Username = username;
            Email = email;
            if (string.IsNullOrEmpty(username))
            {
                usernameText.gameObject.SetActive(false);
            }
            else
            {
                usernameText.text = username;
            }
            emailText.text = email;
            SetSigninStatus(isSignin);
            this.onClickTask = onClickTask;
        }

        /// <summary>
        /// Set signin status indicator.
        /// </summary>
        /// <param name="isSignin">True if the user is currently signed in, else false.</param>
        internal void SetSigninStatus(bool isSignin)
        {
            if (isSignin)
            {
                signinStatus.color = Color.green;
            }
            else
            {
                signinStatus.color = Color.red;
            }
        }

        /// <summary>
        /// Called every time MikrosAccountButton is enabled.
        /// </summary>
		private void OnEnable()
        {
            mainButton.onClick.AddListener(() => onClickTask?.Invoke());
        }

        /// <summary>
        /// Called every time MikrosAccountButton is disabled.
        /// </summary>
		private void OnDisable()
        {
            mainButton.onClick.RemoveAllListeners();
        }
    }
}
