using Microsoft.AspNetCore.Mvc.Filters;
using MiniSen_Service.Service_Interface.SystemBaseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Controllers;
using MiniSen_Backend.MVCFilter.FilterAttribute;
using MiniSen_MVC_Common.Helper;
using MiniSen_MVC_Common.ApiResultExtensions;
using Microsoft.AspNetCore.Mvc;

namespace MiniSen_Backend.MVCFilter.Filter
{
    public class CheckApiPermFilter : IAuthorizationFilter
    {
        /// <summary>
        /// Api访问权限检查
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                IAccountService accountService = filterContext.HttpContext.RequestServices.GetService<IAccountService>();
                var apiPermAttrObjs = controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(ApiPermAttribute), false);
                if (null == apiPermAttrObjs || apiPermAttrObjs.Length <= 0) return;

                //check login state
                var loginState = filterContext.HttpContext.GetSessionBool("LoginState");

                if (!loginState.HasValue || !loginState.Value)
                {
                    filterContext.Result = new JsonResult(new AjaxResult { Status = "error", ErrorMsg = "redirect to login" });
                    return;
                }

                //check api permission
                var apiPermAttr = apiPermAttrObjs[0] as ApiPermAttribute;

                //auto create current api url
                if (apiPermAttr.AutoCreate)
                {
                    string areaName = controllerActionDescriptor.RouteValues["area"];
                    string controllerName = controllerActionDescriptor.RouteValues["controller"];
                    string actionName = controllerActionDescriptor.RouteValues["action"];
                    apiPermAttr.ApiUrl = $"{ApiPermAttribute.autoDefaultPrefix}{areaName}/{controllerName}/{actionName}";
                }

                string loginAccountId = filterContext.HttpContext.GetSessionStr("LoginUserId");

                if (!accountService.JudgeIfAccountHasPerms(loginAccountId, apiPermAttr.ApiUrl))
                {
                    filterContext.Result = new JsonResult(new AjaxResult { Status = "error", ErrorMsg = "you have no permission of current operation" });
                    return;
                }
            }
            else
            {
                throw new InvalidCastException("[filterContext.ActionDescriptor] is not ControllerActionDescriptor");
            }
            
        }
    }
}
