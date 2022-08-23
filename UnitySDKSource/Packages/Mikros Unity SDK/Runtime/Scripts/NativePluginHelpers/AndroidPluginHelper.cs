using UnityEngine;

namespace MikrosClient.NativeFramework
{
    /// <summary>
    /// Helper class to work with android native frameworks in Unity
    /// </summary>
    internal sealed class AndroidPluginHelper
    {
#if UNITY_ANDROID && !UNITY_EDITOR
		/// <summary>
		/// Get the Unity player class of native android.
		/// </summary>
		internal AndroidJavaClass UnityPlayerClass { get; private set; }

		/// <summary>
		/// Get the Unity player activity of native android.
		/// </summary>
		internal AndroidJavaObject UnityPlayerActivity { get; private set; }

		/// <summary>
		/// Get the Unity player context of native android.
		/// </summary>
		internal AndroidJavaObject UnityPlayerContext { get; private set; }

		/// <summary>
		/// Object of the class from native framework.
		/// </summary>
		private AndroidJavaObject pluginClassObject;

		/// <summary>
		/// AndroidPluginHelper private default constructor to restrict object creation of the class.
		/// </summary>
		private AndroidPluginHelper()
		{
		}

		/// <summary>
		/// Preliminary initializations.
		/// </summary>
		private void Initializations()
		{
			UnityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			UnityPlayerActivity = UnityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
			UnityPlayerContext = UnityPlayerActivity.Call<AndroidJavaObject>("getApplicationContext");
		}

		/// <summary>
		/// Create an object of native framework.
		/// </summary>
		/// <param name="classname">Full class path in the native framawork</param>
		/// <param name="parameters">Any number of parameters to pass while creating object of the native framework class</param>
		internal AndroidPluginHelper(string classname, params object[] parameters)
		{
			Initializations();
			pluginClassObject = new AndroidJavaObject(classname, parameters);
		}

		/// <summary>
		/// Get object of the class from native framework.
		/// </summary>
		/// <returns>Object of class in native framework</returns>
		internal AndroidJavaObject GetClassObject()
		{
			return pluginClassObject;
		}

		/// <summary>
		/// Call a non-static method in native framework that returns a generic class.
		/// </summary>
		/// <typeparam name="T">Generic class</typeparam>
		/// <param name="methodName">Name of the method</param>
		/// <param name="parameters">Any number of parameters to pass with the method</param>
		/// <returns>Generic class object</returns>
		internal T CallMethod<T>(string methodName, params object[] parameters)
		{
			return pluginClassObject.Call<T>(methodName, parameters);
		}

		/// <summary>
		/// Call a non-static void method in native framework.
		/// </summary>
		/// <param name="methodName">Name of the method</param>
		/// <param name="parameters">Any number of parameters to pass with the method</param>
		internal void CallMethod(string methodName, params object[] parameters)
		{
			pluginClassObject.Call(methodName, parameters);
		}

		/// <summary>
		/// Call a static method in native framework that returns a generic class.
		/// </summary>
		/// <typeparam name="T">Generic class</typeparam>
		/// <param name="methodName">Name of the method</param>
		/// <param name="parameters">Any number of parameters to pass with the method</param>
		/// <returns>Generic class object</returns>
		internal T CallStaticMethod<T>(string methodName, params object[] parameters)
		{
			return pluginClassObject.CallStatic<T>(methodName, parameters);
		}

		/// <summary>
		/// Call a static void method in native framework.
		/// </summary>
		/// <param name="methodName">Name of the method</param>
		/// <param name="parameters">Any number of parameters to pass with the method</param>
		internal void CallStaticMethod(string methodName, params object[] parameters)
		{
			pluginClassObject.CallStatic(methodName, parameters);
		}
#endif
    }
}