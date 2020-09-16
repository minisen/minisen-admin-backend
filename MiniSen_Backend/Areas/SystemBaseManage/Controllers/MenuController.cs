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
    public class MenuController : ApiController
    {
        public IMenuService MenuService { get; set; }

        [HttpGet]
        public ActionResult getAccountHasNavMenus()
        {
            var items = MenuService.GetAccountHasNavMenus(LoginUserId).ToList();

            return JsonView(items);
        }

        [ApiPerm]
        [HttpGet]
        public ActionResult getAllItems()
        {
            var items = MenuService.GetAllItems().ToList();

            return JsonView(items);
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult addItem(Menu_AddEditDTO addOne)
        {
            if (!ModelState.IsValid)
            {
                throw new PushToUserException(MVCHelper.GetValidMsgStr(ModelState));
            }

            MenuService.AddNewOne(addOne, LoginUserId);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult editItem(Menu_AddEditDTO editOne)
        {
            if (!ModelState.IsValid)
            {
                throw new PushToUserException(MVCHelper.GetValidMsgStr(ModelState));
            }

            MenuService.EditOne(editOne, LoginUserId);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult deleteItem(string id)
        {
            MenuService.MarkDelete(id);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }
    }
}