using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_MVC_Common.ApiResultExtensions
{
    /// <summary>
    /// 统一的ajax请求返回数据格式
    /// </summary>
    public class AjaxResult
    {
        public string Status { get; set; }
        public string Timestamp { get; set; } = DateTime.Now.ToString();
        public string ErrorMsg { get; set; }
        public object SendData { get; set; }
    }
}
