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
    public class DepartmentController : ApiController
    {
        public IDepartmentService DepartmentService { get; set; }

        [ApiPerm]
        [HttpGet]
        public ActionResult getAllItems()
        {
            var items = DepartmentService.GetAllItems().ToList();

            return JsonView(items);
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult addItem(Department_AddEditDTO addOne)
        {
            if (!ModelState.IsValid)
            {
                throw new PushToUserException(MVCHelper.GetValidMsgStr(ModelState));
            }

            DepartmentService.AddNewOne(addOne, LoginUserId);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult editItem(Department_AddEditDTO editOne)
        {
            if (!ModelState.IsValid)
            {
                throw new PushToUserException(MVCHelper.GetValidMsgStr(ModelState));
            }

            DepartmentService.EditOne(editOne, LoginUserId);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult deleteItem(string id)
        {
            DepartmentService.MarkDelete(id);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }
    }
}