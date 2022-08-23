using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MikrosClient.Analytics
{
    /// <summary>
    /// Base interface implemented by all preset event request classes.
    /// </summary>
    public interface IPresetAnalyticsEventRequest
    {
        /// <summary>
        /// Indication of AnalyticsEvent validation success or not.
        /// </summary>
        [JsonIgnore]
        bool IsCreated { get; }

        /// <summary>
        /// Create function is used for validation of variables of the preset event request class.
        /// </summary>
        /// <param name="onSuccess">Callback for the preset event request success.</param>
        /// <param name="onFailure">Callback for the preset event request failure.</param>
        /// <returns>IPresetAnalyticsEventRequest object.</returns>
        IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure);

        /// <summary>
        /// Get the Event Name.
        /// </summary>
        [JsonIgnore]
        string GetEventName { get; }

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        [JsonIgnore]
        string GetEventEndpointName { get; }
    }

    /// <summary>
    /// Preset event request to track app open.
    /// </summary>
    internal sealed class TrackAppOpenRequest : IPresetAnalyticsEventRequest
    {
        /// <summary>
        /// Indication of TrackAppOpenRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackAppOpenRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackAppOpenRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackAppOpenRequest class.
        /// </summary>
        /// <returns>Returns new TrackAppOpenRequest object.</returns>
        public static TrackAppOpenRequest Builder()
        {
            return new TrackAppOpenRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackAppOpenRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackAppOpenRequest success.</param>
        /// <param name="onFailure">Callback for TrackAppOpenRequest failure.</param>
        /// <returns>TrackAppOpenRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
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
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_app_open";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackAppOpen";
    }

    /// <summary>
    /// Preset event request to track game over.
    /// </summary>
    public sealed class TrackGameOverRequest : IPresetAnalyticsEventRequest
    {
        /// <summary>
        /// Indication of TrackGameOverRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackGameOverRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackGameOverRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackGameOverRequest class.
        /// </summary>
        /// <returns>Returns new TrackGameOverRequest object.</returns>
        public static TrackGameOverRequest Builder()
        {
            return new TrackGameOverRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackGameOverRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackGameOverRequest success.</param>
        /// <param name="onFailure">Callback for TrackGameOverRequest failure.</param>
        /// <returns>TrackGameOverRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
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
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_game_over";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackGameOver";
    }

    /// <summary>
    /// Preset event request to track handled exceptions.
    /// This interface is to be used inside of try-catch to capture exceptions that have been handled.
    /// </summary>
    public sealed class TrackHandledExceptionRequest : IPresetAnalyticsEventRequest
    {
        internal sealed class Exception
        {
            [JsonProperty(PropertyName = "cause")]
            internal Cause cause;

            [JsonProperty(PropertyName = "detailMessage")]
            internal string detailMessage;

            [JsonProperty(PropertyName = "fullStackTrace")]
            internal List<Cause> fullStackTrace = new List<Cause>();

            [JsonProperty(PropertyName = "thread")]
            internal string thread = Constants.MainThread;
        }

        internal sealed class Cause
        {
            [JsonProperty(PropertyName = "declaringClass")]
            internal string declaringClass = string.Empty;

            [JsonProperty(PropertyName = "fileName")]
            internal string fileName = string.Empty;

            [JsonProperty(PropertyName = "isNativeMethod")]
            internal bool isNativeMethod = false;

            [JsonProperty(PropertyName = "lineNumber")]
            internal int lineNumber = 0;

            [JsonProperty(PropertyName = "methodName")]
            internal string methodName = string.Empty;
        }

        #region Request Properties

        [JsonProperty(PropertyName = "exceptionSubType")]
        private string exceptionSubType = Constants.Handled;

        [JsonProperty(PropertyName = "exception")]
        private TrackHandledExceptionRequest.Exception exception = new TrackHandledExceptionRequest.Exception();

        #endregion Request Properties

        /// <summary>
        /// Indication of TrackExceptionRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackExceptionRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackHandledExceptionRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackExceptionRequest class.
        /// </summary>
        /// <returns>Returns new TrackExceptionRequest object.</returns>
        public static TrackHandledExceptionRequest Builder()
        {
            return new TrackHandledExceptionRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackExceptionRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackExceptionRequest success.</param>
        /// <param name="onFailure">Callback for TrackExceptionRequest failure.</param>
        /// <returns>TrackExceptionRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
            // Check for parameter validation
            if (exception == null)
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Exception object not provided.");
            }
            else if (exception.cause == null || exception.fullStackTrace == null || exception.fullStackTrace.Count <= 0)
            {
                mikrosException = new MikrosException(ExceptionType.OTHER, "Invalid exception provided.");
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
        /// SetException function is used for setting the main exception for the TrackExceptionRequest class.
        /// </summary>
        /// <param name="exceptionMessage">Exception object.</param>
        /// <returns>TrackExceptionRequest object.</returns>
        public TrackHandledExceptionRequest SetException(System.Exception exception)
        {
            this.exception = GetAllFootprintsAlternate(exception);
            return this;
        }

        /// <summary>
        /// Initialize and setup the custom exception object with stack frames data.
        /// </summary>
        /// <param name="exception">Provided exception.</param>
        /// <returns>Custom exception object.</returns>
        private TrackHandledExceptionRequest.Exception GetAllFootprints(System.Exception exception)
        {
            TrackHandledExceptionRequest.Exception customException = new TrackHandledExceptionRequest.Exception();
            try
            {
                var stackTrace = new StackTrace(4, true);
                var frames = stackTrace.GetFrames();
                List<Cause> fullStackTrace = new List<Cause>();

                if (frames.Length > 0)
                {
                    foreach (var frame in frames)
                    {
                        Cause cause = new Cause();
                        string declaringClass = frame?.GetMethod().ReflectedType.FullName;
                        if (frame != null && declaringClass.Contains(nameof(MikrosClient)))
                        {
                            continue;
                        }
                        cause.declaringClass = frame.GetMethod().ReflectedType.FullName;
                        cause.fileName = GetFormattedFilename(frame.GetFileName());
                        cause.methodName = frame.GetMethod().Name;
                        cause.lineNumber = frame.GetFileLineNumber();
                        cause.isNativeMethod = Equals(StackFrame.OFFSET_UNKNOWN, frame.GetNativeOffset());
                        fullStackTrace.Add(cause);
                    }
                    customException.cause = fullStackTrace[0];
                }
                customException.detailMessage = exception.Message;
                customException.fullStackTrace = fullStackTrace;
            }
            catch (System.Exception e)
            {
                MikrosLogger.LogError(Constants.StackTraceErrorHeader + e.Message);
            }
            customException.thread = Constants.MainThread;
            return customException;
        }

        /// <summary>
        /// Get only name of file without full path.
        /// </summary>
        /// <param name="fullFilename">Full path to file.</param>
        /// <returns>Formatted file name.</returns>
        private string GetFormattedFilename(string fullFilename)
        {
            string[] filePathSplitted = fullFilename.Replace('\\', '/').Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            return filePathSplitted[filePathSplitted.Length - 1];
        }

        /// <summary>
        /// Checks if file path is encrypted or not.
        /// </summary>
        /// <param name="fullFileName">Full path to file.</param>
        /// <returns>True if file path found, else false.</returns>
        private bool IsCorrectPath(string fullFileName)
        {
            return !fullFileName.StartsWith("<");
        }

        /// <summary>
        /// Initialize and setup the custom exception object with stack frames data.
        /// (Alternate method)
        /// </summary>
        /// <param name="exception">Provided exception.</param>
        /// <returns>Custom exception object.</returns>
        private TrackHandledExceptionRequest.Exception GetAllFootprintsAlternate(System.Exception exception)
        {
            TrackHandledExceptionRequest.Exception customException = new TrackHandledExceptionRequest.Exception();
            MikrosLogger.Log("StackTrace:\n" + exception.StackTrace);
            if (string.IsNullOrEmpty(exception.StackTrace))
            {
                return customException;
            }
            List<Cause> fullStackTrace = new List<Cause>();
            string frameDivider = " at ";
            string propertiesDivider = " in ";
            string stackTrace = " " + exception.StackTrace.TrimEnd();
            string[] frames = stackTrace.Split(new string[] { frameDivider }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = frames[i].Trim();
                if (Equals(frames[i], string.Empty))
                {
                    continue;
                }
                string[] properties = frames[i].Split(new string[] { propertiesDivider }, StringSplitOptions.RemoveEmptyEntries);
                (string, string) classAndMethodName = GetMethodAndClassName(properties[0]);
                Cause cause = new Cause();
                cause.declaringClass = classAndMethodName.Item1;
                cause.methodName = classAndMethodName.Item2;
                if (properties.Length > 1)
                {
                    string[] fileData = properties[1].Split(':');
                    cause.fileName = string.Empty;
                    if (IsCorrectPath(fileData[0]))
                    {
                        string fileName = string.Empty;
                        for (int j = 0; j < fileData.Length - 1; j++)
                        {
                            fileName += fileData[j];
                        }
                        cause.fileName = GetFormattedFilename(fileName);
                    }
                    cause.lineNumber = int.Parse(fileData[fileData.Length - 1]);
                }
                cause.isNativeMethod = false;
                fullStackTrace.Add(cause);
            }
            customException.cause = fullStackTrace[fullStackTrace.Count - 1];
            customException.detailMessage = exception.Message;
            customException.fullStackTrace = fullStackTrace;
            customException.thread = Constants.MainThread;
            return customException;
        }

        /// <summary>
        /// Get class name and method name.
        /// </summary>
        /// <param name="info">Stack frame info.</param>
        /// <returns>Collection of class and method name.</returns>
        private (string, string) GetMethodAndClassName(string info)
        {
            string classname = string.Empty;
            string methodname = string.Empty;
            info = info.Trim();
            if (info.StartsWith("("))
            {
                info = info.Remove(0, info.IndexOf(')') + 1).Trim();
            }
            if (!Equals(info, string.Empty))
            {
                int indexBracketOpen = info.IndexOf('(');
                int indexBracketClose = info.IndexOf(')');
                int indexDot = 0;
                for (int i = indexBracketOpen - 1; i >= 0; i--)
                {
                    if (info[i] == '.')
                    {
                        indexDot = i;
                        break;
                    }
                }
                classname = info.Substring(0, indexDot).Trim('.');
                methodname = info.Substring(indexDot + 1, indexBracketClose - indexDot);
            }
            return (classname, methodname);
        }

        /// <summary>
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_handle_exception";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackException";
    }

    /// <summary>
    /// Preset event request to track unsuccessful HTTP request.
    /// </summary>
    public sealed class TrackHttpFailureRequest : IPresetAnalyticsEventRequest
    {
        #region Request Properties

        [JsonProperty(PropertyName = "url")]
        private string url;

        [JsonProperty(PropertyName = "statusCode")]
        private long statusCode;

        [JsonProperty(PropertyName = "message")]
        private string message;

        [JsonProperty(PropertyName = "networkSpeed")]
        private string networkSpeed;

        #endregion Request Properties

        /// <summary>
        /// Indication of TrackHttpFailureRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackHttpFailureRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackHttpFailureRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackHttpFailureRequest class.
        /// </summary>
        /// <returns>Returns new TrackHttpFailureRequest object.</returns>
        public static TrackHttpFailureRequest Builder()
        {
            return new TrackHttpFailureRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackHttpFailureRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackHttpFailureRequest success.</param>
        /// <param name="onFailure">Callback for TrackHttpFailureRequest failure.</param>
        /// <returns>TrackHttpFailureRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
            if (string.IsNullOrEmpty(url))
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Proper URL not provided");  // throw an exception about parameter is invalid.
            }
            else if (statusCode < 0) // Check for parameter validation
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Proper status code not provided");  // throw an exception about parameter is invalid.
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
        /// Url function is used for setting the "url" variable of the TrackHttpFailureRequest class.
        /// </summary>
        /// <param name="url">URL of the web request.</param>
        /// <returns>TrackHttpFailureRequest object.</returns>
        public TrackHttpFailureRequest Url(string url)
        {
            this.url = url;
            return this;
        }

        /// <summary>
        /// StatusCode function is used for setting the "statusCode" variable of the TrackHttpFailureRequest class.
        /// </summary>
        /// <param name="statusCode">Status code of the web request.</param>
        /// <returns>TrackHttpFailureRequest object.</returns>
        public TrackHttpFailureRequest StatusCode(long statusCode)
        {
            this.statusCode = statusCode; // setting the statusCode.
            return this;
        }

        /// <summary>
        /// Message function is used for setting the "message" variable of the TrackHttpFailureRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="message">Status message of the web request.</param>
        /// <returns>TrackHttpFailureRequest object.</returns>
        public TrackHttpFailureRequest Message(string message)
        {
            this.message = message; // setting the message.
            return this;
        }

        /// <summary>
        /// NetworkSpeed function is used for setting the "networkSpeed" variable of the TrackHttpFailureRequest class.
        /// </summary>
        /// <param name="networkSpeed">Network speed of device.</param>
        /// <returns>TrackHttpFailureRequest object.</returns>
        public TrackHttpFailureRequest NetworkSpeed(string networkSpeed)
        {
            this.networkSpeed = networkSpeed; // setting the networkSpeed.
            return this;
        }

        /// <summary>
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_http_failure";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackHttpFailure";
    }

    /// <summary>
    /// Preset event request to track successful HTTP request.
    /// </summary>
    public sealed class TrackHttpSuccessRequest : IPresetAnalyticsEventRequest
    {
        #region Request Properties

        [JsonProperty(PropertyName = "url")]
        private string url;

        [JsonProperty(PropertyName = "statusCode")]
        private long statusCode;

        [JsonProperty(PropertyName = "message")]
        private string message;

        [JsonProperty(PropertyName = "networkSpeed")]
        private string networkSpeed;

        #endregion Request Properties

        /// <summary>
        /// Indication of TrackHttpSuccessRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackHttpSuccessRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackHttpSuccessRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackHttpSuccessRequest class.
        /// </summary>
        /// <returns>Returns new TrackHttpSuccessRequest object.</returns>
        public static TrackHttpSuccessRequest Builder()
        {
            return new TrackHttpSuccessRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackHttpSuccessRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackHttpSuccessRequest success.</param>
        /// <param name="onFailure">Callback for TrackHttpSuccessRequest failure.</param>
        /// <returns>TrackHttpSuccessRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
            if (string.IsNullOrEmpty(url))
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Proper URL not provided");  // throw an exception about parameter is invalid.
            }
            else if (statusCode < 0) // Check for parameter validation
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Proper status code not provided");  // throw an exception about parameter is invalid.
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
        /// Url function is used for setting the "url" variable of the TrackHttpSuccessRequest class.
        /// </summary>
        /// <param name="url">URL of the web request.</param>
        /// <returns>TrackHttpSuccessRequest object.</returns>
        public TrackHttpSuccessRequest Url(string url)
        {
            this.url = url;
            return this;
        }

        /// <summary>
        /// StatusCode function is used for setting the "statusCode" variable of the TrackHttpSuccessRequest class.
        /// </summary>
        /// <param name="statusCode">Status code of the web request.</param>
        /// <returns>TrackHttpSuccessRequest object.</returns>
        public TrackHttpSuccessRequest StatusCode(long statusCode)
        {
            this.statusCode = statusCode; // setting the statusCode.
            return this;
        }

        /// <summary>
        /// Message function is used for setting the "message" variable of the TrackHttpSuccessRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="message">Status message of the web request.</param>
        /// <returns>TrackHttpSuccessRequest object.</returns>
        public TrackHttpSuccessRequest Message(string message)
        {
            this.message = message; // setting the message.
            return this;
        }

        /// <summary>
        /// NetworkSpeed function is used for setting the "networkSpeed" variable of the TrackHttpSuccessRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="networkSpeed">Network speed of device.</param>
        /// <returns>TrackHttpSuccessRequest object.</returns>
        public TrackHttpSuccessRequest NetworkSpeed(string networkSpeed)
        {
            this.networkSpeed = networkSpeed; // setting the networkSpeed.
            return this;
        }

        /// <summary>
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_http_success";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackHttpSuccess";
    }

    /// <summary>
    /// Preset event request to track level end in game.
    /// </summary>
    public sealed class TrackLevelEndRequest : IPresetAnalyticsEventRequest
    {
        #region Request Properties

        [JsonProperty(PropertyName = "level")]
        private long level;

        [JsonProperty(PropertyName = "subLevel")]
        private long subLevel;

        [JsonProperty(PropertyName = "levelName")]
        private string levelName;

        [JsonProperty(PropertyName = "description")]
        private string description;

        [JsonProperty(PropertyName = "completeDuration")]
        private string completeDuration;

        [JsonProperty(PropertyName = "success")]
        private string success;

        #endregion Request Properties

        /// <summary>
        /// Indication of TrackLevelEndRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackLevelEndRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackLevelEndRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackLevelEndRequest class.
        /// </summary>
        /// <returns>Returns new TrackLevelEndRequest object.</returns>
        public static TrackLevelEndRequest Builder()
        {
            return new TrackLevelEndRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackLevelEndRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackLevelEndRequest success.</param>
        /// <param name="onFailure">Callback for TrackLevelEndRequest failure.</param>
        /// <returns>TrackLevelEndRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
            // Check for parameter validation
            if (level < 0) // Check for parameter validation
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Level cannot be negative value");  // throw an exception about parameter is invalid.
            }
            // Check for parameter validation
            else if (subLevel < 0)
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Sub-level cannot be negative value");  // throw an exception about parameter is invalid.
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
        /// Level function is used for setting the "level" variable of the TrackLevelEndRequest class.
        /// </summary>
        /// <param name="level">Level of game.</param>
        /// <returns>TrackLevelEndRequest object.</returns>
        public TrackLevelEndRequest Level(long level)
        {
            this.level = level; // setting the level.
            return this;
        }

        /// <summary>
        /// SubLevel function is used for setting the "subLevel" variable of the TrackLevelEndRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="subLevel">Sub-level of game.</param>
        /// <returns>TrackLevelEndRequest object.</returns>
        public TrackLevelEndRequest SubLevel(long subLevel)
        {
            this.subLevel = subLevel; // setting the subLevel.
            return this;
        }

        /// <summary>
        /// LevelName function is used for setting the "levelName" variable of the TrackLevelEndRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="levelName">Name of level.</param>
        /// <returns>TrackLevelEndRequest object.</returns>
        public TrackLevelEndRequest LevelName(string levelName)
        {
            this.levelName = levelName; // setting the levelName.
            return this;
        }

        /// <summary>
        /// Description function is used for setting the "description" variable of the TrackLevelEndRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="description">Description related to the level end.</param>
        /// <returns>TrackLevelEndRequest object.</returns>
        public TrackLevelEndRequest Description(string description)
        {
            this.description = description; // setting the description.
            return this;
        }

        /// <summary>
        /// CompleteDuration function is used for setting the "completeDuration" variable of the TrackLevelEndRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="completeDuration">Time taken to complete the level.</param>
        /// <returns>TrackLevelEndRequest object.</returns>
        public TrackLevelEndRequest CompleteDuration(string completeDuration)
        {
            this.completeDuration = completeDuration; // setting the completeDuration.
            return this;
        }

        /// <summary>
        /// Success function is used for setting the "success" variable of the TrackLevelEndRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="success">Level success info.</param>
        /// <returns>TrackLevelEndRequest object.</returns>
        public TrackLevelEndRequest Success(string success)
        {
            this.success = success; // setting the success.
            return this;
        }

        /// <summary>
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_level_end";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackLevelEnd";
    }

    /// <summary>
    /// Preset event request to track level start in game.
    /// </summary>
    public sealed class TrackLevelStartRequest : IPresetAnalyticsEventRequest
    {
        #region Request Properties

        [JsonProperty(PropertyName = "level")]
        private long level;

        [JsonProperty(PropertyName = "subLevel")]
        private long subLevel;

        [JsonProperty(PropertyName = "levelName")]
        private string levelName;

        [JsonProperty(PropertyName = "description")]
        private string description;

        #endregion Request Properties

        /// <summary>
        /// Indication of TrackLevelStartRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackLevelStartRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackLevelStartRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackLevelStartRequest class.
        /// </summary>
        /// <returns>Returns new TrackLevelStartRequest object.</returns>
        public static TrackLevelStartRequest Builder()
        {
            return new TrackLevelStartRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackLevelStartRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackLevelStartRequest success.</param>
        /// <param name="onFailure">Callback for TrackLevelStartRequest failure.</param>
        /// <returns>TrackLevelStartRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
            // Check for parameter validation
            if (level < 0)
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Level cannot be negative value");  // throw an exception about parameter is invalid.
            }
            else if (subLevel < 0) // Check for parameter validation
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Sub-level cannot be negative value");  // throw an exception about parameter is invalid.
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
        /// Level function is used for setting the "level" variable of the TrackLevelStartRequest class.
        /// </summary>
        /// <param name="level">Level of game.</param>
        /// <returns>TrackLevelStartRequest object.</returns>
        public TrackLevelStartRequest Level(long level)
        {
            this.level = level; // setting the level.
            return this;
        }

        /// <summary>
        /// SubLevel function is used for setting the "subLevel" variable of the TrackLevelStartRequest class.
        /// </summary>
        /// <param name="subLevel">Sub-level of game.</param>
        /// <returns>TrackLevelStartRequest object.</returns>
        public TrackLevelStartRequest SubLevel(long subLevel)
        {
            this.subLevel = subLevel; // setting the subLevel.
            return this;
        }

        /// <summary>
        /// LevelName function is used for setting the "levelName" variable of the TrackLevelStartRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="levelName">Name of level.</param>
        /// <returns>TrackLevelStartRequest object.</returns>
        public TrackLevelStartRequest LevelName(string levelName)
        {
            this.levelName = levelName; // setting the levelName.
            return this;
        }

        /// <summary>
        /// Description function is used for setting the "description" variable of the TrackLevelStartRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="description">Description related to the level start.</param>
        /// <returns>TrackLevelStartRequest object.</returns>
        public TrackLevelStartRequest Description(string description)
        {
            this.description = description; // setting the description.
            return this;
        }

        /// <summary>
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_level_start";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackLevelStart";
    }

    /// <summary>
    /// Preset event request to track level up in game.
    /// </summary>
    public sealed class TrackLevelUpRequest : IPresetAnalyticsEventRequest
    {
        #region Request Properties

        [JsonProperty(PropertyName = "level")]
        private long level;

        [JsonProperty(PropertyName = "subLevel")]
        private long subLevel;

        [JsonProperty(PropertyName = "levelName")]
        private string levelName;

        [JsonProperty(PropertyName = "characterName")]
        private string character;

        [JsonProperty(PropertyName = "description")]
        private string description;

        #endregion Request Properties

        /// <summary>
        /// Indication of TrackLevelUpRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackLevelUpRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackLevelUpRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackLevelUpRequest class.
        /// </summary>
        /// <returns>Returns new TrackLevelUpRequest object.</returns>
        public static TrackLevelUpRequest Builder()
        {
            return new TrackLevelUpRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackLevelUpRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackLevelUpRequest success.</param>
        /// <param name="onFailure">Callback for TrackLevelUpRequest failure.</param>
        /// <returns>TrackLevelUpRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
            // Check for parameter validation
            if (level < 0)
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Level cannot be negative value");  // throw an exception about parameter is invalid.
            }
            // Check for parameter validation
            else if (subLevel < 0)
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Sub-level cannot be negative value");  // throw an exception about parameter is invalid.
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
        /// Level function is used for setting the "level" variable of the TrackLevelUpRequest class.
        /// </summary>
        /// <param name="level">Level of game.</param>
        /// <returns>TrackLevelUpRequest object.</returns>
        public TrackLevelUpRequest Level(long level)
        {
            this.level = level; // setting the level.
            return this;
        }

        /// <summary>
        /// SubLevel function is used for setting the "subLevel" variable of the TrackLevelUpRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="subLevel">Sub-level of game.</param>
        /// <returns>TrackLevelUpRequest object.</returns>
        public TrackLevelUpRequest SubLevel(long subLevel)
        {
            this.subLevel = subLevel; // setting the subLevel.
            return this;
        }

        /// <summary>
        /// LevelName function is used for setting the "levelName" variable of the TrackLevelUpRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="levelName">Name of level.</param>
        /// <returns>TrackLevelUpRequest object.</returns>
        public TrackLevelUpRequest LevelName(string levelName)
        {
            this.levelName = levelName; // setting the levelName.
            return this;
        }

        /// <summary>
        /// Character function is used for setting the "character" variable of the TrackLevelUpRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="character">Name of game character played with.</param>
        /// <returns>TrackLevelUpRequest object.</returns>
        public TrackLevelUpRequest Character(string character)
        {
            this.character = character; // setting the character.
            return this;
        }

        /// <summary>
        /// Description function is used for setting the "description" variable of the TrackLevelUpRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="description">Description related to the level-up.</param>
        /// <returns>TrackLevelUpRequest object.</returns>
        public TrackLevelUpRequest Description(string description)
        {
            this.description = description; // setting the description.
            return this;
        }

        /// <summary>
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_level_up";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackLevelUp";
    }

    /// <summary>
    /// Preset event request to track gameplay scoring.
    /// </summary>
    public sealed class TrackPostScoreRequest : IPresetAnalyticsEventRequest
    {
        #region Request Properties

        [JsonProperty(PropertyName = "score")]
        private long score;

        [JsonProperty(PropertyName = "level")]
        private long level;

        [JsonProperty(PropertyName = "subLevel")]
        private long subLevel;

        [JsonProperty(PropertyName = "levelName")]
        private string levelName;

        [JsonProperty(PropertyName = "characterName")]
        private string character;

        #endregion Request Properties

        /// <summary>
        /// Indication of TrackPostScoreRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackPostScoreRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackPostScoreRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackPostScoreRequest class.
        /// </summary>
        /// <returns>Returns new TrackPostScoreRequest object.</returns>
        public static TrackPostScoreRequest Builder()
        {
            return new TrackPostScoreRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackPostScoreRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackPostScoreRequest success.</param>
        /// <param name="onFailure">Callback for TrackPostScoreRequest failure.</param>
        /// <returns>TrackPostScoreRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
            // Check for parameter validation
            if (level < 0)
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Level cannot be negative value");  // throw an exception about parameter is invalid.
            }
            // Check for parameter validation
            else if (subLevel < 0)
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Sub-level cannot be negative value");  // throw an exception about parameter is invalid.
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
        /// Score function is used for setting the "score" variable of the TrackPostScoreRequest class.
        /// </summary>
        /// <param name="score">Score earned.</param>
        /// <returns>TrackPostScoreRequest object.</returns>
        public TrackPostScoreRequest Score(long score)
        {
            this.score = score; // setting the score.
            return this;
        }

        /// <summary>
        /// Level function is used for setting the "level" variable of the TrackPostScoreRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="level">Level of game.</param>
        /// <returns>TrackPostScoreRequest object.</returns>
        public TrackPostScoreRequest Level(long level)
        {
            this.level = level; // setting the level.
            return this;
        }

        /// <summary>
        /// SubLevel function is used for setting the "subLevel" variable of the TrackPostScoreRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="subLevel">Sub-level of game.</param>
        /// <returns>TrackPostScoreRequest object.</returns>
        public TrackPostScoreRequest SubLevel(long subLevel)
        {
            this.subLevel = subLevel; // setting the subLevel.
            return this;
        }

        /// <summary>
        /// LevelName function is used for setting the "levelName" variable of the TrackPostScoreRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="levelName">Name of level.</param>
        /// <returns>TrackPostScoreRequest object.</returns>
        public TrackPostScoreRequest LevelName(string levelName)
        {
            this.levelName = levelName; // setting the levelName.
            return this;
        }

        /// <summary>
        /// Character function is used for setting the "character" variable of the TrackPostScoreRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="character">Name of game character played with.</param>
        /// <returns>TrackPostScoreRequest object.</returns>
        public TrackPostScoreRequest Character(string character)
        {
            this.character = character; // setting the character.
            return this;
        }

        /// <summary>
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_post_score";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackPostScore";
    }

    /// <summary>
    /// Preset event request to track sharing.
    /// </summary>
    public sealed class TrackShareRequest : IPresetAnalyticsEventRequest
    {
        #region Request Properties

        [JsonProperty(PropertyName = "method")]
        private string method;

        [JsonProperty(PropertyName = "contentType")]
        private string contentType;

        #endregion Request Properties

        /// <summary>
        /// Indication of TrackShareRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackShareRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackShareRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackShareRequest class.
        /// </summary>
        /// <returns>Returns new TrackShareRequest object.</returns>
        public static TrackShareRequest Builder()
        {
            return new TrackShareRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackShareRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackShareRequest success.</param>
        /// <param name="onFailure">Callback for TrackShareRequest failure.</param>
        /// <returns>TrackShareRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
            // Check for parameter validation
            if (string.IsNullOrEmpty(method))
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Share method not provided");  // throw an exception about parameter is invalid.
            }
            else if (string.IsNullOrEmpty(contentType)) // Check for parameter validation
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Share content type not provided");  // throw an exception about parameter is invalid.
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
        /// Method function is used for setting the "method" variable of the TrackShareRequest class.
        /// </summary>
        /// <param name="method">The of share.</param>
        /// <returns>TrackShareRequest object.</returns>
        public TrackShareRequest Method(string method)
        {
            this.method = method; // setting the method.
            return this;
        }

        /// <summary>
        /// ContentType function is used for setting the "contentType" variable of the TrackShareRequest class.
        /// </summary>
        /// <param name="contentType">Content to be shared.</param>
        /// <returns>TrackShareRequest object.</returns>
        public TrackShareRequest ContentType(string contentType)
        {
            this.contentType = contentType; // setting the contentType.
            return this;
        }

        /// <summary>
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_share";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackShare";
    }

    /// <summary>
    /// Preset event request to track user signin.
    /// </summary>
    public sealed class TrackSigninRequest : IPresetAnalyticsEventRequest
    {
        #region Request Properties

        [JsonProperty(PropertyName = "method")]
        private string method;

        #endregion Request Properties

        /// <summary>
        /// Indication of TrackSigninRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackSigninRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackSigninRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackSigninRequest class.
        /// </summary>
        /// <returns>Returns new TrackSigninRequest object.</returns>
        public static TrackSigninRequest Builder()
        {
            return new TrackSigninRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackSigninRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackSigninRequest success.</param>
        /// <param name="onFailure">Callback for TrackSigninRequest failure.</param>
        /// <returns>TrackSigninRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
            if (string.IsNullOrEmpty(method)) // Check for parameter validation
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Signin medium not provided");  // throw an exception about parameter is invalid.
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
        /// Method function is used for setting the "method" variable of the TrackSigninRequest class.
        /// </summary>
        /// <param name="method">The method of Signin.</param>
        /// <returns>TrackSigninRequest object.</returns>
        public TrackSigninRequest Method(string method)
        {
            this.method = method; // setting the method.
            return this;
        }

        /// <summary>
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_signin";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackSignin";
    }

    /// <summary>
    /// Preset event request to track user signup.
    /// </summary>
    public sealed class TrackSignupRequest : IPresetAnalyticsEventRequest
    {
        #region Request Properties

        [JsonProperty(PropertyName = "method")]
        private string method;

        #endregion Request Properties

        /// <summary>
        /// Indication of TrackSignupRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackSignupRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackSignupRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackSignupRequest class.
        /// </summary>
        /// <returns>Returns new TrackSignupRequest object.</returns>
        public static TrackSignupRequest Builder()
        {
            return new TrackSignupRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackSignupRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackSignupRequest success.</param>
        /// <param name="onFailure">Callback for TrackSignupRequest failure.</param>
        /// <returns>TrackSignupRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
            // Check for parameter validation
            if (string.IsNullOrEmpty(method))
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Signup medium not provided");  // throw an exception about parameter is invalid.
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
        /// Method function is used for setting the "method" variable of the TrackSignupRequest class.
        /// </summary>
        /// <param name="method">The method of Signup.</param>
        /// <returns>TrackSignupRequest object.</returns>
        public TrackSignupRequest Method(string method)
        {
            this.method = method; // setting the method.
            return this;
        }

        /// <summary>
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_signup";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackSignup";
    }

    /// <summary>
    /// Preset event request to track other event's timer start.
    /// </summary>
    public sealed class TrackStartTimerRequest : IPresetAnalyticsEventRequest
    {
        #region Request Properties

        [JsonProperty(PropertyName = "eventKey")]
        private string eventKey;

        #endregion Request Properties

        /// <summary>
        /// Indication of TrackStartTimerRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackStartTimerRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackStartTimerRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackStartTimerRequest class.
        /// </summary>
        /// <returns>Returns new TrackStartTimerRequest object.</returns>
        public static TrackStartTimerRequest Builder()
        {
            return new TrackStartTimerRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackStartTimerRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackStartTimerRequest success.</param>
        /// <param name="onFailure">Callback for TrackStartTimerRequest failure.</param>
        /// <returns>TrackStartTimerRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
            // Check for parameter validation
            if (string.IsNullOrEmpty(eventKey))
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Valid timer event key not provided");  // throw an exception about parameter is invalid.
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
        /// Event function is used for setting the "eventKey" variable of the TrackStartTimerRequest class.
        /// </summary>
        /// <param name="presetEvent">Preset event type to track time.</param>
        /// <returns>TrackStartTimerRequest object.</returns>
        public TrackStartTimerRequest Event(EVENT presetEvent)
        {
            this.eventKey = MikrosManager.Instance.AnalyticsController.GetEventKey(presetEvent); // setting the eventKey
            return this;
        }

        /// <summary>
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_start_timer";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackStartTimer";
    }

    /// <summary>
    /// Preset event request to track other event's timer stop.
    /// </summary>
    public sealed class TrackStopTimerRequest : IPresetAnalyticsEventRequest
    {
        #region Request Properties

        [JsonProperty(PropertyName = "eventKey")]
        private string eventKey;

        #endregion Request Properties

        /// <summary>
        /// Indication of TrackStopTimerRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackStopTimerRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackStopTimerRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackStopTimerRequest class.
        /// </summary>
        /// <returns>Returns new TrackStopTimerRequest object.</returns>
        public static TrackStopTimerRequest Builder()
        {
            return new TrackStopTimerRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackStopTimerRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackStopTimerRequest success.</param>
        /// <param name="onFailure">Callback for TrackStopTimerRequest failure.</param>
        /// <returns>TrackStopTimerRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
            // Check for parameter validation
            if (string.IsNullOrEmpty(eventKey))
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Valid timer event key not provided");  // throw an exception about parameter is invalid.
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
        /// Event function is used for setting the "eventKey" variable of the TrackStopTimerRequest class.
        /// </summary>
        /// <param name="presetEvent">Preset event type to track time</param>
        /// <returns>TrackStopTimerRequest object.</returns>
        public TrackStopTimerRequest Event(EVENT presetEvent)
        {
            this.eventKey = MikrosManager.Instance.AnalyticsController.GetEventKey(presetEvent); // setting the eventKey
            return this;
        }

        /// <summary>
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_stop_timer";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackStopTimer";
    }

    /// <summary>
    /// Preset event request to track tutorial begin.
    /// </summary>
    public sealed class TrackTutorialBeginRequest : IPresetAnalyticsEventRequest
    {
        /// <summary>
        /// Indication of TrackTutorialBeginRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackTutorialBeginRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackTutorialBeginRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackTutorialBeginRequest class.
        /// </summary>
        /// <returns>Returns new TrackTutorialBeginRequest object.</returns>
        public static TrackTutorialBeginRequest Builder()
        {
            return new TrackTutorialBeginRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackTutorialBeginRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackTutorialBeginRequest success.</param>
        /// <param name="onFailure">Callback for TrackTutorialBeginRequest failure.</param>
        /// <returns>TrackTutorialBeginRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
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
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_tutorial_begin";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackTutorialBegin";
    }

    /// <summary>
    /// Preset event request to track tutorial complete.
    /// </summary>
    public sealed class TrackTutorialCompleteRequest : IPresetAnalyticsEventRequest
    {
        /// <summary>
        /// Indication of TrackTutorialCompleteRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackTutorialCompleteRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackTutorialCompleteRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackTutorialCompleteRequest class.
        /// </summary>
        /// <returns>Returns new TrackTutorialCompleteRequest object.</returns>
        public static TrackTutorialCompleteRequest Builder()
        {
            return new TrackTutorialCompleteRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackTutorialCompleteRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackTutorialCompleteRequest success.</param>
        /// <param name="onFailure">Callback for TrackTutorialCompleteRequest failure.</param>
        /// <returns>TrackTutorialCompleteRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
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
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_tutorial_complete";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackTutorialComplete";
    }

    /// <summary>
    /// Preset event request to track achievement unlock.
    /// </summary>
    public sealed class TrackUnlockAchievementRequest : IPresetAnalyticsEventRequest
    {
        #region Request Properties

        [JsonProperty(PropertyName = "achievementId")]
        private string achievementId;

        [JsonProperty(PropertyName = "achievementName")]
        private string achievementName;

        #endregion Request Properties

        /// <summary>
        /// Indication of TrackUnlockAchievementRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackUnlockAchievementRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackUnlockAchievementRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackUnlockAchievementRequest class.
        /// </summary>
        /// <returns>Returns new TrackUnlockAchievementRequest object.</returns>
        public static TrackUnlockAchievementRequest Builder()
        {
            return new TrackUnlockAchievementRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackUnlockAchievementRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackUnlockAchievementRequest success.</param>
        /// <param name="onFailure">Callback for TrackUnlockAchievementRequest failure.</param>
        /// <returns>TrackUnlockAchievementRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
            // Check for parameter validation
            if (string.IsNullOrEmpty(achievementId))
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Achievement ID not provided");  // throw an exception about parameter is invalid.
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
        /// AchievementId function is used for setting the "achievementId" variable of the TrackUnlockAchievementRequest class.
        /// </summary>
        /// <param name="achievementId">Achievement ID.</param>
        /// <returns>TrackUnlockAchievementRequest object.</returns>
        public TrackUnlockAchievementRequest AchievementId(string achievementId)
        {
            this.achievementId = achievementId; // setting the achievementId.
            return this;
        }

        /// <summary>
        /// AchievementName function is used for setting the "achievementName" variable of the TrackUnlockAchievementRequest class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="achievementName">Achievement Name.</param>
        /// <returns>TrackUnlockAchievementRequest object.</returns>
        public TrackUnlockAchievementRequest AchievementName(string achievementName)
        {
            this.achievementName = achievementName; // setting the achievementName.
            return this;
        }

        /// <summary>
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_unlock_achievement";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackUnlockAchievement";
    }

    /// <summary>
    /// Preset event request to track screen time in game.
    /// </summary>
    public sealed class TrackScreenTimeRequest : IPresetAnalyticsEventRequest
    {
        #region Request Properties
        [JsonProperty(PropertyName = "screenData")]
        internal ScreenData screenData = new ScreenData();
        internal sealed class ScreenData
        {
            [JsonProperty(PropertyName = "screenName")]
            internal string screenName;

            [JsonProperty(PropertyName = "screenClass")]
            internal string screenClass;

            [JsonProperty(PropertyName = "timeSpentOnScreen")]
            internal string timeSpentOnScreen;
        }
        #endregion Request Properties
        /// <summary>
        /// Indication of TrackScreenTimeRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackScreenTimeRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackScreenTimeRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackScreenTimeRequest class.
        /// </summary>
        /// <returns>Returns new TrackScreenTimeRequest object.</returns>
        public static TrackScreenTimeRequest Builder()
        {
            return new TrackScreenTimeRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackScreenTimeRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackScreenTimeRequest success.</param>
        /// <param name="onFailure">Callback for TrackScreenTimeRequest failure.</param>
        /// <returns>TrackScreenTimeRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
            if (string.IsNullOrEmpty(screenData.screenName))
            {
                mikrosException = mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "ScreenName not provided.");  // throw an exception about parameter is invalid.
            }
            if (string.IsNullOrEmpty(screenData.screenClass))
            {
                mikrosException = mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "ScreenClass not provided.");  // throw an exception about parameter is invalid.
            }
            if (string.IsNullOrEmpty(screenData.timeSpentOnScreen))
            {
                mikrosException = mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "TimeSpentonScreen not provided.");  // throw an exception about parameter 
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
        /// ScreenName function is used for setting the "screenName" variable of the TrackScreenTimeRequest class.
        /// </summary>
        /// <param name="screenName">screenName.</param>
        /// <returns>TrackScreenTimeRequest object.</returns>
        public TrackScreenTimeRequest ScreenName(string screenName)
        {
            this.screenData.screenName = screenName;
            return this;
        }
        /// <summary>
        /// ScreenClass function is used for setting the name of the screenClass for the TrackScreenTimeRequest class.
        /// </summary>
        /// <param name="screenClass">screenClass.</param>
        /// <returns>TrackScreenTimeRequest object.</returns>
        public TrackScreenTimeRequest ScreenClass(string screenClass)
        {
            this.screenData.screenClass = screenClass;
            return this;
        }
        /// <summary>
        /// ScreenTime function is used for setting the "timeSpentOnScreen" variable of the TrackScreenTimeRequest class.
        /// </summary>
        /// <param name="timeSpentOnScreen">screenTime.</param>
        /// <returns>TrackScreenTimeRequest object.</returns>
        public TrackScreenTimeRequest TimeSpentOnScreen(float timeSpentOnScreen)
        {
            this.screenData.timeSpentOnScreen =Mathf.Abs(Mathf.RoundToInt(timeSpentOnScreen)).ToString();
            return this;
        }

        /// <summary>
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_track_screen";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackScreen";
    }

    /// <summary>
    /// Preset event request to track purchase in game.
    /// </summary>
    public sealed class TrackPurchaseRequest : IPresetAnalyticsEventRequest
    {
        #region Request Properties
        [JsonProperty(PropertyName = "purchaseData")]
        internal PurchaseData purchaseData = new PurchaseData();
        public class PurchaseData
        {
            [JsonProperty(PropertyName = "skuName")]
            internal string skuName;

            [JsonProperty(PropertyName = "skuDescription")]
            internal string skuDescription;

            [JsonProperty(PropertyName = "skuType")]
            internal int skuType;

            [JsonProperty(PropertyName = "skuSubType")]
            internal int skuSubType;

            [JsonProperty(PropertyName = "purchaseType")]
            internal PurchaseType purchaseType;

            [JsonProperty(PropertyName = "purchaseCurrencyType")]
            internal PurchaseCurrencyType purchaseCurrencyType;

            [JsonProperty(PropertyName = "purchasePrice")]
            internal float purchasePrice;

            [JsonProperty(PropertyName = "percentDiscount")]
            internal float percentDiscount;

            [JsonProperty(PropertyName = "amountRewarded")]
            internal float amountRewarded;

            [JsonIgnore]
            internal string productDetails;

            [JsonProperty(PropertyName = "purchaseDetails")]
            internal List<PurchaseDetails> purchaseDetails = new List<PurchaseDetails>();
        }

        public class PurchaseDetails
        {
            [JsonProperty(PropertyName = "skuName")]
            internal string skuName;

            [JsonProperty(PropertyName = "skuDescription")]
            internal string skuDescription;

            [JsonProperty(PropertyName = "skuType")]
            internal int skuType;

            [JsonProperty(PropertyName = "skuSubType")]
            internal int skuSubType;

            [JsonProperty(PropertyName = "timestamp")]
            internal string timestamp;

            /// <summary>
            /// Builder function return a new object of the PurchaseDetails class.
            /// </summary>
            /// <returns>Returns new PurchaseDetails object.</returns>
            public static PurchaseDetails Builder()
            {
                return new PurchaseDetails();
            }

            /// <summary>
            /// SkuName function is used for setting the skuName for the PurchaseDetails class.
            /// </summary>
            /// <param name="skuName">Setting the value for skuName.</param>
            /// <returns>PurchaseDetails object.</returns>
            public PurchaseDetails SkuName(string skuName)
            {
                this.skuName = skuName;
                return this;
            }

            /// <summary>
            /// SkuDescription function is used for setting the skuDescription for the PurchaseDetails class.
            /// </summary>
            /// <param name="skuDescription">Setting the value for skuDescription.</param>
            /// <returns>PurchaseDetails object.</returns>
            public PurchaseDetails SkuDescription(string skuDescription)
            {
                this.skuDescription = skuDescription;
                return this;
            }

            /// <summary>
            /// PurchaseCategory function is used for setting the skuType and skuSubType for the PurchaseDetails class.
            /// </summary>
            /// <param name="purchaseCategory">Setting the value for skuType and skuSubType.</param>
            /// <returns>PurchaseDetails object.</returns>
            public PurchaseDetails PurchaseCategory(PurchaseCategory purchaseCategory)
            {
                this.skuType = purchaseCategory.purchaseType;
                this.skuSubType = purchaseCategory.purchaseSubType;
                return this;
            }

            /// <summary>
            /// Create function is used for validation of variables of the PurchaseDetails class.
            /// </summary>
            /// <returns>PurchaseDetails object.</returns>
            public PurchaseDetails Create()
            {
                timestamp = Utils.CurrentLocalTime();
                if (string.IsNullOrEmpty(skuName))
                    skuName = "";
                if (string.IsNullOrEmpty(skuDescription))
                    skuDescription = "";
                return this; // return class object if validation success.
            }
        }
        #endregion Request Properties
        /// <summary>
        /// Indication of TrackPurchaseRequest validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// TrackPurchaseRequest private constructor to restrict object creation of the class from outside.
        /// </summary>
        private TrackPurchaseRequest()
        {
        }

        /// <summary>
        /// Builder function return a new object of the TrackPurchaseRequest class.
        /// </summary>
        /// <returns>Returns new TrackPurchaseRequest object.</returns>
        public static TrackPurchaseRequest Builder()
        {
            return new TrackPurchaseRequest();
        }

        /// <summary>
        /// Create function is used for validation of variables of the TrackPurchaseRequest class.
        /// </summary>
        /// <param name="onSuccess">Callback for TrackPurchaseRequest success.</param>
        /// <param name="onFailure">Callback for TrackPurchaseRequest failure.</param>
        /// <returns>TrackPurchaseRequest object.</returns>
        public IPresetAnalyticsEventRequest Create(Action<IPresetAnalyticsEventRequest> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
            if (string.IsNullOrEmpty(purchaseData.skuName))
            {
                mikrosException = mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "SkuName not provided.");  // throw an exception about parameter is invalid.
            }
            if(purchaseData.purchasePrice<0)
            {
                mikrosException = mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Price cannot be negative value.");  // throw an exception about parameter 
            }
            if (purchaseData.percentDiscount > 100 || purchaseData.percentDiscount < 0)
            {
                mikrosException = mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Discount Percentage cannot be negative value or more that 100 %.");  // throw an exception about parameter is invalid.
            }
            if (purchaseData.amountRewarded < 0 )
            {
                mikrosException = mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Rewarded amount can't be negative.");  // throw an exception about parameter is invalid.
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
        /// SkuName function is used for setting the skuName for the TrackPurchaseRequest class.
        /// </summary>
        /// <param name="skuName">Setting the value for SkuName.</param>
        /// <returns>TrackPurchaseRequest object.</returns>
        public TrackPurchaseRequest SkuName(string skuName)
        {
            this.purchaseData.skuName = skuName;
            return this;
        }

        /// <summary>
        /// SkuDescription function is used for setting the skuDescription for the TrackPurchaseRequest class.
        /// </summary>
        /// <param name="skuDescription">Setting the value for skuDescription.</param>
        /// <returns>TrackPurchaseRequest object.</returns>
        public TrackPurchaseRequest SkuDescription(string skuDescription)
        {
            this.purchaseData.skuDescription = skuDescription;
            return this;
        }

        /// <summary>
        /// PurchaseCategory function is used for setting the skuType and skuSubType for the TrackPurchaseRequest class.
        /// For Example:
        /// <code>
        /// .PurchaseCategory(PurchaseCategory.Currency.GOLD);
        /// .PurchaseCategory(PurchaseCategory.Currency.DIAMONDS);
        /// .PurchaseCategory(PurchaseCategory.Character.OTHER);
        /// .PurchaseCategory(PurchaseCategory.CharacterSkin.EASTER);
        /// .PurchaseCategory(PurchaseCategory.Cosmetic.AUDIO);
        /// .PurchaseCategory(PurchaseCategory.Weapon.DAGGER);
        /// .PurchaseCategory(PurchaseCategory.Armor.GAUNTLET);
        /// .PurchaseCategory(PurchaseCategory.LevelUnlock.OTHER);
        /// .PurchaseCategory(PurchaseCategory.ContentUnlock.OTHER);
        /// .PurchaseCategory(PurchaseCategory.TimeReduction.OTHER);
        /// .PurchaseCategory(PurchaseCategory.Bundle.OTHER);
        /// .PurchaseCategory(PurchaseCategory.Other);
        /// </code>
        /// </summary>
        /// <param name="purchaseCategory">Setting the value for skuType and skuSubType.</param>
        /// <returns>TrackPurchaseRequest object.</returns>
        public TrackPurchaseRequest PurchaseCategory(PurchaseCategory purchaseCategory)
        {
            this.purchaseData.skuType =(int) purchaseCategory.purchaseType;
            this.purchaseData.skuSubType = purchaseCategory.purchaseSubType;
            return this;
        }

        /// <summary>
        /// PurchaseType function is used for setting the purchaseType for the TrackPurchaseRequest class.
        /// </summary>
        /// <param name="purchaseType">Setting the value for purchaseType.</param>
        /// <returns>TrackPurchaseRequest object.</returns>
        public TrackPurchaseRequest PurchaseType(PurchaseType purchaseType)
        {
            this.purchaseData.purchaseType = purchaseType;
            return this;
        }

        /// <summary>
        /// PurchaseCurrencyType function is used for setting the purchaseCurrencyType for the TrackPurchaseRequest class.
        /// </summary>
        /// <param name="purchaseCurrencyType">Setting the value for purchaseCurrencyType.</param>
        /// <returns>TrackPurchaseRequest object.</returns>
        public TrackPurchaseRequest PurchaseCurrencyType(PurchaseCurrencyType purchaseCurrencyType)
        {
            this.purchaseData.purchaseCurrencyType = purchaseCurrencyType;
            return this;
        }

        /// <summary>
        /// PurchasePrice function is used for setting the purchasePrice for the TrackPurchaseRequest class.
        /// </summary>
        /// <param name="purchasePrice">Setting the value for purchasePrice.</param>
        /// <returns>TrackPurchaseRequest object.</returns>
        public TrackPurchaseRequest PurchasePrice(float purchasePrice)
        {
            this.purchaseData.purchasePrice = purchasePrice;
            return this;
        }

        /// <summary>
        /// PercentDiscount function is used for setting the percentDiscount for the TrackPurchaseRequest class.
        /// </summary>
        /// <param name="percentDiscount">Setting the value for percentDiscount.</param>
        /// <returns>TrackPurchaseRequest object.</returns>
        public TrackPurchaseRequest PercentDiscount(float percentDiscount)
        {
            this.purchaseData.percentDiscount = percentDiscount;
            return this;
        }
        /// <summary>
        /// AmountRewarded function is used for setting the amountRewarded for the TrackPurchaseRequest class.
        /// </summary>
        /// <param name="amountRewarded">Setting the value for amountRewarded.</param>
        /// <returns>TrackPurchaseRequest object.</returns>
        public TrackPurchaseRequest AmountRewarded(float amountRewarded)
        {
            this.purchaseData.amountRewarded = amountRewarded;
            return this;
        }
        /// <summary>
        /// PurchaseDetails function is used for setting the purchaseDetails for the TrackPurchaseRequest class.
        /// </summary>
        /// <param name="purchaseDetails">Setting the value for purchaseDetails.</param>
        /// <returns>TrackPurchaseRequest object.</returns>
        public TrackPurchaseRequest PurchaseDetail(List<PurchaseDetails> purchaseDetails)
        {
            this.purchaseData.purchaseDetails = purchaseDetails;
            return this;
        }

        /// <summary>
        /// Get the Event Name.
        /// </summary>
        /// <returns>Event Name.</returns>
        public string GetEventName => "mikros_track_purchase";

        /// <summary>
        /// Get the Event endpoint name.
        /// </summary>
        /// <returns>Event endpoint name.</returns>
        public string GetEventEndpointName => "trackPurchase";
    }

    /// <summary>
    /// Data structure for default parameters for any type of event logging (Custom/Preset).
    /// </summary>
    internal sealed class AnalyticsDefaultParameters 
    {
        #region Request Properties

        [JsonProperty(PropertyName = "appGameId")]
        private string appGameId = MikrosManager.Instance.ConfigurationController.MikrosSettings.AppGameID;

        [JsonProperty(PropertyName = "apiKeyType")]
        private string apiKeyType = MikrosManager.Instance.ConfigurationController.MikrosSettings.GetApiKeyTypeString();

        [JsonProperty(PropertyName = "deviceId")]
        private string deviceId = Constants.DeviceID;

        [JsonProperty(PropertyName = "appVersion")]
        private string appVersion = Application.version;

        [JsonProperty(PropertyName = "sdkVersion")]
        private string sdkVersion = Constants.SDKVersion;

        [JsonProperty(PropertyName = "platform")]
        private string platform = Utils.GetCurrentPlatform();

        [JsonProperty(PropertyName = "timestamp")]
        private string timestamp;

        #endregion Request Properties

        /// <summary>
        /// Indication of AnalyticsDefaultParameters validation success or not.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// AnalyticsDefaultParameters private constructor to restrict object creation of the class from outside.
        /// </summary>
        private AnalyticsDefaultParameters()
        {
        }

        /// <summary>
        /// Builder function return a new object of the AnalyticsDefaultParameters class.
        /// </summary>
        /// <returns>Returns new AnalyticsDefaultParameters object.</returns>
        public static AnalyticsDefaultParameters Builder()
        {
            return new AnalyticsDefaultParameters();
        }

        /// <summary>
        /// Create function is used for validation of variables of the AnalyticsDefaultParameters class.
        /// </summary>
        /// <returns>AnalyticsDefaultParameters object.</returns>
        public AnalyticsDefaultParameters Create()
        {
            timestamp = Utils.CurrentLocalTime();

            // Check for parameter validation
            if (string.IsNullOrEmpty(appGameId) || string.IsNullOrEmpty(apiKeyType) || string.IsNullOrEmpty(deviceId) || string.IsNullOrEmpty(appVersion) || string.IsNullOrEmpty(sdkVersion) || string.IsNullOrEmpty(platform) || string.IsNullOrEmpty(timestamp))
            {
                throw new MikrosException(ExceptionType.INVALID_PARAMETER);  // throw an exception about parameter is invalid.
            }
            IsCreated = true;
            return this; // return class object if validation success.
        }
    }
}