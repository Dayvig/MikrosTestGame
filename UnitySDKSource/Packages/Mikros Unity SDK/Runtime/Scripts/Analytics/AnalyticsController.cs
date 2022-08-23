using MikrosClient.NativeFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MikrosClient.FileHandle;
using System.Runtime.CompilerServices;

// Making the internal functions of AnalyticsController to be accessed from assembly of Unit Tests for better debugging.
[assembly: InternalsVisibleTo("PlayModeTests")]
namespace MikrosClient.Analytics
{
    /// <summary>
    /// Mikros Analytics event logging controller.
    /// </summary>
    public sealed class AnalyticsController
    {
        internal List<Hashtable> editorCustomEvents = new List<Hashtable>();

        /// <summary>
        /// Preventing object creation of this class from outside.
        /// </summary>
        internal AnalyticsController()
        {
        }

        internal void Initialize()
        {
#if UNITY_EDITOR
            CheckToSendEditorCustomEvents();
#endif
            SetupOnDestroyInitaliser();
        }

        /// <summary>
        /// Setup event listener for OnDestroy.
        /// </summary>
        private void SetupOnDestroyInitaliser()
        {
#if UNITY_EDITOR
            MikrosManager.Instance.OnDestroyCallback += SaveEditorEvents;
#endif
        }

        /// <summary>
        /// Get the event key with respect to the provided preset event.
        /// </summary>
        /// <param name="presetEvent">Type of preset event.</param>
        /// <returns>Event key string.</returns>
        internal string GetEventKey(EVENT presetEvent)
        {
            IPresetAnalyticsEventRequest presetAnalyticsEvent = null;
            switch (presetEvent)
            {
                case EVENT.APP_OPEN:
                    presetAnalyticsEvent = TrackAppOpenRequest.Builder();
                    break;

                case EVENT.GAME_OVER:
                    presetAnalyticsEvent = TrackGameOverRequest.Builder();
                    break;

                case EVENT.HANDLED_EXCEPTION:
                    presetAnalyticsEvent = TrackHandledExceptionRequest.Builder();
                    break;

                case EVENT.HTTP_FAILURE:
                    presetAnalyticsEvent = TrackHttpFailureRequest.Builder();
                    break;

                case EVENT.HTTP_SUCCESS:
                    presetAnalyticsEvent = TrackHttpSuccessRequest.Builder();
                    break;

                case EVENT.LEVEL_END:
                    presetAnalyticsEvent = TrackLevelEndRequest.Builder();
                    break;

                case EVENT.LEVEL_START:
                    presetAnalyticsEvent = TrackLevelStartRequest.Builder();
                    break;

                case EVENT.LEVEL_UP:
                    presetAnalyticsEvent = TrackLevelUpRequest.Builder();
                    break;

                case EVENT.POST_SCORE:
                    presetAnalyticsEvent = TrackPostScoreRequest.Builder();
                    break;

                case EVENT.SHARE:
                    presetAnalyticsEvent = TrackShareRequest.Builder();
                    break;

                case EVENT.SIGNIN:
                    presetAnalyticsEvent = TrackSigninRequest.Builder();
                    break;

                case EVENT.SIGNUP:
                    presetAnalyticsEvent = TrackSignupRequest.Builder();
                    break;

                case EVENT.START_TIMER:
                    presetAnalyticsEvent = TrackStartTimerRequest.Builder();
                    break;

                case EVENT.STOP_TIMER:
                    presetAnalyticsEvent = TrackStopTimerRequest.Builder();
                    break;

                case EVENT.TUTORIAL_BEGIN:
                    presetAnalyticsEvent = TrackTutorialBeginRequest.Builder();
                    break;

                case EVENT.TUTORIAL_COMPLETE:
                    presetAnalyticsEvent = TrackTutorialCompleteRequest.Builder();
                    break;

                case EVENT.UNLOCK_ACHIEVEMENT:
                    presetAnalyticsEvent = TrackUnlockAchievementRequest.Builder();
                    break;

                case EVENT.TRACK_SCREEN:
                    presetAnalyticsEvent = TrackScreenTimeRequest.Builder();
                    break;

                case EVENT.TRACK_PURCHASE:
                    presetAnalyticsEvent = TrackPurchaseRequest.Builder();
                    break;
            }
            return presetAnalyticsEvent.GetEventName;
        }

        #region Preset Event Logging

        /// <summary>
        /// Log preset event with a IPresetAnalyticsEventRequest type object.
        /// </summary>
        /// <param name="presetAnalyticsEventRequest">Preset event request object.</param>
        public void LogEvent(IPresetAnalyticsEventRequest presetAnalyticsEventRequest, Action<GeneralResponse> callback = null)
        {
            if (!MikrosManager.Instance.ConfigurationController.IsEventLogging)
            {
                MikrosLogger.Log(Constants.EventLoggingDisableError);
                return;
            }
            else if (!MikrosManager.Instance.IsInitialized)
            {
                throw new MikrosException(ExceptionType.INITIALIZE_MIKROS_SDK);
            }
            string eventName = presetAnalyticsEventRequest.GetEventName;
            string eventEndpointName = presetAnalyticsEventRequest.GetEventEndpointName;

            AnalyticsDefaultParameters analyticsDefaultParameters = AnalyticsDefaultParameters.Builder().Create();

            string requestJson = DataConverter.MergeSerializeObject(analyticsDefaultParameters, presetAnalyticsEventRequest);
            LogPresetEventPostRequest(ServerData.GetUrl(UrlType.PresetAnalytics) + eventEndpointName, requestJson, callback);
        }

        /// <summary>
        /// Dispatch preset event data to backend.
        /// </summary>
        /// <param name="endpoint">The URL of event.</param>
        /// <param name="requestJson">The JSON passed as request.</param>
        /// <param name="callback">Callback for the event logging process.</param>
        private void LogPresetEventPostRequest(string endpoint, string requestJson, Action<GeneralResponse> callback = null)
        {
            Dictionary<string, string> customHeaders = new Dictionary<string, string>();
            customHeaders.Add(Constants.ApiKeyHeaderKey, MikrosManager.Instance.ConfigurationController.MikrosSettings.GetCurrentApiKey());
            WebRequest<GeneralResponse>.Builder()
                .Url(endpoint)
                .RawJsonData(requestJson)
                .CustomHeaders(customHeaders)
                .CreatePostRequest(callback);
        }

        #endregion Preset Event Logging

        #region Custom Event Logging

        /// <summary>
        /// Log custom event without any parameters.
        /// </summary>
        /// <param name="eventName">Name of custom event.</param>
        /// <param name="onSuccess">Callback for the custom event logging success.</param>
        /// <param name="onFailure">Callback for the custom event logging failure.</param>
        public void LogEvent(string eventName, Action<Hashtable> onSuccess, Action<MikrosException> onFailure = null)
        {
            Hashtable data = new Hashtable();
            LogEvent(eventName, data, onSuccess, onFailure);
        }

        /// <summary>
        /// Log custom event with a single string type parameter value.
        /// </summary>
        /// <param name="eventName">Name of custom event.</param>
        /// <param name="key">Name of the parameter.</param>
        /// <param name="value">Value of the parameter.</param>
        /// <param name="onSuccess">Callback for the custom event logging success.</param>
        /// <param name="onFailure">Callback for the custom event logging failure.</param>
        public void LogEvent(string eventName, string key, string value, Action<Hashtable> onSuccess, Action<MikrosException> onFailure = null)
        {
            Hashtable data = new Hashtable();
            data.Add(key, value);
            LogEvent(eventName, data, onSuccess, onFailure);
        }

        /// <summary>
        /// Log custom event with a single double type parameter value.
        /// </summary>
        /// <param name="eventName">Name of custom event.</param>
        /// <param name="key">Name of the parameter.</param>
        /// <param name="value">Value of the parameter.</param>
        /// <param name="onSuccess">Callback for the custom event logging success.</param>
        /// <param name="onFailure">Callback for the custom event logging failure.</param>
        public void LogEvent(string eventName, string key, double value, Action<Hashtable> onSuccess, Action<MikrosException> onFailure = null)
        {
            Hashtable data = new Hashtable();
            data.Add(key, value);
            LogEvent(eventName, data, onSuccess, onFailure);
        }

        /// <summary>
        /// Log custom event with a single long type parameter value.
        /// </summary>
        /// <param name="eventName">Name of custom event.</param>
        /// <param name="key">Name of the parameter.</param>
        /// <param name="value">Value of the parameter.</param>
        /// <param name="onSuccess">Callback for the custom event logging success.</param>
        /// <param name="onFailure">Callback for the custom event logging failure.</param>
        public void LogEvent(string eventName, string key, long value, Action<Hashtable> onSuccess, Action<MikrosException> onFailure = null)
        {
            Hashtable data = new Hashtable();
            data.Add(key, value);
            LogEvent(eventName, data, onSuccess, onFailure);
        }

        /// <summary>
        /// Log custom event with a Hashtable containing one or more parameters.
        /// </summary>
        /// <param name="eventName">Name of custom event.</param>
        /// <param name="data">Collection of parameters.</param>
        /// <param name="onSuccess">Callback for the custom event logging success.</param>
        /// <param name="onFailure">Callback for the custom event logging failure.</param>
        public void LogEvent(string eventName, Hashtable data, Action<Hashtable> onSuccess, Action<MikrosException> onFailure = null)
        {
            MikrosException mikrosException = DetermineException(eventName, data);
            if (mikrosException == null)
            {
                PostDataCollection(false,
                    () => LogDataSuccess(eventName.Trim(), data, onSuccess),
                    onFailure);
            }
            else
            {
                onFailure?.Invoke(mikrosException);
            }
        }

        /// <summary>
        /// For logging internal events having "mikros" prefix.
        /// </summary>
        /// <param name="eventName">Name of custom event.</param>
        /// <param name="data">Collection of parameters.</param>
        /// <param name="onSuccess">Callback for the custom event logging success.</param>
        /// <param name="onFailure">Callback for the custom event logging failure.</param>
        internal void LogInternalEvent(string eventName, Hashtable data, Action<Hashtable> onSuccess, Action<MikrosException> onFailure = null)
        {
            MikrosException mikrosException = DetermineException(eventName, data, true);
            if (mikrosException == null)
            {
                PostDataCollection(false,
                    () => LogDataSuccess(eventName.Trim(), data, onSuccess),
                    onFailure);
            }
            else
            {
                onFailure?.Invoke(mikrosException);
            }
        }

        /// <summary>
        /// Starts a coroutine to wait for Mikros Initialization success before proceeding to custom event logging or flushing.
        /// </summary>
        /// <param name="isFlush">Indicate if custom events are to be flushed.</param>
        /// <param name="onSuccess">Callback for the custom event logging success.</param>
        /// <param name="onFailure">Callback for the custom event logging failure.</param>
        private void PostDataCollection(bool isFlush, Action onSuccess, Action<MikrosException> onFailure = null)
        {
            MikrosManagerInitializer.Instance.StartCoroutine(WaitForSDKInitialization(isFlush,
                () => onSuccess(),
                () => onFailure?.Invoke(new MikrosException(ExceptionType.INITIALIZE_MIKROS_SDK))
            ));
        }

        /// <summary>
        /// Couroutine to wait for Mikros Initialization.
        /// </summary>
        /// <param name="isFlush">Indicate if custom events are to be flushed.</param>
        /// <param name="onSuccess">Callback for the custom event logging success.</param>
        /// <param name="onFailure">Callback for the custom event logging failure.</param>
        /// <returns></returns>
        private IEnumerator WaitForSDKInitialization(bool isFlush, Action onSuccess, Action onFailure)
        {
            bool isInitialized = MikrosManager.Instance != null && MikrosManager.Instance.IsInitialized;
            float waitInterval = 0.2f, totalTimeProgress = 0, waitTime = 6f;
            while (!isInitialized && totalTimeProgress < waitTime)
            {
                yield return new WaitForSeconds(waitInterval);
                isInitialized = MikrosManager.Instance != null && MikrosManager.Instance.IsInitialized;
                totalTimeProgress += waitInterval;
            }
            if (isInitialized)
            {
                if (isFlush)
                {
                    // Flushing one frame after in order to gather any events that might be logged in current frame.
                    yield return null;
                }
                onSuccess();
            }
            else
            {
                onFailure?.Invoke();
            }
            yield break;
        }

        /// <summary>
        /// Determine any exception while logging the event.
        /// </summary>
        /// <param name="eventName">Name of custom event.</param>
        /// <param name="data">Collection of parameters.</param>
        /// <returns>Exception object.</returns>
        private MikrosException DetermineException(string eventName, Hashtable data, bool isInternalEvent = false)
        {
            MikrosException mikrosException = null;
            (bool, string) eventDataValidityCheck = Utils.IsValidEvent(eventName.Trim(), data, isInternalEvent);
            if (!MikrosManager.Instance.ConfigurationController.IsAllTrackingEnabled)
            {
                mikrosException = new MikrosException(ExceptionType.DISABLED_ALL_TRACKING);
            }
            else if (!MikrosManager.Instance.ConfigurationController.IsEventLogging)
            {
                mikrosException = new MikrosException(ExceptionType.DISABLED_EVENT_LOGGING);
            }
            else if (!eventDataValidityCheck.Item1)
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, eventDataValidityCheck.Item2);
            }
            return mikrosException;
        }

        /// <summary>
        /// Custom event logging success activity.
        /// </summary>
        /// <param name="eventName">Name of custom event.</param>
        /// <param name="data">Collection of parameters.</param>
        /// <param name="onSuccess">Callback for the custom event logging success.</param>
        private void LogDataSuccess(string eventName, Hashtable data, Action<Hashtable> onSuccess)
        {
            string eventNameKeyString = Constants.EventNameKey;
            string timestampKeyString = Constants.TimestampKey;

            if (data.ContainsKey(eventNameKeyString))
                data.Remove(eventNameKeyString);
            if (data.ContainsKey(timestampKeyString))
                data.Remove(timestampKeyString);

            data.Add(eventNameKeyString, eventName);
#if UNITY_IOS || UNITY_EDITOR
            data.Add(timestampKeyString, Utils.CurrentLocalTime());
#endif
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
			LogEventViaNativePlugin(data);
#elif UNITY_EDITOR
            editorCustomEvents.Add(data);
            if (editorCustomEvents.Count >= Constants.EditorCustomEventsBatchCount)
            {
                FlushEvents();
            }
#endif
            onSuccess(data);
        }

        #region Editor Custom Events
#if UNITY_EDITOR
        /// <summary>
        /// Sends events directly from editor.
        /// </summary>
        private void LogEventEditor()
        {
            if(editorCustomEvents.Count <= 1)
            {
                return;
            }
            CustomEventsPostRequest(editorCustomEvents, response =>
            {
                MikrosLogger.Log(Constants.EditorCustomEventAddMessage);
            });
            editorCustomEvents.Clear();
        }

        /// <summary>
        /// Save batched custom editor events to PlayerPrefs.
        /// </summary>
        private void SaveEditorEvents()
        {
            if (editorCustomEvents.Count > 0)
            {
                FileHandler.SaveToPlayerPrefs(Constants.EditorHashTable, DataConverter.SerializeObject(editorCustomEvents));
            }
            MikrosManager.Instance.OnDestroyCallback -= SaveEditorEvents;
        }

        /// <summary>
        /// Checks and flushes events (for editor only).
        /// </summary>
        private void CheckToSendEditorCustomEvents()
        {
            string editorData = "";
            FileHandler.ReadFromPlayerPrefs(Constants.EditorHashTable, out editorData);
            if (!string.IsNullOrEmpty(editorData))
            {
                editorCustomEvents = DataConverter.DeserializeObject<List<Hashtable>>(editorData);
                FlushEvents();
                FileHandler.SaveToPlayerPrefs(Constants.EditorHashTable, string.Empty);
            }
        }

        /// <summary>
        /// (EDITOR only) For Custom Events Testing purpose only during development phase.
        /// N.B: On successful event uploading, you can see the result in Mikros Web Console.
        /// N.B: Recommended to use Development or QA API Key for all testing purposes.
        /// </summary>
        /// <param name="customEvents">Collection of custom events.</param>
        /// <param name="callback">Callback for the custom events' logging.</param>
        internal void TestCustomEvents(List<Hashtable> customEvents, Action<GeneralResponse> callback)
        {
            PostDataCollection(false,
            () => CustomEventsPostRequest(customEvents, callback),
            (exception) => MikrosLogger.Log(Constants.ExceptionCustom + ": " + exception.Message));
        }

        /// <summary>
        /// Dispatch custom event data to backend. (For internal testing purpose only)
        /// </summary>
        /// <param name="customEventsQueue">Collection of custom events.</param>
        /// <param name="callback">Callback for the custom events' logging.</param>
        private void CustomEventsPostRequest(List<Hashtable> customEventsQueue, Action<GeneralResponse> callback = null)
        {
            Dictionary<string, string> customHeaders = new Dictionary<string, string>();
            customHeaders.Add(Constants.ApiKeyHeaderKey, MikrosManager.Instance.ConfigurationController.MikrosSettings.GetCurrentApiKey());
            Dictionary<string, List<Hashtable>> events = new Dictionary<string, List<Hashtable>>();
            events.Add(Constants.EventsKey, customEventsQueue);
            AnalyticsDefaultParameters analyticsDefaultParameters = AnalyticsDefaultParameters.Builder().Create();

            string requestJson = DataConverter.MergeSerializeObject(analyticsDefaultParameters, events);
            MikrosLogger.Log(Constants.CustomJson + ": \n" + requestJson);

            Action<GeneralResponse> queueClearAction = response =>
            {
                callback?.Invoke(response);
            };
            WebRequest<GeneralResponse>.Builder()
                .Url(ServerData.GetUrl(UrlType.CustomAnalytics))
                .RawJsonData(requestJson)
                .CustomHeaders(customHeaders)
                .CreatePostRequest(queueClearAction);
        }
#endif
        #endregion Editor Custom Events

        /// <summary>
        /// Transfer the collected custom event data to native plugin for final flushing at feasible time.
        /// </summary>
        /// <param name="data">Collection of parameters.</param>
        private void LogEventViaNativePlugin(Hashtable data)
        {
            string customEventData = DataConverter.SerializeObject<Hashtable>(data);
            MikrosLogger.Log(Constants.CustomEventData + ":\n" + customEventData);
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidPluginHelper mikrosEvent = new AndroidPluginHelper("com.tatumgames.mikros.framework.android.http.request.MikrosEvent$Builder");
            AndroidJavaObject mikrosEventObject = mikrosEvent.CallMethod<AndroidJavaObject>(Constants.SetEvent, customEventData)
                .Call<AndroidJavaObject>(Constants.Create);
            AndroidJavaObject mikrosApiClient = MikrosManager.Instance.mikrosApiClientProvider.CallStaticMethod<AndroidJavaObject>(Constants.GetInstance);
            mikrosApiClient.Call(Constants.LogEvent, mikrosEventObject);

#elif UNITY_IOS && !UNITY_EDITOR
			SwiftForUnity.LogEventNatively(customEventData);
#endif
            if (MikrosManager.Instance.ConfigurationController.IsEventLogging)
            {
                MikrosLogger.Log(Constants.CustomEventSuccessful);
            }
            else
            {
                MikrosLogger.Log(Constants.EventLoggingDisableError);
            }
        }

        /// <summary>
        /// Uploads custom events to backend.
        /// </summary>
        public void FlushEvents()
        {
            PostDataCollection(true,
                () =>
                {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
					FlushEventsViaNativePlugin();
#elif UNITY_EDITOR
                    LogEventEditor();
#endif
                });
        }

        /// <summary>
        /// Uploads custom events to backend via native plugin.
        /// </summary>
        private void FlushEventsViaNativePlugin()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaObject mikrosApiClient = MikrosManager.Instance.mikrosApiClientProvider.CallStaticMethod<AndroidJavaObject>(Constants.GetInstance);
            mikrosApiClient.Call(Constants.FlushEvents);

#elif UNITY_IOS && !UNITY_EDITOR
			SwiftForUnity.FlushEventsNatively();
#endif
            if (MikrosManager.Instance.ConfigurationController.IsEventLogging)
            {
                MikrosLogger.Log(Constants.FlushedEventSuccessful);
            }
            else
            {
                MikrosLogger.Log(Constants.EventLoggingDisableError);
            }
        }

        #endregion Custom Event Logging
    }
}