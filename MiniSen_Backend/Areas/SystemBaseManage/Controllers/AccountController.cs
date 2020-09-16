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
using MiniSen_DTO.AddEditDTO.SystemBaseManage;
using MiniSen_Common.Exceptions;

namespace MiniSen_Backend.Areas.SystemBaseManage.Controllers
{
    [LoginCheck]
    [Area("SystemBaseManage")]
    public class AccountController : ApiController
    {
        public IAccountService AccountService { get; set; }

        [ApiPerm]
        [HttpGet]
        public ActionResult getItemsPaged(int? pageNum, int? pageSize, string name, string account)
        {
            var items = AccountService.SearchItemsPaged(pageNum, pageSize, name, account).ToList();

            return JsonView(items);
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult addItem(Account_AddEditDTO addOne)
        {
            if (!ModelState.IsValid)
            {
                throw new PushToUserException(MVCHelper.GetValidMsgStr(ModelState));
            }

            string newAddOneId = AccountService.AddNewOne(addOne, LoginUserId);
            AccountService.UpdateRolesForAccount(newAddOneId, addOne.HasRoleIds, LoginUserId);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult editItem(Account_AddEditDTO editOne)
        {
            if (!ModelState.IsValid)
            {
                throw new PushToUserException(MVCHelper.GetValidMsgStr(ModelState));
            }

            AccountService.EditOne(editOne, LoginUserId);
            AccountService.UpdateRolesForAccount(editOne.Id, editOne.HasRoleIds, LoginUserId);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult deleteItem(string[] ids)
        {
            foreach (string deleteId in ids)
            {
                AccountService.MarkDelete(deleteId);
            }

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

    }
}