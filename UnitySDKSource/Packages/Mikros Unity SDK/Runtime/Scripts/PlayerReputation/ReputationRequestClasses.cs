using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MikrosClient.GameService
{
    /// <summary>
    /// Data Structure for Player Rating API request.
    /// </summary>
    public sealed class PlayerRating
    {
        #region Request Properties

        [JsonProperty(PropertyName = "appGameId")]
        private string appGameId = MikrosManager.Instance.ConfigurationController.MikrosSettings.AppGameID;

        [JsonProperty(PropertyName = "apiKeyType")]
        private string apiKeyType = MikrosManager.Instance.ConfigurationController.MikrosSettings.GetApiKeyTypeString();

        [JsonProperty(PropertyName = "sender")]
        private Sender sender;

        [JsonProperty(PropertyName = "participants")]
        private List<Participant> participants;

        #endregion Request Properties

        /// <summary>
        /// Returns true if PlayerRating object creation is completed successfully, else false.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// PlayerRating private constructor to restrict object creation of the class from outside.
        /// </summary>
        private PlayerRating()
        {
        }

        /// <summary>
        /// Builder function return a new object of the PlayerRating class.
        /// </summary>
        /// <returns> Returns new PlayerRating object.</returns>
        public static PlayerRating Builder()
        {
            return new PlayerRating();
        }

        /// <summary>
        /// Create function is used for validation of variables of the PlayerRating class.
        /// </summary>
        /// <param name="onSuccess">Callback for PlayerRating object creation success.</param>
        /// <param name="onFailure">Callback for PlayerRating object creation failure.</param>
        /// <returns>PlayerRating object.</returns>
        public PlayerRating Create(Action<PlayerRating> onSuccess, Action<MikrosException> onFailure)
        {
            this.sender = Sender.Builder().Create();
            MikrosException mikrosException = null;
            if (string.IsNullOrEmpty(appGameId)) // Check for appGameId validation.
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_APP_GAME_ID); // throw an exception about appGameId is invalid.
            }
            else if (string.IsNullOrEmpty(MikrosManager.Instance.ConfigurationController.MikrosSettings.GetCurrentApiKey())) // Check for parameter validation.
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_API_KEY); // throw an exception about apiKeyType is invalid.
            }
            else if (sender == null)
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Sender information not provided"); // throw an exception about parameter is invalid.
            }
            else if (participants == null || participants.Count <= 0)
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "No participants added"); // throw an exception about parameter is invalid.
            }
            if (mikrosException == null)
            {
                IsCreated = true;
                onSuccess(this);
            }
            else
            {
                onFailure(mikrosException);
            }
            return this; // return class object if validation success.
        }

        /// <summary>
        /// Participants function is used for setting the "participants" variable of the PlayerRating class.
        /// </summary>
        /// <param name="participants">Collection of Participants.</param>
        /// <returns>PlayerRating object.</returns>
        public PlayerRating Participants(List<Participant> participants)
        {
            this.participants = participants; // setting the participants.
            return this;
        }
    }

    /// <summary>
    /// Data structure to hold information of sender of the player rating.
    /// </summary>
    internal sealed class Sender
    {
        #region Request Properties

        [JsonProperty(PropertyName = "deviceId")]
        private string deviceId = Constants.DeviceID;

        #endregion Request Properties

        /// <summary>
        /// Returns true if Sender object creation is completed successfully, else false.
        /// </summary>
        [JsonIgnore]
        internal bool IsCreated { get; private set; }

        /// <summary>
        /// Sender private constructor to restrict object creation of the class from outside.
        /// </summary>
        private Sender()
        {
        }

        /// <summary>
        /// Builder function return a new object of the Sender class.
        /// </summary>
        /// <returns> Returns new Sender object.</returns>
        internal static Sender Builder()
        {
            return new Sender();
        }

        /// <summary>
        /// Create function is used for validation of variables of the Sender class.
        /// </summary>
        /// <returns>Sender object.</returns>
        internal Sender Create()
        {
            IsCreated = true;
            return this;
        }
    }

    /// <summary>
    /// Data structure to hold information of the player being rated.
    /// </summary>
    public class Participant
    {
        #region Request Properties

        [JsonProperty(PropertyName = "deviceId")]
        private string deviceId;

        [JsonProperty(PropertyName = "email")]
        private string email = "";

        [JsonProperty(PropertyName = "value")]
        private int value;

        #endregion Request Properties

        /// <summary>
        /// Returns true if Participant object creation is completed successfully, else false.
        /// </summary>
        [JsonIgnore]
        public bool IsCreated { get; private set; }

        /// <summary>
        /// Participant private constructor to restrict object creation of the class from outside.
        /// </summary>
        private Participant()
        {
        }

        /// <summary>
        /// Builder function return a new object of the Participant class.
        /// </summary>
        /// <returns> Returns new Participant object.</returns>
        public static Participant Builder()
        {
            return new Participant();
        }

        /// <summary>
        /// Create function is used for validation of variables of the Participant class.
        /// </summary>
        /// <param name="onSuccess">Callback for Participant object creation success.</param>
        /// <param name="onFailure">Callback for Participant object creation failure.</param>
        /// <returns>Participant object.</returns>
        public Participant Create(Action<Participant> onSuccess, Action<MikrosException> onFailure)
        {
            MikrosException mikrosException = null;
            if (string.IsNullOrEmpty(deviceId))
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "ID of participant not provided");  // throw an exception about parameter is invalid.
            }
            else if (!new List<int> { 1, 2, 3, 5 }.Contains(Mathf.Abs(value)))
            {
                mikrosException = new MikrosException(ExceptionType.INVALID_PARAMETER, "Invalid Value provided");  // throw an exception about parameter is invalid.
            }
            if (mikrosException == null)
            {
                IsCreated = true;
                onSuccess(this);
            }
            else
            {
                onFailure(mikrosException);
            }
            return this; // return class object if validation success.
        }

        /// <summary>
        /// DeviceId function is used for setting the "deviceId" variable of the Participant class.
        /// </summary>
        /// <param name="deviceId">Device ID of participant.</param>
        /// <returns>Participant object.</returns>
        public Participant DeviceId(string deviceId)
        {
            this.deviceId = deviceId; // setting the deviceId.
            return this;
        }

        /// <summary>
        /// Email function is used for setting the "email" variable of the Participant class.
        /// (Optional Parameter)
        /// </summary>
        /// <param name="email">Email of participant.</param>
        /// <returns>Participant object.</returns>
        public Participant Email(string email)
        {
            this.email = email; // setting the email.
            return this;
        }

        /// <summary>
        /// Behavior function is used for setting the "value" and "behavior" variables of the Participant class.
        /// </summary>
        /// <param name="playerBehavior">Behavior type of a participant player.</param>
        /// <returns>Participant object.</returns>
        public Participant Behavior(PlayerBehavior playerBehavior)
        {
            this.value = MikrosManager.Instance.GameServiceController.ReputationScoreHandler.ReputationScore(playerBehavior); // setting the value.
            return this;
        }
    }
}