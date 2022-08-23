using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MikrosClient.UI
{
    /// <summary>
    /// Full view panel for any game category in Games tab.
    /// </summary>
    internal sealed class GameCategoryFullViewPanel : MonoBehaviour
    {
        /// <summary>
        /// Text field for the name of game's category.
        /// </summary>
        [SerializeField] private Text categoryNameText;

        /// <summary>
        /// Parent transform of panel under which all apps are to be placed.
        /// </summary>
        [SerializeField] private Transform categoryFullViewAppsContainer;

        /// <summary>
        /// Temp record of game category apps' parent.
        /// </summary>
        private Transform appsParentInTab;

        /// <summary>
        /// Preliminary initialization of the panel view.
        /// </summary>
        /// <param name="categoryName">Name of the game category.</param>
        /// <param name="appsParent">Current parent of apps in category.</param>
        internal void Initialize(string categoryName, Transform appsParent)
        {
            categoryNameText.text = categoryName;
            appsParentInTab = appsParent;
            TransferAppsParent(appsParentInTab, categoryFullViewAppsContainer);
        }

        /// <summary>
        /// Transfer parent of all apps from one transform to another.
        /// </summary>
        /// <param name="fromParent">Parent under which apps are currently placed.</param>
        /// <param name="toParent">New parent to transfer all apps.</param>
        private void TransferAppsParent(Transform fromParent, Transform toParent)
        {
            NormalApp[] apps = fromParent.GetComponentsInChildren<NormalApp>(true);
            for (int i = 0; i < apps.Length; i++)
            {
                apps[i].transform.SetParent(toParent);
            }
        }

        /// <summary>
        /// Called everytime attached gameobject is disabled.
        /// </summary>
		private void OnDisable()
        {
            TransferAppsParent(categoryFullViewAppsContainer, appsParentInTab);
        }
    }
}
