using CoffeeOOM.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiniSen_DTO.AddEditDTO.SystemBaseManage
{
    [Mapper]
    public class Dictionary_AddEditDTO
    {
        public string Id { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "字典项名称的字串长度范围为1-50")]
        [Required(ErrorMessage = "名称为必须项")]
        public string Name { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "字典项值的字串长度范围为1-100")]
        [Required(ErrorMessage = "字典项值为必须项")]
        public string Value { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "字典项类型的字串长度范围为1-50")]
        [Required(ErrorMessage = "字典项类型为必须项")]
        public string Type { get; set; }

        [Mapper("SortNumber")]
        public int Sort { get; set; }

        [StringLength(100, ErrorMessage = "备注项字串最长为100")]
        [Mapper("RemarkInfo")]
        public string Remarks { get; set; }
    }
}
