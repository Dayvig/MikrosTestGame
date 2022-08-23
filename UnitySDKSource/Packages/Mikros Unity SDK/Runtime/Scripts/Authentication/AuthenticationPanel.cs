using MikrosClient.Tweening;
using MikrosClient.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MikrosClient.Authentication
{
    /// <summary>
    /// Authentication Panel display.
    /// </summary>
    internal sealed class AuthenticationPanel : MonoBehaviour
    {
        [SerializeField] private Text titleText;
        [SerializeField] private Text subtitleText;
        [SerializeField] private Button backButton;
        [SerializeField] private CanvasGroup authPanelCanvasGroup;
        [SerializeField] private Button closePanelButton;
        [SerializeField] private Transform mainPanel;
        [SerializeField] private Text legalText;

        [Space(10)]
        [Header("Choose an account Setup")]
        [Space(30)]
        [SerializeField] private MikrosSSOPage chooseAccountPage;

        [SerializeField] private Transform mikrosAccountButtonContainer;
        [SerializeField] private MikrosAccountButton mikrosAccountButtonPrefab;
        [SerializeField] private Button addAccountButton;

        [Space(10)]
        [Header("Signin Setup")]
        [Space(30)]
        [SerializeField] private MikrosSSOPage signinPage;

        [SerializeField] private CustomUserInputSetup signinUsernameEmailInputSetup;
        [SerializeField] private CustomUserInputSetup signinPasswordInputSetup;
        [SerializeField] private Button signinButton;
        [SerializeField] private Button goToSignupButton;

        [Space(10)]
        [Header("Signup Setup")]
        [Space(30)]
        [SerializeField] private MikrosSSOPage signupPage;

        [SerializeField] private CustomUserInputSetup signupUsernameInputSetup;
        [SerializeField] private CustomUserInputSetup signupEmailInputSetup;
        [SerializeField] private CustomUserInputSetup signupPasswordInputSetup;
        [SerializeField] private Button signupButton;

        [Space(10)]
        [Header("Authenticate Setup")]
        [Space(30)]
        [SerializeField] private MikrosSSOPage authenticatePage;

        [SerializeField] private Text authenticateEmailText;
        [SerializeField] private CustomUserInputSetup authenticatePasswordInputSetup;
        [SerializeField] private Button authenticateSigninButton;

        private static AuthenticationPanel instance;

        private MikrosSSOPage currentActivePage;
        private MikrosSSOPage tempNextPage = null;
        private MikrosSSORequest onHandleAuthSuccess = null;
        private float fadeTime = 0.12f;
        private float pageTransitionTime = 0.15f;

        /// <summary>
        /// Builder function is used for creating an object of AuthenticationPanel class.
        /// </summary>
        /// <returns>AuthenticationPanel object.</returns>
        internal static AuthenticationPanel Builder()
        {
            // if object not created then create the object first
            if (instance == null)
            {
                instance = Instantiate(Resources.Load<AuthenticationPanel>(Constants.AuthenticationPanelPrefabName), MikrosUiCanvas.Instance.transform, false);
                instance.gameObject.SetActive(false); // Disable AuthenticationPanel to enable form outside.
            }
            return instance;
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
		private void Awake()
        {
            // Setting the legal description text.
            legalText.text = Constants.LegalDescriptionText;

            // Setting the subtitle of Choose Account page.
            subtitleText.text = Constants.ChooseAccountSubtitleText;

            DisplayAllMikrosAccounts();
        }

        /// <summary>
        /// Display all Mikros accounts.
        /// </summary>
        private void DisplayAllMikrosAccounts()
        {
            List<MikrosUserData> mikrosAccounts = new List<MikrosUserData>(MikrosManager.Instance.AuthenticationController.SavedMikrosAccounts);
            for (int i = 0; i < mikrosAccounts.Count; i++)
            {
                GenerateMikrosAccountView(mikrosAccounts[i], Equals(mikrosAccounts[i].Email, MikrosManager.Instance.AuthenticationController.MikrosUser?.Email));
            }
        }

        /// <summary>
        /// Generate a Mikros account in UI.
        /// </summary>
        /// <param name="mikrosUserData">Details of the user.</param>
        /// <param name="isSignin">True if the user is currently signed in, else false.</param>
        private void GenerateMikrosAccountView(MikrosUserData mikrosUserData, bool isSignin)
        {
            Instantiate(mikrosAccountButtonPrefab, mikrosAccountButtonContainer, false)
                .InitializeView(mikrosUserData.UserName, mikrosUserData.Email, isSignin, () => OnMikrosAccountClick(mikrosUserData.Email));
        }

        /// <summary>
        /// On click handler for clicking Mikros account.
        /// </summary>
        private void OnMikrosAccountClick(string mikrosUserEmail)
        {
            PageTransition(authenticatePage);
            authenticateEmailText.text = mikrosUserEmail;
        }

        /// <summary>
        /// Display the authentication panel.
        /// </summary>
        /// <param name="onHandleAuthSuccess">Callback for authentication success.</param>
        internal void LaunchSignin(MikrosSSORequest onHandleAuthSuccess)
        {
            this.onHandleAuthSuccess = onHandleAuthSuccess;
            authPanelCanvasGroup.alpha = 0;
            if (MikrosManager.Instance.AuthenticationController.SavedMikrosAccounts.Count > 0)
            {
                chooseAccountPage.PageGameObject.SetActive(true);
                subtitleText.gameObject.SetActive(true);
                currentActivePage = chooseAccountPage;
            }
            else
            {
                signinPage.PageGameObject.SetActive(true);
                currentActivePage = signinPage;
            }
            titleText.text = currentActivePage.Title;
            subtitleText.gameObject.SetActive(currentActivePage.ShowSubtitleText);
            gameObject.SetActive(true);
            PanelFadeInOut(0, 1);
        }

        /// <summary>
        /// Called everytime the Authentication panel is enabled.
        /// </summary>
		private void OnEnable()
        {
            closePanelButton.onClick.AddListener(CloseWithAnimation);
            addAccountButton.onClick.AddListener(() => PageTransition(signinPage));
            SigninRegisterEventListeners();
            SignupRegisterEventListeners();
            AuthenticateRegisterEventListeners();
        }

        /// <summary>
        /// Register event listeners in Signin page.
        /// </summary>
        private void SigninRegisterEventListeners()
        {
            signinUsernameEmailInputSetup.InputField.onValueChanged.AddListener(currentValue => OnInputFieldValueChanged(signinUsernameEmailInputSetup));
            signinUsernameEmailInputSetup.InputField.onEndEdit.AddListener(currentValue => OnInputFieldEndEdit(signinUsernameEmailInputSetup, () => Utils.AuthUtils.SigninUsernameEmailError(currentValue)));

            signinPasswordInputSetup.InputField.onValueChanged.AddListener(currentValue => OnInputFieldValueChanged(signinPasswordInputSetup));
            signinPasswordInputSetup.InputField.onEndEdit.AddListener(currentValue => OnInputFieldEndEdit(signinPasswordInputSetup, () => Utils.AuthUtils.SigninPasswordError(currentValue)));

            signinButton.onClick.AddListener(OnClickSignin);
            goToSignupButton.onClick.AddListener(() => PageTransition(signupPage));
        }

        /// <summary>
        /// Remove event listeners in Signin page.
        /// </summary>
        private void SigninRemoveEventListeners()
        {
            signinUsernameEmailInputSetup.InputField.onValueChanged.RemoveAllListeners();
            signinUsernameEmailInputSetup.InputField.onEndEdit.RemoveAllListeners();

            signinPasswordInputSetup.InputField.onValueChanged.RemoveAllListeners();
            signinPasswordInputSetup.InputField.onEndEdit.RemoveAllListeners();

            signinButton.onClick.RemoveAllListeners();
            goToSignupButton.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// Register event listeners in Signup page.
        /// </summary>
        private void SignupRegisterEventListeners()
        {
            signupUsernameInputSetup.InputField.onValueChanged.AddListener(currentValue => OnInputFieldValueChanged(signupUsernameInputSetup));
            signupUsernameInputSetup.InputField.onEndEdit.AddListener(currentValue => OnInputFieldEndEdit(signupUsernameInputSetup, () => Utils.AuthUtils.SignupUsernameError(currentValue, onHandleAuthSuccess.IsEnableUsernameSpecialCharacters)));

            signupEmailInputSetup.InputField.onValueChanged.AddListener(currentValue => OnInputFieldValueChanged(signupEmailInputSetup));
            signupEmailInputSetup.InputField.onEndEdit.AddListener(currentValue => OnInputFieldEndEdit(signupEmailInputSetup, () => Utils.AuthUtils.SignupEmailError(currentValue)));

            signupPasswordInputSetup.InputField.onValueChanged.AddListener(currentValue => OnInputFieldValueChanged(signupPasswordInputSetup));
            signupPasswordInputSetup.InputField.onEndEdit.AddListener(currentValue => OnInputFieldEndEdit(signupPasswordInputSetup, () => Utils.AuthUtils.SignupPasswordError(currentValue)));

            signupButton.onClick.AddListener(OnClickSignup);
        }

        /// <summary>
        /// Remove event listeners in Signup page.
        /// </summary>
        private void SignupRemoveEventListeners()
        {
            signupUsernameInputSetup.InputField.onValueChanged.RemoveAllListeners();
            signupUsernameInputSetup.InputField.onEndEdit.RemoveAllListeners();

            signupEmailInputSetup.InputField.onValueChanged.RemoveAllListeners();
            signupEmailInputSetup.InputField.onEndEdit.RemoveAllListeners();

            signupPasswordInputSetup.InputField.onValueChanged.RemoveAllListeners();
            signupPasswordInputSetup.InputField.onEndEdit.RemoveAllListeners();

            signupButton.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// Register event listeners in Authenticate page.
        /// </summary>
        private void AuthenticateRegisterEventListeners()
        {
            authenticatePasswordInputSetup.InputField.onValueChanged.AddListener(currentValue => OnInputFieldValueChanged(authenticatePasswordInputSetup));
            authenticatePasswordInputSetup.InputField.onEndEdit.AddListener(currentValue => OnInputFieldEndEdit(authenticatePasswordInputSetup, () => Utils.AuthUtils.SigninPasswordError(currentValue)));

            authenticateSigninButton.onClick.AddListener(OnClickAuthenticateSignin);
        }

        /// <summary>
        /// Remove event listeners in Authenticate page.
        /// </summary>
        private void AuthenticateRemoveEventListeners()
        {
            authenticatePasswordInputSetup.InputField.onValueChanged.RemoveAllListeners();
            authenticatePasswordInputSetup.InputField.onEndEdit.RemoveAllListeners();

            authenticateSigninButton.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// Input field value change handler.
        /// </summary>
        /// <param name="customUserInputSetup">Input-field and inline error text setup.</param>
        private void OnInputFieldValueChanged(CustomUserInputSetup customUserInputSetup)
        {
            customUserInputSetup.InlineErrorText.text = string.Empty;
        }

        /// <summary>
        /// Input field value edit end handler.
        /// </summary>
        /// <param name="customUserInputSetup">Input-field and inline error text setup.</param>
        /// <param name="callback">Task to be done after editing end.</param>
        private void OnInputFieldEndEdit(CustomUserInputSetup customUserInputSetup, Func<string> callback)
        {
            if (!string.IsNullOrEmpty(customUserInputSetup.InputField.text))
            {
                ShowInputError(customUserInputSetup, callback());
            }
        }

        /// <summary>
        /// Button click task for page transitions.
        /// </summary>
        /// <param name="toPage">New page to show.</param>
        private void PageTransition(MikrosSSOPage toPage)
        {
            ResetAllView();
            RectTransform toPageRect = toPage.PageGameObject.GetComponent<RectTransform>();
            float initialX = toPageRect.anchoredPosition.x;
            // Setting up initial position of next page for the transition.
            toPageRect.anchoredPosition = new Vector2(-toPageRect.rect.width - toPageRect.offsetMin.x, toPageRect.anchoredPosition.y);
            toPage.PageGameObject.SetActive(true);
            titleText.text = toPage.Title;
            subtitleText.gameObject.SetActive(toPage.ShowSubtitleText);
            backButton.onClick.RemoveAllListeners();
            backButton.gameObject.SetActive(toPage.HasBackButton);
            if (toPage.HasBackButton)
            {
                MikrosSSOPage currentPage = currentActivePage;
                backButton.onClick.AddListener(() => PageTransition(currentPage));
            }
            tempNextPage = toPage;
            PageMoveRight(toPageRect.anchoredPosition.x, initialX);
        }

        /// <summary>
        /// Page transition animation.
        /// </summary>
        /// <param name="fromX">Initial anchored X value of next page.</param>
        /// <param name="toX">Final anchored X value of next page.</param>
        private void PageMoveRight(float fromX, float toX)
        {
            Hashtable tweenArgs = new Hashtable
            {
                { "from", fromX },
                { "to", toX },
                { "time", pageTransitionTime },
                { "onupdatetarget", gameObject },
                { "onupdate", nameof(OnPageMovePositionChange) },
                { "oncompletetarget", gameObject },
                { "oncomplete", nameof(OnMoveRightCompleted) },
                { "easetype", iTween.EaseType.linear }
            };
            iTween.ValueTo(gameObject, tweenArgs);
        }

        /// <summary>
        /// Tween value change handler for page transition.
        /// </summary>
        /// <param name="newValue">Updated value of next page anchored position X.</param>
        private void OnPageMovePositionChange(float newValue)
        {
            RectTransform nextPageRect = tempNextPage.PageGameObject.GetComponent<RectTransform>();
            nextPageRect.anchoredPosition = new Vector2(newValue, nextPageRect.anchoredPosition.y);

            RectTransform currentPageRect = currentActivePage.PageGameObject.GetComponent<RectTransform>();
            currentPageRect.anchoredPosition = new Vector2(nextPageRect.rect.width + newValue, currentPageRect.anchoredPosition.y);
        }

        /// <summary>
        /// Handler for page transition completion.
        /// </summary>
        private void OnMoveRightCompleted()
        {
            currentActivePage.PageGameObject.SetActive(false);
            // Reseting the anchored position of the previous active page.
            currentActivePage.PageGameObject.GetComponent<RectTransform>().anchoredPosition = tempNextPage.PageGameObject.GetComponent<RectTransform>().anchoredPosition;
            currentActivePage = tempNextPage;
            tempNextPage = null;
        }

        /// <summary>
        /// Button click tasks for Signin.
        /// </summary>
        private void OnClickSignin()
        {
            bool isUsernameEmailError = ShowInputError(signinUsernameEmailInputSetup, Utils.AuthUtils.SigninUsernameEmailError(signinUsernameEmailInputSetup.InputField.text));
            bool isPasswordError = ShowInputError(signinPasswordInputSetup, Utils.AuthUtils.SigninPasswordError(signinPasswordInputSetup.InputField.text));

            if (isUsernameEmailError || isPasswordError)
            {
                return;
            }
            MikrosSignin(signinUsernameEmailInputSetup.InputField.text, signinPasswordInputSetup.InputField.text);
        }

        /// <summary>
        /// Button click tasks for Signin in Authenticate page.
        /// </summary>
        private void OnClickAuthenticateSignin()
        {
            bool isPasswordError = ShowInputError(authenticatePasswordInputSetup, Utils.AuthUtils.SigninPasswordError(authenticatePasswordInputSetup.InputField.text));

            if (isPasswordError)
            {
                return;
            }
            MikrosSignin(authenticateEmailText.text, authenticatePasswordInputSetup.InputField.text);
        }

        /// <summary>
        /// Button click tasks for Signup.
        /// </summary>
        private void OnClickSignup()
        {
            bool isUsernameError = ShowInputError(signupUsernameInputSetup, Utils.AuthUtils.SignupUsernameError(signupUsernameInputSetup.InputField.text,onHandleAuthSuccess.IsEnableUsernameSpecialCharacters ));
            bool isEmailError = ShowInputError(signupEmailInputSetup, Utils.AuthUtils.SignupEmailError(signupEmailInputSetup.InputField.text));
            bool isPasswordError = ShowInputError(signupPasswordInputSetup, Utils.AuthUtils.SignupPasswordError(signupPasswordInputSetup.InputField.text));

            if (isUsernameError || isEmailError || isPasswordError)
            {
                return;
            }
            MikrosSignup(signupUsernameInputSetup.InputField.text, signupEmailInputSetup.InputField.text, signupPasswordInputSetup.InputField.text);
        }

        /// <summary>
        /// Show error in inline text field.
        /// </summary>
        /// <param name="customUserInputSetup">Input-field and inline error text setup.</param>
        /// <param name="error">Error details.</param>
        /// <returns>True if any error is shown, else False.</returns>
        private bool ShowInputError(CustomUserInputSetup customUserInputSetup, string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                customUserInputSetup.InlineErrorText.text = error;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Perform Mikros Signin.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="email">Email.</param>
        /// <param name="password">Password.</param>
        /// <param name="callback">Signin success callback.</param>
        private void MikrosSignin(string usernameEmail, string password)
        {
            string username = "", email = "";
            (username, email) = Utils.AuthUtils.DetermineUsernameOrEmail(usernameEmail.Trim(), true);
            authPanelCanvasGroup.interactable = false;
            SigninRequest.Builder()
                .Username(username)
                .Email(email)
                .Password(password)
                .Create(signinRequest =>
                {
                    MikrosLogger.Log("All input correct.");
                    MikrosManager.Instance.AuthenticationController.Signin(signinRequest,
                        successResponse => HandleAuthSuccess(successResponse),
                        error => HandleFailure(error, true));
                },
                error => HandleFailure(error));
        }

        /// <summary>
        /// Perform Mikros Signup.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="email">Email.</param>
        /// <param name="password">Password.</param>
        /// <param name="callback">Signup success callback.</param>
        private void MikrosSignup(string username, string email, string password)
        {
            authPanelCanvasGroup.interactable = false;
            SignupRequest.Builder()
                .Username(username)
                .Email(email)
                .Password(password)
                .EnableUsernameSpecialCharacters(onHandleAuthSuccess.IsEnableUsernameSpecialCharacters)
                .Create(signupRequest =>
                {
                    MikrosLogger.Log("All input correct.");

                    MikrosManager.Instance.AuthenticationController.Signup(signupRequest, successResponse =>
                    {
                        HandleAuthSuccess(successResponse);
                    },
                    error => HandleFailure(error, true));
                },
                error => HandleFailure(error));
        }

        /// <summary>
        /// Handle success event for Signin/Signup.
        /// </summary>
        /// <param name="mikrosUser">Details of signed in user.</param>
        private void HandleAuthSuccess(MikrosUser mikrosUser)
        {
            onHandleAuthSuccess.mikrosUserAction?.Invoke(mikrosUser);
            Crouton.Builder(mainPanel, STATUS_TYPE.SUCCESS).ShowMessage(Constants.SuccessMessageGeneric).OnCroutonEnd(CloseWithAnimation);
        }

        /// <summary>
        /// Handle request builder error.
        /// </summary>
        /// <param name="customException">Exception occured.</param>
        /// <param name="showCrouton">Show a crouton for the exception message.</param>
        private void HandleFailure(MikrosException customException, bool showCrouton = false)
        {
            string message = customException.Message.Replace('_', ' ').Replace('{', ' ');
            MikrosLogger.LogError(Constants.SSOErrorHeader + message);
            if (showCrouton)
            {
                Crouton.Builder(mainPanel, STATUS_TYPE.ERROR).ShowMessage(message);
            }
            authPanelCanvasGroup.interactable = true;
        }

        /// <summary>
        /// Reset view of full panel.
        /// </summary>
        private void ResetAllView()
        {
            ResetInputView(signinUsernameEmailInputSetup);
            ResetInputView(signinPasswordInputSetup);
            ResetInputView(signupUsernameInputSetup);
            ResetInputView(signupEmailInputSetup);
            ResetInputView(signupPasswordInputSetup);
            ResetInputView(authenticatePasswordInputSetup);
            authenticateEmailText.text = string.Empty;
        }

        /// <summary>
        /// Reset input-field and inline errors texts.
        /// </summary>
        /// <param name="customUserInputSetup"></param>
        private void ResetInputView(CustomUserInputSetup customUserInputSetup)
        {
            customUserInputSetup.InputField.SetTextWithoutNotify(string.Empty);
            customUserInputSetup.InlineErrorText.text = string.Empty;
        }

        /// <summary>
        /// Tween fade panel view.
        /// </summary>
        /// <param name="from">Start value.</param>
        /// <param name="to">End Value.</param>
        /// <param name="additionalArgs">Additional arguments for the tween.</param>
        private void PanelFadeInOut(float from, float to, Hashtable additionalArgs = null)
        {
            Hashtable tweenArgs = new Hashtable
            {
                { "from", from },
                { "to", to },
                { "time", fadeTime },
                { "onupdatetarget", gameObject },
                { "onupdate", nameof(OnFadeValueChange) },
                { "easetype", iTween.EaseType.linear }
            };
            if (!Equals(additionalArgs, null))
            {
                foreach (DictionaryEntry item in additionalArgs)
                {
                    tweenArgs.Add(item.Key, item.Value);
                }
            }
            iTween.ValueTo(gameObject, tweenArgs);
        }

        /// <summary>
        /// Handle change of fade value of panel.
        /// </summary>
        /// <param name="newValue">Updated fade value.</param>
        private void OnFadeValueChange(float newValue)
        {
            authPanelCanvasGroup.alpha = newValue;
        }

        /// <summary>
        /// Close the authentication panel with animation.
        /// </summary>
        private void CloseWithAnimation()
        {
            Hashtable args = new Hashtable
            {
                { "oncompletetarget", gameObject },
                { "oncomplete", nameof(CloseAuthenticationPanel) }
            };
            PanelFadeInOut(1, 0, args);
        }

        /// <summary>
        /// Close the authentication panel.
        /// </summary>
        private void CloseAuthenticationPanel()
        {
            authPanelCanvasGroup.interactable = true;
            Destroy(gameObject);
        }

        /// <summary>
        /// Called everytime the Authentication panel is disabled.
        /// </summary>
		private void OnDisable()
        {
            onHandleAuthSuccess = null;
            closePanelButton.onClick.RemoveAllListeners();
            addAccountButton.onClick.RemoveAllListeners();
            SigninRemoveEventListeners();
            SignupRemoveEventListeners();
            AuthenticateRemoveEventListeners();
        }
    }
}
