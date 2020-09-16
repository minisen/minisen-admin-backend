using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using MiniSen_Common.Exceptions;
using MiniSen_Common.Helpers.Log;
using MiniSen_MVC_Common.ApiResultExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_MVC_Common.MVCFilter.Filter
{
    /// <summary>
    /// 截获处理未处理异常
    /// </summary>
    public class HandleExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;

            if (exception is PushToUserException) //需要推送到用户的异常信息
            {
                var data = new
                {
                    result = "fail",
                    failMessage = exception.Message.ToString()
                };

                filterContext.HttpContext.Response.StatusCode = 200;
                filterContext.Result = new JsonResult(new AjaxResult { Status = "success", SendData = data });

                //标记异常已经处理完毕
                filterContext.ExceptionHandled = true;
                return;
            }
            else  //处理未处理异常
            {
                LogHelper.error($"Unhandled exception:\r\n{exception.Message.ToString()}\r\n{exception.StackTrace}");

                filterContext.Result = new JsonResult(new AjaxResult { Status = "error", ErrorMsg = "system error, please contact the system manager" });

                //标记异常已经处理完毕
                filterContext.ExceptionHandled = true;
                return;
            }

        }
    }
}
