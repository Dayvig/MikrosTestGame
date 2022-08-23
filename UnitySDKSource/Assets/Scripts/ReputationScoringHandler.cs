using System.Collections;
using System.Collections.Generic;
using MikrosClient;
using MikrosClient.GameService;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handle Reputation score submission.
/// </summary>
public class ReputationScoringHandler : ScreenBase
{
    [SerializeField] private Text senderEmail;
    [SerializeField] private Text senderDeviceId;
    [SerializeField] private ParticipantData participantPrefab;
    [SerializeField] private Transform participantsContainer;

    private List<Participant> participants = new List<Participant>();
    private List<ParticipantData> participantDatas = new List<ParticipantData>();

    /// <summary>
    /// Called every time gameobject is enabled.
    /// </summary>
    private void OnEnable()
    {
        ResetAll();
        SetupSender();
        SetupParticipants();
        screenTime = Time.time;
    }

    /// <summary>
    /// Setup Sender details on UI.
    /// </summary>
    private void SetupSender()
    {
        senderEmail.text = MikrosManager.Instance.AuthenticationController.MikrosUser?.Email;
        senderDeviceId.text = MikrosManager.Instance.AuthenticationController.MikrosUser?.DeviceId;
    }

    /// <summary>
    /// Setup all participants in UI.
    /// </summary>
    private void SetupParticipants()
    {
        var mikrosUsers = ScreenManager.Instance.SavedMikrosUsers;
        for(int i = 0; i < mikrosUsers.Count; i++)
        {
            if(Equals(mikrosUsers[i].Email, MikrosManager.Instance.AuthenticationController.MikrosUser?.Email))
            {
                continue;
            }
            ParticipantData participantData = Instantiate(participantPrefab, participantsContainer, false);
            participantData.SetData(mikrosUsers[i].Email, PlayerPrefs.GetString(mikrosUsers[i].Email));
            participantDatas.Add(participantData);
        }
    }

    /// <summary>
    /// Add details of a participant.
    /// </summary>
    /// <param name="email">Email of participant.</param>
    /// <param name="deviceId">Device ID of participant.</param>
    /// <param name="participantBehavior">Behavior of participant.</param>
    private void AddParticipant(string email, string deviceId, PlayerBehavior participantBehavior)
    {
        Participant.Builder()
            .DeviceId(deviceId)
            .Email(email)
            .Behavior(participantBehavior)
            .Create(
            participantRequest =>
            {
                participants.Add(participantRequest);
            },
            onFailure =>
            {
                Debug.Log(onFailure.Message);
            });
    }

    /// <summary>
    /// Submit rating of all added participants.
    /// </summary>
    public void SubmitAllPlayerReputationScoring()
    {
        Instruction.Instance.ShowLoader();
        for(int i = 0; i < participantDatas.Count; i++)
        {
            AddParticipant(participantDatas[i].Email, participantDatas[i].DeviceId, participantDatas[i].GetBehaviour());
        }
        PlayerRating.Builder()
            .Participants(participants)
            .Create(
            playerRatingRequest =>
            {
                MikrosManager.Instance.GameServiceController.SendPlayerRating(playerRatingRequest, response =>
                {
                    Instruction.Instance.HideLoader();
                    STATUS_TYPE statusType = Utils.DetectStatusType(response.Status.StatusCode);
                    if (statusType == STATUS_TYPE.SUCCESS)
                    {
                        Instruction.Instance.ShowPopup("Success message: " + response.Status.StatusMessage);
                    }
                    else if (statusType == STATUS_TYPE.ERROR)
                    {
                        Instruction.Instance.ShowPopup("Error message: " + response.Status.StatusMessage);
                    }
                    else
                    {
                        Instruction.Instance.ShowPopup("Other issue status message: " + response.Status.StatusMessage);
                    }
                });
            },
            onFailure =>
            {
                Instruction.Instance.HideLoader();
                Instruction.Instance.ShowPopup(onFailure.Message);
            });
    }

    /// <summary>
    /// Reseting all data.
    /// </summary>
    private void ResetAll()
    {
        participants.Clear();
        for (int i = 0; i < participantDatas.Count; i++)
        {
            Destroy(participantDatas[i].gameObject);
        }
        participantDatas.Clear();
    }

    /// <summary>
    /// Called every time gameobject is disabled.
    /// </summary>
    private void OnDisable()
    {
        ResetAll();
    }
}
