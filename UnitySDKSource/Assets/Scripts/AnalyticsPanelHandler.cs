using System.Collections;
using System.Collections.Generic;
using MikrosClient;
using UnityEngine;
using UnityEngine.UI;

public class AnalyticsPanelHandler : ScreenBase
{
    [SerializeField] private InputField eventNameInputField;

    private const string newCustomTest = "new_custom_test_";
    private const string newTestParam = "new_Test_param";
    private const string newTestValue = "new_Test_value";

    private void OnEnable()
    {
        // Set the screentime to the time at the beginning of this frame
        screenTime = Time.time;
    }

    /// <summary>
    /// Track some sample custom events.
    /// </summary>
    public void TrackCustom()
    {
        List<Hashtable> customEvents = new List<Hashtable>();
        for (int i = 1; i <= 10; i++)
        {
            MikrosManager.Instance.AnalyticsController.LogEvent(newCustomTest + i, newTestParam, newTestValue, (Hashtable customEvent) =>
            {
                // handle success
                customEvents.Add(customEvent);
                Instruction.Instance.ShowPopup(Constants.Success);
            },
            onFailure =>
            {
                // handle failure
                Debug.Log(onFailure.Message);
                Instruction.Instance.ShowPopup(onFailure.Message);
            });
        }
    }

    /// <summary>
    /// Flush events on button click.
    /// </summary>
    public void FlushEvents()
    {
        // Attempts to flush immediately all queued events.
        MikrosManager.Instance.AnalyticsController.FlushEvents();
    }

    /// <summary>
    /// Log a custom event set by user.
    /// </summary>
    public void LogCustomEvent()
    {
        MikrosManager.Instance.AnalyticsController.LogEvent(eventNameInputField.text, customEvent =>
        {
            // handle on success
            Instruction.Instance.ShowPopup(Constants.LoggedEvent + ":\n" + eventNameInputField.text);
            eventNameInputField.SetTextWithoutNotify(string.Empty);
        },
        onFailure =>
        {
            // handle on failure
            Instruction.Instance.ShowPopup(Constants.CustomEventsFailed + ":\n" + onFailure.Message);
        });
    }
}
