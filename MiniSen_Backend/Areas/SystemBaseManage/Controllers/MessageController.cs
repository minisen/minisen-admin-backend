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
using MiniSen_DTO.ShowDTO.SystemBaseManage;
using MiniSen_Common.Exceptions;

namespace MiniSen_Backend.Areas.SystemBaseManage.Controllers
{
    [LoginCheck]
    [Area("SystemBaseManage")]
    public class MessageController : ApiController
    {
        public IMessageService MessageService { get; set; }

        [ApiPerm]
        [HttpGet]
        public ActionResult getAccountMessages()
        {
            string loginAccount = HttpContext.GetSessionStr("LoginUserAccount");

            //未發送消息 => 改變為未閱讀狀態
            var noSendMessages = MessageService.SearchItemsPaged(null, null, null, 0, loginAccount, null);
            MessageService.ChangeMessageStatus(2, noSendMessages.Select(m => m.Id).ToArray());

            //未閱讀消息
            var nonReadMessages = MessageService.SearchItemsPaged(null, null, null, 2, loginAccount, null);

            //已閱讀（一天之內）
            var haveReadMessages = MessageService.SearchItemsPaged(null, null, null, 3, loginAccount, 1);

            List<Message_ShowDTO> allMessages = nonReadMessages.ToList();
            allMessages.AddRange(haveReadMessages);

            return JsonView(allMessages);
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult markReadMessage(string[] ids)
        {
            MessageService.ChangeMessageStatus(3, ids);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

    }
}