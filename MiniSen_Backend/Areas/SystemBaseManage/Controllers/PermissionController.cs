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
using MiniSen_Common.Helpers.Utils;

namespace MiniSen_Backend.Areas.SystemBaseManage.Controllers
{
    [LoginCheck]
    [Area("SystemBaseManage")]
    public class PermissionController : ApiController
    {
        public IPermissionService PermissionService { get; set; }

        [HttpGet]
        public ActionResult getAccountHasApiPermissions()
        {
            var items = PermissionService.GetAccountHasApiPerms(LoginUserId).ToList();

            return JsonView(items);
        }

        [ApiPerm]
        [HttpGet]
        public ActionResult getItemsPaged(int? pageNum, int? pageSize, string apiType, string nameKey)
        {
            var items = PermissionService.SearchItemsPaged(pageNum, pageSize, apiType, nameKey).ToList();

            return JsonView(items);
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult addItem(Permission_AddEditDTO addOne)
        {
            if (!ModelState.IsValid)
            {
                throw new PushToUserException(MVCHelper.GetValidMsgStr(ModelState));
            }

            addOne.ApiUrl = Utils.UrlToHump(addOne.ApiUrl.Trim('/'));
            PermissionService.AddNewOne(addOne, LoginUserId);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult editItem(Permission_AddEditDTO editOne)
        {
            if (!ModelState.IsValid)
            {
                throw new PushToUserException(MVCHelper.GetValidMsgStr(ModelState));
            }

            editOne.ApiUrl = Utils.UrlToHump(editOne.ApiUrl.Trim('/'));
            PermissionService.EditOne(editOne, LoginUserId);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult deleteItem(string[] ids)
        {
            foreach (string deleteId in ids)
            {
                PermissionService.MarkDelete(deleteId);
            }

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }
    }
}