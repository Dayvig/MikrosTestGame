using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MikrosClient;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handle multiple toggle data and UI.
/// </summary>
public class MultiToggleHandle : MonoBehaviour
{
    [SerializeField] private List<DataTrackToggle> dataTrackingToggles;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    void Start()
    {
        InitializeView();
    }

    /// <summary>
    /// Setting up the toggle views.
    /// </summary>
    private void InitializeView()
    {
        for (int i = 0; i < dataTrackingToggles.Count; i++)
        {
            SetupToggle(dataTrackingToggles[i]);
        }
    }

    /// <summary>
    /// Determine status of each toggle.
    /// </summary>
    /// <param name="dataTrackToggle">Toggle for showing data tracking status.</param>
    private void SetupToggle(DataTrackToggle dataTrackToggle)
    {
        switch (dataTrackToggle.DataTrackType)
        {
            case DATA_TRACK_TYPE.SESSION:
                dataTrackToggle.SetToggleStatus(PlayerPrefs.GetInt(nameof(MikrosManager.Instance.ConfigurationController.IsTrackUserSession), 1) == 1);
                break;

            case DATA_TRACK_TYPE.METADATA:
                dataTrackToggle.SetToggleStatus(PlayerPrefs.GetInt(nameof(MikrosManager.Instance.ConfigurationController.IsTrackUserMetadata), 1) == 1);
                break;

            case DATA_TRACK_TYPE.LOG_EVENT:
                dataTrackToggle.SetToggleStatus(PlayerPrefs.GetInt(nameof(MikrosManager.Instance.ConfigurationController.IsEventLogging), 1) == 1);
                break;

            case DATA_TRACK_TYPE.CRASH_REPORTING:
                dataTrackToggle.SetToggleStatus(PlayerPrefs.GetInt(nameof(MikrosManager.Instance.ConfigurationController.IsCrashReporting), 1) == 1);
                break;

            case DATA_TRACK_TYPE.TRACK_DEVICE_MEMORY:
                dataTrackToggle.SetToggleStatus(PlayerPrefs.GetInt(nameof(MikrosManager.Instance.ConfigurationController.IsTrackDeviceMemory), 1) == 1);
                break;

        }
    }
}