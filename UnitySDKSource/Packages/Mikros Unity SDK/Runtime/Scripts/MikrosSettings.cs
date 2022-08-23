using UnityEditor;
using UnityEngine;

namespace MikrosClient
{
	/// <summary>
	/// Mikros Settings Scriptable Object for defining app game ID path and api keys.
	/// </summary>
	public sealed class MikrosSettings : ScriptableObject
	{
		[Tooltip(Constants.AppGameID)]
		[SerializeField]
		private string appGameID;

		[Tooltip(Constants.ApiKeyProduction)]
		[SerializeField]
		private string apiKeyProduction;

		[Tooltip(Constants.ApiKeyQA)]
		[SerializeField]
		private string apiKeyQA;

		[Tooltip(Constants.ApiKeyDev)]
		[SerializeField]
		private string apiKeyDevelopment;

		[Tooltip(Constants.ApiKeyMikrosSettings)]
		[SerializeField]
		private API_KEY_TYPE apiKeyToUse = API_KEY_TYPE.PRODUCTION;

		[Space(25)]
	
		[Tooltip(Constants.AutoInitializeMikros)]
		[SerializeField]
		private bool autoInitializeAtStart = true;

		[Tooltip(Constants.AutoTrackSessionMikros)]
		[SerializeField]
		private bool autoTrackSession = true;

		[Tooltip(Constants.AutoTrackMetadataMikros)]
		[SerializeField]
		private bool autoTrackMetadata = true;

		[Tooltip(Constants.EventLogging)]
		[SerializeField]
		private bool isEventLogging = true;

		[Tooltip(Constants.EnableUserNameSpecialCharacters)]
		[SerializeField]
		private bool isEnableUserNameSpecialCharacters = true;

		[Space(20)]

		[Tooltip(Constants.CrashReporting)]
		[SerializeField]
		private bool isCrashReporting = true;

		[Tooltip(Constants.TrackDeviceMemory)]
		[SerializeField]
		private bool isTrackDeviceMemory= true;

		/// <summary>
		/// Get App Game ID.
		/// </summary>
		public string AppGameID => appGameID;

		/// <summary>
		/// Get API Key for Production phase.
		/// </summary>
		public string ApiKeyProduction => apiKeyProduction;

		/// <summary>
		/// Get API Key for QA phase.
		/// </summary>
		public string ApiKeyQA => apiKeyQA;

		/// <summary>
		/// Get API Key for Development phase.
		/// </summary>
		public string ApiKeyDevelopment => apiKeyDevelopment;

		/// <summary>
		/// Get current API key type.
		/// </summary>
		public API_KEY_TYPE CurrentApiKeyType => apiKeyToUse;

		/// <summary>
		/// Returns true if Mikros is initialized automatically, else false.
		/// </summary>
		public bool IsAutoInitialize => autoInitializeAtStart;

		/// <summary>
		/// Returns true if user session activity is tracked automatically, else false.
		/// </summary>
		public bool IsAutoTrackSession => autoTrackSession;

		/// <summary>
		/// Returns true if user metadata is tracked automatically, else false.
		/// </summary>
		public bool IsAutoTrackMetadata => autoTrackMetadata;

		/// <summary>
		/// Returns true if event logging is enabled, else false.
		/// </summary>
		public bool IsEventLogging => isEventLogging;
		/// <summary>
		/// Returns true if enable username special characters is enabled, else false.
		/// </summary>
		public bool IsEnableUserNameSpecialCharacters => isEnableUserNameSpecialCharacters;

		/// <summary>
		/// Return true if crash reporting is enabled, else false.
		/// </summary>
		public bool IsCrashReporting => isCrashReporting;

		/// <summary>
		/// Return true if trackDeviceMemory reporting is enabled, else false.
		/// </summary>
		public bool IsTrackDeviceMemory => isTrackDeviceMemory;

		/// <summary>
		/// To prevent object creation of this class.
		/// </summary>
		private MikrosSettings()
		{
		}

		/// <summary>
		/// Get the API Key that is being used currently.
		/// </summary>
		/// <returns>API key being used currently.</returns>
		public string GetCurrentApiKey()
		{
			switch (apiKeyToUse)
			{
				case API_KEY_TYPE.PRODUCTION:
					return apiKeyProduction;

				case API_KEY_TYPE.QA:
					return apiKeyQA;

				case API_KEY_TYPE.DEVELOPMENT:
					return apiKeyDevelopment;

				default:
					return apiKeyProduction;
			}
		}

		/// <summary>
		/// Get string equivalent of the current API key type being used currently.
		/// </summary>
		/// <returns>API key type being used currently, as string.</returns>
		internal string GetApiKeyTypeString()
		{
			return ConvertApiKeyTypeToString(apiKeyToUse);
		}

		/// <summary>
		/// Convert API key type to its equivalent string.
		/// </summary>
		/// <param name="apiKeyType">Desired API key type enum</param>
		/// <returns>API key type as string</returns>
		internal string ConvertApiKeyTypeToString(API_KEY_TYPE apiKeyType)
		{
			switch (apiKeyType)
			{
				case API_KEY_TYPE.PRODUCTION:
					return Constants.Production;

				case API_KEY_TYPE.QA:
					return Constants.Qa;

				case API_KEY_TYPE.DEVELOPMENT:
					return Constants.Development;

				default:
					return Constants.Production;
			}
		}
	}
}