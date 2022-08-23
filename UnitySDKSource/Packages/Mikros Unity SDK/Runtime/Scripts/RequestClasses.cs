using MikrosClient.ProfanityFilter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MikrosClient
{
    /// <summary>
    /// Data structure for Metadata upload request.
    /// </summary>
    internal sealed class Metadata
    {
        #region Request Properties

        [JsonProperty(PropertyName = "deviceId")]
        private string deviceId = Constants.DeviceID;

        [JsonProperty(PropertyName = "latitude")]
        internal string latitude = "";

        [JsonProperty(PropertyName = "longitude")]
        internal string longitude = "";

        [JsonProperty(PropertyName = "deviceModel")]
        internal string deviceModel = Constants.DeviceModel;

        [JsonProperty(PropertyName = "deviceOSVersion")]
        internal string deviceOSVersion = Constants.DeviceOSVersion;

        [JsonProperty(PropertyName = "deviceOperatingSystem")]
        internal string deviceOperatingSystem = Utils.GetCurrentPlatform();

        [JsonProperty(PropertyName = "deviceScreenDpi")]
        internal string deviceScreenDpi = Constants.ScreenDPI;

        [JsonProperty(PropertyName = "deviceScreenHeight")]
        internal string deviceScreenHeight = Constants.ScreenHeight;

        [JsonProperty(PropertyName = "deviceScreenWidth")]
        internal string deviceScreenWidth = Constants.ScreenWidth;

        [JsonProperty(PropertyName = "sdkVersion")]
        internal string sdkVersion = Constants.SDKVersion;

        [JsonProperty(PropertyName = "sdkType")]
        internal string sdkType = "Unity";

        [JsonProperty(PropertyName = "isWifi")]
        internal string isWifi = Constants.IsWifi;

        #endregion Request Properties

        /// <summary>
        /// Metadata private constructor to restrict object creation of the class from outside.
        /// </summary>
        private Metadata()
        {
        }

        /// <summary>
        /// Builder function return a new object of the Metadata class.
        /// </summary>
        /// <returns>New Metadata object.</returns>
        public static Metadata Builder()
        {
            return new Metadata();
        }

        /// <summary>
        /// Create function is used for validation of variables of the Metadata class.
        /// </summary>
        /// <returns>Metadata object.</returns>
        public Metadata Create()
        {
            return this; // return class object if validation success.
        }
    }

    /// <summary>
    /// Interface for authentication request data structures.
    /// </summary>
    public interface IAuthenticationRequest
    {
        /// <summary>
        /// Indication of AnalyticsEvent validation success or not.
        /// </summary>
        bool IsCreated { get; }

        /// <summary>
        /// Create object for request parameter.
        /// </summary>
        /// <param name="onSuccess">Callback for authentication process success.</param>
        /// <param name="onFailure">Callback for authentication process failure.</param>
        /// <returns>IAuthenticationRequest object.</returns>
        IAuthenticationRequest Create(Action<IAuthenticationRequest> onSuccess, Action<MikrosException> onFailure);
    }

    /// <summary>
    /// Sign In Data Structure for request parameter of api call.
    /// </summary>
    public sealed class SigninRequest : IAuthenticationRequest
    {
        #region Request Properties

        [JsonProperty(PropertyName = "deviceId")]
        private string deviceId = Constants.DeviceID;

        [JsonProperty(PropertyName = "username")]
        private string username;

        [JsonProperty(PropertyName = "email")]
        private string email;

        [JsonProperty(PropertyName = "password")]
        private string password;

        #endregion Request Properties

        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// SigninRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private SigninRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the SigninRequest class.
        /// </summary>
        /// <returns> Returns new SigninRequest object.</returns>
        public static SigninRequest Builder()
        {
            return new SigninRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the SigninRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for SigninRequest process success.</param>
        /// <param name="onFailure">Callback for SigninRequest process failure.</param>
        /// <returns>SigninRequest object.</returns>
        public IAuthenticationRequest Create(Action<IAuthenticationRequest> onSuccess, Action<MikrosException> onFailure)
        {
            string usernameEmailInput;
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(email))
            {
                usernameEmailInput = string.Empty;
            }
            else
            {
                if (!string.IsNullOrEmpty(username))
                {
                    usernameEmailInput = username;
                }
                else
                {
                    usernameEmailInput = email;
                }
            }
            Dictionary<string, string> errors = new Dictionary<string, string>
            {
                { "username", Utils.AuthUtils.SigninUsernameEmailError(usernameEmailInput) },
                { "password", Utils.AuthUtils.SigninPasswordError(password) }
            };
            JObject jObject = new JObject();
            foreach (var error in errors)
            {
                if (!string.IsNullOrEmpty(error.Value))
                {
                    jObject.Add(error.Key, error.Value);
                }
            }
            MikrosException mikrosException = jObject.Count <= 0 ? null : new MikrosException(ExceptionType.OTHER, jObject.ToString());
            if (!Constants.IsNetworkAvailable)
            {
                mikrosException = new MikrosException(ExceptionType.OTHER, Constants.NoNetworkError);
            }

            if (mikrosException == null)
            {
                IsCreated = true;
                onSuccess(this);
            }
            else
            {
                onFailure(mikrosException);
            }
            return this; // return class object if validation success.
        }

        /// <summary>
        /// Username function is used for setting the "username" variable of the SigninRequest class.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <returns>SigninRequest object.</returns>
        public SigninRequest Username(string username)
        {
            this.username = username; // setting the username.
            return this;
        }

        /// <summary>
        /// Email function is used for setting the "email" variable of the SigninRequest class.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <returns>SigninRequest object.</returns>
        public SigninRequest Email(string email)
        {
            this.email = email; // setting the email.
            return this;
        }

        /// <summary>
        /// Password function is used for setting the "password" variable of the SigninRequest class.
        /// </summary>
        /// <param name="password">Password.</param>
        /// <returns>SigninRequest object.</returns>
        public SigninRequest Password(string password)
        {
            this.password = password; // setting the password.
            return this;
        }
    }
    /// <summary>
    /// Mikros SSO Data Structure for request parameter of api call.
    /// </summary>
    public sealed class MikrosSSORequest
    {
        #region Request Properties

        private bool enableUsernameSpecialCharacters = MikrosManager.Instance.ConfigurationController.MikrosSettings.IsEnableUserNameSpecialCharacters;

        internal Action<MikrosUser> mikrosUserAction { get; private set; }

        internal bool IsEnableUsernameSpecialCharacters => enableUsernameSpecialCharacters;

        #endregion Request Properties
        public static MikrosSSORequest Builder()
        {
            return new MikrosSSORequest();
        }

        /// <summary>
        /// EnableUsernameSpecialCharacters function is used for setting the "enableUsernameSpecialCharacters" variable of the MikrosSSORequest class.
        /// </summary>
        /// <param name="enableUsernameSpecialCharacters"></param>
        /// <returns>MikrosSSORequest object</returns>
        public MikrosSSORequest EnableUsernameSpecialCharacters(bool enableUsernameSpecialCharacters)
        {
            this.enableUsernameSpecialCharacters = enableUsernameSpecialCharacters;
            return this;
        }
        /// <summary>
        /// MikrosUserAction function is used for setting the "mikrosUserAction" variable of the MikrosSSORequest class.
        /// </summary>
        /// <param name="mikrosUserAction"></param>
        /// <returns>MikrosSSORequest object</returns>
        public MikrosSSORequest MikrosUserAction(Action<MikrosUser> mikrosUserAction)
        {
            this.mikrosUserAction = mikrosUserAction;
            return this;
        }

        /// <summary>
        /// Create function is used for validation of variables of the MikrosSSORequest class.
        /// </summary>
        /// <returns>MikrosSSORequest object.</returns>
        public MikrosSSORequest Create()
        {
            return this;
        }
    }

    /// <summary>
    /// Sign Up Data Structure for request parameter of api call.
    /// </summary>
    public sealed class SignupRequest : IAuthenticationRequest
    {
        #region Request Properties

        [JsonProperty(PropertyName = "deviceId")]
        private string deviceId = Constants.DeviceID;

        [JsonProperty(PropertyName = "username")]
        public string username;

        [JsonProperty(PropertyName = "email")]
        private string email;

        [JsonProperty(PropertyName = "password")]
        private string password;

        [JsonIgnore]
        private bool enableUsernameSpecialCharacters;

        #endregion Request Properties

        /// <summary>
        /// Indication of Signup validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// SignupRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private SignupRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the SignupRequest class.
        /// </summary>
        /// <returns>Returns new SignupRequest object.</returns>
        public static SignupRequest Builder()
        {
            return new SignupRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the SignupRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for SignupRequest process success.</param>
        /// <param name="onFailure">Callback for SignupRequest process failure.</param>
        /// <returns>SignupRequest object.</returns>
        public IAuthenticationRequest Create(Action<IAuthenticationRequest> onSuccess, Action<MikrosException> onFailure)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>
            {
                { "username", Utils.AuthUtils.SignupUsernameError(username, enableUsernameSpecialCharacters) },
                { "email", Utils.AuthUtils.SignupEmailError(email) },
                { "password", Utils.AuthUtils.SignupPasswordError(password) }
            };
            JObject jObject = new JObject();
            foreach (var error in errors)
            {
                if (!string.IsNullOrEmpty(error.Value))
                    jObject.Add(error.Key, error.Value);
            }
            MikrosException mikrosException = jObject.Count <= 0 ? null : new MikrosException(ExceptionType.OTHER, jObject.ToString());
            if (!Constants.IsNetworkAvailable)
            {
                mikrosException = new MikrosException(ExceptionType.OTHER, Constants.NoNetworkError);
            }

            if (mikrosException == null)
            {
                IsCreated = true;
                onSuccess(this);
            }
            else
            {
                onFailure(mikrosException);
            }
            return this; // return class object if validation success.
        }

        /// <summary>
        /// Email function is used for setting the "username" variable of the SignupRequest class.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <returns>SignupRequest object.</returns>
        public SignupRequest Username(string username)
        {
            this.username = username; // setting the username.
            return this;
        }

        /// <summary>
        /// Email function is used for setting the "email" variable of the SignupRequest class.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <returns>SignupRequest object.</returns>
        public SignupRequest Email(string email)
        {
            this.email = email; // setting the email.
            return this;
        }

        /// <summary>
        /// Password function is used for setting the "password" variable of the SignupRequest class.
        /// </summary>
        /// <param name="password">Password.</param>
        /// <returns>SignupRequest object.</returns>
        public SignupRequest Password(string password)
        {
            this.password = password; // setting the password
            return this;
        }

        /// <summary>
        /// EnableUsernameSpecialCharacters function is used for setting the "enableUsernameSpecialCharacters" variable of the SignupRequest class.
        /// </summary>
        /// <param name="enableUsernameSpecialCharacters">Special Character Validation</param>
        /// <returns></returns>
        public SignupRequest EnableUsernameSpecialCharacters(bool enableUsernameSpecialCharacters)
        {
            this.enableUsernameSpecialCharacters = enableUsernameSpecialCharacters;
            return this;
        }
    }

    /// <summary>
    /// Sign out data structure for request parameter of api call.
    /// </summary>
    internal sealed class SignoutRequest : IAuthenticationRequest
    {
        #region Request Properties

        [JsonProperty(PropertyName = "deviceId")]
        private string deviceId = Constants.DeviceID;

        [JsonProperty(PropertyName = "accessToken")]
        private string accessToken;

        #endregion Request Properties

        /// <summary>
        /// Indication of Signout validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// SignoutRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private SignoutRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the SignoutRequest class.
        /// </summary>
        /// <returns>New SignoutRequest object.</returns>
        public static SignoutRequest Builder()
        {
            return new SignoutRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the SignoutRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for SignoutRequest process success.</param>
        /// <param name="onFailure">Callback for SignoutRequest process failure.</param>
        /// <returns>SignoutRequest object.</returns>
        public IAuthenticationRequest Create(Action<IAuthenticationRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
            // check for access token validation.
            if (string.IsNullOrEmpty(accessToken))
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_ACCESS_TOKEN); // throw an exception about access token is invalid.
            }
            if (!Constants.IsNetworkAvailable)
            {
                mikrosException = new MikrosException(ExceptionType.OTHER, Constants.NoNetworkError);
            }

            if (mikrosException == null)
            {
                IsCreated = true;
                onSuccess(this);
            }
            else
            {
                onFailure(mikrosException);
            }
            return this; // return class object if validation success.
        }

        /// <summary>
        /// AccessToken function is used for setting the m_accessToken variable of the SignoutRequest class.
        /// </summary>
        /// <param name="accessToken">Current user access token.</param>
        /// <returns>SignoutRequest object.</returns>
        public SignoutRequest AccessToken(string accessToken)
        {
            this.accessToken = accessToken; // setting the m_accessToken
            return this;
        }
    }

    /// <summary>
    /// GetAllAppsRequest class is used as a request parameter object of GetAllApps function.
    /// </summary>
    internal sealed class GetAllAppsRequest
    {
        [JsonProperty(PropertyName = "appGameId")]
        private string appGameId = MikrosManager.Instance.ConfigurationController.MikrosSettings.AppGameID;

        [JsonProperty(PropertyName = "accessToken")]
        private string accessToken;

        /// <summary>
        /// Indication of GetAllAppsRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool isCreated { get; private set; }

        /// <summary>
        /// GetAllAppsRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private GetAllAppsRequest()
        {
        }

        /// <summary>
        /// AccessToken function is used for setting the _accessToken variable of the GetAllAppsRequest class.
        /// </summary>
        /// <param name="accessToken">Access Token.</param>
        /// <returns>GetAllAppsRequest object.</returns>
        public GetAllAppsRequest AccessToken(string accessToken)
        {
            this.accessToken = accessToken; // setting the _accessToken
            return this;
        }

        /// <summary>
        /// Builder function return a new object of the GetAllAppsRequest class.
        /// </summary>
        /// <returns>Returns new GetAllAppsRequest object.</returns>
        public static GetAllAppsRequest Builder()
        {
            return new GetAllAppsRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the GetAllAppsRequest class.
        /// </summary>
        /// <returns>GetAllAppsRequest object.</returns>
        public GetAllAppsRequest Create()
        {
            // check for access token validation.
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new MikrosException(ExceptionType.INVALID_ACCESS_TOKEN); // throw an exception about access token is invalid.
            }

            isCreated = true;
            return this;
        }
    }
}