using System;
using System.Collections.Generic;

namespace MikrosClient.GameService
{
    /// <summary>
    /// Reputation scoring handling.
    /// </summary>
    internal sealed class ReputationScoreHandler
    {
        /// <summary>
        /// Object creation of this class only allowed internally.
        /// </summary>
        internal ReputationScoreHandler()
        { }

        /// <summary>
        /// Get the rating score value.
        /// </summary>
        /// <param name="playerBehavior">Behavior type of player.</param>
        /// <returns>Reputation score according to behaviour type.</returns>
        internal int ReputationScore(PlayerBehavior playerBehavior)
        {
            switch (playerBehavior)
            {
                case PlayerBehavior.POOR_SPORTSMANSHIP:
                    return -1;

                case PlayerBehavior.TROLLING:
                    return -2;

                case PlayerBehavior.CONSTANT_PINGING:
                    return -2;

                case PlayerBehavior.AFK:
                    return -3;

                case PlayerBehavior.COMPLAINING:
                    return -3;

                case PlayerBehavior.OFFENSIVE_LANGUAGE:
                    return -5;

                case PlayerBehavior.CHEATING:
                    return -5;

                case PlayerBehavior.GOOD_SPORTSMANSHIP:
                    return 1;

                case PlayerBehavior.GREAT_LEADERSHIP:
                    return 2;

                case PlayerBehavior.EXCELLENT_TEAMMATE:
                    return 3;

                case PlayerBehavior.MVP:
                    return 5;

                default:
                    return 0;
            }
        }

        /// <summary>
        /// Upload behavior rating of other players.
        /// </summary>
        /// <param name="playerRatingRequest">Request object for player rating.</param>
        /// <param name="callback">Response callback for sending player rating.</param>
        internal void SendPlayerRating(PlayerRating playerRatingRequest, Action<GeneralResponse> callback = null)
        {
            string requestJson = DataConverter.SerializeObject(playerRatingRequest);
            MikrosLogger.Log("Player rating:\n" + requestJson);
            PlayerRatingPostRequest(requestJson, callback);
        }

        /// <summary>
        /// Upload player rating.
        /// </summary>
        /// <param name="requestJson">Json format for the request.</param>
        /// <param name="callback">Callback for the player rating upload process.</param>
        private void PlayerRatingPostRequest(string requestJson, Action<GeneralResponse> callback = null)
        {
            if (!MikrosManager.Instance.IsInitialized)
            {
                callback?.Invoke(null);
                throw new MikrosException(ExceptionType.INITIALIZE_MIKROS_SDK);
            }
            Dictionary<string, string> customHeaders = new Dictionary<string, string>();
            customHeaders.Add(Constants.ApiKeyHeaderKey, MikrosManager.Instance.ConfigurationController.MikrosSettings.GetCurrentApiKey());
            WebRequest<GeneralResponse>.Builder()
                .Url(ServerData.GetUrl(UrlType.PlayerRating))
                .RawJsonData(requestJson)
                .CustomHeaders(customHeaders)
                .CreatePostRequest(callback);
        }
    }
}