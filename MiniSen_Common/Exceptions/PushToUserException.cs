using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Common.Exceptions
{
    /// <summary>
    /// 可推送到用户查看的异常信息
    /// </summary>
    public class PushToUserException : Exception
    {
        public PushToUserException(string errorMessage) : base(errorMessage) { }
    }
}
