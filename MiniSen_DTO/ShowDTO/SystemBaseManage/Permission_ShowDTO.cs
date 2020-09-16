using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_DTO.ShowDTO.SystemBaseManage
{
    public class Permission_ShowDTO : BaseDTO
    {
        public string Name { get; set; }
        public int SortNumber { get; set; }
        public string ApiUrl { get; set; }
        public string ApiType { get; set; }
        public string ApiMethod { get; set; }
    }
}
