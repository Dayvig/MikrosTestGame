using MikrosClient.FileHandle;
using MikrosClient.ProfanityFilter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MikrosClient
{
    /// <summary>
    /// Utility class for various Mikros internal operations.
    /// </summary>
    public sealed class Utils
    {
        private const int eventNameCharacterLimit = 40; // Fixed character limit for a custom event name

        private const int minimumUsernameLength=6;
        private const int maximumUsernameLength=30;

        /// <summary>
        /// Some reserved prefixes for eventName.
        /// </summary>
        private static readonly List<string> reservedEventNamePrefixes = new List<string> { "mikros", "tatumgames", "tg" };

        /// <summary>
        /// Some reserved prefixes for parameter key.
        /// </summary>
        private static readonly List<string> reservedEventParameterKeys = new List<string> { "timestamp", "eventName" };

        /// <summary>
        /// Generate current UTC time.
        /// </summary>
        /// <returns>Current UTC time as string.</returns>
        internal static string CurrentUTCTime()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Generate current local time.
        /// </summary>
        /// <returns></returns>
        internal static string CurrentLocalTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// True if the phrase character limit is correct, otherwise false.
        /// </summary>
        /// <param name="phrase">The string phrase to check character limit.</param>
        /// <param name="limit">Allowed limit.</param>
        /// <returns></returns>
        internal static bool IsCorrectCharacterLimit(string phrase, int limit)
        {
            return phrase.Length <= limit;
        }

        /// <summary>
        /// Checks if a custom event created by integrator is valid or not.
        /// </summary>
        /// <param name="eventName">The name of the custom event.</param>
        /// <param name="data">All parameter set of the event.</param>
        /// <param name="isInternalEvent">State if its a Mikros internal event.</param>
        /// <returns></returns>
        internal static (bool, string) IsValidEvent(string eventName, Hashtable data, bool isInternalEvent = false)
        {
            string errorMessage = "";
            if (isInternalEvent && string.IsNullOrEmpty(eventName))
            {
                errorMessage = "Custom event name is null or empty";
            }
            else if (!IsCorrectCharacterLimit(eventName, eventNameCharacterLimit))
            {
                errorMessage = "Event name cannot be longer than " + eventNameCharacterLimit + " characters";
            }
            else if (!isInternalEvent && reservedEventNamePrefixes.Any(item => eventName.StartsWith(item, StringComparison.OrdinalIgnoreCase)))
            {
                errorMessage = "Phrases \"mikros\", \"tatumgames\", \"tg\" are reserved prefixes";
            }
            else if (data.Count > 0 && data.ContainsKey(""))
            {
                errorMessage = "One or more parameter key is null or empty";  // throw an exception about parameter is invalid.
            }
            else if (!isInternalEvent && data.Count > 0 && reservedEventParameterKeys.Any(item => data.Keys.Cast<object>().Any(key => key.ToString().Equals(item, StringComparison.OrdinalIgnoreCase))))
            {
                errorMessage = "Parameter Keys \"timestamp\", \"eventName\" are reserved.";
            }

            return (string.IsNullOrEmpty(errorMessage), errorMessage);
        }

        /// <summary>
        /// Generate initials of a string phrase.
        /// </summary>
        /// <param name="phrase">The string phrase to get initials of.</param>
        /// <returns>The initials in string format.</returns>
        internal static string GetInitials(string phrase)
        {
            string initials = "";
            phrase.Split(' ').ToList().ForEach(part => initials += (part.Trim().Length > 0) ? part[0] : '\0');
            return initials.Trim().ToUpper();
        }

        /// <summary>
        /// Check for empty string and assign a default value according to required format.
        /// </summary>
        /// <param name="value">String to operate.</param>
        /// <returns>Required non-empty string.</returns>
        internal static string ModifyStringIfEmpty(string value)
        {
            return string.IsNullOrEmpty(value) ? "N/A" : value;
        }

        /// <summary>
        /// Get app link for relevant platform.
        /// </summary>
        /// <param name="googlePlayStoreLink">Google Play Store Link.</param>
        /// <param name="appleAppStoreLink">Apple App Store Link.</param>
        /// <returns>Relevant app store link.</returns>
        internal static string GetRelevantStoreLink(string googlePlayStoreLink, string appleAppStoreLink)
        {
#if UNITY_ANDROID
            return googlePlayStoreLink;
#elif UNITY_IOS
			return appleAppStoreLink;
#else
			return String.Empty;
#endif
        }

        /// <summary>
        /// Set activity indicator style.
        /// </summary>
        internal static void SetActivityIndicatorStyle()
        {
#if UNITY_IOS
            Handheld.SetActivityIndicatorStyle(UnityEngine.iOS.ActivityIndicatorStyle.Gray);
#elif UNITY_ANDROID
            Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Large);
#endif
        }

        /// <summary>
        /// Detects the status type of a specific API status code.
        /// </summary>
        /// <param name="a_statusCode">The status code returned for any API request.</param>
        /// <returns>Respective Status Type.</returns>
        public static STATUS_TYPE DetectStatusType(int a_statusCode)
        {
            int firstDigit = a_statusCode / 100;
            switch (firstDigit)
            {
                case 2:
                    return STATUS_TYPE.SUCCESS;

                case 4:
                    return STATUS_TYPE.ERROR;

                default:
                    return STATUS_TYPE.UNKNOWN;
            }
        }

        /// <summary>
        /// Get current OS type.
        /// </summary>
        /// <returns>Name of the current OS as string.</returns>
        internal static string GetCurrentPlatform()
        {
            RuntimePlatform currentPlatform = RuntimePlatform.WindowsEditor;
#if UNITY_ANDROID
            currentPlatform = RuntimePlatform.Android;
#elif UNITY_IOS
            currentPlatform = RuntimePlatform.IPhonePlayer;
#elif UNITY_STANDALONE_OSX
            currentPlatform = RuntimePlatform.OSXPlayer;
#elif UNITY_STANDALONE_WIN
            currentPlatform = RuntimePlatform.WindowsPlayer;
#endif

            switch (currentPlatform)
            {
                case RuntimePlatform.Android:
                    return "android";

                case RuntimePlatform.IPhonePlayer:
                    return "ios";

                case RuntimePlatform.OSXPlayer:
                    return "osx";

                case RuntimePlatform.WindowsPlayer:
                    return "windows";

                default:
                    return "editor";
            }
        }

        /// <summary>
        /// Get relevant string for exception sub-type.
        /// </summary>
        /// <param name="exceptionSubType">Exception Sub-type.</param>
        /// <returns>String for sub-type of exception.</returns>
        internal static string GetExceptionSubTypeString(EXCEPTION_SUB_TYPE exceptionSubType)
        {
            switch (exceptionSubType)
            {
                case EXCEPTION_SUB_TYPE.HANDLED:
                    return Constants.Handled;

                case EXCEPTION_SUB_TYPE.UNHANDLED:
                    return Constants.Unhandled;

                default:
                    return Constants.Handled;
            }
        }

        /// <summary>
        /// Get current screen orientation status.
        /// </summary>
        /// <returns>Current screen orientation.</returns>
        internal static ScreenOrientation GetCurrentScreenOrientation()
        {
            return Screen.width > Screen.height ? ScreenOrientation.Landscape : ScreenOrientation.Portrait;
        }

        /// <summary>
        /// Reset all Mikros Auth data.
        /// </summary>
        public static void ResetMikrosAuth()
        {
            FileHandler.DeleteAll();
        }

        /// <summary>
        /// Auth specific utilities.
        /// </summary>
        public sealed class AuthUtils
        {
            /// <summary>
            /// Store authentication success data.
            /// </summary>
            /// <param name="authResponse">Auth response object.</param>
            internal static void StoreAuthData(AuthResponse authResponse)
            {
                SecureFileHandler.SaveToFile(authResponse, Constants.AuthDataFileName, Constants.FileEncryptKey);
            }

            /// <summary>
            /// Store authentication data.
            /// </summary>
            /// <param name="authResponse">Auth response object.</param>
            /// <returns>True if file retrieval succeeds, else False.</returns>
            internal static bool RetrieveAuthData(out AuthResponse authResponse)
            {
                return SecureFileHandler.ReadFromFile(Constants.AuthDataFileName, out authResponse, Constants.FileEncryptKey);
            }

            /// <summary>
            /// Delete file containing auth data.
            /// </summary>
            /// <returns>True if file deletion succeeds, else False.</returns>
            internal static bool DeleteAuthData()
            {
                return FileHandler.DeleteFile(Constants.AuthDataFileName);
            }

            /// <summary>
            /// Check session validity.
            /// </summary>
            /// <param name="session">Session data object.</param>
            /// <returns>True if session is active, else False.</returns>
            internal static bool IsSessionActive(SessionResponse session)
            {
                return DateTime.UtcNow.CompareTo(DateTime.Parse(session.UtcExpirationTime)) <= 0;
            }

            /// <summary>
            /// Save details of Mikros account.
            /// </summary>
            /// <param name="savedMikrosAccounts">Collection of all Mikros accounts to be saved.</param>
            internal static void SaveMikrosAccounts(List<MikrosUserData> savedMikrosAccounts)
            {
                SecureFileHandler.SaveToFile(savedMikrosAccounts, Constants.MikrosSavedAccountsFileName, Constants.FileEncryptKey);
            }

            /// <summary>
            /// Get all saved Mikros accounts for app.
            /// </summary>
            /// <param name="savedMikrosAccounts">Collection of all saved Mikros accounts.</param>
            /// <returns>True if saved accounts found, else False.</returns>
            internal static bool GetSavedMikrosAccounts(out List<MikrosUserData> savedMikrosAccounts)
            {
                return SecureFileHandler.ReadFromFile(Constants.MikrosSavedAccountsFileName, out savedMikrosAccounts, Constants.FileEncryptKey);
            }

            /// <summary>
            /// Validate email.
            /// </summary>
            /// <param name="email">Email string to check.</param>
            /// <returns>True if valid email, else False.</returns>
            internal static bool ValidateEmail(string email)
            {
                Regex mailValidator = new Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$");
                if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
                {
                    return false;
                }
                if (mailValidator.IsMatch(email))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Validate Username.
            /// </summary>
            /// <param name="username">Username string to check.</param>
            /// <param name="enableUsernameSpecialCharacters">c.</param>
            /// <returns>True if valid username, else False.</returns>
            internal static bool ValidateUsername(string username, bool enableUsernameSpecialCharacters, out string errorMessage)
            {
                Debug.LogError(enableUsernameSpecialCharacters);
                errorMessage = String.Empty;
                if (username.Length >= minimumUsernameLength && username.Length <= maximumUsernameLength)
                {
                    bool isAllLettersDigits = username.All(e => char.IsLetterOrDigit(e));
                    if(enableUsernameSpecialCharacters)
                    {
                        return true;
                    }
                    else
                    {
                        if (!isAllLettersDigits)
                            errorMessage = "Special characters not allowed in username";
                        return isAllLettersDigits;
                    }
                }
                else
                {
                    errorMessage = "Username must contain 6 to 30 alphanumeric characters";
                    return false;
                }
            }

            /// <summary>
            /// Validate Password.
            /// </summary>
            /// <param name="password">Password string to check.</param>
            /// <returns>True if valid password, else False.</returns>
            internal static bool ValidatePassword(string password)
            {
                if (password.Length >= 6)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Checks whether username or email is been given as input by user.
            /// </summary>
            /// <param name="userInput">Input of user.</param>
            /// <returns>Corresponding (username, email) set.</returns>
            public static (string, string) DetermineUsernameOrEmail(string userInput, bool isEnableUsernameSpecialCharacters)
            {
                string errorMessage = string.Empty;
                if (ValidateEmail(userInput))
                {
                    return ("", userInput);
                }
                else if (ValidateUsername(userInput, isEnableUsernameSpecialCharacters, out errorMessage))
                {
                    return (userInput, "");
                }
                else
                {
                    return ("", "");
                }
            }

            /// <summary>
            /// Check for error in signin email/username input.
            /// </summary>
            /// <param name="userInput">Input of user.</param>
            /// <returns>Details of error in signin email/username input (if any).</returns>
            internal static string SigninUsernameEmailError(string userInput)
            {
                if (string.IsNullOrEmpty(userInput))
                {
                    return Constants.ValueRequiredMessage;
                }
                else
                {
                    if (new ProfanityHandler().ContainsProfanity(userInput))
                    {
                        return Constants.InappropriateLanguageError;
                    }
                    string username = "", email = "";
                    (username, email) = DetermineUsernameOrEmail(userInput.Trim(), true);
                    if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(email))
                    {
                        return "Invalid Email/Username format";
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

            /// <summary>
            /// Check for error in signin password input.
            /// </summary>
            /// <param name="userInput">Input of user.</param>
            /// <returns>Details of error in signin password input (if any).</returns>
            internal static string SigninPasswordError(string userInput)
            {
                if (string.IsNullOrEmpty(userInput))
                {
                    return Constants.ValueRequiredMessage;
                }
                else if (string.IsNullOrWhiteSpace(userInput))
                {
                    return "The password you submitted contains spaces";
                }
                else
                {
                    return string.Empty;
                }
            }

            /// <summary>
            /// Check for error in signup username input.
            /// </summary>
            /// <param name="userInput">Input of user.</param>
            /// <returns>Details of error in signup username input (if any).</returns>
            internal static string SignupUsernameError(string userInput, bool enableUsernameSpecialCharacters)
            {
                string errorMessage = string.Empty;
                if (string.IsNullOrEmpty(userInput))
                {
                    return Constants.ValueRequiredMessage;
                }
                else if (new ProfanityHandler().ContainsProfanity(userInput))
                {
                    return Constants.InappropriateLanguageError;
                }
                else if (!ValidateUsername(userInput, enableUsernameSpecialCharacters, out errorMessage))
                {
                    return errorMessage;
                }
                else
                {
                    return string.Empty;
                }
            }

            /// <summary>
            /// Check for error in signup email input.
            /// </summary>
            /// <param name="userInput">Input of user.</param>
            /// <returns>Details of error in signup email input (if any).</returns>
            internal static string SignupEmailError(string userInput)
            {
                if (string.IsNullOrEmpty(userInput))
                {
                    return Constants.ValueRequiredMessage;
                }
                else if (new ProfanityHandler().ContainsProfanity(userInput))
                {
                    return Constants.InappropriateLanguageError;
                }
                else if (!ValidateEmail(userInput))
                {
                    return "Invalid email format";
                }
                else
                {
                    return string.Empty;
                }
            }

            /// <summary>
            /// Check for error in signup password input.
            /// </summary>
            /// <param name="userInput">Input of user.</param>
            /// <returns>Details of error in signup password input (if any).</returns>
            internal static string SignupPasswordError(string userInput)
            {
                if (string.IsNullOrEmpty(userInput))
                {
                    return Constants.ValueRequiredMessage;
                }
                else if (string.IsNullOrWhiteSpace(userInput))
                {
                    return "The password you submitted contains spaces";
                }
                else if (!ValidatePassword(userInput))
                {
                    return "Password must contain minimum 6 characters";
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
