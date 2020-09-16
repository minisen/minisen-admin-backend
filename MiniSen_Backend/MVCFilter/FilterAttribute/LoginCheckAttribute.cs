using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSen_Backend.MVCFilter.FilterAttribute
{
    /// <summary>
    /// 给Controller | Action标注上，用作检查登陆的标志
    /// </summary>
    public class LoginCheckAttribute : Attribute { }
}
