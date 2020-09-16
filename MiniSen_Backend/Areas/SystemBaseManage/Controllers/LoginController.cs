using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiniSen_Backend.Areas.SystemBaseManage.Models;
using MiniSen_MVC_Common.ApiResultExtensions;
using MiniSen_MVC_Common.ControllerExtensions;
using MiniSen_Service.Service_Interface.SystemBaseManage;
using MiniSen_MVC_Common.Helper;
using MiniSen_Backend.MVCFilter.FilterAttribute;

namespace MiniSen_Backend.Areas.SystemBaseManage.Controllers
{
    [Area("SystemBaseManage")]
    public class LoginController : ApiController
    {
        public IAccountService AccountService { get; set; }

        [HttpPost]
        public ActionResult Login(LoginModel loginParam)
        {
            var loginUserInfo = AccountService.MatchLogin(loginParam.Account, loginParam.Password);

            HttpContext.SetSessionBool("LoginState", true);
            HttpContext.SetSessionStr("LoginUserId", loginUserInfo.Id);
            HttpContext.SetSessionStr("LoginUserName", loginUserInfo.Name);
            HttpContext.SetSessionStr("LoginUserAccount", loginUserInfo.Account);

            return Json(new AjaxResult
            {
                Status = "success",
                SendData = new
                {
                    loginStatus = "success",
                    userInformation = new
                    {
                        account = loginUserInfo.Account,
                        userName = loginUserInfo.Name,
                        role = loginUserInfo.Roles
                    },
                    token = HttpContext.Session.Id
                }
            });
        }

        [LoginCheck]
        [HttpPost]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

        [ApiPerm]
        [LoginCheck]
        [HttpPost]
        public ActionResult ChangePassword(string oldPwd, string newPwd, string repeatNewPwd)
        {
            AccountService.ChangePassword(oldPwd, newPwd, repeatNewPwd, LoginUserId);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }


    }
}