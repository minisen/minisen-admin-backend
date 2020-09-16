using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using MiniSen_Backend.MVCFilter.FilterAttribute;
using MiniSen_Common.Exceptions;
using MiniSen_MVC_Common.ApiResultExtensions;
using MiniSen_MVC_Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSen_Backend.MVCFilter.Filter
{
    /// <summary>
    /// 登录检查
    /// </summary>
    public class CheckLoginFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            bool needLoginCheck = false;

            if (filterContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                //controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(LoginCheckAttribute), false);
                var controllerLoginAttr = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(LoginCheckAttribute), false);

                if (controllerLoginAttr.Length <= 0)
                {
                    var actionLoginAttr = controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(LoginCheckAttribute), false);
                    if (actionLoginAttr.Length <= 0)
                        needLoginCheck = false;
                    else
                        needLoginCheck = true;
                }
                else
                    needLoginCheck = true;

                if (needLoginCheck)
                {
                    //check login state
                    var loginState = filterContext.HttpContext.GetSessionBool("LoginState");

                    if (!loginState.HasValue || !loginState.Value)
                    {
                        filterContext.Result = new JsonResult(new AjaxResult { Status = "error", ErrorMsg = "redirect to login" });
                        return;
                    }
                }
            }
            else
            {
                throw new InvalidCastException("[filterContext.ActionDescriptor] is not ControllerActionDescriptor");
            }

        }
    }
}
