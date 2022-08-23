using UnityEngine;

namespace MikrosClient
{
    /// <summary>
    /// Custom Logger Class.
    /// </summary>
    internal static class MikrosLogger
    {
        private const bool IsLogging = true; // IMPORTANT. Change to false before deploying.

        private static Logger loggerObject; // The Logger Object for logging.

        /// <summary>
        /// Check for logging tag that it's enabled or not.
        /// </summary>
        /// <param name="logTag">Tag used for the logging.</param>
        /// <returns>Logging enable status.</returns>
        internal static bool IsLogEnabled(LogTag logTag)
        {
            if (!IsLogging)
            {
                return false;
            }
            switch (logTag)
            {
                case LogTag.REQ_TAG:
#if REQ_TAG // Check for this tag is included in "Scripting Define Symbols"
                    return true;
#else
                    return false;
#endif
                case LogTag.RES_TAG:
#if RES_TAG // Check for this tag is included in "Scripting Define Symbols"
                    return true;
#else
                    return false;
#endif

                case LogTag.URL_TAG:
#if URL_TAG // Check for this tag is included in "Scripting Define Symbols"
                    return true;
#else
                    return false;
#endif

                default:
                    return false;
            }
        }

        /// <summary>
        /// Log a custom message.
        /// </summary>
        /// <param name="logTag">Tag of the log.</param>
        /// <param name="logType">Log type of the log.</param>
        /// <param name="logMessage">Meesage of the log.</param>
        internal static void Log(LogTag logTag, LogType logType, string logMessage)
        {
            if (!IsLogging)
            {
                return;
            }
            // If loggerObject is null then initialize it with default log handler.
            if (loggerObject == null)
            {
                loggerObject = new Logger(Debug.unityLogger.logHandler);
            }
            loggerObject.Log(logType, logTag.ToString(), logMessage); // calling the log function with Log Type, Log Tag and Log Message.
        }

        /// <summary>
        /// Log a custom message
        /// </summary>
        /// <param name="logMessage">Meesage of the log.</param>
        internal static void Log(object logMessage)
        {
            if (!IsLogging)
            {
                return;
            }
            // If loggerObject is null then initialize it with default log handler.
            if (loggerObject == null)
            {
                loggerObject = new Logger(Debug.unityLogger.logHandler);
            }
            loggerObject.Log(logMessage); // calling the log function with Log Message.
        }

        /// <summary>
        /// Log a custom error message
        /// </summary>
        /// <param name="logMessage">Meesage of the log.</param>
        internal static void LogError(object logMessage)
        {
            if (!IsLogging)
            {
                return;
            }
            // If loggerObject is null then initialize it with default log handler.
            if (loggerObject == null)
            {
                loggerObject = new Logger(Debug.unityLogger.logHandler);
            }
            loggerObject.LogError(Constants.Mikros, logMessage); // calling the log function with Log error Message.
        }
    }
}