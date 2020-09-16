using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_DTO.ShowDTO.SystemBaseManage
{
    public class Account_ShowDTO : BaseDTO
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public string Roles { get; set; }
        public string[] HasRoleIds { get; set; }

    }
}
