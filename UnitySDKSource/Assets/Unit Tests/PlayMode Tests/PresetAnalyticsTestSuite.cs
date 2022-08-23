using MikrosClient;
using MikrosClient.Analytics;
using NUnit.Framework;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.TestTools;
using UnitTestConstants;
using System.Collections.Generic;

namespace Tests
{
    public class PresetAnalyticsTestSuite
    {
        private static object exceptionMessage = new object[] { "Valid Exception Message", "" };
        private static object exceptionObject = new object[2];

        private static object url = new object[] { "https://www.google.com", "Wrong URL", "" };
        private static object statusCode = new object[] { 200, 0, -200 };
        private static object statusMessage = new object[] { "Valid Status Message", "" };
        private static object networkSpeed = new object[] { "Valid Speed", "" };

        private static object levelNumber = new object[] { 1, 0, -1 };
        private static object subLevelNumber = new object[] { 1, 0, -1 };
        private static object levelName = new object[] { "Valid Level Name", "" };
        private static object levelDescription = new object[] { "Valid Level Description", "" };
        private static object levelCompleteDuration = new object[] { "Valid Level Completion Time", "" };
        private static object levelSuccess = new object[] { "Valid Level Success Info", "" };
        private static object levelUpCharacter = new object[] { "Valid Character Name", "" };
        private static object levelScore = new object[] { 1, 0, -1 };

        private static object method = new object[] { "Valid Platform name", "" };
        private static object contentType = new object[] { "Valid Content", "" };

        private static object timerEventKey = new object[] { EVENT.APP_OPEN, EVENT.GAME_OVER, EVENT.HANDLED_EXCEPTION, EVENT.HTTP_FAILURE, EVENT.HTTP_SUCCESS, EVENT.LEVEL_END, EVENT.LEVEL_START, EVENT.LEVEL_UP, EVENT.POST_SCORE, EVENT.SHARE, EVENT.SIGNIN, EVENT.SIGNUP, EVENT.START_TIMER, EVENT.STOP_TIMER, EVENT.TUTORIAL_BEGIN, EVENT.TUTORIAL_COMPLETE, EVENT.UNHANDLED_EXCEPTION, EVENT.UNLOCK_ACHIEVEMENT, EVENT.TRACK_SCREEN, EVENT.TRACK_PURCHASE };

        private static object achievementId = new object[] { "Valid Achievement ID", "" };
        private static object achievementName = new object[] { "Valid Achievement Name", "" };

        private static object screenName = new object[] { "Home", "Profile", "Settings", "", null };
        private static object screenClass = new object[] { "Splash Scene", "Main Scene", "Gameplay Scene", "Lobby Scene", "", null };
        private static object screenTime = new object[] { 20.0f, 15.0f, 25.0f, 0.0f, -50.0f };

        private static object skuName = new object[] { "Weapon", "Diamond", "Bundle", "", null };
        private static object skuDescription = new object[] { "SoulSucker", "500 Diamonds", "", null };
        private static object purchaseCatagory = new object[] { PurchaseCategory.Weapon.GUN, PurchaseCategory.Currency.GOLD, PurchaseCategory.Bundle.OTHER, null };
        private static object purchaseType = new object[] { PurchaseType.IN_APP, PurchaseType.IN_GAME,null };
        private static object purchaseCurrencyType = new object[] { PurchaseCurrencyType.INR, PurchaseCurrencyType.USD, null };
        private static object purchasePrice = new object[] { 99.99f, 89.99f, -85.99f, 0, -89.99f};
        private static object percentDiscount = new object[] { 0, 32, 25, -35 };
        private static object amountRewarded = new object[] { 0, 2500, 3000,-5000 };


        /// <summary>
        /// This is called every time before any test method executes
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }

        [UnityTest]
        public IEnumerator TrackGameOver()
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackGameOverRequest.Builder()
                .Create(
                trackGameOverRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackGameOverRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackHandledException([ValueSourceAttribute(nameof(exceptionObject))] Exception exceptionObject)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            CreateExceptions();
            TrackHandledExceptionRequest.Builder()
                .SetException(exceptionObject)
                .Create(
                trackHandledExceptionRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackHandledExceptionRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }
        /// <summary>
        /// CreateExceptions function creates an exception.
        /// </summary>
        private void CreateExceptions()
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
                ((object[])exceptionObject)[0] = e;
            }
            ((object[])exceptionObject)[1] = new Exception((string)((object[])exceptionMessage)[0]);
        }

        [UnityTest]
        public IEnumerator TrackHttpFailure([ValueSourceAttribute(nameof(url))] string url, [ValueSourceAttribute(nameof(statusCode))] long statusCode, [ValueSourceAttribute(nameof(statusMessage))] string message, [ValueSourceAttribute(nameof(networkSpeed))] string networkSpeed)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackHttpFailureRequest.Builder()
                .Url(url)
                .StatusCode(statusCode)
                .Message(message)
                .NetworkSpeed(networkSpeed)
                .Create(
                trackHttpFailureRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackHttpFailureRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackHttpSuccess([ValueSourceAttribute(nameof(url))] string url, [ValueSourceAttribute(nameof(statusCode))] long statusCode, [ValueSourceAttribute(nameof(statusMessage))] string message, [ValueSourceAttribute(nameof(networkSpeed))] string networkSpeed)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackHttpSuccessRequest.Builder()
                .Url(url)
                .StatusCode(statusCode)
                .Message(message)
                .NetworkSpeed(networkSpeed)
                .Create(
                trackHttpSuccessRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackHttpSuccessRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackLevelEnd([ValueSourceAttribute(nameof(levelNumber))] long level, [ValueSourceAttribute(nameof(subLevelNumber))] long subLevel, [ValueSourceAttribute(nameof(levelName))] string levelName, [ValueSourceAttribute(nameof(levelDescription))] string description, [ValueSourceAttribute(nameof(levelCompleteDuration))] string completeDuration, [ValueSourceAttribute(nameof(levelSuccess))] string success)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackLevelEndRequest.Builder()
                .Level(level)
                .SubLevel(subLevel)
                .LevelName(levelName)
                .Description(description)
                .CompleteDuration(completeDuration)
                .Success(success)
                .Create(
                trackLevelEndRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackLevelEndRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackLevelStart([ValueSourceAttribute(nameof(levelNumber))] long level, [ValueSourceAttribute(nameof(subLevelNumber))] long subLevel, [ValueSourceAttribute(nameof(levelName))] string levelName, [ValueSourceAttribute(nameof(levelDescription))] string description)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackLevelStartRequest.Builder()
                .Level(level)
                .SubLevel(subLevel)
                .LevelName(levelName)
                .Description(description)
                .Create(
                trackLevelStartRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackLevelStartRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackLevelUp([ValueSourceAttribute(nameof(levelNumber))] long level, [ValueSourceAttribute(nameof(subLevelNumber))] long subLevel, [ValueSourceAttribute(nameof(levelName))] string levelName, [ValueSourceAttribute(nameof(levelUpCharacter))] string character, [ValueSourceAttribute(nameof(levelDescription))] string description)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackLevelUpRequest.Builder()
                .Level(level)
                .SubLevel(subLevel)
                .LevelName(levelName)
                .Character(character)
                .Description(description)
                .Create(
                trackLevelUpRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackLevelUpRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackPostScore([ValueSourceAttribute(nameof(levelScore))] long score, [ValueSourceAttribute(nameof(levelNumber))] long level, [ValueSourceAttribute(nameof(subLevelNumber))] long subLevel, [ValueSourceAttribute(nameof(levelName))] string levelName, [ValueSourceAttribute(nameof(levelUpCharacter))] string character)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackPostScoreRequest.Builder()
                .Score(score)
                .Level(level)
                .SubLevel(subLevel)
                .LevelName(levelName)
                .Character(character)
                .Create(
                trackPostScoreRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackPostScoreRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackShare([ValueSourceAttribute(nameof(method))] string method, [ValueSourceAttribute(nameof(contentType))] string contentType)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackShareRequest.Builder()
                .Method(method)
                .ContentType(contentType)
                .Create(
                trackShareRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackShareRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackSignin([ValueSourceAttribute(nameof(method))] string method)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackSigninRequest.Builder()
                .Method(method)
                .Create(
                trackSigninRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackSigninRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackSignup([ValueSourceAttribute(nameof(method))] string method)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackSignupRequest.Builder()
                .Method(method)
                .Create(
                trackSignupRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackSignupRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackStartTimer([ValueSourceAttribute(nameof(timerEventKey))] EVENT presetEvent)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackStartTimerRequest.Builder()
                .Event(presetEvent)
                .Create(
                trackStartTimerRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackStartTimerRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackStopTimer([ValueSourceAttribute(nameof(timerEventKey))] EVENT presetEvent)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackStopTimerRequest.Builder()
                .Event(presetEvent)
                .Create(
                trackStopTimerRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackStopTimerRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackTutorialBegin()
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackTutorialBeginRequest.Builder()
                .Create(
                trackTutorialBeginRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackTutorialBeginRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackTutorialComplete()
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackTutorialCompleteRequest.Builder()
                .Create(
                trackTutorialCompleteRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackTutorialCompleteRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackUnlockAchievement([ValueSourceAttribute(nameof(achievementId))] string achievementId, [ValueSourceAttribute(nameof(achievementName))] string achievementName)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackUnlockAchievementRequest.Builder()
                .AchievementId(achievementId)
                .AchievementName(achievementName)
                .Create(
                trackUnlockAchievementRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackUnlockAchievementRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackLevelStartAndEnd([ValueSourceAttribute(nameof(levelNumber))] long level, [ValueSourceAttribute(nameof(subLevelNumber))] long subLevel, [ValueSourceAttribute(nameof(levelName))] string levelName, [ValueSourceAttribute(nameof(levelDescription))] string description, [ValueSourceAttribute(nameof(levelCompleteDuration))] string completeDuration, [ValueSourceAttribute(nameof(levelSuccess))] string success)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackLevelStartRequest.Builder()
                .Level(level)
                .SubLevel(subLevel)
                .LevelName(levelName)
                .Description(description)
                .Create(
                trackLevelStartRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackLevelStartRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));

            isEventLogCompleted = false;
            eventLogResponse = null;
            yield return new WaitForSeconds(5f);

            TrackLevelEndRequest.Builder()
                .Level(level)
                .SubLevel(subLevel)
                .LevelName(levelName)
                .Description(description)
                .CompleteDuration(completeDuration)
                .Success(success)
                .Create(
                trackLevelEndRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackLevelEndRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackTimerStartAndStop([ValueSourceAttribute(nameof(timerEventKey))] EVENT presetEvent)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackStartTimerRequest.Builder()
                .Event(presetEvent)
                .Create(
                trackLevelStartRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackLevelStartRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));

            isEventLogCompleted = false;
            eventLogResponse = null;
            yield return new WaitForSeconds(2f);

            TrackStopTimerRequest.Builder()
                .Event(presetEvent)
                .Create(
                trackLevelEndRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackLevelEndRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }
        [UnityTest]
        public IEnumerator TrackScreenTime([ValueSourceAttribute(nameof(screenName))] string screenName, [ValueSourceAttribute(nameof(screenClass))] string screenClass, [ValueSourceAttribute(nameof(screenTime))] float screenTime)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            TrackScreenTimeRequest.Builder()
                .ScreenName(screenName)
                .ScreenClass(screenClass)
                .TimeSpentOnScreen(screenTime)
                .Create(
                trackScreenTimeRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackScreenTimeRequest, response =>
                {
                    isEventLogCompleted = true;
                    eventLogResponse = response;
                }),
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackPurchaseRequestEvent([ValueSourceAttribute(nameof(skuName))] string skuName, [ValueSourceAttribute(nameof(skuDescription))] string skuDescription, [ValueSourceAttribute(nameof(purchaseCatagory))] PurchaseCategory purchaseCatagory,
           [ValueSourceAttribute(nameof(purchaseType))] PurchaseType purchaseType, [ValueSourceAttribute(nameof(purchaseCurrencyType))] PurchaseCurrencyType purchaseCurrencyType, [ValueSourceAttribute(nameof(purchasePrice))] float purchasePrice,
            [ValueSourceAttribute(nameof(percentDiscount))] int percentDiscount, [ValueSourceAttribute(nameof(amountRewarded))] int amountRewarded)

        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            int randIndex = UnityEngine.Random.Range(0, 3);
            List<TrackPurchaseRequest.PurchaseDetails> purchaseDetails = new List<TrackPurchaseRequest.PurchaseDetails>();
            if (randIndex != 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    TrackPurchaseRequest.PurchaseDetails data = TrackPurchaseRequest.PurchaseDetails.Builder()
                        .SkuName(skuName)
                        .SkuDescription(skuDescription)
                        .PurchaseCategory(purchaseCatagory)
                        .Create();
                    purchaseDetails.Add(data);
                }
            }
            TrackPurchaseRequest.Builder()
            .SkuName(skuName)
            .SkuDescription(skuDescription)
            .PurchaseCategory(purchaseCatagory)
            .PurchaseType(purchaseType)
            .PurchaseCurrencyType(purchaseCurrencyType)
            .PurchasePrice(purchasePrice)
            .PercentDiscount(percentDiscount)
            .AmountRewarded(amountRewarded)
            .PurchaseDetail(purchaseDetails)
            .Create(
            trackPurchaseRequest => MikrosManager.Instance.AnalyticsController.LogEvent(trackPurchaseRequest, response =>
            {
                isEventLogCompleted = true;
                eventLogResponse = response;
            }),
            onFailure =>
            {
                LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                Assert.Fail(onFailure.Message);
            });

            yield return new WaitUntil(() => isEventLogCompleted);
            // Testing to check if Success status code is returned in response for the preset event request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }
    }
}