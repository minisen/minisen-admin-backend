using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Common.Helpers.Utils
{
    public static class Utils
    {
        /// <summary>
        /// 获取GUID字符串
        /// </summary>
        /// <returns></returns>
        public static string GetGuidStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// 将url的格式转换为驼峰的格式
        /// 例：sys/user   =>   Sys/User
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlToHump(string url)
        {
            string humpUrl = string.Empty;

            string[] urlSegments = url.Split('/');
            foreach (string urlSegment in urlSegments)
            {
                humpUrl += urlSegment.Substring(0, 1).ToUpper() + urlSegment.Substring(1) + "/";
            }

            humpUrl = humpUrl.Substring(0, humpUrl.Length - 1);

            return humpUrl;
        }


        /// <summary>
        /// 从一个对象信息生成Json串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="datetimeFormat"></param>
        /// <returns></returns>
        public static string ObjectToJson(object obj, string datetimeFormat = null)
        {
            if (string.IsNullOrEmpty(datetimeFormat))
                datetimeFormat = "yyyy-MM-dd HH:mm:ss";

            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsSettings.Converters.Add(new IsoDateTimeConverter
            {
                DateTimeFormat = datetimeFormat
            });
            return JsonConvert.SerializeObject(obj, jsSettings);
        }

        /// <summary>
        /// 从一个Json串生成对象信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(string jsonString) where T : class
        {
            return JsonConvert.DeserializeObject(jsonString, typeof(T)) as T;
        }
    }
}
