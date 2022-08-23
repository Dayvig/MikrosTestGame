using MikrosClient.Analytics;
using MikrosClient.NativeFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MikrosClient
{
    /// <summary>
    /// Handles all internal calls.
    /// </summary>
    internal sealed class InternalController
    {
        /// <summary>
        /// Object creation of this class only allowed internally.
        /// </summary>
        internal InternalController()
        { }

        /// <summary>
        /// Upload relevant user metadata.
        /// </summary>
        internal void UploadMetadata()
        {
            if (!MikrosManager.Instance.ConfigurationController.IsTrackUserMetadata)
            {
                MikrosLogger.LogError(Constants.MetadataTrackingDisableError);
                return;
            }
            Metadata metadata = Metadata.Builder().Create();
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
			UploadMetadataNatively(metadata);
#else
            UploadMetadataDirect(metadata);
#endif
        }

        /// <summary>
        /// Upload metadata via native framework.
        /// </summary>
        /// <param name="metadata">Metadata object to be uploaded.</param>
        private void UploadMetadataNatively(Metadata metadata)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidPluginHelper userMetadataRequest = new AndroidPluginHelper("com.tatumgames.mikros.framework.android.http.request.UpdateUserMetadataRequest$Builder");
			AndroidJavaObject userMetadataRequestObject = userMetadataRequest
				.CallMethod<AndroidJavaObject>(Constants.SetLatitude, metadata.latitude)
				.Call<AndroidJavaObject>(Constants.SetLongitude, metadata.longitude)
				.Call<AndroidJavaObject>(Constants.SetDeviceModel, metadata.deviceModel)
				.Call<AndroidJavaObject>(Constants.SetDeviceOS, metadata.deviceOperatingSystem)
				.Call<AndroidJavaObject>(Constants.SetDeviceOSVersion, metadata.deviceOSVersion)
				.Call<AndroidJavaObject>(Constants.SetDeviceScreenDpi, metadata.deviceScreenDpi)
				.Call<AndroidJavaObject>(Constants.SetDeviceScreenHeight, metadata.deviceScreenHeight)
				.Call<AndroidJavaObject>(Constants.SetDeviceScreenWidth, metadata.deviceScreenWidth)
				.Call<AndroidJavaObject>(Constants.SetSdkType, metadata.sdkType)
				.Call<AndroidJavaObject>(Constants.SetIsWifi, metadata.isWifi)
				.Call<AndroidJavaObject>(Constants.Create);
			AndroidJavaObject mikrosApiClient = MikrosManager.Instance.mikrosApiClientProvider.CallStaticMethod<AndroidJavaObject>(Constants.GetInstance);
			ResponseCallback responseCallback = new ResponseCallback(mikrosApiClient, userMetadataRequestObject);

#elif UNITY_IOS && !UNITY_EDITOR
			SwiftForUnity.UpdateUserMetadataNatively(metadata);
#endif
        }

#if UNITY_ANDROID && !UNITY_EDITOR
		/// <summary>
        /// Handle response callback for UpdateUserMetadata from native android.
        /// </summary>
		internal class ResponseCallback : AndroidJavaProxy
        {
			public ResponseCallback(AndroidJavaObject mikrosApiClient, AndroidJavaObject userMetadataRequestObject) : base("com.tatumgames.mikros.framework.android.http.response.internal.ResponseCallback")
			{
				mikrosApiClient.Call(Constants.UpdateUserMetadata, userMetadataRequestObject, this);
			}
			void onSuccess(AndroidJavaObject response)
			{
				MikrosLogger.Log(Constants.MetadataSuccessMessage);
			}

			void onFailure(AndroidJavaObject response)
            {
				MikrosLogger.Log(Constants.MetadataFailureMessage);
            }
		}
#endif

        /// <summary>
        /// Upload metadata in Editor only.
        /// </summary>
        /// <param name="metadata">Metadata object to be uploaded.</param>
        /// <param name="callback">Callback for the upload metadata process.</param>
        private void UploadMetadataDirect(Metadata metadata)
        {
            WebRequest<GeneralResponse>.Builder()
                .Url(ServerData.GetUrl(UrlType.UpdateMetaData))
                .RawJsonData(DataConverter.SerializeObject(metadata))
                .CreatePostRequest(
                delegate (GeneralResponse response)
                {
                    if (response != null && Utils.DetectStatusType(response.Status.StatusCode) == STATUS_TYPE.SUCCESS)
                    {
                        MikrosLogger.Log(Constants.MetadataSuccessMessage);
                    }
                    else
                    {
                        MikrosLogger.Log(Constants.MetadataFailureMessage);
                    }
                });
        }

        /// <summary>
        /// Track app open present event.
        /// </summary>
        internal void TrackAppOpen()
        {
            TrackAppOpenRequest.Builder()
                .Create(
                trackAppOpenRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackAppOpenRequest),
                onFailure =>
                {
                    MikrosLogger.Log(Constants.ErrorMessageGeneric);
                });
        }
    }
}
