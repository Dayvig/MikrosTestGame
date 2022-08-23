using MikrosClient.Analytics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace MikrosClient
{
    /// <summary>
    /// Handle all web related operations.
    /// </summary>
    internal sealed class WebRequest<T>
    {
        /// <summary>
        /// Target URL path of an API.
        /// </summary>
        private string url;

        /// <summary>
        /// Request parameter in JSON format.
        /// </summary>
        private string rawJsonData;

        /// <summary>
        /// Headers to include in the web request.
        /// </summary>
        private Dictionary<string, string> customHeaders;

        /// <summary>
        /// Private default constructor to restrict object creation.
        /// </summary>
        private WebRequest()
        { 
        }

        /// <summary>
        /// Builder function return a new object of the WebRequest class.
        /// </summary>
        /// <returns>New WebRequest object.</returns>
        internal static WebRequest<T> Builder()
		{
            return new WebRequest<T>();
		}

        /// <summary>
        /// Set target URL path of an API.
        /// </summary>
        /// <param name="url">Target URL path of an API.</param>
        /// <returns>WebRequest object.</returns>
        internal WebRequest<T> Url(string url)
		{
            this.url = url;
            return this;
		}

        /// <summary>
        /// Set request parameter in JSON format.
        /// </summary>
        /// <param name="rawJsonData">Request parameter in JSON format.</param>
        /// <returns>WebRequest object.</returns>
        internal WebRequest<T> RawJsonData(string rawJsonData)
		{
            this.rawJsonData = rawJsonData;
            return this;
		}

        /// <summary>
        /// Set headers to include in the web request.
        /// </summary>
        /// <param name="customHeaders">Headers to include in the web request.</param>
        /// <returns>WebRequest object.</returns>
        internal WebRequest<T> CustomHeaders(Dictionary<string, string> customHeaders)
		{
            if(customHeaders != null && customHeaders.Count > 0)
            {
                this.customHeaders = new Dictionary<string, string>(customHeaders);
            }
            return this;
		}

		/// <summary>
		/// Initiate a POST web request.
		/// </summary>
		/// <param name="callback">API Response callback data with Generic type object.</param>
		internal void CreatePostRequest(Action<T> callback = null)
		{
            #region ForLogger

            if(MikrosLogger.IsLogEnabled(LogTag.URL_TAG))
            {
                MikrosLogger.Log(LogTag.URL_TAG, LogType.Log, "Url Data : " + url);
            }
            if(MikrosLogger.IsLogEnabled(LogTag.REQ_TAG))
            {
                MikrosLogger.Log(LogTag.REQ_TAG, LogType.Log, "Request :: " + url + "\nJSON :: " + rawJsonData);
            }

            #endregion ForLogger

            UnityWebRequest unityWebRequest = UnityWebRequest.Put(url, rawJsonData); // Creating a web request with given url and raw json data
            unityWebRequest.method = UnityWebRequest.kHttpVerbPOST; // setting web request method type
            unityWebRequest.SetRequestHeader(Constants.ContentTypeKey, Constants.JsonWebContent); // setting content type of web request.
            unityWebRequest.SetRequestHeader(Constants.AcceptTypeKey, Constants.JsonWebContent); // setting accept type of web request.
            if(customHeaders != null && customHeaders.Count > 0)
            {
                foreach(var header in customHeaders)
                {
                    unityWebRequest.SetRequestHeader(header.Key, header.Value);
                }
            }
            MikrosManagerInitializer.Instance.StartCoroutine(PostRoutine(unityWebRequest, callback)); // Start a coroutine for sending web request.
        }

		/// <summary>
		/// Initiate downloading an image from a given url.
		/// </summary>
		/// <param name="callback">Callback is an Action or delegate with downloaded texture2d parameter.</param>
		internal void CreateDownloadTextureRequest(Action<Texture2D> callback)
		{
			MikrosManagerInitializer.Instance.StartCoroutine(DownloadImageCorutine(url, callback)); // Start the downloading corutine.
		}

		/// <summary>
		/// Generalized coroutine for POST request of an API.
		/// </summary>
		/// <typeparam name="T">Generic type object.</typeparam>
		/// <param name="unityWebRequest">Web request object for an api request.</param>
		/// <param name="callback">Api Response call back data with Generic type object.</param>
		/// <returns>Coroutine.</returns>
		private IEnumerator PostRoutine(UnityWebRequest unityWebRequest, Action<T> callback)
		{
			float startTime = Time.unscaledTime;
			yield return unityWebRequest.SendWebRequest(); // sending web request.
			MikrosLogger.Log(LogTag.RES_TAG, LogType.Log, unityWebRequest.responseCode.ToString() + "\nURL :: " + unityWebRequest.url);

			// check that web request has no network error and done.
			if(!unityWebRequest.isNetworkError && unityWebRequest.isDone)
			{
				#region ForLogger

				if(MikrosLogger.IsLogEnabled(LogTag.RES_TAG))
				{
					MikrosLogger.Log(LogTag.RES_TAG, LogType.Log, "Response :: " + unityWebRequest.url + "\nJSON :: " + unityWebRequest.downloadHandler.text);
				}

				#endregion ForLogger

				// if callback is not null then returning the response text to callabck method.
				if(callback != null)
				{
					callback(DataConverter.DeserializeObject<T>(unityWebRequest.downloadHandler.text));
				}
			}
			else
			{
				// if callback is not null and there is some error then send a null string to callback method.
				if(callback != null)
				{
					callback(DataConverter.DeserializeObject<T>(""));
				}

				#region ForLogger

				if(MikrosLogger.IsLogEnabled(LogTag.RES_TAG))
				{
					MikrosLogger.Log(LogTag.RES_TAG, LogType.Error, "Response Error : " + unityWebRequest.error);
				}

				#endregion ForLogger
			}
			float responseTime = Time.unscaledTime - startTime;
			if(!unityWebRequest.isNetworkError)
			{
				HandleHTTPEvents(unityWebRequest, responseTime);
			}
		}

		/// <summary>
		/// DownloadImage is generalized corutine to download an image from a given url.
		/// </summary>
		/// <param name="imageUrl">Image url of the image.</param>
		/// <param name="callback">Callback is an Action or delegate with downloaded texture2d parameter.</param>
		/// <returns>Coroutine.</returns>
		private IEnumerator DownloadImageCorutine(string imageUrl, Action<Texture2D> callback)
		{
			UnityWebRequest unityWebRequest = UnityWebRequest.Get(imageUrl); // Creating a web request with given url
			unityWebRequest.method = UnityWebRequest.kHttpVerbGET;

			float startTime = Time.unscaledTime;
			yield return unityWebRequest.SendWebRequest(); // Sending web request.

			// Check that web request has no network error and done.
			if(!unityWebRequest.isNetworkError && unityWebRequest.isDone)
			{
				Texture2D texture2D = new Texture2D(2, 2); // Creating a blank texture2d.
				texture2D.LoadImage(unityWebRequest.downloadHandler.data); // Loading the image to txture2D from downloaded bytes.
				callback(texture2D); // Calling the attached callback method with final texture2d.
			}
			else
			{
				callback?.Invoke(null);
			}
			float responseTime = Time.unscaledTime - startTime;
			if(!unityWebRequest.isNetworkError)
			{
				HandleHTTPEvents(unityWebRequest, responseTime, true);
			}
		}

		/// <summary>
		/// Handle all events to be logged regarding HTTP requests.
		/// </summary>
		/// <param name="unityWebRequest">Web request object of Unity.</param>
		/// <param name="responseTime">Response time of API.</param>
		private void HandleHTTPEvents(UnityWebRequest unityWebRequest, float responseTime, bool isTextureRequest = false)
		{
			bool sendPresetEvents = true;
			string url = unityWebRequest.url;
			List<string> ignoreEndpoints = new List<string>() { "trackHttpSuccess", "trackHttpFailure" };
			if(ignoreEndpoints.Any(item => url.Contains(item)))
				sendPresetEvents = false;

			int statusCode = 0;
			string statusMessage = "";
			if(!unityWebRequest.isNetworkError)
			{
				if(!isTextureRequest)
				{
					try
					{
						JObject responseJson = JObject.Parse(unityWebRequest.downloadHandler.text);
						JToken statusCodeRaw = responseJson["status"]["status_code"];
						JToken statusMessageRaw = responseJson["status"]["status_message"];
						statusCode = statusCodeRaw != null ? (int) statusCodeRaw : (int) unityWebRequest.responseCode;
						statusMessage = statusMessageRaw != null ? (string) statusMessageRaw : Constants.ErrorMessageGeneric;
					}
					catch(Exception e)
					{
						if(!url.Contains("trackHandledException"))
							new MikrosException(ExceptionType.OTHER, e.Message);
						else
							sendPresetEvents = false;
						statusCode = (int) unityWebRequest.responseCode;
						statusMessage = Constants.ErrorMessageGeneric;
					}
				}
				else
				{
					statusCode = (int) unityWebRequest.responseCode;
					statusMessage = Constants.SuccessMessageGeneric;
				}
			}
			if(unityWebRequest.isNetworkError || unityWebRequest.isHttpError || Utils.DetectStatusType(statusCode) != STATUS_TYPE.SUCCESS)
			{
				string errorMessage = unityWebRequest.error != null ? unityWebRequest.error : statusMessage;
				if(sendPresetEvents)
				{
					MikrosSDKApiFailureEvent.Builder().SetUrl(url).SetStatusCode(statusCode).SetErrorDetails(errorMessage).SetResponseTime(responseTime).Create();
					TrackHttpFailureRequest.Builder()
						.Url(url)
						.Message(errorMessage)
						.StatusCode(statusCode)
						.Create(requestObject =>
						{
							MikrosManager.Instance.AnalyticsController.LogEvent(requestObject);
						},
						failureException =>
						{
							MikrosLogger.Log(failureException.Message);
						});
				}
			}
			else
			{
				if(sendPresetEvents)
				{
					MikrosSDKApiSuccessEvent.Builder().SetUrl(url).SetStatusCode(statusCode).SetStatusMessage(statusMessage).SetResponseTime(responseTime).Create();
					TrackHttpSuccessRequest.Builder()
						.Url(url)
						.Message(statusMessage)
						.StatusCode(statusCode)
						.Create(requestObject =>
						{
							MikrosManager.Instance.AnalyticsController.LogEvent(requestObject);
						},
						failureException =>
						{
							MikrosLogger.Log(failureException.Message);
						});
				}
			}
		}
	}
}
