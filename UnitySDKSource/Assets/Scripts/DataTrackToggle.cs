using System;
using System.Collections;
using System.Collections.Generic;
using MikrosClient;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Toggle for data tracking.
/// </summary>
[RequireComponent(typeof(Toggle))]
public class DataTrackToggle : MonoBehaviour
{
    [SerializeField] private DATA_TRACK_TYPE dataTrackType;

    private Toggle toggle;

    public DATA_TRACK_TYPE DataTrackType => dataTrackType;

    /// <summary>
    /// Awake is called before the first frame update.
    /// </summary>
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    /// <summary>
    /// Called when gameobject is enabled.
    /// </summary>
    private void OnEnable()
    {
        toggle.onValueChanged.AddListener(HandleToggleChange);
    }

    /// <summary>
    /// Handle toggle change events.
    /// </summary>
    /// <param name="isOn">Status of toggle.</param>
    private void HandleToggleChange(bool isOn)
    {
        switch (dataTrackType)
        {
            case DATA_TRACK_TYPE.SESSION:
                MikrosManager.Instance.ConfigurationController.SetAutoTrackUserSession(isOn);
                PlayerPrefs.SetInt(nameof(MikrosManager.Instance.ConfigurationController.IsTrackUserSession), isOn ? 1 : 0);
                break;

            case DATA_TRACK_TYPE.METADATA:
                MikrosManager.Instance.ConfigurationController.SetAutoTrackUserMetadata(isOn);
                PlayerPrefs.SetInt(nameof(MikrosManager.Instance.ConfigurationController.IsTrackUserMetadata), isOn ? 1 : 0);
                break;

            case DATA_TRACK_TYPE.LOG_EVENT:
                MikrosManager.Instance.ConfigurationController.SetEventLogging(isOn);
                PlayerPrefs.SetInt(nameof(MikrosManager.Instance.ConfigurationController.IsEventLogging), isOn ? 1 : 0);
                break;

            case DATA_TRACK_TYPE.CRASH_REPORTING:
                MikrosManager.Instance.ConfigurationController.SetAutoCrashReporting(isOn);
                PlayerPrefs.SetInt(nameof(MikrosManager.Instance.ConfigurationController.IsCrashReporting), isOn ? 1 : 0);
                break;

            case DATA_TRACK_TYPE.TRACK_DEVICE_MEMORY:
                MikrosManager.Instance.ConfigurationController.SetAutoTrackDeviceMemory(isOn);
                PlayerPrefs.SetInt(nameof(MikrosManager.Instance.ConfigurationController.IsTrackDeviceMemory), isOn ? 1 : 0);
                break;
        }
    }

    /// <summary>
    /// Set toggle status without arising event.
    /// </summary>
    /// <param name="isOn">Status of toggle.</param>
    public void SetToggleStatus(bool isOn)
    {
        toggle.isOn = isOn;
    }

    /// <summary>
    /// Called when gameobject is disabled.
    /// </summary>
    private void OnDisable()
    {
        toggle.onValueChanged.RemoveAllListeners();
    }
}

/// <summary>
/// Enumeration for all types of data tracking.
/// </summary>
public enum DATA_TRACK_TYPE
{
    SESSION,
    METADATA,
    LOG_EVENT,
    CRASH_REPORTING,
    TRACK_DEVICE_MEMORY
}