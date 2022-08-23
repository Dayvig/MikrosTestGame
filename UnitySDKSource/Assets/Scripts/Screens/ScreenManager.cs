using MikrosClient;
using MikrosClient.Analytics;
using MikrosClient.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    private static ScreenManager screenManagerInstance;

    [SerializeField] private List<ScreenBase> screens;

    private List<MikrosUser> savedMikrosUsers = new List<MikrosUser>();
    public ReadOnlyCollection<MikrosUser> SavedMikrosUsers => savedMikrosUsers.AsReadOnly();

    /// <summary>
    /// Public instance of MikrosManager class.
    /// </summary>
    public static ScreenManager Instance
    {
        get
        {
            return screenManagerInstance; // Return the Sc object.
        }
    }
    private void Awake()
    {
        if (Equals(screenManagerInstance, null))
        {
            screenManagerInstance = this;
        }
    }
    private void Start()
    {
        SetupData();
        Initialize();
    }

    /// <summary>
    /// Setup all auth related data required.
    /// </summary>
    internal void SetupData()
    {
        // Retrieve saved Mikros accounts.
        Utility.GetSavedMikrosAccounts(out savedMikrosUsers);
        if (Equals(savedMikrosUsers, null))
        {
            savedMikrosUsers = new List<MikrosUser>();
        }
    }

    /// <summary>
	/// Preliminary initializations.
	/// </summary>
	private void Initialize()
    {
        ManualInitializationMikros();
        Debug.Log(SystemInfo.deviceName);
        Debug.Log(SystemInfo.deviceModel);
        Debug.Log(SystemInfo.deviceType);
        Debug.Log(SystemInfo.operatingSystem);
        Debug.Log(SystemInfo.operatingSystemFamily);       
        ForceException();
        MikrosManager.Instance.AdController.StoreListener.OnOpened += AppStoreOpened;
        MikrosManager.Instance.AdController.StoreListener.OnClosed += AppStoreClosed;
        MikrosManager.Instance.AdController.StoreListener.OnError += AppStoreError;
    }

    /// <summary>
	/// Initiating manual Mikros initialization.
	/// </summary>
	private void ManualInitializationMikros()
    {
        bool isTrackSession = PlayerPrefs.GetInt(nameof(MikrosManager.Instance.ConfigurationController.IsTrackUserSession), 1) == 1;
        bool isTrackMetadata = PlayerPrefs.GetInt(nameof(MikrosManager.Instance.ConfigurationController.IsTrackUserMetadata), 1) == 1;
        bool isLogEvent = PlayerPrefs.GetInt(nameof(MikrosManager.Instance.ConfigurationController.IsEventLogging), 1) == 1;
        bool isCrashTracking = PlayerPrefs.GetInt(nameof(MikrosManager.Instance.ConfigurationController.IsCrashReporting), 1) == 1;
        bool isTrackDeviceMemory = PlayerPrefs.GetInt(nameof(MikrosManager.Instance.ConfigurationController.IsTrackDeviceMemory), 1) == 1;
        Configuration configuration = Configuration.Builder().SetPrivacyLevel(PRIVACY_LEVEL.EXTREME)
            .SetSessionTracking(isTrackSession)
            .SetMetadataTracking(isTrackMetadata)
            .SetEventLogging(isLogEvent)
            .SetCrashReporting(isCrashTracking)
            .SetTrackDeviceMemory(isTrackDeviceMemory)
            .Create();
        MikrosManager.Instance.InitializeMikrosSDK(configuration);
    }

    /// <summary>
	/// Event handler for Mikros App Store error.
	/// </summary>
	/// <param name="exception">Exception.</param>
	private void AppStoreError(MikrosException exception)
    {
        Debug.Log("App store error\n" + exception.Message);
    }

    /// <summary>
    /// Event handler for Mikros App Store close.
    /// </summary>
    private void AppStoreClosed()
    {
        Debug.Log("App store closed");
    }

    /// <summary>
    /// Event handler for Mikros App Store open.
    /// </summary>
    private void AppStoreOpened()
    {
        Debug.Log("App store opened");
    }

    /// <summary>
    /// Test handled exception.
    /// </summary>
    private void ForceException()
    {
        try
        {
            // Create random exceptions here
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open("D:/Work/x.cs", FileMode.Open);
            string t_ReadString = (string)binaryFormatter.Deserialize(file);
            file.Close();
        }
        catch (Exception e)
        {
            TrackHandledExceptionRequest.Builder().SetException(e).Create(onSuccess =>
            {
                MikrosManager.Instance.AnalyticsController.LogEvent(onSuccess,
                    response =>
                    {
                        if (Utils.DetectStatusType(response.Status.StatusCode) == STATUS_TYPE.SUCCESS)
                        {
                            Debug.Log("Exception handling success.");
                        }
                        else
                        {
                            Debug.Log("Exception handling failed.");
                        }
                    });
            },
            onFailure =>
            {
                Debug.Log("Exception handling failed.");
            });
        }
    }

    /// <summary>
    /// SetScreenTime fuction to set the new screentime after successfully event logged.
    /// </summary>
    /// <param name="screenIndex"></param>
    private void SetScreenTime(int screenIndex)
    {
        screens[screenIndex].screenTime = Time.time;
    }

    /// <summary>
    /// SendScreenEvent function track the screen time and sends the event.
    /// </summary>
    public void SendScreenEvent(int screenIndex)
    {
        float spentTime = (Time.time - screens[screenIndex].screenTime);
        TrackScreenTimeRequest.Builder()
        .ScreenName(screens[screenIndex].screenType.ToString())
        .ScreenClass(SceneManager.GetActiveScene().name)
        .TimeSpentOnScreen(spentTime)
        .Create(
            trackScreenTimeRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackScreenTimeRequest, response =>
            {
                SetScreenTime(screenIndex);
            }),
            onFailure =>
            {
                Instruction.Instance.ShowPopup(onFailure.Message);
            });
    }
    /// <summary>
    /// AddNewMikrosUser fuction to add new user when sucessfully signed
    /// </summary>
    /// <param name="mikrosUser"></param>
    public void AddNewMikrosUser(MikrosUser mikrosUser)
    {
        if (!savedMikrosUsers.Any(e => e.Email == mikrosUser.Email))
            savedMikrosUsers.Add(mikrosUser);
    }

    /// <summary>
    /// Invoked when the script is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        MikrosManager.Instance.AdController.StoreListener.OnOpened -= AppStoreOpened;
        MikrosManager.Instance.AdController.StoreListener.OnClosed -= AppStoreClosed;
        MikrosManager.Instance.AdController.StoreListener.OnError -= AppStoreError;
    }
}