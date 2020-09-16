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
    public class RoleController : ApiController
    {
        public IRoleService RoleService { get; set; }

        [ApiPerm]
        [HttpGet]
        public ActionResult getItemsPaged(int? pageNum, int? pageSize, string name)
        {
            var items = RoleService.SearchItemsPaged(pageNum, pageSize, name).ToList();

            return JsonView(items);
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult addItem(Role_AddEditDTO addOne)
        {
            if (!ModelState.IsValid)
            {
                throw new PushToUserException(MVCHelper.GetValidMsgStr(ModelState));
            }

            RoleService.AddNewOne(addOne, LoginUserId);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult editItem(Role_AddEditDTO editOne)
        {
            if (!ModelState.IsValid)
            {
                throw new PushToUserException(MVCHelper.GetValidMsgStr(ModelState));
            }

            RoleService.EditOne(editOne, LoginUserId);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult deleteItem(string[] ids)
        {
            foreach (string deleteId in ids)
            {
                RoleService.MarkDelete(deleteId);
            }

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult setApiPermissions(string roleId, string[] apiPermissionIds)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                throw new PushToUserException("当前编辑的角色为空");
            }

            RoleService.EditApiPermForRole(roleId, apiPermissionIds, LoginUserId);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult setMenuPermissions(string roleId, string[] menuIds)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                throw new PushToUserException("当前编辑的角色为空");
            }

            RoleService.EditMenuForRole(roleId, menuIds, LoginUserId);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

    }
}