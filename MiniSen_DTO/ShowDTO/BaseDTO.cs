using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_DTO.ShowDTO
{
    public abstract class BaseDTO
    {
        public string Id { get; set; }
        public string RemarkInfo { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
