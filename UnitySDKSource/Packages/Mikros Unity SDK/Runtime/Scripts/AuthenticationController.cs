using MikrosClient.Analytics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace MikrosClient.Authentication
{
    /// <summary>
    /// Handles all authentication related operations.
    /// </summary>
    public sealed class AuthenticationController
    {
        /// <summary>
        /// Reference of the current Session. (if any)
        /// </summary>
        internal SessionResponse Session { get; private set; }

        /// <summary>
        /// Reference of full data of Mikros user received in auth response.
        /// </summary>
        internal MikrosUserData MikrosUserData { get; private set; }

        /// <summary>
        /// Reference of the current user. (if any)
        /// </summary>
        public MikrosUser MikrosUser { get; private set; }

        /// <summary>
        /// Collection of all Mikros saved accounts for an app.
        /// </summary>
        private List<MikrosUserData> savedMikrosAccounts = new List<MikrosUserData>();

        /// <summary>
        /// Read only collection of all Mikros saved accounts for an app.
        /// </summary>
        internal ReadOnlyCollection<MikrosUserData> SavedMikrosAccounts => savedMikrosAccounts.AsReadOnly();

        /// <summary>
        /// Event for when Mikros SSO button is clicked.
        /// </summary>
        internal event Action<MikrosSSORequest> OnClickMikrosSSO;

        /// <summary>
        /// Object creation of this class only allowed internally.
        /// </summary>
        internal AuthenticationController()
        { }

        /// <summary>
        /// Setup all auth related data required.
        /// </summary>
        internal void SetupEssentialData()
        {
            CheckAndRetrieveAuthData();

            // Retrieve saved Mikros accounts.
            Utils.AuthUtils.GetSavedMikrosAccounts(out savedMikrosAccounts);
            if (Equals(savedMikrosAccounts, null))
            {
                savedMikrosAccounts = new List<MikrosUserData>();
            }
        }

        /// <summary>
        /// Check and retrieve active auth sessions.
        /// </summary>
        private void CheckAndRetrieveAuthData()
        {
            AuthResponse authResponse;
            if (Utils.AuthUtils.RetrieveAuthData(out authResponse) && Utils.AuthUtils.IsSessionActive(authResponse.Data.Session))
            {
                MikrosLogger.Log("Active session found.");
                InitializeData(authResponse);
            }
        }

        /// <summary>
        /// Initialize the data of current signed in user and session.
        /// </summary>
        /// <param name="authResponse">Auth success data.</param>
        private void InitializeData(AuthResponse authResponse)
        {
            // populate user model from response.
            MikrosUserData = authResponse.Data.User;
            // adapt response into a simplified model which is accessible by Integrators.
            MikrosUser = MikrosUserData.PopulateData();
            // populate session model from response.
            Session = authResponse.Data.Session;
        }

        /// <summary>
        /// Save the Mikros user details locally.
        /// </summary>
        /// <param name="mikrosUserData">Mikros account.</param>
        private void SaveMikrosAccount(MikrosUserData mikrosUserData)
        {
            if (!savedMikrosAccounts.Exists(item => Equals(item.Email, mikrosUserData.Email)))
            {
                savedMikrosAccounts.Add(mikrosUserData);
                Utils.AuthUtils.SaveMikrosAccounts(savedMikrosAccounts);
            }
        }

        /// <summary>
        /// Display the authentication panel.
        /// </summary>
        /// <param name="onHandleAuthSuccess">Callback for authentication success.</param>
        public void LaunchSignin(Action<MikrosUser> onHandleAuthSuccess = null)
        {
            MikrosSSORequest mikrosSSORequest = MikrosSSORequest.Builder().MikrosUserAction(onHandleAuthSuccess).Create();
            OnClickMikrosSSO?.Invoke(mikrosSSORequest);
        }
        /// <summary>
        /// Display the authentication panel.
        /// </summary>
        /// <param name="mikrosSSORequest">Callback the authentication success.</param>
        public void LaunchSignin(MikrosSSORequest mikrosSSORequest)
        {
            OnClickMikrosSSO?.Invoke(mikrosSSORequest);
        }

        /// <summary>
        /// For Sign in api calling function.
        /// </summary>
        /// <param name="signinDataDTO">SigninData object parameter for request parameter of the signin api.</param>
        /// <param name="onSuccess">Callback for Signin success.</param>
        /// <param name="onFailure">Callback for Signin failure.</param>
        public void Signin(IAuthenticationRequest signinDataDTO, Action<MikrosUser> onSuccess, Action<MikrosException> onFailure = null)
        {
            if (typeof(SigninRequest).IsAssignableFrom(signinDataDTO.GetType()))
            {
                CheckSessionAndInitiateAuthentication(signinDataDTO, UrlType.SignIn, onSuccess, onFailure);
            }
            else
            {
                onFailure?.Invoke(null);
            }
        }

        /// <summary>
        /// For sign up api calling function.
        /// </summary>
        /// <param name="signupDataDTO">SignupData object parameter for request parameter of the signup api.</param>
        /// <param name="onSuccess">Callback for Signup success.</param>
        /// <param name="onFailure">Callback for Signup failure.</param>
        public void Signup(IAuthenticationRequest signupDataDTO, Action<MikrosUser> onSuccess, Action<MikrosException> onFailure = null)
        {
            if (typeof(SignupRequest).IsAssignableFrom(signupDataDTO.GetType()))
            {
                CheckSessionAndInitiateAuthentication(signupDataDTO, UrlType.SignUp, onSuccess, onFailure);
            }
            else
            {
                onFailure?.Invoke(null);
            }
        }

        /// <summary>
        /// Initiate signin/signup authentication after signout of existing session (if any)
        /// </summary>
        /// <param name="authDataDTO">Object parameter for request parameter.</param>
        /// <param name="authUrlType">Type of auth URL.</param>
        /// <param name="onSuccess">Callback for authentication success.</param>
        /// <param name="onFailure">Callback for authentication failure.</param>
        private void CheckSessionAndInitiateAuthentication(IAuthenticationRequest authDataDTO, UrlType authUrlType, Action<MikrosUser> onSuccess, Action<MikrosException> onFailure)
        {
            Action authRequestOperation = () => AuthenticationRequest(authDataDTO, authUrlType, onSuccess, onFailure);
            if (!Equals(MikrosUser, null))
            {
                // Signout existing session first.
                Signout(authRequestOperation,
                    exception => onFailure?.Invoke(new MikrosException(ExceptionType.OTHER, Constants.ExistingSessionSignoutError)));
            }
            else
            {
                authRequestOperation();
            }
        }

        /// <summary>
        /// For signout api calling function.
        /// </summary>
        /// <param name="onSuccess">Callback for Signout success.</param>
        /// <param name="onFailure">Callback for Signout failure.</param>
        public void Signout(Action onSuccess, Action<MikrosException> onFailure = null)
        {
            SignoutRequest.Builder()
            .AccessToken(Session.AccessToken)
            .Create(signoutRequest =>
            {
                AuthenticationRequest(signoutRequest, UrlType.SignOut, success => onSuccess?.Invoke(), onFailure);
            },
            failure => onFailure?.Invoke(failure));
        }

        /// <summary>
        /// Initiate the authentication process.
        /// </summary>
        /// <param name="authDataDTO">Object parameter for request parameter.</param>
        /// <param name="authUrlType">Type of auth URL.</param>
        /// <param name="onSuccess">Callback for authentication success.</param>
        /// <param name="onFailure">Callback for authentication failure.</param>
        private void AuthenticationRequest(IAuthenticationRequest authDataDTO, UrlType authUrlType, Action<MikrosUser> onSuccess, Action<MikrosException> onFailure)
        {
            if (!MikrosManager.Instance.IsInitialized)
            {
                onFailure?.Invoke(new MikrosException(ExceptionType.INITIALIZE_MIKROS_SDK));
                return;
            }
            // if request parameter object is null then send a null object to it's callback method and break the current code calling.
            if (authDataDTO == null || !authDataDTO.IsCreated)
            {
                onFailure?.Invoke(new MikrosException(ExceptionType.OBJECT_NOT_CREATED));
                return;
            }

            /**
            * Get the relevant auth url from serverdata class.
            * convert the authentication data structre to json format for requesting parameter.
            * Calling the generalized post function with required parameters.
			*/
            WebRequest<AuthResponse>.Builder()
                .Url(ServerData.GetUrl(authUrlType))
                .RawJsonData(DataConverter.SerializeObject(authDataDTO))
                .CreatePostRequest(
                delegate (AuthResponse authResponse)
                {
                    // If authentication successful then call update user metadata api function.
                    if (authResponse != null)
                    {
                        if (Utils.DetectStatusType(authResponse.Status.StatusCode) == STATUS_TYPE.SUCCESS)
                        {
                            AUTH_TYPE authType;
                            bool isSignout;
                            (authType, isSignout) = DetermineAuthDetail(authUrlType);

                            if (!isSignout)
                            {
                                InitializeData(authResponse);
                                Utils.AuthUtils.StoreAuthData(authResponse);
                                SaveMikrosAccount(MikrosUserData);
                                MikrosSDKAuthEvent.Builder(authType).SetUser(MikrosUserData).Create();
                            }
                            else
                            {
                                MikrosSDKAuthEvent.Builder(authType).SetUser(MikrosUserData).Create();
                                ResetAuthentication();
                            }
                            onSuccess?.Invoke(!isSignout ? MikrosUser : null);
                        }
                        else
                            onFailure?.Invoke(new MikrosException(ExceptionType.OTHER, authResponse.Status.StatusMessage));
                    }
                    else
                        onFailure?.Invoke(new MikrosException(ExceptionType.OTHER, Constants.AuthGeneralError));
                });
        }

        /// <summary>
        /// Determine auth type from url type.
        /// </summary>
        /// <param name="authUrlType">URL type.</param>
        /// <returns>Combination of (authType, signout status).</returns>
        internal (AUTH_TYPE, bool) DetermineAuthDetail(UrlType authUrlType)
        {
            bool isSignout = false;
            AUTH_TYPE authType = AUTH_TYPE.SIGNUP;
            if (authUrlType == UrlType.SignUp)
                authType = AUTH_TYPE.SIGNUP;
            else if (authUrlType == UrlType.SignIn)
                authType = AUTH_TYPE.SIGNIN;
            else if (authUrlType == UrlType.SignOut)
            {
                authType = AUTH_TYPE.SIGNOUT;
                isSignout = true;
            }
            return (authType, isSignout);
        }

        /// <summary>
        /// Reseting all authentication data.
        /// </summary>
        internal void ResetAuthentication()
        {
            MikrosUserData = null;
            MikrosUser = null;
            Session = null;
            Utils.AuthUtils.DeleteAuthData();
        }
    }
}
