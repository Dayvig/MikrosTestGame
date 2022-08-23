using System.Collections;
using System.Collections.Generic;
using MikrosClient;
using MikrosClient.Analytics;
using MikrosClient.Config;
using MikrosClient.GameService;
using UnityEngine;

public class MikrosCodeExamples
{
    /*
 * There are some scenarios for the Mikros initialization:
 * 1. If the "Auto Initialize Mikros SDK" option is set to TRUE in Editor, then integrators don't require to write any lines of code to initialize Mikros.
 *    The privacy status will be set up according to the configuration of Mikros Settings in Editor. The following settings' options will be taken into consideration:
 *    a) "Auto Track User Session"
 *    b) "Auto Track User Metadata"
 *
 * 2. If the "Auto Initialize Mikros SDK" option is set to TRUE in Editor, the integrators will still have option to Initialize Mikros SDK from code.
 *    However if a Configuration parameter is passed to the MikrosManager.Instance.InitializeMikrosSDK method, then it will override the privacy configurations set in Mikros Settings from Editor.
 *
 * 3. If the "Auto Initialize Mikros SDK" option is set to FALSE in Editor, then its mandatory for integrators to call MikrosManager.Instance.InitializeMikrosSDK method from code.
 *
 *
 * Note: The privacy configuration that is set in MikrosSettings from Editor, will always get overriden whenever a Configuration parameter is passed to the MikrosManager.Instance.InitializeMikrosSDK method.
 */

    /// <summary>
    /// Initialize Mikros with Configuration.
    /// Create a Configuration class with a Set Privacy Level to Extreme.  No metadata and session tracking.
    /// Initialize Mikros with the object instantiated from Configuration class.
    /// Manual Configuration settings:
    /// 1. Configure the setting to Set Auto Track User Session, by default set to true.
    /// 2. Configure the setting to Set Auto Track User Meta data, by default set to true.
    /// 3. Configure the setting to Set Event Logging, by default set to true.
    /// 4. Configure the setting to Set Auto Crash Reporting, by default set to true.
    /// 5. Configure the setting to Set Auto Track Device Memory, by default set to true.
    /// 6. Configure the setting to Set All Tracking Enabled, by default set to true.
    /// Note: At this point, the client configuration status is similar to Set Privacy Level to Default.
    /// </summary>
    public void InitializeMikrosWithConfiguration()
    {
        Configuration configuration =
            Configuration
                .Builder()
                .SetPrivacyLevel(PRIVACY_LEVEL.EXTREME)
                .Create();

        MikrosManager.Instance.InitializeMikrosSDK(configuration);

        MikrosManager
            .Instance
            .ConfigurationController
            .SetAutoTrackUserSession(true);

        MikrosManager
            .Instance
            .ConfigurationController
            .SetAutoTrackUserMetadata(true);

        MikrosManager
            .Instance
            .ConfigurationController
            .SetEventLogging(true);

        MikrosManager
            .Instance
            .ConfigurationController
            .SetAutoCrashReporting(true);

        MikrosManager
            .Instance
            .ConfigurationController
            .SetAutoTrackDeviceMemory(true);

        MikrosManager
            .Instance
            .ConfigurationController
            .SetAllTrackingEnabled(true);
    }

    /// <summary>
    /// Initialize Mikros without Configuration.
    /// </summary>
    public void InitializeMikrosWithoutConfiguration()
    {
        MikrosManager.Instance.InitializeMikrosSDK();
    }

    /// <summary>
    /// Get Mikros Setting Config.
    /// Create variables that store the data of the key value, key type, and current api key selection.
    /// Variables:
    /// 1. Store the value of App Game ID set by Integrator from Editor.
    /// 2. Store the value of API key Production set by Integrator from Editor.
    /// 3. Store the value of API key Development set by Integrator from Editor.
    /// 4. Store the value of API key QA set by Integrator from Editor.
    /// 5. Store the current API key type set by Integrator from Editor.
    /// 6. Store the current API key being used set by Integrator from Editor.
    /// </summary>
    public void GetMikrosSettingsConfig()
    {
        string appGameId =
            MikrosManager
                .Instance
                .ConfigurationController
                .MikrosSettings
                .AppGameID;
        Debug.Log(appGameId);

        string apiKeyProduction =
            MikrosManager
                .Instance
                .ConfigurationController
                .MikrosSettings
                .ApiKeyProduction;
        Debug.Log(apiKeyProduction);

        string apiKeyDevelopment =
            MikrosManager
                .Instance
                .ConfigurationController
                .MikrosSettings
                .ApiKeyDevelopment;
        Debug.Log(apiKeyDevelopment);

        string apiKeyQA =
            MikrosManager
                .Instance
                .ConfigurationController
                .MikrosSettings
                .ApiKeyQA;
        Debug.Log(apiKeyQA);

        API_KEY_TYPE apiKeyType =
            MikrosManager
                .Instance
                .ConfigurationController
                .MikrosSettings
                .CurrentApiKeyType;
        Debug.Log(apiKeyType.ToString());

        string currentUsedApiKey =
            MikrosManager
                .Instance
                .ConfigurationController
                .MikrosSettings
                .GetCurrentApiKey();
        Debug.Log(currentUsedApiKey);
    }

    /// <summary>
    /// Track Custom Event.
    /// When creating a Custom Event the first parameter is mandatory for the event name.  The following parameters are optional for the type and value.
    /// Templates for Custom Events:
    /// 1. Track Custom Event without arguments.
    /// 2. Track Custom Event with string type arguments.
    /// 3. Track Custom Event with long type parameter.
    /// 4. Track Custom Event with double type parameter.
    /// 5. Track Custom Event with multiple parameters of any datatype.
    /// Ref- https://developer.tatumgames.com/documentation/log-events
    /// </summary>
    public void TrackCustomEvent()
    {
        MikrosManager
            .Instance
            .AnalyticsController
            .LogEvent("Event Name 1",
            (Hashtable customEvent) =>
            {
                // a Hashtable of the custom event is returned after success.
            },
            onFailure =>
            {
                Debug.Log(onFailure.Message);
            });

        MikrosManager
            .Instance
            .AnalyticsController
            .LogEvent("Event Name 2",
            "test_key",
            "custom_test_value",
            (Hashtable customEvent) =>
            {
                // a Hashtable of the custom event is returned after success.
            },
            onFailure =>
            {
                Debug.Log(onFailure.Message);
            });

        MikrosManager
            .Instance
            .AnalyticsController
            .LogEvent("Event Name 3",
            "test_key",
            1,
            (Hashtable customEvent) =>
            {
                // a Hashtable of the custom event is returned after success.
            },
            onFailure =>
            {
                Debug.Log(onFailure.Message);
            });

        MikrosManager
            .Instance
            .AnalyticsController
            .LogEvent("Event Name 4",
            "test_key",
            3.67f,
            (Hashtable customEvent) =>
            {
                // a Hashtable of the custom event is returned after success.
            },
            onFailure =>
            {
                Debug.Log(onFailure.Message);
            });

        Hashtable dataset = new Hashtable();
        dataset.Add("key_1", "custom_test_value");
        dataset.Add("key_2", 1);
        dataset.Add("key_3", 3.67f);
        MikrosManager
            .Instance
            .AnalyticsController
            .LogEvent("Event Name 5",
            dataset,
            (Hashtable customEvent) =>
            {
                Debug.Log("Events Logged successfully.");
            },
            onFailure =>
            {
                Debug.Log(onFailure.Message);
            });
    }

    /// <summary>
    /// Showing example with one Preset event.
    /// </summary>
    public void TrackPresetEvent()
    {
        TrackPostScoreRequest
            .Builder()
            .Score(157)
            .Level(3)
            .SubLevel(7)
            .LevelName("Dungeons")
            .Character("Scorpio")
            .Create(trackPostScoreRequest =>
                MikrosManager
                    .Instance
                    .AnalyticsController
                    .LogEvent(trackPostScoreRequest,
                    response =>
                    {
                        Debug.Log(response.Status.StatusMessage);
                    }),
            onFailure =>
            {
                Debug.Log(onFailure.Message);
            });
    }

    /// <summary>
    /// Example of sending a player rating through MIKROS by creating a player rating request.
    /// </summary>
    public void SendPlayerRating()
    {
        List<Participant> participants = new List<Participant>();

        Participant
            .Builder()
            .DeviceId("AXBY342")
            .Email("test@test.com")
            .Behavior(PlayerBehavior.GREAT_LEADERSHIP)
            .Create(participantRequest =>
            {
                participants.Add(participantRequest);
            },
            onFailure =>
            {
                Debug.Log(onFailure.Message);
            });

        PlayerRating
            .Builder()
            .Participants(participants)
            .Create(playerRatingRequest =>
            {
                MikrosManager
                    .Instance
                    .GameServiceController
                    .SendPlayerRating(playerRatingRequest,
                    response =>
                    {
                        STATUS_TYPE statusType =
                            Utils.DetectStatusType(response.Status.StatusCode);
                        if (statusType == STATUS_TYPE.SUCCESS)
                        {
                            Debug
                                .Log("Success message: " +
                                response.Status.StatusMessage);
                        }
                        else if (statusType == STATUS_TYPE.ERROR)
                        {
                            Debug
                                .Log("Error message: " +
                                response.Status.StatusMessage);
                        }
                        else
                        {
                            Debug
                                .Log("Other issue status message: " +
                                response.Status.StatusMessage);
                        }
                    });
            },
            onFailure =>
            {
                Debug.Log(onFailure.Message);
            });
    }

    /// <summary>
    /// Single Sign-on Authentication for user.
    /// </summary>
    public void OpenMikrosSSO()
    {
        MikrosManager.Instance.AuthenticationController.LaunchSignin();
        MikrosManager
            .Instance
            .AuthenticationController
            .LaunchSignin((MikrosUser mikrosUser) =>
            {
                Debug.Log(mikrosUser.Email);
            });
    }

    /// <summary>
    /// Example of a user sign in with MIKROS.
    /// </summary>
    public void MikrosSignin()
    {
        SigninRequest
            .Builder()
            .Username("sampleUser")
            .Email("sampleUser@gmail.com")
            .Password("abcd1234")
            .Create(signinRequest =>
            {
                MikrosManager
                    .Instance
                    .AuthenticationController
                    .Signin(signinRequest,
                    successResponse =>
                    {
                        Debug.Log("Sign in success.");
                    },
                    failureResponse =>
                    {
                        Debug.Log(failureResponse.Message);
                    });
            },
            onFailure =>
            {
                Debug.Log(onFailure.Message);
            });
    }

    /// <summary>
    /// Example of a user signup request with MIKROS.
    /// </summary>
    public void MikrosSignup()
    {
        SignupRequest
            .Builder()
            .Username("sampleUser")
            .Email("sampleUser@gmail.com")
            .Password("abcd1234")
            .Create(signupRequest =>
            {
                Debug.Log("All input correct!!!!");

                MikrosManager
                    .Instance
                    .AuthenticationController
                    .Signup(signupRequest,
                    successResponse =>
                    {
                        Debug.Log("Sign up success.");
                    },
                    failureResponse =>
                    {
                        Debug.Log(failureResponse.Message);
                    });
            },
            onFailure =>
            {
                Debug.Log(onFailure.Message);
            });
    }

    /// <summary>
    /// Example of signing out with MIKROS.
    /// </summary>
    public void MikrosSignout()
    {
        MikrosManager
            .Instance
            .AuthenticationController
            .Signout(() =>
            {
                Debug.Log("Sign out success.");
            },
            failureResponse =>
            {
                Debug.Log(failureResponse.Message);
            });
    }
}