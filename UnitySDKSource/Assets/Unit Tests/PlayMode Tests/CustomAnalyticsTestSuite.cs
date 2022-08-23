using MikrosClient;
using MikrosClient.Analytics;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnitTestConstants;

namespace Tests
{
    public class CustomAnalyticsTestSuite : MonoBehaviour
    {
        private static object eventName = new object[] { "mikros_internal_event", "custom_event_name", "" };
        private static object parameterKey = new object[] { "timestamp", "parameter_key", "" };
        private static object stringParameterValue = new object[] { "parameter_value", "" };
        private static object longParameterValue = new object[] { 1, 0, -1 };
        private static object doubleParameterValue = new object[] { 1, 1.5, 0.5f, 2.5d, 0, -1, -1.5, -0.5f, -2.5d };
        private static object hashtableDataIndex = new object[] { 0, 1, 2, 3, 4, 5, 6 };

        private static Hashtable[] hashtableParameter = new Hashtable[]
        {
        new Hashtable()
        {
            { "param1", "value1" },
            { "param2", 1 },
            { "param3", -2.505 },
            { "param4", true }
        },
        new Hashtable()
        {
            { "param1", "value1" },
            { "param2", 1 },
            { "param3", -2.505 },
            { "param4", true },
            { "param5", "" }
        },
        new Hashtable()
        {
            { "param1", "value1" },
            { "param2", 1 },
            { "param3", -2.505 },
            { "param4", true },
            { "", "value2" }
        },
        new Hashtable()
        {
            { "param1", "value1" },
            { "", "" }
        },
        new Hashtable()
        {
            { "param1", "value1" },
            { 2, "value2" }
        },
        new Hashtable()
        {
            { "param1", "value1" },
            { 2, 2 }
        },
        new Hashtable()
        {
            { "param1", "value1" },
            { false, true }
        }
        };

        /// <summary>
        /// This is called every time before any test method executes
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }

        [UnityTest]
        public IEnumerator TrackCustomEventWithoutParam([ValueSourceAttribute(nameof(eventName))] string eventName)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            List<Hashtable> customEvents = new List<Hashtable>();
            MikrosManager.Instance.AnalyticsController.LogEvent(eventName, customEvent =>
            {
                customEvents.Add(customEvent);
            },
            onFailure =>
            {
                LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                Assert.Fail(onFailure.Message);
            });

            MikrosManager.Instance.AnalyticsController.TestCustomEvents(customEvents, response =>
            {
                eventLogResponse = response;
                isEventLogCompleted = true;
            });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the sign-in request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackCustomEventWithStringParam([ValueSourceAttribute(nameof(eventName))] string eventName, [ValueSourceAttribute(nameof(parameterKey))] string key, [ValueSourceAttribute(nameof(stringParameterValue))] string value)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            List<Hashtable> customEvents = new List<Hashtable>();
            MikrosManager.Instance.AnalyticsController.LogEvent(eventName, key, value, customEvent =>
            {
                customEvents.Add(customEvent);
            },
            onFailure =>
            {
                LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                Assert.Fail(onFailure.Message);
            });

            MikrosManager.Instance.AnalyticsController.TestCustomEvents(customEvents, response =>
            {
                eventLogResponse = response;
                isEventLogCompleted = true;
            });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the sign-in request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackCustomEventWithLongParam([ValueSourceAttribute(nameof(eventName))] string eventName, [ValueSourceAttribute(nameof(parameterKey))] string key, [ValueSourceAttribute(nameof(longParameterValue))] long value)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            List<Hashtable> customEvents = new List<Hashtable>();
            MikrosManager.Instance.AnalyticsController.LogEvent(eventName, key, value, customEvent =>
            {
                customEvents.Add(customEvent);
            },
            onFailure =>
            {
                LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                Assert.Fail(onFailure.Message);
            });

            MikrosManager.Instance.AnalyticsController.TestCustomEvents(customEvents, response =>
            {
                eventLogResponse = response;
                isEventLogCompleted = true;
            });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the sign-in request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackCustomEventWithDoubleParam([ValueSourceAttribute(nameof(eventName))] string eventName, [ValueSourceAttribute(nameof(parameterKey))] string key, [ValueSourceAttribute(nameof(doubleParameterValue))] double value)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            List<Hashtable> customEvents = new List<Hashtable>();
            MikrosManager.Instance.AnalyticsController.LogEvent(eventName, key, value, customEvent =>
            {
                customEvents.Add(customEvent);
            },
            onFailure =>
            {
                LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                Assert.Fail(onFailure.Message);
            });

            MikrosManager.Instance.AnalyticsController.TestCustomEvents(customEvents, response =>
            {
                eventLogResponse = response;
                isEventLogCompleted = true;
            });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the sign-in request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackCustomEventWithHashtable([ValueSourceAttribute(nameof(eventName))] string eventName, [ValueSourceAttribute(nameof(hashtableDataIndex))] int hashtableDataIndex)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            List<Hashtable> customEvents = new List<Hashtable>();
            MikrosManager.Instance.AnalyticsController.LogEvent(eventName, hashtableParameter[hashtableDataIndex], customEvent =>
            {
                customEvents.Add(customEvent);
            },
            onFailure =>
            {
                LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                Assert.Fail(onFailure.Message);
            });

            MikrosManager.Instance.AnalyticsController.TestCustomEvents(customEvents, response =>
            {
                eventLogResponse = response;
                isEventLogCompleted = true;
            });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the sign-in request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackCustomEventWithMoreThan20Params([ValueSourceAttribute(nameof(eventName))] string eventName)
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            Hashtable hashtable = new Hashtable();
            List<Hashtable> customEvents = new List<Hashtable>();
            for (int i = 0; i < 22; i++)
            {
                hashtable.Add("newHash_param_" + i, "newParam_value_" + i);
            }
            MikrosManager.Instance.AnalyticsController.LogEvent(eventName, hashtable, customEvent =>
            {
                customEvents.Add(customEvent);
            },
            onFailure =>
            {
                LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                Assert.Fail(onFailure.Message);
            });

            MikrosManager.Instance.AnalyticsController.TestCustomEvents(customEvents, response =>
            {
                eventLogResponse = response;
                isEventLogCompleted = true;
            });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the sign-in request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }

        [UnityTest]
        public IEnumerator TrackCustomEventsWithMixedEvents()
        {
            yield return new WaitUntil(() => MikrosManager.Instance.IsInitialized);
            bool isEventLogCompleted = false;
            GeneralResponse eventLogResponse = null;
            List<Hashtable> customEvents = new List<Hashtable>();
            List<KeyValuePair<string, int>> paramCounts = new List<KeyValuePair<string, int>> {
                new KeyValuePair<string, int>(DateTime.Now.ToShortDateString() + "random_custom", 22),
                new KeyValuePair<string, int>( "", 10) };

            for (int i = 0; i < paramCounts.Count; i++)
            {
                Hashtable hashtable = new Hashtable();
                for (int j = 0; j < paramCounts[i].Value; j++)
                {
                    hashtable.Add("2NewHash_param_" + j, "2NewParam_value_" + j);
                }
                MikrosManager.Instance.AnalyticsController.LogEvent(paramCounts[i].Key, hashtable, customEvent =>
                 {
                     customEvents.Add(customEvent);
                 },
                onFailure =>
                {
                    LogAssert.Expect(LogType.Error, TestConstants.MikrosError + onFailure.Message);
                    Assert.Fail(onFailure.Message);
                });
            }

            MikrosManager.Instance.AnalyticsController.TestCustomEvents(customEvents, response =>
            {
                eventLogResponse = response;
                isEventLogCompleted = true;
            });

            yield return new WaitUntil(() => isEventLogCompleted);

            // Testing to check if Success status code is returned in response for the sign-in request
            Assert.AreEqual(STATUS_TYPE.SUCCESS, Utils.DetectStatusType(eventLogResponse.Status.StatusCode));
        }
    }
}
