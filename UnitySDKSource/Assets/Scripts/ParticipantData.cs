using System.Collections;
using System.Collections.Generic;
using MikrosClient;
using UnityEngine;
using UnityEngine.UI;

public class ParticipantData : MonoBehaviour
{
    [SerializeField] private Text emailText;
    [SerializeField] private Text deviceIdText;
    [SerializeField] private Dropdown behaviorDropdown;

    public string Email { get; private set; }

    public string DeviceId { get; private set; }

    public void SetData(string email, string deviceId)
    {
        Email = email;
        DeviceId = deviceId;
        emailText.text = email;
        deviceIdText.text = deviceId;
    }

    public PlayerBehavior GetBehaviour()
    {
        return (PlayerBehavior)behaviorDropdown.value;
    }
}
