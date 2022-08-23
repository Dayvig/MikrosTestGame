using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;

namespace MikrosClient
{
    /// <summary>
    /// Data format handling using Newtonsoft JSON.
    /// </summary>
    public static class DataConverter
    {
        private static readonly DefaultContractResolver _contractResolver = new DefaultContractResolver();

        private static readonly JsonSerializerSettings _serializerSettings =
            new JsonSerializerSettings()
            {
                ContractResolver = _contractResolver,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
            };

        /// <summary>
        /// Serialize an object of a class into JSON format.
        /// </summary>
        /// <typeparam name="T">Generic class.</typeparam>
        /// <param name="obj">Object of generic class.</param>
        /// <returns>JSON string.</returns>
        public static string SerializeObject<T>(T obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, Formatting.None, _serializerSettings);
            }
            catch (Exception exception)
            {
                new MikrosException(ExceptionType.SERIALIZATION, exception.Message);
                return "";
            }
        }

        /// <summary>
        /// Merge and serialize objects of 2 classes into JSON format.
        /// </summary>
        /// <typeparam name="T1">Generic class.</typeparam>
        /// <typeparam name="T2">Generic class.</typeparam>
        /// <param name="obj1">Object of generic class.</param>
        /// <param name="obj2">Object of generic class.</param>
        /// <returns>JSON string.</returns>
        public static string MergeSerializeObject<T1, T2>(T1 obj1, T2 obj2)
        {
            try
            {
                JObject json1 = JObject.FromObject(obj1);
                JObject json2 = JObject.FromObject(obj2);
                json1.Merge(json2, new JsonMergeSettings
                {
                    // union array values together to avoid duplicates
                    MergeArrayHandling = MergeArrayHandling.Union
                });
                return json1.ToString(Formatting.None);
            }
            catch (Exception exception)
            {
                new MikrosException(ExceptionType.SERIALIZATION, exception.Message);
                return "";
            }
        }

        /// <summary>
        /// De-serialize a JSON string to a desired generic class.
        /// </summary>
        /// <typeparam name="T">Generic class.</typeparam>
        /// <param name="json">JSON string.</param>
        /// <returns>Object of generic class.</returns>
        public static T DeserializeObject<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, _serializerSettings);
            }
            catch (Exception exception)
            {
                new MikrosException(ExceptionType.DESERIALIZATION, exception.Message);
                return default(T);
            }
        }

        /// <summary>
        /// De-serialize a JSON string to an anonymous class object.
        /// </summary>
        /// <typeparam name="T">Generic class.</typeparam>
        /// <param name="json">JSON string.</param>
        /// <param name="anonymousTypeObject">Object of the anonymous class.</param>
        /// <returns>Object of generic class.</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            try
            {
                return JsonConvert.DeserializeAnonymousType<T>(json, anonymousTypeObject, _serializerSettings);
            }
            catch (Exception exception)
            {
                new MikrosException(ExceptionType.DESERIALIZATION, exception.Message);
                return default(T);
            }
        }
    }
}