using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_DTO.ShowDTO.SystemBaseManage
{
    public class Role_ShowDTO : BaseDTO
    {
        public string Name { get; set; }
        public int SortNumber { get; set; }
        public string[] HasApiPermissionIds { get; set; }
        public string[] HasMenuIds { get; set; }

    }
}
