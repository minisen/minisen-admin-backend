using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_DTO.ShowDTO.SystemBaseManage
{
    public class Dictionary_ShowDTO : BaseDTO
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public int SortNumber { get; set; }
    }
}
