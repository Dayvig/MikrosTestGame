using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashScreen : ScreenBase
{
    private void OnEnable()
    {
		screenTime = Time.time;
	}

    /// <summary>
    /// Button click tasks for forcefully crashing the app and checking the crash reporting event.
    /// </summary>
    public void ForceCrashException()
	{
#if UNITY_ANDROID
		CrashOnAndroid();
#endif
		UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.FatalError);
	}

	/// <summary>
	/// For forcefully crashing the app and checking the crash reporting event only for android.
	/// </summary>
	private void CrashOnAndroid()
	{
		// https://stackoverflow.com/questions/17511070/android-force-crash-with-uncaught-exception-in-thread
		var message = new AndroidJavaObject("java.lang.String", "This is a test crash, ignore.");
		var exception = new AndroidJavaObject("java.lang.Exception", message);

		var looperClass = new AndroidJavaClass("android.os.Looper");
		var mainLooper = looperClass.CallStatic<AndroidJavaObject>("getMainLooper");
		var mainThread = mainLooper.Call<AndroidJavaObject>("getThread");
		var exceptionHandler = mainThread.Call<AndroidJavaObject>("getUncaughtExceptionHandler");
		exceptionHandler.Call("uncaughtException", mainThread, exception);
	}
}