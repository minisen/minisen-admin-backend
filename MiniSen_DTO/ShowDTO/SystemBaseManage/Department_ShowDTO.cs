using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_DTO.ShowDTO.SystemBaseManage
{
    public class Department_ShowDTO : BaseDTO
    {
        public string Name { get; set; }
        public string ParentId { get; set; }
        public int Grade { get; set; }
        public int IsLeaf { get; set; }
        public int SortNumber { get; set; }

    }
}
