using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_MVC_Common.Helper
{
    public static class MVCHelper
    {
        #region session相關操作封裝
        /// <summary>
        /// 獲取session值（session內容為string）
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="sessionKey">session項的鍵</param>
        /// <returns></returns>
        public static string GetSessionStr(this HttpContext httpContext, string sessionKey)
        {
            string result = string.Empty;
            result = httpContext.Session.GetString(sessionKey);

            return result;
        }

        /// <summary>
        /// 設置session的值（session內容為string）
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="sessionKey">session項的鍵</param>
        /// <param name="sessionValue">session項的值</param>
        public static void SetSessionStr(this HttpContext httpContext, string sessionKey, string sessionValue)
        {
            if (String.IsNullOrWhiteSpace(httpContext.Session.GetString(sessionKey)))
            {
                httpContext.Session.Remove(sessionKey);
            }
            httpContext.Session.SetString(sessionKey, sessionValue);
        }

        public static bool? GetSessionBool(this HttpContext httpContext, string sessionKey)
        {
            bool? boolValue;

            byte[] sessionValue = httpContext.Session.Get(sessionKey);

            if (sessionValue != null)
            {
                boolValue = Convert.ToBoolean(sessionValue[0]);
            }
            else
            {
                boolValue = null;
            }

            return boolValue;
        }

        public static void SetSessionBool(this HttpContext httpContext, string sessionKey, bool sessionValue)
        {
            byte boolVal = Convert.ToByte(sessionValue);

            httpContext.Session.Set(sessionKey, new Byte[] { boolVal });
        }
        #endregion

        #region 數據驗證封裝
        /// <summary>
        /// 将数据验证的结果包装成字典传出
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetValidMsgDic(ModelStateDictionary modelState)
        {
            Dictionary<string, string> errorMsg = new Dictionary<string, string>();
            foreach (var key in modelState.Keys)
            {
                StringBuilder errorSb = new StringBuilder();

                if (modelState[key].Errors.Count <= 0)
                {
                    continue;
                }

                foreach (var modelError in modelState[key].Errors)
                {
                    errorSb.AppendLine(modelError.ErrorMessage);
                }

                errorMsg[key] = errorSb.ToString();
            }

            return errorMsg;
        }

        /// <summary>
        /// 将数据验证的结果组合成字符串传出
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static string GetValidMsgStr(ModelStateDictionary modelState)
        {
            StringBuilder errorMsgSB = new StringBuilder();

            foreach (var key in modelState.Keys)
            {
                StringBuilder errorSb = new StringBuilder();

                if (modelState[key].Errors.Count <= 0)
                {
                    continue;
                }

                foreach (var modelError in modelState[key].Errors)
                {
                    errorSb.AppendLine(modelError.ErrorMessage);
                }

                errorMsgSB.AppendLine(errorSb.ToString());
            }

            return errorMsgSB.ToString();
        }
        #endregion
    }
}
