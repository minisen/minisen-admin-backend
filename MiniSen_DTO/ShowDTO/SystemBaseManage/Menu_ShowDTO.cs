using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_DTO.ShowDTO.SystemBaseManage
{
    public class Menu_ShowDTO : BaseDTO
    {
        public string Name { get; set; }
        public string PagePath { get; set; }
        public string ParentId { get; set; }
        public int Type { get; set; }
        public int SortNumber { get; set; }
        public string Icon { get; set; }
        public bool IsMenuItem { get; set; }

    }
}
