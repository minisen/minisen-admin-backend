using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiniSen_Backend.MVCFilter.FilterAttribute;
using MiniSen_Common.Exceptions;
using MiniSen_DTO.AddEditDTO.MiniSenBlog;
using MiniSen_MVC_Common.ApiResultExtensions;
using MiniSen_MVC_Common.ControllerExtensions;
using MiniSen_MVC_Common.Helper;
using MiniSen_Service.Service_Interface.MiniSenBlog;

namespace MiniSen_Backend.Areas.Blog.Controllers
{
    [LoginCheck]
    [Area("Blog")]
    public class ArticleController : ApiController
    {
        public IArticleService ArticleService { get; set; }

        [ApiPerm]
        [HttpGet]
        public ActionResult getItemsPaged(int? pageNum, int? pageSize, int? status, string category)
        {
            var items = ArticleService.SearchItemsPaged(pageNum, pageSize, status, category).ToList();

            var sendDataObj = items.Select(item => new {

                id = item.Id,
                title = item.Title,
                category = item.Category,
                fullTitle = item.FullTitle,
                subHead = item.SubHead,
                intro = item.Intro,
                content = item.Content,
                author = item.Author,
                copyFrom = item.CopyFrom,
                keyword = item.Keyword,
                isOnTop = item.IsOnTop,
                isOnTopText = item.IsOnTopText,
                status = item.Status,
                statusText = item.StatusText,
                sort = item.SortNumber,
                remarks = item.RemarkInfo
            }).ToArray();

            return Json(new AjaxResult { Status = "success", SendData = new { ItemsPaged = sendDataObj } });
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult addItem(Article_AddEditDTO addOne)
        {
            if (!ModelState.IsValid)
            {
                throw new PushToUserException(MVCHelper.GetValidMsgStr(ModelState));
            }

            ArticleService.AddNewOne(addOne, LoginUserId);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult editItem(Article_AddEditDTO editOne)
        {
            if (!ModelState.IsValid)
            {
                throw new PushToUserException(MVCHelper.GetValidMsgStr(ModelState));
            }

            ArticleService.EditOne(editOne, LoginUserId);

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }

        [ApiPerm]
        [HttpPost]
        public ActionResult deleteItem(string[] ids)
        {
            foreach (string deleteId in ids)
            {
                ArticleService.MarkDelete(deleteId);
            }

            return Json(new AjaxResult { Status = "success", SendData = new { Result = "success" } });
        }
    }
}