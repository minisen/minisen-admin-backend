using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using MiniSen_MVC_Common.Helper;

namespace MiniSen_MVC_Common.ControllerExtensions
{
    public class ApiController : Controller
    {
        #region User Login Info
        public bool LoginState
        {
            get
            {
                return (bool)HttpContext.GetSessionBool("LoginState");
            }
        }
        public string LoginUserId
        {
            get
            {
                return HttpContext.GetSessionStr("LoginUserId");
            }
        }
        public string LoginUserName
        {
            get
            {
                return HttpContext.GetSessionStr("LoginUserName");
            }
        }
        public string LoginUserAccount
        {
            get
            {
                return HttpContext.GetSessionStr("LoginUserAccount");
            }
        }
        #endregion

        private void ConvertResponseTypeToJson()
        {
            base.HttpContext.Response.ContentType = "application/json; charset=utf-8";
        }

        protected internal ViewResult JsonView(string viewName, object model)
        {
            ConvertResponseTypeToJson();
            return base.View(viewName, model);
        }

        protected internal ViewResult JsonView(string viewName, string masterName)
        {
            ConvertResponseTypeToJson();
            return base.View(viewName, masterName);
        }

        protected internal ViewResult JsonView(object model)
        {
            ConvertResponseTypeToJson();
            return base.View(model);
        }

        protected internal ViewResult JsonView()
        {
            ConvertResponseTypeToJson();
            return base.View();
        }

        protected internal ViewResult JsonView(string viewName)
        {
            ConvertResponseTypeToJson();
            return base.View(viewName);
        }
    }
}
