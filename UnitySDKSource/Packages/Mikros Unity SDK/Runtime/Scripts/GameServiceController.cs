using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MikrosClient.GameService
{
    /// <summary>
    /// Handles all game services provided by Mikros.
    /// </summary>
    public sealed class GameServiceController
    {
        /// <summary>
        /// Instance of the ReputationScoreController class.
        /// </summary>
        internal ReputationScoreHandler ReputationScoreHandler = new ReputationScoreHandler();

        /// <summary>
        /// Object creation of this class only allowed internally.
        /// </summary>
        internal GameServiceController()
        { }

        /// <summary>
        /// Upload behavior rating of other players.
        /// </summary>
        /// <param name="playerRatingRequest">Request object for player rating.</param>
        /// <param name="callback">Response callback for sending player rating.</param>
        public void SendPlayerRating(PlayerRating playerRatingRequest, Action<GeneralResponse> callback = null)
        {
            ReputationScoreHandler.SendPlayerRating(playerRatingRequest, callback);
        }
    }
}
