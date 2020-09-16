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
    public class DictionaryController : ApiController
    {
        public IDictionaryService DictionaryService { get; set; }

        [ApiPerm]
        [HttpGet]
        public ActionResult getItemsPaged(int? pageNum, int? pageSize, string type)
        {
            var items = DictionaryService.SearchItemsPaged(pageNum, pageSize, type).ToList();

            return JsonView(items);
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult addItem(Dictionary_AddEditDTO addOne)
        {
            if (!ModelState.IsValid)
            {
                throw new PushToUserException(MVCHelper.GetValidMsgStr(ModelState));
            }

            DictionaryService.AddNewOne(addOne, LoginUserId);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult editItem(Dictionary_AddEditDTO editOne)
        {
            if (!ModelState.IsValid)
            {
                throw new PushToUserException(MVCHelper.GetValidMsgStr(ModelState));
            }

            DictionaryService.EditOne(editOne, LoginUserId);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult deleteItem(string[] ids)
        {
            foreach (string deleteId in ids)
            {
                DictionaryService.MarkDelete(deleteId);
            }

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

    }
}