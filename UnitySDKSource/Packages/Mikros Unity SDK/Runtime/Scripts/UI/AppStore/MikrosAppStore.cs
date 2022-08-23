using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace MikrosClient.UI
{
    /// <summary>
    /// Class to represent the Mikros UI for mobile users.
    /// </summary>
    internal sealed class MikrosAppStore : MonoBehaviour
    {
        [SerializeField] private Button storeCloseButton;
        [SerializeField] private GameObject staticBackgroundSetup;
        [SerializeField] private GameObject appsViewSetup;
        [SerializeField] private Transform gameCategoriesContainer;
        [SerializeField] private Transform featuredAppsContainer;
        [SerializeField] private GameCategory gameCategoryPrefab;
        [SerializeField] private FeaturedApp featuredAppPrefab;
        [SerializeField] private NormalApp normalAppPrefab;
        [SerializeField] private GameCategoryFullViewPanel gameCategoryFullViewPanel;
        [SerializeField] private UserProfilePanel userProfilePanel;

        private static MikrosAppStore instance;

        /// <summary>
        /// MikrosAppStore private constructor to restrict object creation of the class.
        /// </summary>
        private MikrosAppStore()
        {
        }

        /// <summary>
        /// Builder function is used for creating an object of MikrosAppStore class.
        /// </summary>
        /// <returns>MikrosAppStore object.</returns>
        internal static MikrosAppStore Builder()
        {
            // if object not created then create the object first
            if (instance == null)
            {
                instance = Instantiate(Resources.Load<MikrosAppStore>(Constants.MikrosAppStorePrefabName), MikrosUiCanvas.Instance.transform, false);
                instance.gameObject.SetActive(false); // Disable MikrosAppStore to enable form outside.
            }
            return instance;
        }

        /// <summary>
        /// Called everytime attached gameobject is enabled.
        /// </summary>
		private void OnEnable()
        {
            storeCloseButton.onClick.AddListener(Close);
        }

        /// <summary>
        /// Show function is used for showing the MikrosAppStore.
        /// </summary>
        internal void Show()
        {
            gameObject.SetActive(true);

            // TODO : Uncomment the following line for next phase of Mikros release.
            //GetAllApps();

            userProfilePanel.Initialize();
        }

        /// <summary>
        /// Close the MikrosAppStore.
        /// </summary>
        private void Close()
        {
            MikrosManager.Instance.AdController.StoreListener.InvokeStoreClosed();
            Destroy(gameObject);
        }

        /// <summary>
        /// Handle any error in custom UI.
        /// </summary>
        /// <param name="popupTitle">Title of the popup.</param>
        /// <param name="popupMessage">Message to show in the popup.</param>
        private void HandleError(string popupTitle, string popupMessage)
        {
            PopupPanel.Builder().ShowPopup(popupTitle, popupMessage);
            LoadingPanel.Builder().HideLoading();
            Close(); // Closing the panel
        }

        /// <summary>
        /// GetAllApps function is used for fetching all apps.
        /// </summary>
        internal void GetAllApps()
        {
            Handheld.StartActivityIndicator();
            LoadingPanel.Builder().ShowLoading();

            /**
                * Creating the GetAllAppsRequest class object for request parameter of  MikrosManager.Instance.GetAllApps function.
                * .AccessToken is for initializing accessToken parameter.
                * .Create is for creating the GetAllAppsRequest class oject.
            */
            GetAllAppsRequest getAllAppsRequest = GetAllAppsRequest.Builder()
                .AccessToken(MikrosManager.Instance.AuthenticationController.Session.AccessToken)
                .Create();

            /**
                * Calling the MikrosManager.Instance.GetAllApps function.
                * getAllAppsRequest is used as DTO parameter.
                * allAppsResponse is an object that contains callback response.
                *
            */
            GetAllAppsPostRequest(getAllAppsRequest, delegate (AllAppsResponse allAppsResponse)
            {
                if (allAppsResponse == null) // If allAppsResponse object is null then throw an error popup.
                {
                    HandleError("Error", Constants.ErrorMessageGeneric);
                }
                if (Utils.DetectStatusType(allAppsResponse.Status.StatusCode) == STATUS_TYPE.SUCCESS) // status_code 200 means api called successfully
                {
                    StartCoroutine(GettingAppsCoroutine(allAppsResponse.Data));
                }
                else if (Utils.DetectStatusType(allAppsResponse.Status.StatusCode) == STATUS_TYPE.ERROR) // if status_code is 401 then either session time is over or user not logged in or access token is invalid
                {
                    MikrosManager.Instance.AuthenticationController.ResetAuthentication(); // Resetting the auth
                    HandleError("Error", Constants.MikrosAppStoreError);
                }
                else // other status_code that realated to other types of errors.
                {
                    HandleError("Error", allAppsResponse.Status.StatusMessage);
                }
            });
        }

        /// <summary>
        /// For getAllApps api calling function.
        /// </summary>
        /// <param name="getAllAppsRequestDTO">GetAllAppsRequest object parameter for request parameter of the getAllApps api.</param>
        /// <param name="callback">Api Response call back data with AllAppsResponse type object.</param>
        private void GetAllAppsPostRequest(GetAllAppsRequest getAllAppsRequestDTO, Action<AllAppsResponse> callback = null)
        {
            if (!MikrosManager.Instance.IsInitialized)
            {
                callback?.Invoke(null);
                throw new MikrosException(ExceptionType.INITIALIZE_MIKROS_SDK);
            }
            // if request parameter object is null then send a null object to it's callback method and break the current code calling.
            if (getAllAppsRequestDTO == null)
            {
                if (callback != null)
                    callback(null);
                return;
            }
            // If request parameter is not created then throw an exception.
            if (!getAllAppsRequestDTO.isCreated)
            {
                callback?.Invoke(null);
                throw new MikrosException(ExceptionType.OBJECT_NOT_CREATED);
            }

            /**
              * Get the Get All Apps api url from serverdata class.
              * convert the getAllAppsRequestDTO object to json format for requesting parameter.
              * Calling the generalized post function with required parameters.
          */
            WebRequest<AllAppsResponse>.Builder()
                .Url(ServerData.GetUrl(UrlType.GetAllApps))
                .RawJsonData(DataConverter.SerializeObject(getAllAppsRequestDTO))
                .CreatePostRequest(callback);
        }

        /// <summary>
        /// Getting all app category data.
        /// </summary>
        /// <param name="allAppCategories">Data of all app categories.</param>
        /// <returns>Coroutine</returns>
        private IEnumerator GettingAppsCoroutine(AllAppsResponseData allAppCategories)
        {
            SetupCategory(allAppCategories.Featured, featuredAppPrefab, featuredAppsContainer);

            Transform normalAppsContainer = null;

            Instantiate(gameCategoryPrefab, gameCategoriesContainer, false).Initialize(allAppCategories.Games.JustToFun, Constants.JustForFunCategoryDescription, gameCategoryFullViewPanel, out normalAppsContainer);
            SetupCategory(allAppCategories.Games.JustToFun, normalAppPrefab, normalAppsContainer);
            yield return null;
            Instantiate(gameCategoryPrefab, gameCategoriesContainer, false).Initialize(allAppCategories.Games.Favorites, Constants.FavoritesCategoryDescription, gameCategoryFullViewPanel, out normalAppsContainer);
            SetupCategory(allAppCategories.Games.Favorites, normalAppPrefab, normalAppsContainer);
            yield return null;
            Instantiate(gameCategoryPrefab, gameCategoriesContainer, false).Initialize(allAppCategories.Games.Casual, Constants.CasualCategoryDescription, gameCategoryFullViewPanel, out normalAppsContainer);
            SetupCategory(allAppCategories.Games.Casual, normalAppPrefab, normalAppsContainer);
            yield return null;
            Instantiate(gameCategoryPrefab, gameCategoriesContainer, false).Initialize(allAppCategories.Games.Competitive, Constants.CompetitiveCategoryDescription, gameCategoryFullViewPanel, out normalAppsContainer);
            SetupCategory(allAppCategories.Games.Competitive, normalAppPrefab, normalAppsContainer);
            yield return null;
            Instantiate(gameCategoryPrefab, gameCategoriesContainer, false).Initialize(allAppCategories.Games.InDevelopment, Constants.InDevelopmentCategoryDescription, gameCategoryFullViewPanel, out normalAppsContainer);
            SetupCategory(allAppCategories.Games.InDevelopment, normalAppPrefab, normalAppsContainer);

            Handheld.StopActivityIndicator();
            LoadingPanel.Builder().HideLoading();
            yield break;
        }

        /// <summary>
        /// Fetch apps of category.
        /// </summary>
        /// <param name="categoryAppsMicroData">Data of single app category.</param>
        /// <param name="appPrefab">Prefab of app to be instantiated.</param>
        /// <param name="container">Parent transform of the instantiated app.</param>
        private void SetupCategory(AppsMicroData categoryAppsMicroData, AppBase appPrefab, Transform container)
        {
            if (categoryAppsMicroData == null || string.IsNullOrEmpty(categoryAppsMicroData.Url))
            {
                return;
            }

            /**
                * Creating the GetAllAppsRequest class object for request parameter of  MikrosManager.Instance.GetAllApps function.
                * .AccessToken is for initializing accessToken parameter.
                * .Create is for creating the GetAllAppsRequest class oject.
            */
            GetAllAppsRequest getAllAppsRequest = GetAllAppsRequest.Builder()
                .AccessToken(MikrosManager.Instance.AuthenticationController.Session.AccessToken)
                .Create();

            GetAppsInCategoryPostRequest(categoryAppsMicroData.Url, getAllAppsRequest, delegate (AppCategoryData appCategoryData)
            {
                if (appCategoryData == null)
                {
                    return;
                }

                if (Utils.DetectStatusType(appCategoryData.Status.StatusCode) == STATUS_TYPE.SUCCESS) // status_code 200 means api called successfully
                {
                    if (appCategoryData.Data.Apps.Length > 0)
                        StartCoroutine(GettingAppsInCategory(appCategoryData.Data.Apps, appPrefab, container));
                }
                else if (Utils.DetectStatusType(appCategoryData.Status.StatusCode) == STATUS_TYPE.ERROR) // if status_code is 401 then either session time is over or user not logged in or access token is invalid
                {
                    MikrosManager.Instance.AuthenticationController.ResetAuthentication(); // Resetting the auth
                    HandleError("Error", Constants.MikrosAppStoreError);
                }
                else // other status_code that realated to other types of errors.
                {
                    HandleError("Error", appCategoryData.Status.StatusMessage);
                }
            });
        }

        /// <summary>
        /// Get apps in a specific category.
        /// </summary>
        /// <param name="url">URL to send web request.</param>
        /// <param name="getAppsInCategoryRequestDTO">Category specific app request object.</param>
        /// <param name="callback">Callback for category specific app request.</param>
        private void GetAppsInCategoryPostRequest(string url, GetAllAppsRequest getAppsInCategoryRequestDTO, Action<AppCategoryData> callback = null)
        {
            if (!MikrosManager.Instance.IsInitialized)
            {
                callback?.Invoke(null);
                throw new MikrosException(ExceptionType.INITIALIZE_MIKROS_SDK);
            }
            // if request parameter object is null then send a null object to it's callback method and break the current code calling.
            if (string.IsNullOrEmpty(url) || getAppsInCategoryRequestDTO == null)
            {
                callback?.Invoke(null);
                return;
            }
            // If request parameter is not created then throw an exception.
            if (!getAppsInCategoryRequestDTO.isCreated)
            {
                callback?.Invoke(null);
                throw new MikrosException(ExceptionType.OBJECT_NOT_CREATED);
            }
            WebRequest<AppCategoryData>.Builder()
                .Url(url)
                .RawJsonData(DataConverter.SerializeObject(getAppsInCategoryRequestDTO))
                .CreatePostRequest(callback);
        }

        /// <summary>
        /// Instantiating all apps in a certain category.
        /// </summary>
        /// <param name="apps">Array of all apps in a specific category.</param>
        /// <param name="appPrefab">Prefab of app to be instantiated.</param>
        /// <param name="container">Parent transform of the instantiated app.</param>
        /// <returns></returns>
        private IEnumerator GettingAppsInCategory(AppDetailsData[] apps, AppBase appPrefab, Transform container)
        {
            if (apps.Length <= 0)
            {
                yield break;
            }

            for (int i = 0; i < apps.Length; i++)
            {
                Instantiate(appPrefab, container, false).Initialize(apps[i].AppDetails);
                yield return null;
            }
            yield break;
        }

        /// <summary>
        /// Called everytime attached gameobject is disabled.
        /// </summary>
		private void OnDisable()
        {
            storeCloseButton.onClick.RemoveAllListeners();
        }
    }
}
