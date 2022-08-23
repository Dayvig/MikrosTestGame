using Newtonsoft.Json;
using System.Collections.Generic;

namespace MikrosClient
{
    /// <summary>
    /// General response structure used for preset, custom events.
    /// </summary>
    public sealed class GeneralResponse
    {
        [JsonProperty(PropertyName = "status")]
        public StatusResponse Status;
    }

    /// <summary>
    /// Auth Response Data Structure used for both Signin and Signout Response.
    /// </summary>
    internal sealed class AuthResponse
    {
        [JsonProperty(PropertyName = "data")]
        internal AuthResponseData Data;

        [JsonProperty(PropertyName = "status")]
        internal StatusResponse Status;
    }

    /// <summary>
    /// Auth Response Data format with user and session format.
    /// </summary>
    internal sealed class AuthResponseData
    {
        [JsonProperty(PropertyName = "user")]
        internal MikrosUserData User;

        [JsonProperty(PropertyName = "session")]
        internal SessionResponse Session;
    }

    /// <summary>
    /// Sign-in response data format with user data format.
    /// MikrosUserData contains all the original response properties, and is used to populate MikrosUser
    /// object which is an adapted class which contains a simplified version of the original response.
    /// </summary>
    public sealed class MikrosUserData
    {
        [JsonProperty(PropertyName = "id")]
        internal string Id;

        [JsonProperty(PropertyName = "deviceId")]
        internal string DeviceId;

        [JsonProperty(PropertyName = "phone")]
        internal string Phone;

        [JsonProperty(PropertyName = "email")]
        internal string Email;

        [JsonProperty(PropertyName = "userTypeId")]
        internal string UserTypeId;

        [JsonProperty(PropertyName = "username")]
        internal string UserName;

        [JsonProperty(PropertyName = "name")]
        internal string Name;

        [JsonProperty(PropertyName = "firstName")]
        internal string FirstName;

        [JsonProperty(PropertyName = "lastName")]
        internal string LastName;

        [JsonProperty(PropertyName = "dateOfBirth")]
        internal string DateOfBirth;

        [JsonProperty(PropertyName = "sex")]
        internal string Sex;

        [JsonProperty(PropertyName = "verified")]
        internal string Verified;

        [JsonProperty(PropertyName = "verifiedEmail")]
        internal string VerifiedEmail;

        [JsonProperty(PropertyName = "verifiedPhone")]
        internal string VerifiedPhone;

        [JsonProperty(PropertyName = "spendingScore")]
        internal string SpendingScore;

        [JsonProperty(PropertyName = "activityScore")]
        internal string ActivityScore;

        [JsonProperty(PropertyName = "reputationScore")]
        internal string ReputationScore;

        [JsonProperty(PropertyName = "active")]
        internal string Active;

        [JsonProperty(PropertyName = "archived")]
        internal string Archived;

        [JsonProperty(PropertyName = "created")]
        internal string Created;

        [JsonProperty(PropertyName = "updated")]
        internal string Updated;

        [JsonProperty(PropertyName = "deleted")]
        internal string Deleted;

        [JsonProperty(PropertyName = "userTypeCode")]
        internal string UserTypeCode;

        [JsonProperty(PropertyName = "userTypeName")]
        internal string UserTypeName;

        /// <summary>
        /// Populate relevant data of Mikros User.
        /// </summary>
        /// <returns>Mikros user main data.</returns>
        internal MikrosUser PopulateData()
        {
            MikrosUser userData = new MikrosUser
            {
                Id = Id,
                DeviceId = DeviceId,
                Email = Email,
                UserName = UserName,
                SpendingScore = SpendingScore,
                ActivityScore = ActivityScore,
                ReputationScore = ReputationScore,
                Created = Created,
                Updated = Updated
            };
            return userData;
        }
    }

    /// <summary>
    /// Essential MikrosUser data.
    /// </summary>
    public sealed class MikrosUser
    {
        public string Id { get; internal set; }

        public string DeviceId { get; internal set; }

        public string Email { get; internal set; }

        public string UserName { get; internal set; }

        public string SpendingScore { get; internal set; }

        public string ActivityScore { get; internal set; }

        public string ReputationScore { get; internal set; }

        public string Created { get; internal set; }

        public string Updated { get; internal set; }

        /// <summary>
        /// Internal constructor to block public creation of object.
        /// </summary>
        internal MikrosUser()
        { }
    }

    /// <summary>
    /// Sign-in Response data format for session data of api callback.
    /// </summary>
    internal sealed class SessionResponse
    {
        [JsonProperty(PropertyName = "accessToken")]
        internal string AccessToken;

        [JsonProperty(PropertyName = "expirationMinutes")]
        internal int ExpirationMinutes;

        [JsonProperty(PropertyName = "UTCExpirationTime")]
        internal string UtcExpirationTime;

        [JsonProperty(PropertyName = "PTExpirationTime")]
        internal string PtExpirationTime;
    }

    /// <summary>
    /// Status Response Data format of api callback.
    /// </summary>
    public sealed class StatusResponse
    {
        [JsonProperty(PropertyName = "status_code")]
        public int StatusCode;

        [JsonProperty(PropertyName = "status_message")]
        public string StatusMessage;
    }

    /// <summary>
    /// GetAllAppsResponse class is used to store the data of GetAllApps api callback response.
    /// </summary>
    internal sealed class AllAppsResponse
    {
        [JsonProperty(PropertyName = "data")]
        internal AllAppsResponseData Data;

        [JsonProperty(PropertyName = "status")]
        internal StatusResponse Status;
    }

    /// <summary>
    /// AllAppsResponseData class is used as an object of AllAppsResponse class.
    /// </summary>
    internal sealed class AllAppsResponseData
    {
        [JsonProperty(PropertyName = "featured")]
        internal AppsMicroData Featured;

        [JsonProperty(PropertyName = "games")]
        internal GamesSectionData Games;
    }

    /// <summary>
    /// Non-featured game section data structure.
    /// </summary>
    internal sealed class GamesSectionData
    {
        [JsonProperty(PropertyName = "justToFun")]
        internal AppsMicroData JustToFun;

        [JsonProperty(PropertyName = "favorites")]
        internal AppsMicroData Favorites;

        [JsonProperty(PropertyName = "casual")]
        internal AppsMicroData Casual;

        [JsonProperty(PropertyName = "competitive")]
        internal AppsMicroData Competitive;

        [JsonProperty(PropertyName = "inDevelopment")]
        internal AppsMicroData InDevelopment;
    }

    /// <summary>
    /// Micro data for each section.
    /// </summary>
    internal sealed class AppsMicroData
    {
        [JsonProperty(PropertyName = "title")]
        internal string Title;

        [JsonProperty(PropertyName = "url")]
        internal string Url;
    }

    /// <summary>
    /// Root class structure of GetAllApps API response.
    /// </summary>
    internal sealed class AppCategoryData
    {
        [JsonProperty(PropertyName = "data")]
        internal AllAppsInCategory Data;

        [JsonProperty(PropertyName = "status")]
        internal StatusResponse Status;
    }

    /// <summary>
    /// Data structure for array of apps in a category.
    /// </summary>
    internal sealed class AllAppsInCategory
    {
        [JsonProperty(PropertyName = "apps")]
        internal AppDetailsData[] Apps;
    }

    /// <summary>
    /// AppDetailsData class is used in AllAppsResponseData class as array of objects.
    /// </summary>
    internal sealed class AppDetailsData
    {
        [JsonProperty(PropertyName = "appDetails")]
        internal AppDetailsContent AppDetails;
    }

    /// <summary>
    /// AppDetailsContent class is used as an object in AppDetailsData class.
    /// </summary>
    internal sealed class AppDetailsContent
    {
        [JsonProperty(PropertyName = "appId")]
        internal string AppId;

        [JsonProperty(PropertyName = "appGameId")]
        internal string AppGameId;

        [JsonProperty(PropertyName = "appName")]
        internal string AppName;

        [JsonProperty(PropertyName = "companyName")]
        internal string CompanyName;

        [JsonProperty(PropertyName = "title")]
        internal string Title;

        [JsonProperty(PropertyName = "shortDescription")]
        internal string ShortDesc;

        [JsonProperty(PropertyName = "longDescription")]
        internal string LongDesc;

        [JsonProperty(PropertyName = "googlePlayStoreLink")]
        public string GooglePlayStoreLink;

        [JsonProperty(PropertyName = "appleAppStoreLink")]
        public string AppleAppStoreLink;

        [JsonProperty(PropertyName = "images")]
        internal ImagesData Images;
    }

    /// <summary>
    /// ImagesData class is used as an object of AppDetailsContent class.
    /// </summary>
    internal sealed class ImagesData
    {
        [JsonProperty(PropertyName = "featureGraphics")]
        internal List<ImagesContent> FeatureGraphics;

        [JsonProperty(PropertyName = "hiResIcons")]
        internal List<ImagesContent> HiResIcons;

        [JsonProperty(PropertyName = "promoGraphics")]
        internal List<ImagesContent> PromoGraphics;

        [JsonProperty(PropertyName = "screenshots")]
        internal List<ImagesContent> Screenshots;
    }

    /// <summary>
    /// ImagesContent class is used in ImagesData class to store different type of Image contents.
    /// </summary>
    internal sealed class ImagesContent
    {
        [JsonProperty(PropertyName = "caption")]
        internal string Caption;

        [JsonProperty(PropertyName = "imageUrl")]
        internal string ImageUrl;

        [JsonProperty(PropertyName = "imageType")]
        internal string ImageType;
    }

    /// <summary>
    /// VideosData class is used as an object of AppDetailsContent class.
    /// </summary>
    internal sealed class VideosData
    {
        [JsonProperty(PropertyName = "others")]
        internal VideosContent[] Others;

        [JsonProperty(PropertyName = "promotional")]
        internal VideosContent[] Promotional;
    }

    /// <summary>
    /// VideosContent class is used in VideosData class to store different type of video contents.
    /// </summary>
    internal sealed class VideosContent
    {
        [JsonProperty(PropertyName = "caption")]
        internal string Caption;

        [JsonProperty(PropertyName = "videoUrl")]
        internal string VideoUrl;

        [JsonProperty(PropertyName = "videoType")]
        internal string VideoType;
    }
}