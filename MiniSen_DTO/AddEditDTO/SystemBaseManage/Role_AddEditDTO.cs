using CoffeeOOM.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiniSen_DTO.AddEditDTO.SystemBaseManage
{
    [Mapper]
    public class Role_AddEditDTO
    {
        public string Id { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "名称的字串长度范围为1-50")]
        [Required(ErrorMessage = "名称为必须项")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "备注项字串最长为100")]
        [Mapper("RemarkInfo")]
        public string Remarks { get; set; }

        [Mapper("SortNumber")]
        public int Sort { get; set; }
    }
}
