using UnityEngine;
using UnityEngine.UI;

namespace MikrosClient.UI
{
    /// <summary>
    /// LoadingPanel class is used for showing a loading whenever an api is called.
    /// </summary>
    public sealed class LoadingPanel : MonoBehaviour
    {
        /// <summary>
        /// private object of the LoadingPanel class.
        /// </summary>
        private static LoadingPanel _instance;

        /// <summary>
        /// Text component of loading message.
        /// </summary>
        [SerializeField]
        private Text txtLoading;

        /// <summary>
        /// Builder function is used for creating an object of LoadingPanel class.
        /// </summary>
        /// <returns>LoadingPanel object.</returns>
        public static LoadingPanel Builder()
        {
            // if object not created then create the object first
            if (_instance == null)
            {
                _instance = Instantiate(Resources.Load<LoadingPanel>(Constants.LoadingPanelPrefabName), MikrosUiCanvas.Instance.transform, false);
                _instance.gameObject.SetActive(false); // Disable LoadingPanel to enable form outside.
            }
            _instance.transform.SetAsLastSibling(); // Setting this object as last sibling of it's parent
            return _instance;
        }

        /// <summary>
        /// ShowLoading function is used for showing the loading panel with required message.
        /// </summary>
        /// <param name="loadingMessage">loading message that will shown when loading panel is active.</param>
        public void ShowLoading(string loadingMessage = "Loading..")
        {
            gameObject.SetActive(true);
            txtLoading.text = loadingMessage;
        }

        /// <summary>
        /// Hide the loading panel function.
        /// </summary>
        public void HideLoading()
        {
            gameObject.SetActive(false);
        }
    }
}