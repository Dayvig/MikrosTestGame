using MikrosClient;
using MikrosClient.GameService;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnitTestConstants;

namespace Tests
{
    public class ReputationScoringTestSuite
    {
        private static object participantDeviceId = new object[] { "Valid_Device_ID_1", "Valid_Device_ID_2", "" };
        private static object participantEmail = new object[] { "Valid_Email_1", "Valid_Email_2", "" };
        private static object participantBehavior = new object[] { PlayerBehavior.POOR_SPORTSMANSHIP, PlayerBehavior.TROLLING, PlayerBehavior.CONSTANT_PINGING, PlayerBehavior.AFK, PlayerBehavior.COMPLAINING, PlayerBehavior.OFFENSIVE_LANGUAGE, PlayerBehavior.CHEATING, PlayerBehavior.GOOD_SPORTSMANSHIP, PlayerBehavior.GREAT_LEADERSHIP, PlayerBehavior.EXCELLENT_TEAMMATE, PlayerBehavior.MVP };

        /// <summary>
        /// This is called every time before any test method executes
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }

        [UnityTest]
        public IEnumerator SubmitPlayerRating1([ValueSourceAttribute(nameof(participantDeviceId))] string participantDeviceId, [ValueSourceAttribute(nameof(participantEmail))] string email, [ValueSourceAttribute(nameof(participantBehavior))] PlayerBehavior playerBehavior)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isRatingCompleted = false;
            GeneralResponse playerRatingResponse = null;

            List<Participant> participants = new List<Participant>();

            Participant.Builder()
                .DeviceId(participantDeviceId)
                .Email(email)
                .Behavior(playerBehavior)
                .Create(
                participantRequest =>
                {
                    participants.Add(participantRequest);
                },
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            PlayerRating.Builder()
                .Participants(participants)
                .Create(
                playerRatingRequest =>
                {
                    MikrosManager.Instance.GameServiceController.SendPlayerRating(playerRatingRequest, response =>
                    {
                        isRatingCompleted = true;
                        playerRatingResponse = response;
                    });
                },
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isRatingCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(playerRatingResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator SubmitPlayerRating2([ValueSourceAttribute(nameof(participantDeviceId))] string participantDeviceId1, [ValueSourceAttribute(nameof(participantEmail))] string email1, [ValueSourceAttribute(nameof(participantBehavior))] PlayerBehavior playerBehavior1, [ValueSourceAttribute(nameof(participantDeviceId))] string participantDeviceId2, [ValueSourceAttribute(nameof(participantEmail))] string email2, [ValueSourceAttribute(nameof(participantBehavior))] PlayerBehavior playerBehavior2)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isRatingCompleted = false;
            GeneralResponse playerRatingResponse = null;

            List<Participant> participants = new List<Participant>();

            Participant.Builder()
                .DeviceId(participantDeviceId1)
                .Email(email1)
                .Behavior(playerBehavior1)
                .Create(
                participantRequest =>
                {
                    participants.Add(participantRequest);
                },
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            Participant.Builder()
                .DeviceId(participantDeviceId2)
                .Email(email2)
                .Behavior(playerBehavior2)
                .Create(
                participantRequest =>
                {
                    participants.Add(participantRequest);
                },
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            PlayerRating.Builder()
                .Participants(participants)
                .Create(
                playerRatingRequest =>
                {
                    MikrosManager.Instance.GameServiceController.SendPlayerRating(playerRatingRequest, response =>
                    {
                        isRatingCompleted = true;
                        playerRatingResponse = response;
                    });
                },
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isRatingCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(playerRatingResponse.Status.StatusCode));
        }
    }
}