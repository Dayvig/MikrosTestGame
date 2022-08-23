using UnityEngine;

namespace MikrosClient
{
	/// <summary>
	/// Internal class to initialize MikrosManager during app start.
	/// </summary>
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-10)]
	internal sealed class MikrosManagerInitializer : MonoBehaviour
	{
		/// <summary>
		/// Private instance of the class.
		/// </summary>
		private static MikrosManagerInitializer mikrosManagerInitializerInstance;

		/// <summary>
		/// Internal instance of MikrosManagerInitializer class.
		/// </summary>
		internal static MikrosManagerInitializer Instance
		{
			get
			{
				// instantiate the class if the class object is null.
				if (Equals(mikrosManagerInitializerInstance, null))
				{
					// throw an exception that MikrosManagerInitializer class is not initialized.
					throw new MikrosException(ExceptionType.INITIALIZE_MIKROS_SDK);
				}
				return mikrosManagerInitializerInstance; // Return the MikrosManagerInitializer object.
			}
		}

		/// <summary>
		/// Define private constructor for singleton class.
		/// </summary>
		private MikrosManagerInitializer()
		{
			if (Equals(mikrosManagerInitializerInstance, null))
			{
				mikrosManagerInitializerInstance = this;
			}
		}

		/// <summary>
		/// Automatic essential initializations for perfect working of SDK.
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitializeBeforeSceneLoad()
		{
			SetInstance();
		}
				
        /// <summary>
        /// Set Singleton instance of MikrosManagerInitializer for internal usage.
        /// </summary>
        private static void SetInstance()
		{
			if (Equals(mikrosManagerInitializerInstance, null))
			{
				mikrosManagerInitializerInstance = new GameObject(Constants.MikrosManager).AddComponent<MikrosManagerInitializer>();
				DontDestroyOnLoad(mikrosManagerInitializerInstance);				
				MikrosManager.SetInstance();				
			}
		}

		/// <summary>
		/// Called once every frame.
		/// </summary>
		private void Update()
		{
#if ENABLE_LEGACY_INPUT_MANAGER
#if UNITY_ANDROID || UNITY_IOS
			if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
			{
				MikrosManager.Instance?.MotionEventDetect();
			}
#endif
#endif
		}

		/// <summary>
		/// Called when this gameObject is destroyed.
		/// </summary>
		private void OnDestroy()
		{
			MikrosManager.Instance?.OnDestroyCallback?.Invoke();
		}
    }

	
}
