using MikrosClient.Analytics;
using System;

namespace MikrosClient
{
    /// <summary>
    /// MikrosException class is used for log any custom exceptions.
    /// </summary>
    public sealed class MikrosException : Exception // Inherits Exception class.
    {
        /// <summary>
        /// MikrosException constuctor with ExceptionType enum parameter and a custom exception message string parameter for logging an exception.
        /// </summary>
        /// <param name="exceptionType"></param>
        /// <param name="customExceptionMessage"></param>
        public MikrosException(ExceptionType exceptionType, string customExceptionMessage = "") : base(string.IsNullOrEmpty(customExceptionMessage) ? GetExceptionMessage(exceptionType) : customExceptionMessage)
        {
            string errorMessage = string.IsNullOrEmpty(customExceptionMessage) ? GetExceptionMessage(exceptionType) : customExceptionMessage;
            if (!MikrosManager.Instance.ConfigurationController.IsEventLogging || !MikrosManager.Instance.IsInitialized)
            {
                return;
            }
            MikrosSDKExceptionEvent.Builder()
                .SetErrorDetails(errorMessage)
                .Create();
            MikrosLogger.LogError(Constants.MikrosExceptionHeader + errorMessage);
        }

        /// <summary>
        /// Get exception message with respect to exception type.
        /// </summary>
        /// <param name="exceptionType">Exception type.</param>
        /// <returns>Relevant message for the exception occured.</returns>
        private static string GetExceptionMessage(ExceptionType exceptionType)
        {
            switch (exceptionType)
            {
                case ExceptionType.INAPPROPRIATE_INPUT:
                    return Constants.InappropriateInputError;

                case ExceptionType.INVALID_PARAMETER:
                    return Constants.InvalidParameterError;

                case ExceptionType.INVALID_USERNAME:
                    return Constants.InvalidUsernameError;

                case ExceptionType.INVALID_EMAIL:
                    return Constants.InvalidEmailError;

                case ExceptionType.INVALID_PASSWORD:
                    return Constants.InvalidPasswordError;

                case ExceptionType.INVALID_ACCESS_TOKEN:
                    return Constants.InvalidAccessTokenError;

                case ExceptionType.INITIALIZE_MIKROS_SDK:
                    return Constants.MikrosInitializeSDKError;

                case ExceptionType.OBJECT_NOT_CREATED:
                    return Constants.ObjectNotCreatedError;

                case ExceptionType.INVALID_APP_GAME_ID:
                    return Constants.InvalidGameIDError;

                case ExceptionType.INVALID_API_KEY:
                    return Constants.InvalidAPIKeyError;

                case ExceptionType.SERIALIZATION:
                    return Constants.SerializationError;

                case ExceptionType.DESERIALIZATION:
                    return Constants.DeserializationError;

                case ExceptionType.DISABLED_EVENT_LOGGING:
                    return Constants.EventLoggingDisableError;

                case ExceptionType.DISABLED_ALL_TRACKING:
                    return Constants.AllTrackingDisableError;

                default:
                    return Constants.DefaultError;
            }
        }
    }
}