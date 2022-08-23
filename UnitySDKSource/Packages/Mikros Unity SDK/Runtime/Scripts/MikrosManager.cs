using MikrosClient.Analytics;
using MikrosClient.Authentication;
using MikrosClient.NativeFramework;
using MikrosClient.GameService;
using System;
using System.Collections;
using UnityEngine;
using MikrosClient.Ad;
using MikrosClient.Config;

namespace MikrosClient
{
	/// <summary>
	/// This class is for core operations of Mikros SDK.
	/// </summary>
	[DefaultExecutionOrder(-9)]
	public sealed class MikrosManager
	{
		/// <summary>
		/// Private instance of the class.
		/// </summary>
		private static MikrosManager mikrosManagerInstance;

#if UNITY_ANDROID && !UNITY_EDITOR
		/// <summary>
		/// Reference of object of class in Mikros native framework for android.
		/// </summary>
		internal AndroidPluginHelper mikrosApiClientProvider;
#endif

		/// <summary>
		/// Instance of the InternalController class.
		/// </summary>
		internal InternalController InternalController = new InternalController();

		/// <summary>
		/// Access all configuration related interfaces.
		/// </summary>
		public ConfigurationController ConfigurationController = new ConfigurationController();

		/// <summary>
		/// Access all user authentication related interfaces.
		/// </summary>
		public AuthenticationController AuthenticationController = new AuthenticationController();

		/// <summary>
		/// Access all analytics related interfaces.
		/// </summary>
		public AnalyticsController AnalyticsController = new AnalyticsController();

		/// <summary>
		/// Access all game-service related interfaces.
		/// </summary>
		public GameServiceController GameServiceController = new GameServiceController();

		/// <summary>
		/// Access all ads related interfaces.
		/// Note: In development. Watch out for this in future versions.
		/// </summary>
		public AdController AdController = new AdController();

		/// <summary>
		/// Returns true if Mikros is initialized correctly, else false.
		/// </summary>
		public bool IsInitialized { get; private set; }

		/// <summary>
		/// Action to handle callback of monobehaviour destroy.
		/// </summary>
		internal Action OnDestroyCallback= null;

		/// <summary>
		/// Public instance of MikrosManager class.
		/// </summary>
		public static MikrosManager Instance
		{
			get
			{
				// instantiate the class if the class object is null.
				if(mikrosManagerInstance == null)
				{
					SetInstance();
				}
				return mikrosManagerInstance; // Return the MikrosManager object.
			}
		}

		/// <summary>
		/// Define private constructor for singleton class.
		/// </summary>
		private MikrosManager()
		{
			if(mikrosManagerInstance == null)
			{
				mikrosManagerInstance = this;
			}
		}

		/// <summary>
		/// Set Singleton instance of MikrosManager.
		/// </summary>
		internal static void SetInstance()
		{
			if (Equals(mikrosManagerInstance, null))
			{
				new MikrosManager();
				mikrosManagerInstance.InitialConfigurations();
			}
		}

		/// <summary>
		/// Initializing important assets after MikrosManager instance is created successfully.
		/// </summary>
		private void InitialConfigurations()
		{			
			ConfigurationController.LoadMikrosSettings();
			AuthenticationController.SetupEssentialData();
			AnalyticsController.Initialize();
			if(ConfigurationController.MikrosSettings.IsAutoInitialize)
			{
				InitializeMikrosSDK();
			}
		}

		/// <summary>
		/// Initialize the Mikros Unity SDK.
		/// (To be done every time at app start, if "Auto Initialize Mikros SDK" option is turned OFF from Mikros Settings)
		/// </summary>
		/// <param name="configuration">Client configuration for initializing Mikros.</param>
		public void InitializeMikrosSDK(Configuration configuration = null)
		{
			if(IsInitialized)
			{
				MikrosLogger.Log(Constants.MikrosInitializeWarning);
				return;
			}
			ConfigurationController.SetConfiguration(configuration);
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
			PluginInitialization();
#endif
			IsInitialized = true;
			// Uploading metadata.
			InternalController.UploadMetadata();
			// Uploading preset event data for app open.
			InternalController.TrackAppOpen();
			// Initialization event of Mikros SDK.
			MikrosSDKInitializeEvent.Builder().Create();
		}

		/// <summary>
		/// Initialization of Mikros native plugins.
		/// </summary>
		private void PluginInitialization()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			MikrosLogger.Log("Initializing plugin.");

			AndroidPluginHelper clientConfiguration = new AndroidPluginHelper("com.tatumgames.mikros.framework.android.configuration.ClientConfiguration$Builder");
			AndroidJavaObject configurationObject = clientConfiguration
				.CallMethod<AndroidJavaObject>(Constants.SetApiKey, ConfigurationController.MikrosSettings.GetCurrentApiKey())
				.Call<AndroidJavaObject>(Constants.SetBaseUrl, ServerData.GetUrl(UrlType.BaseURL))
				.Call<AndroidJavaObject>(Constants.SetAppGameId, ConfigurationController.MikrosSettings.AppGameID)
				.Call<AndroidJavaObject>(Constants.SetApiKeyType, ConfigurationController.MikrosSettings.GetApiKeyTypeString())
				.Call<AndroidJavaObject>(Constants.SetDeviceId, Constants.DeviceID)
				.Call<AndroidJavaObject>(Constants.SetAppVersion, Application.version)
				.Call<AndroidJavaObject>(Constants.SetSdkVersion, Constants.SDKVersion)
				.Call<AndroidJavaObject>(Constants.Create);

			AndroidPluginHelper analyticsConfiguration = new AndroidPluginHelper("com.tatumgames.mikros.framework.android.analytics.configuration.AnalyticsEventConfiguration$Builder");
			AndroidJavaObject analyticsConfigurationObject = analyticsConfiguration
				.CallMethod<AndroidJavaObject>(Constants.SetEventLogging, ConfigurationController.IsEventLogging)
				.Call<AndroidJavaObject>(Constants.Create);

			AndroidPluginHelper analyticsSessionConfiguration = new AndroidPluginHelper("com.tatumgames.mikros.framework.android.analytics.configuration.AnalyticsSessionConfiguration$Builder");
			AndroidJavaObject analyticsSessionConfigurationObject = analyticsSessionConfiguration
				.CallMethod<AndroidJavaObject>(Constants.SetEventLogging, ConfigurationController.IsTrackUserSession)
				.Call<AndroidJavaObject>(Constants.Create);

			AndroidPluginHelper analyticsExceptionConfiguration = new AndroidPluginHelper("com.tatumgames.mikros.framework.android.analytics.configuration.AnalyticsExceptionConfiguration$Builder");
			AndroidJavaObject analyticsExceptionConfigurationObject = analyticsExceptionConfiguration
				.CallMethod<AndroidJavaObject>(Constants.SetEventLogging, ConfigurationController.IsCrashReporting)
				.Call<AndroidJavaObject>(Constants.Create);

			AndroidPluginHelper analyticsMemoryConfiguration = new AndroidPluginHelper("com.tatumgames.mikros.framework.android.analytics.configuration.AnalyticsMemoryConfiguration$Builder");
			AndroidJavaObject analyticsMemoryConfigurationObject = analyticsMemoryConfiguration
				.CallMethod<AndroidJavaObject>(Constants.SetEventLogging, ConfigurationController.IsTrackDeviceMemory)
				.Call<AndroidJavaObject>(Constants.Create);

			mikrosApiClientProvider = new AndroidPluginHelper("com.tatumgames.mikros.framework.android.http.provider.MikrosApiClientProvider");
			mikrosApiClientProvider.CallStaticMethod(Constants.Initialize, mikrosApiClientProvider.UnityPlayerContext, configurationObject, analyticsConfigurationObject, analyticsSessionConfigurationObject, analyticsExceptionConfigurationObject, analyticsMemoryConfigurationObject);
			
#elif UNITY_IOS && !UNITY_EDITOR
			SwiftForUnity.PerformClientConfiguration(ConfigurationController.IsEventLogging, ConfigurationController.IsTrackUserSession, ConfigurationController.IsCrashReporting);
#endif
			MikrosLogger.Log("Initializing plugin COMPLETE.");
		}

		/// <summary>
		/// Detect screen touches by end-user to determine session continuation.
		/// </summary>
		internal void MotionEventDetect()
		{
			if(!IsInitialized)
				return;
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
			NativeMotionEventDetect();
#endif
		}

		/// <summary>
        /// Detect motion event in native platform.
        /// </summary>
		private void NativeMotionEventDetect()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaObject mikrosApiClient = mikrosApiClientProvider.CallStaticMethod<AndroidJavaObject>(Constants.GetInstance);
            mikrosApiClient.Call(Constants.OnMotionEvent);
#elif UNITY_IOS && !UNITY_EDITOR
			SwiftForUnity.OnMotionEventNative();
#endif
			MikrosLogger.Log("onMotionEvent() called.");
		}
	}
}