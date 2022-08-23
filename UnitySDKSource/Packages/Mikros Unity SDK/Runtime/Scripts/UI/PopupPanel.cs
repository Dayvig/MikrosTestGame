using UnityEngine;
using UnityEngine.UI;

namespace MikrosClient.UI
{
    /// <summary>
    /// PopupPanel class.
    /// Creates an object, display in Unity Editor, and closes the popup panel.
    /// Two Unity Text components:
    /// 1. txtTile stores data for message title.
    /// 2. txtMessage stores data for popup messages.
    /// </summary>
    internal sealed class PopupPanel : MonoBehaviour
    {
        [SerializeField]
        private Text txtTitle;

        [SerializeField]
        private Text txtMessage;

        /// <summary>
        /// Builder function is used for creating an object of PopupPanel class.
        /// Create an object of PopupPanel, disable PopupPanel to enable from outside, and set object as last sibling of its parent.
        /// </summary>
        /// <returns>popupPanel object</returns>
        internal static PopupPanel Builder()
        {
            PopupPanel popupPanel = Instantiate(Resources.Load<PopupPanel>(Constants.PopupPanelPrefabName), MikrosUiCanvas.Instance.transform, false);
            popupPanel.gameObject.SetActive(false);
            popupPanel.transform.SetAsLastSibling();
            return popupPanel;
        }

        /// <summary>
        /// ShowPopup function is used for showing a popup with required message title and message.
        /// 
        /// 1. Set the object to true to display the popup in Unity Editor.
        /// 2. Assign the parameter value title to txtTitle and message to txtMessage.
        /// </summary>
        /// <param name="title"> title is for message title</param>
        /// <param name="message"> message is for a message string</param>
        public void ShowPopup(string title, string message)
        {
            gameObject.SetActive(true);
            txtTitle.text = title;
            txtMessage.text = message;
        }

        /// <summary>
        /// Close function is used for closing the popup panel.
        /// </summary>
        public void Close()
        {
            Destroy(gameObject);
        }
    }
}