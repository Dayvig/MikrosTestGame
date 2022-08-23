using UnityEngine;
using UnityEngine.UI;

namespace MikrosClient.UI
{
    /// <summary>
    /// Class to represent UI for each game category.
    /// </summary>
    internal sealed class GameCategory : MonoBehaviour
    {
        /// <summary>
        /// Text field for the name of game's category.
        /// </summary>
        [SerializeField] private Text categoryNameText;

        /// <summary>
        /// Text field for the short description of game's category.
        /// </summary>
        [SerializeField] private Text categoryDescriptionText;

        /// <summary>
        /// Parent transform of all the apps.
        /// </summary>
        [SerializeField] private Transform normalAppsContainer;

        private AppsMicroData appsMicroData;
        private GameCategoryFullViewPanel gameCategoryFullViewPanel;

        /// <summary>
        /// Preliminary initialization for the game category view in UI.
        /// </summary>
        /// <param name="categoryData">Data of the game category received from response.</param>
        /// <param name="categoryDescription">Short description of the category.</param>
        /// <param name="gameCategoryFullViewPanel">Reference of the full view panel of game category.</param>
        /// <param name="appsContainer">Returns the tranform which acts as a container for the apps of this game category.</param>
        internal void Initialize(AppsMicroData categoryData, string categoryDescription, GameCategoryFullViewPanel gameCategoryFullViewPanel, out Transform appsContainer)
        {
            this.appsMicroData = categoryData;
            this.gameCategoryFullViewPanel = gameCategoryFullViewPanel;
            categoryNameText.text = categoryData.Title;
            categoryDescriptionText.text = categoryDescription;
            appsContainer = normalAppsContainer;
        }

        /// <summary>
        /// On click event for button to open game category full view.
        /// </summary>
        public void OnClickOpenFullView()
        {
            gameCategoryFullViewPanel.gameObject.SetActive(true);
            gameCategoryFullViewPanel.Initialize(appsMicroData.Title, normalAppsContainer);
        }
    }
}
