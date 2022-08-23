using MikrosClient.Authentication;
using MikrosClient.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace MikrosClient.UI
{
    /// <summary>
    /// MikrosUiCanvas class is used for showing custom UI of Mikros SDK.
    /// </summary>
    [DefaultExecutionOrder(-8)]
    internal sealed class MikrosUiCanvas : MonoBehaviour
    {
        /// <summary>
        /// Animation Speed for general purpose.
        /// </summary>
        private float generalAnimationSpeed = 0.3f;

        /// <summary>
        /// Minimum value of local scale of Mikros logo while scaling down.
        /// </summary>
        private Vector3 mikrosLogoScaleDownMinValue => Vector3.one * 0.3f;

        /// <summary>
        /// mikrosButtonRectTransform contains Widget small icon object that will shown on device's right or left screen.
        /// </summary>
        private RectTransform mikrosButtonRectTransform;

        /// <summary>
        /// mikrosMainPanel contains a panel that will shown when user will click on Widget small icon.
        /// </summary>
        [SerializeField]
        private GameObject mikrosMainPanel;

        /// <summary>
        /// mikrosLogo contains Widget big icon object in main panel.
        /// </summary>
        [SerializeField]
        private Transform mikrosLogo;

        /// <summary>
        ///  _openButton contains Open button of main panel.
        /// </summary>
        [SerializeField]
        private Button openButton;

        /// <summary>
        /// _closeButton contains Close button of main panel.
        /// </summary>
        [SerializeField]
        private Button closeButton;

        internal event Action<ScreenOrientation> OnOrientationChange;

        private bool rectDimensionChanged = false;

        /// <summary>
        /// private object of MikrosUiCanvas.
        /// </summary>
        private static MikrosUiCanvas instance;

        /// <summary>
        /// internal object of MikrosUiCanvas.
        /// </summary>
        internal static MikrosUiCanvas Instance
        {
            get
            {
                CreateInstance();
                return instance;
            }
        }

        /// <summary>
        /// Creates a new instance of MikrosUiCanvas if its currently null.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateInstance()
        {
            if (Equals(instance, null))
            {
                instance = Instantiate(Resources.Load<MikrosUiCanvas>(Constants.MikrosUiCanvasPrefabName), MikrosManagerInitializer.Instance.transform);
                MikrosLogger.Log("Canvas created");
            }
        }

        /// <summary>
        /// Called every time MikrosUiCanvas is enabled.
        /// </summary>
        private void OnEnable()
        {
            MikrosManager.Instance.AuthenticationController.OnClickMikrosSSO += OnClickMikrosSSO;
            openButton.onClick.AddListener(LaunchMikrosAppStore);
            closeButton.onClick.AddListener(CloseMikrosPanel);
        }

        /// <summary>
        /// Handler for Mikros SSO open event.
        /// </summary>
        /// <param name="onHandleAuthSuccess">Callback for authentication success.</param>
        private void OnClickMikrosSSO(MikrosSSORequest onHandleAuthSuccess)
        {
            AuthenticationPanel.Builder().LaunchSignin(onHandleAuthSuccess);
        }

        /// <summary>
        /// Called when user clicked on Open button in main panel.
        /// </summary>
        private void LaunchMikrosAppStore()
        {
            // if access token is not null then show All games fetching
            if (!string.IsNullOrEmpty(MikrosManager.Instance.AuthenticationController.Session?.AccessToken))
            {
                MikrosAppStore.Builder().Show();
                MikrosManager.Instance.AdController.StoreListener.InvokeStoreOpened();
            }
            // If access token is null or empty then show the error popup.
            else
            {
                MikrosManager.Instance.AdController.StoreListener.InvokeStoreError(new MikrosException(ExceptionType.OTHER, Constants.MikrosAppStoreError));
            }
        }

        /// <summary>
        /// Called upon changing of dimension of this canvas rect or any child of this.
        /// Note: Used here to detect screen orientation change at runtime.
        /// </summary>
        private void OnRectTransformDimensionsChange()
        {
            rectDimensionChanged = true;
        }

        /// <summary>
        /// OpenMikrosPanel function is used for Showing the mikros main panel when user clicked on Widget big icon.
        /// </summary>
        /// <param name="mikrosButtonRectTransform">RectTransform of Mikros button.</param>
        internal void OpenMikrosPanel(RectTransform mikrosButtonRectTransform)
        {
            this.mikrosButtonRectTransform = mikrosButtonRectTransform;
            iTween.Stop(openButton.gameObject); // Stopping the openButton animation if playing.
            iTween.Stop(closeButton.gameObject); // Stopping the closeButton animation if playing.
            iTween.Stop(mikrosLogo.gameObject); // Stopping the mikrosLogo animation if playing.

            mikrosMainPanel.SetActive(true); // Activate mikrosMainPanel window.
            mikrosButtonRectTransform.gameObject.SetActive(false); // Deactivating Widget small icon.
            Vector3 currentPosition = mikrosLogo.position;
            mikrosLogo.position = mikrosButtonRectTransform.position; // resetting the position of mikrosLogo to Mikros Button position.
            mikrosLogo.localScale = mikrosLogoScaleDownMinValue; // resetting the scale of mikrosLogo.

            // Moving mikrosLogo to center position of the screen.
            iTween.MoveTo(mikrosLogo.gameObject, iTween.Hash("position", currentPosition, "time", generalAnimationSpeed, "oncomplete", nameof(MikrosPanelOpenComplete), "oncompletetarget", gameObject));
            iTween.ScaleTo(mikrosLogo.gameObject, Vector3.one, generalAnimationSpeed);
            closeButton.transform.localScale = new Vector3(0, 0, 0);
            openButton.transform.localScale = closeButton.transform.localScale;
        }

        /// <summary>
        /// Task after Mikros Panel opening completed.
        /// </summary>
        private void MikrosPanelOpenComplete()
        {
            iTween.ScaleTo(openButton.gameObject, Vector3.one, generalAnimationSpeed); // Scaling the openButton to scale one
            iTween.ScaleTo(closeButton.gameObject, Vector3.one, generalAnimationSpeed); // Scaling the closeButton to scale one
        }

        /// <summary>
        /// CloseMikrosPanel function is used for closing the mikros main panel.
        /// </summary>
        private void CloseMikrosPanel()
        {
            iTween.Stop(openButton.gameObject);
            iTween.Stop(closeButton.gameObject);
            iTween.Stop(mikrosLogo.gameObject);

            iTween.ScaleTo(closeButton.gameObject, Vector3.zero, generalAnimationSpeed); // Scaling the closeButton to scale zero
            iTween.ScaleTo(openButton.gameObject, Vector3.zero, generalAnimationSpeed); // Scaling the openButton to scale zero
            iTween.ScaleTo(mikrosLogo.gameObject, mikrosLogoScaleDownMinValue, generalAnimationSpeed); // Scaling down the mikrosLogo
            Vector3 currentPosition = mikrosLogo.position;

            // Moving mikrosLogo to Mikros Button position.
            iTween.MoveTo(mikrosLogo.gameObject, iTween.Hash("position", mikrosButtonRectTransform.position, "time", generalAnimationSpeed, "oncomplete", nameof(MikrosPanelCloseCompleted), "oncompleteparams", mikrosLogo.position, "oncompletetarget", gameObject));
        }

        /// <summary>
        /// Task after Mikros Panel closing completed.
        /// </summary>
        /// <param name="currentMikrosLogoPosition">Default position for Mikros logo.</param>
        private void MikrosPanelCloseCompleted(Vector3 currentMikrosLogoPosition)
        {
            mikrosLogo.position = currentMikrosLogoPosition;
            mikrosMainPanel.SetActive(false);  // Deactivating mikros main panel when mikrosLogo moving animation completed
            mikrosButtonRectTransform.gameObject.SetActive(true); // Activating Widget small icon when mikrosLogo moving animation completed
        }

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        private void Update()
        {
            // Checking occurrence of rect dimension change and invoking relevant event.
            if (rectDimensionChanged)
            {
                rectDimensionChanged = false;
                OnOrientationChange?.Invoke(Utils.GetCurrentScreenOrientation());
            }
        }

        /// <summary>
        /// Called every time MikrosUiCanvas is disabled.
        /// </summary>
        private void OnDisable()
        {
            MikrosManager.Instance.AuthenticationController.OnClickMikrosSSO -= OnClickMikrosSSO;
            openButton.onClick.RemoveAllListeners();
            closeButton.onClick.RemoveAllListeners();
        }
    }
}